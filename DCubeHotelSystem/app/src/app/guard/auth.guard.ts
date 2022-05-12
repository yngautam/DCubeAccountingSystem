import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthenticationService } from '../Service/authentication.service';
@Injectable()
export class AuthGuard implements CanActivate {

   constructor(public auth: AuthenticationService, public router: Router) { }
   
   canActivate(): boolean {
      // debugger;
        if (!this.auth.isAuthenticated()) {
            this.router.navigate(['/login']);
            return false;
        }        
        return true;
    }
}