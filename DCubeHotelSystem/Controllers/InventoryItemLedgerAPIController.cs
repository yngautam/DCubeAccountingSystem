using DCubeHotelBusinessLayer.Inventory;
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
  public class InventoryItemLedgerAPIController : BaseAPIController
  {
    private IDCubeRepository<Account> AccountRepository;
    private IDCubeRepository<AccountTransaction> AccountTransactionRepository;
    private IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository;
    private IDCubeRepository<Ticket> TicketRepository;
    private IDCubeRepository<Order> orderRepository;
    private IDCubeRepository<PurchaseDetails> purchaseDetailsRepository;
    private DCubeRepository<MenuItemPortion> Menuportionrepo;
    private DCubeRepository<MenuItem> MenuItemRepo;
    private DCubeRepository<FinancialYear> FinancialYearRepo;

    public InventoryItemLedgerAPIController()
    {
      this.AccountRepository = (IDCubeRepository<Account>) new DCubeRepository<Account>();
      this.AccountTransactionRepository = (IDCubeRepository<AccountTransaction>) new DCubeRepository<AccountTransaction>();
      this.AccountTransactionValueRepository = (IDCubeRepository<AccountTransactionValue>) new DCubeRepository<AccountTransactionValue>();
      this.TicketRepository = (IDCubeRepository<Ticket>) new DCubeRepository<Ticket>();
      this.orderRepository = (IDCubeRepository<Order>) new DCubeRepository<Order>();
      this.purchaseDetailsRepository = (IDCubeRepository<PurchaseDetails>) new DCubeRepository<PurchaseDetails>();
      this.Menuportionrepo = new DCubeRepository<MenuItemPortion>();
      this.MenuItemRepo = new DCubeRepository<MenuItem>();
      this.FinancialYearRepo = new DCubeRepository<FinancialYear>();
    }

    [HttpGet]
    public HttpResponseMessage Get([FromUri] string ItemId, [FromUri] string FinancialYear)
    {
      List<ViewInventoryItemLedger> source = new List<ViewInventoryItemLedger>();
      try
      {
        source = InventoryItemLedger.ListInventoryItemLedger((IDCubeRepository<FinancialYear>) this.FinancialYearRepo, this.AccountRepository, this.AccountTransactionRepository, this.TicketRepository, this.orderRepository, this.purchaseDetailsRepository, this.Menuportionrepo, this.MenuItemRepo, ItemId, FinancialYear);
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<ViewInventoryItemLedger>());
    }
  }
}
