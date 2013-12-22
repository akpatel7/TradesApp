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


var baseApiUri = '@ViewBag.ApiGroupUrl';

function TradeLine(trade_line_id, position_id, tradable_thing_id) {
    var self = this;
    trade_line_id = typeof (trade_line_id) !== 'undefined' ? trade_line_id : 0;
    this.trade_line_id = ko.observable(trade_line_id);

    position_id = typeof (position_id) !== 'undefined' ? position_id : 0;
    this.position_id = ko.observable(position_id).extend({ required: true});

    tradable_thing_id = typeof (tradable_thing_id) !== 'undefined' ? tradable_thing_id : 0;
    this.tradable_thing_id = ko.observable(tradable_thing_id).extend({ required: true });

    this.positionString = ko.observable("");
    this.tradableThingString = ko.observable("");

    this.canonicalLabelPart = ko.computed(function () {
        return self.positionString() + ", " + self.tradableThingString();
    });

}

function TradeGroup(trade_line_group_id, trade_line_group_type_id, trade_line_group_label, trade_line_group_editorial_label, tradeLines) {
    var self = this;

    trade_line_group_id = typeof (trade_line_group_id) !== 'undefined' ? trade_line_group_id : 0;
    this.trade_line_group_id = ko.observable(trade_line_group_id);

    trade_line_group_type_id = typeof (trade_line_group_type_id) !== 'undefined' ? trade_line_group_type_id : 0;
    this.trade_line_group_type_id = ko.observable(trade_line_group_type_id);
    
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
    this.service_id = ko.observable(service_id);

    length_type_id = typeof (length_type_id) !== 'undefined' ? length_type_id : 2; //default value
    this.length_type_id = ko.observable(2);
    relativity_id

    relativity_id = typeof (relativity_id) !== 'undefined' ? relativity_id : 2; //default value
    this.relativity_id = ko.observable(2);

    benchmark_id = typeof (benchmark_id) !== 'undefined' ? benchmark_id : 0;
    this.benchmark_id = ko.observable(benchmark_id).extend({
        required: {
            onlyIf: function () {
                return self.relativity_id() == 2;
            }
        }
    });;

    created_on = typeof (created_on) !== 'undefined' ? created_on : "";
    this.created_on = ko.observable(created_on);



    trade_editorial_label = typeof (trade_editorial_label) !== 'undefined' ? trade_editorial_label : "";
    this.trade_editorial_label = ko.observable(trade_editorial_label);

    structure_type_id = typeof (structure_type_id) !== 'undefined' ? structure_type_id : 4; //default value
    this.structure_type_id = ko.observable(4);


    //tradegroups
    this.tradegroups = ko.observableArray([]);

    this.addGroup = function () {
        var nextIndex = this.tradegroups().length;
        var newGroup = new TradeGroup(nextIndex,1,"","",[new TradeLine()]);
        self.tradegroups.push(newGroup);
    };

    this.removeGroup = function (item) {
        self.tradegroups.remove(item);
    };
    //end tradegroups

    //form values - bottom
    instruction_entry = typeof (instruction_entry) !== 'undefined' ? instruction_entry : "";
    this.instruction_entry = ko.observable(instruction_entry);

    instruction_entry_date = typeof (instruction_entry_date) !== 'undefined' ? instruction_entry_date : "";
    this.instruction_entry_date = ko.observable(instruction_entry_date);

    instruction_exit = typeof (instruction_exit) !== 'undefined' ? instruction_exit : "";
    this.instruction_exit = ko.observable(instruction_exit);

    instruction_exit_date = typeof (instruction_exit_date) !== 'undefined' ? instruction_exit_date : "";
    this.instruction_exit_date = ko.observable(instruction_exit_date);

    instruction_type_id = typeof (instruction_type_id) !== 'undefined' ? instruction_type_id : 0;
    this.instruction_type_id = ko.observable(instruction_type_id);
    
    instruction_label = typeof (instruction_label) !== 'undefined' ? instruction_label : "";
    this.instruction_label = ko.observable(instruction_label);

    hedge_id = typeof (hedge_id) !== 'undefined' ? hedge_id : 2; //default value
    this.hedge_id = ko.observable(2);

    currency_id = typeof (currency_id) !== 'undefined' ? currency_id : 0;
    this.currency_id = ko.observable(currency_id);

    related_trade_ids = typeof (related_trade_ids) !== 'undefined' ? related_trade_ids : [];
    this.related_trade_ids = ko.observableArray(related_trade_ids);

    apl_func = typeof (apl_func) !== 'undefined' ? apl_func : "";
    this.apl_func = ko.observable(apl_func);

    mark_to_mark_rate = typeof (mark_to_mark_rate) !== 'undefined' ? mark_to_mark_rate : "";
    this.mark_to_mark_rate = ko.observable(mark_to_mark_rate);

    interest_rate_diff = typeof (interest_rate_diff) !== 'undefined' ? interest_rate_diff : "";
    this.interest_rate_diff = ko.observable(interest_rate_diff);

    abs_measure_type_id = typeof (abs_measure_type_id) !== 'undefined' ? abs_measure_type_id : 1; //default value
    this.abs_measure_type_id = ko.observable(1);

    abs_currency_id = typeof(abs_currency_id) !== 'undefined' ? abs_currency_id : 1;
    this.abs_currency_id = ko.observable(abs_currency_id);

    abs_return_value = typeof (abs_return_value) !== 'undefined' ? abs_return_value : "";
    this.abs_return_value = ko.observable(abs_return_value);

    rel_measure_type_id = typeof (rel_measure_type_id) !== 'undefined' ? rel_measure_type_id : 2; //default value
    this.rel_measure_type_id = ko.observable(2); 

    rel_currency_id = typeof (rel_currency_id) !== 'undefined' ? rel_currency_id : 0;
    this.rel_currency_id = ko.observable(rel_currency_id);

    rel_return_value = typeof (rel_return_value) !== 'undefined' ? rel_return_value : "";
    this.rel_return_value = ko.observable(rel_return_value);

    return_benchmark_id = typeof (return_benchmark_id) !== 'undefined' ? return_benchmark_id : 0;
    this.return_benchmark_id = ko.observable(return_benchmark_id);

    comments = typeof (comments) !== 'undefined' ? comments : "";
    this.comments = ko.observable(comments);

    self.saveTradeData = function(baseApiUrl) {
        var apiURL = baseUrl;
        apiURL += "api/values/post";
        $.ajax({
            url: apiURL,
            type: 'post',
            data: JSON.stringify(ko.toJSON(this)),
            contentType: 'application/json',
            success: function(result) {

                $('#message').html(result);
            }
        });
    };

}

var vm = new TradeViewModel();
    vm.tradegroups.push(new TradeGroup(0, 0, "", "", [new TradeLine()]));

ko.applyBindings(vm);
