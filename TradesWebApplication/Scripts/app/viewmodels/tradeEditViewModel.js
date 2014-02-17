
var createModalElement = function (templateName, viewModel) {
    var temporaryDiv = addHiddenDivToBody();
    var deferredElement = $.Deferred();
    ko.renderTemplate(
        templateName,
        viewModel,
        // We need to know when the template has been rendered,
        // so we can get the resulting DOM element.
        // The resolve function receives the element.
        {
            afterRender: function (nodes) {
                // Ignore any #text nodes before and after the modal element.
                var elements = nodes.filter(function (node) {
                    return node.nodeType === 1; // Element
                });
                deferredElement.resolve(elements[0]);
            }
        },
        // The temporary div will get replaced by the rendered template output.
        temporaryDiv,
        "replaceNode"
    );
    // Return the deferred DOM element so callers can wait until it's ready for use.
    return deferredElement;
};

var addHiddenDivToBody = function () {
    var div = document.createElement("div");
    div.style.display = "none";
    document.body.appendChild(div);
    return div;
};

ko.bindingHandlers.uniqueIdTradableThing = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        value.id = value.id || ko.bindingHandlers.uniqueIdTradableThing.prefix + (++ko.bindingHandlers.uniqueIdTradableThing.counter);

        element.id = value.id;
    },
    counter: 0,
    prefix: "tradableThing"
}

ko.bindingHandlers.uniqueIdPosition = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        value.id = value.id || ko.bindingHandlers.uniqueIdPosition.prefix + (++ko.bindingHandlers.uniqueIdPosition.counter);

        element.id = value.id;
    },
    counter: 0,
    prefix: "position"
}

ko.bindingHandlers.selectedText = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        value($("option:selected", element).text());

        $(element).change(function () {
            value($("option:selected", this).text());
        });
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        $("option", element).filter(function (el) { return $(el).text() === value; }).prop("selected", "selected");
    }
};

ko.bindingHandlers.select2 = {
    init: function (element, valueAccessor) {
        $(element).select2(valueAccessor());

        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).select2('destroy');
        });
    },
    update: function (element) {
        $(element).trigger('change');
    }
};

ko.bindingHandlers.datePicker = {
	    init: function (element, valueAccessor, allBindingsAccessor) {       
	        //initialize datepicker with some optional options
	        var options = allBindingsAccessor().datepickerOptions || { format: 'dd/mm/yyyy', autoclose: true };
	        $(element).datepicker(options);
	        
	        //when a user changes the date, update the view model
	        ko.utils.registerEventHandler(element, "changeDate", function (event) {
	            var value = valueAccessor();
	            if (ko.isObservable(value)) {
	                value(event.date);
	            }
	        });
	         
	        ko.utils.registerEventHandler(element, "change", function () {
	            var widget = $(element).data("datepicker");
	             
	            var value = valueAccessor();           
	            if (ko.isObservable(value)) {
	                if (element.value) {
	                    var date = widget.getUTCDate();                                       
	                    value(date);
	                } else {                   
	                    value(null);
	                }
	 
	            }
	        });
	    },
	    update: function (element, valueAccessor) {
	             
	        var widget = $(element).data("datepicker");
	 
	        //when the view model is updated, update the widget
	        if (widget) {
	            widget.date = ko.utils.unwrapObservable(valueAccessor());
	 
	            if (!widget.date) {
	                return;
	            }
	 
	            if (_.isString(widget.date)) {
	                widget.setDate(moment(widget.date).toDate());
	                return;
	            }
	 
	            widget.setValue();
	        }       
	    }
	};


ko.validation.init({ insertMessages: false });
ko.validation.configure({
    decorateElement: true
});
// enable validation
ko.validation.init({ grouping: { deep: true, observable: true } });

var baseApiUri = '@ViewBag.ApiGroupUrl';

