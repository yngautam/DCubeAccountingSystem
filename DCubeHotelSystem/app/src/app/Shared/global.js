"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Global = /** @class */ (function () {
    function Global() {
    }
    //Dashboard
    Global.BASE_LOGIN_ENDPOINT = '/api/LoginAPI/';
    //Account Dashboard
    Global.BASE_ACCOUNT_ENDPOINT = '/api/AccountAPI/';
    Global.BASE_ACCOUNTTRANSTYPE_ENDPOINT = '/api/AccountTransactionTypeAPI/';
    Global.BASE_ACCOUNTTYPE_ENDPOINT = '/api/AccountTypeAPI/';
    Global.BASE_FILE_UPLOAD_ENDPOINT = 'api/FileUploadAPI/';
    Global.BASE_JOURNALVOUCHER_ENDPOINT = '/api/AccountTransactionAPI/';
    Global.BASE_HOST_ENDPOINT = 'http://' + location.hostname + ':' + location.port + '/';
    Global.BASE_JOURNAL_ENDPOINT = '/api/AccountTransValuesAPI/';
    Global.BASE_SCREENCustomerTicket_ENDPOINT = '/api/TicketCustomerAPI';
    Global.BASE_POSBILLING_API_ENDPOINT = 'api/POSBillingAPI/';
    Global.BASE_PURCHASE_ENDPOINT = '/api/PurchaseAPI/';
    Global.BASE_PURCHASEDETAILS_ENDPOINT = '/api/PurchaseDetailsAPI/';
    Global.BASE_SALE_BILLING_ENDPOINT = '/api/SaleBillingAPI/';
    Global.BASE_SALE_BILLING_DETAILS_ENDPOINT = '/api/SalesBillingDetailsAPI/';
    Global.BASE_INVENTORYRECEIPTDETAIL_ENDPOINT = '/api/InventoryReceiptDetailAPI/';
    //Inventory Dasshboard
    Global.BASE_MENUCATEGORY_ENDPOINT = '/api/MenuCategoryAPI/';
    Global.BASE_MENUCONSUMPTION_ENDPOINT = '/api/MenuConsumptionAPI/';
    Global.BASE_MENUCONSUMPTIONDETAILS_ENDPOINT = '/api/MenuConsumptionDetailAPI/';
    Global.BASE_MENUITEM_ConsumptionCategory_ENDPOINT = '/api/MenuConsumptionCategoryFilterAPI/';
    Global.BASE_MENUITEMPORTION_ENDPOINT = '/api/MenuItemPortionAPI/';
    Global.BASE_MENUITEM_ENDPOINT = '/api/MenuItemAPI/';
    Global.BASE_PERIODICCONSUMPTION_ENDPOINT = '/api/PeriodicConsumptionAPI/';
    Global.BASE_PERIODICCONSUMPTIONITEM_ENDPOINT = '/api/PeriodicConsumptionItemAPI/';
    Global.BASE_COSTDETAILS_ENDPOINT = '/api/CostDetailsAPI/';
    Global.BASE_STOCKINHAND_ENDPOINT = '/api/StockInHand/';
    Global.BASE_UNITTYPE_ENDPOINT = '/api/UnitType/';
    //Manage Dashboard
    Global.BASE_COMPANY_ENDPOINT = 'api/Company/';
    Global.BASE_FINANCIAL_YEAR_ENDPOINT = 'api/FinancialYearAPI/';
    Global.BASE_DEPARTMENT_ENDPOINT = '/api/DepartmentAPI/';
    Global.BASE_ROLES_ENDPOINT = '/api/RoleAPI/';
    Global.BASE_USERROLE_ENDPOINT = '/api/userRoleAPI/';
    Global.BASE_USERACCOUNT_ENDPOINT = '/api/UserAccountAPI/';
    Global.BASE_USER_ENDPOINT = '/api/userapi/';
    //Report Dashboard
    Global.BASE_BALANCE_SHEET_ENDPOINT = 'api/AccountBalanceSheetAPI/';
    Global.BASE_ACCOUNT_BillReturnView_ENDPOINT = '/api/BillReturnView/';
    Global.BASE_ACCOUNTLEDGERVIEW_ENDPOINT = '/api/AccountLedgerView/';
    Global.BASE_ACCOUNT_MaterializedView_ENDPOINT = '/api/MaterializedView/';
    Global.BASE_ACCOUNTPROFITANDLOSS_ENDPOINT = '/api/AccountProfitLoss/';
    Global.BASE_ACCOUNTSALEBOOK_ENDPOINT = '/api/AccountSaleBook/';
    Global.BASE_ACCOUNTTRIALBALANCE_ENDPOINT = '/api/AccountTrialBalance/';
    return Global;
}());
exports.Global = Global;
