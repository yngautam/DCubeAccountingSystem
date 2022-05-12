"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var forms_1 = require("@angular/forms");
var enum_1 = require("../../../Shared/enum");
var global_1 = require("../../../Shared/global");
var SalesBillingComponent = /** @class */ (function () {
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
    function SalesBillingComponent(fb, _purchaseService, _purchaseDetailsService, _accountTransValues, _customerService, date, modalService, fileService) {
        this.fb = fb;
        this._purchaseService = _purchaseService;
        this._purchaseDetailsService = _purchaseDetailsService;
        this._accountTransValues = _accountTransValues;
        this._customerService = _customerService;
        this.date = date;
        this.modalService = modalService;
        this.fileService = fileService;
        this.selectedValue = '0.00';
        this.indLoading = false;
        this.currentYear = {};
        this.currentUser = {};
        this.company = {};
        this.dropMessage = "Upload Reference File";
        this.toExportFileName = 'Sales Voucher of ' + this.date.transform(new Date, "dd-MM-yyyy") + '.xls';
        this.uploadUrl = global_1.Global.BASE_FILE_UPLOAD_ENDPOINT;
        this.fileUrl = '';
        this.settings = {
            bigBanner: false,
            timePicker: false,
            format: 'dd/MM/yyyy',
            defaultOpen: false
        };
        this.currentYear = JSON.parse(localStorage.getItem('currentYear'));
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.company = JSON.parse(localStorage.getItem('company'));
        this.fromDate = new Date(this.currentYear['StartDate']);
        this.toDate = new Date(this.currentYear['EndDate']);
    }
    SalesBillingComponent.prototype.ngOnInit = function () {
        this.salesBillingForm = this.fb.group({
            Id: [''],
            Name: [''],
            AccountTransactionDocumentId: [''],
            Date: [new Date(), forms_1.Validators.required],
            AccountTransactionTypeId: [''],
            SourceAccountTypeId: ['', forms_1.Validators.required],
            Description: ['', forms_1.Validators.required],
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
    };
    SalesBillingComponent.prototype.LoadCustomers = function () {
        var _this = this;
        debugger;
        this.indLoading = true;
        this._customerService.get(global_1.Global.BASE_SCREENCustomerTicket_ENDPOINT)
            .subscribe(function (customers) {
            _this.accounts = customers;
            _this.indLoading = false;
        }, function (error) { return console.log(error); });
    };
    SalesBillingComponent.prototype.voucherDateValidator = function (control) {
        var today = new Date;
        var tomorrow = new Date(today.setDate(today.getDate() + 1));
        if (!control.value) {
            alert("Please select the Voucher Date");
            return false;
        }
        var voucherDate = new Date(control.value);
        var currentYearStartDate = new Date(this.currentYear.StartDate);
        var currentYearEndDate = new Date(this.currentYear.EndDate);
        if ((voucherDate < currentYearStartDate) || (voucherDate > currentYearEndDate) || voucherDate >= tomorrow) {
            alert("Date should be within current financial year's start date and end date inclusive");
            return false;
        }
        else {
            return true;
        }
    };
    SalesBillingComponent.prototype.viewFile = function (fileUrl, template) {
        this.fileUrl = fileUrl;
        this.modalTitle = "View Attachment";
        this.modalRef = this.modalService.show(template, { keyboard: false, class: 'modal-lg' });
    };
    /**
     * Exports the pOrder voucher data in CSV/ Excel format
     */
    SalesBillingComponent.prototype.exportTableToExcel = function (tableID, filename) {
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
        filename = filename ? filename + '.xls' : this.toExportFileName;
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
    SalesBillingComponent.prototype.loadSalesBillingList = function () {
        var _this = this;
        this.indLoading = true;
        this._purchaseService.get(global_1.Global.BASE_SALE_BILLING_ENDPOINT + '?fromDate=' + this.date.transform(this.fromDate, 'yyyy-MM-dd') + '&toDate=' + this.date.transform(this.toDate, 'yyyy-MM-dd') + '&TransactionTypeId=' + 3)
            .subscribe(function (SalesBilling) {
            SalesBilling.map(function (purch) { return purch['File'] = global_1.Global.BASE_HOST_ENDPOINT + global_1.Global.BASE_FILE_UPLOAD_ENDPOINT + '?Id=' + purch.Id + '&ApplicationModule=JournalVoucher'; });
            _this.SalesBilling = SalesBilling;
            _this.indLoading = false;
        }, function (error) { return _this.msg = error; });
    };
    SalesBillingComponent.prototype.addSalesBilling = function () {
        this.dbops = enum_1.DBOperation.create;
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
    };
    SalesBillingComponent.prototype.getSalesBilling = function (Id) {
        this.indLoading = true;
        return this._purchaseService.get(global_1.Global.BASE_SALE_BILLING_ENDPOINT + '?TransactionId=' + Id);
    };
    /**
     * Opens Edit Existing Journal Voucher Form Modal
     * @param Id
     */
    SalesBillingComponent.prototype.editSalesBilling = function (Id) {
        var _this = this;
        this.reset();
        this.dbops = enum_1.DBOperation.update;
        this.SetControlsState(true);
        this.modalTitle = "Edit Sales Billing";
        this.modalBtnTitle = "Update";
        this.getSalesBilling(Id)
            .subscribe(function (SalesBilling) {
            debugger;
            _this.indLoading = false;
            _this.salesBillingForm.controls['Id'].setValue(SalesBilling.Id);
            _this.salesBillingForm.controls['Date'].setValue(new Date(SalesBilling.Date));
            _this.salesBillingForm.controls['Name'].setValue(SalesBilling.Name);
            _this.salesBillingForm.controls['AccountTransactionDocumentId'].setValue(SalesBilling.AccountTransactionDocumentId);
            _this.salesBillingForm.controls['SourceAccountTypeId'].setValue(SalesBilling.SourceAccountTypeId);
            _this.salesBillingForm.controls['Description'].setValue(SalesBilling.Description);
            _this.salesBillingForm.controls['Discount'].setValue(SalesBilling.Discount);
            _this.salesBillingForm.controls['NetAmount'].setValue(SalesBilling.NetAmount);
            _this.salesBillingForm.controls['Amount'].setValue(SalesBilling.Discount + SalesBilling.NetAmount);
            _this.salesBillingForm.controls['PercentAmount'].setValue(SalesBilling.PercentAmount);
            _this.salesBillingForm.controls['VATAmount'].setValue(SalesBilling.VATAmount);
            _this.salesBillingForm.controls['GrandAmount'].setValue(SalesBilling.VATAmount + SalesBilling.NetAmount);
            _this.salesBillingForm.controls['SalesOrderDetails'] = _this.fb.array([]);
            var control = _this.salesBillingForm.controls['SalesOrderDetails'];
            for (var i = 0; i < SalesBilling.SalesOrderDetails.length; i++) {
                control.push(_this.fb.group(SalesBilling.SalesOrderDetails[i]));
            }
            _this.salesBillingForm.controls['AccountTransactionValues'] = _this.fb.array([]);
            var controlAc = _this.salesBillingForm.controls['AccountTransactionValues'];
            controlAc.controls = [];
            for (var i = 0; i < SalesBilling.AccountTransactionValues.length; i++) {
                var valuesFromServer = SalesBilling.AccountTransactionValues[i];
                var instance = _this.fb.group(valuesFromServer);
                if (valuesFromServer['entityLists'] === "Dr") {
                    instance.controls['Credit'].disable();
                }
                else if (valuesFromServer['entityLists'] === "Cr") {
                    instance.controls['Debit'].disable();
                }
                controlAc.push(instance);
            }
            _this.modalRef = _this.modalService.show(_this.TemplateRef, {
                backdrop: 'static',
                keyboard: false,
                class: 'modal-lg'
            });
        });
    };
    SalesBillingComponent.prototype.deleteSalesBilling = function (Id) {
        var _this = this;
        this.dbops = enum_1.DBOperation.delete;
        this.SetControlsState(true);
        this.modalTitle = "Delete Sales Items";
        this.modalBtnTitle = "Delete";
        this.reset();
        this.getSalesBilling(Id)
            .subscribe(function (SalesBilling) {
            debugger;
            _this.indLoading = false;
            _this.salesBillingForm.controls['Id'].setValue(SalesBilling.Id);
            _this.salesBillingForm.controls['Date'].setValue(new Date(SalesBilling.Date));
            _this.salesBillingForm.controls['Name'].setValue(SalesBilling.Name);
            _this.salesBillingForm.controls['AccountTransactionDocumentId'].setValue(SalesBilling.AccountTransactionDocumentId);
            _this.salesBillingForm.controls['SourceAccountTypeId'].setValue(SalesBilling.SourceAccountTypeId);
            _this.salesBillingForm.controls['Description'].setValue(SalesBilling.Description);
            _this.salesBillingForm.controls['Discount'].setValue(SalesBilling.Discount);
            _this.salesBillingForm.controls['Amount'].setValue(SalesBilling.Discount + SalesBilling.NetAmount);
            _this.salesBillingForm.controls['NetAmount'].setValue(SalesBilling.NetAmount);
            _this.salesBillingForm.controls['PercentAmount'].setValue(SalesBilling.PercentAmount);
            _this.salesBillingForm.controls['VATAmount'].setValue(SalesBilling.VATAmount);
            _this.salesBillingForm.controls['GrandAmount'].setValue(SalesBilling.VATAmount + SalesBilling.NetAmount);
            _this.salesBillingForm.controls['SalesOrderDetails'] = _this.fb.array([]);
            var control = _this.salesBillingForm.controls['SalesOrderDetails'];
            for (var i = 0; i < SalesBilling.SalesOrderDetails.length; i++) {
                control.push(_this.fb.group(SalesBilling.SalesOrderDetails[i]));
            }
            _this.salesBillingForm.controls['AccountTransactionValues'] = _this.fb.array([]);
            var controlAc = _this.salesBillingForm.controls['AccountTransactionValues'];
            controlAc.controls = [];
            for (var i = 0; i < SalesBilling.AccountTransactionValues.length; i++) {
                var valuesFromServer = SalesBilling.AccountTransactionValues[i];
                var instance = _this.fb.group(valuesFromServer);
                if (valuesFromServer['entityLists'] === "Dr") {
                    instance.controls['Credit'].disable();
                }
                else if (valuesFromServer['entityLists'] === "Cr") {
                    instance.controls['Debit'].disable();
                }
                controlAc.push(instance);
            }
            _this.modalRef = _this.modalService.show(_this.TemplateRef, {
                backdrop: 'static',
                keyboard: false,
                class: 'modal-lg'
            });
        });
    };
    // Initialize the formb uilder arrays
    SalesBillingComponent.prototype.initSalesOrderDetails = function () {
        return this.fb.group({
            Id: [''],
            IsSelected: '',
            IsVoid: '',
            ItemId: ['', forms_1.Validators.required],
            OrderId: '',
            OrderNumber: '',
            Qty: ['', forms_1.Validators.required],
            Tags: '',
            UnitPrice: ['', forms_1.Validators.required],
            TotalAmount: [''],
        });
    };
    SalesBillingComponent.prototype.initJournalDetail = function () {
        return this.fb.group({
            entityLists: ['', forms_1.Validators.required],
            AccountId: ['', forms_1.Validators.required],
            Debit: ['', forms_1.Validators.required],
            Credit: ['', forms_1.Validators.required],
            Description: [''],
        });
    };
    // Push the values of purchasdetails
    SalesBillingComponent.prototype.addSalesBillingitems = function () {
        var control = this.salesBillingForm.controls['SalesOrderDetails'];
        var addPurchaseValues = this.initSalesOrderDetails();
        control.push(addPurchaseValues);
    };
    //remove the rows//
    SalesBillingComponent.prototype.removeSalesBillingitems = function (i) {
        var controls = this.salesBillingForm.controls['SalesOrderDetails'];
        var controlToRemove = this.salesBillingForm.controls.SalesOrderDetails['controls'][i].controls;
        var selectedControl = controlToRemove.hasOwnProperty('Id') ? controlToRemove.Id.value : 0;
        if (selectedControl) {
            this._purchaseDetailsService.delete(global_1.Global.BASE_SALE_BILLING_DETAILS_ENDPOINT, controlToRemove.Id.value).subscribe(function (data) {
                alert("Data With Id= " + controlToRemove.Id.value + " Removed Successfully!");
                (data == 1) && controls.removeAt(i);
            });
        }
        else {
            if (i >= 0) {
                controls.removeAt(i);
            }
            else {
                alert("Form requires at least one row");
            }
        }
    };
    SalesBillingComponent.prototype.calculateAmount = function () {
        var controls = this.salesBillingForm.controls['SalesOrderDetails'].value;
        return controls.reduce(function (total, accounts) {
            return (accounts.TotalAmount) ? (total + Math.round(accounts.TotalAmount)) : total;
        }, 0);
    };
    SalesBillingComponent.prototype.calculateNetAmount = function (salesBillingForm) {
        this.getDiscountPercent(salesBillingForm);
        var totalAmt = this.calculateAmount();
        var calcNetAmount = salesBillingForm.NetAmount.setValue((totalAmt - (salesBillingForm.Discount.value)).toFixed(2));
        return calcNetAmount;
    };
    SalesBillingComponent.prototype.calculateVATAmount = function (salesBillingForm) {
        this.getDiscountPercent(salesBillingForm);
        var totalAmt = this.calculateAmount();
        var calcVatAmt = salesBillingForm.VATAmount.setValue(((totalAmt - (salesBillingForm.Discount.value)) * 0.13).toFixed(2));
        return calcVatAmt;
    };
    SalesBillingComponent.prototype.calculateGndAmount = function (salesBillingForm) {
        this.getDiscountPercent(salesBillingForm);
        var totalAmt = this.calculateAmount();
        var Discount = this.salesBillingForm.controls['Discount'];
        var IsDiscountPercentage = this.salesBillingForm.controls['IsDiscountPercentage'];
        var PercentAmount = this.salesBillingForm.controls['PercentAmount'];
        var calcGrandTot = 0;
        if (IsDiscountPercentage.value == true) {
            calcGrandTot = salesBillingForm.GrandAmount.setValue(((totalAmt - (((Discount.value / 100) * totalAmt))) + ((totalAmt - (((Discount.value / 100) * totalAmt))) * 0.13)).toFixed(2));
        }
        else {
            calcGrandTot = salesBillingForm.GrandAmount.setValue(((totalAmt - (salesBillingForm.Discount.value)) + ((totalAmt - (salesBillingForm.Discount.value)) * 0.13)).toFixed(2));
        }
        console.log(calcGrandTot);
        return calcGrandTot;
    };
    SalesBillingComponent.prototype.calculatePercentDiscountAmount = function (salesBillingForm) {
        var IsDiscountPercentage = this.salesBillingForm.controls['IsDiscountPercentage'];
        if (salesBillingForm.Discount.value <= this.calculateAmount() && IsDiscountPercentage.value == false) {
            var totalAmt = this.calculateAmount();
            var calcNetAmount = salesBillingForm.NetAmount.setValue(totalAmt - (salesBillingForm.Discount.value));
            var calcVatAmt = salesBillingForm.VATAmount.setValue((totalAmt - (salesBillingForm.Discount.value)) * 0.13);
            var calcGrandTot = salesBillingForm.GrandAmount.setValue((totalAmt - (salesBillingForm.Discount.value)) + ((totalAmt - (salesBillingForm.Discount.value)) * 0.13));
            return ((calcNetAmount) & (calcVatAmt) & (calcGrandTot)).toFixed(2);
        }
        else {
            alert("Entered value is greater than total amount ? " + "Your entered value is = " + salesBillingForm.Discount.value);
            return false;
        }
    };
    //Calculate discount for more than 0% or less than 100%
    SalesBillingComponent.prototype.getDiscountPercent = function (salesBillingForm) {
        var IsDiscountPercentage = this.salesBillingForm.controls['IsDiscountPercentage'];
        var Discount = this.salesBillingForm.controls['Discount'];
        var PercentAmount = this.salesBillingForm.controls['PercentAmount'];
        var NetAmount = this.salesBillingForm.controls['NetAmount'];
        var VATAmount = this.salesBillingForm.controls['VATAmount'];
        var GrandAmount = this.salesBillingForm.controls['GrandAmount'];
        if (Discount.value > 0 && Discount.value <= 100 && IsDiscountPercentage.value == true) {
            var totalAmt = this.calculateAmount();
            var calcPercentAmount = ((Discount.value / 100) * totalAmt).toFixed(2);
            var calcNetAmount = (totalAmt - (((Discount.value / 100) * totalAmt))).toFixed(2);
            var calcVatAmt = ((totalAmt - (((Discount.value / 100) * totalAmt))) * 0.13).toFixed(2);
            var calcGrandTot = ((totalAmt - (((Discount.value / 100) * totalAmt))) + ((totalAmt - (((Discount.value / 100) * totalAmt))) * 0.13)).toFixed(2);
            salesBillingForm.PercentAmount.setValue(calcPercentAmount);
            salesBillingForm.NetAmount.setValue(calcNetAmount);
            salesBillingForm.VATAmount.setValue(calcVatAmt);
            salesBillingForm.GrandAmount.setValue(calcGrandTot);
        }
        //else {
        //    alert("Entered value is greater than 100% ? " + "Your entered value is = " + Discount.value);
        //    return false;
        //}
    };
    SalesBillingComponent.prototype.validateAllFields = function (formGroup) {
        var _this = this;
        Object.keys(formGroup.controls).forEach(function (field) {
            var control = formGroup.get(field);
            if (control instanceof forms_1.FormControl) {
                control.markAsTouched({ onlySelf: true });
            }
            else if (control instanceof forms_1.FormGroup) {
                _this.validateAllFields(control);
            }
        });
    };
    //Opens confirm window modal//
    SalesBillingComponent.prototype.openModal2 = function (template) {
        this.modalRef2 = this.modalService.show(template, { class: 'modal-sm' });
    };
    SalesBillingComponent.prototype.calcDiscountTotal = function (SalesBilling) {
        var Discount = 0;
        for (var i = 0; i < SalesBilling.length; i++) {
            Discount = Discount + parseFloat(SalesBilling[i].Discount);
        }
        return Discount;
    };
    SalesBillingComponent.prototype.calcNetAmount = function (SalesBilling) {
        var NetAmount = 0;
        for (var i = 0; i < SalesBilling.length; i++) {
            NetAmount = NetAmount + parseFloat(SalesBilling[i].NetAmount);
        }
        return NetAmount;
    };
    SalesBillingComponent.prototype.calcVATAmount = function (SalesBilling) {
        var VATAmount = 0;
        for (var i = 0; i < SalesBilling.length; i++) {
            VATAmount = VATAmount + parseFloat(SalesBilling[i].VATAmount);
        }
        return VATAmount;
    };
    SalesBillingComponent.prototype.calcGrandAmount = function (SalesBilling) {
        var netAmount = this.calcNetAmount(SalesBilling);
        var vatAmount = this.calcVATAmount(SalesBilling);
        var GrandAmt = netAmount + vatAmount;
        return GrandAmt;
    };
    SalesBillingComponent.prototype.calcAmount = function (SalesBilling) {
        var netAmount = this.calcNetAmount(SalesBilling);
        var AmtDicount = this.calcDiscountTotal(SalesBilling);
        var Amounta = netAmount + AmtDicount;
        return Amounta;
    };
    // Calculate sale billing Amount
    SalesBillingComponent.prototype.calcaAmount = function (netAmount) {
        var GrandAmt = netAmount;
        return GrandAmt;
    };
    SalesBillingComponent.prototype.calcaNetAmount = function (netAmt, discountAmt) {
        var NetAmount = netAmt - discountAmt;
        return NetAmount;
    };
    // Calculate sale billing Grand Amount
    SalesBillingComponent.prototype.calculateGrandAmount = function (vatAmount, netAmount, discountAmt) {
        var GrandAmt = vatAmount + (netAmount - discountAmt);
        return GrandAmt;
    };
    SalesBillingComponent.prototype.onSubmit = function (fileUpload) {
        var _this = this;
        this.msg = "";
        this.formSubmitAttempt = true;
        var SalesBilling = this.salesBillingForm;
        var newdate = new Date();
        var voucherDate = new Date(SalesBilling.get('Date').value);
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
                case enum_1.DBOperation.create:
                    debugger;
                    this._purchaseService.post(global_1.Global.BASE_SALE_BILLING_ENDPOINT, SalesBilling.value).subscribe(function (data) { return __awaiter(_this, void 0, void 0, function () {
                        return __generator(this, function (_a) {
                            switch (_a.label) {
                                case 0:
                                    if (!(data > 0)) return [3 /*break*/, 2];
                                    // file upload stuff goes here
                                    return [4 /*yield*/, fileUpload.handleFileUpload({
                                            'moduleName': 'JournalVoucher',
                                            'id': data
                                        })];
                                case 1:
                                    // file upload stuff goes here
                                    _a.sent();
                                    this.modalRef.hide();
                                    this.reset();
                                    this.loadSalesBillingList();
                                    return [3 /*break*/, 3];
                                case 2:
                                    alert("There is some issue in creating records, please contact to system administrator!");
                                    _a.label = 3;
                                case 3:
                                    //this.modalRef.hide();
                                    this.formSubmitAttempt = false;
                                    return [2 /*return*/];
                            }
                        });
                    }); });
                    break;
                case enum_1.DBOperation.update:
                    var purchaseObj = {
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
                    };
                    this._purchaseService.put(global_1.Global.BASE_SALE_BILLING_ENDPOINT, SalesBilling.value.Id, purchaseObj).subscribe(function (data) { return __awaiter(_this, void 0, void 0, function () {
                        return __generator(this, function (_a) {
                            switch (_a.label) {
                                case 0:
                                    if (!(data > 0)) return [3 /*break*/, 2];
                                    // file upload stuff goes here
                                    return [4 /*yield*/, fileUpload.handleFileUpload({
                                            'moduleName': 'JournalVoucher',
                                            'id': data
                                        })];
                                case 1:
                                    // file upload stuff goes here
                                    _a.sent();
                                    this.modalRef.hide();
                                    this.reset();
                                    this.loadSalesBillingList();
                                    return [3 /*break*/, 3];
                                case 2:
                                    alert("There is some issue in updating records, please contact to system administrator!");
                                    _a.label = 3;
                                case 3:
                                    //this.modalRef.hide();
                                    this.formSubmitAttempt = false;
                                    return [2 /*return*/];
                            }
                        });
                    }); });
                    break;
                case enum_1.DBOperation.delete:
                    var purchaseObjc = {
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
                    };
                    this._purchaseService.delete(global_1.Global.BASE_SALE_BILLING_ENDPOINT, purchaseObjc).subscribe(function (data) {
                        if (data == 1) {
                            alert("Data successfully deleted.");
                            _this.loadSalesBillingList();
                        }
                        else {
                            alert("There is some issue in deleting records, please contact to system administrator!");
                        }
                        _this.modalRef.hide();
                        _this.formSubmitAttempt = false;
                        _this.reset();
                    });
            }
        }
        else {
            this.validateAllFields(SalesBilling);
        }
    };
    SalesBillingComponent.prototype.confirm = function () {
        this.modalRef2.hide();
        this.formSubmitAttempt = false;
    };
    SalesBillingComponent.prototype.reset = function () {
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
    };
    SalesBillingComponent.prototype.SetControlsState = function (isEnable) {
        isEnable ? this.salesBillingForm.enable() : this.salesBillingForm.disable();
    };
    /**
     *  Get the list of filtered Purchases by the form and to date
     */
    SalesBillingComponent.prototype.filterPurchasesByDate = function () {
        var _this = this;
        this.indLoading = true;
        this._purchaseService.get(global_1.Global.BASE_SALE_BILLING_ENDPOINT + '?fromDate=' + this.date.transform(this.fromDate, 'yyyy-MM-dd') + '&toDate=' + this.date.transform(this.toDate, 'yyyy-MM-dd') + '&TransactionTypeId=' + 3)
            .subscribe(function (SalesBilling) {
            _this.SalesBilling = SalesBilling;
            _this.indLoading = false;
        }, function (error) { return _this.msg = error; });
    };
    SalesBillingComponent.prototype.onFilterDateSelect = function (selectedDate) {
        var currentYearStartDate = new Date(this.currentYear.StartDate);
        var currentYearEndDate = new Date(this.currentYear.EndDate);
        if (selectedDate < currentYearStartDate) {
            this.fromDate = currentYearStartDate;
            alert("Date should not be less than current financial year's start date");
        }
        if (selectedDate > currentYearEndDate) {
            this.toDate = currentYearEndDate;
            alert("Date should not be greater than current financial year's end date");
        }
    };
    __decorate([
        core_1.ViewChild("template")
    ], SalesBillingComponent.prototype, "TemplateRef", void 0);
    __decorate([
        core_1.ViewChild('templateNested')
    ], SalesBillingComponent.prototype, "TemplateRef2", void 0);
    __decorate([
        core_1.ViewChild('fileInput')
    ], SalesBillingComponent.prototype, "fileInput", void 0);
    SalesBillingComponent = __decorate([
        core_1.Component({
            templateUrl: './sales-billing.component.html',
            styleUrls: ['./sales-billing.component.css']
        })
    ], SalesBillingComponent);
    return SalesBillingComponent;
}());
exports.SalesBillingComponent = SalesBillingComponent;
