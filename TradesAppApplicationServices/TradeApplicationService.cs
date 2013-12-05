using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradesDataAccessServices;
using TradesBusinessServices;


namespace TradesAppApplicationServices
{
    
    public class TradeApplicationService
    {

        ///// <summary>
        ///// Start Trade Entry
        ///// </summary>
        ///// <param name="viewModel"></param>
        ///// <returns></returns>
        //public TradeViewModel BeginTradeEntry(string customerID)
        //{

        //    //CustomerDataAccessService customerDataAccessService = new CustomerDataAccessService();
        //    //Customer customer = customerDataAccessService.GetCustomerInformation(customerID);

        //    //var tradeViewModel = new TradeViewModel();
        //    //TradeViewModel.Customer = customer;

        //    //var TradeDataAccessService = new TradeDataAccessService();
        //    //TradeViewModel.Shippers = TradeDataAccessService.GetShippers();

        //    //var TradeBusinessService = new TradeBusinessService();
        //    //TradeViewModel.Trade = tradeBusinessService.InitializeTradeHeader(customer);
            
        //    var tradeViewModel = new TradeViewModel();
        //    return tradeViewModel;

        //}


        ///// <summary>
        ///// Start Trade Edit
        ///// </summary>
        ///// <param name="viewModel"></param>
        ///// <returns></returns>
        //public TradeViewModel BeginTradeEdit(int TradeID)
        //{

        //    //TradeDataAccessService TradeDataAccessService = new TradeDataAccessService();
        //    //TradeViewModel TradeViewModel = new TradeViewModel();

        //    //TradesCustomer TradeCustomer = TradeDataAccessService.GetTrade(TradeID);
        //    //TradeCustomer.Trade.TradeTotal = TradeDataAccessService.GetTradeTotal(TradeID);
        //    //TradeCustomer.Trade.TradeTotalFormatted = TradeCustomer.Trade.TradeTotal.ToString("C");

        //    //TradeViewModel.Customer = TradeCustomer.Customer;
        //    //TradeViewModel.Trade = TradeCustomer.Trade;
        //    //TradeViewModel.Shippers = TradeDataAccessService.GetShippers();
        //    //TradeViewModel.Trade.ShipperName = TradeCustomer.Shipper.CompanyName;

        //    //return TradeViewModel;

        //    var tradeViewModel = new TradeViewModel();
        //    return tradeViewModel;
        //}

        ///// <summary>
        ///// Get Trade Details
        ///// </summary>
        ///// <param name="viewModel"></param>
        ///// <returns></returns>
        //public TradeViewModel GetTradeDetails(int TradeID)
        //{

        //    //TradeDataAccessService TradeDataAccessService = new TradeDataAccessService();
        //    //TradeViewModel TradeViewModel = new TradeViewModel();

        //    //List<TradeDetailsProducts> TradeDetailsProducts = TradeDataAccessService.GetTradeDetails(TradeID);
        //    //TradesCustomer TradeCustomer = TradeDataAccessService.GetTrade(TradeID);
        //    //TradeViewModel.TradeDetailsProducts = TradeDetailsProducts;
        //    //TradeViewModel.Trade = TradeCustomer.Trade;
        //    //TradeViewModel.Customer = TradeCustomer.Customer;

        //    //return TradeViewModel;

        //    var tradeViewModel = new TradeViewModel();
        //    return tradeViewModel;

        //}

        ///// <summary>
        ///// Add Trade Detail Line Item
        ///// </summary>
        ///// <param name="TradeViewModel"></param>
        ///// <returns></returns>
        //public TradeViewModel AddTradeDetailLineItem(TradeViewModel TradeViewModel)
        //{
        //    //try
        //    //{
        //    //    TradeBusinessService TradeBusinessService = new TradeBusinessService();
        //    //    TradeBusinessService.ValidateTradeDetail(TradeViewModel.TradeDetail, true);
        //    //    if (TradeBusinessService.ValidationStatus == false)
        //    //    {
        //    //        TradeViewModel.ReturnMessage = TradeBusinessService.ValidationMessage;
        //    //        TradeViewModel.ReturnStatus = false;
        //    //        TradeViewModel.ValidationErrors = TradeBusinessService.ValidationErrors;
        //    //        return TradeViewModel;
        //    //    }

        //    //    TradeDataAccessService TradeDataAccessService = new TradeDataAccessService();
        //    //    TradeDataAccessService.CreateTradeDetailLineItem(TradeViewModel.TradeDetail);

        //    //    TradeDetailsProducts TradeDetailsProducts = TradeDataAccessService.GetTradeDetails(
        //    //        TradeViewModel.TradeDetail.TradeID, TradeViewModel.TradeDetail.ProductID);

        //    //    TradeViewModel.TradeLineItem = TradeDetailsProducts;
        //    //    TradeViewModel.ReturnStatus = true;

