﻿import { Component, ViewChild, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { PeriodicConsumptionItemService } from '../../../Service/peroidic-consumption-item.service';
import { InventoryReceiptService } from '../../../Service/InventoryReceipt.service';
import { PeriodicConsumptionItem } from '../../../Model/periodic-consumption-items/periodic-consumption-item';
import { DBOperation } from '../../../Shared/enum';
import { Observable } from 'rxjs/Rx';
import { InventoryReceiptDetails } from '../../../Model/Inventory/InventoryReceipt';
import { Global } from '../../../Shared/global';
import { IMenuItemPortion} from '../../../Model/Menu/MenuItem';
import { MenuItemPortion } from '../../../Model/Menu/MenuItemPortion';
import { MenuConsumptionService } from '../../../Service/MenuConsumptionService';

@Component({
    selector: 'stock-damage-details',
    templateUrl: './stock-damage-details.component.html'
})

export class StockDamageDetailsComponent {
    @Input('group')
    public pcDetails: FormGroup;
    imenuitemPortion: IMenuItemPortion;
    perodicconsume: PeriodicConsumptionItem;
    inventorycost: IMenuItemPortion;
    inventoryconsumption: PeriodicConsumptionItem;
    MenuItemPortions: Observable<MenuItemPortion>;
    public formSubmitAttempt: boolean;
    InStockValue: any;
    invnCost: any;

    constructor(
        private fb: FormBuilder,
        private _pcitemService: PeriodicConsumptionItemService,
        private _menuConsumptionService: MenuConsumptionService,
    ) {
        this._menuConsumptionService.getMenuConsumptionProductPortions().subscribe(data => this.MenuItemPortions = data);
    }

    //total item and total cost
    postToApi(pcDetails: any) {
        debugger;
        var itemId = this.pcDetails.controls['InventoryItemId'].value;
        var sum = this._pcitemService.get(Global.BASE_PERIODICCONSUMPTIONITEM_ENDPOINT + itemId).subscribe((data) => {
            this.imenuitemPortion = data;
            return pcDetails.InStock.setValue(this.imenuitemPortion);
        });

        var cost = this._pcitemService.getCost(Global.BASE_COSTDETAILS_ENDPOINT + itemId).subscribe((data) => {
            debugger
            this.inventorycost = data;
        });
    }

    //calculate net consumption
    calculateNetConsumption(pcDetails: any, InStockValue: any, invnCost: any) {
        var itemId = this.pcDetails.controls['InventoryItemId'].value;
        var netConsumption = this._pcitemService.getConsum(Global.BASE_PERIODICCONSUMPTIONITEM_ENDPOINT + itemId).subscribe(data => {
            debugger;
            this.imenuitemPortion = data;
            return pcDetails.InStock.setValue(this.imenuitemPortion);
        });
        debugger;
        InStockValue = this.imenuitemPortion;
        invnCost = this.inventorycost;
        return (pcDetails.PhysicalInventory.setValue(InStockValue - pcDetails.Consumption.value)),
            (pcDetails.Cost.setValue(invnCost * pcDetails.Consumption.value));
    }

    //Calculates InStock values
    calculateInStock(pcDetails: any, invnQuantitySum: any, ) {
        debugger;
        if (pcDetails.InventoryItemId == 0) {
            invnQuantitySum = this.imenuitemPortion;
            return pcDetails.InStock.setValue(invnQuantitySum);
        }
        else
        {
            debugger;
            var itemId = this.pcDetails.controls['InventoryItemId'].value;
            var netConsumption = this._pcitemService.getConsum(Global.BASE_PERIODICCONSUMPTIONITEM_ENDPOINT + itemId).subscribe((data) => {
                this.imenuitemPortion = data;
                return pcDetails.InStock.setValue(this.imenuitemPortion);
            });
        }
    }

    //Calculates PhysicalInventory
    getPhysicalInventory(pcDetails: any, InStockValue: any) {
        InStockValue = this.imenuitemPortion;
        return pcDetails.PhysicalInventory.setValue(InStockValue - pcDetails.Consumption.value);
    }

    //Correct Calculates Cost
    getCost(pcDetails: any, invnCost: any) {
        invnCost = this.inventorycost;
        return pcDetails.Cost.setValue(invnCost * pcDetails.Consumption.value);
    }
}