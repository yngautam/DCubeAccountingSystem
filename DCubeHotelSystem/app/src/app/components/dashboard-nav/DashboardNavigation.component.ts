import { Component } from "@angular/core"
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    selector: "dashboard-nav-app",
    templateUrl: './DashboardNavigation.component.html',
    styleUrls: ['./DashboardNavigation.component.css']
})

export class DashboardNavigationComponent {
    user: any;

    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router
        ){
        this.user = JSON.parse(localStorage.getItem('currentUser'));
    }

    logout () {
        localStorage.clear();
        this.router.navigate(['/login']);
    }
}
