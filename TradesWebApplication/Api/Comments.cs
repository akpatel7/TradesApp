using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TradesWebApplication.DAL;
using TradesWebApplication.DAL.EFModels;
using TradesWebApplication.ViewModels;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using log4net;

namespace TradesWebApplication.Api
{
    public class CommentsController : ApiController
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
            var comment = unitOfWork.TradeCommentRepository.GetByID(id);

            return JsonConvert.SerializeObject(comment);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]string value)
        {

            if (ModelState.IsValid)
            {
                var vm = new TradesViewModel();

                string jsonData = value;

                string resultingCommentId = "";

                vm = JsonConvert.DeserializeObject<TradesViewModel>(value);
                try
                {
                    resultingCommentId = PersistToDb(vm);
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
                            Message = "Database Exception occured: " + ex.InnerException.ToString(), //return exception
                            result = "Database Exception occured: " + ex.InnerException.ToString()
                        })
                    };
                }

                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new JsonContent(new
                    {
                        Success = true, //error
                        Message = "Trade comment id: " + resultingCommentId + " sucessfully saved",
                        result = resultingCommentId
                    })
                };

            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new JsonContent(new
                {
                    Success = false, //error
                    Message = "Fail", //return exception
                    result = "Trade comment post failed"
                })
            };
        }

        private string PersistToDb(TradesViewModel vm)
        {
            Trade trade = new Trade();
            if (!String.IsNullOrEmpty(vm.CRUDMode) && vm.CRUDMode == "edit")
            { //edit mode
                trade = unitOfWork.TradeRepository.Get(vm.trade_id);
            }
            else
            { //should never be in trade Create mode in this contrller
                return "";
            }

            var comment = new Trade_Comment();
            if (vm.comment_id > 0)
            {
                //update comment
                comment = unitOfWork.TradeCommentRepository.GetByID(vm.comment_id);
                if (!String.IsNullOrEmpty(vm.comments))
                {
                    if (vm.comments.Length > 255)
                    {
                        vm.comments = vm.comments.Substring(0, 255);
                    }
                    comment.comment_label = vm.comments;
                    comment.last_updated = DateTime.Now;
                }
            }
            else
            {  //new comment

                if (!String.IsNullOrEmpty(vm.comments))
                {
                    if (vm.comments.Length > 255)
                    {
                        vm.comments = vm.comments.Substring(0, 255);
                    }
                    
                    comment = new Trade_Comment
                    {
                        trade_id = (int)vm.trade_id,
                        comment_label = vm.comments,
                    };
                    comment.created_on = comment.last_updated = DateTime.Now;
                    unitOfWork.TradeCommentRepository.Insert(comment);
                }
            }
            unitOfWork.Save();

            return comment.comment_id.ToString();

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
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