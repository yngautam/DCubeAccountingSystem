import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    selector: 'dcubehotel-dashboard',
    styleUrls: ['./dashboard.component.css'],
    templateUrl: './dashboard.component.html'
})
export class DashboardComponent {
    title = 'D. Cube Billing Management System';

    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router
    ){}

    logout() {
        localStorage.clear();
        this.router.navigate(['/login']);
    }
}