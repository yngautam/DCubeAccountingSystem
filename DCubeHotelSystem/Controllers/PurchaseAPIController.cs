using DCubeHotelBusinessLayer.Purchases;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class PurchaseAPIController : BaseAPIController
    {
        private IDCubeRepository<Account> AccountRepository;
        private IDCubeRepository<AccountTransaction> accRepository;
        private IDCubeRepository<PurchaseDetails> purchaseDetailsRepository;
        private IDCubeRepository<AccountTransactionValue> accValueRepository;
        private IDCubeRepository<AccountTransactionDocument> accTransDocRepository;
        private IDCubeRepository<AccountTransactionType> accTransTypeRepository;
        private IDCubeRepository<AccountType> AccountTypeRepository;
        private DCubeRepository<MenuItemPortion> Menuportionrepo;

        public PurchaseAPIController()
        {
            this.purchaseDetailsRepository = (IDCubeRepository<PurchaseDetails>)new DCubeRepository<PurchaseDetails>();
            this.accRepository = (IDCubeRepository<AccountTransaction>)new DCubeRepository<AccountTransaction>();
            this.AccountRepository = (IDCubeRepository<Account>)new DCubeRepository<Account>();
            this.accValueRepository = (IDCubeRepository<AccountTransactionValue>)new DCubeRepository<AccountTransactionValue>();
            this.accTransDocRepository = (IDCubeRepository<AccountTransactionDocument>)new DCubeRepository<AccountTransactionDocument>();
            this.accTransTypeRepository = (IDCubeRepository<AccountTransactionType>)new DCubeRepository<AccountTransactionType>();
            this.AccountTypeRepository = (IDCubeRepository<AccountType>)new DCubeRepository<AccountType>();
            this.Menuportionrepo = new DCubeRepository<MenuItemPortion>();
        }

        [HttpGet]
        public HttpResponseMessage Get(
          [FromUri] string fromDate,
          [FromUri] string toDate,
          string TransactionTypeId)
        {
            int BranchId = 0;
            List<AccountScreen> accountScreenList = new List<AccountScreen>();
            return this.ToJson((object)PurchaseBusiness.GetScrenAccountTransaction(this.AccountTypeRepository, this.AccountRepository, this.accRepository, this.accValueRepository, this.purchaseDetailsRepository, fromDate, toDate, int.Parse(TransactionTypeId), BranchId));
        }

        [HttpGet]
        public HttpResponseMessage Get(
          [FromUri] string fromDate,
          [FromUri] string toDate,
          string TransactionTypeId,
          string BranchId)
        {
            List<AccountScreen> accountScreenList = new List<AccountScreen>();
            return this.ToJson((object)PurchaseBusiness.GetScrenAccountTransaction(this.AccountTypeRepository, this.AccountRepository, this.accRepository, this.accValueRepository, this.purchaseDetailsRepository, fromDate, toDate, int.Parse(TransactionTypeId), int.Parse(BranchId)));
        }

        [HttpGet]
        public HttpResponseMessage Get(string TransactionId)
        {
            AccountScreenValue accountScreenValue = new AccountScreenValue();
            return this.ToJson((object)PurchaseBusiness.ScrenAccountTransaction(this.accRepository, this.accValueRepository, this.purchaseDetailsRepository, (IDCubeRepository<MenuItemPortion>)this.Menuportionrepo, TransactionId));
        }

        [HttpPost]
        public HttpResponseMessage Post(AccountTransaction value)
        {
            AccountTransaction accountTransaction = new AccountTransaction();
            int result = 0;
            result = PurchaseBusiness.Create(this.AccountRepository, this.accTransTypeRepository, this.accRepository, this.accValueRepository, this.purchaseDetailsRepository, this.accTransDocRepository, (IDCubeRepository<MenuItemPortion>)this.Menuportionrepo, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, AccountTransaction value)
        {
            AccountTransaction accountTransaction = new AccountTransaction();
            int result = 0;
            result = PurchaseBusiness.Edit(this.AccountRepository, this.accRepository, this.accValueRepository, this.purchaseDetailsRepository, (IDCubeRepository<MenuItemPortion>)this.Menuportionrepo, value, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(AccountTransaction value)
        {
            AccountTransaction accountTransaction = new AccountTransaction();
            int result = 0;
            result = PurchaseBusiness.Delete(this.accValueRepository, this.accRepository, this.accTransDocRepository, this.purchaseDetailsRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string CustomerId, [FromUri] string FinancialYear)
        {
            List<TicketReference> ticketReferenceList = new List<TicketReference>();
            return this.ToJson((object)PurchaseBusiness.GetCustomerInvoiceNo(int.Parse(CustomerId), this.accRepository, this.accValueRepository, FinancialYear));
        }

        [HttpGet]
        public HttpResponseMessage GetCustomerPurchaseDetail(
          [FromUri] string CustomerId,
          [FromUri] string InvoiceNo,
          [FromUri] string FinancialYear)
        {
            List<PurchaseDetails> purchaseDetailsList = new List<PurchaseDetails>();
            return this.ToJson((object)PurchaseBusiness.CustomerScreenOrderDetail(this.accRepository, this.accValueRepository, this.purchaseDetailsRepository, CustomerId, InvoiceNo, FinancialYear));
        }
    }
}