function TradeLine(trade_line_id, position_id, tradable_thing_id) {
    var self = this;
    trade_line_id = typeof (trade_line_id) !== 'undefined' ? trade_line_id : 0;
    this.trade_line_id = ko.observable(trade_line_id);

    position_id = typeof (position_id) !== 'undefined' ? position_id : "";
    this.position_id = ko.validatedObservable(position_id).extend({ required: true});

    tradable_thing_id = typeof (tradable_thing_id) !== 'undefined' ? tradable_thing_id : "";
    this.tradable_thing_id = ko.validatedObservable(tradable_thing_id).extend({ required: true });

    this.positionString = ko.observable("");
    this.tradableThingString = ko.observable("");

    this.CRUDMode = "";

    //to get desc of selected position
    self.position_id.subscribe(
        function (newValue) {
            if (newValue > 0) {
                $.ajax({
                    type: 'POST',
                    url: baseUrl + "Trade/GetPosition",
                    dataType: "json",
                    crossDomain: true,
                    data: {
                        id: newValue, //id of position
                    },
                    success: function (data) {
                        var resultData = data[0];
                        //console.log(resultData.position_label);
                        self.positionString(resultData.position_label);
                    }
                });

            }
            else {
                //console.log("HERE 2");
                return;
            }
        });

    //to get desc of selected financial instrument
    self.tradable_thing_id.subscribe( 
        function (newValue) 
        {
            if (newValue > 0) 
            {
                    $.ajax({
                        type: 'POST',
                        url: baseUrl + "Trade/GetTradableThing",
                        dataType: "json",
                        crossDomain: true,
                        data: {
                            id: newValue, //id of Financial Instrument
                        },
                        success: function (data) {
                            var resultData = data[0];
                            //console.log(resultData.tradable_thing_label);
                            self.tradableThingString(resultData.tradable_thing_label);
                        }
                    });
                 
            }
            else
            {
                //console.log("HERE 2");
                return;
            }
        });

    this.canonicalLabelPart = ko.computed(function () {
        self.position_id.valueHasMutated();
        self.tradable_thing_id.valueHasMutated();
        return self.positionString() + ", " + self.tradableThingString();
    });

   
}

function TradeGroup(trade_line_group_id, trade_line_group_type_id, trade_line_group_label, trade_line_group_editorial_label, tradeLines) {
    var self = this;

    trade_line_group_id = typeof (trade_line_group_id) !== 'undefined' ? trade_line_group_id : 0;
    this.trade_line_group_id = ko.observable(trade_line_group_id);

    trade_line_group_type_id = typeof (trade_line_group_type_id) !== 'undefined' ? trade_line_group_type_id : 0;
    this.trade_line_group_type_id = ko.observable(trade_line_group_type_id).extend({
        required: true
    });
    
    this.trade_line_group_type_string = ko.observable();

    trade_line_group_label = typeof (trade_line_group_label) !== 'undefined' ? trade_line_group_label : "";
    this.trade_line_group_label = ko.observable(trade_line_group_label);

    trade_line_group_editorial_label = typeof (trade_line_group_editorial_label) !== 'undefined' ? trade_line_group_editorial_label : "";
    this.trade_line_group_editorial_label = ko.observable(trade_line_group_editorial_label);

    tradeLines = typeof (tradeLines) !== 'undefined' ? tradeLines : [];
    this.tradeLines = ko.observableArray(tradeLines);

    this.removedTradeLines = [];

    this.CRUDMode = "";

    this.addLine = function () {
        var nextIndex = this.tradeLines().length;
        var newTradeLine = new TradeLine(nextIndex);
        newTradeLine.CRUDMode = "add"
        self.tradeLines.push(newTradeLine);
    };

    this.addParsedLine = function (trade_line_id, position_id, tradable_thing_id) {
        self.tradeLines.push(new TradeLine(trade_line_id, position_id, tradable_thing_id));
    };

    this.removeLine = function (item) {
        if (self.tradeLines().length > 1)
        {
            item.CRUDMode = "delete";
            self.removedTradeLines.push(item);
            self.tradeLines.remove(item);
        }
    };
    
    //canonical label
    trade_line_group_label = typeof (trade_line_group_label) !== 'undefined' ? trade_line_group_label : "";
    this.trade_line_group_label = ko.computed(function () {
        var tradeLineParts = "";
        for (var i = 0; i < self.tradeLines().length; i++) {
            tradeLineParts += self.tradeLines()[i].canonicalLabelPart();
            if (i + 1 != self.tradeLines().length) {
                tradeLineParts += ", ";
            }
        }
        return self.trade_line_group_type_string() + ": " + tradeLineParts;
    });
}


