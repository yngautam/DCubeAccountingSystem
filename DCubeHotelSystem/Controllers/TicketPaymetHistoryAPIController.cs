using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class TicketPaymetHistoryAPIController : BaseAPIController
    {
        private IDCubeRepository<Ticket> TicketRepository;
        private IDCubeRepository<AccountTransaction> AccountTranastionRepository;
        private IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository;
        private IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository;
        private IDCubeRepository<AccountType> AccountTypeRepository;
        private IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository;

        public TicketPaymetHistoryAPIController()
        {
            this.TicketRepository = (IDCubeRepository<Ticket>)new DCubeRepository<Ticket>();
            this.AccountTranastionRepository = (IDCubeRepository<AccountTransaction>)new DCubeRepository<AccountTransaction>();
            this.AccountTransactionValueRepository = (IDCubeRepository<AccountTransactionValue>)new DCubeRepository<AccountTransactionValue>();
            this.TransactionDocumentRepository = (IDCubeRepository<AccountTransactionDocument>)new DCubeRepository<AccountTransactionDocument>();
            this.AccountTypeRepository = (IDCubeRepository<AccountType>)new DCubeRepository<AccountType>();
            this.AccountTransactionTypeRepository = (IDCubeRepository<AccountTransactionType>)new DCubeRepository<AccountTransactionType>();
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string TicketId)
        {
            ScreenTicket screenTicket = new ScreenTicket();
            return this.ToJson((object)TicketBusiness.GetScreenTicket(this.TicketRepository, this.AccountTypeRepository, this.TransactionDocumentRepository, this.AccountTransactionTypeRepository, this.AccountTranastionRepository, this.AccountTransactionValueRepository, TicketId));
        }
    }
}