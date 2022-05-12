using DCubeHotelBusinessLayer.ExtraModel;
using DCubeHotelBusinessLayer.Sales;
using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class SaleBillingAPIController : BaseAPIController
    {
        private IDCubeRepository<AccountType> AccountTypeRepository;
        private IDCubeRepository<AccountTransactionValue> accValueRepository;
        private IDCubeRepository<AccountTransactionDocument> accTransDocRepository;
        private IDCubeRepository<AccountTransactionType> accTransTypeRepository;
        private IDCubeRepository<Account> AccountRepository;
        private IDCubeRepository<AccountTransaction> accRepository;
        private IDCubeRepository<ScreenOrderDetails> ScreenOrderDetailsRepository;
        private IDCubeRepository<Ticket> TicketRepository;
        private IDCubeRepository<Order> OrderRepository;
        private DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepo;
        private DCubeRepository<MenuItem> MenuItemRepo;
        private DCubeRepository<MenuItemPortion> Menuportionrepo;
        private DCubeRepository<MenuItemPortionPriceRange> MenuportionPriceRangerepo;
        private DCubeRepository<FinancialYear> FinancialYeaRepo;

        public SaleBillingAPIController()
        {
            this.AccountTypeRepository = (IDCubeRepository<AccountType>)new DCubeRepository<AccountType>();
            this.accValueRepository = (IDCubeRepository<AccountTransactionValue>)new DCubeRepository<AccountTransactionValue>();
            this.accTransDocRepository = (IDCubeRepository<AccountTransactionDocument>)new DCubeRepository<AccountTransactionDocument>();
            this.accTransTypeRepository = (IDCubeRepository<AccountTransactionType>)new DCubeRepository<AccountTransactionType>();
            this.AccountRepository = (IDCubeRepository<Account>)new DCubeRepository<Account>();
            this.accRepository = (IDCubeRepository<AccountTransaction>)new DCubeRepository<AccountTransaction>();
            this.ScreenOrderDetailsRepository = (IDCubeRepository<ScreenOrderDetails>)new DCubeRepository<ScreenOrderDetails>();
            this.TicketRepository = (IDCubeRepository<Ticket>)new DCubeRepository<Ticket>();
            this.OrderRepository = (IDCubeRepository<Order>)new DCubeRepository<Order>();
            this.MenuCategoryRepo = new DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
            this.MenuItemRepo = new DCubeRepository<MenuItem>();
            this.Menuportionrepo = new DCubeRepository<MenuItemPortion>();
            this.MenuportionPriceRangerepo = new DCubeRepository<MenuItemPortionPriceRange>();
            this.FinancialYeaRepo = new DCubeRepository<FinancialYear>();
        }

        [HttpGet]
        public HttpResponseMessage GetItem(
          [FromUri] string fromDate,
          [FromUri] string toDate,
          [FromUri] string TransactionTypeId,
          [FromUri] string ReportType,
          [FromUri] string BranchId)
        {
            List<ItemSale> itemSaleList = new List<ItemSale>();
            return this.ToJson(!(ReportType != "0") ? (object)SaleBillingBusiness.GetScreenSaleBillItem(this.AccountTypeRepository, this.AccountRepository, this.accRepository, this.accValueRepository, this.accTransTypeRepository, this.TicketRepository, this.OrderRepository, this.accTransDocRepository, this.MenuCategoryRepo, this.MenuItemRepo, this.Menuportionrepo, this.MenuportionPriceRangerepo, fromDate, toDate, int.Parse(TransactionTypeId), int.Parse(BranchId)) : (object)SaleBillingBusiness.GetScreenSaleBillItem(this.AccountTypeRepository, this.AccountRepository, this.accRepository, this.accValueRepository, this.accTransTypeRepository, this.TicketRepository, this.OrderRepository, this.accTransDocRepository, this.MenuCategoryRepo, this.MenuItemRepo, this.Menuportionrepo, this.MenuportionPriceRangerepo, fromDate, toDate, int.Parse(TransactionTypeId), int.Parse(BranchId), int.Parse(ReportType)));
        }

        [HttpGet]
        public HttpResponseMessage Get(
          [FromUri] string fromDate,
          [FromUri] string toDate,
          string TransactionTypeId)
        {
            int BranchId = 0;
            List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
            return this.ToJson((object)SaleBillingBusiness.GetScreenSaleBilling(this.AccountTypeRepository, this.AccountRepository, this.accRepository, this.accValueRepository, this.accTransTypeRepository, this.TicketRepository, this.OrderRepository, this.accTransDocRepository, this.MenuCategoryRepo, this.MenuItemRepo, this.Menuportionrepo, this.MenuportionPriceRangerepo, fromDate, toDate, int.Parse(TransactionTypeId), BranchId));
        }

        [HttpGet]
        public HttpResponseMessage Get(
          [FromUri] string fromDate,
          [FromUri] string toDate,
          string TransactionTypeId,
          string BranchId)
        {
            List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
            return this.ToJson((object)SaleBillingBusiness.GetScreenSaleBilling(this.AccountTypeRepository, this.AccountRepository, this.accRepository, this.accValueRepository, this.accTransTypeRepository, this.TicketRepository, this.OrderRepository, this.accTransDocRepository, this.MenuCategoryRepo, this.MenuItemRepo, this.Menuportionrepo, this.MenuportionPriceRangerepo, fromDate, toDate, int.Parse(TransactionTypeId), int.Parse(BranchId)));
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string TransactionId)
        {
            AccountTransaction accountTransaction = new AccountTransaction();
            try
            {
                accountTransaction = SaleBillingBusiness.ScreenSaleBilling(this.AccountRepository, this.accRepository, this.accValueRepository, this.accTransDocRepository, this.TicketRepository, this.OrderRepository, this.MenuCategoryRepo, this.MenuItemRepo, this.Menuportionrepo, this.MenuportionPriceRangerepo, TransactionId);
            }
            catch (Exception ex)
            {
            }
            return this.ToJson((object)accountTransaction);
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string CustomerId, [FromUri] string FinancialYear)
        {
            List<TicketReference> ticketReferenceList = new List<TicketReference>();
            return this.ToJson((object)TicketSaleBillingBusiness.GetCustomerTicket(int.Parse(CustomerId), this.TicketRepository, (IDCubeRepository<FinancialYear>)this.FinancialYeaRepo, FinancialYear));
        }

        [HttpGet]
        public HttpResponseMessage GetCustomerSale(
          [FromUri] string CustomerId,
          [FromUri] string InvoiceNo,
          [FromUri] string FinancialYear)
        {
            List<ScreenOrderDetails> screenOrderDetailsList = new List<ScreenOrderDetails>();
            return this.ToJson((object)SaleBillingBusiness.CustomerScreenOrderDetail(this.TicketRepository, this.OrderRepository, CustomerId, InvoiceNo, FinancialYear));
        }

        [HttpPost]
        public HttpResponseMessage Post(AccountTransaction value)
        {
            AccountTransaction accountTransaction = new AccountTransaction();
            int result = 0;
            result = SaleBillingBusiness.Create(this.AccountRepository, this.accTransTypeRepository, this.accRepository, this.accValueRepository, this.accTransDocRepository, this.TicketRepository, this.OrderRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, AccountTransaction value)
        {
            AccountTransaction accountTransaction = new AccountTransaction();
            int result = 0;
            result = SaleBillingBusiness.Edit(this.AccountRepository, this.accRepository, this.accValueRepository, this.accTransTypeRepository, this.accTransDocRepository, this.TicketRepository, this.OrderRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(AccountTransaction value)
        {
            AccountTransaction accountTransaction = new AccountTransaction();
            int result = 0;
            result = SaleBillingBusiness.Delete(this.accRepository, this.accValueRepository, this.accTransDocRepository, this.TicketRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
