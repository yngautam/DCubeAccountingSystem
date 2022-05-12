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
var ContraComponent = /** @class */ (function () {
    function ContraComponent(fb, _journalvoucherService, _accountTransValues, date, modalService, fileService) {
        var _this = this;
        this.fb = fb;
        this._journalvoucherService = _journalvoucherService;
        this._accountTransValues = _accountTransValues;
        this.date = date;
        this.modalService = modalService;
        this.fileService = fileService;
        this.indLoading = false;
        this.dropMessage = "Upload Reference File";
        this.uploadUrl = global_1.Global.BASE_FILE_UPLOAD_ENDPOINT;
        this.fileUrl = '';
        this.currentYear = {};
        this.currentUser = {};
        this.company = {};
        this._journalvoucherService.getAccounts().subscribe(function (data) { _this.account = data; });
        this.currentYear = JSON.parse(localStorage.getItem('currentYear'));
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.company = JSON.parse(localStorage.getItem('company'));
        this.fromDate = new Date(this.currentYear['StartDate']);
        this.toDate = new Date(this.currentYear['EndDate']);
    }
    /**
     * Overrides the ngOnInit
     */
    ContraComponent.prototype.ngOnInit = function () {
        this.contraForm = this.fb.group({
            Id: [''],
            Name: [''],
            AccountTransactionDocumentId: [''],
            Description: [''],
            Amount: [''],
            Date: ['', forms_1.Validators.required],
            drTotal: [''],
            crTotal: [''],
            SourceAccountTypeId: [''],
            AccountTransactionValues: this.fb.array([this.initAccountValue()]),
            FinancialYear: [''],
            UserName: [''],
            CompanyCode: ['']
        });
        this.loadPaymentList();
    };
    ContraComponent.prototype.viewFile = function (fileUrl, template) {
        this.fileUrl = fileUrl;
        this.modalTitle = "View Attachment";
        this.modalRef = this.modalService.show(template, { keyboard: false, class: 'modal-lg' });
    };
    ContraComponent.prototype.voucherDateValidator = function (control) {
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
     * Load Payment List
     */
    ContraComponent.prototype.loadPaymentList = function () {
        var _this = this;
        this.indLoading = true;
        this._journalvoucherService.get(global_1.Global.BASE_JOURNALVOUCHER_ENDPOINT + '?fromDate=' + this.date.transform(this.fromDate, 'yyyy-MM-dd') + '&toDate=' + this.date.transform(this.toDate, 'yyyy-MM-dd') + '&TransactionTypeId=' + 13)
            .subscribe(function (paymentList) {
            paymentList.map(function (pay) { return pay['File'] = global_1.Global.BASE_HOST_ENDPOINT + global_1.Global.BASE_FILE_UPLOAD_ENDPOINT + '?Id=' + pay.Id + '&ApplicationModule=JournalVoucher'; });
            _this.paymentList = paymentList;
            _this.indLoading = false;
        }, function (error) { return _this.msg = error; });
    };
    ContraComponent.prototype.exportTableToExcel = function (tableID, filename) {
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
        filename = filename ? filename + '.xls' : 'Contra Voucher of ' + this.date.transform(new Date, 'dd-MM-yyyy') + '.xls';
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
    /**
     * Add Payment
     */
    ContraComponent.prototype.addPayment = function () {
        this.dbops = enum_1.DBOperation.create;
        this.SetControlsState(true);
        this.modalTitle = "Add Contra";
        this.modalBtnTitle = "Save & Submit";
        this.reset();
        this.contraForm.controls['Name'].setValue('Contra');
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
    ContraComponent.prototype.getJournalVoucher = function (Id) {
        this.indLoading = true;
        return this._journalvoucherService.get(global_1.Global.BASE_JOURNALVOUCHER_ENDPOINT + '?TransactionId=' + Id);
    };
    /**
     * Edit the contra
     * @param Id
     */
    ContraComponent.prototype.editPayment = function (Id) {
        var _this = this;
        debugger;
        this.reset();
        this.dbops = enum_1.DBOperation.update;
        this.SetControlsState(true);
        this.modalTitle = "Edit Contra";
        this.modalBtnTitle = "Update";
        this.getJournalVoucher(Id)
            .subscribe(function (contra) {
            debugger;
            _this.indLoading = false;
            _this.contraForm.controls['Id'].setValue(contra.Id);
            _this.contraForm.controls['Name'].setValue(contra.Name);
            _this.contraForm.controls['AccountTransactionDocumentId'].setValue(contra.AccountTransactionDocumentId);
            _this.contraForm.controls['SourceAccountTypeId'].setValue(contra.SourceAccountTypeId);
            _this.contraForm.controls['Description'].setValue(contra.Description);
            _this.formattedDate = new Date(contra.AccountTransactionValues[0]['Date']);
            _this.contraForm.controls['Date'].setValue(_this.formattedDate);
            _this.contraForm.controls['AccountTransactionValues'] = _this.fb.array([]);
            var control = _this.contraForm.controls['AccountTransactionValues'];
            for (var i = 0; i < contra.AccountTransactionValues.length; i++) {
                control.push(_this.fb.group(contra.AccountTransactionValues[i]));
            }
            _this.modalRef = _this.modalService.show(_this.TemplateRef, {
                backdrop: 'static',
                keyboard: false,
                class: 'modal-lg'
            });
        }, function (error) { return _this.msg = error; });
    };
    /**
     *  Deletes the given contra
     * @param Id
     */
    ContraComponent.prototype.deletePayment = function (Id) {
        var _this = this;
        this.dbops = enum_1.DBOperation.delete;
        this.SetControlsState(true);
        this.modalTitle = "Delete Payment";
        this.modalBtnTitle = "Delete";
        this.getJournalVoucher(Id)
            .subscribe(function (contra) {
            debugger;
            _this.indLoading = false;
            _this.contraForm.controls['Id'].setValue(contra.Id);
            _this.contraForm.controls['Name'].setValue(contra.Name);
            _this.contraForm.controls['AccountTransactionDocumentId'].setValue(contra.AccountTransactionDocumentId);
            _this.contraForm.controls['SourceAccountTypeId'].setValue(contra.SourceAccountTypeId);
            _this.contraForm.controls['Description'].setValue(contra.Description);
            _this.formattedDate = new Date(contra.AccountTransactionValues[0]['Date']);
            _this.contraForm.controls['Date'].setValue(_this.formattedDate);
            _this.contraForm.controls['Date'].setValue(_this.formattedDate);
            _this.contraForm.controls['AccountTransactionValues'] = _this.fb.array([]);
            var control = _this.contraForm.controls['AccountTransactionValues'];
            for (var i = 0; i < contra.AccountTransactionValues.length; i++) {
                control.push(_this.fb.group(contra.AccountTransactionValues[i]));
            }
            _this.modalRef = _this.modalService.show(_this.TemplateRef, {
                backdrop: 'static',
                keyboard: false,
                class: 'modal-lg'
            });
        }, function (error) { return _this.msg = error; });
    };
    /**
     * Initialises the account values
     */
    ContraComponent.prototype.initAccountValue = function () {
        //initialize our vouchers
        return this.fb.group({
            AccountId: ['', forms_1.Validators.required],
            Debit: ['', forms_1.Validators.required],
            Credit: [''],
            Description: ['']
        });
    };
    //Push the Account Values in row//
    ContraComponent.prototype.addAccountValues = function () {
        var control = this.contraForm.controls['AccountTransactionValues'];
        var addPayment = this.initAccountValue();
        control.push(addPayment);
    };
    //remove the rows//
    ContraComponent.prototype.removeAccount = function (i) {
        var controls = this.contraForm.controls['AccountTransactionValues'];
        var controlToRemove = this.contraForm.controls.AccountTransactionValues['controls'][i].controls;
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
    //Calculate the sum of debit columns//
    ContraComponent.prototype.sumDebit = function () {
        var controls = this.contraForm.controls.AccountTransactionValues.value;
        return controls.reduce(function (total, accounts) {
            return (accounts.Debit) ? (total + Math.round(accounts.Debit)) : total;
        }, 0);
    };
    //Calculate the sum of credit columns//
    ContraComponent.prototype.sumCredit = function () {
        var controls = this.contraForm.controls.AccountTransactionValues.value;
        return controls.reduce(function (total, accounts) {
            return (accounts.Credit) ? (total + Math.round(accounts.Credit)) : total;
        }, 0);
    };
    /**
     * Validates the fields
     * @param formGroup
     */
    ContraComponent.prototype.validateAllFields = function (formGroup) {
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
    //opens the confirmation window  modal
    ContraComponent.prototype.openModal2 = function (template) {
        this.modalRef2 = this.modalService.show(template, { class: 'modal-sm' });
    };
    //Submits the form//
    ContraComponent.prototype.onSubmit = function (fileUpload) {
        var _this = this;
        this.msg = "";
        var contra = this.contraForm;
        var newdate = new Date();
        var voucherDate = new Date(contra.get('Date').value);
        voucherDate.setTime(voucherDate.getTime() - (newdate.getTimezoneOffset() * 60000));
        contra.get('Date').setValue(voucherDate);
        this.formSubmitAttempt = true;
        if (!this.voucherDateValidator(contra.get('Date'))) {
            return false;
        }
        contra.get('FinancialYear').setValue(this.currentYear['Name'] || '');
        contra.get('UserName').setValue(this.currentUser && this.currentUser['UserName'] || '');
        contra.get('CompanyCode').setValue(this.currentUser && this.company['BranchCode'] || '');
        if (contra.valid) {
            switch (this.dbops) {
                case enum_1.DBOperation.create:
                    this._journalvoucherService.post(global_1.Global.BASE_JOURNALVOUCHER_ENDPOINT, contra.value).subscribe(function (data) { return __awaiter(_this, void 0, void 0, function () {
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
                                    this.loadPaymentList();
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
                    var paymentObj = {
                        Id: this.contraForm.controls['Id'].value,
                        Date: this.contraForm.controls['Date'].value,
                        Name: this.contraForm.controls['Name'].value,
                        SourceAccountTypeId: this.contraForm.controls['SourceAccountTypeId'].value,
                        AccountTransactionDocumentId: this.contraForm.controls['AccountTransactionDocumentId'].value,
                        Description: this.contraForm.controls['Description'].value,
                        FinancialYear: this.contraForm.controls['FinancialYear'].value,
                        UserName: this.contraForm.controls['UserName'].value,
                        CompanyCode: this.contraForm.controls['CompanyCode'].value,
                        AccountTransactionValues: this.contraForm.controls['AccountTransactionValues'].value
                    };
                    this._journalvoucherService.put(global_1.Global.BASE_JOURNALVOUCHER_ENDPOINT, contra.value.Id, paymentObj).subscribe(function (data) { return __awaiter(_this, void 0, void 0, function () {
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
                                    this.loadPaymentList();
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
                    var paymentObject = {
                        Id: this.contraForm.controls['Id'].value,
                        Date: this.contraForm.controls['Date'].value,
                        Name: this.contraForm.controls['Name'].value,
                        SourceAccountTypeId: this.contraForm.controls['SourceAccountTypeId'].value,
                        AccountTransactionDocumentId: this.contraForm.controls['AccountTransactionDocumentId'].value,
                        Description: this.contraForm.controls['Description'].value,
                        FinancialYear: this.contraForm.controls['FinancialYear'].value,
                        UserName: this.contraForm.controls['UserName'].value,
                        CompanyCode: this.contraForm.controls['CompanyCode'].value,
                        AccountTransactionValues: this.contraForm.controls['AccountTransactionValues'].value
                    };
                    this._journalvoucherService.delete(global_1.Global.BASE_JOURNALVOUCHER_ENDPOINT, paymentObject).subscribe(function (data) {
                        if (data == 1) {
                            alert("Data successfully deleted.");
                            _this.loadPaymentList();
                        }
                        else {
                            alert("There is some issue in saving records, please contact to system administrator!");
                        }
                        _this.modalRef.hide();
                        _this.formSubmitAttempt = false;
                        _this.reset();
                    });
            }
        }
        else {
            this.validateAllFields(contra);
        }
    };
    ContraComponent.prototype.confirm = function () {
        this.modalRef2.hide();
        this.formSubmitAttempt = false;
    };
    ContraComponent.prototype.reset = function () {
        this.contraForm.controls['AccountTransactionDocumentId'].reset();
        this.contraForm.controls['Date'].reset();
        this.contraForm.controls['Description'].reset();
        this.contraForm.controls['SourceAccountTypeId'].reset();
        this.contraForm.controls['AccountTransactionValues'] = this.fb.array([]);
        this.addAccountValues();
    };
    ContraComponent.prototype.SetControlsState = function (isEnable) {
        isEnable ? this.contraForm.enable() : this.contraForm.disable();
    };
    /**
     *  Get the list of filtered journals by the form and to date
     */
    ContraComponent.prototype.filterPaymentsByDate = function () {
        var _this = this;
        this.indLoading = true;
        this._journalvoucherService.get(global_1.Global.BASE_JOURNALVOUCHER_ENDPOINT + '?fromDate=' + this.date.transform(this.fromDate, 'yyyy-MM-dd') + '&toDate=' + this.date.transform(this.toDate, 'yyyy-MM-dd') + '&TransactionTypeId=' + 6)
            .subscribe(function (paymentList) {
            _this.paymentList = paymentList;
            _this.indLoading = false;
        }, function (error) { return _this.msg = error; });
    };
    ContraComponent.prototype.onFilterDateSelect = function (selectedDate) {
        debugger;
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
        core_1.ViewChild('template')
    ], ContraComponent.prototype, "TemplateRef", void 0);
    __decorate([
        core_1.ViewChild('templateNested')
    ], ContraComponent.prototype, "TemplateRef2", void 0);
    __decorate([
        core_1.ViewChild('fileInput')
    ], ContraComponent.prototype, "fileInput", void 0);
    ContraComponent = __decorate([
        core_1.Component({
            templateUrl: './contra.component.html',
            styleUrls: ['./contra.component.css']
        })
    ], ContraComponent);
    return ContraComponent;
}());
exports.ContraComponent = ContraComponent;
