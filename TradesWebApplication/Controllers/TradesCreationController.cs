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
using TradesWebApplication.ViewModels;

namespace TradesWebApplication.Controllers
{
    public class TradesCreationController : Controller
    {
        private TradesContext db = new TradesContext();

        // GET: /TradesCreation/
        public ActionResult Index(int? tradeid)
        {
            var viewModel = new TradesCreationViewModel();
            viewModel.Trades = db.Trades.Include(t => t.Benchmark).Include(t => t.Currency).Include(t => t.Length_Type).Include(t => t.Relativity).Include(t => t.Service).Include(t => t.Status1).Include(t => t.Structure_Type);

            //if (tradeid != null)
            //{
            //    ViewBag.TradeID = tradeid.Value;
            //    //Trade lines
            //    viewModel.TradeLines = viewModel.TradeLines.Where(
            //        i => i.trade_id == tradeid.Value).ToList();
            //    //TradeLine groups
            //    if (viewModel.TradeLines.Any())
            //    {
            //        foreach (var tradeLine in viewModel.TradeLines)
            //            viewModel.TradeLineGroups.ToList().Add(db.TradeGroups.Single(i => i.trade_line_group_id == tradeLine.trade_line_group_id)); 
            //    }
            //    //Instructions
            //    //viewModel.TradeInstruction = viewModel.TradeInstruction.Where(
            //    //    i => i.trade_id == tradeid.Value).ToList();
            //    //if (viewModel.TradeInstruction.Any())
            //    //{
            //    //    var tradeInstruction = viewModel.TradeInstruction.Single();
            //    //    viewModel.HedgeInstruction = viewModel.HedgeInstruction.Where(
            //    //    i => i.hedge_id == tradeInstruction.hedge_id).ToList();
            //    //}

            //    //Supplementary Information

            //    //FX Spot and Carry

            //    //Absolute Performance

            //    //Relative Performance

            //    //Trade Comments

            //}

            return View(viewModel);
        }

        // GET: /TradesCreation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            return View(trade);
        }

        // GET: /TradesCreation/Create
        public ActionResult Create()
        {
            var viewModel = new TradesCreationViewModel();

            viewModel.Trades = db.Trades.ToList();
            viewModel.TradeLineGroups = db.TradeLineGroups.ToList();
            viewModel.TradeLines = db.TradeLines.ToList();

            viewModel.Services = db.Services.ToList();
            viewModel.Length_Types = db.LengthTypes.ToList();
            viewModel.Benchmarks = db.Benchmarks.ToList();
            viewModel.Currencies = db.Currencies.ToList();
            viewModel.Structure_Types = db.StructureTypes.ToList();
            viewModel.Relativitys = db.Relativities.ToList();
            viewModel.created_on = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
           
            //viewModel = db.Positions.ToList();
            //viewModel.TradableThings = db.Tradable_Thing.ToList();
            //viewModel.Releated_Trades = db.RelatedTrades.ToList();

            viewModel.Instruction_Types = db.InstructionTypes.ToList();
            viewModel.Hedge_Types = db.HedgeTypes.ToList();
            viewModel.Measure_Types = db.MeasureTypes.ToList();

            viewModel.length_type_id = 2;
            viewModel.relativity_id = 2;
            viewModel.structure_type_id = 4;
            viewModel.hedge_id = 2;
            viewModel.abs_measure_type_id = 1;
            viewModel.rel_measure_type_id = 2;

            return View(viewModel);
        }

        // POST: /TradesCreation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="trade_id,trade_uri,relativity_id,length_type_id,structure_type_id,service_id,currency_id,benchmark_id,trade_label,trade_editorial_label,created_on,created_by,last_updated,status")] Trade trade)
        {
            if (ModelState.IsValid)
            {
                db.Trades.Add(trade);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

          
            return View(trade);
        }

        // GET: /TradesCreation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
         
            return View(trade);
        }

        // POST: /TradesCreation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="trade_id,trade_uri,relativity_id,length_type_id,structure_type_id,service_id,currency_id,benchmark_id,trade_label,trade_editorial_label,created_on,created_by,last_updated,status")] Trade trade)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(trade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(trade);
        }

        // GET: /TradesCreation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            return View(trade);
        }

        // POST: /TradesCreation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trade trade = db.Trades.Find(id);
            db.Trades.Remove(trade);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
