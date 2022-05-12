import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { MenuItemService } from '../../../Service/MenuItem.service';
import { MenuItemPortionService } from '../../../Service/MenuItemPortion.services';
import { FormBuilder, FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { IMenuItem, IMenuItemPortion } from '../../../Model/Menu/MenuItem';
import { IMenuCategory } from '../../../Model/Menu/MenuCategory';
import { UnitType } from '../../../Model/Inventory/UnitType';

import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

import { DBOperation } from '../../../Shared/enum';
import { Observable } from 'rxjs/Rx';
import { Global } from '../../../Shared/global';

@Component({
    templateUrl: './MenuItem.component.html'
})

export class MenuItemComponent implements OnInit {
    menuItems: IMenuItem[];
    menuItem: IMenuItem;
    menucategories: IMenuCategory[];
    UnitTypes: UnitType[];
    menucategory: IMenuCategory;
    msg: string;
    indLoading: boolean = false;
    private formSubmitAttempt: boolean;
    MenuItemForm: FormGroup;
    dbops: DBOperation;
    modalTitle: string;
    modalBtnTitle: string;
    modalRef: BsModalRef;
    menuitemPortions: IMenuItemPortion[];
    MenuItemName: string = '';

    constructor(
        private fb: FormBuilder,
        private _menuItemService: MenuItemService,
        private _menuportionservice: MenuItemPortionService,
        private modalService: BsModalService)
    {
        this._menuItemService.getMenuCategories().subscribe(data => { this.menucategory = data });
        this._menuItemService.getMenuUnits(Global.BASE_UNITTYPE_ENDPOINT).subscribe(data => { this.UnitTypes = data });
    }

    ngOnInit(): void {
        this.MenuItemForm = this.fb.group({
            Id: [''],
            Name: ['', Validators.required],
            categoryId: ['', Validators.required],
            Barcode: ['', Validators.required],
            Tag: ['', Validators.required],
            UnitType: ['', Validators.required],
            MenuItemPortions: this.fb.array([]),
        });
        this.LoadMenuItems();
    }

    initMenuItemPortions() {
        return this.fb.group({
            Id:[''],
            Name: ['', Validators.required],
            Multiplier: ['', Validators.required],
            Price: ['', Validators.required],
            OpeningStock: ['', Validators.required]
        });
    }

    LoadMenuItems(): void {
        this.indLoading = true;
        debugger
        this._menuItemService.get(Global.BASE_MENUITEM_ENDPOINT)
            .subscribe(menuItems => { this.menuItems = menuItems; this.indLoading = false; },
            error => this.msg = <any>error);
    }

    addMenuItemPortions() {
        debugger
        const control = <FormArray>this.MenuItemForm.controls['MenuItemPortions'];
        const AddPortions = this.initMenuItemPortions();
        control.push(AddPortions);
    }

    removeMenuItemPortions(i: number) {
        let controls = <FormArray>this.MenuItemForm.controls['MenuItemPortions'];
        let controlToRemove = this.MenuItemForm.controls.MenuItemPortions['controls'][i].controls;
        let selectedControl = controlToRemove.hasOwnProperty('Id') ? controlToRemove.Id.value : 0;

        if (selectedControl) {
            this._menuportionservice.delete(Global.BASE_MENUITEMPORTION_ENDPOINT, i).subscribe(data => {
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

    addMenuItems(template: TemplateRef<any>) {
        debugger
        this.modalTitle = "Add Product";
        this.modalBtnTitle = "Save";
        this.MenuItemForm.reset();
        this.dbops = DBOperation.create;
        this.SetControlsState(true);
        this.modalRef = this.modalService.show(template, {
            backdrop: 'static',
            keyboard: false,
            class: 'modal-lg',
        });
    }

    editMenuItems(Id: number, template: TemplateRef<any>) {
        debugger
        this.dbops = DBOperation.update;
        this.SetControlsState(true);
        this.modalTitle = "Edit Product";
        this.modalBtnTitle = "Update";
        this.menuItem = this.menuItems.filter(x => x.Id == Id)[0];
        this.MenuItemForm.controls['Id'].setValue(this.menuItem.Id);
        this.MenuItemForm.controls['categoryId'].setValue(this.menuItem.categoryId);
        this.MenuItemForm.controls['Name'].setValue(this.menuItem.Name);
        this.MenuItemForm.controls['Barcode'].setValue(this.menuItem.Barcode);
        this.MenuItemForm.controls['Tag'].setValue(this.menuItem.Tag);
        this.MenuItemForm.controls['UnitType'].setValue(this.menuItem.UnitType);

        this.MenuItemForm.controls['MenuItemPortions'] = this.fb.array([]);
        let control = <FormArray>this.MenuItemForm.controls['MenuItemPortions'];

        for (let i = 0; i < this.menuItem.MenuItemPortions.length; i++)
        {
            control.push(this.fb.group(this.menuItem.MenuItemPortions[i]));
        }
        
        this.modalRef = this.modalService.show(template, { backdrop: 'static', keyboard: false });
    }

    deleteMenuItems(id: number, template: TemplateRef<any>) {
        debugger
        this.dbops = DBOperation.delete;
        this.SetControlsState(true);
        this.modalTitle = "Confirm to Delete?";
        this.modalBtnTitle = "Delete";
        this.menuItem = this.menuItems.filter(x => x.Id == id)[0];
        this.MenuItemForm.controls['Id'].setValue(this.menuItem.Id);
        this.MenuItemForm.controls['categoryId'].setValue(this.menuItem.categoryId);
        this.MenuItemForm.controls['Name'].setValue(this.menuItem.Name);
        this.MenuItemForm.controls['Barcode'].setValue(this.menuItem.Barcode);
        this.MenuItemForm.controls['Tag'].setValue(this.menuItem.Tag);
        this.MenuItemForm.controls['UnitType'].setValue(this.menuItem.UnitType);

        this.MenuItemForm.controls['MenuItemPortions'] = this.fb.array([]);
        let control = <FormArray>this.MenuItemForm.controls['MenuItemPortions'];

        for (let i = 0; i < this.menuItem.MenuItemPortions.length; i++) {
            control.push(this.fb.group(this.menuItem.MenuItemPortions[i]));
        }
        this.modalRef = this.modalService.show(template, { backdrop: 'static', keyboard: false });
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

    onSubmit(formData: any) {
        debugger
        this.msg = "";
        this.formSubmitAttempt = true;
        let MenuItemForm = this.MenuItemForm;
        if (MenuItemForm.valid) {
            switch (this.dbops) {
                case DBOperation.create:
                    this._menuItemService.post(Global.BASE_MENUITEM_ENDPOINT, formData._value).subscribe(
                        data => {
                            debugger
                            if (data == 1) //Success
                            {
                                alert("Data successfully added.");
                                this.modalRef.hide();
                                this.reset();
                                this.LoadMenuItems();
                                this.formSubmitAttempt = false;
                                
                            }
                            else {
                                alert("There is some issue in creating records, please contact to system administrator!");
                            }
                        },
                        error => {
                            this.msg = error;
                        }
                    );
                    break;
                case DBOperation.update:
                    debugger
                    let MenuItemObj = {
                        Id: this.MenuItemForm.controls['Id'].value,
                        Name: this.MenuItemForm.controls['Name'].value,
                        categoryId: this.MenuItemForm.controls['categoryId'].value,
                        Barcode: this.MenuItemForm.controls['Barcode'].value,
                        Tag: this.MenuItemForm.controls['Tag'].value,
                        UnitType: this.MenuItemForm.controls['UnitType'].value,
                        MenuItemPortions: this.MenuItemForm.controls['MenuItemPortions'].value
                    }

                    this._menuItemService.put(Global.BASE_MENUITEM_ENDPOINT, formData._value.Id, MenuItemObj).subscribe(
                        
                        data => {
                            debugger
                            if (data == 1) //Success
                            {
                                alert("Data successfully updated.");
                                this.modalRef.hide();
                                this.formSubmitAttempt = false;
                                this.reset();
                                this.LoadMenuItems();
                            }
                            else {
                                alert("There is some issue in updating records, please contact to system administrator!");
                            }
                        },
                        error => {
                            this.msg = error;
                        }
                    );
                    break;
                case DBOperation.delete:
                    this._menuItemService.deletes(Global.BASE_MENUITEM_ENDPOINT, formData._value.Id).subscribe(
                        data => {
                            if (data == 1) //Success
                            {
                                alert("Data successfully deleted.");
                                this.modalRef.hide();
                                this.reset();
                                this.LoadMenuItems();
                                this.formSubmitAttempt = false;
                            }
                            else {
                                alert("There is some issue in deleting records, please contact to system administrator!");
                            }
                        },
                        error => {
                            this.msg = error;
                        }
                    );
                    break;

            }
        }
        else {
            this.validateAllFields(MenuItemForm);
        }
    }
    SetControlsState(isEnable: boolean) {
        isEnable ? this.MenuItemForm.enable() : this.MenuItemForm.disable();
    }

    cancel() {
        debugger
        this.modalRef.hide();
        this.MenuItemForm.reset();
    }
    reset() {
        this.MenuItemForm.controls['Id'].reset();
        this.MenuItemForm.controls['Name'].reset();
        this.MenuItemForm.controls['categoryId'].reset();
        this.MenuItemForm.controls['Barcode'].reset();
        this.MenuItemForm.controls['Tag'].reset();
        this.MenuItemForm.controls['MenuItemPortions'] = this.fb.array([]);
    }
}