using DCubeHotelBusinessLayer.Accounts;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
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
  public class AccountSaleBookController : BaseAPIController
  {
    private IDCubeRepository<Account> AccountRepository;
    private IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository;
    private IDCubeRepository<AccountTransactionDocument> AccountTransactionDocumentRepository;
    private IDCubeRepository<Ticket> TicketRepository;
    private IDCubeRepository<AccountType> AccountTypeRepository;
    private IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository;
    private IDCubeRepository<MenuItemPortion> MenuItemPortionRepository;
    private IDCubeRepository<Order> OrderRepository;

    public AccountSaleBookController()
    {
      this.AccountRepository = (IDCubeRepository<Account>) new DCubeRepository<Account>();
      this.AccountTransactionValueRepository = (IDCubeRepository<AccountTransactionValue>) new DCubeRepository<AccountTransactionValue>();
      this.AccountTransactionDocumentRepository = (IDCubeRepository<AccountTransactionDocument>) new DCubeRepository<AccountTransactionDocument>();
      this.TicketRepository = (IDCubeRepository<Ticket>) new DCubeRepository<Ticket>();
      this.AccountTypeRepository = (IDCubeRepository<AccountType>) new DCubeRepository<AccountType>();
      this.AccountTransactionTypeRepository = (IDCubeRepository<AccountTransactionType>) new DCubeRepository<AccountTransactionType>();
      this.MenuItemPortionRepository = (IDCubeRepository<MenuItemPortion>) new DCubeRepository<MenuItemPortion>();
      this.OrderRepository = (IDCubeRepository<Order>) new DCubeRepository<Order>();
    }

    [HttpGet]
    public HttpResponseMessage Get([FromUri] string Month, [FromUri] string FinancialYear)
    {
      List<SaleBook> source = new List<SaleBook>();
      try
      {
        source = SaleBookBusiness.GetSaleBook(this.AccountTypeRepository, this.AccountRepository, this.AccountTransactionTypeRepository, this.TicketRepository, this.AccountTransactionValueRepository, this.AccountTransactionDocumentRepository, Month, FinancialYear);
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<SaleBook>());
    }

    [HttpGet]
    public HttpResponseMessage Get([FromUri] string fromDate, [FromUri] string toDate, string Item)
    {
      List<SalesBillItem> source = new List<SalesBillItem>();
      try
      {
        source = SaleBookBusiness.GetSaleBookItemWise(this.TicketRepository, this.OrderRepository, this.MenuItemPortionRepository, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<SalesBillItem>());
    }

    [HttpGet]
    public HttpResponseMessage Get(
      string CustomerId,
      string FinancialYear,
      string CustomerReport,
      string report)
    {
      List<SaleBookCustomer> source = new List<SaleBookCustomer>();
      try
      {
        source = SaleBookBusiness.GetSaleBookCustomerWise(this.TicketRepository, this.OrderRepository, this.MenuItemPortionRepository, this.AccountRepository, CustomerId, FinancialYear);
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<SaleBookCustomer>());
    }

    [HttpGet]
    public HttpResponseMessage Get(
      [FromUri] string fromDate,
      [FromUri] string toDate,
      string Item,
      string date,
      string sale)
    {
      List<SaleBookDate> source = new List<SaleBookDate>();
      try
      {
        source = SaleBookBusiness.GetSaleBookDateWise(this.AccountRepository, this.TicketRepository, this.OrderRepository, this.MenuItemPortionRepository, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));
      }
      catch (Exception ex)
      {
      }
      return this.ToJson((object) source.AsEnumerable<SaleBookDate>());
    }
  }
}
