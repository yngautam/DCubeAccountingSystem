using DCubeHotelBusinessLayer.Inventory;
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
  public class StockInHandController : BaseAPIController
  {
    private IDCubeRepository<InventoryReceipt> inventReceiptRepo;
    private IDCubeRepository<MenuItem> MenuItemRepo;
    private IDCubeRepository<MenuItemPortion> MenuItemPortionRepo;
    private IDCubeRepository<PeriodicConsumption> PeriodicConsumptionRepository;
    private IDCubeRepository<PeriodicConsumptionItem> PeriodicConsumptionItemRepository;
    private IDCubeRepository<InventoryItem> inventItemRepo;
    private IDCubeRepository<ViewInventoryItem> ViewInventoryItemRepo;
    private IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepo;
    private IDCubeRepository<PurchaseDetails> PurchaseDetailsRepository;
    private IDCubeRepository<Order> OrderRepository;

    public StockInHandController()
    {
      this.inventReceiptRepo = (IDCubeRepository<InventoryReceipt>) new DCubeRepository<InventoryReceipt>();
      this.MenuItemRepo = (IDCubeRepository<MenuItem>) new DCubeRepository<MenuItem>();
      this.MenuItemPortionRepo = (IDCubeRepository<MenuItemPortion>) new DCubeRepository<MenuItemPortion>();
      this.PeriodicConsumptionRepository = (IDCubeRepository<PeriodicConsumption>) new DCubeRepository<PeriodicConsumption>();
      this.PeriodicConsumptionItemRepository = (IDCubeRepository<PeriodicConsumptionItem>) new DCubeRepository<PeriodicConsumptionItem>();
      this.inventItemRepo = (IDCubeRepository<InventoryItem>) new DCubeRepository<InventoryItem>();
      this.ViewInventoryItemRepo = (IDCubeRepository<ViewInventoryItem>) new DCubeRepository<ViewInventoryItem>();
      this.MenuCategoryRepo = (IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory>) new DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
      this.PurchaseDetailsRepository = (IDCubeRepository<PurchaseDetails>) new DCubeRepository<PurchaseDetails>();
      this.OrderRepository = (IDCubeRepository<Order>) new DCubeRepository<Order>();
    }

    [HttpGet]
    public HttpResponseMessage Get() => this.ToJson((object) this.inventItemRepo.GetAllData());

    [HttpGet]
    public HttpResponseMessage Get([FromUri] string FinancialYear)
    {
      List<ViewInventoryItem> source = new List<ViewInventoryItem>();
      try
      {
        source = StockInHandBusiness.GetStockQuantity(this.MenuItemRepo, this.PeriodicConsumptionItemRepository, this.PurchaseDetailsRepository, this.OrderRepository, this.MenuItemPortionRepo, this.MenuCategoryRepo, FinancialYear);
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<ViewInventoryItem>());
    }
  }
}
