// Core Modules                                          
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { APP_BASE_HREF, CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AngularDateTimePickerModule } from 'angular2-datetimepicker';
import { RouterModule, Routes } from '@angular/router';
import { BsModalModule } from 'ng2-bs3-modal';
import { DatePipe } from '@angular/common';

// Third Party Modules                                 
import { ModalModule } from 'ngx-bootstrap';
import { BsDatepickerModule } from 'ngx-bootstrap';
import { TimepickerModule } from 'ngx-bootstrap';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { SelectDropDownModule } from 'ngx-select-dropdown';

// For development/debugging mode only
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

// Custom Modules                                      
import { DirectivesModule } from './directives/directives.module';
import { FiltersModule } from './filters/filters.module';

// Reducers                                            
import { OrdersReducer } from './reducers/orders.reducer';
import { TablesReducer } from './reducers/tables.reducer';
import { CategoriesReducer } from './reducers/categories.reducer';
import { UsersReducer } from './reducers/users.reducer';
import { ProductsReducer } from './reducers/products.reducer';
import { CustomersReducer } from './reducers/customers.reducer';
import { TicketsReducer } from './reducers/tickets.reducer';

// Services
import { DepartmentService } from '././Service/Department.service';
import { MenuService } from './Service/Menu.service';
import { MenuCategoryService } from '././Service/MenuCategory.service';
import { tableService } from './Service/tableService';
import { MenuItemPortionService } from '././././Service/MenuItemPortion.services';
import { MenuItemService } from '././Service/MenuItem.service';
import { CategorysService } from '././Service/Category.services';
import { InventoryItemService } from '././Service/InventoryItem.service';
import { InventoryReceiptService } from '././Service/InventoryReceipt.service';
import { InventoryReceiptDetailsService } from '././Service/InventoryReceiptDetails.service';
import { PurchaseService } from '././Service/purchase.service';
import { JournalVoucherService } from '././Service/journalVoucher.service';
import { AccountTypeService } from '././Service/account-type.service';
import { AccountService } from '././Service/account.service';
import { AccountTransactionTypeService } from '././Service/account-trans-type.service';
import { PeriodicConsumptionItemService } from './Service/peroidic-consumption-item.service';
import { PeriodicConsumptionService } from './Service/periodic-consumption.service';
import { MenuConsumptionService } from '././Service/MenuConsumptionService';
import { MenuConsumptionDetailsService } from '././Service/MenuConsumptionDetailsService';
import { ReservationCustomerService } from './Service/customer.services';

//User And role services
import { AuthenticationService } from './Service/authentication.service';
import { UsersService } from './Service/user.service';
import { LoginService } from './Service/login.service';
import { AuthGuard } from './guard/auth.guard';
import { RoleService } from './Service/role.service';
import { UserRoleService } from './Service/userRole.service';
import { AccountTransValuesService } from './Service/accountTransValues.service'
import { FileService } from './Service/file.service';
import { PurchaseDetailsService } from './Service/PurchaseDetails.service';
import { ReservationService } from './Service/reservation.services';

// Components
//Dashboard Component 
import { AppComponent } from './components/app/app.component';
import { FileUploadComponent } from './components/file-upload/file-upload.component';
import { LoginComponent } from './components/login/login.component';
import { NotFoundComponent } from './components/notfound/notfound.component';

import { DashboardComponent } from './components/dashboard/dashboard.component';
import { DashboardNavigationComponent } from './components/dashboard-nav/DashboardNavigation.component';

//AccountDashboard components
import { AccountDashboardComponent } from './components/AccountDashboard/AccountDashboard.component';
import { AccountComponent } from './components/AccountDashboard/account/account.component';
import { AccountTransactionTypeComponent } from './components/AccountDashboard/account-transaction-type/account-transaction-type.component';
import { AccountTypeComponent } from './components/AccountDashboard/account-type/account-type.component';
import { ContraComponent } from './components/AccountDashboard/contra/contra.component';
import { CreditNoteComponent } from './components/AccountDashboard/credit-note/cerdit-note.component';
import { DebitNoteComponent } from './components/AccountDashboard/debit-note/debit-note.component';
import { JournalVouchercomponent } from './components/AccountDashboard/journal/journaVoucher.component';
import { SalesBillingComponent } from './components/AccountDashboard/sales-billing/sales-billing.component';
import { SalesBillingvatComponent } from './components/AccountDashboard/sales-billingvat/sales-billingvat.component';
import { SalesBillingDetailComponent } from './components/AccountDashboard/sales-billing-detail/sales-billing-details.component';
import { PurchaseComponent } from './components/AccountDashboard/purchase/purchase.component';
import { PurchaseDetailsComponent } from './components/AccountDashboard/purchase/purchaseDetail/purchaseDetail.component';
import { ReceiptComponent } from './components/AccountDashboard/receipt/receipt.component';
import { PaymentComponent } from './components/AccountDashboard/payment/payment.component';

