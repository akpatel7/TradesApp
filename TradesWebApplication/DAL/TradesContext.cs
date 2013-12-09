using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using TradesWebApplication.DAL.EFModels;

namespace TradesWebApplication.DAL
{
    public class TradesContext : DbContext
    {
        public TradesContext() : base("BCATradeEntities")
        {
        }

        public DbSet<Trade> Trades { get; set; }
        public DbSet<Benchmark> Benchmarks { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Length_Type> LengthTypes { get; set; }
        public DbSet<Related_Trade> RelatedTrades { get; set; }
        public DbSet<Relativity> Relativities { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Structure_Type> StructureTypes { get; set; }
        public DbSet<Track_Record> TrackRecords { get; set; }
        public DbSet<Trade_Comment> TradeComments { get; set; }
        public DbSet<Trade_Performance> TradePerformances { get; set; }
        public DbSet<Trade_Line> TradeLines { get; set; }
        public virtual DbSet<Hedge_Type> HedgeTypes { get; set; }
        public virtual DbSet<Instruction_Type> InstructionTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Measure_Type> MeasureTypes { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Track_Record_Type> TrackRecordTypes { get; set; }
        public virtual DbSet<Tradable_Thing> TradableThings { get; set; }
        public virtual DbSet<Tradable_Thing_Class> TradableThingClasses { get; set; }
        public virtual DbSet<Trade_Line_Group> TradeLineGroups { get; set; }
        public virtual DbSet<Trade_Line_Group_Type> TradeLineGroupTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}