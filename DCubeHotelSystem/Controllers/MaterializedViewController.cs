using DCubeHotelBusinessLayer.Accounts;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.CBMSAPI;
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
  public class MaterializedViewController : BaseAPIController
  {
    private IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository;
    private IDCubeRepository<AccountTransactionDocument> AccountTransactionDocumentRepository;
    private IDCubeRepository<Ticket> TicketRepository;
    private IDCubeRepository<AccountType> AccountTypeRepository;
    private IDCubeRepository<Account> AccountRepository;
    private IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository;
    private IDCubeRepository<AccountTransaction> AccountTransactionRepository;
    private IDCubeRepository<MaterializedView> MaterializedViewRepository;

    public MaterializedViewController()
    {
      this.AccountTransactionValueRepository = (IDCubeRepository<AccountTransactionValue>) new DCubeRepository<AccountTransactionValue>();
      this.AccountTransactionDocumentRepository = (IDCubeRepository<AccountTransactionDocument>) new DCubeRepository<AccountTransactionDocument>();
      this.TicketRepository = (IDCubeRepository<Ticket>) new DCubeRepository<Ticket>();
      this.AccountTypeRepository = (IDCubeRepository<AccountType>) new DCubeRepository<AccountType>();
      this.AccountRepository = (IDCubeRepository<Account>) new DCubeRepository<Account>();
      this.AccountTransactionTypeRepository = (IDCubeRepository<AccountTransactionType>) new DCubeRepository<AccountTransactionType>();
      this.AccountTransactionRepository = (IDCubeRepository<AccountTransaction>) new DCubeRepository<AccountTransaction>();
      this.MaterializedViewRepository = (IDCubeRepository<MaterializedView>) new DCubeRepository<MaterializedView>();
    }

    public HttpResponseMessage Get([FromUri] string Month, [FromUri] string FinancialYear)
    {
      List<MaterializedView> source = new List<MaterializedView>();
      try
      {
        source = MaterializedViewBusiness.getMaterializedView(this.AccountTransactionRepository, this.AccountTypeRepository, this.AccountRepository, this.AccountTransactionTypeRepository, this.TicketRepository, this.AccountTransactionValueRepository, this.AccountTransactionDocumentRepository, Month, FinancialYear);
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<MaterializedView>());
    }
  }
}
