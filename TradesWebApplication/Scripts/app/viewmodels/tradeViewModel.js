

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

    position_id = typeof (position_id) !== 'undefined' ? position_id : 0;
    this.position_id = ko.validatedObservable(position_id).extend({ required: true});

    tradable_thing_id = typeof (tradable_thing_id) !== 'undefined' ? tradable_thing_id : 0;
    this.tradable_thing_id = ko.validatedObservable(tradable_thing_id).extend({ required: true });

    this.positionString = ko.observable("");
    this.tradableThingString = ko.observable(null).extend({ required: true });;

    this.canonicalLabelPart = ko.computed(function () {
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

    this.addLine = function () {
        var nextIndex = this.tradeLines().length;
        self.tradeLines.push(new TradeLine(nextIndex));
        $("select.tradableThingList").select2({ placeholder: "{Financial Instrument}", width: '200px' });
    };

    this.removeLine = function (item) {
        self.tradeLines.remove(item);
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
  created_on,
  trade_label,
  trade_editorial_label,
  structure_type_id,
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
  mark_to_mark_rate,
  interest_rate_diff,
  abs_measure_type_id,
  abs_currency_id,
  abs_return_value,
  rel_measure_type_id,
  rel_currency_id,
  rel_return_value,
  return_benchmark_id,
  comments
  ) {
    var self = this;

    trade_id = typeof (id) !== 'undefined' ? trade_id : 0;
    this.trade_id = ko.observable(trade_id);

    //form values - top
    service_id = typeof (serviceId) !== 'undefined' ? service_id : 0;
    this.service_id = ko.observable(service_id).extend({ required: true });

    length_type_id = typeof (length_type_id) !== 'undefined' ? length_type_id : 2; //default value
    this.length_type_id = ko.observable(2).extend({ required: true });


    relativity_id = typeof (relativity_id) !== 'undefined' ? relativity_id : 2; //default value
    this.relativity_id = ko.observable(2).extend({ required: true });

    benchmark_id = typeof (benchmark_id) !== 'undefined' ? benchmark_id : ""; 
    this.benchmark_id = ko.observable(null).extend({
        required: {
            onlyIf: function () {
                return self.relativity_id() == 2;
            },
        }, 
    });
    
    created_on = typeof (created_on) !== 'undefined' ? created_on : "";
    this.created_on = ko.observable(created_on);

    trade_editorial_label = typeof (trade_editorial_label) !== 'undefined' ? trade_editorial_label : "";
    this.trade_editorial_label = ko.observable(trade_editorial_label);

    structure_type_id = typeof (structure_type_id) !== 'undefined' ? structure_type_id : 4; //default value
    this.structure_type_id = ko.observable(4).extend({ required: true });

    //form values - bottom
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

    mark_to_mark_rate = typeof (mark_to_mark_rate) !== 'undefined' ? mark_to_mark_rate : "";
    this.mark_to_mark_rate = ko.observable(mark_to_mark_rate).extend({ number: true });

    interest_rate_diff = typeof (interest_rate_diff) !== 'undefined' ? interest_rate_diff : "";
    this.interest_rate_diff = ko.observable(interest_rate_diff).extend({ number: true });

    abs_measure_type_id = typeof (abs_measure_type_id) !== 'undefined' ? abs_measure_type_id : 1; //default value
    this.abs_measure_type_id = ko.observable(1);

    abs_currency_id = typeof (abs_currency_id) !== 'undefined' ? abs_currency_id : 1; //default value
    this.abs_currency_id = ko.observable(1);

    abs_return_value = typeof (abs_return_value) !== 'undefined' ? abs_return_value : "";
    this.abs_return_value = ko.observable(abs_return_value).extend({ number: true });

    rel_measure_type_id = typeof (rel_measure_type_id) !== 'undefined' ? rel_measure_type_id : 2; //default value
    this.rel_measure_type_id = ko.observable(2); 

    rel_currency_id = typeof (rel_currency_id) !== 'undefined' ? rel_currency_id : 0;
    this.rel_currency_id = ko.observable(rel_currency_id);

    rel_return_value = typeof (rel_return_value) !== 'undefined' ? rel_return_value : "";
    this.rel_return_value = ko.observable(rel_return_value).extend({ number: true });

    return_benchmark_id = typeof (return_benchmark_id) !== 'undefined' ? return_benchmark_id : 0;
    this.return_benchmark_id = ko.observable(0); //Hack to avoid null in json

    comments = typeof (comments) !== 'undefined' ? comments : "";
    this.comments = ko.observable(comments);
    
    //tradegroups
    this.tradegroups = ko.observableArray([]);

    this.addGroup = function () {
        var nextIndex = this.tradegroups().length;
        var newGroup = new TradeGroup(nextIndex, "", "", "", [new TradeLine()]);
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
                        console.log(data.Message);
                        self.vmMessages(data.Message); //display success
                        //bootbox.alert(self.vmMessages());
                        var message = self.vmMessages() + ". Create another?";
                        bootbox.dialog({
                            message: message,
                            title: "Trade Creation",
                            buttons: {
                                success: {
                                    label: "Create",
                                    className: "btn-success",
                                    callback: function () {
                                        document.location.href = $('#createUrl').attr('href');
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
                        title: "Trade Creation",
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
                            },
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
                title: "Trade Creation",
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
                    },
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

    this.instructionDateCheck = ko.computed(function () {
        if (self.instruction_exit_date() == "")
        {
            self.instruction_exit_date.__valid__(true);
            return true;
        }

        if (self.instruction_entry_date.isValid() && self.instruction_exit_date() != "")
        {
            var startDate = moment(self.instruction_entry_date());
            var endDate = moment(self.instruction_exit_date());
            self.instruction_exit_date.__valid__(moment(startDate).isBefore(endDate));
            return moment(startDate).isBefore(endDate);
        }

        self.instruction_exit_date.__valid__(true);
        return true;

    }, self);

    //set this to true to see ko.toJson on form for debugging knockout bindings
    this.debug = false;
    ////////////////

}

var vm = new TradeViewModel();

vm.tradegroups.push(new TradeGroup(0, 0, "", "", [new TradeLine()]));

ko.applyBindings(ko.validatedObservable(vm));
    