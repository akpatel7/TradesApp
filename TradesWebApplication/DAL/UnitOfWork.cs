using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradesWebApplication.DAL.EFModels;

namespace TradesWebApplication.DAL
{
    public class UnitOfWork : IDisposable
    {
        private TradesContext context = new TradesContext();

        //For trades
        private ITradeRepository tradeRepository;

        //for view models
        private GenericRepository<Trade_Line> tradeLineRepository;
        private GenericRepository<Trade_Line_Group> tradeLineGroupRepository;
        private GenericRepository<Trade_Line_Group_Type> tradeLineGroupTypeRepository;
        //for view
        private GenericRepository<Service> serviceRepository;
        private GenericRepository<Relativity> relativityRepository;
        private GenericRepository<Benchmark> benchmarkRepository;
        private GenericRepository<Length_Type> lengthTypeRepository;
        private GenericRepository<Currency> currencyRepository;
        private GenericRepository<Hedge_Type> hedgeTypeRepository;
        private GenericRepository<Measure_Type> measureTypeRepository;
        private GenericRepository<Instruction_Type> instructionTypeRepository;
        private GenericRepository<Structure_Type> structureTypeRepository;
        //rest of tables
        private GenericRepository<Location> locationRepository;
        private GenericRepository<Tradable_Thing> tradableThingRepository;
        private GenericRepository<Tradable_Thing_Class> tradableThingClassRepository;
        private GenericRepository<Position> positionRepository;
        private GenericRepository<Status> statusRepository;
        private GenericRepository<Trade_Comment> tradeCommentRepository;
        private GenericRepository<Track_Record> trackRecordRepository;
        private GenericRepository<Track_Record_Type> trackRecordTypeRepository;
        private GenericRepository<Trade_Performance> tradePerformanceRepository;
        private GenericRepository<Trade_Instruction> tradeInstructionRepository;
        //Related Trade
        private GenericRepository<Related_Trade> relatedTradeRepository;


        public ITradeRepository TradeRepository
        {
            get
            {

                if (this.tradeRepository == null)
                {
                    this.tradeRepository = new TradeRepository(context);
                }
                return tradeRepository;
            }
        }

        public GenericRepository<Trade_Line> TradeLineRepository
        {
            get
            {

                if (this.tradeLineRepository == null)
                {
                    this.tradeLineRepository = new GenericRepository<Trade_Line>(context);
                }
                return tradeLineRepository;
            }
        }

        public GenericRepository<Trade_Line_Group> TradeLineGroupRepository
        {
            get
            {

                if (this.tradeLineGroupRepository == null)
                {
                    this.tradeLineGroupRepository = new GenericRepository<Trade_Line_Group>(context);
                }
                return tradeLineGroupRepository;
            }
        }

        public GenericRepository<Trade_Line_Group_Type> TradeLineGroupTypeRepository
        {
            get
            {

                if (this.tradeLineGroupTypeRepository == null)
                {
                    this.tradeLineGroupTypeRepository = new GenericRepository<Trade_Line_Group_Type>(context);
                }
                return tradeLineGroupTypeRepository;
            }
        }

        //for view
        public GenericRepository<Service> ServiceRepository
        {
            get
            {

                if (this.serviceRepository == null)
                {
                    this.serviceRepository = new GenericRepository<Service>(context);
                }
                return serviceRepository;
            }
        }

        public GenericRepository<Relativity> RelativityRepository
        {
            get
            {

                if (this.relativityRepository == null)
                {
                    this.relativityRepository = new GenericRepository<Relativity>(context);
                }
                return relativityRepository;
            }
        }

        public GenericRepository<Benchmark> BenchmarkRepository
        {
            get
            {

                if (this.benchmarkRepository == null)
                {
                    this.benchmarkRepository = new GenericRepository<Benchmark>(context);
                }
                return benchmarkRepository;
            }
        }

        public GenericRepository<Length_Type> LengthTypeRepository
        {
            get
            {

                if (this.lengthTypeRepository == null)
                {
                    this.lengthTypeRepository = new GenericRepository<Length_Type>(context);
                }
                return lengthTypeRepository;
            }
        }

        public GenericRepository<Currency> CurrencyRepository
        {
            get
            {

                if (this.currencyRepository == null)
                {
                    this.currencyRepository = new GenericRepository<Currency>(context);
                }
                return currencyRepository;
            }
        }

