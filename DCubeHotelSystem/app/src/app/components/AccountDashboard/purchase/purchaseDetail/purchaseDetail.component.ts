import { Component, Input } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms'
import { Observable } from 'rxjs/Rx';
import { MenuItemPortion } from '../../../../Model/Menu/MenuItemPortion';
import { IMenuItemPortion, IMenuItem } from '../../../../Model/Menu/MenuItem';
import { MenuConsumptionService } from '../../../../Service/MenuConsumptionService';

@Component({
    selector: 'my-purchaseDetail-list',
    templateUrl: './purchaseDetail.component.html'
})
export class PurchaseDetailsComponent {
    @Input('group')
    public purchaseDetails: FormGroup;
    public imenuitemPortion: Observable<IMenuItemPortion>;
    MenuItemPortions: Observable<MenuItemPortion>;

    constructor(private _menuConsumptionService: MenuConsumptionService) {
        this._menuConsumptionService.getMenuConsumptionProductPortions().subscribe(data => this.MenuItemPortions = data);
    }

    // calculate Purchase Amount//
    calculateAmount(purchaseDetails: any) {
        debugger;
        return purchaseDetails.PurchaseAmount.setValue(purchaseDetails.Quantity.value * purchaseDetails.PurchaseRate.value);
    }
    searchChange($event) {
        console.log($event);
    }
    config = {
        displayKey: 'Name', // if objects array passed which key to be displayed defaults to description
        search: true,
        limitTo: 1000
    };
}