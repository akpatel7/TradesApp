using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using TradesWebApplication.DAL;
using TradesWebApplication.DAL.EFModels;
using TradesWebApplication.ViewModels;
using log4net;

namespace TradesWebApplication.Api
{
    public class InterestRateDiffController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] {"value1", "value2"};
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

        // POST api/<controller>
        public HttpResponseMessage Post([FromUri] string tradeId, [FromUri] string interestRateDiff)
        {

            if (ModelState.IsValid)
            {
                //var vm = new TradesViewModel();

                //string jsonData = value;

                string resultingTrackRecordId = "";

                //vm = JsonConvert.DeserializeObject<TradesViewModel>(value);
                try
                {
                    resultingTrackRecordId = PersistToDb(tradeId, interestRateDiff);
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

                return new HttpResponseMessage(HttpStatusCode.Created)
                    {
                        Content = new JsonContent(new
                            {
                                Success = true, //error
                                Message = "Track record id: " + resultingTrackRecordId + " sucessfully saved",
                                result = resultingTrackRecordId
                            })
                    };

            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new JsonContent(new
                        {
                            Success = false, //error
                            Message = "Fail", //return exception
                            result = "Track record post failed"
                        })
                };
        }

        private string PersistToDb(string tradeId, string interestRateDiff)
        {

            var trade = unitOfWork.TradeRepository.Get(int.Parse(tradeId));


            if (!String.IsNullOrEmpty(interestRateDiff))
            {
                var markTR = new Track_Record
                    {
                        trade_id = int.Parse(tradeId),
                        track_record_type_id = 2,
                        track_record_value = decimal.Parse(interestRateDiff)
                    };
                //TODO: NO field exists!! interestTR.created_on = 
                markTR.last_updated = DateTime.Now;
                unitOfWork.TrackRecordRepository.Insert(markTR);
                unitOfWork.Save();

                return markTR.track_record_id.ToString();

            }

            return "";

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