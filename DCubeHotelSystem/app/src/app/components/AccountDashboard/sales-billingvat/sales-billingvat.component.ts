import { Component, OnInit, ViewChild, TemplateRef, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, NgModel, FormArray, FormControl, AbstractControl } from '@angular/forms';
import { AccountTransactionValues, AccountTrans, PurchaseOrderDetail, ScreenOrderDetail, EntityMock } from '../../../Model/AccountTransaction/accountTrans';
import { Account } from '../../../Model/Account/account';
import { PurchaseService } from '../../../Service/purchase.service';
import { PurchaseDetailsService } from '../../../Service/PurchaseDetails.service';
import { AccountTransValuesService } from '../../../Service/accountTransValues.service';
import { DBOperation } from '../../../Shared/enum';
import { Observable } from 'rxjs/Rx';
import { Global } from '../../../Shared/global';
import { DatePipe } from '@angular/common'
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import * as XLSX from 'xlsx';
import { FileService } from '../../../Service/file.service';
import { ReservationCustomerService } from '../../../Service/customer.services';
type CSV = any[][];

@Component({
    templateUrl: './sales-billingvat.component.html',
    styleUrls: ['./sales-billingvat.component.css']
})
export class SalesBillingvatComponent implements OnInit {
    @ViewChild("template") TemplateRef: TemplateRef<any>;
    @ViewChild('templateNested') TemplateRef2: TemplateRef<any>;
    @ViewChild('fileInput') fileInput: ElementRef;

    IsDiscountPercentage: boolean;
    selectedValue: any = '0.00';
    modalRef: BsModalRef;
    modalRef2: BsModalRef;
    public salesBillingForm: FormGroup;
    SalesBilling: AccountTrans[];
    accounts: Account[];
    

    dbops: DBOperation;
    msg: string;
    modalTitle: string;
    modalBtnTitle: string;
    indLoading: boolean = false;
    private formSubmitAttempt: boolean;
    private buttonDisabled: boolean;
    formattedDate: any;
    public entityLists: EntityMock[];
    public fromDate: any;
    public toDate: any;
    public currentYear: any = {};
    public currentUser: any = {};
    public company: any = {};
    dropMessage: string = "Upload Reference File";
    toExportFileName: string = 'Sales Voucher of ' + this.date.transform(new Date, "dd-MM-yyyy") + '.xls';
    uploadUrl = Global.BASE_FILE_UPLOAD_ENDPOINT;
    fileUrl: string = '';
    settings = {
        bigBanner: false,
        timePicker: false,
        format: 'dd/MM/yyyy',
        defaultOpen: false
    };
    /**
     * Constructor
     * 
     * @param fb 
     * @param _purchaseService 
     * @param _purchaseDetailsService 
     * @param _accountTransValues 
     * @param date 
     * @param modalService 
     */
    constructor(
        private fb: FormBuilder,
        private _purchaseService: PurchaseService,
        private _purchaseDetailsService: PurchaseDetailsService,
        private _accountTransValues: AccountTransValuesService,
        private _customerService: ReservationCustomerService,
        private date: DatePipe,
        private modalService: BsModalService,
        private fileService: FileService
    ) {
        this.currentYear = JSON.parse(localStorage.getItem('currentYear'));
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.company = JSON.parse(localStorage.getItem('company'));
        this.fromDate = new Date(this.currentYear['StartDate']);
        this.toDate = new Date(this.currentYear['EndDate']);
    }

    ngOnInit(): void {
        this.salesBillingForm = this.fb.group({
            Id: [''],
            Name: [''],
            AccountTransactionDocumentId: [''],
            Date: [new Date(), Validators.required],
            AccountTransactionTypeId: [''],
            SourceAccountTypeId: ['', Validators.required],
            Description: ['', Validators.required],
            Amount: [''],
            PercentAmount: [''],
            ref_invoice_number: [''],
            NetAmount: [''],
            Discount: [''], 
            VATAmount: [''],
            GrandAmount: [''],
            VType: [''],
            VoucherNo: [''],
            IsDiscountPercentage: [''], 
            SalesOrderDetails: this.fb.array([this.initSalesOrderDetails()]),
            AccountTransactionValues: this.fb.array([this.initJournalDetail()]),
            FinancialYear: [''],
            UserName: [''],
            CompanyCode: ['']
        });
        this.LoadCustomers();
        this.loadSalesBillingList();
    }

