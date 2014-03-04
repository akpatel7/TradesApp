using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using TradesWebApplication.DAL;
using TradesWebApplication.DAL.EFModels;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TradesWebApplication.ViewModels;
using log4net;

namespace TradesWebApplication.Api
{
    public class AbsolutePerformancesController : ApiController
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
            return RetrieveFromDb(id);
        }

        private string RetrieveFromDb(int id)
        {
            var comment = unitOfWork.TrackRecordRepository.GetByID(id);

            return JsonConvert.SerializeObject(comment);
        }




        // POST api/<controller> [FromBody] string value
        public HttpResponseMessage Post(AbsolutePerformanceDTO vm)
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
                                Content = new JsonContent(new
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
                            Content = new JsonContent(new
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
                Content = new JsonContent(new
                {
                    Success = false, //error
                    Message = "Fail", //return exception
                    result = "Trade performance post failed"
                })
            };
        }

        private string PersistToDb(AbsolutePerformanceDTO vm)
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
            tradePerformance.return_date = vm.return_date;
            tradePerformance.last_updated = DateTime.Now;

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

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        public class JsonContent : HttpContent
        {

            private readonly MemoryStream _Stream = new MemoryStream();

            public JsonContent(object value)
            {

                Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var jw = new JsonTextWriter(new StreamWriter(_Stream));
                jw.Formatting = Formatting.Indented;
                var serializer = new JsonSerializer();
                serializer.Serialize(jw, value);
                jw.Flush();
                _Stream.Position = 0;

            }

            protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                return _Stream.CopyToAsync(stream);
            }

            protected override bool TryComputeLength(out long length)
            {
                length = _Stream.Length;
                return true;
            }

        }
    }
}