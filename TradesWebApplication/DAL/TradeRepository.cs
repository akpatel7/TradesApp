﻿using System;
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
            return context.Trades.Include(t => t.Benchmark).Include(t => t.Currency).Include(t => t.Length_Type).Include(t => t.Relativity).Include(t => t.Service).Include(t => t.Status1).Include(t => t.Structure_Type).Include( t => t.Trade_Instruction).ToList();

        }

        public Trade Get(int id)
        {
            var trade = context.Trades.Include(t => t.Benchmark).Include(t => t.Currency).Include(t => t.Length_Type).Include(t => t.Relativity).Include(t => t.Service).Include(t => t.Status1).Include(t => t.Structure_Type).Where( t => t.trade_id == id).Single();
            return trade; 
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
