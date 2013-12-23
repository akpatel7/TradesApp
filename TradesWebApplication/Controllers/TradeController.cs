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
    public class TradeController : Controller
    {
        
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Trade/
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TradeIdSortParm = String.IsNullOrEmpty(sortOrder) ? "TradeId" : "";
            ViewBag.StatusSortParm = String.IsNullOrEmpty(sortOrder) ? "Status" : "";
            ViewBag.CreatedDateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";

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

            //var trades = from t in tradeRepository.GetTrades() 
            //             select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                int searchIndex = int.Parse(searchString);
                trades = trades.Where(s => s.trade_id == searchIndex);
                // || s.trade_label.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "TradeId":
                    trades = trades.OrderByDescending(s => s.trade_id);
                    break;
                case "Status":
                    trades = trades.OrderBy(s => s.status);
                    break;
                case "Date":
                    trades = trades.OrderBy(s => s.created_on);
                    break;
                case "Date_desc":
                    trades = trades.OrderByDescending(s => s.created_on);
                    break;
                default:
                    trades = trades.OrderBy(s => s.trade_id);
                    break;
            }

            int pageSize = 7;
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
            var viewModel = new TradesCreationViewModel();

            viewModel.Trade = unitOfWork.TradeRepository.Get(id);
            viewModel.trade_id = id;

            if (viewModel.Trade == null)
            {
                return HttpNotFound();
            }

            viewModel.currency_id = viewModel.Trade.currency_id;
            PopulateDropDownEntities(viewModel, false);
            PopulateRelatedTradeLinesAndGroups(viewModel);
            PopulateInstructions(viewModel);
            PopulateAbsoluteAndRelativePerformance(viewModel);
            PopulateRelatedTrades(viewModel);
            PopulateComment(viewModel);

            return View(viewModel);
        }

        private void PopulateAbsoluteAndRelativePerformance(TradesCreationViewModel viewModel)
        {
            //TODO check logic
            //absolute

            //related
            viewModel.MeasureTypes = unitOfWork.MeasureTypeRepository.GetAll().ToList();
            var relatedTradePerformances =
                unitOfWork.TradePerformanceRepository.GetAll()
                    .Where(t => t.trade_id == viewModel.Trade.trade_id)
                    .ToList();
            if (relatedTradePerformances.Any())
            {
                var latestPerformance = relatedTradePerformances.Last();
                viewModel.rel_measure_type_id = latestPerformance.measure_type_id;
                viewModel.rel_currency_id = latestPerformance.return_currency_id;
                viewModel.rel_return_value = latestPerformance.return_value;
                viewModel.return_benchmark_id = latestPerformance.return_benchmark_id;
                //apl
                viewModel.apl_func = latestPerformance.return_apl_function;
            }


        }

        private void PopulateComment(TradesCreationViewModel viewModel)
        {
            viewModel.Comment =
                unitOfWork.TradeCommentRepository.Get(c => c.trade_id == viewModel.Trade.trade_id).LastOrDefault();
        }

        private void PopulateRelatedTrades(TradesCreationViewModel viewModel)
        {
            viewModel.RelatedTrades = unitOfWork.TradeRepository.GetTrades().ToList();
        }

        private void PopulateInstructions(TradesCreationViewModel viewModel)
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

        private void PopulateRelatedTradeLinesAndGroups(TradesCreationViewModel viewModel)
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

                        viewModel.TradeLineGroups.Add(tradeLineGroupVM);
                    }
                }     
            }
       
        }

        // GET: /Trade/Create
        public ActionResult Create()
        {
            var viewModel = new TradesCreationViewModel();

            viewModel.Trade = new Trade();

            PopulateDropDownEntities(viewModel, false);
            PopulateRelatedTradeLinesAndGroups(viewModel);
            PopulateInstructions(viewModel);
            PopulateAbsoluteAndRelativePerformance(viewModel);
            PopulateRelatedTrades(viewModel);
            PopulateComment(viewModel);

            return View(viewModel);
     
        }

        private void PopulateDropDownEntities(TradesCreationViewModel viewModel, bool initialize)
        {

            viewModel.Services = unitOfWork.ServiceRepository.GetAll().ToList();
            viewModel.LengthTypes = unitOfWork.LengthTypeRepository.GetAll().ToList();
            viewModel.Benchmarks = unitOfWork.BenchmarkRepository.GetAll().ToList();
            viewModel.Currencies = unitOfWork.CurrencyRepository.GetAll().ToList();
            viewModel.StructureTypes = unitOfWork.StructureTypeRepository.GetAll().ToList();
            viewModel.Relativitys = unitOfWork.RelativityRepository.GetAll().ToList();
            viewModel.created_on = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


            viewModel.TradeLineGroupTypes = unitOfWork.TradeLineGroupTypeRepository.GetAll().ToList();
            viewModel.Positions = unitOfWork.PositionRepository.GetAll().ToList();
            viewModel.TradeTradableThings = unitOfWork.TradableThingRepository.GetAll().ToList();


            viewModel.InstructionTypes = unitOfWork.InstructionTypeRepository.GetAll().ToList();
            viewModel.HedgeTypes = unitOfWork.HedgeTypeRepository.GetAll().ToList();
            viewModel.MeasureTypes = unitOfWork.MeasureTypeRepository.GetAll().ToList();

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
            return View(new TradesCreationViewModel());

        }

        // GET: /Trade/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trade trade = unitOfWork.TradeRepository.Get(id);
            if (trade == null)
            {
                return HttpNotFound();
            }

            return View(trade);
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

        //TradeableThing (Financial Instrument) typeahead
        public JsonResult AutoCompleteTradableThing(string term)
        {
            var list = unitOfWork.TradableThingRepository.GetAll();
            var result = (from r in list
                          where r.tradable_thing_label.ToLower().Contains(term.ToLower())
                          select new { r.tradable_thing_label, r.tradable_thing_id }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Linked Trades typeahead
        public JsonResult AutoCompleteLinkedTrades(string term)
        {
            var list = unitOfWork.TradeRepository.GetTrades();
            var result = (from r in list
                          where r.trade_label.ToLower().Contains(term.ToLower())
                          select new { r.trade_label, r.trade_id }); //TODO: Verify will retrieve duplicate editorial labels
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
