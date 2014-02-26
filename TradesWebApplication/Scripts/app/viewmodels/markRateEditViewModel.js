//Start Comments section//////////////////////////
//Add Comment modal 
var AddMarkToMarketRateViewModel = function () {
    this.text = ko.validatedObservable().extend({ required: true }).extend({ number: true });
}

// The name of the template to render
AddMarkToMarketRateViewModel.prototype.template = "AddMarkToMarketRate";

AddMarkToMarketRateViewModel.prototype.add = function () {
    var newMarkToMarketRate = {
        text: this.text(),
    };
    var result = ko.validation.group(this, { deep: true });
    if (this.text.isValid()) {
        // Close the modal, passing the new note object as the result data.
        this.modal.close(newMarkToMarketRate);
    } else {
        result.showAllMessages(true);
    }
   
};

AddMarkToMarketRateViewModel.prototype.cancel = function () {
    // Close the modal without passing any result data.
    this.modal.close();
};

vm.addMarkToMarketRate = function () {
    showModal({
        viewModel: new AddMarkToMarketRateViewModel(),
        context: this // Set context so we don't need to bind the callback function
    }).then(this._addMarkToMarketRateToMarkToMarketRate);
};

vm._addMarkToMarketRateToMarkToMarketRate = function (newMarkToMarketRate) {
    
        console.log('saving added MarkToMarketRate');
        //this.trackRecord_id(0);
        //this.markToMarketRate(newMarkToMarketRate.text);
        //Start Mark to Market Rate save//////////////////////////
        console.log('Posting addMarkToMarketRate to server to save.');
        var apiURL = baseUrl;
        apiURL += "api/MarkToMarketRate/post";
        $.ajax({
            url: apiURL,
            type: 'post',
            data: { tradeId: vm.trade_Id, newMarktoMarketRate: newMarkToMarketRate.text },
            contentType: 'application/json',
            timeout: 15000,
            success: function(data) {
                if (data.Success) {
                    //update trade info
                    window.onbeforeunload = null;
                    console.log(data.Message);
                    bootbox.dialog({
                        message: data.Message,
                        title: "Mark to Mark Rate",
                        buttons: {
                            success: {
                                label: "OK",
                                className: "btn-success",
                                callback: function() {
                                    return true;
                                }
                            },
                            main: {
                                label: "Exit",
                                className: "btn-primary",
                                callback: function() {
                                    document.location.href = $('#cancelUrl').attr('href');
                                }
                            }
                        }
                    });
                    LoadTradeData(vm.trade_id);
                } else {
                    console.log(data.Message);
                    bootbox.alert(data.Message); //display exception

                }
            },
            error: function(XMLHttpRequest, textStatus, errorThrown) {
                console.log("error: " + XMLHttpRequest.responseText);
                var message = "error: " + XMLHttpRequest.responseText;
                bootbox.dialog({
                    message: message,
                    title: "Mark to Mark Rate",
                    className: "alert-danger",
                    buttons: {
                        danger: {
                            label: "OK",
                            className: "btn-danger",
                            callback: function() {
                            }
                        }
                    }
                });
            }
        });
    
}
