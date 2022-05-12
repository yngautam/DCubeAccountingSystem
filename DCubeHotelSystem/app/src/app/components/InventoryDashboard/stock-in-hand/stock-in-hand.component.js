"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var global_1 = require("../../../Shared/global");
var StockInHandComponent = /** @class */ (function () {
    function StockInHandComponent(date, _menuConsumptionService) {
        this.date = date;
        this._menuConsumptionService = _menuConsumptionService;
        this.indLoading = false;
        this.currentYear = JSON.parse(localStorage.getItem('currentYear'));
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.company = JSON.parse(localStorage.getItem('company'));
    }
    StockInHandComponent.prototype.ngOnInit = function () {
        this.LoadmenuConsumption();
    };
    StockInHandComponent.prototype.LoadmenuConsumption = function () {
        var _this = this;
        this.indLoading = true;
        this._menuConsumptionService.get(global_1.Global.BASE_STOCKINHAND_ENDPOINT + "?FinancialYear=" + (this.currentYear['Name'] || ''))
            .subscribe(function (InventoryItems) {
            debugger;
            _this.ViewInventoryItems = InventoryItems;
            _this.indLoading = false;
        }, function (error) { return _this.msg = error; });
    };
    StockInHandComponent.prototype.exportTableToExcel = function (tableID, filename) {
        if (filename === void 0) { filename = ''; }
        var downloadLink;
        var dataType = 'application/vnd.ms-excel';
        var clonedtable = $('#' + tableID);
        var clonedHtml = clonedtable.clone();
        $(clonedtable).find('.export-no-display').remove();
        var tableSelect = document.getElementById(tableID);
        var tableHTML = tableSelect.outerHTML.replace(/ /g, '%20');
        $('#' + tableID).html(clonedHtml.html());
        // Specify file name
        filename = filename ? filename + '.xls' : 'Profit and Loss of ' + this.date.transform(new Date, 'dd-MM-yyyy') + '.xls';
        // Create download link element
        downloadLink = document.createElement("a");
        document.body.appendChild(downloadLink);
        if (navigator.msSaveOrOpenBlob) {
            var blob = new Blob(['\ufeff', tableHTML], { type: dataType });
            navigator.msSaveOrOpenBlob(blob, filename);
        }
        else {
            // Create a link to the file
            downloadLink.href = 'data:' + dataType + ', ' + tableHTML;
            // Setting the file name
            downloadLink.download = filename;
            //triggering the function
            downloadLink.click();
        }
    };
    StockInHandComponent = __decorate([
        core_1.Component({
            templateUrl: './stock-in-hand.component.html'
        })
    ], StockInHandComponent);
    return StockInHandComponent;
}());
exports.StockInHandComponent = StockInHandComponent;
