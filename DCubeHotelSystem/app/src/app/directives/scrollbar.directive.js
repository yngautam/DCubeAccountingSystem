"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
// Directive declaration
var ScrollbarDirective = /** @class */ (function () {
    // Constructor
    function ScrollbarDirective(el) {
        this.el = el;
    }
    // Override directive life cycle method
    ScrollbarDirective.prototype.ngOnInit = function () {
        // Applying mCustonScrollbar pugin to the attached element
        $(this.el.nativeElement).mCustomScrollbar({
            autoHideScrollbar: true,
            theme: "dark-3",
            setHeight: "340px"
        });
    };
    ScrollbarDirective = __decorate([
        core_1.Directive({
            selector: 'scrollbar',
            host: { 'class': 'mCustomScrollbar' },
        })
    ], ScrollbarDirective);
    return ScrollbarDirective;
}());
exports.ScrollbarDirective = ScrollbarDirective;
