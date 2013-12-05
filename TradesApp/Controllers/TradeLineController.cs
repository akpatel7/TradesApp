using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TradesApp.Models;
using TradesApp.ViewModels;

namespace TradesApp.Controllers
{
    public class TradeLineController : ApiController
    {
        private BCATradeEntities db = new BCATradeEntities();

        // GET api/TradeLine
        public ICollection<TradeLineVM> GetTrade_Line()
        {

            var positions = from p in db.Positions select new PositionDTO() { position_id = p.position_id, position_label = p.position_label};
            var tradableThings = from t in db.Tradable_Thing select new TradableThingDTO() { tradable_thing_id = t.tradable_thing_id, tradable_thing_label = t.tradable_thing_label };
            
            TradeLineVM TradeLine = new TradeLineVM();
            TradeLine.Positions = positions.ToList();
            TradeLine.TradableThings = tradableThings.ToList();

            ICollection<TradeLineVM> myList = new List<TradeLineVM>();
            myList.Add(TradeLine);

            return myList;
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
