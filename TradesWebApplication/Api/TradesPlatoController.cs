using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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

                    return jsonObject;

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new JsonContent(new
                        {
                            Success = true, //error
                            Message = "Success", //return exception
                            Data = jsonObject
                        })
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
        [HttpPut]
        public object Put(string id, string endpoint)
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

                    var response = new RestClient
                    {
                        ContentType = "application/json+ld",
                        EndPoint = endpoint,
                        Method = HttpVerb.PUT,
                        PostData = jsonObject
                    };

                    try
                    {
                        var Httpresponse = response.MakeRequest("");
                        return Httpresponse;
                    }
                    catch (Exception ex)
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                        {
                            Content = new JsonContent(new
                            {
                                Success = false,
                                Message = "Exception occured: " + ex.InnerException.ToString(),
                                //return exception
                                result = "Exception occured: " + ex.InnerException.ToString()
                            })
                        };
                    }
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
            summary.service.id = service != null ? service.service_uri : String.Empty;
           
            //benchmark
            var benchmark =
                   unitOfWork.BenchmarkRepository.Get(
                       t => t.benchmark_id == vm.benchmark_id).SingleOrDefault();
            summary.benchmark.id = benchmark != null ? benchmark.benchmark_uri : String.Empty;
           

            //status
            var status =
                   unitOfWork.StatusRepository.Get(
                       t => t.status_id == vm.status).SingleOrDefault();
            var statusUri = @"http://data.emii.com/status/";
            statusUri += status != null ? status.status_label : String.Empty;
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
                group.tradelineGroupType.id = groupType != null ? groupType.trade_line_group_type_uri : String.Empty;

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

                    //tradableThing
                    var tradableThing =
                           unitOfWork.TradableThingRepository.Get(
                               t => t.tradable_thing_id == l.tradable_thing_id).SingleOrDefault();
                    line.onTradableThing.id = tradableThing != null ? tradableThing.tradable_thing_uri : String.Empty;

                    //position
                    var position =
                           unitOfWork.PositionRepository.Get(
                               t => t.position_id == l.position_id).SingleOrDefault();
                    line.tradeLinePosition.id = position != null ? position.position_uri : String.Empty;
                    
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
