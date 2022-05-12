using DCubeHotelBusinessLayer.Accounts;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
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
  [UserRoleAuthorize]
  public class AccountTransactionAPIController : BaseAPIController
  {
    private IDCubeRepository<AccountTransaction> accRepository;
    private IDCubeRepository<AccountTransactionValue> accValueRepository;
    private IDCubeRepository<AccountTransactionDocument> accTransDocRepository;
    private IDCubeRepository<AccountTransactionType> accTransTypeRepository;
    private IDCubeRepository<Account> AccountRepository;
    private IDCubeRepository<AccountType> AccountTypeRepository;
    private IDCubeRepository<Ticket> TicketRepository;
    private DCubeRepository<FinancialYear> FinancialYeaRepo;

    public AccountTransactionAPIController()
    {
      this.accRepository = (IDCubeRepository<AccountTransaction>) new DCubeRepository<AccountTransaction>();
      this.accValueRepository = (IDCubeRepository<AccountTransactionValue>) new DCubeRepository<AccountTransactionValue>();
      this.accTransDocRepository = (IDCubeRepository<AccountTransactionDocument>) new DCubeRepository<AccountTransactionDocument>();
      this.accTransTypeRepository = (IDCubeRepository<AccountTransactionType>) new DCubeRepository<AccountTransactionType>();
      this.AccountRepository = (IDCubeRepository<Account>) new DCubeRepository<Account>();
      this.AccountTypeRepository = (IDCubeRepository<AccountType>) new DCubeRepository<AccountType>();
      this.TicketRepository = (IDCubeRepository<Ticket>) new DCubeRepository<Ticket>();
      this.FinancialYeaRepo = new DCubeRepository<FinancialYear>();
    }

    [HttpGet]
    public HttpResponseMessage Get(
      [FromUri] string fromDate,
      [FromUri] string toDate,
      string TransactionTypeId)
    {
      int BranchId = 0;
      List<AccountScreen> source = new List<AccountScreen>();
      try
      {
        source = AccountTransactionBusiness.GetScrenAccountTransaction(this.AccountTypeRepository, this.AccountRepository, this.accRepository, this.accValueRepository, fromDate, toDate, int.Parse(TransactionTypeId), BranchId);
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<AccountScreen>());
    }

    [HttpGet]
    public HttpResponseMessage Get(
      [FromUri] string fromDate,
      [FromUri] string toDate,
      string TransactionTypeId,
      string BranchId)
    {
      List<AccountScreen> source = new List<AccountScreen>();
      try
      {
        source = AccountTransactionBusiness.GetScrenAccountTransaction(this.AccountTypeRepository, this.AccountRepository, this.accRepository, this.accValueRepository, fromDate, toDate, int.Parse(TransactionTypeId), int.Parse(BranchId));
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<AccountScreen>());
    }

    [HttpGet]
    public HttpResponseMessage Get([FromUri] string TransactionId)
    {
      AccountScreenValue accountScreenValue = new AccountScreenValue();
      try
      {
        accountScreenValue = AccountTransactionBusiness.ScrenAccountTransaction(this.accRepository, this.accValueRepository, this.TicketRepository, (IDCubeRepository<FinancialYear>) this.FinancialYeaRepo, TransactionId);
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) accountScreenValue);
    }

        [HttpPost]
        public HttpResponseMessage Post(AccountTransaction value)
        {
            int result = 1;
            AccountTransaction objAccountTransaction = new AccountTransaction();
            result = AccountTransactionBusiness.Create(AccountRepository, accRepository, accTransTypeRepository, accValueRepository, accTransDocRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPut]
        public HttpResponseMessage Put(int id, AccountTransaction value)
        {
            int result = 1;
            AccountTransaction objaccoutTransType = new AccountTransaction();
            result = AccountTransactionBusiness.Edit(AccountRepository, accRepository, accValueRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpDelete]
        public HttpResponseMessage Delete(AccountTransaction value)
        {
            int result = 1;
            AccountTransaction objAccountTransaction = new AccountTransaction();
            result = AccountTransactionBusiness.Delete(accValueRepository, accRepository, accTransDocRepository, AccountTypeRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
