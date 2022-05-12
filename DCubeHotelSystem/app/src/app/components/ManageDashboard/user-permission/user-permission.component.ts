﻿import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { UsersService } from '../../../Service/user.service';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { UserPermission } from '../../../Model/User/userpermission';
import { DBOperation } from '../../../Shared/enum';
import { Observable } from 'rxjs/Rx';
import { Global } from '../../../Shared/global';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

@Component({
    templateUrl: './user-permission.component.html'
})

export class UserPermissionComponent implements OnInit {
    @ViewChild('template') TemplateRef: TemplateRef<any>;
    userPer: UserPermission[];
    usersPer: UserPermission;
    msg: string;
    indLoading: boolean = false;
    userPermissionFrm: FormGroup;
    dbops: DBOperation;
    modalTitle: string;
    modalBtnTitle: string;
    private editingStatus: boolean;
    private formSubmitAttempt: boolean;
    private buttonDisabled: boolean;
    modalRef: BsModalRef;

    constructor(private fb: FormBuilder, private _userService: UsersService, private modalService: BsModalService) { }

    ngOnInit(): void {
        this.userPermissionFrm = this.fb.group({
            UserPermissionId: [''],
            PermissionId:[''],
            UserId: [''],
            PermissionName: ['', Validators.required],
            UserFullName: ['', Validators.required],
            CreatedDate: ['', Validators.required],
            CreatedBy: ['', Validators.required],
            LastChangedDate: ['', Validators.required],
            LastChangedBy: ['',],
        });

        this.LoadUserPermission();
    }

    LoadUserPermission(): void {
        this.indLoading = true;
        this._userService.get(Global.BASE_USERACCOUNT_ENDPOINT)
            .subscribe(userPer => { this.userPer = userPer; this.indLoading = false; },
            error => this.msg = <any>error);
    }

    addUserPermission() {
        this.dbops = DBOperation.create;
        this.SetControlsState(true);
        this.modalTitle = "Add New User";
        this.modalBtnTitle = "Add";
        this.userPermissionFrm.reset();
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false,
            class: 'modal-lg'
        });
    }

    editUserPermission(Id: number) {
        debugger;
        this.dbops = DBOperation.update;
        this.SetControlsState(true);
        this.modalTitle = "Edit User";
        this.modalBtnTitle = "Update";
        this.usersPer = this.userPer.filter(x => x.UserId == Id)[0];
        this.userPermissionFrm.setValue(this.usersPer);
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false,
            class: 'modal-lg'
        });
    }

    deleteUserPermission(Id: number) {
        debugger;
        this.dbops = DBOperation.delete;
        this.SetControlsState(false);
        this.modalTitle = "Confirm to Delete?";
        this.modalBtnTitle = "Delete";
        this.usersPer = this.userPer.filter(x => x.UserId == Id)[0];
        this.userPermissionFrm.setValue(this.usersPer);
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false
        });
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
        debugger;
        this.formSubmitAttempt = true;
        this.msg = "";
        let usrPermFrm = this.userPermissionFrm

        if (usrPermFrm.valid) {
            switch (this.dbops) {
                case DBOperation.create:
                    this._userService.post(Global.BASE_USERACCOUNT_ENDPOINT, formData.value, ).subscribe(
                        data => {
                            if (data == 1) //Success
                            {
                                this.msg = "Data successfully added.";
                                this.LoadUserPermission();
                            }
                            else {
                                this.msg = "There is some issue in saving records, please contact to system administrator!"
                            }

                            this.modalRef.hide();
                        },
                    );
                    break;
                case DBOperation.update:
                    debugger;
                    this._userService.put(Global.BASE_USERACCOUNT_ENDPOINT, formData.value.UserId, formData.value, ).subscribe(
                        data => {
                            if (data == 1) //Success
                            {
                                this.msg = "Data updated successfully.";
                                this.LoadUserPermission();
                            } else {
                                this.msg = "There is some issue in saving records, please contact to system administrator!"
                            }
                            this.modalRef.hide();
                        },
                    );
                    break;
                case DBOperation.delete:
                    debugger;
                    this._userService.delete(Global.BASE_USER_ENDPOINT, formData.value.UserId).subscribe(
                        data => {
                            if (data == 1) //Success
                            {
                                this.msg = "Data successfully deleted.";
                                this.LoadUserPermission();
                            } else {
                                this.msg = "There is some issue in saving records, please contact to system administrator!"
                            }

                            this.modalRef.hide();
                        },
                    )
            }
        } else {
            this.validateAllFields(usrPermFrm);
        }
    }

    reset() {
        debugger;
        let control = this.userPermissionFrm.controls['UserId'].value;
        if (control > 0) {
            this.buttonDisabled = true;
        }
        else {
            this.userPermissionFrm.reset();

        }

    }

    SetControlsState(isEnable: boolean) {
        isEnable ? this.userPermissionFrm.enable() : this.userPermissionFrm.disable();
    }
}