using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradesApp.Models;
using TradesApp.ViewModels;

namespace TradesApp.Controllers
{
    public class TradeController : Controller
    {
        private BCATradeEntities db = new BCATradeEntities();

        //
        // GET: /Trade/

        public ActionResult Index()
        {
            var trades = db.Trades.Include(t => t.Benchmark).Include(t => t.Currency).Include(t => t.Length_Type).Include(t => t.Relativity).Include(t => t.Service).Include(t => t.Status1).Include(t => t.Structure_Type);
            return View(trades.ToList());
        }

        //
        // GET: /Trade/Details/5

        public ActionResult Details(int id = 0)
        {
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            return View(trade);
        }

        //
        // GET: /Trade/Create

        public ActionResult Create()
        {

            string apiGroupUri = Url.HttpRouteUrl("DefaultApi", new { controller = "tradelinegroup", });
            ViewBag.ApiGroupUrl = new Uri(Request.Url, apiGroupUri).AbsoluteUri.ToString();

            string apiLineUri = Url.HttpRouteUrl("DefaultApi", new { controller = "tradeline", });
            ViewBag.ApiLineUrl = new Uri(Request.Url, apiLineUri).AbsoluteUri.ToString();

            var trade = new TradeVM();
            trade.Services = db.Services.ToList();
            trade.Length_Types = db.Length_Type.ToList();
            trade.Benchmarks = db.Benchmarks.ToList();
            trade.Currencies = db.Currencies.ToList();
            trade.Structure_Types = db.Structure_Type.ToList();
            trade.Relativitys = db.Relativities.ToList();
            trade.created_on = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //tradeViewModel.Trade_Line_Group_Types = db.Trade_Line_Group_Type.ToList();
            //tradeViewModel.Positions = db.Positions.ToList();
            //tradeViewModel.Tradable_Things = db.Tradable_Thing.ToList();
            trade.Instruction_Types = db.Instruction_Type.ToList();
            trade.Hedge_Types = db.Hedge_Type.ToList();
            trade.Releated_Trades = db.Trades.ToList();
            trade.Measure_Types = db.Measure_Type.ToList();

            trade.length_type_id = 2;
            trade.relativity_id = 2;
            trade.structure_type_id = 4;
            trade.hedge_id = 2;
            trade.abs_measure_type_id = 1;
            trade.rel_measure_type_id = 2;

            return View(trade);
        }

        //
        // POST: /Trade/Create

