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
var SalesBillingDetailComponent = /** @class */ (function () {
    function SalesBillingDetailComponent(_menuConsumptionService, _purchaseService) {
        var _this = this;
        this._menuConsumptionService = _menuConsumptionService;
        this._purchaseService = _purchaseService;
        this._menuConsumptionService.getMenuConsumptionProductPortions().subscribe(function (data) { return _this.MenuItemPortions = data; });
    }
    // Calculate Purchase Amount
    SalesBillingDetailComponent.prototype.calculateAmount = function (SalesOrderDetails) {
        return SalesOrderDetails.TotalAmount.setValue(SalesOrderDetails.Qty.value * SalesOrderDetails.UnitPrice.value);
    };
    //calculate rate 
    SalesBillingDetailComponent.prototype.calcRate = function (SalesOrderDetails) {
        var _this = this;
        var itemId = this.SalesOrderDetails.controls['ItemId'].value;
        var sum = this._purchaseService.get(global_1.Global.BASE_SALE_BILLING_DETAILS_ENDPOINT + itemId)
            .subscribe(function (data) {
            _this.imenuitemPortion = data;
            console.log(_this.imenuitemPortion);
            return SalesOrderDetails.UnitPrice.setValue(_this.imenuitemPortion);
        });
    };
    __decorate([
        core_1.Input('group')
    ], SalesBillingDetailComponent.prototype, "SalesOrderDetails", void 0);
    SalesBillingDetailComponent = __decorate([
        core_1.Component({
            selector: 'my-salesBillingDetail-list',
            templateUrl: './sales-billing-details.component.html'
        })
    ], SalesBillingDetailComponent);
    return SalesBillingDetailComponent;
}());
exports.SalesBillingDetailComponent = SalesBillingDetailComponent;
