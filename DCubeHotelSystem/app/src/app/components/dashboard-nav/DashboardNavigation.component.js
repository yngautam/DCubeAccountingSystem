"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var DashboardNavigationComponent = /** @class */ (function () {
    function DashboardNavigationComponent(activatedRoute, router) {
        this.activatedRoute = activatedRoute;
        this.router = router;
        this.user = JSON.parse(localStorage.getItem('currentUser'));
    }
    DashboardNavigationComponent.prototype.logout = function () {
        localStorage.clear();
        this.router.navigate(['/login']);
    };
    DashboardNavigationComponent = __decorate([
        core_1.Component({
            selector: "dashboard-nav-app",
            templateUrl: './DashboardNavigation.component.html',
            styleUrls: ['./DashboardNavigation.component.css']
        })
    ], DashboardNavigationComponent);
    return DashboardNavigationComponent;
}());
exports.DashboardNavigationComponent = DashboardNavigationComponent;
