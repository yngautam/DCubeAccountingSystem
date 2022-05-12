"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var forms_1 = require("@angular/forms");
var enum_1 = require("../../../Shared/enum");
var global_1 = require("../../../Shared/global");
var AccountComponent = /** @class */ (function () {
    function AccountComponent(fb, accountService, modalService, date) {
        var _this = this;
        this.fb = fb;
        this.accountService = accountService;
        this.modalService = modalService;
        this.date = date;
        this.indLoading = false;
        this.accountService.getAccountTypes().subscribe(function (data) { _this.acctype = data; });
    }
    AccountComponent.prototype.ngOnInit = function () {
        this.accountLedgerFrm = this.fb.group({
            Id: [''],
            Name: ['', forms_1.Validators.required],
            AccountTypeId: [''],
            ForeignCurrencyId: [''],
            TaxClassificationName: [''],
            TaxType: [''],
            TaxRate: [''],
            GSTType: [''],
            ServiceCategory: [''],
            ExciseDutyType: [''],
            TraderLedNatureOfPurchase: [''],
            TDSDeducteeType: [''],
            TDSRateName: [''],
            LedgerFBTCategory: [''],
            IsBillWiseOn: [''],
            ISCostCentresOn: [''],
            IsInterestOn: [''],
            AllowInMobile: [''],
            IsCondensed: [''],
            AffectsStock: [''],
            ForPayRoll: [''],
            InterestOnBillWise: [''],
            OverRideInterest: [''],
            OverRideADVInterest: [''],
            IgnoreTDSExempt: [''],
            UseForVat: [''],
            IsTCSApplicable: [''],
            IsTDSApplicable: [''],
            IsFBTApplicable: [''],
            IsGSTApplicable: [''],
            ShowInPaySlip: [''],
            UseForGratuity: [''],
            ForServiceTax: [''],
            IsInputCredit: [''],
            IsExempte: [''],
            IsAbatementApplicable: [''],
            TDSDeducteeIsSpecialRate: [''],
            Audited: [],
            SortPosition: [''],
            OpeningBalance: [''],
            InventoryValue: false,
            MaintainBilByBill: false,
            Address: [''],
            District: [''],
            City: [''],
            Street: [''],
            PanNo: [''],
            Telephone: [''],
            Email: [''],
            Amount: ['']
        });
        this.LoadMasters();
    };
    AccountComponent.prototype.LoadMasters = function () {
        var _this = this;
        this.indLoading = true;
        this.accountService.get(global_1.Global.BASE_ACCOUNT_ENDPOINT)
            .subscribe(function (accounts) { _this.accounts = accounts; _this.indLoading = false; }, function (error) { return _this.msg = error; });
    };
    AccountComponent.prototype.exportTableToExcel = function (tableID, filename) {
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
        filename = filename ? filename + '.xls' : 'Account List of ' + this.date.transform(new Date, 'dd-MM-yyyy') + '.xls';
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
    AccountComponent.prototype.addAccounts = function () {
        debugger;
        this.dbops = enum_1.DBOperation.create;
        this.SetControlsState(true);
        this.modalTitle = "Add Ledger";
        this.modalBtnTitle = "Save & Submit";
        this.accountLedgerFrm.reset();
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false,
            class: 'modal-lg'
        });
    };
    AccountComponent.prototype.editAccounts = function (Id) {
        debugger;
        this.dbops = enum_1.DBOperation.update;
        this.SetControlsState(true);
        this.modalTitle = "Edit Ledger";
        this.modalBtnTitle = "Update";
        this.account = this.accounts.filter(function (x) { return x.Id == Id; })[0];
        this.accountLedgerFrm.setValue(this.account);
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false,
            class: 'modal-lg'
        });
    };
    AccountComponent.prototype.deleteAccounts = function (id) {
        this.dbops = enum_1.DBOperation.delete;
        this.SetControlsState(true);
        this.modalTitle = "Confirm to Delete Ledger?";
        this.modalBtnTitle = "Delete";
        this.account = this.accounts.filter(function (x) { return x.Id == id; })[0];
        this.accountLedgerFrm.setValue(this.account);
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false,
            class: 'modal-lg'
        });
    };
    AccountComponent.prototype.validateAllFields = function (formGroup) {
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
    AccountComponent.prototype.openModal2 = function (template) {
        this.modalRef2 = this.modalService.show(template, { class: 'modal-sm' });
    };
    AccountComponent.prototype.onSubmit = function () {
        var _this = this;
        debugger;
        this.msg = "";
        var master = this.accountLedgerFrm;
        this.formSubmitAttempt = true;
        if (master.valid) {
            switch (this.dbops) {
                case enum_1.DBOperation.create:
                    this.accountService.post(global_1.Global.BASE_ACCOUNT_ENDPOINT, master.value).subscribe(function (data) {
                        debugger;
                        if (data == 1) {
                            _this.openModal2(_this.TemplateRef2);
                            _this.LoadMasters();
                        }
                        else {
                            alert("There is some issue in saving records, please contact to system administrator!");
                        }
                        _this.modalRef.hide();
                        _this.formSubmitAttempt = false;
                    });
                case enum_1.DBOperation.update:
                    this.accountService.put(global_1.Global.BASE_ACCOUNT_ENDPOINT, master.value.Id, master.value).subscribe(function (data) {
                        if (data == 1) {
                            _this.openModal2(_this.TemplateRef2);
                            _this.LoadMasters();
                        }
                        else {
                            alert("There is some issue in saving records, please contact to system administrator!");
                        }
                        _this.modalRef.hide();
                        _this.formSubmitAttempt = false;
                    });
                    break;
                case enum_1.DBOperation.delete:
                    this.accountService.delete(global_1.Global.BASE_ACCOUNT_ENDPOINT, master.value.Id).subscribe(function (data) {
                        if (data == 1) {
                            alert("Data deleted sucessfully");
                            _this.LoadMasters();
                        }
                        else {
                            alert("There is some issue in saving records, please contact to system administrator!");
                        }
                        _this.modalRef.hide();
                        _this.formSubmitAttempt = false;
                    });
            }
        }
        else {
            this.validateAllFields(master);
        }
    };
    AccountComponent.prototype.confirm = function () {
        this.modalRef2.hide();
        this.formSubmitAttempt = false;
    };
    AccountComponent.prototype.reset = function () {
        var control = this.accountLedgerFrm.controls['Id'].value;
        if (control > 0) {
            this.buttonDisabled = true;
        }
        else {
            this.accountLedgerFrm.reset();
        }
    };
    AccountComponent.prototype.SetControlsState = function (isEnable) {
        isEnable ? this.accountLedgerFrm.enable() : this.accountLedgerFrm.disable();
    };
    __decorate([
        core_1.ViewChild('template')
    ], AccountComponent.prototype, "TemplateRef", void 0);
    __decorate([
        core_1.ViewChild('templateNested')
    ], AccountComponent.prototype, "TemplateRef2", void 0);
    AccountComponent = __decorate([
        core_1.Component({
            templateUrl: './account.component.html'
        })
    ], AccountComponent);
    return AccountComponent;
}());
exports.AccountComponent = AccountComponent;
