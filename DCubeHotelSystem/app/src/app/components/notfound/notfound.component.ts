import { Component } from '@angular/core';

@Component({
    selector: 'notfound',
    templateUrl: './notfound.component.html'
})
export class NotFoundComponent {
    title: string = '404 Not Found!'
    message: string = 'The path is not correct!';
}