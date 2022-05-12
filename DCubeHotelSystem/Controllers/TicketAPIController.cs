using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models.Accounts;
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
    public class TicketAPIController : BaseAPIController
    {
        private IDCubeRepository<Ticket> TicketRepository;
        private IDCubeRepository<AccountTransaction> AccountTranastionRepository;
        private IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository;
        private IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository;
        private IDCubeRepository<AccountType> AccountTypeRepository;
        private IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository;

        public TicketAPIController()
        {
            this.TicketRepository = (IDCubeRepository<Ticket>)new DCubeRepository<Ticket>();
            this.AccountTranastionRepository = (IDCubeRepository<AccountTransaction>)new DCubeRepository<AccountTransaction>();
            this.AccountTransactionValueRepository = (IDCubeRepository<AccountTransactionValue>)new DCubeRepository<AccountTransactionValue>();
            this.TransactionDocumentRepository = (IDCubeRepository<AccountTransactionDocument>)new DCubeRepository<AccountTransactionDocument>();
            this.AccountTypeRepository = (IDCubeRepository<AccountType>)new DCubeRepository<AccountType>();
            this.AccountTransactionTypeRepository = (IDCubeRepository<AccountTransactionType>)new DCubeRepository<AccountTransactionType>();
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string TableId)
        {
            List<Ticket> ticketList = new List<Ticket>();
            List<ScreenTicket> screenTicketList = new List<ScreenTicket>();
            return this.ToJson((object)TicketBusiness.GetUnsettleTicket(this.TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => !o.IsClosed && !o.IsLocked && o.Table_Customer_Room == int.Parse(TableId) && !o.IS_Bill_Printed)).ToList<Ticket>(), this.TicketRepository, this.AccountTypeRepository, this.TransactionDocumentRepository, this.AccountTransactionTypeRepository, this.AccountTranastionRepository, this.AccountTransactionValueRepository));
        }

        [HttpGet]
        public HttpResponseMessage GetAllTicket()
        {
            List<Ticket> ticketList = new List<Ticket>();
            List<ScreenTicket> screenTicketList = new List<ScreenTicket>();
            return this.ToJson((object)TicketBusiness.GetUnsettleTicket(this.TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => !o.IsClosed && !o.IsLocked && !o.IS_Bill_Printed)).ToList<Ticket>(), this.TicketRepository, this.AccountTypeRepository, this.TransactionDocumentRepository, this.AccountTransactionTypeRepository, this.AccountTranastionRepository, this.AccountTransactionValueRepository));
        }
    }
}