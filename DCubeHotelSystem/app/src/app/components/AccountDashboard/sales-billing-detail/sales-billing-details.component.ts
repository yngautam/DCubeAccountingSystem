import { Component, Input, TemplateRef } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms'
import { Observable } from 'rxjs/Rx';
import { PurchaseService } from '../../../Service/purchase.service';
import { IInventoryItem } from '../../../Model/Inventory/inventoryItem';
import { ScreenOrderDetail } from '../../../Model/AccountTransaction/accountTrans';
import { IMenuItemPortion, IMenuItem} from '../../../Model/Menu/MenuItem';
import { PeriodicConsumptionItemService } from '../../../Service/peroidic-consumption-item.service';
import { Global } from '../../../Shared/global';
import { MenuItemPortion } from '../../../Model/Menu/MenuItemPortion';
import { MenuConsumptionService } from '../../../Service/MenuConsumptionService';
@Component({
    selector: 'my-salesBillingDetail-list',
    templateUrl: './sales-billing-details.component.html'
})
export class SalesBillingDetailComponent {
    @Input('group')
    public SalesOrderDetails: FormGroup;
    public screenorderDetails: Observable<ScreenOrderDetail>;
    public imenuitemPortion: Observable<IMenuItemPortion>;
    MenuItemPortions: Observable<MenuItemPortion>;

    constructor(
        private _menuConsumptionService: MenuConsumptionService,
        private _purchaseService: PurchaseService,  ) {
        this._menuConsumptionService.getMenuConsumptionProductPortions().subscribe(data => this.MenuItemPortions = data);
    }

    // Calculate Purchase Amount
    calculateAmount(SalesOrderDetails: any) {
        return SalesOrderDetails.TotalAmount.setValue(SalesOrderDetails.Qty.value * SalesOrderDetails.UnitPrice.value);
    }

    //calculate rate 
    calcRate(SalesOrderDetails: any) {
        var itemId = this.SalesOrderDetails.controls['ItemId'].value;
        var sum = this._purchaseService.get(Global.BASE_SALE_BILLING_DETAILS_ENDPOINT + itemId)
            .subscribe((data) => {
                this.imenuitemPortion = data;
                console.log(this.imenuitemPortion);
                return SalesOrderDetails.UnitPrice.setValue(this.imenuitemPortion);
            });
    }
    searchChangeItem($event) {
        console.log($event);
    }
    configItem = {
        displayKey: 'Name', // if objects array passed which key to be displayed defaults to description
        search: true,
        limitTo: 1000
    };
}
