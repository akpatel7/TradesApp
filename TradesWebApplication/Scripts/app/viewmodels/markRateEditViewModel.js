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
    this.mark_to_mark_rate(newMarkToMarketRate.text);
    this.saveMarkToMarketRateData();
}
