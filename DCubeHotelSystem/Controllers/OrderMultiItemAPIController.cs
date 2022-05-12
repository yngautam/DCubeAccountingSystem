using DCubeHotelBusinessLayer.HotelReservationBL;
using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Domain.Models.Settings;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    public class OrderMultiItemAPIController : BaseAPIController
    {
        private IDCubeRepository<Order> OrderRepository;
        private IDCubeRepository<ScreenOrder> ScreenOrderRepository;
        private IDCubeRepository<ScreenOrderDetails> ScreenOrderItemsRepository;
        private IDCubeRepository<Ticket> TicketRepository;
        private IDCubeRepository<AccountTransaction> AccountTranastionRepository;
        private IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository;
        private IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository;
        private IDCubeRepository<AccountType> AccountTypeRepository;
        private IDCubeRepository<Account> AccountRepository;
        private IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository;
        private IDCubeRepository<ExceptionLog> exceptionRepository;
        private IDCubeRepository<MenuItem> MenuItemRepository;
        private IDCubeRepository<MenuItemPortion> MenuItemPortionRepository;
        private IDCubeRepository<Table> TableRepository;
        private DCubeRepository<MenuCategoryItem> Menucatrepo;
        private DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepo;
        private DCubeRepository<MenuItem> MenuItemRepo;
        private DCubeRepository<MenuItemPortion> Menuportionrepo;
        private DCubeRepository<Printer> Printerrepo;
        private DCubeRepository<Department> Departmentrepo;

        public OrderMultiItemAPIController()
        {
            this.OrderRepository = (IDCubeRepository<Order>)new DCubeRepository<Order>();
            this.ScreenOrderRepository = (IDCubeRepository<ScreenOrder>)new DCubeRepository<ScreenOrder>();
            this.ScreenOrderItemsRepository = (IDCubeRepository<ScreenOrderDetails>)new DCubeRepository<ScreenOrderDetails>();
            this.TicketRepository = (IDCubeRepository<Ticket>)new DCubeRepository<Ticket>();
            this.AccountTranastionRepository = (IDCubeRepository<AccountTransaction>)new DCubeRepository<AccountTransaction>();
            this.AccountTransactionValueRepository = (IDCubeRepository<AccountTransactionValue>)new DCubeRepository<AccountTransactionValue>();
            this.TransactionDocumentRepository = (IDCubeRepository<AccountTransactionDocument>)new DCubeRepository<AccountTransactionDocument>();
            this.AccountTypeRepository = (IDCubeRepository<AccountType>)new DCubeRepository<AccountType>();
            this.AccountRepository = (IDCubeRepository<Account>)new DCubeRepository<Account>();
            this.AccountTransactionTypeRepository = (IDCubeRepository<AccountTransactionType>)new DCubeRepository<AccountTransactionType>();
            this.exceptionRepository = (IDCubeRepository<ExceptionLog>)new DCubeRepository<ExceptionLog>();
            this.MenuItemRepository = (IDCubeRepository<MenuItem>)new DCubeRepository<MenuItem>();
            this.MenuItemPortionRepository = (IDCubeRepository<MenuItemPortion>)new DCubeRepository<MenuItemPortion>();
            this.TableRepository = (IDCubeRepository<Table>)new DCubeRepository<Table>();
            this.Menucatrepo = new DCubeRepository<MenuCategoryItem>();
            this.MenuCategoryRepo = new DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
            this.MenuItemRepo = new DCubeRepository<MenuItem>();
            this.Menuportionrepo = new DCubeRepository<MenuItemPortion>();
            this.Printerrepo = new DCubeRepository<Printer>();
            this.Departmentrepo = new DCubeRepository<Department>();
        }

        [HttpPost]
        public HttpResponseMessage PostMultiOrder(
          ScreenMultiOrderItemRequest OrderItemRequest)
        {
            string name = HttpContext.Current.User.Identity.Name;
            ScreenMultiOrderItemResponse orderItemResponse1 = new ScreenMultiOrderItemResponse();
            ScreenMultiOrderItemResponse orderItemResponse2 = TicketBusiness.SaveTicketOrder(this.AccountTypeRepository, this.AccountTransactionTypeRepository, this.AccountRepository, this.TicketRepository, this.OrderRepository, this.AccountTranastionRepository, this.AccountTransactionValueRepository, this.TransactionDocumentRepository, this.exceptionRepository, OrderItemRequest);
            List<MenuItemWithPrice> menuCategoryItem = MenuBusinessLayer.GetMenuCategoryItem(this.MenuCategoryRepo, (IDCubeRepository<MenuItem>)this.MenuItemRepo, (IDCubeRepository<MenuItemPortion>)this.Menuportionrepo);
            List<Table> table = TableBusinessLayer.GetTable(this.TableRepository);
            List<Department> list = this.Departmentrepo.GetAllData().ToList<Department>();
            List<Printer> printerList = new List<Printer>();
            try
            {
                printerList = this.Printerrepo.GetAllData().ToList<Printer>();
            }
            catch
            {
            }
            List<ScreenOrder> listOrder = orderItemResponse2.ListOrder;
            string printerName = "RONGTA RP76II Printer";
            string str = "12345";
            string DepartmentName = "No Department";
            string empty = string.Empty;
            try
            {
                if (printerList != null)
                {
                    foreach (ScreenOrder screenOrder in listOrder)
                    {
                        string userId = screenOrder.UserId;
                        foreach (int num in screenOrder.OrderItems.Select<ScreenOrderDetails, int>((Func<ScreenOrderDetails, int>)(p => p.DepartmentId)).Distinct<int>())
                        {
                            int dept = num;
                            Department department1 = new Department();
                            Department department2 = list.Where<Department>((Func<Department, bool>)(o => o.Id.ToString() == dept.ToString())).FirstOrDefault<Department>();
                            if (department2 != null)
                                DepartmentName = department2.Name;
                            if (printerList != null)
                            {
                                Printer printer1 = new Printer();
                                Printer printer2 = printerList.Where<Printer>((Func<Printer, bool>)(o => o.DepartmentId.ToString() == dept.ToString())).FirstOrDefault<Printer>();
                                if (printer2 != null)
                                    new ReceiptPrint().Print(printer2.ShareName, str.Replace("print://", string.Empty).Replace("/", string.Empty), menuCategoryItem, table, list, printerList, listOrder, dept.ToString(), DepartmentName, userId);
                                else
                                    new ReceiptPrint().Print(printerName, str.Replace("print://", string.Empty).Replace("/", string.Empty), menuCategoryItem, table, list, printerList, listOrder, dept.ToString(), DepartmentName, userId);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return Request.CreateResponse(HttpStatusCode.OK, orderItemResponse2);
        }
    }
}