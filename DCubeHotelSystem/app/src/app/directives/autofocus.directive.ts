import { Directive, AfterViewInit, ElementRef } from '@angular/core';
 
@Directive({
  selector: '[setAutoFocus]'
})
export class AutofocusDirective implements AfterViewInit {

    // Constructor
    constructor(private el: ElementRef) {}
 
    // Overrides directive life cycle method
    ngAfterViewInit() {
        this.el.nativeElement.focus();
    }
}