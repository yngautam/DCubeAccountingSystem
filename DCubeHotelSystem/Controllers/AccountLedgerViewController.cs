using DCubeHotelBusinessLayer.Accounts;
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
  public class AccountLedgerViewController : BaseAPIController
  {
    private IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository;
    private IDCubeRepository<AccountTransaction> AccountTransactionRepository;
    private IDCubeRepository<Account> AccountRepository;
    private IDCubeRepository<AccountTransactionDocument> AccountTransactionDocumentRepository;
    private IDCubeRepository<Ticket> TicketRepository;
    private IDCubeRepository<PurchaseDetails> purchaseDetailsRepository;
    private IDCubeRepository<Order> orderRepository;
    private DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepo;
    private DCubeRepository<MenuItem> MenuItemRepo;
    private DCubeRepository<MenuItemPortion> Menuportionrepo;
    private DCubeRepository<FinancialYear> FinancialYearrepo;
    private DCubeRepository<MenuItemPortionPriceRange> MenuportionPriceRangerepo;

    public AccountLedgerViewController()
    {
      this.AccountTransactionValueRepository = (IDCubeRepository<AccountTransactionValue>) new DCubeRepository<AccountTransactionValue>();
      this.AccountTransactionRepository = (IDCubeRepository<AccountTransaction>) new DCubeRepository<AccountTransaction>();
      this.AccountRepository = (IDCubeRepository<Account>) new DCubeRepository<Account>();
      this.AccountTransactionDocumentRepository = (IDCubeRepository<AccountTransactionDocument>) new DCubeRepository<AccountTransactionDocument>();
      this.TicketRepository = (IDCubeRepository<Ticket>) new DCubeRepository<Ticket>();
      this.purchaseDetailsRepository = (IDCubeRepository<PurchaseDetails>) new DCubeRepository<PurchaseDetails>();
      this.orderRepository = (IDCubeRepository<Order>) new DCubeRepository<Order>();
      this.MenuCategoryRepo = new DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
      this.MenuItemRepo = new DCubeRepository<MenuItem>();
      this.Menuportionrepo = new DCubeRepository<MenuItemPortion>();
      this.FinancialYearrepo = new DCubeRepository<FinancialYear>();
      this.MenuportionPriceRangerepo = new DCubeRepository<MenuItemPortionPriceRange>();
    }

    [HttpGet]
    public HttpResponseMessage GetLedgerPreviousBalance(
      [FromUri] string LedgerId,
      [FromUri] string TransactionId)
    {
      Decimal num = 0M;
      try
      {
        num = AccountLedgerViewBusiness.GetLedgerPreviousBalance(this.AccountRepository, this.AccountTransactionValueRepository, int.Parse(LedgerId), int.Parse(TransactionId));
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) num);
    }

    [HttpGet]
    public HttpResponseMessage Get(
      [FromUri] string LedgerId,
      [FromUri] string fromDate,
      [FromUri] string toDate)
    {
      int BranchId = 0;
      List<LedgerView> source = new List<LedgerView>();
      try
      {
        source = AccountLedgerViewBusiness.GetLedgerView(this.AccountRepository, this.AccountTransactionDocumentRepository, this.AccountTransactionValueRepository, this.AccountTransactionRepository, this.TicketRepository, this.purchaseDetailsRepository, this.orderRepository, this.MenuCategoryRepo, this.MenuItemRepo, this.Menuportionrepo, this.MenuportionPriceRangerepo, this.FinancialYearrepo, int.Parse(LedgerId), BranchId, fromDate, toDate);
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<LedgerView>());
    }

    [HttpGet]
    public HttpResponseMessage Get(
      [FromUri] string BranchId,
      [FromUri] string LedgerId,
      [FromUri] string fromDate,
      [FromUri] string toDate)
    {
      List<LedgerView> source = new List<LedgerView>();
      try
      {
        source = AccountLedgerViewBusiness.GetLedgerView(this.AccountRepository, this.AccountTransactionDocumentRepository, this.AccountTransactionValueRepository, this.AccountTransactionRepository, this.TicketRepository, this.purchaseDetailsRepository, this.orderRepository, this.MenuCategoryRepo, this.MenuItemRepo, this.Menuportionrepo, this.MenuportionPriceRangerepo, this.FinancialYearrepo, int.Parse(LedgerId), int.Parse(BranchId), fromDate, toDate);
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<LedgerView>());
    }

    [HttpGet]
    public HttpResponseMessage GetLedgerStatement()
    {
      List<LedgerView> source = new List<LedgerView>();
      try
      {
        source = AccountLedgerViewBusiness.GetLedgerStatement(this.AccountRepository, this.AccountTransactionValueRepository);
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<LedgerView>());
    }

    [HttpGet]
    public HttpResponseMessage GetDayBook([FromUri] string FinancialYear)
    {
      List<LedgerView> source = new List<LedgerView>();
      try
      {
        source = AccountLedgerViewBusiness.GetDayBook(this.AccountRepository, this.AccountTransactionValueRepository, this.AccountTransactionRepository, FinancialYear);
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<LedgerView>());
    }
  }
}