        //    //    List<String> returnMessage = new List<String>();
        //    //    returnMessage.Add("Trade line item has been added at " + DateTime.Now.ToString());
        //    //    TradeViewModel.ReturnMessage = returnMessage;

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    List<String> returnMessage = new List<String>();
        //    //    returnMessage.Add(ex.Message);

        //    //    TradeViewModel.ReturnMessage = returnMessage;
        //    //    TradeViewModel.ReturnStatus = false;
        //    //}

        //    var tradeViewModel = new TradeViewModel();
        //    return tradeViewModel;

        //}

        ///// <summary>
        ///// Update Trade Detail Line Item
        ///// </summary>
        ///// <param name="TradeViewModel"></param>
        ///// <returns></returns>
        //public TradeViewModel UpdateTradeDetailLineItem(TradeViewModel TradeViewModel)
        //{
        //    //try
        //    //{
        //    //    TradeBusinessService TradeBusinessService = new TradeBusinessService();
        //    //    TradeBusinessService.ValidateTradeDetail(TradeViewModel.TradeDetail, false);
        //    //    if (TradeBusinessService.ValidationStatus == false)
        //    //    {
        //    //        TradeViewModel.ReturnMessage = TradeBusinessService.ValidationMessage;
        //    //        TradeViewModel.ReturnStatus = false;
        //    //        TradeViewModel.ValidationErrors = TradeBusinessService.ValidationErrors;
        //    //        return TradeViewModel;
        //    //    }

        //    //    TradeDataAccessService TradeDataAccessService = new TradeDataAccessService();
        //    //    TradeDataAccessService.UpdateTradeDetailLineItem(TradeViewModel.TradeDetail);

        //    //    TradeDetailsProducts TradeDetailsProducts = TradeDataAccessService.GetTradeDetails(
        //    //        TradeViewModel.TradeDetail.TradeID, TradeViewModel.TradeDetail.ProductID);

        //    //    TradeViewModel.TradeLineItem = TradeDetailsProducts;
        //    //    TradeViewModel.ReturnStatus = true;

        //    //    List<String> returnMessage = new List<String>();
        //    //    returnMessage.Add("Trade line item has been updated at " + DateTime.Now.ToString());
        //    //    TradeViewModel.ReturnMessage = returnMessage;

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    List<String> returnMessage = new List<String>();
        //    //    returnMessage.Add(ex.Message);

        //    //    TradeViewModel.ReturnMessage = returnMessage;
        //    //    TradeViewModel.ReturnStatus = false;
        //    //}

        //    var tradeViewModel = new TradeViewModel();
        //    return tradeViewModel;

        //}


        ///// <summary>
        ///// Delete Trade Detail Line Item
        ///// </summary>
        ///// <param name="TradeViewModel"></param>
        ///// <returns></returns>
        //public TradeViewModel DeleteTradeDetailLineItem(TradeViewModel TradeViewModel)
        //{
        //    //try
        //    //{
        //    //    TradeBusinessService TradeBusinessService = new TradeBusinessService();

        //    //    int productID = TradeViewModel.TradeLineItem.TradeDetails.ProductID;
        //    //    int TradeID = TradeViewModel.TradeLineItem.TradeDetails.TradeID;

        //    //    TradeDataAccessService TradeDataAccessService = new TradeDataAccessService();
        //    //    TradeDataAccessService.DeleteTradeDetailLineItem(TradeID, productID);

        //    //    TradeViewModel.ReturnStatus = true;

        //    //    List<String> returnMessage = new List<String>();
        //    //    returnMessage.Add(TradeViewModel.TradeLineItem.Products.ProductName + " has been delete from this Trade at " + DateTime.Now.ToString());
        //    //    TradeViewModel.ReturnMessage = returnMessage;

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    List<String> returnMessage = new List<String>();
        //    //    returnMessage.Add(ex.Message);

        //    //    TradeViewModel.ReturnMessage = returnMessage;
        //    //    TradeViewModel.ReturnStatus = false;
        //    //}

        //    var tradeViewModel = new TradeViewModel();
        //    return tradeViewModel;

        //}

        ///// <summary>
        ///// Create Trade
        ///// </summary>
        ///// <param name="TradeViewModel"></param>
        ///// <returns></returns>
        //public TradeViewModel CreateTrade(TradeViewModel TradeViewModel)
        //{
        //    //try
        //    //{

        //    //    TradeBusinessService TradeBusinessService = new TradeBusinessService();
        //    //    TradeBusinessService.ValidateTradeHeader(TradeViewModel.Trade);
        //    //    if (TradeBusinessService.ValidationStatus == false)
        //    //    {
        //    //        TradeViewModel.ReturnMessage = TradeBusinessService.ValidationMessage;
        //    //        TradeViewModel.ReturnStatus = false;
        //    //        TradeViewModel.ValidationErrors = TradeBusinessService.ValidationErrors;
        //    //        return TradeViewModel;
        //    //    }

