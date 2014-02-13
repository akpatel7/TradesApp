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
        public object Get(int id)
        {
            if (ModelState.IsValid)
            {
                var vm = new TradesDTOViewModel();
                var tradeId = id;

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
                    var jsonResponse = JsonConvert.SerializeObject(platoTradeDTO);

                    var response = new RestClient
                        {
                            ContentType = "application/ld+json",
                            EndPoint = "$37383root:7373plato:848484:trades",
                            Method = HttpVerb.PUT,
                            PostData = jsonResponse
                        };

                    var response1 = new RestClient
                    {
                        ContentType = "application/json",
                        EndPoint = "http://localhost:63242/api/values/get",
                        Method = HttpVerb.GET,
                        PostData = jsonResponse
                    };

                    try
                    {
                        return response1.MakeRequest();
                        //return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                    catch
                    {
                        return new HttpResponseMessage(HttpStatusCode.NotAcceptable);
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

            return null;
        }


        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        private PlatoTradeContextDTO ConvertTradeDTOtoPlatoTradeDTO(TradesDTOViewModel vm)
        {
            return new PlatoTradeContextDTO();
        }

        private TradesDTOViewModel RetrieveTradeFromDb(int id)
        {
            var trade = unitOfWork.TradeRepository.Get(id);

            // this view model is to match the knockout json format to be easily serializable on the client side view
            var viewModel = new TradesDTOViewModel();

            viewModel.trade_id = trade.trade_id;

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
            else
            {
                viewModel.status = 1;
            }


            // trade instructions
            var tradeInstructionList = unitOfWork.TradeInstructionRepository.GetAll().Where(i => i.trade_id == trade.trade_id).ToList();
            var tradeInstruction = tradeInstructionList.LastOrDefault();
            if (tradeInstruction != null)
            {
                viewModel.trade_instruction_id = tradeInstruction.trade_instruction_id;
                viewModel.instruction_entry = tradeInstruction.instruction_entry;
                viewModel.instruction_entry_date = ((DateTime)tradeInstruction.instruction_entry_date).ToString("yyyy-MM-dd");
                viewModel.instruction_exit = tradeInstruction.instruction_exit;
                if (tradeInstruction.instruction_exit_date.HasValue)
                {
                    viewModel.instruction_exit_date = ((DateTime)tradeInstruction.instruction_exit_date).ToString("yyyy-MM-dd");
                }
                viewModel.instruction_type_id = tradeInstruction.instruction_type_id;
                viewModel.instruction_label = tradeInstruction.instruction_label;
                viewModel.hedge_id = tradeInstruction.hedge_id;
            }


            // related trades, TODO:
            var relatedTrades = unitOfWork.RelatedTradeRepository.GetAll().Where(r => r.trade_id == trade.trade_id).ToList();
            if (relatedTrades.Any())
            {
                int i = 0;
                viewModel.related_trade_ids = new int[relatedTrades.Count];
                foreach (var r in relatedTrades)
                {
                    viewModel.related_trade_ids[i] = r.related_trade_id;
                    viewModel.related_trade_ids_list += r.related_trade_id + ",";
                    i++;
                }
            }

            var trackRecordList = unitOfWork.TrackRecordRepository.GetAll().Where(r => r.trade_id == trade.trade_id).ToList();

            if (trackRecordList.Any())
            {
                // mark to mark rate
                var markTR = trackRecordList.LastOrDefault(m => m.track_record_type_id == 1);
                if (markTR != null)
                {
                    viewModel.mark_track_record_id = markTR.track_record_id;
                    viewModel.mark_to_mark_rate = markTR.track_record_value.ToString();
                }
                // intrest rate diff
                var interestTR = trackRecordList.LastOrDefault(m => m.track_record_type_id == 2);
                if (interestTR != null)
                {
                    viewModel.int_track_record_id = interestTR.track_record_id;
                    viewModel.interest_rate_diff = interestTR.track_record_value.ToString();
                }
            }


            //TODO: where does this go, which tradePerfomance
            string apl_func = "";
            bool isTradePerfomanceCreated = false;


            var tradePerformances = unitOfWork.TradePerformanceRepository.GetAll().Where(t => t.trade_id == trade.trade_id).ToList();

            if (tradePerformances.Any())
            {

                var abs_perf = tradePerformances.LastOrDefault(t => t.return_benchmark_id == null);

                // absolute performance
                if (abs_perf != null)
                {
                    viewModel.abs_track_performance_id = abs_perf.trade_performance_id;
                    viewModel.abs_measure_type_id = abs_perf.measure_type_id;
                    if (abs_perf.measure_type_id == 2)
                    {
                        viewModel.abs_currency_id = abs_perf.return_currency_id;
                    }
                    viewModel.abs_return_value = abs_perf.return_value;

                    if (!String.IsNullOrEmpty(abs_perf.return_apl_function))
                    {
                        apl_func = abs_perf.return_apl_function;
                        isTradePerfomanceCreated = true;
                    }

                }

                var rel_perf = tradePerformances.LastOrDefault(t => t.return_benchmark_id != null);

                // relative performance
                if (rel_perf != null)
                {
                    viewModel.rel_track_performance_id = rel_perf.trade_performance_id;
                    viewModel.rel_measure_type_id = rel_perf.measure_type_id;
                    if (rel_perf.measure_type_id == 2)
                    {
                        viewModel.rel_currency_id = rel_perf.return_currency_id;
                    }
                    viewModel.rel_return_value = rel_perf.return_value;

                    if (!String.IsNullOrEmpty(rel_perf.return_apl_function) && !isTradePerfomanceCreated)
                    {
                        isTradePerfomanceCreated = true;
                        apl_func = rel_perf.return_apl_function;
                    }

                    if (rel_perf.return_benchmark_id != null && rel_perf.return_benchmark_id > 0)
                    {
                        viewModel.return_benchmark_id = rel_perf.return_benchmark_id;
                    }
                }
            }

            //TODO: Verify if creating empty trade performacne for apl_func
            if (isTradePerfomanceCreated && !String.IsNullOrEmpty(apl_func))
            {
                viewModel.apl_func = apl_func;
            }


            //trade comments
            var comments = unitOfWork.TradeCommentRepository.GetAll().Where(c => c.trade_id == trade.trade_id).ToList();

            if (comments.Any())
            {
                var latestComment = comments.LastOrDefault();
                viewModel.comment_id = latestComment.comment_id;
                viewModel.comments = latestComment.comment_label;
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
                        tradable_thing_id = (int)tradeline.tradable_thing_id
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
