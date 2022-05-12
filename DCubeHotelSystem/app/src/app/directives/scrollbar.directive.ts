import  { Directive, ElementRef, OnInit } from '@angular/core';

// Accessing global variable
declare var $:any;

// Directive declaration
@Directive({
    selector: 'scrollbar',
    host: {'class':'mCustomScrollbar'}, 
})
export class ScrollbarDirective implements OnInit {
    // Variables
    el: ElementRef;

    // Constructor
    constructor(el:ElementRef) {
        this.el = el;
    }

    // Override directive life cycle method
    ngOnInit() {
        // Applying mCustonScrollbar pugin to the attached element
        $(this.el.nativeElement).mCustomScrollbar({
            autoHideScrollbar: true,
            theme: "dark-3",
            setHeight: "340px"
        });
    }
}