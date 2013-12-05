using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TradesApp.Models;
using TradesApp.ViewModels;

namespace TradesApp.Controllers
{
    public class TradeLineGroupController : ApiController
    {
        private BCATradeEntities db = new BCATradeEntities();

        // GET api/TradeGroup
        public ICollection<TradeLineGroupVM> GetTrade_Line_Group_Type()
        {
            // load one trade line
            var positions = from p in db.Positions select new PositionDTO() { position_id = p.position_id, position_label = p.position_label };
            var tradableThings = from t in db.Tradable_Thing select new TradableThingDTO() { tradable_thing_id = t.tradable_thing_id, tradable_thing_label = t.tradable_thing_label };
            TradeLineVM TradeLine = new TradeLineVM();
            TradeLine.Positions = positions.ToList();
            TradeLine.TradableThings = tradableThings.ToList();

            ICollection<TradeLineVM> tradeLineGroupList = new List<TradeLineVM>();
            tradeLineGroupList.Add(TradeLine);

            // load trade group
            var tradeLineGroupTypes = from g in db.Trade_Line_Group_Type select new TradeLineGroupTypeDTO() { trade_line_group_type_id = g.trade_line_group_type_id, trade_line_group_type_label = g.trade_line_group_type_label };

            TradeLineGroupVM TradeLineGroup = new TradeLineGroupVM();
            TradeLineGroup.TradeLineGroupTypes = tradeLineGroupTypes.ToList();
            TradeLineGroup.TradeLines = tradeLineGroupList.ToList();

            TradeLineGroup.trade_line_group_editorial_label = "Editorial Label";

            ICollection<TradeLineGroupVM> myList = new List<TradeLineGroupVM>();
            myList.Add(TradeLineGroup);

            return myList.ToList();
        }

        /*
        // GET api/TradeGroup/5
        public Trade_Line_Group_Type GetTrade_Line_Group_Type(int id)
        {
            Trade_Line_Group_Type trade_line_group_type = db.Trade_Line_Group_Type.Find(id);
            if (trade_line_group_type == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return trade_line_group_type;
        }

        // PUT api/TradeGroup/5
        public HttpResponseMessage PutTrade_Line_Group_Type(int id, Trade_Line_Group_Type trade_line_group_type)
        {
            if (ModelState.IsValid && id == trade_line_group_type.trade_line_group_type_id)
            {
                db.Entry(trade_line_group_type).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/TradeGroup
        public HttpResponseMessage PostTrade_Line_Group_Type(Trade_Line_Group_Type trade_line_group_type)
        {
            if (ModelState.IsValid)
            {
                db.Trade_Line_Group_Type.Add(trade_line_group_type);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, trade_line_group_type);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = trade_line_group_type.trade_line_group_type_id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/TradeGroup/5
        public HttpResponseMessage DeleteTrade_Line_Group_Type(int id)
        {
            Trade_Line_Group_Type trade_line_group_type = db.Trade_Line_Group_Type.Find(id);
            if (trade_line_group_type == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Trade_Line_Group_Type.Remove(trade_line_group_type);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, trade_line_group_type);
        }
        */

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}