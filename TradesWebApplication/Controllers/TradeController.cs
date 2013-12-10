using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TradesWebApplication.DAL.EFModels;
using TradesWebApplication.DAL;
using PagedList;
using TradesWebApplication.ViewModels;

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

            PopulateDropDownEntities(viewModel, false);

            return View(viewModel);
        }

        // GET: /Trade/Create
        public ActionResult Create()
        {
            var viewModel = new TradesCreationViewModel();

            viewModel.Trade = new Trade();

            PopulateDropDownEntities(viewModel, true);

            return View(viewModel);
     
        }

        private void PopulateDropDownEntities(TradesCreationViewModel viewModel, bool initialize)
        {
            viewModel.TradeLineGroups = unitOfWork.TradeLineGroupRepository.GetAll();
            viewModel.TradeLines = unitOfWork.TradeLineRepository.GetAll();

            viewModel.Services = unitOfWork.ServiceRepository.GetAll().ToList();
            viewModel.Length_Types = unitOfWork.LengthTypeRepository.GetAll().ToList();
            viewModel.Benchmarks = unitOfWork.BenchmarkRepository.GetAll().ToList();
            viewModel.Currencies = unitOfWork.CurrencyRepository.GetAll().ToList();
            viewModel.Structure_Types = unitOfWork.StructureTypeRepository.GetAll().ToList();
            viewModel.Relativitys = unitOfWork.RelativityRepository.GetAll().ToList();
            viewModel.created_on = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //viewModel = db.Positions.ToList();
            //viewModel.TradableThings = db.Tradable_Thing.ToList();
            //viewModel.Releated_Trades = db.RelatedTrades.ToList();

            viewModel.Instruction_Types = unitOfWork.InstructionTypeRepository.GetAll().ToList();
            viewModel.Hedge_Types = unitOfWork.HedgeTypeRepository.GetAll().ToList();
            viewModel.Measure_Types = unitOfWork.MeasureTypeRepository.GetAll().ToList();

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="trade_id,trade_uri,relativity_id,length_type_id,structure_type_id,service_id,currency_id,benchmark_id,trade_label,trade_editorial_label,created_on,created_by,last_updated,status")] Trade trade)
        {
            var viewModel = new TradesCreationViewModel();

            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.TradeRepository.InsertTrade(trade);
                    unitOfWork.TradeRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            return View(viewModel);
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
