using DCubeHotelBusinessLayer.Accounts;
using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    //ScreenOrderItemRequest
    [UserRoleAuthorize]
    public class POSBillingAPIController : BaseAPIController
    {
        private IDCubeRepository<ScreenOrderItemRequest> ScreenOrderItemRequestRepo = null;
        private IDCubeRepository<Ticket> TicketRepository = null;
        private IDCubeRepository<AccountTransaction> AccountTranastionRepository = null;
        private IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository = null; 
        private IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository = null; 
        private IDCubeRepository<AccountType> AccountTypeRepository = null; 
        private IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository = null;
        private IDCubeRepository<Account> AccountRepository = null;
        private IDCubeRepository<SaleBillingBook> SaleBillingBookRepository = null;
        private IDCubeRepository<Table> TableRepository = null;

        public POSBillingAPIController()
        {
            this.ScreenOrderItemRequestRepo = new DCubeRepository<ScreenOrderItemRequest>();
            this.TicketRepository = new DCubeRepository<Ticket>();
            this.AccountTranastionRepository = new DCubeRepository<AccountTransaction>();
            this.AccountTransactionValueRepository = new DCubeRepository<AccountTransactionValue>();
            this.TransactionDocumentRepository = new DCubeRepository<AccountTransactionDocument>();
            this.AccountTypeRepository = new DCubeRepository<AccountType>();
            this.AccountTransactionTypeRepository = new DCubeRepository<AccountTransactionType>();
            this.AccountRepository = new DCubeRepository<Account>();
            this.SaleBillingBookRepository = new DCubeRepository<SaleBillingBook>();
            this.TableRepository = new DCubeRepository<Table>();
        }
        [HttpGet]
        public HttpResponseMessage Get([FromUri]string fromDate, [FromUri]string toDate, string TransactionTypeId)
        {
            List<SaleBillingBook> listSaleBillingBook = new List<SaleBillingBook>();
            try
            {
                listSaleBillingBook = SaleBookBusiness.GetSaleBillingBook(TableRepository, TicketRepository, TransactionDocumentRepository, AccountTypeRepository, AccountTransactionTypeRepository, AccountRepository, AccountTransactionValueRepository,  SaleBillingBookRepository, ScreenOrderItemRequestRepo, fromDate.Trim(), toDate.Trim(), int.Parse(TransactionTypeId.Trim()));
            }
            catch (Exception ex)
            {

            }
            return ToJson(listSaleBillingBook.AsEnumerable());
        }
        [HttpGet]
        public HttpResponseMessage Get([FromUri]string TicketId, [FromUri]string AccountId)
        {
            int result = 1;
            result = TicketBusiness.TicketUpdateTransferTableToCustomer(TicketRepository, TicketId, AccountId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
