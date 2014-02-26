//Start Comments section//////////////////////////
//Add Comment modal 
var AddCommentViewModel = function () {
    this.text = ko.validatedObservable().extend({ required: true }).extend({ maxLength: 255 });
}

// The name of the template to render
AddCommentViewModel.prototype.template = "AddComment";

AddCommentViewModel.prototype.add = function () {
    var newComment = {
        text: this.text(),
    };
    if (this.text.isValid()) {
        // Close the modal, passing the new note object as the result data.
        this.modal.close(newComment);
    }
};

AddCommentViewModel.prototype.cancel = function () {
    // Close the modal without passing any result data.
    this.modal.close();
};

vm.addComment = function () {
    showModal({
        viewModel: new AddCommentViewModel(),
        context: this // Set context so we don't need to bind the callback function
    }).then(this._addCommentToComments);
};

vm._addCommentToComments = function (newComment) {
    console.log('saving added comment');
    this.comment_id(0);
    this.comments(newComment.text);
    this.saveCommentData();
};
//End Add Comment modal 

//Edit Comment modal 
var EditCommentViewModel = function () {
    this.id = ko.observable();
    this.text = ko.validatedObservable().extend({ required: true }).extend({ maxLength: 255 });
}

// The name of the template to render
EditCommentViewModel.prototype.template = "EditComment";

EditCommentViewModel.prototype.save = function () {
    var editedComment = {
        id: this.id,
        text: this.text(),
    };
    if (this.text.isValid()) {
        // Close the modal, passing the new note object as the result data.
        this.modal.close(editedComment);
    }
};

EditCommentViewModel.prototype.cancel = function () {
    // Close the modal without passing any result data.
    this.modal.close();
};

vm.editComment = function () {
    var viewModel = new EditCommentViewModel();
    viewModel.id(vm.comment_id());
    viewModel.text(vm.comments());
    showModal({
        viewModel: viewModel,
        context: this // Set context so we don't need to bind the callback function
    }).then(this._editCommentToComments);
};

vm._editCommentToComments = function (editedComment) {
    console.log('saving edited comment');
    this.comment_id(editedComment.id);
    this.comments(editedComment.text);
    this.saveCommentData();
};
//End Edit Comment modal


//End Comments section//////////////////////////