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
var MenuItemComponent = /** @class */ (function () {
    function MenuItemComponent(fb, _menuItemService, _menuportionservice, modalService) {
        var _this = this;
        this.fb = fb;
        this._menuItemService = _menuItemService;
        this._menuportionservice = _menuportionservice;
        this.modalService = modalService;
        this.indLoading = false;
        this._menuItemService.getMenuCategories().subscribe(function (data) { _this.menucategory = data; });
    }
    MenuItemComponent.prototype.ngOnInit = function () {
        this.MenuItemForm = this.fb.group({
            Id: [''],
            Name: ['', forms_1.Validators.required],
            categoryId: ['', forms_1.Validators.required],
            Barcode: ['', forms_1.Validators.required],
            Tag: ['', forms_1.Validators.required],
            MenuItemPortions: this.fb.array([]),
        });
        this.LoadMenuItems();
    };
    MenuItemComponent.prototype.initMenuItemPortions = function () {
        return this.fb.group({
            Id: [''],
            Name: ['', forms_1.Validators.required],
            Multiplier: ['', forms_1.Validators.required],
            Price: ['', forms_1.Validators.required]
        });
    };
    MenuItemComponent.prototype.LoadMenuItems = function () {
        var _this = this;
        this.indLoading = true;
        debugger;
        this._menuItemService.get(global_1.Global.BASE_MENUITEM_ENDPOINT)
            .subscribe(function (menuItems) { _this.menuItems = menuItems; _this.indLoading = false; }, function (error) { return _this.msg = error; });
    };
    MenuItemComponent.prototype.addMenuItemPortions = function () {
        debugger;
        var control = this.MenuItemForm.controls['MenuItemPortions'];
        var AddPortions = this.initMenuItemPortions();
        control.push(AddPortions);
    };
    MenuItemComponent.prototype.removeMenuItemPortions = function (i) {
        var controls = this.MenuItemForm.controls['MenuItemPortions'];
        var controlToRemove = this.MenuItemForm.controls.MenuItemPortions['controls'][i].controls;
        var selectedControl = controlToRemove.hasOwnProperty('Id') ? controlToRemove.Id.value : 0;
        if (selectedControl) {
            this._menuportionservice.delete(global_1.Global.BASE_MENUITEMPORTION_ENDPOINT, i).subscribe(function (data) {
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
    MenuItemComponent.prototype.addMenuItems = function (template) {
        debugger;
        this.modalTitle = "Add New MenuItem";
        this.modalBtnTitle = "Save & Submit";
        this.MenuItemForm.reset();
        this.dbops = enum_1.DBOperation.create;
        this.SetControlsState(true);
        this.modalRef = this.modalService.show(template, {
            backdrop: 'static',
            keyboard: false,
            class: 'modal-lg',
        });
    };
    MenuItemComponent.prototype.editMenuItems = function (Id, template) {
        debugger;
        this.dbops = enum_1.DBOperation.update;
        this.SetControlsState(true);
        this.modalTitle = "Edit MenuItem";
        this.modalBtnTitle = "Update";
        this.menuItem = this.menuItems.filter(function (x) { return x.Id == Id; })[0];
        this.MenuItemForm.controls['Id'].setValue(this.menuItem.Id);
        this.MenuItemForm.controls['categoryId'].setValue(this.menuItem.categoryId);
        this.MenuItemForm.controls['Name'].setValue(this.menuItem.Name);
        this.MenuItemForm.controls['Barcode'].setValue(this.menuItem.Barcode);
        this.MenuItemForm.controls['Tag'].setValue(this.menuItem.Tag);
        this.MenuItemForm.controls['MenuItemPortions'] = this.fb.array([]);
        var control = this.MenuItemForm.controls['MenuItemPortions'];
        for (var i = 0; i < this.menuItem.MenuItemPortions.length; i++) {
            control.push(this.fb.group(this.menuItem.MenuItemPortions[i]));
        }
        this.modalRef = this.modalService.show(template, { backdrop: 'static', keyboard: false });
    };
    MenuItemComponent.prototype.deleteMenuItems = function (id, template) {
        debugger;
        this.dbops = enum_1.DBOperation.delete;
        this.SetControlsState(true);
        this.modalTitle = "Confirm to Delete?";
        this.modalBtnTitle = "Delete";
        this.menuItem = this.menuItems.filter(function (x) { return x.Id == id; })[0];
        this.MenuItemForm.controls['Id'].setValue(this.menuItem.Id);
        this.MenuItemForm.controls['categoryId'].setValue(this.menuItem.categoryId);
        this.MenuItemForm.controls['Name'].setValue(this.menuItem.Name);
        this.MenuItemForm.controls['Barcode'].setValue(this.menuItem.Barcode);
        this.MenuItemForm.controls['Tag'].setValue(this.menuItem.Tag);
        this.MenuItemForm.controls['MenuItemPortions'] = this.fb.array([]);
        var control = this.MenuItemForm.controls['MenuItemPortions'];
        for (var i = 0; i < this.menuItem.MenuItemPortions.length; i++) {
            control.push(this.fb.group(this.menuItem.MenuItemPortions[i]));
        }
        this.modalRef = this.modalService.show(template, { backdrop: 'static', keyboard: false });
    };
    MenuItemComponent.prototype.validateAllFields = function (formGroup) {
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
    MenuItemComponent.prototype.onSubmit = function (formData) {
        var _this = this;
        debugger;
        this.msg = "";
        this.formSubmitAttempt = true;
        var MenuItemForm = this.MenuItemForm;
        if (MenuItemForm.valid) {
            switch (this.dbops) {
                case enum_1.DBOperation.create:
                    this._menuItemService.post(global_1.Global.BASE_MENUITEM_ENDPOINT, formData._value).subscribe(function (data) {
                        debugger;
                        if (data == 1) {
                            alert("Data successfully added.");
                            _this.modalRef.hide();
                            _this.LoadMenuItems();
                            _this.formSubmitAttempt = false;
                        }
                        else {
                            alert("There is some issue in creating records, please contact to system administrator!");
                        }
                    }, function (error) {
                        _this.msg = error;
                    });
                    break;
                case enum_1.DBOperation.update:
                    debugger;
                    var MenuItemObj = {
                        Id: this.MenuItemForm.controls['Id'].value,
                        Name: this.MenuItemForm.controls['Name'].value,
                        categoryId: this.MenuItemForm.controls['categoryId'].value,
                        Barcode: this.MenuItemForm.controls['Barcode'].value,
                        Tag: this.MenuItemForm.controls['Tag'].value,
                        MenuItemPortions: this.MenuItemForm.controls['MenuItemPortions'].value
                    };
                    this._menuItemService.put(global_1.Global.BASE_MENUITEM_ENDPOINT, formData._value.Id, MenuItemObj).subscribe(function (data) {
                        debugger;
                        if (data == 1) {
                            alert("Data successfully updated.");
                            _this.modalRef.hide();
                            _this.formSubmitAttempt = false;
                            _this.LoadMenuItems();
                        }
                        else {
                            alert("There is some issue in updating records, please contact to system administrator!");
                        }
                    }, function (error) {
                        _this.msg = error;
                    });
                    break;
                case enum_1.DBOperation.delete:
                    this._menuItemService.deletes(global_1.Global.BASE_MENUITEM_ENDPOINT, formData._value.Id).subscribe(function (data) {
                        if (data == 1) {
                            alert("Data successfully deleted.");
                            _this.modalRef.hide();
                            _this.LoadMenuItems();
                            _this.formSubmitAttempt = false;
                        }
                        else {
                            alert("There is some issue in deleting records, please contact to system administrator!");
                        }
                    }, function (error) {
                        _this.msg = error;
                    });
                    break;
            }
        }
        else {
            this.validateAllFields(MenuItemForm);
        }
    };
    MenuItemComponent.prototype.SetControlsState = function (isEnable) {
        isEnable ? this.MenuItemForm.enable() : this.MenuItemForm.disable();
    };
    MenuItemComponent.prototype.cancel = function () {
        debugger;
        this.modalRef.hide();
        this.MenuItemForm.reset();
    };
    MenuItemComponent = __decorate([
        core_1.Component({
            templateUrl: './MenuItem.component.html'
        })
    ], MenuItemComponent);
    return MenuItemComponent;
}());
exports.MenuItemComponent = MenuItemComponent;