//InventoryDashboard Component
import { InventoryDashboardComponent } from './components/InventoryDashboard/InventoryDashboard.Component';
import { MenuCategoryComponent } from './components/InventoryDashboard/MenuCategory/MenuCategory.component';
import { MenuItemComponent } from './components/InventoryDashboard/MenuItem/MenuItem.component';
import { StockInHandComponent } from './components/InventoryDashboard/stock-in-hand/stock-in-hand.component';
import { StockDamageComponent } from './components/InventoryDashboard/stock-damage/stock-damage.component';
import { StockDamageDetailsComponent } from './components/InventoryDashboard/stock-damage-details/stock-damage-details.component';
import { UnitTypeComponent } from './components/InventoryDashboard/UnitType/UnitType.Component';

//ManageDashboard Component
import { ManageDashboardComponent } from './components/managedashboard/managedashboard.component';
import { UserComponent } from './components/managedashboard/user/user.component';
import { RoleComponent } from './components/managedashboard/role/role.component';
import { RoleModuleComponent } from './components/managedashboard/rolemodule/rolemodule.component';
import { DepartmentComponent } from './components/managedashboard/Department/Department.component';
import { CompanyComponent } from './components/managedashboard/company/company.component';
import { FinancialYearComponent } from './components/managedashboard/FinancialYear/FinancialYear.component';
import { RoleAssignmentComponent } from './components/managedashboard/role-assign/role-assign.component';
import { RoleNameComponent } from './components/managedashboard/role-assign/role-name/role-name.component';
import { UserPermissionComponent } from './components/managedashboard/user-permission/user-permission.component';

//ReportDashboard Component
import { ReportDashboardComponent } from './components/ReportDashboard/ReportDashboard.component';
import { TrialBalanceComponent } from './components/ReportDashboard/Report/TrialBalance/TrialBalance.component';
import { AccountLedgerViewComponent } from './components/ReportDashboard/Report/LedgerView/AccountLedgerView.Component';
import { AccountSaleBookComponent } from './components/ReportDashboard/Report/SaleBook/AccountSaleBook.Component';
import { AccountSaleBookItem } from './components/ReportDashboard/Report/SaleBookItem/AccountSaleBookItem.Component';
import { AccountSaleBookDaywise } from './components/ReportDashboard/Report/SaleBookDate/AccountSaleBookDatewise.Component';
import { AccountSaleBookCustomer } from './components/ReportDashboard/Report/SaleBookCustomer/AccountSaleBookCustomer.Component';
import { MaterializedViewComponent } from './components/ReportDashboard/Report/materialized view/materializedview.component';
import { BillReturnViewComponent } from './components/ReportDashboard/Report/BillReturnView/BillReturnView.component';
import { AccountProfitAndLossComponent } from './components/ReportDashboard/Report/ProfitAndLoss/AccountProfitAndLoss.Component';
import { BalanceSheetComponent } from './components/ReportDashboard/Report/balance-sheet/balance-sheet.component';

// Routes
const appRoutes: Routes = [
    {
        path: '',
        redirectTo: '/login',
        pathMatch: 'full'
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'file-upload',
        component: FileUploadComponent,
        pathMatch: 'full'
    },
    {
        path: 'accountdashboard',
        component: AccountDashboardComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'accountdashboard/purchase',
        component: PurchaseComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'accountdashboard/sales-billing',
        component: SalesBillingComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'accountdashboard/sales-billingvat',
        component: SalesBillingvatComponent,
        canActivate: [AuthGuard]
    },    
    {
        path: 'accountdashboard/account',
        component: AccountComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'accountdashboard/accountType',
        component: AccountTypeComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'accountdashboard/accounttransType',
        component: AccountTransactionTypeComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'accountdashboard/receipt',
        component: ReceiptComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'accountdashboard/payment',
        component: PaymentComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'accountdashboard/contra',
        component: ContraComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'accountdashboard/credit-note',
        component: CreditNoteComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'accountdashboard/debit-note',
        component: DebitNoteComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'accountdashboard/journalVoucher',
        component: JournalVouchercomponent,
        pathMatch: 'full'
    },
    {
        path: 'InventoryDashboard',
        component: InventoryDashboardComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'InventoryDashboard/menucategory',
        component: MenuCategoryComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'InventoryDashboard/products',
        component: MenuItemComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'InventoryDashboard/stock-damage',
        component: StockDamageComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'InventoryDashboard/stockinhand',
        component: StockInHandComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'InventoryDashboard/unittype',
        component: UnitTypeComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'managedashboard',
        component: ManageDashboardComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'managedashboard/department',
        component: DepartmentComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'managedashboard/userRole',
        component: RoleAssignmentComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'managedashboard/company',
        component: CompanyComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'managedashboard/financial',
        component: FinancialYearComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'managedashboard/dashboard',
        component: DashboardComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'managedashboard/user',
        component: UserComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'managedashboard/role',
        component: RoleComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'managedashboard/rolemodule',
        component: RoleModuleComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'reportdashboard/TrialBalance',
        component: TrialBalanceComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'reportdashboard/BalanceSheet',
        component: BalanceSheetComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'reportdashboard/ProfitLoss',
        component: AccountProfitAndLossComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'reportdashboard/AccountLedgerView',
        component: AccountLedgerViewComponent,
        pathMatch: 'full', canActivate: [AuthGuard]
    },
    {
        path: 'reportdashboard/SaleBook',
        component: AccountSaleBookComponent,
        pathMatch: 'full', canActivate: [AuthGuard]
    },
    {
        path: 'reportdashboard/SaleBookItem',
        component: AccountSaleBookItem,
        pathMatch: 'full', canActivate: [AuthGuard]
    },
    {
        path: 'reportdashboard/SaleBookDatewise',
        component: AccountSaleBookDaywise,
        pathMatch: 'full', canActivate: [AuthGuard]
    },
    {
        path: 'reportdashboard/SaleBookCustomer',
        component: AccountSaleBookCustomer,
        pathMatch: 'full', canActivate: [AuthGuard]
    },
    {
        path: 'reportdashboard/materializedview',
        component: MaterializedViewComponent,
        pathMatch: 'full', canActivate: [AuthGuard]
    },
    {
        path: 'reportdashboard/BillReturnView',
        component: BillReturnViewComponent,
        pathMatch: 'full', canActivate: [AuthGuard]
    },
    {
        path: 'reportdashboard',
        component: ReportDashboardComponent,
        pathMatch: 'full', canActivate: [AuthGuard]
    },
    {
        path: 'receipt',
        component: ReceiptComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'role-name/:roleid',
        component: RoleNameComponent,
        canActivate: [AuthGuard]
    },
    {
        path: '404', component: NotFoundComponent
    },
    {
        path: '**', redirectTo: '/404'
    }
];

