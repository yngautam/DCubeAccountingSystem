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
var JournalVouchercomponent = /** @class */ (function () {
    function JournalVouchercomponent(fb, _journalvoucherService, _accountTransValues, date, modalService, fileService) {
        var _this = this;
        this.fb = fb;
        this._journalvoucherService = _journalvoucherService;
        this._accountTransValues = _accountTransValues;
        this.date = date;
        this.modalService = modalService;
        this.fileService = fileService;
        this.indLoading = false;
        this.dropMessage = "Upload Reference File";
        this.toExportData = [
            ["Journal Voucher of " + this.date.transform(new Date, "dd-MM-yyyy")],
            ['Date', 'Particular', 'Voucher Type', 'Voucher No.', 'Debit Amount', 'Credit Amount']
        ];
        this.toExportFileName = 'Journal-voucher-' + this.date.transform(new Date, "dd-MM-yyyy") + '.xlsx';
        this.uploadUrl = global_1.Global.BASE_FILE_UPLOAD_ENDPOINT;
        this.fileUrl = '';
        this.settings = {
            bigBanner: false,
            timePicker: false,
            format: 'dd/MM/yyyy',
            defaultOpen: false
        };
        this.currentYear = {};
        this.currentUser = {};
        this.company = {};
        this.currentYear = JSON.parse(localStorage.getItem('currentYear'));
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.company = JSON.parse(localStorage.getItem('company'));
        this.fromDate = new Date(this.currentYear['StartDate']);
        this.toDate = new Date(this.currentYear['EndDate']);
        this.entityLists = [
            { id: 0, name: 'Dr' },
            { id: 1, name: 'Cr' }
        ];
        this._journalvoucherService.getAccounts()
            .subscribe(function (accountsList) { _this.account = accountsList; });
    }
    JournalVouchercomponent.prototype.ngOnInit = function () {
        // Initialize reactive form 
        this.journalFrm = this.fb.group({
            Id: [''],
            Name: ['',],
            AccountTransactionDocumentId: [''],
            Description: [''],
            Amount: [''],
            Date: ['', forms_1.Validators.required],
            drTotal: [''],
            crTotal: [''],
            AccountTransactionValues: this.fb.array([this.initAccountValue()]),
            FinancialYear: [''],
            UserName: [''],
            CompanyCode: ['']
        });
        // Load list of journal vouchers
        this.loadJournalVoucherList();
    };
    /**
     * Display file in modal
     * @param fileUrl
     * @param template
     */
    JournalVouchercomponent.prototype.viewFile = function (fileUrl, template) {
        this.fileUrl = fileUrl;
        this.modalTitle = "View Attachment";
        this.modalRef = this.modalService.show(template, { keyboard: false, class: 'modal-lg' });
    };
    /**
     * Export formatter table in excel
     * @param tableID
     * @param filename
     */
    JournalVouchercomponent.prototype.exportTableToExcel = function (tableID, filename) {
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
        filename = filename ? filename + '.xls' : 'Journal Voucher of ' + this.date.transform(new Date, 'dd-MM-yyyy') + '.xls';
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
    JournalVouchercomponent.prototype.voucherDateValidator = function (control) {
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
    /**
     * Load list of journal vouchers form the server
     */
    JournalVouchercomponent.prototype.loadJournalVoucherList = function () {
        var _this = this;
        this.indLoading = true;
        this._journalvoucherService.get(global_1.Global.BASE_JOURNALVOUCHER_ENDPOINT + '?fromDate=' + this.date.transform(this.fromDate, 'yyyy-MM-dd') + '&toDate=' + this.date.transform(this.toDate, 'yyyy-MM-dd') + '&TransactionTypeId=' + 5)
            .subscribe(function (journalVoucher) {
            _this.indLoading = false;
            journalVoucher.map(function (voucher) { return voucher['File'] = global_1.Global.BASE_HOST_ENDPOINT + global_1.Global.BASE_FILE_UPLOAD_ENDPOINT + '?Id=' + voucher.Id + '&ApplicationModule=JournalVoucher'; });
            return _this.journalVoucher = journalVoucher;
        }, function (error) { return _this.msg = error; });
    };
    /**
     * Open Add New Journal Voucher Form Modal
     */
    JournalVouchercomponent.prototype.addJournalVoucher = function () {
        this.dbops = enum_1.DBOperation.create;
        this.SetControlsState(true);
        this.modalTitle = "Add Journal Voucher";
        this.modalBtnTitle = "Save & Submit";
        this.reset();
        this.journalFrm.controls['Name'].setValue("Journal");
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false,
            class: 'modal-lg'
        });
    };
    /**
     * Gets individual journal voucher
     * @param Id
     */
    JournalVouchercomponent.prototype.getJournalVoucher = function (Id) {
        this.indLoading = true;
        return this._journalvoucherService.get(global_1.Global.BASE_JOURNALVOUCHER_ENDPOINT + '?TransactionId=' + Id);
    };
    /**
     * Opens Edit Existing Journal Voucher Form Modal
     * @param Id {String} Voucher Id
     */
    JournalVouchercomponent.prototype.editJournalVoucher = function (Id) {
        var _this = this;
        this.dbops = enum_1.DBOperation.update;
        this.SetControlsState(true);
        this.modalTitle = "Edit";
        this.modalBtnTitle = "Update";
        this.reset();
        this.getJournalVoucher(Id)
            .subscribe(function (journalVoucher) {
            _this.indLoading = false;
            _this.journalFrm.controls['Id'].setValue(journalVoucher.Id);
            _this.journalFrm.controls['Name'].setValue(journalVoucher.Name);
            _this.journalFrm.controls['Date'].setValue(new Date(journalVoucher.AccountTransactionValues[0]['Date']));
            _this.journalFrm.controls['AccountTransactionDocumentId'].setValue(journalVoucher.AccountTransactionDocumentId);
            _this.journalFrm.controls['Description'].setValue(journalVoucher.Description);
            _this.journalFrm.controls['Amount'].setValue(journalVoucher.Amount);
            _this.journalFrm.controls['drTotal'].setValue(journalVoucher.drTotal);
            _this.journalFrm.controls['crTotal'].setValue(journalVoucher.crTotal);
            _this.journalFrm.controls['AccountTransactionValues'] = _this.fb.array([]);
            var control = _this.journalFrm.controls['AccountTransactionValues'];
            for (var i = 0; i < journalVoucher.AccountTransactionValues.length; i++) {
                var valuesFromServer = journalVoucher.AccountTransactionValues[i];
                var instance = _this.fb.group(valuesFromServer);
                if (valuesFromServer['entityLists'] === "Dr") {
                    instance.controls['Credit'].disable();
                }
                if (valuesFromServer['entityLists'] === "Cr") {
                    instance.controls['Debit'].disable();
                }
                control.push(instance);
            }
            _this.modalRef = _this.modalService.show(_this.TemplateRef, {
                backdrop: 'static',
                keyboard: false,
                class: 'modal-lg'
            });
        }, function (error) { return _this.msg = error; });
    };
    /**
     * Delete Existing Journal Voucher
     * @param id
     */
    JournalVouchercomponent.prototype.deleteJournalVoucher = function (id) {
        var _this = this;
        this.dbops = enum_1.DBOperation.delete;
        this.SetControlsState(true);
        this.modalTitle = "Confirm to Delete?";
        this.modalBtnTitle = "Delete";
        this.getJournalVoucher(id)
            .subscribe(function (journalVoucher) {
            _this.indLoading = false;
            _this.journalFrm.controls['Id'].setValue(journalVoucher.Id);
            _this.journalFrm.controls['Name'].setValue(journalVoucher.Name);
            _this.journalFrm.controls['AccountTransactionDocumentId'].setValue(journalVoucher.AccountTransactionDocumentId);
            _this.journalFrm.controls['Description'].setValue(journalVoucher.Description);
            _this.journalFrm.controls['Amount'].setValue(journalVoucher.Amount);
            _this.journalFrm.controls['drTotal'].setValue(journalVoucher.drTotal);
            _this.journalFrm.controls['crTotal'].setValue(journalVoucher.crTotal);
            _this.journalFrm.controls['Date'].setValue(new Date(journalVoucher.AccountTransactionValues[0]['Date']));
            _this.journalFrm.controls['AccountTransactionValues'] = _this.fb.array([]);
            var control = _this.journalFrm.controls['AccountTransactionValues'];
            for (var i = 0; i < journalVoucher.AccountTransactionValues.length; i++) {
                var valuesFromServer = journalVoucher.AccountTransactionValues[i];
                var instance = _this.fb.group(valuesFromServer);
                if (valuesFromServer['entityLists'] === "Dr") {
                    instance.controls['Credit'].disable();
                }
                else if (valuesFromServer['entityLists'] === "Cr") {
                    instance.controls['Debit'].disable();
                }
                control.push(instance);
            }
            _this.modalRef = _this.modalService.show(_this.TemplateRef, {
                backdrop: 'static',
                keyboard: false,
                class: 'modal-lg'
            });
        });
    };
    /**
     * Initializes Account values
     */
    JournalVouchercomponent.prototype.initAccountValue = function () {
        //initialize our vouchers
        return this.fb.group({
            entityLists: ['', forms_1.Validators.required],
            AccountId: ['', forms_1.Validators.required],
            Debit: ['', forms_1.Validators.required],
            Credit: ['', forms_1.Validators.required],
            Description: ['']
        });
    };
    // Push Account Values in row
    JournalVouchercomponent.prototype.addAccountValues = function () {
        var control = this.journalFrm.controls['AccountTransactionValues'];
        var addJournalVoucher = this.initAccountValue();
        control.push(addJournalVoucher);
    };
    //remove the rows//
    JournalVouchercomponent.prototype.removeAccount = function (i) {
        var controls = this.journalFrm.controls['AccountTransactionValues'];
        var controlToRemove = this.journalFrm.controls.AccountTransactionValues['controls'][i].controls;
        var selectedControl = controlToRemove.hasOwnProperty('Id') ? controlToRemove.Id.value : 0;
        if (selectedControl) {
            this._accountTransValues.delete(global_1.Global.BASE_JOURNAL_ENDPOINT, i).subscribe(function (data) {
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
    //calulate the sum of debit columns//
    JournalVouchercomponent.prototype.sumDebit = function () {
        var controls = this.journalFrm.controls.AccountTransactionValues.value;
        return controls.reduce(function (total, accounts) {
            return (accounts.Debit) ? (total + Math.round(accounts.Debit)) : total;
        }, 0);
    };
    //calculate the sum of credit columns//
    JournalVouchercomponent.prototype.sumCredit = function () {
        var controls = this.journalFrm.controls.AccountTransactionValues.value;
        return controls.reduce(function (total, accounts) {
            return (accounts.Credit) ? (total + Math.round(accounts.Credit)) : total;
        }, 0);
    };
    /**
     * Validate fields
     * @param formGroup
     */
    JournalVouchercomponent.prototype.validateAllFields = function (formGroup) {
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
    /**
     * Open Modal
     * @param template
     */
    JournalVouchercomponent.prototype.openModal2 = function (template) {
        this.modalRef2 = this.modalService.show(template, { class: 'modal-sm' });
    };
    /**
     * Enable or Disable the form fields
     * @param data
     */
    JournalVouchercomponent.prototype.enableDisable = function (data) {
        if (data.entityLists.value == 'Dr') {
            data.Debit.enable();
            data.Credit.disable();
            data.Credit.reset();
        }
        else if (data.entityLists.value == 'Cr') {
            data.Credit.enable();
            data.Debit.disable();
            data.Debit.reset();
        }
        else {
            data.Debit.enable();
            data.Credit.enable();
        }
    };
    /**
     * Performs the form submit action for CRUD Operations
     * @param formData
     */
    JournalVouchercomponent.prototype.onSubmit = function (formData, fileUpload) {
        var _this = this;
        //debugger
        this.msg = "";
        var journal = this.journalFrm;
        var newDate = new Date();
        var voucherDate = new Date(journal.get('Date').value);
        voucherDate.setTime(voucherDate.getTime() - (newDate.getTimezoneOffset() * 60000));
        journal.get('Date').setValue(voucherDate);
        this.formSubmitAttempt = true;
        if (!this.voucherDateValidator(journal.get('Date'))) {
            return false;
        }
        journal.get('FinancialYear').setValue(this.currentYear['Name'] || '');
        journal.get('UserName').setValue(this.currentUser && this.currentUser['UserName'] || '');
        journal.get('CompanyCode').setValue(this.currentUser && this.company['BranchCode'] || '');
        if (journal.valid) {
            var totalDebit = this.sumDebit();
            var totalCredit = this.sumCredit();
            if (totalDebit != totalCredit || totalDebit == 0 || totalCredit == 0) {
                alert("Debit and Credit are not Equal | Value must be greater than Amount Zero.");
                return;
            }
            switch (this.dbops) {
                case enum_1.DBOperation.create:
                    this._journalvoucherService.post(global_1.Global.BASE_JOURNALVOUCHER_ENDPOINT, journal.value)
                        .subscribe(function (data) { return __awaiter(_this, void 0, void 0, function () {
                        var upload;
                        return __generator(this, function (_a) {
                            switch (_a.label) {
                                case 0:
                                    if (!(data > 0)) return [3 /*break*/, 2];
                                    return [4 /*yield*/, fileUpload.handleFileUpload({
                                            'moduleName': 'JournalVoucher',
                                            'id': data
                                        })];
                                case 1:
                                    upload = _a.sent();
                                    if (upload == 'error') {
                                        alert('There is error uploading file!');
                                    }
                                    if (upload == true || upload == false) {
                                        this.modalRef.hide();
                                        this.formSubmitAttempt = false;
                                        this.reset();
                                    }
                                    this.modalRef.hide();
                                    this.loadJournalVoucherList();
                                    return [3 /*break*/, 3];
                                case 2:
                                    alert("There is some issue in saving records, please contact to system administrator!");
                                    _a.label = 3;
                                case 3: return [2 /*return*/];
                            }
                        });
                    }); });
                    break;
                case enum_1.DBOperation.update:
                    var journalObj = {
                        Id: this.journalFrm.controls['Id'].value,
                        Date: this.journalFrm.controls['Date'].value,
                        Name: this.journalFrm.controls['Name'].value,
                        AccountTransactionDocumentId: this.journalFrm.controls['AccountTransactionDocumentId'].value,
                        Description: this.journalFrm.controls['Description'].value,
                        Amount: this.journalFrm.controls['Amount'].value,
                        drTotal: this.journalFrm.controls['drTotal'].value,
                        crTotal: this.journalFrm.controls['crTotal'].value,
                        FinancialYear: this.journalFrm.controls['FinancialYear'].value,
                        UserName: this.journalFrm.controls['UserName'].value,
                        CompanyCode: this.journalFrm.controls['CompanyCode'].value,
                        AccountTransactionValues: this.journalFrm.controls['AccountTransactionValues'].value
                    };
                    this._journalvoucherService.put(global_1.Global.BASE_JOURNALVOUCHER_ENDPOINT, journal.value.Id, journalObj).subscribe(function (data) { return __awaiter(_this, void 0, void 0, function () {
                        var upload;
                        return __generator(this, function (_a) {
                            switch (_a.label) {
                                case 0:
                                    if (!(data > 0)) return [3 /*break*/, 2];
                                    return [4 /*yield*/, fileUpload.handleFileUpload({
                                            'moduleName': 'JournalVoucher',
                                            'id': data
                                        })];
                                case 1:
                                    upload = _a.sent();
                                    if (upload == 'error') {
                                        alert('There is error uploading file!');
                                    }
                                    if (upload == true || upload == false) {
                                        this.modalRef.hide();
                                        this.formSubmitAttempt = false;
                                        this.reset();
                                    }
                                    this.modalRef.hide();
                                    this.loadJournalVoucherList();
                                    return [3 /*break*/, 3];
                                case 2:
                                    alert("There is some issue in saving records, please contact to system administrator!");
                                    _a.label = 3;
                                case 3: return [2 /*return*/];
                            }
                        });
                    }); });
                    break;
                case enum_1.DBOperation.delete:
                    var journalObjc = {
                        Id: this.journalFrm.controls['Id'].value,
                        Date: this.journalFrm.controls['Date'].value,
                        Name: this.journalFrm.controls['Name'].value,
                        AccountTransactionDocumentId: this.journalFrm.controls['AccountTransactionDocumentId'].value,
                        Description: this.journalFrm.controls['Description'].value,
                        Amount: this.journalFrm.controls['Amount'].value,
                        drTotal: this.journalFrm.controls['drTotal'].value,
                        crTotal: this.journalFrm.controls['crTotal'].value,
                        FinancialYear: this.journalFrm.controls['FinancialYear'].value,
                        UserName: this.journalFrm.controls['UserName'].value,
                        CompanyCode: this.journalFrm.controls['CompanyCode'].value,
                        AccountTransactionValues: this.journalFrm.controls['AccountTransactionValues'].value
                    };
                    this._journalvoucherService.delete(global_1.Global.BASE_JOURNALVOUCHER_ENDPOINT, journalObjc).subscribe(function (data) {
                        if (data == 1) {
                            alert("Data successfully deleted.");
                            _this.loadJournalVoucherList();
                        }
                        else {
                            alert("There is some issue in saving records, please contact to system administrator!");
                        }
                        _this.modalRef.hide();
                        _this.formSubmitAttempt = false;
                        _this.journalFrm.reset();
                    });
            }
        }
        else {
            this.validateAllFields(journal);
        }
    };
    /**
     * Hides the confirm modal
     */
    JournalVouchercomponent.prototype.confirm = function () {
        this.modalRef2.hide();
        this.formSubmitAttempt = false;
    };
    /**
     * Resets the journal form
     */
    JournalVouchercomponent.prototype.reset = function () {
        this.journalFrm.controls['Id'].reset();
        this.journalFrm.controls['Date'].reset();
        this.journalFrm.controls['drTotal'].reset();
        this.journalFrm.controls['crTotal'].reset();
        this.journalFrm.controls['Description'].reset();
        this.journalFrm.controls['AccountTransactionValues'] = this.fb.array([]);
        this.addAccountValues();
    };
    /**
     * Sets control's state
     * @param isEnable
     */
    JournalVouchercomponent.prototype.SetControlsState = function (isEnable) {
        isEnable ? this.journalFrm.enable() : this.journalFrm.disable();
    };
    /**
     *  Get the list of filtered journals by the form and to date
     */
    JournalVouchercomponent.prototype.filterJournalByDate = function () {
        var _this = this;
        this.indLoading = true;
        this._journalvoucherService.get(global_1.Global.BASE_JOURNALVOUCHER_ENDPOINT + '?fromDate=' + this.date.transform(this.fromDate, 'yyyy-MM-dd') + '&toDate=' + this.date.transform(this.toDate, 'yyyy-MM-dd') + '&TransactionTypeId=' + 5)
            .subscribe(function (journalVoucher) {
            _this.indLoading = false;
            return _this.journalVoucher = journalVoucher;
        }, function (error) { return _this.msg = error; });
    };
    JournalVouchercomponent.prototype.onFilterDateSelect = function (selectedDate) {
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
    ], JournalVouchercomponent.prototype, "TemplateRef", void 0);
    __decorate([
        core_1.ViewChild('templateNested')
    ], JournalVouchercomponent.prototype, "TemplateRef2", void 0);
    __decorate([
        core_1.ViewChild('fileInput')
    ], JournalVouchercomponent.prototype, "fileInput", void 0);
    JournalVouchercomponent = __decorate([
        core_1.Component({
            templateUrl: './journalVoucher.component.html',
            styleUrls: ['./journaVoucher.component.css']
        })
    ], JournalVouchercomponent);
    return JournalVouchercomponent;
}());
exports.JournalVouchercomponent = JournalVouchercomponent;
