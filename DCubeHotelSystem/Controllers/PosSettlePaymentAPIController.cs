using DCubeHotelBusinessLayer.HotelPOSPaymentBusinessLayer;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class PosSettlePaymentAPIController : BaseAPIController
    {
        private IDCubeRepository<Order> OrderRepository;
        private IDCubeRepository<Ticket> TicketRepository;
        private IDCubeRepository<AccountTransaction> AccountTranastionRepository;
        private IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository;
        private IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository;
        private IDCubeRepository<AccountType> AccountTypeRepository;
        private IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository;

        public PosSettlePaymentAPIController()
        {
            this.OrderRepository = (IDCubeRepository<Order>)new DCubeRepository<Order>();
            this.TicketRepository = (IDCubeRepository<Ticket>)new DCubeRepository<Ticket>();
            this.AccountTranastionRepository = (IDCubeRepository<AccountTransaction>)new DCubeRepository<AccountTransaction>();
            this.AccountTransactionValueRepository = (IDCubeRepository<AccountTransactionValue>)new DCubeRepository<AccountTransactionValue>();
            this.TransactionDocumentRepository = (IDCubeRepository<AccountTransactionDocument>)new DCubeRepository<AccountTransactionDocument>();
            this.AccountTypeRepository = (IDCubeRepository<AccountType>)new DCubeRepository<AccountType>();
            this.AccountTransactionTypeRepository = (IDCubeRepository<AccountTransactionType>)new DCubeRepository<AccountTransactionType>();
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] PaymentSettle possettle)
        {
            ScreenTicket screenTicket = new ScreenTicket();

            if (possettle.PosSettle != null)
            {
                screenTicket = POSPaymentBusinessLayer.POSTDiscount(this.TicketRepository, this.AccountTranastionRepository, this.AccountTransactionValueRepository, this.TransactionDocumentRepository, this.AccountTypeRepository, this.AccountTransactionTypeRepository, DateTime.Now, possettle);

            }
            else
            {
                screenTicket = POSPaymentBusinessLayer.POSTPayment(this.TicketRepository, this.AccountTranastionRepository, this.AccountTransactionValueRepository, this.TransactionDocumentRepository, this.AccountTypeRepository, this.AccountTransactionTypeRepository, DateTime.Now, possettle);
            }

            return Request.CreateResponse(HttpStatusCode.OK, screenTicket);
        }
    }
}