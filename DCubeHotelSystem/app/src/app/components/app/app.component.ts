import { OnInit, Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';

// Models
import { Company } from '../../models/company.model';
import { Global } from '../../Shared/global';

// Services
import { ReservationService } from '../../Service/reservation.services';
import { IFinancialYear } from '../../Model/FinancialYear';
 
@Component({
    selector: 'dcubehotel-app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']       
})
export class AppComponent implements OnInit {
    message$: Observable<string>;
    companies: Company[];
    financialYears: IFinancialYear[];
    company: Company;
    msg: string;

	constructor (
        private _reservationService: ReservationService
    ) { }

    ngOnInit () {
        this.loadCompanies();
        this.loadFinancialYear();
    }

    loadCompanies() {
        this._reservationService.get(Global.BASE_COMPANY_ENDPOINT)
            .subscribe(
                companies => {
                    this.company = companies.length && companies[0];
                    localStorage.setItem("company", JSON.stringify(this.company));
                },
                error => this.msg = <any>error
            );
    }

    loadFinancialYear(): void {
        this._reservationService.get(Global.BASE_FINANCIAL_YEAR_ENDPOINT)
            .subscribe(
                financials => { 
                    this.financialYears = financials;
                    localStorage.setItem('currentYear', JSON.stringify(this.financialYears[0]));
                },
                error => this.msg = <any>error
            );
    }

    changeFinalcialYear(index: any) {
        if (index) {
            localStorage.setItem('currentYear', JSON.stringify(this.financialYears[index]));
        }
    }

    getUTCFullYear() {
        var year = new Date();
        return year.getFullYear();
    }
}
