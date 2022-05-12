import { Component } from "@angular/core"
import { RouterModule, Router, NavigationEnd, ActivatedRoute } from '@angular/router';

@Component({
    selector: "accountdashboard-app",
    templateUrl: './AccountDashboard.component.html',
    styleUrls: ['./AccountDashboard.component.css']
})

export class AccountDashboardComponent {
    user: any;
    hideElement = false;
    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router
    )
    {
        this.user = JSON.parse(localStorage.getItem('currentUser'));
        this.router.events.subscribe((event) => {
            if (event instanceof NavigationEnd) {
                if (event.url === '/accountdashboard') {
                    this.hideElement = true;
                } else {
                    this.hideElement = false;
                }
            }
        });
    }

    /**
     * Logout user
     */
    logout() {
        localStorage.clear();
        this.router.navigate(['/login']);
    }
}
