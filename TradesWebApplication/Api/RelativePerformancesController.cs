using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using TradesWebApplication.DAL;
using TradesWebApplication.DAL.EFModels;
using TradesWebApplication.ViewModels;
using log4net;

namespace TradesWebApplication.Api
{
    public class RelativePerformancesController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        /// POST api/<controller>
        public HttpResponseMessage Post(RelativePerformanceDTO vm)
        {

            if (ModelState.IsValid)
            {

                string resultingTrackPerformanceRecordId = "";

                try
                {
                    resultingTrackPerformanceRecordId = PersistToDb(vm);

                    if (!String.IsNullOrEmpty(resultingTrackPerformanceRecordId))
                    {
                        return new HttpResponseMessage(HttpStatusCode.Created)
                        {
                            Content = new AbsolutePerformancesController.JsonContent(new
                            {
                                Success = true, //error
                                Message =
                                                      "Trade Performance id: " +
                                                      resultingTrackPerformanceRecordId + " sucessfully saved",
                                result = resultingTrackPerformanceRecordId
                            })
                        };
                    }

                }
                catch (DataException ex)
                {

                    LogManager.GetLogger("ErrorLogger").Error(ex);
                    LogManager.GetLogger("EmailLogger").Error(ex);

                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new AbsolutePerformancesController.JsonContent(new
                        {
                            Success = false,
                            Message = "Database Exception occured: " + ex.InnerException.ToString(),
                            //return exception
                            result = "Database Exception occured: " + ex.InnerException.ToString()
                        })
                    };
                }

            }//end  if (ModelState.IsValid)

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new AbsolutePerformancesController.JsonContent(new
                {
                    Success = false, //error
                    Message = "Fail", //return exception
                    result = "Trade performance post failed"
                })
            };
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        private string PersistToDb(RelativePerformanceDTO vm)
        {
            var isNewTradePerfromance = true;
            var tradePerformance = new Trade_Performance();
            if (vm.trade_performance_id != null && vm.trade_performance_id > 0)
            {
                isNewTradePerfromance = false;
                tradePerformance = unitOfWork.TradePerformanceRepository.GetByID((int)vm.trade_performance_id);
            }

            tradePerformance.trade_id = vm.trade_id;
            tradePerformance.return_benchmark_id = null;
            tradePerformance.measure_type_id = vm.measure_type_id;
            tradePerformance.return_currency_id = vm.return_currency_id;
            tradePerformance.return_value = vm.return_value;
            tradePerformance.return_benchmark_id = vm.return_benchmark_id;
            tradePerformance.last_updated = vm.last_updated;

            if (isNewTradePerfromance)
            {
                unitOfWork.TradePerformanceRepository.Insert(tradePerformance);
            }
            else
            {
                unitOfWork.TradePerformanceRepository.Update(tradePerformance);
            }

            unitOfWork.Save();

            return tradePerformance.trade_performance_id.ToString();

        }
    }
}