        //    //    TradeDataAccessService TradeDataAccessService = new TradeDataAccessService();
        //    //    TradeDataAccessService.CreateTrade(TradeViewModel.Trade);
        //    //    TradeViewModel = BeginTradeEdit(TradeViewModel.Trade.TradeID);
        //    //    TradeViewModel.Trade.RequiredDateFormatted = TradeViewModel.Trade.RequiredDate.ToShortDateString();

        //    //    List<String> returnMessage = new List<String>();
        //    //    returnMessage.Add("Trade number " + TradeViewModel.Trade.TradeID.ToString() + " has been created.");

        //    //    TradeViewModel.ReturnMessage = returnMessage;
        //    //    TradeViewModel.ReturnStatus = true;

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    List<String> returnMessage = new List<String>();
        //    //    returnMessage.Add(ex.Message);

        //    //    TradeViewModel.ReturnMessage = returnMessage;
        //    //    TradeViewModel.ReturnStatus = false;
        //    //}

        //    var tradeViewModel = new TradeViewModel();
        //    return tradeViewModel;

        //}


        ///// <summary>
        ///// Update Trade
        ///// </summary>
        ///// <param name="TradeViewModel"></param>
        ///// <returns></returns>
        //public TradeViewModel UpdateTrade(TradeViewModel TradeViewModel)
        //{
        //    //try
        //    //{

        //    //    TradeBusinessService TradeBusinessService = new TradeBusinessService();
        //    //    TradeBusinessService.ValidateTradeHeader(TradeViewModel.Trade);
        //    //    if (TradeBusinessService.ValidationStatus == false)
        //    //    {
        //    //        TradeViewModel.ReturnMessage = TradeBusinessService.ValidationMessage;
        //    //        TradeViewModel.ReturnStatus = false;
        //    //        TradeViewModel.ValidationErrors = TradeBusinessService.ValidationErrors;
        //    //        return TradeViewModel;
        //    //    }

        //    //    TradeDataAccessService TradeDataAccessService = new TradeDataAccessService();
        //    //    TradeDataAccessService.UpdateTrade(TradeViewModel.Trade);
        //    //    TradeViewModel = BeginTradeEdit(TradeViewModel.Trade.TradeID);
        //    //    TradeViewModel.Trade.RequiredDateFormatted = TradeViewModel.Trade.RequiredDate.ToShortDateString();

        //    //    List<String> returnMessage = new List<String>();
        //    //    returnMessage.Add("Trade has been updated at " + DateTime.Now.ToString());

        //    //    TradeViewModel.ReturnMessage = returnMessage;
        //    //    TradeViewModel.ReturnStatus = true;

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    List<String> returnMessage = new List<String>();
        //    //    returnMessage.Add(ex.Message);

        //    //    TradeViewModel.ReturnMessage = returnMessage;
        //    //    TradeViewModel.ReturnStatus = false;
        //    //}

        //    var tradeViewModel = new TradeViewModel();
        //    return tradeViewModel;

        //}

        ///// <summary>
        ///// Customer Inquiry
        ///// </summary>
        ///// <param name="viewModel"></param>
        ///// <returns></returns>
        //public TradeViewModel TradeInquiry(TradeViewModel viewModel)
        //{

        //    //int totalTrades;

        //    //List<String> returnMessage = new List<String>();

        //    //TradeDataAccessService TradeDataAccessService = new TradeDataAccessService();

        //    //try
        //    //{
        //    //    List<TradesCustomer> TradesCustomers = TradeDataAccessService.TradeInquiry(viewModel.Trade,
        //    //        viewModel.Customer,
        //    //        viewModel.CurrentPageNumber,
        //    //        viewModel.SortExpression,
        //    //        viewModel.SortDirection,
        //    //        viewModel.PageSize,
        //    //        out totalTrades);

        //    //    viewModel.TotalTrades = totalTrades;
        //    //    viewModel.TotalPages = Utilities.CalculateTotalPages(totalTrades, viewModel.PageSize);
        //    //    viewModel.TradeCustomer = TradesCustomers;

        //    //    if (viewModel.TotalTrades > 0 && viewModel.TotalPages < viewModel.CurrentPageNumber)
        //    //    {
        //    //        returnMessage.Add("Please resubmit your request.");
        //    //        viewModel.ReturnMessage = returnMessage;
        //    //        viewModel.ReturnStatus = false;
        //    //    }
        //    //    else
        //    //    {
        //    //        returnMessage.Add(totalTrades.ToString() + " Trades found.");
        //    //        viewModel.ReturnMessage = returnMessage;
        //    //        viewModel.ReturnStatus = true;
        //    //    }

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    returnMessage.Add("An application error has occurred. Please call technical support.");
        //    //    returnMessage.Add(ex.ToString());
        //    //    viewModel.ReturnStatus = false;
        //    //}

        //    //viewModel.ReturnMessage = returnMessage;

        //    //return viewModel;

        //    var tradeViewModel = new TradeViewModel();
        //    return tradeViewModel;

        //}


        //public List<TradeViewModel> GetAllTrades()
        //{
        //    var tradesList = new List<TradeViewModel>();

        //    return tradesList;
        //}
    }
}
