using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using RestSharp;
using TradesWebApplication.DAL;
using TradesWebApplication.SemanticModels;
using TradesWebApplication.ViewModels;

namespace TradesWebApplication.Api
{
    public class TradesPlatoController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private TradesAppSettings tradesConfig = TradesAppSettings.Settings;

        // GET api/<controller>
        public object GetFromIsis([FromUri(Name = "endpoint")]string endpoint = "")
        {
            var client = new RestSharp.RestClient();
            //client.BaseUrl = endpoint;
            //client.Authenticator = new HttpBasicAuthenticator("username", "password");
            var userId = User.Identity.Name;

            var request = new RestRequest(endpoint, Method.GET);

            request.AddHeader("Accept", "application/ld+json");
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/ld+json; charset=utf-8");
            //request.AddHeader("Content-Type", "application/ld+json; charset=utf-8");
            request.AddHeader("consumer-id", TradesAppSettings.Settings.ConsumerId);
            request.AddHeader("Authorization", GetAuthenticationHeader(userId, TradesAppSettings.Settings.SharedSecret));

            IRestResponse response = client.Execute(request);

            return String.Format("Status Code: {0}\nResponse: {1}\nContent-Length: {2}\nBody: {3}", response.StatusCode, response.StatusDescription, response.ContentLength, response.Content);

        }

