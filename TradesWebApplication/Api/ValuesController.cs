using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TradesWebApplication.DAL;
using TradesWebApplication.ViewModels;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TradesWebApplication.Api
{
    public class ValuesController : ApiController
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

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]string value)
        {

            if (ModelState.IsValid)
            {
                var vm = new TradesCreationViewModel();

                string jsonData = value;

                vm = JsonConvert.DeserializeObject<TradesCreationViewModel>(value);

                PersistToDb(vm);

                //return new HttpResponseMessage(HttpStatusCode.NoContent);
                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new JsonContent(new
                    {
                        Success = true, //error
                        Message = "Success" //return exception
                    })
                };
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new JsonContent(new
                {
                    Success = false, //error
                    Message = "Fail" //return exception
                })
            };
        }

        private void PersistToDb(TradesCreationViewModel vm)
        {
            return;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }


        // GET api/<controller>/<action>
        [System.Web.Http.HttpGet]
         public IEnumerable<string> Test()
        {
            var values = new[] { "John", "Pete", "Ben" };

            return values;
   
       }

       public class JsonContent : HttpContent {

    private readonly MemoryStream _Stream = new MemoryStream();
    public JsonContent(object value) {

        Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var jw = new JsonTextWriter( new StreamWriter(_Stream));
        jw.Formatting = Formatting.Indented;
        var serializer = new JsonSerializer();
        serializer.Serialize(jw, value);
        jw.Flush();
        _Stream.Position = 0;

    }
    protected override Task SerializeToStreamAsync(Stream stream, TransportContext context) {
        return _Stream.CopyToAsync(stream);
    }

    protected override bool TryComputeLength(out long length) {
        length = _Stream.Length;
        return true;
    }
}


    }
}