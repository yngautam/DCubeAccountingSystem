import { Component } from "@angular/core"
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';

@Component({
    selector: "ReportDashboard-app",
    templateUrl: './ReportDashboard.component.html',
    styleUrls: ['./ReportDashboard.component.css']
})

export class ReportDashboardComponent {
    user: any;
    hideElement = false;

    constructor(private activatedRoute: ActivatedRoute,
        private router: Router) {
        this.user = JSON.parse(localStorage.getItem('currentUser'));
        this.router.events.subscribe((event) => {
            if (event instanceof NavigationEnd) {
                if (event.url === '/reportdashboard') {
                    this.hideElement = true;
                } else {
                    this.hideElement = false;
                }
            }
        });
    }

    logout() {
        localStorage.clear();
        this.router.navigate(['/login']);
    }
}
