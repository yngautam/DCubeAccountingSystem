import { Component, OnInit, ViewChild,TemplateRef } from '@angular/core';
import { LoginService } from '../../Service/login.service';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { IUser } from '../../Model/User/user';
import { Observable } from 'rxjs/Rx';
import { Global } from '../../Shared/global';
import { DBOperation } from '../../Shared/enum';
import { AuthenticationService } from '../../Service/authentication.service';

@Component({
    styleUrls: ['./login.component.css'],
    templateUrl: './login.component.html'
})

export class LoginComponent implements OnInit {
    model: any = {};
    loading = false;
    msg: string;
    returnUrl: string;
    dbops: DBOperation;
    form: FormGroup;
    private formSubmitAttempt: boolean;

    constructor(
        private fb: FormBuilder,
        private loginService: LoginService,
        private authenticationSevice: AuthenticationService,
        private route: ActivatedRoute,
        private router: Router,
      
    ) { }

    ngOnInit() {
        this.form = this.fb.group({
            UserName: ['', Validators.required],
            Password: ['', Validators.required],
            Remember: ['']
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
    
    onSubmit() {
        let loginfrm = this.form;
        this.authenticationSevice.login(Global.BASE_LOGIN_ENDPOINT, loginfrm.value).subscribe(
                data => {
                    if (data != 0) {
                        alert("User Logged in successfully.");
                        this.router.navigate(['/dashboard']);
                    } else {
                        alert("Login failed");
                    }
                },
                error => {
                    alert("Login failed");
                }
            );
    }
}