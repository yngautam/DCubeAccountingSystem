using DCubeHotelBusinessLayer.Accounts;
using DCubeHotelBusinessLayer.ExtraModel;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelDomain.Models.Menus;
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
  public class AccountProfitLossController : BaseAPIController
  {
    private IDCubeRepository<AccountTransaction> AccountTransactionRepository;
    private IDCubeRepository<AccountTransactionValue> accValueRepository;
    private IDCubeRepository<Account> AccountRepository;
    private IDCubeRepository<AccountType> AccountTypeRepository;
    private DCubeRepository<FinancialYear> FinancialYearrepo;
    private DCubeRepository<MenuItemPortion> MenuItemPortionRepo;
    private DCubeRepository<PurchaseDetails> PurchaseDetailsRepo;
    private DCubeRepository<Ticket> TicketRepo;
    private DCubeRepository<Order> OrderRepo;

    public AccountProfitLossController()
    {
      this.AccountTransactionRepository = (IDCubeRepository<AccountTransaction>) new DCubeRepository<AccountTransaction>();
      this.accValueRepository = (IDCubeRepository<AccountTransactionValue>) new DCubeRepository<AccountTransactionValue>();
      this.AccountRepository = (IDCubeRepository<Account>) new DCubeRepository<Account>();
      this.AccountTypeRepository = (IDCubeRepository<AccountType>) new DCubeRepository<AccountType>();
      this.FinancialYearrepo = new DCubeRepository<FinancialYear>();
      this.MenuItemPortionRepo = new DCubeRepository<MenuItemPortion>();
      this.PurchaseDetailsRepo = new DCubeRepository<PurchaseDetails>();
      this.TicketRepo = new DCubeRepository<Ticket>();
      this.OrderRepo = new DCubeRepository<Order>();
    }

    [HttpGet]
    public HttpResponseMessage Get([FromUri] string FinancialYear)
    {
      List<ProfitAndLoss> source = new List<ProfitAndLoss>();
      try
      {
        source = TrialBalanceBusiness.GetProfitLoss(this.AccountTypeRepository, this.AccountRepository, this.accValueRepository, this.FinancialYearrepo, FinancialYear);
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<ProfitAndLoss>());
    }

    [HttpGet]
    public HttpResponseMessage Get(
      [FromUri] string fromDate,
      [FromUri] string toDate,
      [FromUri] string BranchId)
    {
      List<ProfitAndLoss> source = new List<ProfitAndLoss>();
      try
      {
        source = TrialBalanceBusiness.GetProfitLoss(this.AccountTypeRepository, this.AccountRepository, this.AccountTransactionRepository, this.accValueRepository, (IDCubeRepository<MenuItemPortion>) this.MenuItemPortionRepo, (IDCubeRepository<PurchaseDetails>) this.PurchaseDetailsRepo, (IDCubeRepository<Ticket>) this.TicketRepo, (IDCubeRepository<Order>) this.OrderRepo, fromDate, toDate, int.Parse(BranchId));
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<ProfitAndLoss>());
    }

    [HttpGet]
    public HttpResponseMessage Get([FromUri] string fromDate, [FromUri] string toDate)
    {
      List<DayWiseSales> source = new List<DayWiseSales>();
      try
      {
        source = TrialBalanceBusiness.TotalDayWiseSale((IDCubeRepository<Order>) this.OrderRepo, fromDate, toDate);
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<DayWiseSales>());
    }
  }
}
