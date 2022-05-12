import { Component } from "@angular/core"
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';

@Component({
    selector: "manage-app-navigation",
    templateUrl: './managedashboard.component.html',
    styleUrls: ['./managedashboard.component.css']
})

export class ManageDashboardComponent {
    title = 'D. Cube Hotel Management System';
    user: any;
    hideElement = false;
    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router
    ) {
        this.user = JSON.parse(localStorage.getItem('currentUser'));
        this.router.events.subscribe((event) => {
            if (event instanceof NavigationEnd) {
                if (event.url === '/managedashboard') {
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