        // GET api/<controller>/5
        public object Get(string id)
        {
            if (ModelState.IsValid)
            {
                var vm = new TradesDTOViewModel();
                var tradeId = int.Parse(id);
                vm.trade_id = tradeId;
                try
                {
                    vm = RetrieveTradeFromDb(tradeId);
                }
                catch (DataException ex)
                {
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

                try
                {
                    var platoTradeDTO = ConvertTradeDTOtoPlatoTradeDTO(vm);
                    var jsonObject = JsonConvert.SerializeObject(platoTradeDTO);

                    //return jsonObject;
                    

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new JsonContent(jsonObject)
                    };


                }
                catch (Newtonsoft.Json.JsonException ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new JsonContent(new
                                {
                                    Success = false,
                                    Message = "Json conversion exception occured: " + ex.InnerException.ToString(),
                                    //return exception
                                    result = "Json conversion exception occured: " + ex.InnerException.ToString()
                                })
                        };
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new JsonContent(new
                                {
                                    Success = false,
                                    Message = "Unknown exception occured: " + ex.InnerException.ToString(),
                                    //return exception
                                    result = "Unknown exception occured: " + ex.InnerException.ToString()
                                })
                        };
                }

            }

            return null;
        }


        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]string postdata)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent(new
                {
                    Success = true, //error
                    Message = "Post test", //return exception
                    result = postdata
                })
            };
        }

        // PUT api/<controller>/5
        [System.Web.Http.AcceptVerbs("PUT","GET")]
        public object Put([FromUri(Name = "id")]string id, [FromUri(Name = "endpoint")]string endpoint = "")
        {
            if (String.IsNullOrEmpty(endpoint))
            {
                endpoint = TradesAppSettings.Settings.IsisTradesEndpoint;
            }

            //if (ModelState.IsValid) removed because we retrieve from successful trades entires only

                var vm = new TradesDTOViewModel();
                var tradeId = int.Parse(id);
                vm.trade_id = tradeId;
                try
                {
                    vm = RetrieveTradeFromDb(tradeId);
                }
                catch (DataException ex)
                {
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

                try
                {
                    var platoTradeDTO = ConvertTradeDTOtoPlatoTradeDTO(vm);
                    var jsonObject = JsonConvert.SerializeObject(platoTradeDTO);

                    var response =  SendTradeToIsis(jsonObject, endpoint);

                    return String.Format("Status Code: {0}\nResponse: {1}\nContent-Length: {2}\nBody: {3}", response.StatusCode, response.StatusDescription, response.ContentLength, response.Content);

                    //var response = new RestClient
                    //{
                    //    ContentType = "application/ld+json; charset=utf-8",
                    //    EndPoint = TradesAppSettings.Settings.IsisTradeEndpoint,
                    //    Method = HttpVerb.PUT,
                    //    PostData = jsonObject
                    //};

                    //try
                    //{
                    //    var Httpresponse = response.MakeRequest("");
                    //    return Httpresponse;
                    //}
                    //catch (Exception ex)
                    //{
                    //    return new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    //    {
                    //        Content = new JsonContent(new
                    //        {
                    //            Success = false,
                    //            Message = "Exception occured: " + ex.InnerException.ToString(),
                    //            //return exception
                    //            result = "Exception occured: " + ex.InnerException.ToString()
                    //        })
                    //    };
                    //}
                }
                catch (Newtonsoft.Json.JsonException ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new JsonContent(new
                        {
                            Success = false,
                            Message = "Json conversion exception occured: " + ex.InnerException.ToString(),
                            //return exception
                            result = "Json conversion exception occured: " + ex.InnerException.ToString()
                        })
                    };
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new JsonContent(new
                        {
                            Success = false,
                            Message = "Unknown exception occured: " + ex.InnerException.ToString(),
                            //return exception
                            result = "Unknown exception occured: " + ex.InnerException.ToString()
                        })
                    };
                }

       

            return new HttpResponseMessage(HttpStatusCode.InternalServerError);

        }


        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        private PlatoTradeBodyDTO ConvertTradeDTOtoPlatoTradeDTO(TradesDTOViewModel vm)
        {
            var platoTradeDTO = new PlatoTradeBodyDTO();
            platoTradeDTO.context = new PlatoTradeContextDTO();
            
            var summary = new PlatoTradeGraphSummary();
            var tradeGroupNodes = new List<PlatoTradeGroupDTO>();
            var tradelineNodes = new List<PlatoTradeLineDTO>();

            //@id
            summary.id = vm.trade_uri;
            
            //service
            var service = 
                   unitOfWork.ServiceRepository.Get(
                       t => t.service_id == vm.service_id).SingleOrDefault();
            summary.service.id = service != null ? service.service_uri : "";
           
            //benchmark
            var benchmark =
                   unitOfWork.BenchmarkRepository.Get(
                       t => t.benchmark_id == vm.benchmark_id).SingleOrDefault();
            summary.benchmark.id = benchmark != null ? benchmark.benchmark_uri : "";
           

            //status
            var status =
                   unitOfWork.StatusRepository.Get(
                       t => t.status_id == vm.status).SingleOrDefault();
            var statusUri = @"http://data.emii.com/status/";
            if (status != null)
            {
                var statusDesc = status.status_label;
                statusDesc = statusDesc.Replace(' ', '-');
                statusUri += statusDesc;
            }
            summary.status.id = statusUri;

            
            //canonicalLabel
            summary.canonicalLabel.language = "en";
            summary.canonicalLabel.value = vm.trade_label;

            //rights
            summary.rights.id = @"http://data.emii.com/business-groups/bca";

            //add tradelines to summary
            foreach( var g in vm.tradegroups )
            {
                //add to root of graph node
                var group = new PlatoTradeGroupDTO()
                    {
                        id = g.trade_line_group_uri,
                        type = @"http://data.emii.com/ontologies/bcatrading/TradeLineGroup",
                    };

                //group type
                var groupType =
                       unitOfWork.TradeLineGroupTypeRepository.Get(
                           t => t.trade_line_group_type_id == g.trade_line_group_type_id).SingleOrDefault();
                group.tradelineGroupType.id = groupType != null ? groupType.trade_line_group_type_uri : "";

                group.canonicalLabel.language = "en";
                group.canonicalLabel.value = g.trade_line_group_label;

                tradeGroupNodes.Add(group);

                foreach (var l in g.tradeLines)
                {
                    //add line to graphsummary
                    summary.tradeLine.Add( new GenericIdPlatoSemanticDTO() { id = l.trade_line_uri});

                    //add to graph root
                    var line = new PlatoTradeLineDTO()
                    {
                        id = l.trade_line_uri,
                        type = @"http://data.emii.com/ontologies/bcatrading/TradeLine",
                    };

                    //tradeline group
                    line.tradeLineGroup.id = g.trade_line_group_uri;

                    //tradableThing
                    var tradableThing =
                           unitOfWork.TradableThingRepository.Get(
                               t => t.tradable_thing_id == l.tradable_thing_id).SingleOrDefault();
                    line.onTradableThing.id = tradableThing != null ? tradableThing.tradable_thing_uri : "";

                    //position
                    var position =
                           unitOfWork.PositionRepository.Get(
                               t => t.position_id == l.position_id).SingleOrDefault();
                    line.tradeLinePosition.id = position != null ? position.position_uri : "";
                    
                    line.canonicalLabel.language = "en";
                    line.canonicalLabel.value = l.canonicalLabelPart;

                    tradelineNodes.Add(line);

                }
            }

            
            platoTradeDTO.graph.Add(summary);
            foreach (var g in tradeGroupNodes)
            {
                platoTradeDTO.graph.Add(g);
            }
            foreach (var t in tradelineNodes)
            {
                platoTradeDTO.graph.Add(t);
            }

            return platoTradeDTO;
        }

        private TradesDTOViewModel RetrieveTradeFromDb(int id)
        {
            var trade = unitOfWork.TradeRepository.Get(id);

            // this view model is to match the knockout json format to be easily serializable on the client side view
            var viewModel = new TradesDTOViewModel();

            viewModel.trade_id = trade.trade_id;

            viewModel.trade_uri = trade.trade_uri;

            // service
            viewModel.service_id = (int)trade.service_id;
            // trade type
            viewModel.length_type_id = (int)trade.length_type_id;

            // benchmark
            viewModel.relativity_id = (int)trade.relativity_id;

            // benchmark selection
            viewModel.benchmark_id = trade.benchmark_id;

            //last_updated
            if (trade.last_updated.HasValue)
            {
                viewModel.last_updated = ((DateTime)trade.last_updated).ToString("yyyy-MM-dd");
            }

            // canonical label
            viewModel.trade_label = trade.trade_label;

            // editorial label
            viewModel.trade_editorial_label = trade.trade_editorial_label;

            // trade structure
            viewModel.structure_type_id = (int)trade.structure_type_id;

            viewModel.currency_id = trade.currency_id;

            //TODO: createdby
            viewModel.last_updated_by = 0;

            //STATUS Always Visible on create
            if (trade.status.HasValue)
            {
                viewModel.status = (int)trade.status; //1 is Unpublished, 2:Ready To Publish, 3: Published, 4: Deleted
            }
            
            //trade groups and lines
            var tradeLines = unitOfWork.TradeLineRepository.GetAll().Where(t => t.trade_id == viewModel.trade_id).ToList();

            if (tradeLines.Any())
            {
                viewModel.tradegroups = new List<TradeLineGroupDTOViewModel>();

                foreach (var tradeline in tradeLines)
                {

                    var tradeLineVM = new TradeLineDTOViewModel
                    {
                        trade_line_id = tradeline.trade_line_id,
                        position_id = (int)tradeline.position_id,
                        tradable_thing_id = (int)tradeline.tradable_thing_id,
                        trade_line_editorial_label = tradeline.trade_line_editorial_label,
                        trade_line_uri = tradeline.trade_line_uri,
                        canonicalLabelPart = tradeline.trade_line_label
                    };

                    var tradeLineGroup = unitOfWork.TradeLineGroupRepository.GetByID(tradeline.trade_line_group_id);

                    if (tradeLineGroup != null)
                    {
                        var tradeLineGroupVM = new TradeLineGroupDTOViewModel
                        {
                            trade_line_group_id = tradeLineGroup.trade_line_group_id,
                            trade_line_group_type_id = (int)tradeLineGroup.trade_line_group_type_id,
                            trade_line_group_editorial_label = tradeLineGroup.trade_line_group_editorial_label,
                            trade_line_group_label = tradeLineGroup.trade_line_group_label,
                            trade_line_group_uri = tradeLineGroup.trade_line_group_uri

                        };

                        bool groupExists = false;
                        //check to see if exists
                        if (viewModel.tradegroups.Any())
                        {
                            for (int i = 0; i < viewModel.tradegroups.Count; i++)
                            {
                                if (viewModel.tradegroups[i].trade_line_group_id == tradeLineGroupVM.trade_line_group_id)
                                {
                                    viewModel.tradegroups[i].tradeLines.Add(tradeLineVM);
                                    groupExists = true;
                                    break;
                                }
                            }
                        }

                        if (!groupExists)
                        {
                            if (tradeLineGroupVM.tradeLines == null)
                            {
                                tradeLineGroupVM.tradeLines = new List<TradeLineDTOViewModel>();
                            }
                            tradeLineGroupVM.tradeLines.Add(tradeLineVM);
                            viewModel.tradegroups.Add(tradeLineGroupVM);
                        }

                    }
                }
            }

            return viewModel;
        }

        public IRestResponse SendTradeToIsis(object tradeGraph, string endpoint)
        {
            var client = new RestSharp.RestClient();
            //client.BaseUrl = endpoint;
            //client.Authenticator = new HttpBasicAuthenticator("username", "password");
            var userId = User.Identity.Name;

            var request = new RestRequest(endpoint, Method.PUT);
            
            request.AddHeader("Accept", "application/ld+json");
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "application/ld+json; charset=utf-8");
            //request.AddHeader("Content-Type", "application/ld+json; charset=utf-8");
            request.AddHeader("consumer-id", TradesAppSettings.Settings.ConsumerId);
            request.AddHeader("Authorization", GetAuthenticationHeader(userId, TradesAppSettings.Settings.SharedSecret));
            
            //request.RequestFormat = DataFormat.Json;
            //request.AddBody(tradeGraph);
            request.AddParameter("application/ld+json", tradeGraph, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return response;
        }

        private string GetAuthenticationHeader(string user_id, string secret_key)
        {

            //$version = "1";
            var version = 1;
            //$device_id = "0000";
            var device_id = "device";
            //$permissions = "3";
            var permissions = "3";
            //$expiry = mktime(0, 0, 0, 1, 1, 2015);
            var expiry = mktime(2015, 1, 1);
            //$random_str = rand_str(15);
            //var random_str = rand_str(15);
            var random_str = "A Random String";
            var token = version + ":" + device_id + ":" + user_id + ":" +
                permissions + ":" + expiry + ":" + random_str;
            //$signature = (string)base64_encode(hash_hmac("sha256", $token, $CI->config->item('shared_secret'), TRUE));
            var signature = (hash_hmac_256_base64(token, secret_key));
            //$unsigned_token = utf8_encode($token . ":" . $signature);
            var unsigned_token = token + ":" + signature;
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(unsigned_token);
            //var auth_token = System.Convert.ToBase64String(toEncodeAsBytes);
            string auth_token = Convert.ToBase64String(encbuff);
           
            //// set the header
            var auth_str = @"ISIS realm=""bcaresearch.com"" token=""" + auth_token + @"""";

            return auth_str;
        }


        private string rand_str(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        private string mktime(int year, int month, int day)
        {
            var unixTimestamp = (Int32)((new DateTime(year,month,day)).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp.ToString();
        }

        private string hash_hmac_256_base64(string token, string key)
        {
            Encoding encoding = Encoding.UTF8;
            var keyByte = encoding.GetBytes(key);
            var hmac256 = new HMACSHA256(keyByte);
            hmac256.ComputeHash(encoding.GetBytes(token));

            return Convert.ToBase64String(hmac256.Hash);
        }

        public void PushToPlato(int tradeId, string endpoint = "")
        {
            if (String.IsNullOrEmpty(endpoint))
            {
                endpoint = TradesAppSettings.Settings.IsisTradesEndpoint;
            }

            //if (ModelState.IsValid) removed because we retrieve from successful trades entires only

            var vm = new TradesDTOViewModel();
            vm.trade_id = tradeId;
            try
            {
                vm = RetrieveTradeFromDb(tradeId);
            }
            catch (DataException ex)
            {
                //log ex;
                throw ex;
            }

            try
            {
                var platoTradeDTO = ConvertTradeDTOtoPlatoTradeDTO(vm);
                var jsonObject = JsonConvert.SerializeObject(platoTradeDTO);

                var response = SendTradeToIsis(jsonObject, endpoint);

                //log error if responseStatus is not no content
                String.Format("Status Code: {0}\nResponse: {1}\nContent-Length: {2}\nBody: {3}",
                              response.StatusCode, response.StatusDescription, response.ContentLength,
                              response.Content);
            }
            catch (Exception ex)
            {
                //log ex;
                throw ex;
            }
        }

        public void PushStatusToPlato(string tradeUri, int status, string endpoint = "")
        {
            if (String.IsNullOrEmpty(endpoint))
            {
                endpoint = TradesAppSettings.Settings.IsisTradesStatusEndpoint;
            }

            var statusURL = endpoint + "?uri=" + tradeUri;

            switch (status)
            {
                case 1:
                    statusURL += "&status=http://data.emii.com/status/unpublished";
                    break;

                case 2:
                    statusURL += "&status=http://data.emii.com/status/ready-for-publish";
                    break;

                case 3:
                    statusURL += "&status=http://data.emii.com/status/published";
                    break;

                case 4:
                    statusURL += "&status=http://data.emii.com/status/deleted";
                    break;

                default:
                    break;
            }

            try
            {
                var client = new RestSharp.RestClient();
                var userId = User.Identity.Name;

                var request = new RestRequest(statusURL, Method.PUT);

                request.AddHeader("Accept", "application/ld+json");
                request.Parameters.Clear();
                request.AddHeader("Content-Type", "application/ld+json; charset=utf-8");
                request.AddHeader("consumer-id", TradesAppSettings.Settings.ConsumerId);
                request.AddHeader("Authorization", GetAuthenticationHeader(userId, TradesAppSettings.Settings.SharedSecret));


                IRestResponse response = client.Execute(request);

                //log error if responseStatus is not no content
                String.Format("Status Code: {0}\nResponse: {1}\nContent-Length: {2}\nBody: {3}",
                              response.StatusCode, response.StatusDescription, response.ContentLength,
                              response.Content);
            }
            catch (Exception ex)
            {
                //log ex;
                throw ex;
            }
        }

    }
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
