

function TradeLine(id, positionId, financialInstrumentId) {
    var self = this;
    id = typeof (id) !== 'undefined' ? id : 0;
    this.id = ko.observable(id);

    positionId = typeof (positionId) !== 'undefined' ? positionId : 0;
    this.positionId = ko.observable(positionId);

    financialInstrumentId = typeof (financialInstrumentId) !== 'undefined' ? financialInstrumentId : 0;
    this.financialInstrumentId = ko.observable(financialInstrumentId);

}

function TradeGroup(id, groupStructureId, groupCanonicalLabel, groupEditorialLabel, tradeLines) {
    var self = this;

    id = typeof (id) !== 'undefined' ? id : 0;
    this.id = ko.observable(id);

    groupStructureId = typeof (groupStructureId) !== 'undefined' ? groupStructureId : 0;
    this.groupStructureId = ko.observable(groupStructureId);

    groupCanonicalLabel = typeof (groupCanonicalLabel) !== 'undefined' ? groupCanonicalLabel : "";
    this.groupCanonicalLabel = ko.observable(groupCanonicalLabel);

    groupEditorialLabel = typeof (groupEditorialLabel) !== 'undefined' ? groupEditorialLabel : "";
    this.groupEditorialLabel = ko.observable(groupEditorialLabel);

    tradeLines = typeof (tradeLines) !== 'undefined' ? tradeLines : [];
    this.tradeLines = ko.observableArray(tradeLines);

    this.addLine = function (id) {
        self.tradeLines.push(new TradeLine(id));
    };

    this.removeLine = function (item) {
        self.tradeLines.remove(item);
    };
}

function TradeViewModel(
  id,
  serviceId,
  tradeTypeId,
  benchmarkId,
  benchMarkSelectionId,
  systemUpdatedCreatedDate,
  tradeCanonicalLabel,
  tradeEditorialLabel,
  tradeStructureId,
  instructionEntry,
  instructionEntryDate,
  instructionExit,
  instructionExitDate,
  instructionTypeId,
  hedgeId,
  relatedTrades,
  aplFunc,
  mark_to_mark_rate,
  interest_rate_diff,
  abs_measure_type_id,
  abs_currency_id,
  rel_measure_type_id,
  rel_currency_id,
  return_benchmark_id,
  comments
  ) {
    var self = this;

    id = typeof (id) !== 'undefined' ? id : 0;
    this.id = ko.observable(id);

    //form values - top
    serviceId = typeof (serviceId) !== 'undefined' ? serviceId : 0;
    this.serviceId = ko.observable(serviceId);

    tradeTypeId = typeof (tradeTypeId) !== 'undefined' ? tradeTypeId : 0;
    this.tradeTypeId = ko.observable(tradeTypeId);

    benchmarkId = typeof (benchmarkId) !== 'undefined' ? benchmarkId : 0;
    this.benchmarkId = ko.observable(benchmarkId);

    benchMarkSelectionId = typeof (benchMarkSelectionId) !== 'undefined' ? benchMarkSelectionId : 0;
    this.benchMarkSelectionId = ko.observable(benchMarkSelectionId);

    systemUpdatedCreatedDate = typeof (systemUpdatedCreatedDate) !== 'undefined' ? systemUpdatedCreatedDate : "";
    this.systemUpdatedCreatedDate = ko.observable(systemUpdatedCreatedDate);


    tradeCanonicalLabel = typeof (tradeCanonicalLabel) !== 'undefined' ? tradeCanonicalLabel : "";
    this.tradeCanonicalLabel = ko.observable(tradeCanonicalLabel);

    tradeEditorialLabel = typeof (tradeEditorialLabel) !== 'undefined' ? tradeEditorialLabel : "";
    this.tradeEditorialLabel = ko.observable(tradeEditorialLabel);

    tradeStructureId = typeof (tradeStructureId) !== 'undefined' ? tradeStructureId : 0;
    this.tradeStructureId = ko.observable(tradeStructureId);

    //tradegroups
    this.tradegroups = ko.observableArray([
      new TradeGroup(),
      new TradeGroup(),
      new TradeGroup(),
    ]);

    this.addGroup = function () {
        self.tradegroups.push(new TradeGroup());
    };

    this.removeGroup = function (item) {
        self.tradegroups.remove(item);
    };
    //end tradegroups

    //form values - bottom
    instructionEntry = typeof (instructionentry) !== 'undefined' ? instructionEntry : "";
    this.instructionEntry = ko.observable(instructionEntry);

    instructionEntryDate = typeof (instructionEntryDate) !== 'undefined' ? instructionEntryDate : "";
    this.instructionEntryDate = ko.observable(instructionEntryDate);

    instructionExit = typeof (instructionExit) !== 'undefined' ? instructionExit : "";
    this.instructionExit = ko.observable(instructionExit);

    instructionExitDate = typeof (instructionExitDate) !== 'undefined' ? instructionExitDate : "";
    this.instructionExitDate = ko.observable(instructionExitDate);

    instructionTypeId = typeof (instructionTypeId) !== 'undefined' ? instructionTypeId : 0;
    this.instructionTypeId = ko.observable(instructionTypeId);

    hedgeId = typeof (hedgeId) !== 'undefined' ? hedgeId : 0;
    this.hedgeId = ko.observable(hedgeId);

    currencyId = typeof (currencyId) !== 'undefined' ? currencyId : 0;
    this.currencyId = ko.observable(currencyId);

    relatedTrades = typeof (relatedTrades) !== 'undefined' ? relatedTrades : [];
    this.relatedTrades = ko.observableArray(relatedTrades);

    aplFunc = typeof (aplFunc) !== 'undefined' ? aplFunc : "";
    this.aplFunc = ko.observable(aplFunc);

    mark_to_mark_rate = typeof (mark_to_mark_rate) !== 'undefined' ? mark_to_mark_rate : "";
    this.mark_to_mark_rate = ko.observable(mark_to_mark_rate);

    interest_rate_diff = typeof (interest_rate_diff) !== 'undefined' ? interest_rate_diff : "";
    this.interest_rate_diff = ko.observable(interest_rate_diff);

    abs_measure_type_id = typeof (abs_measure_type_id) !== 'undefined' ? abs_measure_type_id : 0;
    this.abs_measure_type_id = ko.observable(abs_measure_type_id);

    abs_currency_id = typeof (abs_currency_id) !== 'undefined' ? abs_currency_id : 0;
    this.abs_currency_id = ko.observable(abs_currency_id);

    rel_measure_type_id = typeof (rel_measure_type_id) !== 'undefined' ? rel_measure_type_id : 0;
    this.rel_measure_type_id = ko.observable(rel_measure_type_id);

    rel_currency_id = typeof (rel_currency_id) !== 'undefined' ? rel_currency_id : 0;
    this.rel_currency_id = ko.observable(rel_currency_id);

    return_benchmark_id = typeof (return_benchmark_id) !== 'undefined' ? return_benchmark_id : 0;
    this.return_benchmark_id = ko.observable(return_benchmark_id);

    comments = typeof (comments) !== 'undefined' ? comments : "";
    this.comments = ko.observable(comments);

}

var vm = new TradeViewModel();
vm.tradegroups.push(new TradeGroup(2, 2, "canon", "edit", [new TradeLine(), new TradeLine()]));

ko.applyBindings(vm);