    LoadCustomers() {
        debugger
        this.indLoading = true;
        this._customerService.get(Global.BASE_SCREENCustomerTicket_ENDPOINT)
            .subscribe(
            customers => {
                this.accounts = customers;
                this.indLoading = false;
            },
            error => console.log(error)
            );
    }

    voucherDateValidator(control: any) {
        let today = new Date;
        let tomorrow = new Date(today.setDate(today.getDate() + 1));

        if (!control.value) {
            alert("Please select the Voucher Date");
            return false;
        }

        let voucherDate = new Date(control.value);
        let currentYearStartDate = new Date(this.currentYear.StartDate);
        let currentYearEndDate = new Date(this.currentYear.EndDate);

        if ((voucherDate < currentYearStartDate) || (voucherDate > currentYearEndDate) || voucherDate >= tomorrow) {
            alert("Date should be within current financial year's start date and end date inclusive");
            return false;
        }
        else {
            return true;
        }
    }

    viewFile(fileUrl, template: TemplateRef<any>) {
        this.fileUrl = fileUrl;
        this.modalTitle = "View Attachment";
        this.modalRef = this.modalService.show(template, { keyboard: false, class: 'modal-lg' });
    }

    /**
     * Exports the pOrder voucher data in CSV/ Excel format
     */
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
        filename = filename ? filename + '.xls' : this.toExportFileName;

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

    loadSalesBillingList(): void {
        this.indLoading = true;
        this._purchaseService.get(Global.BASE_SALE_BILLING_ENDPOINT + '?fromDate=' + this.date.transform(this.fromDate, 'yyyy-MM-dd') + '&toDate=' + this.date.transform(this.toDate, 'yyyy-MM-dd') + '&TransactionTypeId=' + 3)
            .subscribe(
            SalesBilling => {
                SalesBilling.map((purch) => purch['File'] = Global.BASE_HOST_ENDPOINT + Global.BASE_FILE_UPLOAD_ENDPOINT + '?Id=' + purch.Id + '&ApplicationModule=JournalVoucher');
                this.SalesBilling = SalesBilling;
                this.indLoading = false;
            },
            error => this.msg = <any>error
            );
    }

