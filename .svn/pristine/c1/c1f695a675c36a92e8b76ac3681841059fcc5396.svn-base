using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using TradesDataModel;

namespace TradesWebApplication.Data
{
    public class TradeEntitiesDataService: DbContext, ITradeEntitiesDataService
    {
        public TradeEntitiesDataService()
            : base("name=BCATrade_devEntities")
        {
        }

        public DbSet<Trades> Trades { get; set; }

        IQueryable<T> ITradeEntitiesDataService.Query<T>() 
        {
            throw new NotImplementedException();
        }

        void ITradeEntitiesDataService.Add<T>(T entity)
        {
            Set<T>().Add(entity);
        }

        void ITradeEntitiesDataService.Update<T>(T entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        void ITradeEntitiesDataService.Remove<T>(T entity)
        {
            Set<T>().Remove(entity);
        }

        void ITradeEntitiesDataService.SaveChanges()
        {
            SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        
    }
}