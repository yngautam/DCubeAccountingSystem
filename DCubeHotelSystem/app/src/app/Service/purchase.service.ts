
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/catch';


@Injectable()

export class PurchaseService {

    constructor(private _http: Http) { }


    get(url: string): Observable<any> {
        debugger
        return this._http.get(url)
            .map((response: Response) => <any>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
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

    put(url: string, Id: number, model: any): Observable<any> {
        debugger;
        let body = JSON.stringify(model,);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this._http.put(url + Id, body, options)
            .map((response: Response) => <any>response.json())
            .catch(this.handleError);
    }

    delete(url: string, model: any): Observable<any> {
        debugger;
        let body = JSON.stringify(model);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers, body: body });
        return this._http.delete(url, options)
            .map((response: Response) => <any>response.json())
            .catch(this.handleError);
    }


    getInventoryItems() {
        return this._http.get("/api/InventoryItemAPI/get")
            .map((responseData) => responseData.json());
    }

    getSalesItems() {
        return this._http.get("/api/MenuCategoryItemAPI")
            .map((responseData) => responseData.json());
    }

    getAccounts() {

        return this._http.get("/api/AccountAPI/get")
            .map((responseData) => responseData.json());
    } 

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || 'Server error');
    }



}

