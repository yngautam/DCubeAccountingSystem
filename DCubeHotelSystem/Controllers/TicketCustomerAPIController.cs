using DCubeHotelBusinessLayer.DCubeHotelAccount;
using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Reservation;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class TicketCustomerAPIController : BaseAPIController
    {
        private IDCubeRepository<Ticket> TicketRepository;
        private IDCubeRepository<Account> AccountRepository;
        private IDCubeRepository<AccountTransaction> AccountTranastionRepository;
        private IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository;
        private IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository;
        private IDCubeRepository<AccountType> AccountTypeRepository;
        private IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository;

        public TicketCustomerAPIController()
        {
            this.TicketRepository = (IDCubeRepository<Ticket>)new DCubeRepository<Ticket>();
            this.AccountRepository = (IDCubeRepository<Account>)new DCubeRepository<Account>();
            this.AccountTranastionRepository = (IDCubeRepository<AccountTransaction>)new DCubeRepository<AccountTransaction>();
            this.AccountTransactionValueRepository = (IDCubeRepository<AccountTransactionValue>)new DCubeRepository<AccountTransactionValue>();
            this.TransactionDocumentRepository = (IDCubeRepository<AccountTransactionDocument>)new DCubeRepository<AccountTransactionDocument>();
            this.AccountTypeRepository = (IDCubeRepository<AccountType>)new DCubeRepository<AccountType>();
            this.AccountTransactionTypeRepository = (IDCubeRepository<AccountTransactionType>)new DCubeRepository<AccountTransactionType>();
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            List<Customer> customerList = new List<Customer>();
            return this.ToJson((object)AccountsBusiness.GetAllAccount(this.AccountRepository));
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string CustomerId)
        {
            List<Ticket> ticketList = new List<Ticket>();
            List<ScreenTicket> screenTicketList = new List<ScreenTicket>();
            return this.ToJson((object)TicketBusiness.GetUnsettleTicket(this.TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => !o.IsClosed && !o.IsLocked && o.Table_Customer_Room == int.Parse(CustomerId) && o.TicketTypeId == 1)).ToList<Ticket>(), this.TicketRepository, this.AccountTypeRepository, this.TransactionDocumentRepository, this.AccountTransactionTypeRepository, this.AccountTranastionRepository, this.AccountTransactionValueRepository));
        }
    }
}