        [HttpPost]
        public ActionResult Create(TradeVM tradeVM)
        {
            Trade trade = new Trade();
            trade.service_id = tradeVM.service_id;
            trade.length_type_id = tradeVM.length_type_id;
            trade.relativity_id = tradeVM.relativity_id;
            trade.benchmark_id = tradeVM.benchmark_id;
            trade.created_on = DateTime.Parse(tradeVM.created_on);
            trade.trade_label = tradeVM.trade_label;
            trade.trade_editorial_label = tradeVM.trade_editorial_label;
            trade.currency_id = tradeVM.currency_id;

            // var prodcutCategory = Repository.GetProductCategory(productViewModel.SelectedValue);
            /* ---------------------------------------------------------------------------------------------------------------- */
            System.Diagnostics.Debug.WriteLine("--------------------------------------------------------------------");
            //System.Diagnostics.Debug.Indent();
            System.Diagnostics.Debug.WriteLine("service: " + trade.service_id);
            System.Diagnostics.Debug.WriteLine("type: " + trade.length_type_id);
            System.Diagnostics.Debug.WriteLine("benchmark: " + trade.relativity_id);
            System.Diagnostics.Debug.WriteLine("benchmark selection: " + trade.benchmark_id);
            System.Diagnostics.Debug.WriteLine("created on: " + trade.created_on);
            System.Diagnostics.Debug.WriteLine("canonical label: " + trade.trade_label);
            System.Diagnostics.Debug.WriteLine("editorrial label: " + trade.trade_editorial_label);
            //System.Diagnostics.Debug.WriteLine("line_group_count_str: " + line_group_count_str);
            //System.Diagnostics.Debug.WriteLine("line_group_count: " + line_group_count);
            //System.Diagnostics.Debug.WriteLine("group_id: " + group_id);
            //System.Diagnostics.Debug.WriteLine("group_id_1: " + group_id_1);
            /* ---------------------------------------------------------------------------------------------------------------- */

            bool valid_groups = false;
            bool valid_lines = false;
            ICollection<Trade_Line> TradeLines = new List<Trade_Line>();
            // get the line groups count
            var line_group_count_str = Request.Form["line_group_count"];
            int line_group_count = 0;
            if ((line_group_count_str != null) && (line_group_count_str != ""))
            {
                string[] str = line_group_count_str.ToString().Split(new Char[] { ' ', ',' });
                line_group_count = str.Length;

                System.Diagnostics.Debug.WriteLine("line_group_count_str: " + line_group_count_str);
                System.Diagnostics.Debug.WriteLine("line_group_count: " + line_group_count);
                
                if (line_group_count > 0)
                {
                    for (int i = 0; i < line_group_count; i++)
                    {
                        // build line groups
                        var line_group_type_id_str = Request.Form["trade_line_group_type_id_" + i.ToString()];
                        var line_group_editroial_label = Request.Form["trade_line_group_editorial_label_" + i.ToString()];
                        var trade_line_group_type_label = Request.Form["trade_line_group_type_label_" + i.ToString()];

                        int line_group_type_id = 0;
                        if (line_group_type_id_str != "")
                        {
                            line_group_type_id = Convert.ToInt32(line_group_type_id_str);
                        }

                        System.Diagnostics.Debug.WriteLine("line_group_type_id: " + line_group_type_id);

                        if (line_group_type_id > 0)
                        {
                            // we have atleast 1 group
                            valid_groups = true;
                            Trade_Line_Group lineGroup = new Trade_Line_Group();
                            lineGroup.trade_line_group_type_id = line_group_type_id;
                            lineGroup.trade_line_group_editorial_label = line_group_editroial_label;
                            lineGroup.trade_line_group_label = trade_line_group_type_label;

                            // build the lines for this group
                            var line_count_str = Request.Form["line_count"];
                            int line_count = 0;
                            if ((line_count_str != null) && (line_count_str != ""))
                            {
                                string[] str_l = line_count_str.ToString().Split(new Char[] { ' ', ',' });
                                line_count = str_l.Length;

                                System.Diagnostics.Debug.WriteLine("line_count_str: " + line_count_str);
                                System.Diagnostics.Debug.WriteLine("line_count: " + line_count);
                                System.Diagnostics.Debug.Indent();
                                
                                if (line_count > 0)
                                {
                                    for (int j = 0; j < line_count; j++)
                                    {
                                        var line_position_id_str = Request.Form["position_id_" + j.ToString()];
                                        var line_tradable_id_str = Request.Form["tradable_thing_id_" + j.ToString()];

                                        if ((line_position_id_str != null) && (line_tradable_id_str != null))
                                        {
                                            string[] positions_str = line_position_id_str.ToString().Split(new Char[] { ' ', ',' });
                                            string[] tradables_str = line_tradable_id_str.ToString().Split(new Char[] { ' ', ',' });

                                            line_position_id_str = positions_str[i];
                                            line_tradable_id_str = tradables_str[i];

                                            int line_position_id = 0;
                                            if (line_position_id_str != "")
                                            {
                                                line_position_id = Convert.ToInt32(line_position_id_str);
                                            }
                                            int line_tradable_id = 0;
                                            if (line_tradable_id_str != "")
                                            {
                                                line_tradable_id = Convert.ToInt32(line_tradable_id_str);
                                            }

                                            System.Diagnostics.Debug.WriteLine("line_position_id: " + line_position_id);
                                            System.Diagnostics.Debug.WriteLine("line_tradable_id: " + line_tradable_id);

                                            if ((line_position_id > 0) && (line_tradable_id > 0))
                                            {
                                                // we have atleast 1 line
                                                valid_lines = true;
                                                Trade_Line line = new Trade_Line();
                                                line.position_id = line_position_id;
                                                line.tradable_thing_id = line_tradable_id;
                                                line.Trade_Line_Group = lineGroup;
                                                TradeLines.Add(line);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            trade.Trade_Line = TradeLines;

            // trade instructions
            Trade_Instruction tradeInstruction = new Trade_Instruction();
            tradeInstruction.instruction_entry = tradeVM.instruction_entry;
            tradeInstruction.instruction_entry_date = DateTime.Parse(tradeVM.instruction_entry_date);
            tradeInstruction.instruction_exit = tradeVM.instruction_exit;
            if (tradeVM.instruction_exit_date != null)
            {
                tradeInstruction.instruction_exit_date = DateTime.Parse(tradeVM.instruction_exit_date);
            }
            tradeInstruction.instruction_type_id = tradeVM.instruction_type_id;
            tradeInstruction.instruction_label = tradeVM.instruction_label;
            tradeInstruction.hedge_id = tradeVM.hedge_id;
            
            // related trades
            Related_Trade relatedTrade = new Related_Trade();
            var related_trade_id = tradeVM.related_trade_id;
            if (related_trade_id != null)
            {
                relatedTrade.related_trade_id = (int)related_trade_id;
            }


            string apl_func = Request.Form["apl_func"];

            Track_Record markTR = new Track_Record();
            markTR.track_record_type_id = 1;
            if (tradeVM.mark_to_mark_rate != null)
            {
                markTR.track_record_value = decimal.Parse(tradeVM.mark_to_mark_rate);
            }
            Track_Record interestTR = new Track_Record();
            interestTR.track_record_type_id = 2;
            if (tradeVM.interest_rate_diff != null)
            {
                interestTR.track_record_value = decimal.Parse(tradeVM.interest_rate_diff);
            }
            
            // absolute performance
            Trade_Performance absPerformance = new Trade_Performance();
            var abs_measure_id = tradeVM.abs_measure_type_id;
            if ( abs_measure_id != null )
            {
            int abs_measure_type_id = (int)abs_measure_id;
            absPerformance.measure_type_id = abs_measure_type_id;
            if (abs_measure_type_id == 2)
            {
                absPerformance.return_currency_id = tradeVM.abs_currency_id;
            }
            absPerformance.return_value = tradeVM.abs_return_value;
            }

            // relative performance
            Trade_Performance relPerformance = new Trade_Performance();
            var rel_measure_id = tradeVM.rel_measure_type_id;
            if (rel_measure_id != null)
            {
                int rel_measure_type_id = (int)rel_measure_id;
                relPerformance.measure_type_id = rel_measure_type_id;
                if (rel_measure_type_id == 2)
                {
                    relPerformance.return_currency_id = tradeVM.rel_currency_id;
                }
                relPerformance.return_value = tradeVM.rel_return_value;
                relPerformance.return_benchmark_id = tradeVM.return_benchmark_id;
            }

            Trade_Comment comment = new Trade_Comment();
            comment.created_on = System.DateTime.Now;
            comment.comment_label = tradeVM.comments;

            if (!valid_groups || !valid_lines)
            {
                System.Diagnostics.Debug.WriteLine("NOT SAVING TRADE: Should have alteast 1 group with at leaset 1 line");
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var item in errors)
            {
                System.Diagnostics.Debug.WriteLine("error: " + item.ToString());
            }

            if ( (ModelState.IsValid) && valid_groups && valid_lines )
            {
                db.Trades.Add(trade);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            /*
            ViewBag.benchmark_id = new SelectList(db.Benchmarks, "benchmark_id", "benchmark_uri", trade.benchmark_id);
            ViewBag.currency_id = new SelectList(db.Currencies, "currency_id", "currency_uri", trade.currency_id);
            ViewBag.length_type_id = new SelectList(db.Length_Type, "length_type_id", "length_type_label", trade.length_type_id);
            ViewBag.relativity_id = new SelectList(db.Relativities, "relativity_id", "relativity_label", trade.relativity_id);
            ViewBag.service_id = new SelectList(db.Services, "service_id", "service_code", trade.service_id);
            //ViewBag.status = new SelectList(db.Status, "status_id", "status_label", trade.status);
            ViewBag.structure_type_id = new SelectList(db.Structure_Type, "structure_type_id", "structure_type_label", trade.structure_type_id);
            */

            // for display
            /*
            TradeVM tradeVM = new TradeVM();
            tradeVM.service_id = Convert.ToInt32(fc["service_id"]);
            tradeVM.length_type_id = Convert.ToInt32(fc["length_type_id"]);
            tradeVM.relativity_id = Convert.ToInt32(fc["relativity_id"]);
            tradeVM.benchmark_id = Convert.ToInt32(fc["benchmark_id"]);
            tradeVM.created_on = fc["created_on"].ToString();
            tradeVM.trade_label = fc["trade_label"].ToString();
            tradeVM.trade_editorial_label = fc["trade_editorial_label"].ToString();
            */
            tradeVM.Services = db.Services.ToList();
            tradeVM.Length_Types = db.Length_Type.ToList();
            tradeVM.Benchmarks = db.Benchmarks.ToList();
            tradeVM.Currencies = db.Currencies.ToList();
            tradeVM.Structure_Types = db.Structure_Type.ToList();
            tradeVM.Relativitys = db.Relativities.ToList();
            tradeVM.Instruction_Types = db.Instruction_Type.ToList();
            tradeVM.Hedge_Types = db.Hedge_Type.ToList();
            tradeVM.Releated_Trades = db.Trades.ToList();
            tradeVM.Measure_Types = db.Measure_Type.ToList();
                        
            return View(tradeVM);
        }

        //
        // GET: /Trade/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            ViewBag.benchmark_id = new SelectList(db.Benchmarks, "benchmark_id", "benchmark_label", trade.benchmark_id);
            ViewBag.currency_id = new SelectList(db.Currencies, "currency_id", "currency_uri", trade.currency_id);
            ViewBag.length_type_id = new SelectList(db.Length_Type, "length_type_id", "length_type_label", trade.length_type_id);
            ViewBag.relativity_id = new SelectList(db.Relativities, "relativity_id", "relativity_label", trade.relativity_id);
            ViewBag.service_id = new SelectList(db.Services, "service_id", "service_code", trade.service_id);
            ViewBag.status = new SelectList(db.Status, "status_id", "status_label", trade.status);
            ViewBag.structure_type_id = new SelectList(db.Structure_Type, "structure_type_id", "structure_type_label", trade.structure_type_id);
            return View(trade);
        }

        //
        // POST: /Trade/Edit/5

        [HttpPost]
        public ActionResult Edit(Trade trade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.benchmark_id = new SelectList(db.Benchmarks, "benchmark_id", "benchmark_uri", trade.benchmark_id);
            ViewBag.currency_id = new SelectList(db.Currencies, "currency_id", "currency_uri", trade.currency_id);
            ViewBag.length_type_id = new SelectList(db.Length_Type, "length_type_id", "length_type_label", trade.length_type_id);
            ViewBag.relativity_id = new SelectList(db.Relativities, "relativity_id", "relativity_label", trade.relativity_id);
            ViewBag.service_id = new SelectList(db.Services, "service_id", "service_uri", trade.service_id);
            ViewBag.status = new SelectList(db.Status, "status_id", "status_label", trade.status);
            ViewBag.structure_type_id = new SelectList(db.Structure_Type, "structure_type_id", "structure_type_label", trade.structure_type_id);
            return View(trade);
        }

        //
        // GET: /Trade/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            return View(trade);
        }

        //
        // POST: /Trade/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Trade trade = db.Trades.Find(id);
            db.Trades.Remove(trade);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}