    addSalesBilling() {
        this.dbops = DBOperation.create;
        this.SetControlsState(true);
        this.modalTitle = "Add Sales Billing";
        this.modalBtnTitle = "Save & Submit";
        this.reset();
        this.salesBillingForm.controls['Name'].setValue('Direct Sales');
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false,
            class: 'modal-lg'
        });
    }

    getSalesBilling(Id: number) {
        this.indLoading = true;
        return this._purchaseService.get(Global.BASE_SALE_BILLING_ENDPOINT + '?TransactionId=' + Id);
    }

    /**
     * Opens Edit Existing Journal Voucher Form Modal
     * @param Id 
     */
    editSalesBilling(Id: number) {
        this.reset();
        this.dbops = DBOperation.update;
        this.SetControlsState(true);
        this.modalTitle = "Edit Sales Billing";
        this.modalBtnTitle = "Update";
        this.getSalesBilling(Id)
            .subscribe((SalesBilling: AccountTrans) => {
                debugger
                this.indLoading = false;
                this.salesBillingForm.controls['Id'].setValue(SalesBilling.Id);
                this.salesBillingForm.controls['Date'].setValue(new Date(SalesBilling.Date));
                this.salesBillingForm.controls['Name'].setValue(SalesBilling.Name);
                this.salesBillingForm.controls['AccountTransactionDocumentId'].setValue(SalesBilling.AccountTransactionDocumentId);
                this.salesBillingForm.controls['SourceAccountTypeId'].setValue(SalesBilling.SourceAccountTypeId);
                this.salesBillingForm.controls['Description'].setValue(SalesBilling.Description);
                this.salesBillingForm.controls['Discount'].setValue(SalesBilling.Discount);
                this.salesBillingForm.controls['NetAmount'].setValue(SalesBilling.NetAmount);
                this.salesBillingForm.controls['Amount'].setValue(SalesBilling.Discount + SalesBilling.NetAmount );
                this.salesBillingForm.controls['PercentAmount'].setValue(SalesBilling.PercentAmount);
                this.salesBillingForm.controls['VATAmount'].setValue(SalesBilling.VATAmount);
                this.salesBillingForm.controls['GrandAmount'].setValue(SalesBilling.VATAmount + SalesBilling.NetAmount );

                this.salesBillingForm.controls['SalesOrderDetails'] = this.fb.array([]);
                const control = <FormArray>this.salesBillingForm.controls['SalesOrderDetails'];

                for (let i = 0; i < SalesBilling.SalesOrderDetails.length; i++) {
                    control.push(this.fb.group(SalesBilling.SalesOrderDetails[i]));
                }

                this.salesBillingForm.controls['AccountTransactionValues'] = this.fb.array([]);
                const controlAc = <FormArray>this.salesBillingForm.controls['AccountTransactionValues'];
                controlAc.controls = [];

                for (let i = 0; i < SalesBilling.AccountTransactionValues.length; i++) {
                    let valuesFromServer = SalesBilling.AccountTransactionValues[i];
                    let instance = this.fb.group(valuesFromServer);

                    if (valuesFromServer['entityLists'] === "Dr") {
                        instance.controls['Credit'].disable();

                    } else if (valuesFromServer['entityLists'] === "Cr") {
                        instance.controls['Debit'].disable();
                    }
                    controlAc.push(instance);
                }

                this.modalRef = this.modalService.show(this.TemplateRef, {
                    backdrop: 'static',
                    keyboard: false,
                    class: 'modal-lg'
                });
            });
    }

    deleteSalesBilling(Id: number) {
        this.dbops = DBOperation.delete;
        this.SetControlsState(true);
        this.modalTitle = "Delete Sales Items";
        this.modalBtnTitle = "Delete";
        this.reset();
        this.getSalesBilling(Id)
            .subscribe((SalesBilling: AccountTrans) => {
                debugger
                this.indLoading = false;
                this.salesBillingForm.controls['Id'].setValue(SalesBilling.Id);
                this.salesBillingForm.controls['Date'].setValue(new Date(SalesBilling.Date));
                this.salesBillingForm.controls['Name'].setValue(SalesBilling.Name);
                this.salesBillingForm.controls['AccountTransactionDocumentId'].setValue(SalesBilling.AccountTransactionDocumentId);
                this.salesBillingForm.controls['SourceAccountTypeId'].setValue(SalesBilling.SourceAccountTypeId);
                this.salesBillingForm.controls['Description'].setValue(SalesBilling.Description);
                this.salesBillingForm.controls['Discount'].setValue(SalesBilling.Discount);
                this.salesBillingForm.controls['Amount'].setValue(SalesBilling.Discount + SalesBilling.NetAmount);
                this.salesBillingForm.controls['NetAmount'].setValue(SalesBilling.NetAmount);
                this.salesBillingForm.controls['PercentAmount'].setValue(SalesBilling.PercentAmount);
                this.salesBillingForm.controls['VATAmount'].setValue(SalesBilling.VATAmount);
                this.salesBillingForm.controls['GrandAmount'].setValue(SalesBilling.VATAmount + SalesBilling.NetAmount);

                this.salesBillingForm.controls['SalesOrderDetails'] = this.fb.array([]);
                const control = <FormArray>this.salesBillingForm.controls['SalesOrderDetails'];

                for (let i = 0; i < SalesBilling.SalesOrderDetails.length; i++) {
                    control.push(this.fb.group(SalesBilling.SalesOrderDetails[i]));
                }

                this.salesBillingForm.controls['AccountTransactionValues'] = this.fb.array([]);
                const controlAc = <FormArray>this.salesBillingForm.controls['AccountTransactionValues'];
                controlAc.controls = [];

                for (let i = 0; i < SalesBilling.AccountTransactionValues.length; i++) {
                    let valuesFromServer = SalesBilling.AccountTransactionValues[i];
                    let instance = this.fb.group(valuesFromServer);

                    if (valuesFromServer['entityLists'] === "Dr") {
                        instance.controls['Credit'].disable();

                    } else if (valuesFromServer['entityLists'] === "Cr") {
                        instance.controls['Debit'].disable();
                    }
                    controlAc.push(instance);
                }

                this.modalRef = this.modalService.show(this.TemplateRef, {
                    backdrop: 'static',
                    keyboard: false,
                    class: 'modal-lg'
                });
            });
    }

    // Initialize the formb uilder arrays
    initSalesOrderDetails() {
        return this.fb.group({
            Id: [''],
            IsSelected: '',
            IsVoid: '',
            ItemId: ['', Validators.required],
            OrderId: '',
            OrderNumber: '',
            Qty: ['', Validators.required],
            Tags: '',
            UnitPrice: ['', Validators.required],
            TotalAmount: [''],
        });
    }

    initJournalDetail() {
        return this.fb.group({
            entityLists: ['', Validators.required],
            AccountId: ['', Validators.required],
            Debit: ['', Validators.required],
            Credit: ['', Validators.required],
            Description: [''],
        });
    }

    // Push the values of purchasdetails
    addSalesBillingitems() {
        const control = <FormArray>this.salesBillingForm.controls['SalesOrderDetails'];
        const addPurchaseValues = this.initSalesOrderDetails();
        control.push(addPurchaseValues);
    }

    //remove the rows//
    removeSalesBillingitems(i: number) {
        let controls = <FormArray>this.salesBillingForm.controls['SalesOrderDetails'];
        let controlToRemove = this.salesBillingForm.controls.SalesOrderDetails['controls'][i].controls;
        let selectedControl = controlToRemove.hasOwnProperty('Id') ? controlToRemove.Id.value : 0;

        if (selectedControl) {
            this._purchaseDetailsService.delete(Global.BASE_SALE_BILLING_DETAILS_ENDPOINT, controlToRemove.Id.value).subscribe(data => {
                alert("Data With Id= " + controlToRemove.Id.value + " Removed Successfully!");
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

    calculateAmount() {
        let controls = this.salesBillingForm.controls['SalesOrderDetails'].value;
        return controls.reduce(function (total: any, accounts: any) {
            return (accounts.TotalAmount) ? (total + Math.round(accounts.TotalAmount)) : total;
        }, 0);
    }

    calculateNetAmount(salesBillingForm: any) {
        this.getDiscountPercent(salesBillingForm);
        let totalAmt = this.calculateAmount();
        let calcNetAmount = salesBillingForm.NetAmount.setValue((totalAmt - (salesBillingForm.Discount.value)).toFixed(2));
        return calcNetAmount;
    }

    calculateVATAmount(salesBillingForm: any) {
        this.getDiscountPercent(salesBillingForm);
        let totalAmt = this.calculateAmount();
        let calcVatAmt = salesBillingForm.VATAmount.setValue(((totalAmt - (salesBillingForm.Discount.value)) * 0.13).toFixed(2));
        return calcVatAmt;
    }

    calculateGndAmount(salesBillingForm: any) {
        this.getDiscountPercent(salesBillingForm);
        let totalAmt = this.calculateAmount();
        const Discount = (<FormControl>this.salesBillingForm.controls['Discount']);
        const IsDiscountPercentage = (<FormControl>this.salesBillingForm.controls['IsDiscountPercentage']);
        const PercentAmount = (<FormControl>this.salesBillingForm.controls['PercentAmount']);
        let calcGrandTot = 0;
        if (IsDiscountPercentage.value == true) {
            calcGrandTot = salesBillingForm.GrandAmount.setValue(((totalAmt - (((Discount.value / 100) * totalAmt))) + ((totalAmt - (((Discount.value / 100) * totalAmt))) * 0.13)).toFixed(2));
        }
        else {
            calcGrandTot = salesBillingForm.GrandAmount.setValue(((totalAmt - (salesBillingForm.Discount.value)) + ((totalAmt - (salesBillingForm.Discount.value)) * 0.13)).toFixed(2));
        }
        console.log(calcGrandTot);
        return calcGrandTot;
    }

    calculatePercentDiscountAmount(salesBillingForm: any) {
        const IsDiscountPercentage = (<FormControl>this.salesBillingForm.controls['IsDiscountPercentage']);
        if (salesBillingForm.Discount.value <= this.calculateAmount() && IsDiscountPercentage.value == false) {
            let totalAmt = this.calculateAmount();
            let calcNetAmount = salesBillingForm.NetAmount.setValue(totalAmt - (salesBillingForm.Discount.value));
            let calcVatAmt = salesBillingForm.VATAmount.setValue((totalAmt - (salesBillingForm.Discount.value)) * 0.13);
            let calcGrandTot = salesBillingForm.GrandAmount.setValue((totalAmt - (salesBillingForm.Discount.value)) + ((totalAmt - (salesBillingForm.Discount.value)) * 0.13));
            return ((calcNetAmount) & (calcVatAmt) & (calcGrandTot)).toFixed(2);
        }
        else {
            alert("Entered value is greater than total amount ? " + "Your entered value is = " + salesBillingForm.Discount.value);
            return false;
        }
    }

    //Calculate discount for more than 0% or less than 100%
    getDiscountPercent(salesBillingForm) {
        const IsDiscountPercentage = (<FormControl>this.salesBillingForm.controls['IsDiscountPercentage']);

        const Discount = (<FormControl>this.salesBillingForm.controls['Discount']);
        const PercentAmount = (<FormControl>this.salesBillingForm.controls['PercentAmount']);
        const NetAmount = (<FormControl>this.salesBillingForm.controls['NetAmount']);
        const VATAmount = (<FormControl>this.salesBillingForm.controls['VATAmount']);
        const GrandAmount = (<FormControl>this.salesBillingForm.controls['GrandAmount']);

        if (Discount.value > 0 && Discount.value <= 100 && IsDiscountPercentage.value == true) {
            let totalAmt = this.calculateAmount();
            let calcPercentAmount = ((Discount.value / 100) * totalAmt).toFixed(2);
            let calcNetAmount = (totalAmt - (((Discount.value / 100) * totalAmt))).toFixed(2);
            let calcVatAmt = ((totalAmt - (((Discount.value / 100) * totalAmt))) * 0.13).toFixed(2);
            let calcGrandTot = ((totalAmt - (((Discount.value / 100) * totalAmt))) + ((totalAmt - (((Discount.value / 100) * totalAmt))) * 0.13)).toFixed(2);
            salesBillingForm.PercentAmount.setValue(calcPercentAmount);
            salesBillingForm.NetAmount.setValue(calcNetAmount);
            salesBillingForm.VATAmount.setValue(calcVatAmt);
            salesBillingForm.GrandAmount.setValue(calcGrandTot);
        }
        //else {
        //    alert("Entered value is greater than 100% ? " + "Your entered value is = " + Discount.value);
        //    return false;
        //}
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

    //Opens confirm window modal//
    openModal2(template: TemplateRef<any>) {
        this.modalRef2 = this.modalService.show(template, { class: 'modal-sm' });
    }

    calcDiscountTotal(SalesBilling) {
        var Discount = 0;
        for (var i = 0; i < SalesBilling.length; i++) {
            Discount = Discount + parseFloat(SalesBilling[i].Discount);
        }
        return Discount;
    }

   
    calcNetAmount(SalesBilling) {
        var NetAmount = 0;
        for (var i = 0; i < SalesBilling.length; i++) {
            NetAmount = NetAmount + parseFloat(SalesBilling[i].NetAmount);
        }
        return NetAmount;
    }

    calcVATAmount(SalesBilling) {
        var VATAmount = 0;
        for (var i = 0; i < SalesBilling.length; i++) {
            VATAmount = VATAmount + parseFloat(SalesBilling[i].VATAmount);
        }
        return VATAmount;
    }

    calcGrandAmount(SalesBilling) {
        let netAmount = this.calcNetAmount(SalesBilling);
        let vatAmount = this.calcVATAmount(SalesBilling);

        let GrandAmt = netAmount + vatAmount;
        return GrandAmt;
    }

    calcAmount(SalesBilling) {
        let netAmount = this.calcNetAmount(SalesBilling);
        let AmtDicount = this.calcDiscountTotal(SalesBilling);
        let Amounta = netAmount + AmtDicount;
        return Amounta;
    }

    // Calculate sale billing Amount
    calcaAmount(netAmount: number) {
        let GrandAmt = netAmount;
        return GrandAmt;
    }

    calcaNetAmount(netAmt: number, discountAmt: number) {
        let NetAmount = netAmt - discountAmt;
        return NetAmount;
    }

    // Calculate sale billing Grand Amount
    calculateGrandAmount(vatAmount: number, netAmount: number, discountAmt: number) {
        let GrandAmt = vatAmount + (netAmount - discountAmt) ;
        return GrandAmt;
    }

    onSubmit(fileUpload) {
        this.msg = "";
        this.formSubmitAttempt = true;
        let SalesBilling = this.salesBillingForm;
        let newdate = new Date();
        let voucherDate = new Date(SalesBilling.get('Date').value);
        voucherDate.setTime(voucherDate.getTime() - (newdate.getTimezoneOffset() * 60000));
        SalesBilling.get('Date').setValue(voucherDate);
        if (!this.voucherDateValidator(SalesBilling.get('Date'))) {
            return false;
        }

        SalesBilling.get('FinancialYear').setValue(this.currentYear['Name'] || '');
        SalesBilling.get('UserName').setValue(this.currentUser && this.currentUser['UserName'] || '');
        SalesBilling.get('CompanyCode').setValue(this.currentUser && this.company['BranchCode'] || '');

        if (SalesBilling.valid) {
            switch (this.dbops) {
                case DBOperation.create:
                    debugger
                    this._purchaseService.post(Global.BASE_SALE_BILLING_ENDPOINT, SalesBilling.value).subscribe(
                        async (data) => {
                            if (data > 0) {
                                // file upload stuff goes here
                                await fileUpload.handleFileUpload({
                                    'moduleName': 'JournalVoucher',
                                    'id': data
                                });
                                this.modalRef.hide();
                                this.reset();
                                this.loadSalesBillingList();
                            } else {
                                alert("There is some issue in creating records, please contact to system administrator!");
                            }
                            //this.modalRef.hide();
                            this.formSubmitAttempt = false;
                            //this.reset();
                        }
                    );
                    break;
                case DBOperation.update:
                    let purchaseObj = {
                        Id: this.salesBillingForm.controls['Id'].value,
                        Date: this.salesBillingForm.controls['Date'].value,
                        FinancialYear: this.salesBillingForm.controls['FinancialYear'].value,
                        Name: this.salesBillingForm.controls['Name'].value,
                        AccountTransactionDocumentId: this.salesBillingForm.controls['AccountTransactionDocumentId'].value,
                        Description: this.salesBillingForm.controls['Description'].value,
                        SourceAccountTypeId: this.salesBillingForm.controls['SourceAccountTypeId'].value,
                        Amount: this.calculateAmount(), 
                        NetAmount: this.salesBillingForm.controls['NetAmount'].value,
                        PercentAmount: this.salesBillingForm.controls['PercentAmount'].value,
                        VATAmount: this.salesBillingForm.controls['VATAmount'].value,
                        GrandAmount: this.salesBillingForm.controls['GrandAmount'].value,
                        Discount: this.salesBillingForm.controls['Discount'].value,
                        SalesOrderDetails: this.salesBillingForm.controls['SalesOrderDetails'].value,
                        AccountTransactionValues: this.salesBillingForm.controls['AccountTransactionValues'].value
                    }
                    this._purchaseService.put(Global.BASE_SALE_BILLING_ENDPOINT, SalesBilling.value.Id, purchaseObj).subscribe(
                        async (data) => {
                            if (data > 0) {
                                // file upload stuff goes here
                                await fileUpload.handleFileUpload({
                                    'moduleName': 'JournalVoucher',
                                    'id': data
                                });
                                this.modalRef.hide();
                                this.reset();
                                this.loadSalesBillingList();
                            } else {
                                alert("There is some issue in updating records, please contact to system administrator!");
                            }
                            //this.modalRef.hide();
                            this.formSubmitAttempt = false;
                            //this.reset();
                        },
                    );
                    break;
                case DBOperation.delete:
                    let purchaseObjc = {
                        Id: this.salesBillingForm.controls['Id'].value,
                        Date: this.salesBillingForm.controls['Date'].value,
                        FinancialYear: this.salesBillingForm.controls['FinancialYear'].value,
                        Name: this.salesBillingForm.controls['Name'].value,
                        AccountTransactionDocumentId: this.salesBillingForm.controls['AccountTransactionDocumentId'].value,
                        Description: this.salesBillingForm.controls['Description'].value,
                        SourceAccountTypeId: this.salesBillingForm.controls['SourceAccountTypeId'].value,
                        NetAmount: this.salesBillingForm.controls['NetAmount'].value,
                        Amount: this.salesBillingForm.controls['Amount'].value,
                        PercentAmount: this.salesBillingForm.controls['PercentAmount'].value,
                        VATAmount: this.salesBillingForm.controls['VATAmount'].value,
                        GrandAmount: this.salesBillingForm.controls['GrandAmount'].value,
                        Discount: this.salesBillingForm.controls['Discount'].value,
                        SalesOrderDetails: this.salesBillingForm.controls['SalesOrderDetails'].value,
                        AccountTransactionValues: this.salesBillingForm.controls['AccountTransactionValues'].value
                    }
                    this._purchaseService.delete(Global.BASE_SALE_BILLING_ENDPOINT, purchaseObjc).subscribe(
                        data => {
                            if (data == 1) {
                                alert("Data successfully deleted.");
                                this.loadSalesBillingList();
                            } else {
                                alert("There is some issue in deleting records, please contact to system administrator!");
                            }
                            this.modalRef.hide();
                            this.formSubmitAttempt = false;
                            this.reset();
                        }
                    );
            }
        }
        else {
            this.validateAllFields(SalesBilling);
        }
    }

    confirm(): void {
        this.modalRef2.hide();
        this.formSubmitAttempt = false;
    }

    reset() {
        this.salesBillingForm.controls['AccountTransactionDocumentId'].reset();
        this.salesBillingForm.controls['Date'].reset();
        this.salesBillingForm.controls['Discount'].reset();
        this.salesBillingForm.controls['PercentAmount'].reset();
        this.salesBillingForm.controls['NetAmount'].reset();
        this.salesBillingForm.controls['VATAmount'].reset();
        this.salesBillingForm.controls['GrandAmount'].reset(); 
        this.salesBillingForm.controls['SourceAccountTypeId'].reset(); 
        this.salesBillingForm.controls['Description'].reset();
        this.salesBillingForm.controls['SalesOrderDetails'] = this.fb.array([]);
        this.salesBillingForm.controls['AccountTransactionValues'] = this.fb.array([]);
        this.addSalesBillingitems();
    }

    SetControlsState(isEnable: boolean) {
        isEnable ? this.salesBillingForm.enable() : this.salesBillingForm.disable();
    }

    /**
     *  Get the list of filtered Purchases by the form and to date
     */
    filterPurchasesByDate() {
        this.indLoading = true;
        this._purchaseService.get(Global.BASE_SALE_BILLING_ENDPOINT + '?fromDate=' + this.date.transform(this.fromDate, 'yyyy-MM-dd') + '&toDate=' + this.date.transform(this.toDate, 'yyyy-MM-dd') + '&TransactionTypeId=' + 3)
            .subscribe(
            SalesBilling => {
                this.SalesBilling = SalesBilling;
                this.indLoading = false;
            },
            error => this.msg = <any>error
            );
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
