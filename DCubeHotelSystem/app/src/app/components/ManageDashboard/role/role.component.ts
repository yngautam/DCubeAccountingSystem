import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { IRole } from '../../../Model/Roles/role';
import { RoleService } from '../../../Service/role.service';
import { DBOperation } from '../../../Shared/enum';
import { Observable } from 'rxjs/Rx';
import { Global } from '../../../Shared/global';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { DatePipe } from '@angular/common';

@Component({
    templateUrl: './role.component.html'
})

export class RoleComponent implements OnInit {
    @ViewChild('template') TemplateRef: TemplateRef<any>;
    @ViewChild('templateNested') TemplateRef2: TemplateRef<any>;
    modalRef: BsModalRef;
    modalRef2: BsModalRef;
    roles: IRole[];
    role: IRole;
    msg: string;
    indLoading: boolean = false;
    public RoleFrm: FormGroup;
    dbops: DBOperation;
    modalTitle: string;
    modalBtnTitle: string;
    private formSubmitAttempt: boolean;
    private buttonDisabled: boolean;
    formattedDate: any;

    constructor(private fb: FormBuilder, private _roleService: RoleService, private date: DatePipe, private modalService: BsModalService) { }

    ngOnInit(): void {
        this.RoleFrm = this.fb.group({
            RoleId: [''],
            RoleName: [''],
            Description: [''],
            CreatedOn: [''],
            CreatedBy: [''],
            LastChangedDate: [''],
            LastChangedBy: [''],
            Selected: [''],
            IsSysAdmin: ['']
        });
        this.LoadRoles();
    }

    LoadRoles(): void {
        this.indLoading = true;
        this._roleService.get(Global.BASE_ROLES_ENDPOINT)
            .subscribe(roles => { this.roles = roles; this.indLoading = false; },
            error => this.msg = <any>error);
    }

    addRoles() {
        this.dbops = DBOperation.create;
        this.SetControlsState(true);
        this.modalTitle = "Add UserRole";
        this.modalBtnTitle = "Add";
        this.RoleFrm.reset();
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false,
            class: 'modal-lg'
        });
    }

    editUserRole(Id: number) {
        debugger;
        this.dbops = DBOperation.update;
        this.SetControlsState(true);
        this.modalTitle = "Edit UserRole";
        this.modalBtnTitle = "Update";
        this.role = this.roles.filter(x => x.RoleId == Id)[0];
        this.RoleFrm.setValue(this.role);
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false,
            class: 'modal-lg'
        });

    }

    deleteUserRole(id: number) {
        debugger;
        this.dbops = DBOperation.delete;
        this.SetControlsState(true);
        this.modalTitle = "Confirm to Delete?";
        this.modalBtnTitle = "Delete";
        this.role = this.roles.filter(x => x.RoleId == id)[0];
        this.RoleFrm.setValue(this.role);
        this.modalRef = this.modalService.show(this.TemplateRef, {
            backdrop: 'static',
            keyboard: false,
            class: 'modal-lg'
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

    onSubmit(formData: any) {
        this.msg = "";
        let Role = this.RoleFrm;
        this.formSubmitAttempt = true;

        if (Role.valid) {
            switch (this.dbops) {
                case DBOperation.create:
                    this._roleService.post(Global.BASE_ROLES_ENDPOINT, formData.value).subscribe(
                        data => {
                            if (data == 1) //Success
                            {
                                debugger;
                                this.openModal2(this.TemplateRef2);
                                this.LoadRoles();
                            }
                            else {
                                this.msg = "There is some issue in creating records, please contact to system administrator!"
                            }

                            this.modalRef.hide();
                            this.formSubmitAttempt = false;

                        },
                    );
                    break;
                case DBOperation.update:
                    debugger;
                    this._roleService.put(Global.BASE_ROLES_ENDPOINT, formData.value.RoleId, formData.value).subscribe(
                        data => {
                            if (data == 1) //Success
                            {
                                this.msg = "Data successfully updated.";
                                this.LoadRoles();
                            }
                            else {
                                this.msg = "There is some issue in updating records, please contact to system administrator!"
                            }
                            this.modalRef.hide();
                            this.formSubmitAttempt = false;
                        },
                    )
                    break;
                case DBOperation.delete:
                    debugger;
                    this._roleService.delete(Global.BASE_ROLES_ENDPOINT, formData.value.RoleId).subscribe(
                        data => {
                            if (data == 1) //Success
                            {
                                this.msg = "Data successfully deleted.";
                                this.LoadRoles();
                            }
                            else {
                                this.msg = "There is some issue in deleting records, please contact to system administrator!"
                            }

                            this.modalRef.hide();
                            this.formSubmitAttempt = false;
                        },
                    )
            }
        }
        else {
            this.validateAllFields(Role);
        }
    }

    confirm(): void {
        this.modalRef2.hide();
    }

    reset() {
        let control = this.RoleFrm.controls['RoleId'].value;
        if (control > 0) {
            this.buttonDisabled = true;
        }
        else {
            this.RoleFrm.reset();
        }
    }

    SetControlsState(isEnable: boolean) {
        isEnable ? this.RoleFrm.enable() : this.RoleFrm.disable();
    }

}
