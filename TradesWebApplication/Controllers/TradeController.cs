using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using TradesWebApplication.DAL.EFModels;
using TradesWebApplication.DAL;
using PagedList;
using TradesWebApplication.ViewModels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TradesWebApplication.Controllers
{
    [Authorize]
    public class TradeController : Controller
    {
        
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Trade/
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TradeIdSortParm = String.IsNullOrEmpty(sortOrder) ? "TradeId" : "TradeIdDesc";
            ViewBag.LastUpdatedSortParm = String.IsNullOrEmpty(sortOrder) ? "Date_Asc" :  "LastUpdatedDate";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var trades = unitOfWork.TradeRepository.GetTrades();


            if (!String.IsNullOrEmpty(searchString))
            {
                int searchIndex = int.Parse(searchString);
                trades = trades.Where(s => s.trade_id == searchIndex);
                // || s.trade_label.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "TradeId":
                    trades = trades.OrderBy(s => s.trade_id);
                    ViewBag.TradeIdSortParm = "TradeIdDesc";
                    break;
                case "TradeIdDesc":
                    trades = trades.OrderByDescending(s => s.trade_id);
                    ViewBag.TradeIdSortParm = "TradeId";
                    break;
                case "LastUpdatedDate":
                    trades = trades.OrderByDescending(s => s.last_updated).ThenByDescending(t => t.trade_id);
                    ViewBag.LastUpdatedSortParm = "Date_Asc";
                    break;
                case "Date_Asc":
                    trades = trades.OrderBy(s => s.last_updated).ThenBy( t => t.trade_id );
                    ViewBag.LastUpdatedSortParm = "LastUpdatedDate";
                    break;
                default:
                    trades = trades.OrderByDescending(s => s.last_updated).ThenByDescending(t => t.trade_id);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(trades.ToPagedList(pageNumber, pageSize));

        }

        // GET: /Trade/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var vm = new TradesViewModel();

            var trade = unitOfWork.TradeRepository.Get(id);
            vm.trade_id = trade.trade_id;
            vm.Trade = trade;
            if (trade.last_updated.HasValue)
            {
                vm.last_updated = ((DateTime)trade.last_updated).ToString("yyyy-MM-dd");
            }

            PopulateDropDownEntities(vm, false);
            PopulateRelatedTradeLinesAndGroups(vm);
            PopulateInstructions(vm);
            PopulateAbsoluteAndRelativePerformance(vm);
            PopulateRelatedTrades(vm);
            PopulateComment(vm);

            if (trade.status.HasValue)
            {
                vm.status = trade.status;
            }
            else
            {
                //HACK: need to fill statuses in db
                vm.status = 1;
            }


            return View(vm);
        }

        private void PopulateAbsoluteAndRelativePerformance(TradesViewModel viewModel)
        {
       
            //fx spot and carry
            var relatedTrackRecords = unitOfWork.TrackRecordRepository.GetAll()
                                      .Where(t => t.trade_id == viewModel.Trade.trade_id)
                                      .ToList();

            if (relatedTrackRecords.Any())
            {
                //mark to market rate
                var rec1 = relatedTrackRecords.LastOrDefault(r => r.trade_id == viewModel.Trade.trade_id && r.track_record_type_id == 1);
                if (rec1 != null)
                {
                    viewModel.mark_to_mark_rate = rec1.track_record_value.ToString();
                }

                //Interest rate differential
                var rec2 = relatedTrackRecords.LastOrDefault(r => r.trade_id == viewModel.Trade.trade_id && r.track_record_type_id == 2);
                if (rec2 != null)
                {
                    viewModel.interest_rate_diff = rec2.track_record_value.ToString();
                }

            }

            //related
            viewModel.MeasureTypes = unitOfWork.MeasureTypeRepository.GetAll().ToList();
            var relatedTradePerformances =
                unitOfWork.TradePerformanceRepository.GetAll()
                    .Where(t => t.trade_id == viewModel.Trade.trade_id)
                    .ToList();
            if (relatedTradePerformances.Any())
            {
                //absolute performance
                bool perfFound = false;
                var absolutePerformance = relatedTradePerformances.LastOrDefault( r => r.trade_id == viewModel.Trade.trade_id && r.return_benchmark_id == null );
                if (absolutePerformance != null)
                {
                    perfFound = true;
                    viewModel.abs_measure_type_id = absolutePerformance.measure_type_id;
                    viewModel.abs_currency_id = absolutePerformance.return_currency_id;
                    viewModel.abs_return_value = absolutePerformance.return_value;
                    viewModel.apl_func = absolutePerformance.return_apl_function;
                }
               

                //relative performance
                var relativePerformance = relatedTradePerformances.LastOrDefault(r => r.trade_id == viewModel.Trade.trade_id && r.return_benchmark_id != null);
                if (relativePerformance != null)
                {
                    perfFound = true;
                    viewModel.rel_measure_type_id = relativePerformance.measure_type_id;
                    viewModel.rel_currency_id = relativePerformance.return_currency_id;
                    viewModel.rel_return_value = relativePerformance.return_value;
                    viewModel.return_benchmark_id = relativePerformance.return_benchmark_id;
                }
               
                //apl
                if (!perfFound && relatedTradePerformances.Any())
                {
                    var performance = relatedTradePerformances.LastOrDefault(r => r.trade_id == viewModel.Trade.trade_id);
                    viewModel.apl_func = performance.return_apl_function;
                }
                
            }


        }

        private void PopulateComment(TradesViewModel viewModel)
        {
            viewModel.Comment =
                unitOfWork.TradeCommentRepository.Get(c => c.trade_id == viewModel.Trade.trade_id).LastOrDefault();
        }

        private void PopulateRelatedTrades(TradesViewModel viewModel)
        {
            viewModel.RelatedTrades = unitOfWork.RelatedTradeRepository.GetAll()
                                      .Where( r => r.trade_id == viewModel.Trade.trade_id).ToList();
        }

        private void PopulateInstructions(TradesViewModel viewModel)
        {
            var instructionTypes = unitOfWork.TradeInstructionRepository.GetAll().ToList();
            var instructionType = instructionTypes.FindAll(i => i.trade_id == viewModel.Trade.trade_id).FirstOrDefault();

            if (instructionType == null) return;
            viewModel.instruction_type_id = instructionType.instruction_type_id;
            viewModel.hedge_id = instructionType.hedge_id;
            if (instructionType.instruction_entry != null)
            {
                viewModel.instruction_entry = (decimal)instructionType.instruction_entry;
            }
            if (instructionType.instruction_exit != null)
            {
                viewModel.instruction_exit = (decimal)instructionType.instruction_exit;
            }
            if (instructionType.instruction_entry_date != null)
            {
                viewModel.instruction_entry_date = ((DateTime)instructionType.instruction_entry_date).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (instructionType.instruction_exit_date != null)
            {
                viewModel.instruction_exit_date =((DateTime) instructionType.instruction_exit_date).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (!String.IsNullOrEmpty(instructionType.instruction_label))
            {
                viewModel.instruction_label = instructionType.instruction_label;
            }
            
        }

        private void PopulateRelatedTradeLinesAndGroups(TradesViewModel viewModel)
        {
            viewModel.TradeLines = new List<TradeLineViewModel>();
            viewModel.TradeLineGroups = new List<TradeLineGroupViewModel>();

            var tradeLines = unitOfWork.TradeLineRepository.GetAll().Where(t => t.trade_id == viewModel.trade_id).ToList();

            if (tradeLines.Any())
            {
                foreach (var tradeline in tradeLines)
                {

                    var tradeLineVM = new TradeLineViewModel
                    {
                        TradeLine = tradeline,
                        Position = unitOfWork.PositionRepository.GetByID(tradeline.position_id),
                        TradeTradableThing = unitOfWork.TradableThingRepository.GetByID(tradeline.tradable_thing_id),
                        Positions = unitOfWork.PositionRepository.GetAll().ToList(),
                        TradeTradableThings = unitOfWork.TradableThingRepository.GetAll().ToList()
                    };

                    viewModel.TradeLines.Add(tradeLineVM); 

                    var tradeLineGroup = unitOfWork.TradeLineGroupRepository.GetByID(tradeline.trade_line_group_id);

                    if (tradeLineGroup != null)
                    {
                        var tradeLineGroupVM = new TradeLineGroupViewModel
                        {
                            TradeLineGroup = tradeLineGroup,
                            TradeLineGroupTypes = unitOfWork.TradeLineGroupTypeRepository.GetAll().ToList(),
                            TradeLineGroupType = unitOfWork.TradeLineGroupTypeRepository.GetByID(tradeLineGroup.trade_line_group_type_id),
                            EditorialLabel = tradeLineGroup.trade_line_group_editorial_label,
                            CanonicalLabel = tradeLineGroup.trade_line_group_label
                        };

                        bool groupExists = false;
                        //check to see if exists
                        if (viewModel.TradeLineGroups.Any())
                        {
                            for (int i = 0; i < viewModel.TradeLineGroups.Count; i++)
                            {
                                if (viewModel.TradeLineGroups[i].TradeLineGroup.trade_line_group_id == tradeLineGroupVM.TradeLineGroup.trade_line_group_id)
                                {
                                    groupExists = true;
                                    break;
                                }
                            }
                        }

                        if (!groupExists)
                        {
                            viewModel.TradeLineGroups.Add(tradeLineGroupVM);
                        }
                       
                    }
                }     
            }
       
        }

        private void PopulateDropDownEntities(TradesViewModel viewModel, bool initialize)
        {

            viewModel.Services = unitOfWork.ServiceRepository.GetAll().ToList();
            viewModel.LengthTypes = unitOfWork.LengthTypeRepository.GetAll().ToList();
            viewModel.Benchmarks = unitOfWork.BenchmarkRepository.GetAll().ToList();
            viewModel.Currencies = unitOfWork.CurrencyRepository.GetAll().ToList();
            viewModel.StructureTypes = unitOfWork.StructureTypeRepository.GetAll().ToList();
            viewModel.Relativitys = unitOfWork.RelativityRepository.GetAll().ToList();
            viewModel.created_on = System.DateTime.Now.ToString("yyyy-MM-dd");


            viewModel.TradeLineGroupTypes = unitOfWork.TradeLineGroupTypeRepository.GetAll().ToList();
            viewModel.Positions = unitOfWork.PositionRepository.GetAll().ToList();
            viewModel.TradeTradableThings = unitOfWork.TradableThingRepository.GetAll().ToList();


            viewModel.InstructionTypes = unitOfWork.InstructionTypeRepository.GetAll().ToList();
            viewModel.HedgeTypes = unitOfWork.HedgeTypeRepository.GetAll().ToList();
            viewModel.MeasureTypes = unitOfWork.MeasureTypeRepository.GetAll().ToList();
            viewModel.Status = unitOfWork.StatusRepository.GetAll().ToList();

                //default values for trade creation
            if (initialize)
            {
                viewModel.length_type_id = 2;
                viewModel.relativity_id = 2;
                viewModel.structure_type_id = 4;
                viewModel.hedge_id = 2;
                viewModel.abs_measure_type_id = 1;
                viewModel.rel_measure_type_id = 2;
            }

        }

        // GET: /Trade/Create
        public ActionResult Create()
        {
            var viewModel = new TradesViewModel();

            viewModel.Trade = new Trade();
            

            PopulateDropDownEntities(viewModel, false);
            PopulateRelatedTradeLinesAndGroups(viewModel);
            PopulateInstructions(viewModel);
            PopulateAbsoluteAndRelativePerformance(viewModel);
            PopulateRelatedTrades(viewModel);
            PopulateComment(viewModel);

            return View(viewModel);

        }

        // POST: /Trade/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create()
        //{
        //    var viewModel = new TradesCreationViewModel();

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            unitOfWork.TradeRepository.InsertTrade(trade);
        //            unitOfWork.TradeRepository.Save();
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (DataException /* dex */)
        //    {
        //        //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
        //        ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
        //    }

        //    return View(viewModel);
        //}

        // POST: /Trade/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(object data)
        {
            return View(new TradesViewModel());

        }

        // GET: /Trade/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var vm = new TradesViewModel();

            var trade = unitOfWork.TradeRepository.Get(id);
            vm.trade_id = trade.trade_id;
            vm.Trade = trade;
            if (trade.last_updated.HasValue)
            {
                vm.last_updated = ((DateTime)trade.last_updated).ToString("yyyy-MM-dd");
            }
            
            PopulateDropDownEntities(vm, false);
            PopulateRelatedTradeLinesAndGroups(vm);
            PopulateInstructions(vm);
            PopulateAbsoluteAndRelativePerformance(vm);
            PopulateRelatedTrades(vm);
            PopulateComment(vm);

            if (trade.status.HasValue)
            {
                vm.status = trade.status;
            }
            else 
            {   
                //HACK: need to fill statuses in db
                vm.status = 1;
            }


            return View(vm);

        }

        // POST: /Trade/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="trade_id,trade_uri,relativity_id,length_type_id,structure_type_id,service_id,currency_id,benchmark_id,trade_label,trade_editorial_label,created_on,created_by,last_updated,status")] Trade trade)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.TradeRepository.UpdateStudent(trade);
                    unitOfWork.TradeRepository.Save();
                    return RedirectToAction("Index");
                }   
            }
            catch(DataException /* dex*/ )
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            return View(trade);
        }

        // GET: /Trade/Delete/5
        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Trade trade = unitOfWork.TradeRepository.Get(id);
            return View(trade);
        }

        // POST: /Trade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Trade trade = unitOfWork.TradeRepository.Get(id);
                unitOfWork.TradeRepository.DeleteTrade(id);
                unitOfWork.TradeRepository.Save();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        //Benchmark typeahead
        public JsonResult AutoCompleteBenchmark(string term)
        {
            var list = unitOfWork.BenchmarkRepository.GetAll();
            var result = (from r in list
                          where r.benchmark_label.ToLower().Contains(term.ToLower())
                          select new { r.benchmark_label, r.benchmark_id }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Benchmark typeahead
        public JsonResult GetBenchmark(string id)
        {
            var benchmarkId = int.Parse(id);
            var list = unitOfWork.BenchmarkRepository.GetAll();
            var result = (from r in list
                          where r.benchmark_id == benchmarkId
                          select new { r.benchmark_label, r.benchmark_id }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //TradeableThing (Financial Instrument) typeahead
        public JsonResult AutoCompleteTradableThing(string term)
        {
            var list = unitOfWork.TradableThingRepository.GetAll();
            var result = (from r in list
                          where r.tradable_thing_label.ToLower().Contains(term.ToLower())
                          select new { r.tradable_thing_label, r.tradable_thing_id }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //(Position) typeahead
        public JsonResult AutoCompletePosition(string relativityId)
        {
            var relativityID = int.Parse(relativityId);
            var list = unitOfWork.PositionRepository.GetAll();
            var result = (from r in list
                          where r.position_relativity_id == relativityID
                          select new { r.position_label, r.position_id }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //TradeableThing (Financial Instrument) typeahead
        public JsonResult GetTradableThing(string id)
        {
            var trabableThingId = int.Parse(id);
            var list = unitOfWork.TradableThingRepository.Get(r => r.tradable_thing_id == trabableThingId);
            var result = (from r in list
                          select new { r.tradable_thing_label, r.tradable_thing_id }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //(Position) typeahead
        public JsonResult GetPosition(string id)
        {
            var positionId = int.Parse(id);
            var list = unitOfWork.PositionRepository.Get(r => r.position_id == positionId);
            var result = (from r in list
                          select new { r.position_label, r.position_id }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Linked Trades typeahead
        public JsonResult AutoCompleteLinkedTrades(string term)
        {
            var list = unitOfWork.TradeRepository.GetTrades();
            var result = (from r in list
                          where (!String.IsNullOrEmpty(r.trade_editorial_label)
                          &&
                          r.trade_editorial_label.ToLower().Contains(term.ToLower()))
                          select new { r.trade_editorial_label, r.trade_id }); 
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLinkedTrade(string id)
        {
            //var linkedTradeIds = StringToIntList(id);
            //linkedTradeIds.ToList();
            var tradeId = int.Parse(id);
            var tradesListIds = unitOfWork.RelatedTradeRepository.GetAll().Where(r => r.trade_id == tradeId).ToList();

            var tradesList = new List<Trade>();
            foreach (var t in tradesListIds)
            {
                tradesList.Add(unitOfWork.TradeRepository.Get(t.related_trade_id));
            }

            var result = (from r in tradesList
                          select new { r.trade_editorial_label, r.trade_id }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
        //Currency typeahead
        public JsonResult AutoCompleteCurrency(string term)
        {
            var list = unitOfWork.CurrencyRepository.GetAll();
            var result = (from r in list
                          where r.currency_label.ToLower().Contains(term.ToLower())
                          select new { r.currency_label, r.currency_id }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Currency typeahead
        public JsonResult GetCurrency(string id)
        {
            var currencyId = int.Parse(id);
            var list = unitOfWork.CurrencyRepository.GetAll();
            var result = (from r in list
                          where r.currency_id == currencyId
                          select new { r.currency_label, r.currency_id }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<int> StringToIntList(string str)
        {
            if (String.IsNullOrEmpty(str))
                yield break;

            foreach (var s in str.Split(','))
            {
                int num;
                if (int.TryParse(s, out num))
                    yield return num;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.TradeRepository.Dispose();
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
