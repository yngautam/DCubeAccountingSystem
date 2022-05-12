import { NgModule } from '@angular/core';
import { AutofocusDirective } from './autofocus.directive';
import { ScrollbarDirective } from './scrollbar.directive';


@NgModule({
    declarations: [
        AutofocusDirective,
        ScrollbarDirective
    ],
    exports: [
        AutofocusDirective,
        ScrollbarDirective
    ]
})
export class DirectivesModule{}