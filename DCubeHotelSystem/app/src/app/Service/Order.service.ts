//// Main dependencies
//import { Injectable } from '@angular/core';
//import { Headers, Http, Request, RequestMethod, Response } from '@angular/http';
//import { Observable } from 'rxjs/Observable';

//// Models
//import { Order } from '../models/order.model';

//// Mocks
//import { OrdersMock } from '../mocks/orders.mock';

//@Injectable()
//export class OrderService {
//    BASE_API_URL: string = 'http://localhost:50652';
//    ORDER_API_URL: string = `{BASE_API_URL}/api/orders`;

//    ordersMock: any;

//    // Constructor
//    constructor(private http: Http) {
//        // Setting up the orders
//        this.ordersMock = new OrdersMock();
//    }

//    // Load All Orders
//    loadOrders(tableId?: string): Observable<Order[]> {
//        debugger
//        // Call to API here
//        return this.http.get('/db.json')
//            .map((res: Response) => {
//                return res.json()['orders'];
//            });
//    }

//    // Create Order
//    createOrder(body: Order): Observable<Order> {
//        // Call to API here
//        return this.http.post('/db.json', this.getBody(body))
//            .map((res: Response) => {
//                return res.json()['orders'][0];
//            });
//    }

//    // Load single Order
//    loadOrder(): Observable<Order> {
//        // Call to API here
//        return this.http.get('/db.json')
//            .map((res: Response) => {
//                return res.json()['orders'][0];
//            });
//    }

//    // Update single order
//    updateOrder(body: Order): Observable<Order> {
//        // Call to API here
//        return this.http.put('/db.json', this.getBody(body))
//            .map((res: Response) => {
//                return res.json()['orders'][0];
//            });
//    }

//    deleteOrder(orderId: number): Observable<Order> {
//        return this.http.delete('/db.json')
//            .map((res: Response) => res.json());
//    }

//    getBody(data: Order) {
//        return JSON.stringify(data);
//    }

//}
import { Injectable, ComponentRef, TemplateRef, EventEmitter, RendererFactory2 } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';


@Injectable()
export class OrderServices {
    constructor(private _http: Http) {
        this._http = _http;
    }

    get(url: string): Observable<any> {
        return this._http.get(url)
            .map((response: Response) => <any>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    gets(url: string): Observable<any> {
        return this._http.get(url)
            .map((response: Response) => <any>response.json())
            .catch(this.handleError);
    }


    post(url: string, model: any): Observable<any> {
        let body = JSON.stringify(model);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this._http.post(url, body, options)
            .map((response: Response) => <any>response.json())
            .catch(this.handleError);
    }

    posts(url: string, model: any): Observable<any> {
        let body = JSON.stringify(model);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this._http.post(url, body, options)
            .map((response: Response) => <any>response.json())
            .catch(this.handleError);
    }
    put(url: string, id: number, model: any): Observable<any> {
        let body = JSON.stringify(model);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this._http.put(url + id, body, options)
            .map((response: Response) => <any>response.json())
            .catch(this.handleError);
    }

    delete(url: string, id: number): Observable<any> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this._http.delete(url + id, options)
            .map((response: Response) => <any>response.json())
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }

    getMenuItems() {
        return this._http.get("api/MenuItemAPI/get")
            .map((responseData) => responseData.json());
    }

    getMenu() {
        return this._http.get("api/MenuAPI/get")
            .map((responseData) => responseData.json());
    }
}

