﻿import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl, FormArray } from '@angular/forms';
import { PeriodicConsumptionService } from '../../../Service/periodic-consumption.service';
import { PeriodicConsumption } from '../../../Model/periodic-consumption-items/periodic-consumption-item';
import { DBOperation } from '../../../Shared/enum';
import { Observable } from 'rxjs/Rx';
import { Global } from '../../../Shared/global';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { DatePipe } from '@angular/common';
import { PeriodicConsumptionItemService } from '../../../Service/peroidic-consumption-item.service';
import { IInventoryItem } from '../../../Model/Inventory/inventoryItem';
import { MenuItemPortion } from '../../../Model/Menu/MenuItemPortion';
import { MenuConsumptionService } from '../../../Service/MenuConsumptionService';

@Component({
    moduleId: module.id,
    templateUrl: 'stock-damage.component.html'
})

export class StockDamageComponent implements OnInit {
    @ViewChild('template') TemplateRef: TemplateRef<any>;
    @ViewChild('templateNested') TemplateRef2: TemplateRef<any>;
    modalRef: BsModalRef;
    modalRef2: BsModalRef;
    pConsumes: PeriodicConsumption[];
    pConsume: PeriodicConsumption;

    msg: string;
    indLoading: boolean = false;
    pConsumeFrm: FormGroup;
    dbops: DBOperation;
    modalTitle: string;
    modalBtnTitle: string;
    private formSubmitAttempt: boolean;
    private buttonDisabled: boolean;
    formattedSDate: any;
    formattedEDate: any;
    public fromDate: any;
    public toDate: any;
    public currentYear: any = {};
    public currentUser: any = {};
    public company: any = {};
    MenuItemPortions: MenuItemPortion[];

    constructor(
        private fb: FormBuilder,
        private _pcitemService: PeriodicConsumptionItemService,
        private _pConsumeservice: PeriodicConsumptionService,
        private modalService: BsModalService,
        private _menuConsumptionService: MenuConsumptionService,
        private date: DatePipe,
    ) {
        this._menuConsumptionService.getMenuConsumptionProductPortions().subscribe(data => this.MenuItemPortions = data);
        this.currentYear = JSON.parse(localStorage.getItem('currentYear'));
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.company = JSON.parse(localStorage.getItem('company'));
        this.fromDate = new Date(this.currentYear['StartDate']);
        this.toDate = new Date(this.currentYear['EndDate']);
    }

    ngOnInit(): void {
        this.pConsumeFrm = this.fb.group({
            Id: [''],
            Name: [''],
            StartDate: ['', Validators.required],
            LastUpdateTime: [''],
            FinancialYear: [''],
            UserName: [''],
            CompanyCode: [''],
            PeriodicConsumptionItems: this.fb.array([
                this.initPeriodicConsumDetails(),
            ]),
        });
        this.loadPeriodicConsumptions();
    }

    initPeriodicConsumDetails() {
        return this.fb.group({
            InventoryItemId: ['', Validators.required],
            InStock: [''],
            Consumption: ['', Validators.required],
            PhysicalInventory: ['', Validators.required],
            PeriodicConsumptionId: [''],
            Cost: [''],
            FinancialYear: [''],
            UserName: [''],
            CompanyCode: [''],
        })
    }

    loadPeriodicConsumptions(): void {
        this.indLoading = true;
        this._pConsumeservice.get(Global.BASE_PERIODICCONSUMPTION_ENDPOINT)
            .subscribe(pConsumes => { this.pConsumes = pConsumes; this.indLoading = false; },
            error => this.msg = <any>error);
    }

    getIRItem(Id: number) {
        if (this.MenuItemPortions) {
            return this.MenuItemPortions.filter((IRItem) => {
                return IRItem.MenuItemPortionId === Id;
            })[0];
        }
    }

    getPeriodicConsumption(Id: number) {
        debugger
        this.indLoading = true;
        return this._pConsumeservice.get(Global.BASE_PERIODICCONSUMPTION_ENDPOINT + '?Id=' + Id);
    }

    exportTableToExcel(tableID, filename = '') {
        var downloadLink;
        var dataType = 'application/vnd.ms-excel';
        var clonedtable = $('#' + tableID);
        var clonedHtml = clonedtable.clone();
        $(clonedtable).find('.export-no-display').remove();
        var tableSelect = document.getElementById(tableID);
        var tableHTML = tableSelect.outerHTML.replace(/ /g, '%20');
        $('#' + tableID).html(clonedHtml.html());

        // Specify file name
        filename = filename ? filename + '.xls' : 'Inventory Receipts of ' + this.date.transform(new Date, 'dd-MM-yyyy') + '.xls';

        // Create download link element
        downloadLink = document.createElement("a");

        document.body.appendChild(downloadLink);

        if (navigator.msSaveOrOpenBlob) {
            var blob = new Blob(['\ufeff', tableHTML], { type: dataType });
            navigator.msSaveOrOpenBlob(blob, filename);
        } else {
            // Create a link to the file
            downloadLink.href = 'data:' + dataType + ', ' + tableHTML;

            // Setting the file name
            downloadLink.download = filename;

            //triggering the function
            downloadLink.click();
        }
    }

