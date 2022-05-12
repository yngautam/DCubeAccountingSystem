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
var InventoryDashboardComponent = /** @class */ (function () {
    function InventoryDashboardComponent(activatedRoute, router) {
        var _this = this;
        this.activatedRoute = activatedRoute;
        this.router = router;
        this.title = 'D. Cube Hotel Management System';
        this.hideElement = false;
        this.user = JSON.parse(localStorage.getItem('currentUser'));
        this.router.events.subscribe(function (event) {
            if (event instanceof router_1.NavigationEnd) {
                if (event.url === '/InventoryDashboard') {
                    _this.hideElement = true;
                }
                else {
                    _this.hideElement = false;
                }
            }
        });
    }
    InventoryDashboardComponent.prototype.logout = function () {
        localStorage.clear();
        this.router.navigate(['/login']);
    };
    InventoryDashboardComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: "InventoryDashboard-app",
            templateUrl: 'InventoryDashboard.component.html',
            styleUrls: ['InventoryDashboard.component.css']
        })
    ], InventoryDashboardComponent);
    return InventoryDashboardComponent;
}());
exports.InventoryDashboardComponent = InventoryDashboardComponent;