@NgModule({
    imports: [
        BsModalModule,
        BrowserModule,
        SelectDropDownModule,
        HttpModule,
        FormsModule,
        HttpClientModule,
        ReactiveFormsModule,
        RouterModule.forRoot(appRoutes, { enableTracing: true, useHash: true }),
        DirectivesModule,
        FiltersModule,
        ModalModule.forRoot(),
        TimepickerModule.forRoot(),
        BsDatepickerModule.forRoot(),
        AngularDateTimePickerModule,
        StoreModule.forRoot({
            orders: OrdersReducer,
            tables: TablesReducer,
            products: ProductsReducer,
            user: UsersReducer,
            categories: CategoriesReducer,
            customers: CustomersReducer,
            tickets: TicketsReducer
        }),
        EffectsModule.forRoot([
        ]),
        ModalModule.forRoot(),

        // Remove this in Production environment
        StoreDevtoolsModule.instrument({ maxAge: 50 })
    ],
    declarations: [
        //Dashboard components
        AppComponent,
        FileUploadComponent,
        LoginComponent,
        NotFoundComponent,
        DashboardComponent,
        DashboardNavigationComponent,

        //AccountDashboard components
        AccountDashboardComponent,
        AccountComponent,
        AccountTransactionTypeComponent,
        AccountTypeComponent,
        ContraComponent,
        CreditNoteComponent,
        DebitNoteComponent,
        JournalVouchercomponent,
        SalesBillingComponent,
        SalesBillingDetailComponent,
        SalesBillingvatComponent,
        PurchaseComponent,
        PurchaseDetailsComponent,
        ReceiptComponent,
        PaymentComponent,

        //Inventory Dashboard Component
        InventoryDashboardComponent,
        MenuCategoryComponent,
        MenuItemComponent,
        StockInHandComponent,
        StockDamageComponent,
        StockDamageDetailsComponent,
        UnitTypeComponent,

        //Manage Dashboard Component
        DepartmentComponent,
        ManageDashboardComponent,
        UserComponent,
        RoleComponent,
        RoleModuleComponent,
        FinancialYearComponent,
        CompanyComponent,
        RoleAssignmentComponent,
        RoleNameComponent,
        UserPermissionComponent,

        //Report Dashboard Component
        TrialBalanceComponent,
        AccountLedgerViewComponent,
        AccountSaleBookComponent,
        AccountSaleBookItem,
        AccountSaleBookDaywise,
        AccountSaleBookCustomer,
        AccountProfitAndLossComponent,
        ReportDashboardComponent,
        BalanceSheetComponent,
        MaterializedViewComponent,
        BillReturnViewComponent
    ],

    providers: [
        // API services
        DepartmentService,
        MenuService,
        tableService,
        MenuCategoryService,
        MenuItemService,
        ReservationService,
        MenuItemPortionService,
        InventoryItemService,
        InventoryReceiptService,
        InventoryReceiptDetailsService,
        PeriodicConsumptionService,
        PeriodicConsumptionItemService,
        MenuConsumptionService,
        MenuConsumptionDetailsService,
        JournalVoucherService,
        PurchaseDetailsService,
        PurchaseService,
        CategorysService,
        AuthGuard,
        UsersService,
        LoginService,
        AuthenticationService,
        RoleService,
        UserRoleService,
        AccountTypeService,
        AccountService,
        AccountTransactionTypeService,
        AccountTransValuesService,
        ReservationCustomerService,
        DatePipe,
        FileService,
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
    // Code related to App module
}