function TradeViewModel(
  trade_id,
  service_id,
  length_type_id,
  relativity_id,
  benchmark_id,
  last_updated,
  trade_label,
  trade_editorial_label,
  structure_type_id,
  trade_instruction_id,
  instruction_entry,
  instruction_entry_date,
  instruction_exit,
  instruction_exit_date,
  instruction_type_id,
  instruction_label,
  hedge_id,
  currency_id,
  related_trade_ids,
  apl_func,
  mark_track_record_id,
  mark_to_mark_rate,
  int_track_record_id,
  interest_rate_diff,
  abs_track_performance_id,
  abs_measure_type_id,
  abs_currency_id,
  abs_return_value,
  rel_track_performance_id,
  rel_measure_type_id,
  rel_currency_id,
  rel_return_value,
  return_benchmark_id,
  comment_id,
  comments,
  status
  ) {
    var self = this;

    trade_id = typeof (id) !== 'undefined' ? trade_id : 0;
    this.trade_id = ko.observable(trade_id);

    //form values - top
    service_id = typeof (serviceId) !== 'undefined' ? service_id : 0;
    this.service_id = ko.observable(service_id).extend({ required: true });

    length_type_id = typeof (length_type_id) !== 'undefined' ? length_type_id : 2; //default value
    this.length_type_id = ko.observable(length_type_id).extend({ required: true });


    relativity_id = typeof (relativity_id) !== 'undefined' ? relativity_id : 2; //default value
    this.relativity_id = ko.observable(relativity_id).extend({ required: true });

    benchmark_id = typeof (benchmark_id) !== 'undefined' ? benchmark_id : ""; 
    this.benchmark_id = ko.observable(benchmark_id).extend({
        required: {
            onlyIf: function () {
                return self.relativity_id() == 2;
            },
        }, 
    });
    
    last_updated = typeof (last_updated) !== 'undefined' ? last_updated : "";
    this.last_updated = ko.observable(last_updated);

    trade_editorial_label = typeof (trade_editorial_label) !== 'undefined' ? trade_editorial_label : "";
    this.trade_editorial_label = ko.observable(trade_editorial_label);

    structure_type_id = typeof (structure_type_id) !== 'undefined' ? structure_type_id : 0; //default value
    this.structure_type_id = ko.observable(structure_type_id).extend({ required: true });

    //form values - bottom
    trade_instruction_id = typeof (trade_instruction_id) !== 'undefined' ? trade_instruction_id : "";
    this.trade_instruction_id = ko.observable(trade_instruction_id);

    instruction_entry = typeof (instruction_entry) !== 'undefined' ? instruction_entry : "";
    this.instruction_entry = ko.validatedObservable(null).extend({ required: true }).extend({ number: true });

    instruction_entry_date = typeof (instruction_entry_date) !== 'undefined' ? instruction_entry_date : "";
    this.instruction_entry_date = ko.validatedObservable(instruction_entry_date).extend({ required: true, dateISO: true });

    instruction_exit = typeof (instruction_exit) !== 'undefined' ? instruction_exit : "";
    this.instruction_exit = ko.observable(instruction_exit).extend({ number: true });
 
    instruction_exit_date = typeof (instruction_exit_date) !== 'undefined' ? instruction_exit_date : "";
    this.instruction_exit_date = ko.observable(instruction_exit_date);

    instruction_type_id = typeof (instruction_type_id) !== 'undefined' ? instruction_type_id : 0;
    this.instruction_type_id = ko.observable(instruction_type_id);
    
    instruction_label = typeof (instruction_label) !== 'undefined' ? instruction_label : "";
    this.instruction_label = ko.observable(instruction_label);

    hedge_id = typeof (hedge_id) !== 'undefined' ? hedge_id : 2; //default value
    this.hedge_id = ko.observable(2);

    currency_id = typeof (currency_id) !== 'undefined' ? currency_id : "";
    this.currency_id = ko.observable(currency_id);

    related_trade_ids_list = typeof (related_trade_ids_list) !== 'undefined' ? related_trade_ids_list : "";
    this.related_trade_ids_list = ko.observable(related_trade_ids_list);
    //create a separate observableArray and use a subscription to keep it updated
    related_trade_ids = typeof (related_trade_ids) !== 'undefined' ? related_trade_ids : [];
    this.related_trade_ids = ko.observableArray(related_trade_ids);

    this.related_trade_ids_list.subscribe(function (newValue) {
        this.related_trade_ids(newValue.split(","));
    }, this);

    /*
    //compute the split values from the original observable
    this.computedSplitValues = ko.computed(function() {
        return this.value().split(",");
    }, this);
    
    this.computedJoinedValues = ko.computed(function() {
        return this.splitValues().join(",");
    }, this);
    */

    apl_func = typeof (apl_func) !== 'undefined' ? apl_func : "";
    this.apl_func = ko.observable(apl_func);

    mark_track_record_id = typeof (mark_track_record_id) !== 'undefined' ? mark_track_record_id : "";
    this.mark_track_record_id = ko.observable(mark_track_record_id);

    mark_to_mark_rate = typeof (mark_to_mark_rate) !== 'undefined' ? mark_to_mark_rate : "";
    this.mark_to_mark_rate = ko.observable(mark_to_mark_rate).extend({ number: true });

    int_track_record_id = typeof (int_track_record_id) !== 'undefined' ? int_track_record_id : "";
    this.int_track_record_id = ko.observable(int_track_record_id);

    interest_rate_diff = typeof (interest_rate_diff) !== 'undefined' ? interest_rate_diff : "";
    this.interest_rate_diff = ko.observable(interest_rate_diff).extend({ number: true });

    abs_track_performance_id = typeof (abs_track_performance_id) !== 'undefined' ? abs_track_performance_id : "";
    this.abs_track_performance_id = ko.observable(abs_track_performance_id);

    abs_measure_type_id = typeof (abs_measure_type_id) !== 'undefined' ? abs_measure_type_id : 1; //default value
    this.abs_measure_type_id = ko.observable(1);

    abs_currency_id = typeof (abs_currency_id) !== 'undefined' ? abs_currency_id : ""; //default value
    this.abs_currency_id = ko.observable(abs_currency_id);

    abs_return_value = typeof (abs_return_value) !== 'undefined' ? abs_return_value : "";
    this.abs_return_value = ko.observable(abs_return_value).extend({ number: true });

    rel_track_performance_id = typeof (rel_track_performance_id) !== 'undefined' ? rel_track_performance_id : "";
    this.rel_track_performance_id = ko.observable(rel_track_performance_id);

    rel_measure_type_id = typeof (rel_measure_type_id) !== 'undefined' ? rel_measure_type_id : 2; //default value
    this.rel_measure_type_id = ko.observable(2); 

    rel_currency_id = typeof (rel_currency_id) !== 'undefined' ? rel_currency_id : "";
    this.rel_currency_id = ko.observable(rel_currency_id);

    rel_return_value = typeof (rel_return_value) !== 'undefined' ? rel_return_value : "";
    this.rel_return_value = ko.observable(rel_return_value).extend({ number: true });

    return_benchmark_id = typeof (return_benchmark_id) !== 'undefined' ? return_benchmark_id : "";
    this.return_benchmark_id = ko.observable(return_benchmark_id).extend({
        required: {
            onlyIf: function () {
                return (self.rel_return_value() !== "" && self.rel_return_value() !== null && self.rel_return_value() !== 'undefined');
            },
        },
    });

    comment_id = typeof (comment_id) !== 'undefined' ? comment_id : "";
    this.comment_id = ko.observable(comment_id);

    comments = typeof (comments) !== 'undefined' ? comments : "";
    this.comments = ko.observable(comments).extend({ maxLength: 255 });

    status = typeof (status) !== 'undefined' ? status : "";
    this.status = ko.observable(status).extend({ required: true });
    
    //tradegroups
    this.tradegroups = ko.observableArray([]);

    this.addGroup = function () {
        var nextIndex = this.tradegroups().length;
        var newGroup = new TradeGroup(nextIndex, "", "", "", [new TradeLine()]);
        newGroup.CRUDMode = "add";
        self.tradegroups.push(newGroup);
    };

    this.removeGroup = function (item) {
        if (self.tradegroups().length > 1)
        {
            self.tradegroups.remove(item);
        }  
    };
    //end tradegroups
    
    //canonical label
    trade_label = typeof (trade_label) !== 'undefined' ? trade_label : "";
    this.trade_label = ko.computed(function () {
        var tradeGroupParts = "";
        for (var i = 0; i < self.tradegroups().length; i++) {
            tradeGroupParts += self.tradegroups()[i].trade_line_group_label();
            if (i + 1 != self.tradegroups().length) {
                tradeGroupParts += " - ";
            }
        }
        return tradeGroupParts;
    });

    this.vmMessages = ko.observable(""); //used for bootstrap UI alerts

    self.saveTradeData = function (baseApiUrl) {
       
        if (this.isValid()) {
            console.log('Posting Trade to server to save.');
            self.vmMessages("Posting Trade to server to save.");
            var apiURL = baseUrl;
            apiURL += "api/values/post";
            $.ajax({
                url: apiURL,
                type: 'post',
                data: JSON.stringify(ko.toJSON(this)),
                contentType: 'application/json',
                timeout: 5000,
                success: function (data) {
                    if (data.Success) {
                        window.onbeforeunload = null;
                        console.log(data.Message);
                        self.vmMessages(data.Message); //display success
                        //bootbox.alert(self.vmMessages());
                        var message = self.vmMessages();
                        bootbox.dialog({
                            message: message,
                            title: "Trade Edit",
                            buttons: {
                                success: {
                                    label: "OK",
                                    className: "btn-success",
                                    callback: function () {
                                        return true;
                                        //document.location.href = $('#createUrl').attr('href');
                                    }
                                },
                                main: {
                                    label: "Exit",
                                    className: "btn-primary",
                                    callback: function () {
                                        document.location.href = $('#cancelUrl').attr('href');
                                    }
                                }
                            }
                        });
                    }
                    else {
                        console.log(data.Message);
                        self.vmMessages(data.Message); //display exception
                        bootbox.alert(self.vmMessages());
                       
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log("error: " + XMLHttpRequest.responseText);
                    self.vmMessages("error: " + XMLHttpRequest.responseText);
                    var message = self.vmMessages();
                    bootbox.dialog({
                        message: message,
                        title: "Trade Edit",
                        className: "alert-danger",
                        buttons: {
                            //success: {
                            //    label: "Create",
                            //    className: "btn-success",
                            //    callback: function () {
                            //        //Example.show("great success");
                            //    }
                            //},
                            danger: {
                                label: "OK",
                                className: "btn-danger",
                                callback: function () {
                                    //Example.show("uh oh, look out!");
                                }
                            }
                            //main: {
                            //    label: "Exit",
                            //    className: "btn-primary",
                            //    callback: function () {
                            //        //Example.show("Primary button");
                            //    }
                            //}
                        }
                    });
                }
            });
        }
        else
        {
            console.log('Validation errors found, please check form.');
            self.vmMessages("Validation errors found, please check form.");
            //bootbox.alert(self.vmMessages());
            var message = self.vmMessages();
            bootbox.dialog({
                message: message,
                title: "Trade Edit",
                className: "alert-danger",
                buttons: {
                    //success: {
                    //    label: "Create",
                    //    className: "btn-success",
                    //    callback: function () {
                    //        //Example.show("great success");
                    //    }
                    //},
                    danger: {
                        label: "OK",
                        className: "btn-danger",
                        callback: function () {
                            //Example.show("uh oh, look out!");
                        }
                    }
                    //main: {
                    //    label: "Exit",
                    //    className: "btn-primary",
                    //    callback: function () {
                    //        //Example.show("Primary button");
                    //    }
                    //}
                }
            });
        }
    };
    
    self.isValid = ko.computed(function () {
        return ko.validation.group(
            self,
            {
                observable: true,
                deep: true
            }).showAllMessages(true);
    }, self);

    self.valueHasMutated = function () {
        console.log('MUTATION!!!');
        for (var i = 0; i < self.tradegroups().length; i++)
        {
            var curgroup = self.tradegroups()[i];
            for (var j = 0; j > curgroup.tradeLines().length; j++) {
                console.log('MUTATION!!!' + i + ',' + j);
                curgroup.tradeLines()[j].position_id().valueHasMutated();
                curgroup.tradeLines()[j].tradable_thing_id().valueHasMutated();
            }
        }
    };

    this.instructionDateCheck = ko.computed(function () {
        if (self.instruction_exit_date() == "")
        {
            self.instruction_exit_date.__valid__(true);
            return true;
        }

        if (self.instruction_entry_date.isValid() && self.instruction_exit_date() != "" && self.instruction_exit_date() != null)
        {
            var startDate = moment(self.instruction_entry_date());
            var endDate = moment(self.instruction_exit_date());
            self.instruction_exit_date.__valid__(moment(startDate).isBefore(endDate));
            return moment(startDate).isBefore(endDate);
        }

        self.instruction_exit_date.__valid__(true);
        return true;

    }, self);

    //Start Comments save//////////////////////////
    self.saveCommentData = function () {
        console.log('Posting Comments to server to save.');
        self.vmMessages("Posting Comments to server to save.");
        var apiURL = baseUrl;
        apiURL += "api/comments/post";
        $.ajax({
            url: apiURL,
            type: 'post',
            data: JSON.stringify(ko.toJSON(this)),
            contentType: 'application/json',
            timeout: 5000,
            success: function (data) {
                if (data.Success) {
                    //update comment id
                    self.comment_id(data.result);
                    window.onbeforeunload = null;
                    console.log(data.Message);
                    self.vmMessages(data.Message); //display success
                    var message = self.vmMessages();
                    bootbox.dialog({
                        message: message,
                        title: "Trade Comment",
                        buttons: {
                            success: {
                                label: "OK",
                                className: "btn-success",
                                callback: function () {
                                    return true;
                                }
                            },
                            main: {
                                label: "Exit",
                                className: "btn-primary",
                                callback: function () {
                                    document.location.href = $('#cancelUrl').attr('href');
                                }
                            }
                        }
                    });
                }
                else {
                    console.log(data.Message);
                    self.vmMessages(data.Message); //display exception
                    bootbox.alert(self.vmMessages());

                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log("error: " + XMLHttpRequest.responseText);
                self.vmMessages("error: " + XMLHttpRequest.responseText);
                var message = self.vmMessages();
                bootbox.dialog({
                    message: message,
                    title: "Trade Comment",
                    className: "alert-danger",
                    buttons: {
                        danger: {
                            label: "OK",
                            className: "btn-danger",
                            callback: function () {
                            }
                        }
                    }
                });
            }
        });
    }
    this.CRUDMode = "";
    //set this to true to see ko.toJson on form for debugging knockout bindings
    this.APItester = new ResultViewModel();
    this.debug = true;
    //////////////////
    


    //Test api calls section
    function ResultViewModel() {
        var self = this;

        this.trade_id = ko.observable("");
        this.endpoint = ko.observable("");
        this.tradeGraph = ko.observable("");
        this.results = ko.observable("");

        //to get graph of trade for isis plato
        self.trade_id.subscribe(
            function (newValue) {
                if (newValue > 0) {
                    var apiURL = baseUrl;
                    apiURL += "api/tradesplato/get/";
                    $.ajax({
                        type: 'GET',
                        url: apiURL,
                        dataType: "json",
                        crossDomain: true,
                        data: {
                            id: newValue, //id of Financial Instrument
                        },
                        success: function (data) {
                            var resultData = data;
                            console.log(resultData);
                            self.tradeGraph(resultData);
                        }
                    });

                }
                else {
                    //console.log("HERE 2");
                    return;
                }
            });



           self.postTradeData = function () {

            console.log('Posting Trade to server to save.');
            var apiURL = baseUrl;
            apiURL += self.endpoint();
            var postdata = self.tradeGraph();
            $.ajax({
                type: 'POST',
                url: apiURL,
                dataType: "json",
                crossDomain: true,
                data: {
                    postdata: postdata, 
                },
                success: function (data) {
                    console.log("RESULT: " + data);
                    self.result("RESULT: " + data);
                }
            });
        };

    }

}

var vm = new TradeViewModel();
ko.applyBindings(ko.validatedObservable(vm));

//showModal function
// Showing a modal is an asynchronous operation.
// A jQuery Deferred object is returned to allow the calling code to
// attach a callback for when the modal has been closed.

var showModal = function (options) {
    if (typeof options === "undefined") throw new Error("An options argument is required.");
    if (typeof options.viewModel !== "object") throw new Error("options.viewModel is required.");

    var viewModel = options.viewModel;
    var template = options.template || viewModel.template;
    var context = options.context;

    if (!template) throw new Error("options.template or options.viewModel.template is required.");

    return createModalElement(template, viewModel)
        .pipe($) // jQueryify the DOM element
        .pipe(function ($ui) {
            var deferredModalResult = $.Deferred();
            addModalHelperToViewModel(viewModel, deferredModalResult, context);
            showTwitterBootstrapModal($ui);
            whenModalResultCompleteThenHideUI(deferredModalResult, $ui);
            whenUIHiddenThenRemoveUI($ui);
            return deferredModalResult;
        });
};

var createModalElement = function (templateName, viewModel) {
    var temporaryDiv = addHiddenDivToBody();
    var deferredElement = $.Deferred();
    ko.renderTemplate(
        templateName,
        viewModel,
        // We need to know when the template has been rendered,
        // so we can get the resulting DOM element.
        // The resolve function receives the element.
        {
            afterRender: function (nodes) {
                // Ignore any #text nodes before and after the modal element.
                var elements = nodes.filter(function (node) {
                    return node.nodeType === 1; // Element
                });
                deferredElement.resolve(elements[0]);
            }
        },
        // The temporary div will get replaced by the rendered template output.
        temporaryDiv,
        "replaceNode"
    );
    // Return the deferred DOM element so callers can wait until it's ready for use.
    return deferredElement;
};

var addHiddenDivToBody = function () {
    var div = document.createElement("div");
    div.style.display = "none";
    document.body.appendChild(div);
    return div;
};

var addModalHelperToViewModel = function (viewModel, deferredModalResult, context) {
    // Provide a way for the viewModel to close the modal and pass back a result.
    viewModel.modal = {
        close: function (result) {
            if (typeof result !== "undefined") {
                deferredModalResult.resolveWith(context, [result]);
            } else {
                // When result is undefined, we don't want any `done` callbacks of
                // the deferred being called. So reject instead of resolve.
                deferredModalResult.rejectWith(context, []);
            }
        }
    };
};

var showTwitterBootstrapModal = function ($ui) {
    // Display the modal UI using Twitter Bootstrap's modal plug-in.
    $ui.modal({
        // Clicking the backdrop, or pressing Escape, shouldn't automatically close the modal by default.
        // The view model should remain in control of when to close.
        backdrop: "static",
        keyboard: false
    });
};

var whenModalResultCompleteThenHideUI = function (deferredModalResult, $ui) {
    // When modal is closed (with or without a result)
    // Then always hide the UI.
    deferredModalResult.always(function () {
        $ui.modal("hide");
    });
};

var whenUIHiddenThenRemoveUI = function ($ui) {
    // Hiding the modal can result in an animation.
    // The `hidden` event is raised after the animation finishes,
    // so this is the right time to remove the UI element.
    $ui.on("hidden", function () {
        // Call ko.cleanNode before removal to prevent memory leaks.
        $ui.each(function (index, element) {
            ko.cleanNode(element);
        });
        $ui.remove();
    });
};
///End show Modal


//Start Comments section//////////////////////////
//Add Comment modal 
var AddCommentViewModel = function () {
    this.text = ko.observable().extend({ maxLength: 255 });
}

// The name of the template to render
AddCommentViewModel.prototype.template = "AddComment";

AddCommentViewModel.prototype.add = function () {
    var newComment = {
        text: this.text(),
    };
    // Close the modal, passing the new note object as the result data.
    this.modal.close(newComment);
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
    this.text = ko.observable().extend({ maxLength: 255 });
}

// The name of the template to render
EditCommentViewModel.prototype.template = "EditComment";

EditCommentViewModel.prototype.save = function () {
    var editedComment = {
        id: this.id,
        text: this.text(),
    };
    // Close the modal, passing the new note object as the result data.
    this.modal.close(editedComment);
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


    