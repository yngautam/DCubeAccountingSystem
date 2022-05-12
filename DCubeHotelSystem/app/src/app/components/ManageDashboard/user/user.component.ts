﻿import { Component, OnInit, ViewChild, TemplateRef} from '@angular/core';
import { UsersService } from '../../../Service/user.service';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { IUser } from '../../../Model/User/user';
import { DBOperation } from '../../../Shared/enum';
import { Observable } from 'rxjs/Rx';
import { Global } from '../../../Shared/global';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

@Component({
    templateUrl: './user.component.html'
})
export class UserComponent implements OnInit {
    @ViewChild('template') TemplateRef: TemplateRef<any>;
    @ViewChild('templateNested') TemplateRef2: TemplateRef<any>;
    modalRef: BsModalRef;
    modalRef2: BsModalRef;
    user: IUser[];
    users: IUser;
    msg: string;
    indLoading: boolean = false;
    userFrm: FormGroup;
    dbops: DBOperation;
    modalTitle: string;
    modalBtnTitle: string;
    private editingStatus: boolean;
    private formSubmitAttempt: boolean;
    private buttonDisabled: boolean;
 

    constructor(private fb: FormBuilder, private _userService: UsersService, private modalService: BsModalService) { }

    ngOnInit(): void {
        this.userFrm = this.fb.group({
            UserId: [''],
            FullName: ['', Validators.required],
            UserName: ['', Validators.required],
            Password: ['', Validators.required],
            Email: ['', Validators.required],
            PhoneNumber: ['', Validators.required],
            IsActive: ['',],
            ResetPassword: [''],
        });

        this.LoadUsers();
    }

    LoadUsers(): void {
        this.indLoading = true;
        this._userService.get(Global.BASE_USERACCOUNT_ENDPOINT)
            .subscribe(user => { this.user = user; this.indLoading = false; },
            error => this.msg = <any>error);
    }

    addUser() {
        this.dbops = DBOperation.create;
        this.SetControlsState(true);
        this.modalTitle = "Add User";
        this.modalBtnTitle = "Add";
        this.userFrm.reset();
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false
        });
    }

    editUser(Id: number) {
        debugger;
        this.dbops = DBOperation.update;
        this.SetControlsState(true);
        this.modalTitle = "Edit User";
        this.modalBtnTitle = "Update";
        this.users = this.user.filter(x => x.UserId == Id)[0];
        this.userFrm.setValue(this.users);
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false
        });

     
    }

    deleteUser(Id: number) {
        debugger;
        this.dbops = DBOperation.delete;
        this.SetControlsState(false);
        this.modalTitle = "Confirm to Delete User?";
        this.modalBtnTitle = "Delete";
        this.users = this.user.filter(x => x.UserId == Id)[0];
        this.userFrm.setValue(this.users);
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


    openModal2(template: TemplateRef<any>) {
        this.modalRef2 = this.modalService.show(template, { class: 'modal-sm' });
    }

    onSubmit(formData:any) {
        debugger;
        this.formSubmitAttempt = true;
        this.msg = "";
        let users = this.userFrm

        if (users.valid) {
            switch (this.dbops) {
                case DBOperation.create:
                    this._userService.post(Global.BASE_USERACCOUNT_ENDPOINT, formData.value, ).subscribe(
                        data => {
                            if (data == 1) //Success
                            {
                                this.openModal2(this.TemplateRef2); 
                                this.LoadUsers();
                                this.formSubmitAttempt = false;
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
                                this.LoadUsers();
                                
                            }
                            else {
                                this.msg = "There is some issue in saving records, please contact to system administrator!"
                            }
                            this.modalRef.hide();
                            this.formSubmitAttempt = false;
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
                                this.LoadUsers();
                            }
                            else {
                                this.msg = "There is some issue in saving records, please contact to system administrator!"
                            }
                            this.modalRef.hide();
                            this.formSubmitAttempt = false;
                        },
                    )
            }
        }

        else {
            this.validateAllFields(users);
        }
    }

    confirm(): void {
        this.modalRef2.hide();
    }

    reset() {
        debugger;
        let control = this.userFrm.controls['UserId'].value;
        if (control > 0) {
            this.buttonDisabled = true;
        }
        else {
            this.userFrm.reset();
        }
    }

    SetControlsState(isEnable: boolean) {
        isEnable ? this.userFrm.enable() : this.userFrm.disable();
    }
}