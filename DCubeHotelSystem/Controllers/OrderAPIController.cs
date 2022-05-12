using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class OrderAPIController : BaseAPIController
    {
        private IDCubeRepository<Order> OrderRepository;
        private IDCubeRepository<ScreenOrder> ScreenOrderRepository;
        private IDCubeRepository<ScreenOrderDetails> ScreenOrderItemsRepository;
        private IDCubeRepository<Ticket> TicketRepository;
        private IDCubeRepository<AccountTransaction> AccountTranastionRepository;
        private IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository;
        private IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository;
        private IDCubeRepository<AccountType> AccountTypeRepository;
        private IDCubeRepository<Account> AccountRepository;
        private IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository;
        private IDCubeRepository<ExceptionLog> exceptionRepository;
        private IDCubeRepository<MenuItem> MenuItemRepository;
        private IDCubeRepository<MenuItemPortion> MenuItemPortionRepository;
        private IDCubeRepository<Table> TableRepository;

        public OrderAPIController()
        {
            this.OrderRepository = (IDCubeRepository<Order>)new DCubeRepository<Order>();
            this.ScreenOrderRepository = (IDCubeRepository<ScreenOrder>)new DCubeRepository<ScreenOrder>();
            this.ScreenOrderItemsRepository = (IDCubeRepository<ScreenOrderDetails>)new DCubeRepository<ScreenOrderDetails>();
            this.TicketRepository = (IDCubeRepository<Ticket>)new DCubeRepository<Ticket>();
            this.AccountTranastionRepository = (IDCubeRepository<AccountTransaction>)new DCubeRepository<AccountTransaction>();
            this.AccountTransactionValueRepository = (IDCubeRepository<AccountTransactionValue>)new DCubeRepository<AccountTransactionValue>();
            this.TransactionDocumentRepository = (IDCubeRepository<AccountTransactionDocument>)new DCubeRepository<AccountTransactionDocument>();
            this.AccountTypeRepository = (IDCubeRepository<AccountType>)new DCubeRepository<AccountType>();
            this.AccountRepository = (IDCubeRepository<Account>)new DCubeRepository<Account>();
            this.AccountTransactionTypeRepository = (IDCubeRepository<AccountTransactionType>)new DCubeRepository<AccountTransactionType>();
            this.exceptionRepository = (IDCubeRepository<ExceptionLog>)new DCubeRepository<ExceptionLog>();
            this.MenuItemRepository = (IDCubeRepository<MenuItem>)new DCubeRepository<MenuItem>();
            this.MenuItemPortionRepository = (IDCubeRepository<MenuItemPortion>)new DCubeRepository<MenuItemPortion>();
            this.TableRepository = (IDCubeRepository<Table>)new DCubeRepository<Table>();
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string TicketId)
        {
            List<ScreenOrder> screenOrderList = new List<ScreenOrder>();
            return this.ToJson((object)TicketBusiness.GetTicketOrder(int.Parse(TicketId), this.TicketRepository, this.OrderRepository, this.exceptionRepository));
        }

        [HttpGet]
        public HttpResponseMessage GetUserOrder([FromUri] string UserName) => this.ToJson((object)TicketBusiness.TotalDaySale(this.OrderRepository, this.exceptionRepository, UserName));

        [HttpGet]
        public HttpResponseMessage GetUserOrder() => this.ToJson((object)TicketBusiness.TotalDaySale(this.OrderRepository, this.exceptionRepository));

        [HttpGet]
        public HttpResponseMessage GetUserItemOrder([FromUri] string UserName) => this.ToJson((object)TicketBusiness.TotalItemSale(this.OrderRepository, this.exceptionRepository, UserName));

        [HttpGet]
        public HttpResponseMessage GetUserItemOrder() => this.ToJson((object)TicketBusiness.TotalItemSale(this.OrderRepository, this.exceptionRepository));

        [HttpPost]
        public HttpResponseMessage Post(ScreenOrderItemRequest OrderItemRequest)
        {
            ScreenOrderItemResponse orderItemResponse = new ScreenOrderItemResponse();
            orderItemResponse = TicketBusiness.SaveTicketOrder(HttpContext.Current.User.Identity.Name, this.AccountTypeRepository, this.AccountTransactionTypeRepository, this.AccountRepository, this.TicketRepository, this.OrderRepository, this.AccountTranastionRepository, this.AccountTransactionValueRepository, this.TransactionDocumentRepository, this.exceptionRepository, OrderItemRequest);
            return Request.CreateResponse(HttpStatusCode.OK, orderItemResponse);
        }
    }
}
