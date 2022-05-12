using DCubeHotelBusinessLayer.DCubeHotelAccount;
using DCubeHotelBusinessLayer.Purchases;
using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
  [UserRoleAuthorize]
  public class DashBoardAPIController : BaseAPIController
  {
    private IDCubeRepository<Account> accountRepository;
    private IDCubeRepository<Order> OrderRepository;
    private IDCubeRepository<PurchaseDetails> purchaseDetailsRepository;
    private DCubeRepository<FinancialYear> FinancialYeaRepo;
    private DCubeRepository<AccountTransaction> AccountTransactionRepo;

    public DashBoardAPIController()
    {
      this.accountRepository = (IDCubeRepository<Account>) new DCubeRepository<Account>();
      this.OrderRepository = (IDCubeRepository<Order>) new DCubeRepository<Order>();
      this.purchaseDetailsRepository = (IDCubeRepository<PurchaseDetails>) new DCubeRepository<PurchaseDetails>();
      this.FinancialYeaRepo = new DCubeRepository<FinancialYear>();
      this.AccountTransactionRepo = new DCubeRepository<AccountTransaction>();
    }

    [HttpGet]
    public HttpResponseMessage Get([FromUri] string FinancialYear) => this.ToJson((object) new DashBoard()
    {
      CashinHand = 0M,
      Customer = AccountsBusiness.GetAllCustomerAccount(this.accountRepository).ToList<Account>().Count,
      MonthSales = TicketSaleBillingBusiness.TotalListMonthSale(this.OrderRepository, FinancialYear),
      PurchaseYear = PurchaseBusiness.TotalPurchase(this.purchaseDetailsRepository, (IDCubeRepository<AccountTransaction>) this.AccountTransactionRepo, (IDCubeRepository<FinancialYear>) this.FinancialYeaRepo, FinancialYear),
      SaleMonth = TicketSaleBillingBusiness.TotalMonthSale(this.OrderRepository, (IDCubeRepository<FinancialYear>) this.FinancialYeaRepo, FinancialYear),
      SaleYear = TicketSaleBillingBusiness.TotalSale(this.OrderRepository, (IDCubeRepository<FinancialYear>) this.FinancialYeaRepo, FinancialYear),
      YearSales = TicketSaleBillingBusiness.TotalListYearSale(this.OrderRepository)
    });
  }
}
