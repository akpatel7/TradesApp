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
    public class TradeInstructionsController : ApiController
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
        public HttpResponseMessage Post(TradeInstructionDTO vm)
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
                                                      "Trade Instruction id: " +
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
                    result = "Trade Instruction post failed"
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

        private string PersistToDb(TradeInstructionDTO vm)
        {
            var isNewTradeInstruction = true;
            var tradeInstruction = new Trade_Instruction();

            if (vm.currency_id.HasValue)
            {
                var trade = unitOfWork.TradeRepository.Get(vm.trade_id);
                trade.currency_id = vm.currency_id;
            }

            tradeInstruction.trade_id = vm.trade_id;

            if (vm.trade_instruction_id > 0)
            {
                isNewTradeInstruction = false;
                tradeInstruction = unitOfWork.TradeInstructionRepository.GetByID(vm.trade_instruction_id);
            }

            tradeInstruction.instruction_entry = vm.instruction_entry;
            tradeInstruction.instruction_entry_date = DateTime.Parse(vm.instruction_entry_date);
            tradeInstruction.instruction_exit = vm.instruction_exit;
            if (!String.IsNullOrEmpty(vm.instruction_exit_date))
            {
                tradeInstruction.instruction_exit_date = DateTime.Parse(vm.instruction_exit_date);
            }
            tradeInstruction.instruction_type_id = vm.instruction_type_id;
            tradeInstruction.instruction_label = vm.instruction_label;
            tradeInstruction.hedge_id = vm.hedge_id;
            
            if (isNewTradeInstruction)
            {
                tradeInstruction.created_on = tradeInstruction.last_updated = DateTime.Now;
                unitOfWork.TradeInstructionRepository.Insert(tradeInstruction);
            }
            else
            {
                tradeInstruction.last_updated = DateTime.Now;
                unitOfWork.TradeInstructionRepository.Update(tradeInstruction);
            }

            unitOfWork.Save();

            return tradeInstruction.trade_instruction_id.ToString();

        }
    }
}