//Start Comments section//////////////////////////
//Add Comment modal 
var AddInterestRateDiffViewModel = function () {
    this.text = ko.validatedObservable().extend({ required: true }).extend({ number: true });
}

// The name of the template to render
AddInterestRateDiffViewModel.prototype.template = "AddInterestRateDiff";

AddInterestRateDiffViewModel.prototype.add = function () {
    var newInterestRateDiff = {
        text: this.text(),
    };
    var result = ko.validation.group(this, { deep: true });
    if (this.text.isValid()) {
        // Close the modal, passing the new note object as the result data.
        this.modal.close(newInterestRateDiff);
    } else {
        result.showAllMessages(true);
    }

};

AddInterestRateDiffViewModel.prototype.cancel = function () {
    // Close the modal without passing any result data.
    this.modal.close();
};

vm.addInterestRateDiff = function () {
    showModal({
        viewModel: new AddInterestRateDiffViewModel(),
        context: this // Set context so we don't need to bind the callback function
    }).then(this._addInterestRateDiffToInterestRateDiff);
};

vm._addInterestRateDiffToInterestRateDiff = function (newInterestRateDiff) {
    this.interest_rate_diff(newInterestRateDiff.text);
    this.saveInterestRateDiffData();
}
