using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class OrderVoidItemController : BaseAPIController
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

        public OrderVoidItemController()
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

        [HttpPost]
        public HttpResponseMessage Post(ScreenOrderItemRequest OrderItemRequest)
        {
            string updateType = "Void";
            ScreenOrderItemResponse orderItemResponse = new ScreenOrderItemResponse();
            orderItemResponse = TicketBusiness.OrderCancel(OrderItemRequest, this.TicketRepository, this.OrderRepository, this.AccountTypeRepository, this.TransactionDocumentRepository, this.AccountTranastionRepository, this.AccountTransactionValueRepository, updateType);
            return Request.CreateResponse(HttpStatusCode.OK, orderItemResponse);
        }
    }
}