    addPeriodicConsumed() {
        this.dbops = DBOperation.create;
        this.SetControlsState(true);
        this.modalTitle = "Add Stock Damage";
        this.modalBtnTitle = "Save & Submit";
        this.pConsumeFrm.reset();

        this.pConsumeFrm.controls['Name'].setValue('Stock Damage');
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false,
            class: 'modal-lg'
        });
    }

    editPeriodicConsumed(Id: number) {
        this.dbops = DBOperation.update;
        this.SetControlsState(true);
        this.modalTitle = "Edit Stock Damage";
        this.modalBtnTitle = "Update";
        debugger;
        this.getPeriodicConsumption(Id).subscribe((periodicConsumption: PeriodicConsumption) => {
            debugger
            this.indLoading = false;
            this.pConsumeFrm.controls['Id'].setValue(periodicConsumption.Id);
            this.pConsumeFrm.controls['Name'].setValue(periodicConsumption.Name);
            this.pConsumeFrm.controls['StartDate'].setValue(new Date(periodicConsumption.StartDate));

            this.pConsumeFrm.controls['PeriodicConsumptionItems'] = this.fb.array([]);
            const control = <FormArray>this.pConsumeFrm.controls['PeriodicConsumptionItems'];

            for (let i = 0; i < periodicConsumption.PeriodicConsumptionItems.length; i++) {
                control.push(this.fb.group(periodicConsumption.PeriodicConsumptionItems[i]));
            }
            this.modalRef = this.modalService.show(this.TemplateRef, {
                backdrop: 'static',
                keyboard: false,
                class: 'modal-lg'
            });
        },
            error => this.msg = <any>error);
    }

    deletePeriodicConsumed(Id: number) {
        debugger
        this.dbops = DBOperation.delete;
        this.SetControlsState(true);
        this.modalTitle = "Confirm to Delete Stock Damage?";
        this.modalBtnTitle = "Delete";

        this.getPeriodicConsumption(Id).subscribe((periodicConsumption: PeriodicConsumption) => {
            debugger
            this.indLoading = false;
            this.pConsumeFrm.controls['Id'].setValue(periodicConsumption.Id);
            this.pConsumeFrm.controls['Name'].setValue(periodicConsumption.Name);
            this.pConsumeFrm.controls['StartDate'].setValue(new Date(periodicConsumption.StartDate));

            this.pConsumeFrm.controls['PeriodicConsumptionItems'] = this.fb.array([]);
            const control = <FormArray>this.pConsumeFrm.controls['PeriodicConsumptionItems'];

            for (let i = 0; i < periodicConsumption.PeriodicConsumptionItems.length; i++) {
                control.push(this.fb.group(periodicConsumption.PeriodicConsumptionItems[i]));
            }
            this.modalRef = this.modalService.show(this.TemplateRef, {
                backdrop: 'static',
                keyboard: false,
                class: 'modal-lg'
            });
        },
            error => this.msg = <any>error);
    }

    // Push the values of PeriodicConsumptionItems 
    addPeriodicitems() {
        const control = <FormArray>this.pConsumeFrm.controls['PeriodicConsumptionItems'];
        const addpcItems = this.initPeriodicConsumDetails();
        control.push(addpcItems);
    }

    //remove the rows//
    removeInventory(i: number) {
        let controls = <FormArray>this.pConsumeFrm.controls['PeriodicConsumptionItems'];
        let controlToRemove = this.pConsumeFrm.controls.PeriodicConsumptionItems['controls'][i].controls;
        let selectedControl = controlToRemove.hasOwnProperty('Id') ? controlToRemove.Id.value : 0;

        if (selectedControl) {
            this._pcitemService.delete(Global.BASE_PERIODICCONSUMPTIONITEM_ENDPOINT, i).subscribe(data => {
                (data == 1) && controls.removeAt(i);
            });
        } else {
            if (i >= 0) {
                controls.removeAt(i);
            } else {
                alert("Form requires at least one row");
            }
        }
    }


    validateAllFields(formGroup: FormGroup) {
        Object.keys(formGroup.controls).forEach(field => {
            const control = formGroup.get(field);
            if (control instanceof FormControl) {
                control.markAsTouched({ onlySelf: true });
            } else if (control instanceof FormGroup) {
                this.validateAllFields(control);
            }
        });
    }

    //displays the confirm popup-window
    openModal2(template: TemplateRef<any>) {
        this.modalRef2 = this.modalService.show(template, { class: 'modal-sm' });
    }

    //Submit the Form
    onSubmit() {
        debugger;
        this.msg = "";
        this.formSubmitAttempt = true;
        let pConsumer = this.pConsumeFrm;
        let pConsumptionDate = new Date(pConsumer.get('StartDate').value);
        pConsumer.get('StartDate').setValue(pConsumptionDate);

        if (!this.voucherDateValidator(pConsumer.get('StartDate'))) {
            return false;
        }


        pConsumer.get('FinancialYear').setValue(this.currentYear['Name'] || '');
        pConsumer.get('UserName').setValue(this.currentUser && this.currentUser['UserName'] || '');
        pConsumer.get('CompanyCode').setValue(this.currentUser && this.company['BranchCode'] || '');

        if (pConsumer.valid) {
            switch (this.dbops) {
                case DBOperation.create:
                    this._pConsumeservice.post(Global.BASE_PERIODICCONSUMPTION_ENDPOINT, pConsumer.value).subscribe(
                        data => {
                            debugger
                            if (data == 1) //Success
                            {
                                this.openModal2(this.TemplateRef2);
                                this.loadPeriodicConsumptions();
                            }
                            else {
                                // this.modal.backdrop;
                                this.msg = "There is some issue in creating records, please contact to system administrator!";
                            }
                            this.modalRef.hide();
                            this.formSubmitAttempt = false;
                        });
                    break;
                case DBOperation.update:
                    let pConsumeObj = {
                        Id: this.pConsumeFrm.controls['Id'].value,
                        Name: this.pConsumeFrm.controls['Name'].value,
                        StartDate: this.pConsumeFrm.controls['StartDate'].value,
                        PeriodicConsumptionItems: this.pConsumeFrm.controls['PeriodicConsumptionItems'].value
                    }
                    this._pConsumeservice.put(Global.BASE_PERIODICCONSUMPTION_ENDPOINT, pConsumer.value.Id, pConsumeObj).subscribe(
                        data => {
                            if (data == 1) //Success
                            {
                                this.openModal2(this.TemplateRef2);
                                this.loadPeriodicConsumptions();
                            }
                            else {
                                alert("There is some issue in updating records, please contact to system administrator!");
                            }
                            this.modalRef.hide();
                            this.formSubmitAttempt = false;
                        });
                    break;
                case DBOperation.delete:
                    this._pConsumeservice.delete(Global.BASE_PERIODICCONSUMPTION_ENDPOINT, pConsumer.value.Id).subscribe(
                        data => {
                            debugger
                            if (data == 1) {
                                alert("Data deleted sucessfully");
                                this.loadPeriodicConsumptions();
                            }
                            else {
                                alert("There is some issue in deleting records, please contact to system administrator!");
                            }
                            this.modalRef.hide();
                            this.formSubmitAttempt = false;
                        })
            }
        }
        else {
            this.validateAllFields(pConsumer);
        }
    }

    confirm(): void {
        this.modalRef2.hide();
    }

    reset() {
        let control = this.pConsumeFrm.controls['Id'].value;
        if (control > 0) {
            this.buttonDisabled = true;
        }
        else {
            this.pConsumeFrm.reset();
        }
    }

    SetControlsState(isEnable: boolean) {
        isEnable ? this.pConsumeFrm.enable() : this.pConsumeFrm.disable();
    }

    voucherDateValidator(control: any) {
        let today = new Date;
        if (!control.value) {
            alert("Please select the Inventory Date");
            return false;
        }

        let pConsumptionDate = new Date(control.value);
        let currentYearStartDate = new Date(this.currentYear.StartDate);
        let currentYearEndDate = new Date(this.currentYear.EndDate);

        if ((pConsumptionDate < currentYearStartDate) || (pConsumptionDate > currentYearEndDate) || (pConsumptionDate > today)) {
            alert("Date should be within current financial year's start date and end date inclusive, Error Occured!");
            return false;
        }
        return true;
    }

    /**
   *  Get the list of filtered journals by the form and to date
   */
    filterJournalByDate() {
        this.indLoading = true;
        this._pConsumeservice.get(Global.BASE_PERIODICCONSUMPTION_ENDPOINT + '?fromDate=' + this.date.transform(this.fromDate, 'dd-MM-yyyy') + '&toDate=' + this.date.transform(this.toDate, 'dd-MM-yyyy') + '&TransactionTypeId=' + 5)
            .subscribe(pConsumes => {
                this.indLoading = false;
                return this.pConsumes = pConsumes;
            },
            error => this.msg = <any>error);
    }

    onFilterDateSelect(selectedDate) {
        let currentYearStartDate = new Date(this.currentYear.StartDate);
        let currentYearEndDate = new Date(this.currentYear.EndDate);

        if (selectedDate < currentYearStartDate) {
            this.fromDate = currentYearStartDate;
            alert("Date should not be less than current financial year's start date");
        }

        if (selectedDate > currentYearEndDate) {
            this.toDate = currentYearEndDate;
            alert("Date should not be greater than current financial year's end date");
        }
    }
}
