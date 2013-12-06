using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesWebApplication.DAL.EFModels;

namespace TradesWebApplication.DAL
{
    public interface ITradeRepository : IDisposable
    {
        IEnumerable<Trade> GetTrades();
        Trade GetTradeByID(int tradeId);
        void InsertTrade(Trade trade);
        void DeleteTrade(int tradeID);
        void UpdateStudent(Trade trade);
        void Save();
    }
}
