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
        public void Post([FromBody]string value)
        {
            var vm = new TradesCreationViewModel();

            string jsonData = value;
          
            vm = JsonConvert.DeserializeObject<TradesCreationViewModel>(value);

            PersistToDb(vm);
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


    }
}