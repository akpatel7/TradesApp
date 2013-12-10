using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TradesWebApplication.DAL.EFModels;

namespace TradesWebApplication.DAL
{
    public class TradeRepository : ITradeRepository
    {
        private TradesContext context;

        public TradeRepository(TradesContext context)
        {
            this.context = context;
        }

        public IEnumerable<Trade> GetTrades()
        {
            return context.Trades.ToList();
        }

        public Trade Get(int id)
        {
            return context.Trades.Find(id);
        }

        public void InsertTrade(Trade trade)
        {
            context.Trades.Add(trade);
        }

        public void DeleteTrade(int tradeID)
        {
            var trade = context.Trades.Find(tradeID);
            context.Trades.Remove(trade);
        }

        public void UpdateStudent(Trade trade)
        {
            context.Entry(trade).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
