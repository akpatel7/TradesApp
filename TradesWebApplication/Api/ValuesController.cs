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



namespace TradesWebApplication.Api
{
    public class ValuesController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private TradesAppSettings tradesConfig = TradesAppSettings.Settings;
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                return new HttpResponseMessage(HttpStatusCode.Accepted)
                {
                    Content = new JsonContent(new
                    {
                        Success = true, 
                        Message = "Trade: " + id + " sucessfully found", 
                        data = RetrieveTradeFromDb(id)
                    })
                };
                
            }
            catch (DataException ex)
            {
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

         
        }

        private string RetrieveTradeFromDb(int id)
        {
            var trade = unitOfWork.TradeRepository.Get(id);

            // this view model is to match the knockout json format to be easily serializable on the client side view
            var viewModel = new TradesEditViewModel();

            // service
            viewModel.service_id = (int)trade.service_id;
            // trade type
            viewModel.length_type_id = (int)trade.length_type_id;

            // benchmark
            viewModel.relativity_id = (int)trade.relativity_id;

            // benchmark selection
            viewModel.benchmark_id = trade.benchmark_id;

            // canonical label
            viewModel.trade_label = trade.trade_label;

            // editorial label
            viewModel.trade_editorial_label = trade.trade_editorial_label;

            // trade structure
            viewModel.structure_type_id = (int)trade.structure_type_id;

            // entry level
            //viewModel.instruction_entry = trade.Trade_Instruction.LastOrDefault().instruction_entry;

            // start date
            //viewModel.instruction_entry_date = trade.Trade_Instruction.LastOrDefault().instruction_entry_date == null ? "" : ((DateTime)(trade.Trade_Instruction.LastOrDefault().instruction_entry_date)).ToString("YYYY-mm-dd");

            // exit level
            //viewModel.instruction_exit = trade.Trade_Instruction.LastOrDefault().instruction_exit;

            // exit date
            // viewModel.instruction_exit_date = trade.Trade_Instruction.LastOrDefault().instruction_exit_date == null ? "" : ((DateTime)(trade.Trade_Instruction.LastOrDefault().instruction_exit_date)).ToString("YYYY-mm-dd");

            // instruction
            //viewModel.instruction_type_id = trade.Trade_Instruction.LastOrDefault().instruction_type_id;
            // viewModel.instruction_label = trade.Trade_Instruction.LastOrDefault().instruction_label;

            // hedge instruction
            //viewModel.hedge_id = trade.Trade_Instruction.LastOrDefault().hedge_id;

            // curency
            viewModel.currency_id = trade.Currency.currency_id;

            // supplementary info
            // APL function
            viewModel.apl_func = "";

            // mark to mark rate
            viewModel.mark_to_mark_rate = "";
            // internset rate diff
            viewModel.interest_rate_diff = "";

            // abs performance
            viewModel.abs_measure_type_id = null;
            viewModel.abs_currency_id = null;

            // rel performance
            viewModel.rel_measure_type_id = null;
            viewModel.rel_currency_id = null;
            viewModel.return_benchmark_id = null;

            // return value
            viewModel.abs_return_value = "";
            viewModel.rel_return_value = "";

            // comments
            //viewModel.comments = trade.Trade_Comment.LastOrDefault().comment_label;

            ////for ko json response
            //public List<TradeLineGroupViewModel> tradegroups { get; set; }
            //public List<TradeLineViewModel> tradeLines { get; set; }

            return JsonConvert.SerializeObject(viewModel);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]string value)
        {

            if (ModelState.IsValid)
            {
                var vm = new TradesCreationViewModel();

                string jsonData = value;

                vm = JsonConvert.DeserializeObject<TradesCreationViewModel>(value);
                try
                {
                    PersistToDb(vm);
                }
                catch (DataException ex)
                {
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
                        Message = "Trade: " + vm.trade_id + " sucessfully created", //return exception
                        result = "Trade: " + vm.trade_id +" sucessfully created"
                    })
                };
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new JsonContent(new
                {
                    Success = false, //error
                    Message = "Fail", //return exception
                    result = "Trade creation failed"
                })
            };
        }

        private void PersistToDb(TradesCreationViewModel vm)
        {
            //Create Trade
            Trade trade = new Trade();
            trade.service_id = vm.service_id;
            trade.length_type_id = vm.length_type_id;
            trade.relativity_id = vm.relativity_id;
            //only for related trades
            if (trade.relativity_id == 2) //2: benchmark is relative
            {
                if (vm.benchmark_id > 0)
                {
                    trade.benchmark_id = vm.benchmark_id;
                }
                
            }            
            trade.created_on = trade.last_updated = DateTime.Now;
            //TODO: Verify if db was changed to varmax
            if (!String.IsNullOrEmpty(vm.trade_label) )
            {
               if( vm.trade_label.Length > 255 )
               {
                    trade.trade_label = vm.trade_label.Substring(0, 255);
               }
               else
               {
                    trade.trade_label = vm.trade_label;
               }
            }
            
            
            //TODO: Verify if db was changed to varmax
            if (!String.IsNullOrEmpty(vm.trade_editorial_label) )
            {
                if (vm.trade_editorial_label.Length > 255)
                {
                    trade.trade_editorial_label = vm.trade_editorial_label.Substring(0, 255);
                }
                else 
                {
                    trade.trade_editorial_label = vm.trade_editorial_label;
                }
            } 
        
            trade.structure_type_id = vm.structure_type_id;
            trade.currency_id = vm.currency_id;
            //TODO: createdby
            trade.created_by = 0;
            //STATUS Always Visible on create
            trade.status = 1; //1 is Visible, 2: Invisible, 3: Deleted
            unitOfWork.TradeRepository.InsertTrade(trade);
            unitOfWork.Save();
            var newTradeId = trade.trade_id;
            vm.trade_id = newTradeId;
            //TODO: verify uri
            trade.trade_uri = tradesConfig.TradeSemanticURIPrefix + getFlakeID() + tradesConfig.TradeSemanticURISuffix;

            //Add groups
            foreach (var grp in vm.tradegroups)
            {
                var lineGroup = new Trade_Line_Group();
                lineGroup.trade_line_group_type_id = grp.trade_line_group_type_id;
                //TODO: Verify if db was changed to varmax
                if (!String.IsNullOrEmpty(grp.trade_line_group_label))
                {
                    if (grp.trade_line_group_label.Length > 255)
                    {
                        lineGroup.trade_line_group_label = grp.trade_line_group_label.Substring(0, 255);
                    }
                    else
                    {
                        lineGroup.trade_line_group_label = grp.trade_line_group_label;
                    }
                }
                //TODO: Verify if db was changed to varmax
                if (!String.IsNullOrEmpty(grp.trade_line_group_editorial_label))
                {
                    if (grp.trade_line_group_editorial_label.Length > 255)
                    {
                        lineGroup.trade_line_group_editorial_label = grp.trade_line_group_editorial_label.Substring(0, 255);
                    }
                    else
                    {
                        lineGroup.trade_line_group_editorial_label = grp.trade_line_group_editorial_label;
                    }
                }

                unitOfWork.TradeLineGroupRepository.Insert(lineGroup);
                unitOfWork.Save();
                var grpId = lineGroup.trade_line_group_id;
                //TODO: verify uri
                lineGroup.trade_line_group_uri = tradesConfig.TradeLineGroupSemanticURIPrefix + getFlakeID() + tradesConfig.TradeLineGroupSemanticURISuffix;

                //Add tradelines
                foreach (var line in grp.tradeLines)
                {
                    var tradeLine = new Trade_Line();
                    tradeLine.trade_line_group_id = grpId; //this groupID
                    tradeLine.trade_id = newTradeId; //this trade
                    tradeLine.position_id = line.position_id;
                    tradeLine.tradable_thing_id = line.tradable_thing_id;
                    //?trade_line_editorial_label
                    //?trade_line_label
                    //TODO: createdby
                    tradeLine.created_on = tradeLine.last_updated = DateTime.Now;
                    unitOfWork.TradeLineRepository.Insert(tradeLine);
                    unitOfWork.Save();
                    var newTradeLineId = tradeLine.trade_line_id;
                    //TODO: verify uri
                    tradeLine.trade_line_uri = tradesConfig.TradeLineSemanticURIPrefix + getFlakeID() + tradesConfig.TradeLineGroupSemanticURISuffix;
                }
                
            }

            // trade instructions
            var tradeInstruction = new Trade_Instruction
            {
                trade_id = newTradeId,
                instruction_entry = vm.instruction_entry,
                instruction_entry_date = DateTime.Parse(vm.instruction_entry_date),
                instruction_exit = vm.instruction_exit
            };
            if (!String.IsNullOrEmpty(vm.instruction_exit_date))
            {
                tradeInstruction.instruction_exit_date = DateTime.Parse(vm.instruction_exit_date);
            }
            tradeInstruction.instruction_type_id = vm.instruction_type_id;
            tradeInstruction.instruction_label = vm.instruction_label;
            tradeInstruction.hedge_id = vm.hedge_id;
            tradeInstruction.created_on = tradeInstruction.last_updated = DateTime.Now;
            unitOfWork.TradeInstructionRepository.Insert(tradeInstruction);

            // related trades, TODO:
            if (vm.related_trade_ids != null)
            {
                foreach (var i in vm.related_trade_ids)
                {
                    var relatedTrade = new Related_Trade();
                    relatedTrade.trade_id = newTradeId;
                    relatedTrade.related_trade_id = i;
                    relatedTrade.created_on = relatedTrade.last_updated = DateTime.Now;
                    //TODO: created by
                    unitOfWork.RelatedTradeRepository.Insert(relatedTrade);
                }
            }
               
            //TODO: where does this go, which tradePerfomance
            string apl_func = vm.apl_func;
            bool isTradePerfomanceCreated = false;

            if (!String.IsNullOrEmpty(vm.mark_to_mark_rate))
            {
                var markTR = new Track_Record
                {
                    trade_id = newTradeId,
                    track_record_type_id = 1,
                    track_record_value = decimal.Parse(vm.mark_to_mark_rate)
                };
                //TODO: NO field exists!! interestTR.created_on = 
                markTR.last_updated = DateTime.Now;
                unitOfWork.TrackRecordRepository.Insert(markTR);

            }
            
            if (!String.IsNullOrEmpty(vm.interest_rate_diff))
            {
                var interestTR = new Track_Record
                {
                    trade_id = newTradeId,
                    track_record_type_id = 2,
                    track_record_value = decimal.Parse(vm.interest_rate_diff)
                };
                //TODO: NO field exists!! interestTR.created_on = 
                interestTR.last_updated = DateTime.Now;
                unitOfWork.TrackRecordRepository.Insert(interestTR);
            }

            // absolute performance
            
            var abs_measure_id = vm.abs_measure_type_id;
            if (abs_measure_id != null && !String.IsNullOrEmpty(vm.abs_return_value))
            {
                var absPerformance = new Trade_Performance();
                absPerformance.trade_id = newTradeId;
                var abs_measure_type_id = (int)abs_measure_id;
                absPerformance.measure_type_id = abs_measure_type_id;
                if (abs_measure_type_id == 2)
                {
                    absPerformance.return_currency_id = vm.abs_currency_id;
                }
                absPerformance.return_value = vm.abs_return_value;
                isTradePerfomanceCreated = true;
                if(!String.IsNullOrEmpty(apl_func))
                {
                    absPerformance.return_apl_function = apl_func;
                }
                absPerformance.created_on = absPerformance.last_updated = DateTime.Now;
                unitOfWork.TradePerformanceRepository.Insert(absPerformance);
            }

            // relative performance
            var rel_measure_id = vm.rel_measure_type_id;
            if (rel_measure_id != null && !String.IsNullOrEmpty(vm.rel_return_value))
            {
                var relPerformance = new Trade_Performance();
                relPerformance.trade_id = newTradeId;
                var rel_measure_type_id = (int)rel_measure_id;
                relPerformance.measure_type_id = rel_measure_type_id;
                if (rel_measure_type_id == 2)
                {
                    relPerformance.return_currency_id = vm.rel_currency_id;
                }
                relPerformance.return_value = vm.rel_return_value;
                if (vm.return_benchmark_id != null && vm.return_benchmark_id > 0)
                {
                    relPerformance.return_benchmark_id = vm.return_benchmark_id;
                }               
                isTradePerfomanceCreated = true;
                if (!String.IsNullOrEmpty(apl_func))
                {
                    relPerformance.return_apl_function = apl_func;
                }
                relPerformance.created_on = relPerformance.last_updated = DateTime.Now;
                unitOfWork.TradePerformanceRepository.Insert(relPerformance);
            }

            //TODO: Verify if creating empty trade performacne for apl_func
            if (!String.IsNullOrEmpty(apl_func) && 
                !isTradePerfomanceCreated)
            {
                var tradePerformance = new Trade_Performance();
                tradePerformance.trade_id = newTradeId;
                tradePerformance.return_apl_function = apl_func;
                unitOfWork.TradePerformanceRepository.Insert(tradePerformance);
            }

            if (!String.IsNullOrEmpty(vm.comments))
            {
                if (vm.comments.Length > 255)
                {
                    vm.comments = vm.comments.Substring(0, 255);
                }
                var comment = new Trade_Comment
                {
                    trade_id = newTradeId, 
                    comment_label = vm.comments, 
                };
                comment.created_on = comment.last_updated = DateTime.Now;
                unitOfWork.TradeCommentRepository.Insert(comment);
            }
            unitOfWork.Save();
           
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

        public string getFlakeID(string parameters = "")
        {
            string endPoint = tradesConfig.FlakeServiceURI;
            var client = new RestClient(endPoint);
            var response = client.MakeRequest();
            if (String.IsNullOrEmpty(response))
            {
                throw new Exception("Error while calling FlakeId service");
            }
            return response;
        }

    }

}