        public GenericRepository<Hedge_Type> HedgeTypeRepository
        {
            get
            {

                if (this.hedgeTypeRepository == null)
                {
                    this.hedgeTypeRepository = new GenericRepository<Hedge_Type>(context);
                }
                return hedgeTypeRepository;
            }
        }

        public GenericRepository<Measure_Type> MeasureTypeRepository
        {
            get
            {

                if (this.measureTypeRepository == null)
                {
                    this.measureTypeRepository = new GenericRepository<Measure_Type>(context);
                }
                return measureTypeRepository;
            }
        }

        public GenericRepository<Instruction_Type> InstructionTypeRepository
        {
            get
            {

                if (this.instructionTypeRepository == null)
                {
                    this.instructionTypeRepository = new GenericRepository<Instruction_Type>(context);
                }
                return instructionTypeRepository;
            }
        }

        public GenericRepository<Structure_Type> StructureTypeRepository
        {
            get
            {

                if (this.structureTypeRepository == null)
                {
                    this.structureTypeRepository = new GenericRepository<Structure_Type>(context);
                }
                return structureTypeRepository;
            }
        }

        //rest of tables
        public GenericRepository<Location> LocationRepository
        {
            get
            {

                if (this.locationRepository == null)
                {
                    this.locationRepository = new GenericRepository<Location>(context);
                }
                return locationRepository;
            }
        }

        public GenericRepository<Tradable_Thing> TradableThingRepository
        {
            get
            {

                if (this.tradableThingRepository == null)
                {
                    this.tradableThingRepository = new GenericRepository<Tradable_Thing>(context);
                }
                return tradableThingRepository;
            }
        }

        public GenericRepository<Tradable_Thing_Class> TradableThingClassRepository
        {
            get
            {

                if (this.tradableThingClassRepository == null)
                {
                    this.tradableThingClassRepository = new GenericRepository<Tradable_Thing_Class>(context);
                }
                return tradableThingClassRepository;
            }
        }

        public GenericRepository<Position> PositionRepository
        {
            get
            {

                if (this.positionRepository == null)
                {
                    this.positionRepository = new GenericRepository<Position>(context);
                }
                return positionRepository;
            }
        }

        public GenericRepository<Status> StatusRepository
        {
            get
            {

                if (this.statusRepository == null)
                {
                    this.statusRepository = new GenericRepository<Status>(context);
                }
                return statusRepository;
            }
        }

        public GenericRepository<Trade_Comment> TradeCommentRepository
        {
            get
            {

                if (this.tradeCommentRepository == null)
                {
                    this.tradeCommentRepository = new GenericRepository<Trade_Comment>(context);
                }
                return tradeCommentRepository;
            }
        }

        public GenericRepository<Track_Record> TrackRecordRepository
        {
            get
            {

                if (this.trackRecordRepository == null)
                {
                    this.trackRecordRepository = new GenericRepository<Track_Record>(context);
                }
                return trackRecordRepository;
            }
        }

        public GenericRepository<Track_Record_Type> TrackRecordTypeRepository
        {
            get
            {

                if (this.trackRecordTypeRepository == null)
                {
                    this.trackRecordTypeRepository = new GenericRepository<Track_Record_Type>(context);
                }
                return trackRecordTypeRepository;
            }
        }

        public GenericRepository<Trade_Performance> TradePerformanceRepository
        {
            get
            {

                if (this.tradePerformanceRepository == null)
                {
                    this.tradePerformanceRepository = new GenericRepository<Trade_Performance>(context);
                }
                return tradePerformanceRepository;
            }
        }

        public GenericRepository<Trade_Instruction> TradeInstructionRepository
        {
            get
            {

                if (this.tradeInstructionRepository == null)
                {
                    this.tradeInstructionRepository = new GenericRepository<Trade_Instruction>(context);
                }
                return tradeInstructionRepository;
            }
        }

        //Related Trade
        public GenericRepository<Related_Trade> RelatedTradeRepository
        {
            get
            {

                if (this.relatedTradeRepository == null)
                {
                    this.relatedTradeRepository = new GenericRepository<Related_Trade>(context);
                }
                return relatedTradeRepository;
            }
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