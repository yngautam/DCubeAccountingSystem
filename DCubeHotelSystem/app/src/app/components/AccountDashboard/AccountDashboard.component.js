"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var AccountDashboardComponent = /** @class */ (function () {
    function AccountDashboardComponent(activatedRoute, router) {
        var _this = this;
        this.activatedRoute = activatedRoute;
        this.router = router;
        this.hideElement = false;
        this.user = JSON.parse(localStorage.getItem('currentUser'));
        this.router.events.subscribe(function (event) {
            if (event instanceof router_1.NavigationEnd) {
                if (event.url === '/accountdashboard') {
                    _this.hideElement = true;
                }
                else {
                    _this.hideElement = false;
                }
            }
        });
    }
    /**
     * Logout user
     */
    AccountDashboardComponent.prototype.logout = function () {
        localStorage.clear();
        this.router.navigate(['/login']);
    };
    AccountDashboardComponent = __decorate([
        core_1.Component({
            selector: "accountdashboard-app",
            templateUrl: './AccountDashboard.component.html',
            styleUrls: ['./AccountDashboard.component.css']
        })
    ], AccountDashboardComponent);
    return AccountDashboardComponent;
}());
exports.AccountDashboardComponent = AccountDashboardComponent;
