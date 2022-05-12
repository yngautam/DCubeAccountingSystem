using DCubeHotelBusinessLayer.ExtraModel;
using DCubeHotelBusinessLayer.HotelReservationBL;
using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using NepaliDate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.Sales
{
    public class SaleBillingBusiness
    {
        public static List<ItemSale> GetScreenSaleBillItem(
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransaction> AccountTransactionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> orderRepository,
          IDCubeRepository<AccountTransactionDocument> AccountTransactionDocumentRepository,
          DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepo,
          DCubeRepository<MenuItem> MenuItemRepo,
          DCubeRepository<MenuItemPortion> Menuportionrepo,
          DCubeRepository<MenuItemPortionPriceRange> MenuportionPriceRangeRepo,
          string FromDate,
          string ToDate,
          int TransactionTypeId,
          int BranchId,
          int CustomerId)
        {
            DateTime dFromdate = NepalitoEnglishDate.EnglishDate(FromDate);
            DateTime dTodate = NepalitoEnglishDate.EnglishDate(ToDate);
            string accounttypename = string.Empty;
            if (TransactionTypeId == 3)
                accounttypename = "Sales";
            if (TransactionTypeId == 10)
                accounttypename = "Credit Note";
            AccountRepository.GetAllData().ToList<Account>();
            List<ItemSale> screenSaleBillItem = new List<ItemSale>();
            List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
            List<AccountTransaction> list1 = AccountTransactionRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Name.Contains(accounttypename) && o.SourceAccountTypeId == CustomerId)).ToList<AccountTransaction>();
            if (BranchId != 0)
                list1 = list1.Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.BranchId == BranchId)).ToList<AccountTransaction>();
            List<Ticket> ticketList = new List<Ticket>();
            List<Ticket> list2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Date.Date >= dFromdate.Date && o.Date.Date <= dTodate.Date && o.Name.Contains(accounttypename) && o.Table_Customer_Room == CustomerId)).ToList<Ticket>();
            List<Order> orderList = new List<Order>();
            List<Order> list3 = orderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o => o.CreatedDateTime.Date >= dFromdate.Date && o.CreatedDateTime.Date <= dTodate.Date)).ToList<Order>();
            foreach (var data in list2.Join((IEnumerable<AccountTransaction>)list1, (Func<Ticket, int>)(t => t.Table_Customer_Room), (Func<AccountTransaction, int>)(a => a.SourceAccountTypeId), (t, a) => new
            {
                Id = t.Id,
                SourceAccountTypeId = a.SourceAccountTypeId
            }).Distinct().Join((IEnumerable<Order>)list3, b => b.Id, (Func<Order, int>)(o => o.TicketId), (b, o) => new
            {
                DepartmentId = o.DepartmentId,
                MenuItemId = o.MenuItemId,
                Quantity = o.Quantity,
                UnitType = o.UnitType,
                Price = o.Price,
                MRPPrice = o.MRPPrice,
                Discount = o.Discount,
                TaxRate = o.TaxRate,
                ExciseDuty = o.ExciseDuty,
                CompanyCode = o.CompanyCode,
                BranchId = o.BranchId,
                WarehouseId = o.WarehouseId,
                SourceAccountTypeId = b.SourceAccountTypeId
            }).GroupBy(l => new
            {
                MenuItemId = l.MenuItemId,
                SourceAccountTypeId = l.SourceAccountTypeId
            }).Select(cl => new
            {
                DepartmentId = cl.First().DepartmentId,
                ItemId = cl.First().MenuItemId,
                UnitType = cl.First().UnitType,
                Price = cl.First().Price,
                CompanyCode = cl.First().CompanyCode,
                BranchId = cl.First().BranchId,
                WarehouseId = cl.First().WarehouseId,
                CustomerId = cl.First().SourceAccountTypeId,
                Qty = cl.Sum(c => c.Quantity)
            }).ToList())
                screenSaleBillItem.Add(new ItemSale()
                {
                    BranchId = data.BranchId,
                    CompanyCode = data.CompanyCode,
                    CustomerId = data.CustomerId,
                    DepartmentId = data.DepartmentId,
                    ItemId = data.ItemId,
                    Price = data.Price,
                    Qty = data.Qty,
                    UnitType = data.UnitType,
                    WarehouseId = data.WarehouseId
                });
            return screenSaleBillItem;
        }

        public static List<ItemSale> GetScreenSaleBillItem(
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransaction> AccountTransactionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> orderRepository,
          IDCubeRepository<AccountTransactionDocument> AccountTransactionDocumentRepository,
          DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepo,
          DCubeRepository<MenuItem> MenuItemRepo,
          DCubeRepository<MenuItemPortion> Menuportionrepo,
          DCubeRepository<MenuItemPortionPriceRange> MenuportionPriceRangeRepo,
          string FromDate,
          string ToDate,
          int TransactionTypeId,
          int BranchId)
        {
            DateTime dFromdate = NepalitoEnglishDate.EnglishDate(FromDate);
            DateTime dTodate = NepalitoEnglishDate.EnglishDate(ToDate);
            string accounttypename = string.Empty;
            if (TransactionTypeId == 3)
                accounttypename = "Sales";
            if (TransactionTypeId == 10)
                accounttypename = "Credit Note";
            AccountRepository.GetAllData().ToList<Account>();
            List<ItemSale> screenSaleBillItem = new List<ItemSale>();
            List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
            List<AccountTransaction> list1 = AccountTransactionRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Name.Contains(accounttypename))).ToList<AccountTransaction>();
            if (BranchId != 0)
                list1 = list1.Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.BranchId == BranchId)).ToList<AccountTransaction>();
            List<Ticket> ticketList = new List<Ticket>();
            List<Ticket> list2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Date.Date >= dFromdate.Date && o.Date.Date <= dTodate.Date && o.Name.Contains(accounttypename))).ToList<Ticket>();
            List<Order> orderList = new List<Order>();
            List<Order> list3 = orderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o => o.CreatedDateTime.Date >= dFromdate.Date && o.CreatedDateTime.Date <= dTodate.Date)).ToList<Order>();
            foreach (var data in list2.Join((IEnumerable<AccountTransaction>)list1, (Func<Ticket, int>)(t => t.Table_Customer_Room), (Func<AccountTransaction, int>)(a => a.SourceAccountTypeId), (t, a) => new
            {
                Id = t.Id,
                SourceAccountTypeId = a.SourceAccountTypeId
            }).Distinct().Join((IEnumerable<Order>)list3, b => b.Id, (Func<Order, int>)(o => o.TicketId), (b, o) => new
            {
                DepartmentId = o.DepartmentId,
                MenuItemId = o.MenuItemId,
                Quantity = o.Quantity,
                UnitType = o.UnitType,
                Price = o.Price,
                MRPPrice = o.MRPPrice,
                Discount = o.Discount,
                TaxRate = o.TaxRate,
                ExciseDuty = o.ExciseDuty,
                CompanyCode = o.CompanyCode,
                BranchId = o.BranchId,
                WarehouseId = o.WarehouseId,
                SourceAccountTypeId = b.SourceAccountTypeId
            }).GroupBy(l => new
            {
                MenuItemId = l.MenuItemId,
                SourceAccountTypeId = l.SourceAccountTypeId
            }).Select(cl => new
            {
                DepartmentId = cl.First().DepartmentId,
                ItemId = cl.First().MenuItemId,
                UnitType = cl.First().UnitType,
                Price = cl.First().Price,
                CompanyCode = cl.First().CompanyCode,
                BranchId = cl.First().BranchId,
                WarehouseId = cl.First().WarehouseId,
                CustomerId = cl.First().SourceAccountTypeId,
                Qty = cl.Sum(c => c.Quantity)
            }).ToList())
                screenSaleBillItem.Add(new ItemSale()
                {
                    BranchId = data.BranchId,
                    CompanyCode = data.CompanyCode,
                    CustomerId = data.CustomerId,
                    DepartmentId = data.DepartmentId,
                    ItemId = data.ItemId,
                    Price = data.Price,
                    Qty = data.Qty,
                    UnitType = data.UnitType,
                    WarehouseId = data.WarehouseId
                });
            return screenSaleBillItem;
        }

        public static List<AccountTransaction> GetScreenSaleBilling(
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransaction> AccountTransactionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> orderRepository,
          IDCubeRepository<AccountTransactionDocument> AccountTransactionDocumentRepository,
          DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepo,
          DCubeRepository<MenuItem> MenuItemRepo,
          DCubeRepository<MenuItemPortion> Menuportionrepo,
          DCubeRepository<MenuItemPortionPriceRange> MenuportionPriceRangeRepo,
          string FromDate,
          string ToDate,
          int TransactionTypeId,
          int BranchId)
        {
            DateTime dFromdate = NepalitoEnglishDate.EnglishDate(FromDate);
            DateTime dTodate = NepalitoEnglishDate.EnglishDate(ToDate);
            string accounttypename = string.Empty;
            if (TransactionTypeId == 3)
                accounttypename = "Sales";
            if (TransactionTypeId == 10)
                accounttypename = "Credit Note";
            List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
            List<AccountTransactionValue> transactionValueList1 = new List<AccountTransactionValue>();
            List<AccountTransactionValue> list2 = AccountTransactionValueRepository.GetAllData().ToList<AccountTransactionValue>();
            if (BranchId != 0)
                list2 = list2.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.BranchId == BranchId)).ToList<AccountTransactionValue>();
            List<AccountTransaction> screenSaleBilling = new List<AccountTransaction>();
            List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
            List<AccountTransaction> list3 = AccountTransactionRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Name.Contains(accounttypename))).ToList<AccountTransaction>();
            if (BranchId != 0)
                list3 = list3.Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.BranchId == BranchId)).ToList<AccountTransaction>();
            List<AccountTransactionDocument> transactionDocumentList = new List<AccountTransactionDocument>();
            transactionDocumentList = AccountTransactionDocumentRepository.GetAllData().Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Name.Contains(accounttypename))).ToList<AccountTransactionDocument>();
            List<AccountTransactionType> accountTransactionTypeList = new List<AccountTransactionType>();
            accountTransactionTypeList = AccountTransactionTypeRepository.GetAllData().ToList<AccountTransactionType>();
            List<Ticket> ticketList = new List<Ticket>();
            List<Ticket> list4 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Date.Date >= dFromdate.Date && o.Date.Date <= dTodate.Date && o.Name.Contains(accounttypename))).ToList<Ticket>();
            if (BranchId != 0)
                list4 = list4.Where<Ticket>((Func<Ticket, bool>)(o => o.BranchCode == BranchId)).ToList<Ticket>();
            List<AccountTransactionValue> transactionValueList2 = new List<AccountTransactionValue>();
            List<Order> orderList1 = new List<Order>();
            List<Order> list5 = orderRepository.GetAllData().ToList<Order>();
            foreach (Ticket ticket in list4.Where<Ticket>((Func<Ticket, bool>)(o => o.AccountTransactionDocumentId != 0)).ToList<Ticket>())
            {
                Ticket objTicket = ticket;
                try
                {
                    Decimal num1 = 0M;
                    Decimal num2 = 0M;
                    string str = accounttypename + " [#" + objTicket.TicketNumber + "]";
                    List<AccountTransactionValue> list6 = list2.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionDocumentId == objTicket.AccountTransactionDocumentId)).ToList<AccountTransactionValue>();
                    List<AccountTransactionValue> transactionValueList3 = new List<AccountTransactionValue>();
                    AccountTransaction accountTransaction1 = new AccountTransaction();
                    str.Remove(str.IndexOf("["));
                    AccountTransaction accountTransaction2 = list3.Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.AccountTransactionDocumentId == objTicket.AccountTransactionDocumentId)).FirstOrDefault<AccountTransaction>();
                    if (accountTransaction2 == null)
                    {
                        accounttypename = "Sale Transaction";
                        string accounttypePOS = accounttypename + " [#" + objTicket.TicketNumber + "]";
                        accountTransaction2 = list3.Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Name.Contains(accounttypePOS))).FirstOrDefault<AccountTransaction>();
                    }
                    AccountTransactionValue transactionValue1 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue2 = list6.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == 3)).FirstOrDefault<AccountTransactionValue>();
                    AccountTransactionValue transactionValue3 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue4 = list6.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == 1)).FirstOrDefault<AccountTransactionValue>();
                    Account account1 = new Account();
                    Account account2 = list1.Find((Predicate<Account>)(o => o.Name == "Taxable Sales"));
                    int iTaxableAccountId = 0;
                    if (account2 != null)
                        iTaxableAccountId = account2.Id;
                    AccountTransactionValue transactionValue5 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue6 = list6.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == iTaxableAccountId)).FirstOrDefault<AccountTransactionValue>();
                    Account account3 = new Account();
                    Account account4 = list1.Find((Predicate<Account>)(o => o.Name == "Non Taxable Sales"));
                    int inonTaxableAccountId = 0;
                    if (account4 != null)
                        inonTaxableAccountId = account4.Id;
                    AccountTransactionValue transactionValue7 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue8 = list6.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == inonTaxableAccountId)).FirstOrDefault<AccountTransactionValue>();
                    AccountTransactionValue transactionValue9 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue10 = list6.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == 8)).FirstOrDefault<AccountTransactionValue>();
                    Account account5 = new Account();
                    Account account6 = list1.Find((Predicate<Account>)(o => o.Name == "Excise Duty"));
                    int iExciseDutyAccountId = 0;
                    if (account6 != null)
                        iExciseDutyAccountId = account6.Id;
                    AccountTransactionValue transactionValue11 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue12 = list6.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == iExciseDutyAccountId)).FirstOrDefault<AccountTransactionValue>();
                    AccountTransaction accountTransaction3 = new AccountTransaction();
                    accountTransaction3.Id = accountTransaction2.Id;
                    accountTransaction3.Date = objTicket.Date.ToString();
                    accountTransaction3.Name = accountTransaction2.Name;
                    accountTransaction3.ref_invoice_number = objTicket.TicketNumber;
                    accountTransaction3.VehicleNo = accountTransaction2.VehicleNo;
                    accountTransaction3.VehicleHeight = accountTransaction2.VehicleHeight;
                    accountTransaction3.VehicleLength = accountTransaction2.VehicleLength;
                    accountTransaction3.VehicleWidth = accountTransaction2.VehicleWidth;
                    accountTransaction3.SourceAccountTypeId = accountTransaction2.SourceAccountTypeId;
                    accountTransaction3.CompanyCode = accountTransaction2.BranchId;
                    accountTransaction3.BranchId = accountTransaction2.BranchId;
                    accountTransaction3.DepartmentId = accountTransaction2.DepartmentId;
                    accountTransaction3.WareHouseId = accountTransaction2.WareHouseId;
                    if (transactionValue2 != null)
                    {
                        accountTransaction3.Discount = transactionValue2.Debit;
                        transactionValueList3.Add(transactionValue2);
                    }
                    else
                        accountTransaction3.Discount = 0M;
                    if (transactionValue6 != null)
                    {
                        num1 += transactionValue6.Credit;
                        transactionValueList3.Add(transactionValue6);
                    }
                    if (transactionValue8 != null)
                    {
                        num1 += transactionValue8.Credit;
                        transactionValueList3.Add(transactionValue8);
                    }
                    else
                        accountTransaction3.NetAmount = 0M;
                    if (transactionValue4 != null)
                        num1 += transactionValue4.Credit;
                    accountTransaction3.NetAmount = num1;
                    if (transactionValue10 != null)
                    {
                        accountTransaction3.VATAmount = transactionValue10.Credit;
                        num2 = transactionValue10.Credit;
                        transactionValueList3.Add(transactionValue10);
                    }
                    else
                        accountTransaction3.VATAmount = 0M;
                    accountTransaction3.ExciseDuty = transactionValue12 == null ? 0M : transactionValue12.Credit;
                    accountTransaction3.AccountTransactionValues = transactionValueList3;
                    List<Order> orderList2 = new List<Order>();
                    List<Order> list7 = list5.Where<Order>((Func<Order, bool>)(o => o.TicketId == objTicket.Id)).ToList<Order>();
                    List<MenuItemWithPrice> menuItemWithPriceList = new List<MenuItemWithPrice>();
                    List<MenuItemWithPrice> menuCategoryItem = MenuBusinessLayer.GetMenuCategoryItem(MenuCategoryRepo, (IDCubeRepository<MenuItem>)MenuItemRepo, (IDCubeRepository<MenuItemPortion>)Menuportionrepo, (IDCubeRepository<MenuItemPortionPriceRange>)MenuportionPriceRangeRepo);
                    List<MenuItem> menuItemList = new List<MenuItem>();
                    List<MenuItem> list8 = MenuItemRepo.GetAllData().ToList<MenuItem>();
                    List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
                    List<MenuItemPortion> list9 = Menuportionrepo.GetAllData().ToList<MenuItemPortion>();
                    List<ScreenOrderDetails> screenOrderDetailsList = new List<ScreenOrderDetails>();
                    foreach (Order order in list7)
                    {
                        Order objOrder = order;
                        try
                        {
                            MenuItemWithPrice menuItemWithPrice = menuCategoryItem.Where<MenuItemWithPrice>((Func<MenuItemWithPrice, bool>)(o => o.ItemId == objOrder.MenuItemId)).FirstOrDefault<MenuItemWithPrice>();
                            MenuItemPortion objMenuItemPortion = new MenuItemPortion();
                            MenuItem menuItem = new MenuItem();
                            if (menuItemWithPrice != null)
                            {
                                objMenuItemPortion = list9.Find((Predicate<MenuItemPortion>)(o => o.Id == objOrder.MenuItemId));
                                if (objMenuItemPortion != null)
                                    menuItem = list8.Find((Predicate<MenuItem>)(o => o.Id == objMenuItemPortion.MenuItemPortionId));
                            }
                            ScreenOrderDetails screenOrderDetails = new ScreenOrderDetails()
                            {
                                DepartmentId = menuItemWithPrice.DepartmentId,
                                ItemId = menuItemWithPrice.ItemId,
                                ItemName = menuItemWithPrice.Name,
                                Qty = objOrder.Quantity,
                                UnitType = menuItem.UnitType,
                                UnitPrice = objOrder.Price,
                                MRPPrice = objOrder.MRPPrice,
                                Discount = objOrder.Discount,
                                TaxRate = objOrder.TaxRate,
                                ExciseDuty = objOrder.ExciseDuty,
                                TotalAmount = objOrder.Quantity * objOrder.Price,
                                CompanyCode = objOrder.CompanyCode,
                                BranchId = objOrder.BranchId
                            };
                            screenOrderDetails.DepartmentId = objOrder.DepartmentId;
                            screenOrderDetails.WarehouseId = objOrder.WarehouseId;
                            screenOrderDetails.OrderDescription = objOrder.OrderDescription;
                            screenOrderDetailsList.Add(screenOrderDetails);
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                        }
                    }
                    accountTransaction3.SalesOrderDetails = screenOrderDetailsList;
                    screenSaleBilling.Add(accountTransaction3);
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                }
            }
            return screenSaleBilling;
        }

        public static Decimal GetSaleBillingRate(
          IDCubeRepository<MenuItemPortion> MenuItemPortionRepository,
          int id)
        {
            return MenuItemPortionRepository.GetAllData().Where<MenuItemPortion>((Func<MenuItemPortion, bool>)(x => x.Id == id)).Sum<MenuItemPortion>((Func<MenuItemPortion, Decimal>)(x => x.Price));
        }

        public static AccountTransaction ScreenSaleBilling(
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransaction> accRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          IDCubeRepository<AccountTransactionDocument> accTransDocRepository,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepo,
          DCubeRepository<MenuItem> MenuItemRepo,
          DCubeRepository<MenuItemPortion> Menuportionrepo,
          DCubeRepository<MenuItemPortionPriceRange> MenuportionPriceRangerepo,
          string TransactionId)
        {
            string accounttypename = "Sales Transaction";
            List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
            AccountTransaction objAccountTransaction = new AccountTransaction();
            objAccountTransaction = accRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Id.ToString() == TransactionId)).ToList<AccountTransaction>().FirstOrDefault<AccountTransaction>();
            AccountTransactionDocument objAccountTransactionDocument = new AccountTransactionDocument();
            if (objAccountTransaction != null)
                objAccountTransactionDocument = accTransDocRepository.GetAllData().Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Id == objAccountTransaction.AccountTransactionDocumentId)).FirstOrDefault<AccountTransactionDocument>();
            if (objAccountTransactionDocument != null)
            {
                List<AccountTransactionValue> transactionValueList1 = new List<AccountTransactionValue>();
                List<AccountTransactionValue> list2 = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionId == objAccountTransaction.Id)).ToList<AccountTransactionValue>();
                if (objAccountTransaction != null)
                {
                    string str1 = string.Empty;
                    string str2 = string.Empty;
                    if (objAccountTransactionDocument != null)
                    {
                        string str3 = objAccountTransactionDocument.Name.Substring(objAccountTransactionDocument.Name.IndexOf("[#")).Replace("]", "").Replace("[#", "");
                        str2 = objAccountTransactionDocument.Name.Substring(0, objAccountTransactionDocument.Name.IndexOf("[#")).Trim();
                        str1 = str3;
                    }
                    Ticket objTicket = new Ticket();
                    objTicket = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.AccountTransactionDocumentId == objAccountTransaction.AccountTransactionDocumentId)).FirstOrDefault<Ticket>();
                    List<AccountTransactionDocument> transactionDocumentList = new List<AccountTransactionDocument>();
                    transactionDocumentList = accTransDocRepository.GetAllData().Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Name.Contains(accounttypename))).ToList<AccountTransactionDocument>();
                    Decimal num1 = 0M;
                    Decimal num2 = 0M;
                    List<AccountTransactionValue> transactionValueList2 = new List<AccountTransactionValue>();
                    List<AccountTransactionValue> list3 = list2.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionDocumentId == objAccountTransactionDocument.Id)).ToList<AccountTransactionValue>();
                    AccountTransactionValue transactionValue1 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue2 = list3.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == 3)).FirstOrDefault<AccountTransactionValue>();
                    AccountTransactionValue transactionValue3 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue4 = list3.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == 1)).FirstOrDefault<AccountTransactionValue>();
                    AccountTransactionValue transactionValue5 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue6 = list3.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == 8)).FirstOrDefault<AccountTransactionValue>();
                    Account account1 = new Account();
                    Account account2 = list1.Find((Predicate<Account>)(o => o.Name == "Excise Duty"));
                    int iExciseDutyAccountId = 0;
                    if (account2 != null)
                        iExciseDutyAccountId = account2.Id;
                    AccountTransactionValue transactionValue7 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue8 = list3.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == iExciseDutyAccountId)).FirstOrDefault<AccountTransactionValue>();
                    objAccountTransaction.Discount = transactionValue2 == null ? 0M : transactionValue2.Debit;
                    if (transactionValue4 != null)
                    {
                        objAccountTransaction.NetAmount = transactionValue4.Credit;
                        num1 = transactionValue4.Credit;
                    }
                    else
                        objAccountTransaction.NetAmount = 0M;
                    if (transactionValue8 != null)
                    {
                        objAccountTransaction.ExciseDuty = transactionValue8.Credit;
                        num1 = transactionValue8.Credit;
                    }
                    else
                        objAccountTransaction.ExciseDuty = 0M;
                    if (transactionValue6 != null)
                    {
                        objAccountTransaction.VATAmount = transactionValue6.Credit;
                        num2 = transactionValue6.Credit;
                    }
                    else
                        objAccountTransaction.VATAmount = 0M;
                    List<Order> orderList = new List<Order>();
                    if (objTicket != null)
                        orderList = OrderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o => o.TicketId == objTicket.Id)).ToList<Order>();
                    List<MenuItemWithPrice> menuItemWithPriceList = new List<MenuItemWithPrice>();
                    List<MenuItemWithPrice> menuCategoryItem = MenuBusinessLayer.GetMenuCategoryItem(MenuCategoryRepo, (IDCubeRepository<MenuItem>)MenuItemRepo, (IDCubeRepository<MenuItemPortion>)Menuportionrepo, (IDCubeRepository<MenuItemPortionPriceRange>)MenuportionPriceRangerepo);
                    List<MenuItem> menuItemList = new List<MenuItem>();
                    List<MenuItem> list4 = MenuItemRepo.GetAllData().ToList<MenuItem>();
                    List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
                    List<MenuItemPortion> list5 = Menuportionrepo.GetAllData().ToList<MenuItemPortion>();
                    List<ScreenOrderDetails> screenOrderDetailsList = new List<ScreenOrderDetails>();
                    foreach (Order order in orderList)
                    {
                        Order objOrder = order;
                        MenuItemWithPrice menuItemWithPrice = menuCategoryItem.Where<MenuItemWithPrice>((Func<MenuItemWithPrice, bool>)(o => o.ItemId == objOrder.MenuItemId)).FirstOrDefault<MenuItemWithPrice>();
                        MenuItemPortion objMenuItemPortion = new MenuItemPortion();
                        MenuItem menuItem = new MenuItem();
                        if (menuItemWithPrice != null)
                        {
                            objMenuItemPortion = list5.Find((Predicate<MenuItemPortion>)(o => o.Id == objOrder.MenuItemId));
                            if (objMenuItemPortion != null)
                                menuItem = list4.Find((Predicate<MenuItem>)(o => o.Id == objMenuItemPortion.MenuItemPortionId));
                        }
                        screenOrderDetailsList.Add(new ScreenOrderDetails()
                        {
                            Id = objOrder.Id,
                            ItemId = objOrder.MenuItemId,
                            OrderId = objOrder.OrderNumber,
                            Qty = Convert.ToDecimal(objOrder.Quantity),
                            UnitType = menuItem.UnitType,
                            UnitPrice = Convert.ToDecimal(objOrder.Price),
                            TaxRate = objOrder.TaxRate,
                            ExciseDuty = objOrder.ExciseDuty,
                            MRPPrice = Convert.ToDecimal(objOrder.MRPPrice),
                            Discount = Convert.ToDecimal(objOrder.Discount),
                            TotalAmount = Convert.ToDecimal(objOrder.Quantity) * Convert.ToDecimal(objOrder.Price),
                            BranchId = objOrder.BranchId,
                            CompanyCode = objOrder.CompanyCode,
                            DepartmentId = objOrder.DepartmentId,
                            WarehouseId = objOrder.WarehouseId,
                            OrderDescription = objOrder.OrderDescription
                        });
                    }
                    objAccountTransaction.SalesOrderDetails = screenOrderDetailsList;
                    objAccountTransaction.AccountTransactionValues = list3;
                }
            }
            return objAccountTransaction;
        }

        public static int Create(
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransactionType> accTransTypeRepository,
          IDCubeRepository<AccountTransaction> accRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          IDCubeRepository<AccountTransactionDocument> accTransDocRepository,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          AccountTransaction value)
        {
            int num1 = 0;
            if (value.SalesOrderDetails != null)
            {
                if (value.SalesOrderDetails.Count > 0)
                {
                    DateTime now1 = DateTime.Now;
                    DateTime.Now.ToString();
                    string empty1 = string.Empty;
                    string NDate;
                    if (value.Date != null)
                    {
                        NDate = value.Date;
                    }
                    else
                    {
                        string shortDateString = now1.ToShortDateString();
                        int num2 = now1.Year;
                        num2.ToString();
                        num2 = now1.Month;
                        string str1 = num2.ToString();
                        num2 = now1.Day;
                        string str2 = num2.ToString();
                        if (str1.Length == 1)
                        {
                            string str3 = "0" + str1;
                        }
                        if (str2.Length == 1)
                        {
                            string str4 = "0" + str2;
                        }
                        NDate = NepalitoEnglishDate.NepaliDate(DateTime.Parse(shortDateString));
                    }
                    DateTime dateTime = NepalitoEnglishDate.EnglishDate(NDate);
                    DateTime now2 = DateTime.Now;
                    DateTime CurrentDate = dateTime.AddHours((double)now2.Hour).AddMinutes((double)now2.Minute).AddSeconds((double)now2.Second);
                    string nepaliMonth = NepalitoEnglish.englishToNepaliMonth(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day);
                    string empty2 = string.Empty;
                    AccountTransactionType accountTransactionType1 = new AccountTransactionType();
                    AccountTransactionType accountTransactionType2 = accTransTypeRepository.GetAllData().Where<AccountTransactionType>((Func<AccountTransactionType, bool>)(o => o.Name == "Sales Transaction")).FirstOrDefault<AccountTransactionType>() ?? accTransTypeRepository.GetAllData().Where<AccountTransactionType>((Func<AccountTransactionType, bool>)(o => o.Name == "Sale Transaction")).FirstOrDefault<AccountTransactionType>();
                    List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
                    string TicketNo = TicketSaleBillingBusiness.TicketLastNumber(TicketRepository, value);
                    using (UnitOfWork unitOfWork = new UnitOfWork())
                    {
                        unitOfWork.StartTransaction();
                        try
                        {
                            string str5 = value.Name + " [#" + TicketNo + "]";
                            accTransDocRepository.Insert(new AccountTransactionDocument()
                            {
                                Date = CurrentDate,
                                DocumentTypeId = 0,
                                Name = str5,
                                Printed_Time = DateTime.Now,
                                FinancialYear = value.FinancialYear
                            });
                            accTransDocRepository.Save();
                            int id1 = accTransDocRepository.GetAllData().ToList<AccountTransactionDocument>().Last<AccountTransactionDocument>().Id;
                            int num3 = TicketSaleBillingBusiness.TicketSave(value, TicketRepository, TicketNo, CurrentDate, id1);
                            if (value.Name + " [#" + TicketNo + "]" == str5)
                            {
                                accRepository.Insert(new AccountTransaction()
                                {
                                    AccountTransactionDocumentId = id1,
                                    Name = value.Name + " [#" + TicketNo + "]",
                                    Date = CurrentDate.ToString(),
                                    Amount = value.SalesOrderDetails.Where<ScreenOrderDetails>((Func<ScreenOrderDetails, bool>)(o => o.ItemId != 0)).Sum<ScreenOrderDetails>((Func<ScreenOrderDetails, Decimal>)(o => o.TotalAmount)),
                                    Description = value.Description,
                                    ExchangeRate = value.ExchangeRate,
                                    AccountTransactionTypeId = accountTransactionType2.Id,
                                    SourceAccountTypeId = value.SourceAccountTypeId,
                                    TargetAccountTypeId = accountTransactionType2.TargetAccountTypeId,
                                    ref_invoice_number = value.ref_invoice_number,
                                    IsReversed = false,
                                    Reversable = true,
                                    Printed_Time = DateTime.Now,
                                    FinancialYear = value.FinancialYear,
                                    UserName = value.UserName,
                                    VehicleNo = value.VehicleNo,
                                    VehicleHeight = value.VehicleHeight,
                                    VehicleLength = value.VehicleLength,
                                    VehicleWidth = value.VehicleWidth,
                                    BranchId = value.BranchId,
                                    CompanyCode = value.CompanyCode,
                                    DepartmentId = value.DepartmentId,
                                    WareHouseId = value.WareHouseId,
                                    ExciseDuty = value.ExciseDuty
                                });
                                accRepository.Save();
                                int id2 = accRepository.GetAllData().OrderByDescending<AccountTransaction, int>((Func<AccountTransaction, int>)(x => x.Id)).FirstOrDefault<AccountTransaction>().Id;
                                num1 = id2;
                                AccountTransactionValue transactionValue1 = new AccountTransactionValue();
                                transactionValue1.AccountTransactionId = id2;
                                transactionValue1.AccountTransactionDocumentId = id1;
                                if (value.Name == "Credit Note")
                                {
                                    transactionValue1.Credit = value.Discount;
                                    transactionValue1.Debit = 0M;
                                    transactionValue1.entityLists = "Cr";
                                }
                                else
                                {
                                    transactionValue1.Credit = 0M;
                                    if (value.Discount != 0M && value.PercentAmount == 0M)
                                        transactionValue1.Debit = value.Discount;
                                    if (value.PercentAmount != 0M)
                                        transactionValue1.Debit = value.PercentAmount;
                                    transactionValue1.entityLists = "Dr";
                                }
                                transactionValue1.AccountId = 3;
                                transactionValue1.Name = value.Name + " [#" + TicketNo + "]";
                                transactionValue1.AccountTypeId = list1.Find((Predicate<Account>)(o => o.Id == 3)).AccountTypeId;
                                transactionValue1.Description = value.Description;
                                transactionValue1.Date = CurrentDate;
                                transactionValue1.Printed_Time = DateTime.Now;
                                transactionValue1.NVDate = NDate;
                                transactionValue1.FinancialYear = value.FinancialYear;
                                transactionValue1.NepaliMonth = nepaliMonth;
                                transactionValue1.UserName = value.UserName;
                                transactionValue1.BranchId = value.BranchId;
                                transactionValue1.CompanyCode = value.CompanyCode;
                                transactionValue1.DepartmentId = value.DepartmentId;
                                transactionValue1.WareHouseId = value.WareHouseId;
                                accValueRepository.Insert(transactionValue1);
                                accValueRepository.Save();
                                Account account1 = new Account();
                                Account account2 = list1.Find((Predicate<Account>)(o => o.Name == "Taxable Sales"));
                                int iTaxableAccountId = 0;
                                if (account2 != null)
                                    iTaxableAccountId = account2.Id;
                                AccountTransactionValue transactionValue2 = new AccountTransactionValue();
                                transactionValue2.AccountTransactionId = id2;
                                transactionValue2.AccountTransactionDocumentId = id1;
                                if (value.Name == "Credit Note")
                                {
                                    transactionValue2.Debit = value.SalesOrderDetails.Where<ScreenOrderDetails>((Func<ScreenOrderDetails, bool>)(o => o.ItemId != 0 && o.TaxRate != 0M)).Sum<ScreenOrderDetails>((Func<ScreenOrderDetails, Decimal>)(o => o.TotalAmount));
                                    transactionValue2.entityLists = "Dr";
                                    transactionValue2.Credit = 0M;
                                }
                                else
                                {
                                    transactionValue2.Credit = value.SalesOrderDetails.Where<ScreenOrderDetails>((Func<ScreenOrderDetails, bool>)(o => o.ItemId != 0 && o.TaxRate != 0M)).Sum<ScreenOrderDetails>((Func<ScreenOrderDetails, Decimal>)(o => o.TotalAmount));
                                    transactionValue2.entityLists = "Cr";
                                    transactionValue2.Debit = 0M;
                                }
                                transactionValue2.AccountId = iTaxableAccountId;
                                transactionValue2.Name = value.Name + " [#" + TicketNo + "]";
                                transactionValue2.AccountTypeId = list1.Find((Predicate<Account>)(o => o.Id == iTaxableAccountId)).AccountTypeId;
                                transactionValue2.Description = value.Description;
                                transactionValue2.Date = CurrentDate;
                                transactionValue2.Printed_Time = DateTime.Now;
                                transactionValue2.NVDate = NDate;
                                transactionValue2.FinancialYear = value.FinancialYear;
                                transactionValue2.NepaliMonth = nepaliMonth;
                                transactionValue2.UserName = value.UserName;
                                transactionValue2.BranchId = value.BranchId;
                                transactionValue2.CompanyCode = value.CompanyCode;
                                transactionValue2.DepartmentId = value.DepartmentId;
                                transactionValue2.WareHouseId = value.WareHouseId;
                                accValueRepository.Insert(transactionValue2);
                                accValueRepository.Save();
                                Account account3 = new Account();
                                Account account4 = list1.Find((Predicate<Account>)(o => o.Name == "Non Taxable Sales"));
                                int inonTaxableAccountId = 0;
                                if (account4 != null)
                                    inonTaxableAccountId = account4.Id;
                                AccountTransactionValue transactionValue3 = new AccountTransactionValue();
                                transactionValue3.AccountTransactionId = id2;
                                transactionValue3.AccountTransactionDocumentId = id1;
                                if (value.Name == "Credit Note")
                                {
                                    transactionValue3.Debit = value.SalesOrderDetails.Where<ScreenOrderDetails>((Func<ScreenOrderDetails, bool>)(o => o.ItemId != 0 && o.TaxRate == 0M)).Sum<ScreenOrderDetails>((Func<ScreenOrderDetails, Decimal>)(o => o.TotalAmount));
                                    transactionValue3.entityLists = "Dr";
                                    transactionValue3.Credit = 0M;
                                }
                                else
                                {
                                    transactionValue3.Credit = value.SalesOrderDetails.Where<ScreenOrderDetails>((Func<ScreenOrderDetails, bool>)(o => o.ItemId != 0 && o.TaxRate == 0M)).Sum<ScreenOrderDetails>((Func<ScreenOrderDetails, Decimal>)(o => o.TotalAmount));
                                    transactionValue3.entityLists = "Cr";
                                    transactionValue3.Debit = 0M;
                                }
                                transactionValue3.AccountId = inonTaxableAccountId;
                                transactionValue3.Name = value.Name + " [#" + TicketNo + "]";
                                transactionValue3.AccountTypeId = list1.Find((Predicate<Account>)(o => o.Id == inonTaxableAccountId)).AccountTypeId;
                                transactionValue3.Description = value.Description;
                                transactionValue3.Date = CurrentDate;
                                transactionValue3.Printed_Time = DateTime.Now;
                                transactionValue3.NVDate = NDate;
                                transactionValue3.FinancialYear = value.FinancialYear;
                                transactionValue3.NepaliMonth = nepaliMonth;
                                transactionValue3.UserName = value.UserName;
                                transactionValue3.BranchId = value.BranchId;
                                transactionValue3.CompanyCode = value.CompanyCode;
                                transactionValue3.DepartmentId = value.DepartmentId;
                                transactionValue3.WareHouseId = value.WareHouseId;
                                accValueRepository.Insert(transactionValue3);
                                accValueRepository.Save();
                                AccountTransactionValue transactionValue4 = new AccountTransactionValue();
                                transactionValue4.AccountTransactionId = id2;
                                transactionValue4.AccountTransactionDocumentId = id1;
                                if (value.Name == "Credit Note")
                                {
                                    transactionValue4.Credit = 0M;
                                    transactionValue4.Debit = value.VATAmount;
                                    transactionValue4.entityLists = "Dr";
                                }
                                else
                                {
                                    transactionValue4.Credit = value.VATAmount;
                                    transactionValue4.Debit = 0M;
                                    transactionValue4.entityLists = "Cr";
                                }
                                transactionValue4.AccountId = 8;
                                transactionValue4.Name = value.Name + " [#" + TicketNo + "]";
                                transactionValue4.AccountTypeId = list1.Find((Predicate<Account>)(o => o.Id == 8)).AccountTypeId;
                                transactionValue4.Description = value.Description;
                                transactionValue4.Date = CurrentDate;
                                transactionValue4.Printed_Time = DateTime.Now;
                                transactionValue4.NVDate = NDate;
                                transactionValue4.FinancialYear = value.FinancialYear;
                                transactionValue4.NepaliMonth = nepaliMonth;
                                transactionValue4.UserName = value.UserName;
                                transactionValue4.BranchId = value.BranchId;
                                transactionValue4.CompanyCode = value.CompanyCode;
                                transactionValue4.DepartmentId = value.DepartmentId;
                                transactionValue4.WareHouseId = value.WareHouseId;
                                accValueRepository.Insert(transactionValue4);
                                accValueRepository.Save();
                                Account account5 = new Account();
                                Account account6 = list1.Find((Predicate<Account>)(o => o.Name == "Excise Duty"));
                                int num4 = 0;
                                if (account6 != null)
                                    num4 = account6.Id;
                                AccountTransactionValue transactionValue5 = new AccountTransactionValue();
                                transactionValue5.AccountTransactionId = id2;
                                transactionValue5.AccountTransactionDocumentId = id1;
                                if (value.Name == "Credit Note")
                                {
                                    transactionValue5.Credit = 0M;
                                    transactionValue5.Debit = value.ExciseDuty;
                                    transactionValue5.entityLists = "Dr";
                                }
                                else
                                {
                                    transactionValue5.Credit = value.ExciseDuty;
                                    transactionValue5.Debit = 0M;
                                    transactionValue5.entityLists = "Cr";
                                }
                                transactionValue5.AccountId = num4;
                                transactionValue5.Name = value.Name + " [#" + TicketNo + "]";
                                transactionValue5.AccountTypeId = list1.Find((Predicate<Account>)(o => o.Id == 8)).AccountTypeId;
                                transactionValue5.Description = value.Description;
                                transactionValue5.Date = CurrentDate;
                                transactionValue5.Printed_Time = DateTime.Now;
                                transactionValue5.NVDate = NDate;
                                transactionValue5.FinancialYear = value.FinancialYear;
                                transactionValue5.NepaliMonth = nepaliMonth;
                                transactionValue5.UserName = value.UserName;
                                transactionValue5.BranchId = value.BranchId;
                                transactionValue5.CompanyCode = value.CompanyCode;
                                transactionValue5.DepartmentId = value.DepartmentId;
                                transactionValue5.WareHouseId = value.WareHouseId;
                                accValueRepository.Insert(transactionValue5);
                                accValueRepository.Save();
                                AccountTransactionValue transactionValue6 = new AccountTransactionValue();
                                transactionValue6.AccountTransactionId = id2;
                                transactionValue6.AccountTransactionDocumentId = id1;
                                if (value.Name == "Credit Note")
                                {
                                    transactionValue6.Credit = value.GrandAmount;
                                    transactionValue6.Debit = 0M;
                                    transactionValue6.entityLists = "Cr";
                                }
                                else
                                {
                                    transactionValue6.Credit = 0M;
                                    transactionValue6.Debit = value.GrandAmount;
                                    transactionValue6.entityLists = "Dr";
                                }
                                transactionValue6.AccountId = value.SourceAccountTypeId;
                                transactionValue6.Name = value.Name + " [#" + TicketNo + "]";
                                transactionValue6.AccountTypeId = list1.Find((Predicate<Account>)(o => o.Id == value.SourceAccountTypeId)).AccountTypeId;
                                transactionValue6.Description = value.Description;
                                transactionValue6.Date = CurrentDate;
                                transactionValue6.Printed_Time = DateTime.Now;
                                transactionValue6.NVDate = NDate;
                                transactionValue6.NepaliMonth = nepaliMonth;
                                transactionValue6.FinancialYear = value.FinancialYear;
                                transactionValue6.UserName = value.UserName;
                                transactionValue6.BranchId = value.BranchId;
                                transactionValue6.CompanyCode = value.CompanyCode;
                                transactionValue6.DepartmentId = value.DepartmentId;
                                transactionValue6.WareHouseId = value.WareHouseId;
                                accValueRepository.Insert(transactionValue6);
                                accValueRepository.Save();
                                foreach (ScreenOrderDetails salesOrderDetail in value.SalesOrderDetails)
                                {
                                    if (salesOrderDetail.ItemId != 0)
                                    {
                                        Order order = new Order()
                                        {
                                            OrderDescription = salesOrderDetail.OrderDescription,
                                            AccountTransactionTypeId = 3,
                                            CreatedDateTime = DateTime.Now,
                                            DecreaseInventory = true,
                                            DepartmentId = salesOrderDetail.DepartmentId,
                                            IncreaseInventory = false,
                                            IS_Bill_Active = false,
                                            IS_Bill_Printed = true,
                                            Locked = true,
                                            MenuItemId = salesOrderDetail.ItemId,
                                            OrderNumber = 0,
                                            OrderStates = "Closed",
                                            Price = salesOrderDetail.UnitPrice,
                                            TaxRate = salesOrderDetail.TaxRate,
                                            ExciseDuty = salesOrderDetail.ExciseDuty,
                                            MRPPrice = salesOrderDetail.MRPPrice,
                                            Discount = salesOrderDetail.Discount,
                                            Quantity = !(value.Name == "Credit Note") ? salesOrderDetail.Qty : -salesOrderDetail.Qty,
                                            Real_Time = false,
                                            Sync_With_IRD = false,
                                            TicketId = num3,
                                            WarehouseId = salesOrderDetail.WarehouseId,
                                            Printed_Time = DateTime.Now,
                                            FinancialYear = value.FinancialYear,
                                            NVDate = NDate,
                                            NepaliMonth = nepaliMonth,
                                            CreatingUserName = value.UserName,
                                            BranchId = value.BranchId,
                                            CompanyCode = value.CompanyCode
                                        };
                                        order.DepartmentId = value.DepartmentId;
                                        order.WarehouseId = value.WareHouseId;
                                        OrderRepository.Insert(order);
                                        OrderRepository.Save();
                                    }
                                }
                                if (value.Name == "Cash Sales")
                                {
                                    string receipttype = string.Empty;
                                    string empty3 = string.Empty;
                                    List<AccountTransactionType> accountTransactionTypeList = new List<AccountTransactionType>();
                                    List<AccountTransactionType> list2 = accTransTypeRepository.GetAllData().ToList<AccountTransactionType>();
                                    AccountTransactionType accountTransactionType3 = new AccountTransactionType();
                                    AccountTransactionType accountTransactionType4 = list2.Find((Predicate<AccountTransactionType>)(o => o.Name.Contains("Receipt")));
                                    if (accountTransactionType4 != null)
                                        receipttype = accountTransactionType4.Name;
                                    int num5 = 0;
                                    List<AccountTransactionDocument> transactionDocumentList = new List<AccountTransactionDocument>();
                                    List<AccountTransactionDocument> list3 = accTransDocRepository.GetAllData().ToList<AccountTransactionDocument>();
                                    int num6;
                                    if (list3.Count == 0)
                                    {
                                        num6 = num5 + 1;
                                    }
                                    else
                                    {
                                        List<AccountTransactionDocument> list4 = list3.Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Name.Contains(receipttype))).ToList<AccountTransactionDocument>();
                                        if (list4.Count == 0)
                                        {
                                            num6 = num5 + 1;
                                        }
                                        else
                                        {
                                            AccountTransactionDocument transactionDocument1 = new AccountTransactionDocument();
                                            AccountTransactionDocument transactionDocument2 = list4.OrderByDescending<AccountTransactionDocument, int>((Func<AccountTransactionDocument, int>)(o => o.Id)).FirstOrDefault<AccountTransactionDocument>();
                                            num6 = int.Parse(transactionDocument2.Name.Substring(transactionDocument2.Name.IndexOf("[#")).Replace("]", "").Replace("[#", "")) + 1;
                                        }
                                    }
                                    string str6 = accountTransactionType4.Name + " [#" + (object)num6 + "]";
                                    accTransDocRepository.Insert(new AccountTransactionDocument()
                                    {
                                        Date = CurrentDate,
                                        DocumentTypeId = 0,
                                        Name = str6,
                                        Printed_Time = DateTime.Now,
                                        FinancialYear = value.FinancialYear
                                    });
                                    accTransDocRepository.Save();
                                    int id3 = accTransDocRepository.GetAllData().ToList<AccountTransactionDocument>().Last<AccountTransactionDocument>().Id;
                                    accRepository.Insert(new AccountTransaction()
                                    {
                                        AccountTransactionDocumentId = id3,
                                        Name = "Receipt[#" + (object)num6 + "]",
                                        Date = CurrentDate.ToString(),
                                        Amount = value.GrandAmount,
                                        Description = value.Description,
                                        ExchangeRate = value.ExchangeRate,
                                        AccountTransactionTypeId = accountTransactionType4.Id,
                                        SourceAccountTypeId = 5,
                                        TargetAccountTypeId = accountTransactionType4.TargetAccountTypeId,
                                        ref_invoice_number = value.ref_invoice_number,
                                        IsReversed = false,
                                        Reversable = true,
                                        Printed_Time = DateTime.Now,
                                        FinancialYear = value.FinancialYear,
                                        UserName = value.UserName,
                                        VehicleNo = value.VehicleNo,
                                        VehicleHeight = value.VehicleHeight,
                                        VehicleLength = value.VehicleLength,
                                        VehicleWidth = value.VehicleWidth,
                                        BranchId = value.BranchId,
                                        CompanyCode = value.CompanyCode,
                                        DepartmentId = value.DepartmentId,
                                        WareHouseId = value.WareHouseId,
                                        ExciseDuty = value.ExciseDuty
                                    });
                                    accRepository.Save();
                                    int id4 = accRepository.GetAllData().OrderByDescending<AccountTransaction, int>((Func<AccountTransaction, int>)(x => x.Id)).FirstOrDefault<AccountTransaction>().Id;
                                    accValueRepository.Insert(new AccountTransactionValue()
                                    {
                                        AccountTransactionId = id4,
                                        AccountTransactionDocumentId = id3,
                                        Credit = 0M,
                                        Debit = value.GrandAmount,
                                        entityLists = "Dr",
                                        AccountId = 5,
                                        Name = "Receipt [#" + (object)num6 + "]",
                                        ref_invoice_number = value.ref_invoice_number,
                                        Description = value.Name,
                                        AccountTypeId = list1.Find((Predicate<Account>)(o => o.Id == 5)).AccountTypeId,
                                        Date = CurrentDate,
                                        NVDate = NDate,
                                        FinancialYear = value.FinancialYear,
                                        NepaliMonth = nepaliMonth,
                                        Printed_Time = DateTime.Now,
                                        UserName = value.UserName,
                                        BranchId = value.BranchId,
                                        DepartmentId = value.DepartmentId,
                                        CompanyCode = value.CompanyCode,
                                        WareHouseId = value.WareHouseId
                                    });
                                    accValueRepository.Save();
                                    accValueRepository.Insert(new AccountTransactionValue()
                                    {
                                        AccountTransactionId = id4,
                                        AccountTransactionDocumentId = id3,
                                        Credit = value.GrandAmount,
                                        Debit = 0M,
                                        entityLists = "Cr",
                                        AccountId = value.SourceAccountTypeId,
                                        Name = "Receipt [#" + (object)num6 + "]",
                                        ref_invoice_number = value.ref_invoice_number,
                                        Description = value.Name,
                                        AccountTypeId = list1.Find((Predicate<Account>)(o => o.Id == value.SourceAccountTypeId)).AccountTypeId,
                                        Date = CurrentDate,
                                        NVDate = NDate,
                                        FinancialYear = value.FinancialYear,
                                        NepaliMonth = nepaliMonth,
                                        Printed_Time = DateTime.Now,
                                        UserName = value.UserName,
                                        BranchId = value.BranchId,
                                        DepartmentId = value.DepartmentId,
                                        CompanyCode = value.CompanyCode,
                                        WareHouseId = value.WareHouseId
                                    });
                                    accValueRepository.Save();
                                }
                                unitOfWork.CommitTransaction();
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.ErrorLogging(ex);
                            unitOfWork.RollBackTransaction();
                            num1 = 0;
                        }
                    }
                }
                else
                    num1 = 0;
            }
            else
                num1 = 0;
            return num1;
        }

        public static int Edit(
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransaction> accRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<AccountTransactionDocument> AccountTransactionDocumentRepository,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          AccountTransaction value)
        {
            int num = 0;
            if (value.SalesOrderDetails != null)
            {
                if (value.SalesOrderDetails.Count > 0)
                {
                    string date = value.Date;
                    DateTime dateTime1 = NepalitoEnglishDate.EnglishDate(date);
                    DateTime now = DateTime.Now;
                    DateTime dateTime2 = dateTime1.AddHours((double)now.Hour).AddMinutes((double)now.Minute).AddSeconds((double)now.Second);
                    string nepaliMonth = NepalitoEnglish.englishToNepaliMonth(dateTime2.Year, dateTime2.Month, dateTime2.Day);
                    List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
                    AccountTransactionType accountTransactionType1 = new AccountTransactionType();
                    AccountTransactionType accountTransactionType2 = AccountTransactionTypeRepository.GetAllData().Where<AccountTransactionType>((Func<AccountTransactionType, bool>)(o => o.Name == "Sales Transaction")).FirstOrDefault<AccountTransactionType>() ?? AccountTransactionTypeRepository.GetAllData().Where<AccountTransactionType>((Func<AccountTransactionType, bool>)(o => o.Name == "Sale Transaction")).FirstOrDefault<AccountTransactionType>();
                    using (UnitOfWork unitOfWork = new UnitOfWork())
                    {
                        unitOfWork.StartTransaction();
                        try
                        {
                            AccountTransaction accountTransaction1 = new AccountTransaction();
                            AccountTransaction accountTransaction2 = accRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Id == value.Id)).FirstOrDefault<AccountTransaction>();
                            if (accountTransaction2 != null)
                            {
                                string TicketNumber = accountTransaction2.Name.Substring(accountTransaction2.Name.IndexOf("[#")).Replace("]", "").Replace("[#", "");
                                accountTransaction2.AccountTransactionTypeId = accountTransactionType2.Id;
                                accountTransaction2.Amount = value.SalesOrderDetails.Where<ScreenOrderDetails>((Func<ScreenOrderDetails, bool>)(o => o.ItemId != 0)).Sum<ScreenOrderDetails>((Func<ScreenOrderDetails, Decimal>)(o => o.TotalAmount));
                                accountTransaction2.Description = value.Description;
                                accountTransaction2.ExchangeRate = value.ExchangeRate;
                                accountTransaction2.SourceAccountTypeId = value.SourceAccountTypeId;
                                accountTransaction2.TargetAccountTypeId = value.TargetAccountTypeId;
                                accountTransaction2.ref_invoice_number = value.ref_invoice_number;
                                accountTransaction2.Date = dateTime2.ToString();
                                accountTransaction2.IsReversed = false;
                                accountTransaction2.Reversable = true;
                                accountTransaction2.Reversable = true;
                                accountTransaction2.Printed_Time = DateTime.Now;
                                accountTransaction2.FinancialYear = value.FinancialYear;
                                accountTransaction2.UserName = value.UserName;
                                accountTransaction2.VehicleNo = value.VehicleNo;
                                accountTransaction2.VehicleHeight = value.VehicleHeight;
                                accountTransaction2.VehicleLength = value.VehicleLength;
                                accountTransaction2.VehicleWidth = value.VehicleWidth;
                                accountTransaction2.BranchId = value.BranchId;
                                accountTransaction2.CompanyCode = value.CompanyCode;
                                accountTransaction2.DepartmentId = value.DepartmentId;
                                accountTransaction2.WareHouseId = value.WareHouseId;
                                accRepository.Update(accountTransaction2);
                                accRepository.Save();
                                List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
                                List<AccountTransactionValue> list2 = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionId == value.Id)).ToList<AccountTransactionValue>();
                                AccountTransactionValue transactionValue1 = new AccountTransactionValue();
                                AccountTransactionValue transactionValue2 = list2.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == 3)).FirstOrDefault<AccountTransactionValue>();
                                if (value.Name.Contains("Credit Note"))
                                {
                                    transactionValue2.Credit = value.Discount;
                                    transactionValue2.Credit = 0M;
                                }
                                else
                                {
                                    if (value.Discount != 0M && value.PercentAmount == 0M)
                                        transactionValue2.Debit = value.Discount;
                                    if (value.PercentAmount != 0M)
                                        transactionValue2.Debit = value.PercentAmount;
                                    if (value.PercentAmount == 0M && value.Discount == 0M)
                                        transactionValue2.Debit = 0M;
                                }
                                transactionValue2.Description = value.Description;
                                transactionValue2.Date = dateTime2;
                                transactionValue2.NVDate = date;
                                transactionValue2.NepaliMonth = nepaliMonth;
                                transactionValue2.UserName = value.UserName;
                                transactionValue2.BranchId = value.BranchId;
                                transactionValue2.CompanyCode = value.CompanyCode;
                                transactionValue2.DepartmentId = value.DepartmentId;
                                transactionValue2.WareHouseId = value.WareHouseId;
                                accValueRepository.Update(transactionValue2);
                                accValueRepository.Save();
                                Account account1 = new Account();
                                Account account2 = list1.Find((Predicate<Account>)(o => o.Name == "Taxable Sales"));
                                int iTaxableAccountId = 0;
                                if (account2 != null)
                                    iTaxableAccountId = account2.Id;
                                AccountTransactionValue transactionValue3 = new AccountTransactionValue();
                                AccountTransactionValue transactionValue4 = list2.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == iTaxableAccountId)).FirstOrDefault<AccountTransactionValue>();
                                if (value.Name.Contains("Credit Note"))
                                {
                                    transactionValue4.Credit = 0M;
                                    transactionValue4.Debit = value.SalesOrderDetails.Where<ScreenOrderDetails>((Func<ScreenOrderDetails, bool>)(o => o.ItemId != 0 && o.TaxRate != 0M)).Sum<ScreenOrderDetails>((Func<ScreenOrderDetails, Decimal>)(o => o.TotalAmount));
                                }
                                else
                                {
                                    transactionValue4.Credit = value.SalesOrderDetails.Where<ScreenOrderDetails>((Func<ScreenOrderDetails, bool>)(o => o.ItemId != 0 && o.TaxRate != 0M)).Sum<ScreenOrderDetails>((Func<ScreenOrderDetails, Decimal>)(o => o.TotalAmount));
                                    transactionValue4.Debit = 0M;
                                }
                                transactionValue4.Description = value.Description;
                                transactionValue4.Date = dateTime2;
                                transactionValue4.NVDate = date;
                                transactionValue4.NepaliMonth = nepaliMonth;
                                transactionValue4.UserName = value.UserName;
                                transactionValue4.BranchId = value.BranchId;
                                transactionValue4.CompanyCode = value.CompanyCode;
                                transactionValue4.DepartmentId = value.DepartmentId;
                                transactionValue4.WareHouseId = value.WareHouseId;
                                accValueRepository.Update(transactionValue4);
                                accValueRepository.Save();
                                Account account3 = new Account();
                                Account account4 = list1.Find((Predicate<Account>)(o => o.Name == "Non Taxable Sales"));
                                int inonTaxableAccountId = 0;
                                if (account4 != null)
                                    inonTaxableAccountId = account4.Id;
                                AccountTransactionValue transactionValue5 = new AccountTransactionValue();
                                AccountTransactionValue transactionValue6 = list2.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == inonTaxableAccountId)).FirstOrDefault<AccountTransactionValue>();
                                if (value.Name.Contains("Credit Note"))
                                {
                                    transactionValue6.Credit = 0M;
                                    transactionValue6.Debit = value.SalesOrderDetails.Where<ScreenOrderDetails>((Func<ScreenOrderDetails, bool>)(o => o.ItemId != 0 && o.TaxRate == 0M)).Sum<ScreenOrderDetails>((Func<ScreenOrderDetails, Decimal>)(o => o.TotalAmount));
                                }
                                else
                                {
                                    transactionValue6.Credit = value.SalesOrderDetails.Where<ScreenOrderDetails>((Func<ScreenOrderDetails, bool>)(o => o.ItemId != 0 && o.TaxRate == 0M)).Sum<ScreenOrderDetails>((Func<ScreenOrderDetails, Decimal>)(o => o.TotalAmount));
                                    transactionValue6.Debit = 0M;
                                }
                                transactionValue6.Description = value.Description;
                                transactionValue6.Date = dateTime2;
                                transactionValue6.NVDate = date;
                                transactionValue6.NepaliMonth = nepaliMonth;
                                transactionValue6.UserName = value.UserName;
                                transactionValue6.BranchId = value.BranchId;
                                transactionValue6.CompanyCode = value.CompanyCode;
                                transactionValue6.DepartmentId = value.DepartmentId;
                                transactionValue6.WareHouseId = value.WareHouseId;
                                accValueRepository.Update(transactionValue6);
                                accValueRepository.Save();
                                AccountTransactionValue transactionValue7 = new AccountTransactionValue();
                                AccountTransactionValue transactionValue8 = list2.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == 8)).FirstOrDefault<AccountTransactionValue>();
                                if (value.Name.Contains("Credit Note"))
                                {
                                    transactionValue8.Credit = 0M;
                                    transactionValue8.Debit = value.VATAmount;
                                }
                                else
                                {
                                    transactionValue8.Credit = value.VATAmount;
                                    transactionValue8.Debit = 0M;
                                }
                                transactionValue8.Description = value.Description;
                                transactionValue8.Date = dateTime2;
                                transactionValue8.NVDate = date;
                                transactionValue8.NepaliMonth = nepaliMonth;
                                transactionValue8.UserName = value.UserName;
                                transactionValue8.BranchId = value.BranchId;
                                transactionValue8.CompanyCode = value.CompanyCode;
                                transactionValue8.DepartmentId = value.DepartmentId;
                                transactionValue8.WareHouseId = value.WareHouseId;
                                accValueRepository.Update(transactionValue8);
                                accValueRepository.Save();
                                Account account5 = new Account();
                                Account account6 = list1.Find((Predicate<Account>)(o => o.Name == "Excise Duty"));
                                int iExciseDutyAccountId = 0;
                                if (account6 != null)
                                    iExciseDutyAccountId = account6.Id;
                                AccountTransactionValue transactionValue9 = new AccountTransactionValue();
                                AccountTransactionValue transactionValue10 = list2.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == iExciseDutyAccountId)).FirstOrDefault<AccountTransactionValue>();
                                if (value.Name.Contains("Credit Note"))
                                {
                                    transactionValue10.Credit = 0M;
                                    transactionValue10.Debit = value.ExciseDuty;
                                }
                                else
                                {
                                    transactionValue10.Credit = value.ExciseDuty;
                                    transactionValue10.Debit = 0M;
                                }
                                transactionValue10.Description = value.Description;
                                transactionValue10.Date = dateTime2;
                                transactionValue10.NVDate = date;
                                transactionValue10.NepaliMonth = nepaliMonth;
                                transactionValue10.UserName = value.UserName;
                                transactionValue10.BranchId = value.BranchId;
                                transactionValue10.CompanyCode = value.CompanyCode;
                                transactionValue10.DepartmentId = value.DepartmentId;
                                transactionValue10.WareHouseId = value.WareHouseId;
                                accValueRepository.Update(transactionValue10);
                                accValueRepository.Save();
                                AccountTransactionValue transactionValue11 = new AccountTransactionValue();
                                AccountTransactionValue transactionValue12 = list2.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == value.SourceAccountTypeId)).FirstOrDefault<AccountTransactionValue>();
                                if (value.Name.Contains("Credit Note"))
                                {
                                    transactionValue12.Debit = 0M;
                                    transactionValue12.Credit = value.GrandAmount;
                                }
                                else
                                {
                                    transactionValue12.Debit = value.GrandAmount;
                                    transactionValue12.Credit = 0M;
                                }
                                transactionValue12.AccountId = value.SourceAccountTypeId;
                                transactionValue12.AccountTypeId = list1.Find((Predicate<Account>)(o => o.Id == value.SourceAccountTypeId)).AccountTypeId;
                                transactionValue12.Description = value.Description;
                                transactionValue12.Date = dateTime2;
                                transactionValue12.NVDate = date;
                                transactionValue12.NepaliMonth = nepaliMonth;
                                transactionValue12.UserName = value.UserName;
                                transactionValue12.BranchId = value.BranchId;
                                transactionValue12.CompanyCode = value.CompanyCode;
                                transactionValue12.DepartmentId = value.DepartmentId;
                                transactionValue12.WareHouseId = value.WareHouseId;
                                accValueRepository.Update(transactionValue12);
                                accValueRepository.Save();
                                Ticket ticket1 = new Ticket();
                                Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.TicketNumber == TicketNumber && o.FinancialYear == value.FinancialYear)).FirstOrDefault<Ticket>();
                                if (ticket2 != null)
                                    TicketSaleBillingBusiness.TicketUpdate(value, TicketRepository, ticket2.TicketNumber, date);
                                foreach (ScreenOrderDetails salesOrderDetail in value.SalesOrderDetails)
                                {
                                    ScreenOrderDetails objScreenOrderDetails = salesOrderDetail;
                                    Order order1 = new Order();
                                    if (objScreenOrderDetails.Id >= 1)
                                    {
                                        Order order2 = OrderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o => o.Id == objScreenOrderDetails.Id)).FirstOrDefault<Order>();
                                        if (order2 != null)
                                        {
                                            order2.Quantity = !value.Name.Contains("Credit Note") ? objScreenOrderDetails.Qty : -objScreenOrderDetails.Qty;
                                            order2.OrderDescription = objScreenOrderDetails.OrderDescription;
                                            order2.Price = objScreenOrderDetails.UnitPrice;
                                            order2.TaxRate = objScreenOrderDetails.TaxRate;
                                            order2.ExciseDuty = objScreenOrderDetails.ExciseDuty;
                                            order2.MRPPrice = objScreenOrderDetails.MRPPrice;
                                            order2.Discount = objScreenOrderDetails.Discount;
                                            order2.MenuItemId = objScreenOrderDetails.ItemId;
                                            order2.TicketId = ticket2.Id;
                                            order2.OrderNumber = 0;
                                            order2.FinancialYear = value.FinancialYear;
                                            order2.CreatingUserName = value.UserName;
                                            order2.NVDate = date;
                                            order2.NepaliMonth = nepaliMonth;
                                            order2.BranchId = objScreenOrderDetails.BranchId;
                                            order2.CompanyCode = objScreenOrderDetails.CompanyCode;
                                            order2.DepartmentId = objScreenOrderDetails.DepartmentId;
                                            order2.WarehouseId = objScreenOrderDetails.WarehouseId;
                                            OrderRepository.Update(order2);
                                            OrderRepository.Save();
                                        }
                                    }
                                    else if (objScreenOrderDetails.Id == 0 && objScreenOrderDetails.ItemId != 0)
                                    {
                                        order1.OrderDescription = objScreenOrderDetails.OrderDescription;
                                        order1.AccountTransactionTypeId = 3;
                                        order1.CreatedDateTime = DateTime.Now;
                                        order1.DecreaseInventory = true;
                                        order1.DepartmentId = objScreenOrderDetails.DepartmentId;
                                        order1.IncreaseInventory = false;
                                        order1.IS_Bill_Active = false;
                                        order1.IS_Bill_Printed = true;
                                        order1.Locked = true;
                                        order1.MenuItemId = objScreenOrderDetails.ItemId;
                                        order1.OrderNumber = 0;
                                        order1.OrderStates = "Closed";
                                        order1.Price = objScreenOrderDetails.UnitPrice;
                                        order1.TaxRate = objScreenOrderDetails.TaxRate;
                                        order1.ExciseDuty = objScreenOrderDetails.ExciseDuty;
                                        order1.MRPPrice = objScreenOrderDetails.MRPPrice;
                                        order1.Discount = objScreenOrderDetails.Discount;
                                        order1.Quantity = objScreenOrderDetails.Qty;
                                        order1.Real_Time = false;
                                        order1.Sync_With_IRD = false;
                                        order1.TicketId = ticket2.Id;
                                        order1.WarehouseId = objScreenOrderDetails.WarehouseId;
                                        order1.Printed_Time = DateTime.Now;
                                        order1.FinancialYear = value.FinancialYear;
                                        order1.NVDate = date;
                                        order1.NepaliMonth = nepaliMonth;
                                        order1.CreatingUserName = value.UserName;
                                        order1.BranchId = objScreenOrderDetails.BranchId;
                                        order1.CompanyCode = objScreenOrderDetails.CompanyCode;
                                        order1.DepartmentId = objScreenOrderDetails.DepartmentId;
                                        order1.WarehouseId = objScreenOrderDetails.WarehouseId;
                                        OrderRepository.Insert(order1);
                                        OrderRepository.Save();
                                    }
                                }
                                num = 1;
                            }
                            unitOfWork.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.ErrorLogging(ex);
                            num = 0;
                        }
                    }
                }
                else
                    num = 0;
            }
            else
                num = 0;
            return num;
        }

        public static int Delete(
          IDCubeRepository<AccountTransaction> accRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          IDCubeRepository<AccountTransactionDocument> accTransDocRepository,
          IDCubeRepository<Ticket> TicketRepository,
          AccountTransaction value)
        {
            int num = 1;
            AccountTransaction accountTransaction1 = new AccountTransaction();
            List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
            List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                unitOfWork.StartTransaction();
                try
                {
                    AccountTransaction accountTransaction2 = accRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.AccountTransactionDocumentId == value.AccountTransactionDocumentId)).FirstOrDefault<AccountTransaction>();
                    if (accountTransaction2 != null)
                    {
                        accTransDocRepository.Delete((object)value.AccountTransactionDocumentId);
                        accTransDocRepository.Save();
                        foreach (AccountTransaction accountTransaction3 in accRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.AccountTransactionDocumentId == value.AccountTransactionDocumentId)).ToList<AccountTransaction>())
                        {
                            accRepository.Delete((object)accountTransaction3.Id);
                            accRepository.Save();
                        }
                        foreach (AccountTransactionValue transactionValue in accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionDocumentId == value.AccountTransactionDocumentId)).ToList<AccountTransactionValue>())
                        {
                            accValueRepository.Delete((object)transactionValue.Id);
                            accValueRepository.Save();
                        }
                        Ticket ticket1 = new Ticket();
                        string TicketNumber = accountTransaction2.Name.Substring(accountTransaction2.Name.IndexOf("[#")).Replace("]", "").Replace("[#", "");
                        Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.TicketNumber == TicketNumber)).FirstOrDefault<Ticket>();
                        if (ticket2 != null)
                        {
                            TicketRepository.Delete((object)ticket2.Id);
                            TicketRepository.Save();
                        }
                        unitOfWork.CommitTransaction();
                    }
                    else
                    {
                        unitOfWork.RollBackTransaction();
                        num = 0;
                    }
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    num = 0;
                }
                return num;
            }
        }

        public static int DeleteOrderDetails(IDCubeRepository<Order> OrderRepository, int Id)
        {
            int num = 1;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    OrderRepository.Delete((object)Id);
                    OrderRepository.Save();
                    num = 1;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    num = 0;
                }
                unitOfWork.CommitTransaction();
            }
            return num;
        }

        public static List<ScreenOrderDetails> CustomerScreenOrderDetail(
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          string CustomerId,
          string InvoiceNo,
          string FinancialYear)
        {
            List<ScreenOrderDetails> screenOrderDetailsList = new List<ScreenOrderDetails>();
            List<Order> orderList = new List<Order>();
            Ticket objTicket = new Ticket();
            objTicket = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Table_Customer_Room.ToString() == CustomerId && o.TicketNumber == InvoiceNo && o.FinancialYear == FinancialYear)).FirstOrDefault<Ticket>();
            if (objTicket != null)
            {
                foreach (Order order in OrderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o => o.TicketId == objTicket.Id)).ToList<Order>())
                {
                    ScreenOrderDetails screenOrderDetails = new ScreenOrderDetails()
                    {
                        DepartmentId = order.DepartmentId,
                        FinancialYear = order.FinancialYear,
                        Id = order.Id,
                        ItemId = order.MenuItemId,
                        ItemName = order.MenuItemName,
                        OrderDescription = order.OrderDescription,
                        OrderId = order.Id,
                        OrderNumber = order.OrderNumber,
                        Qty = order.Quantity,
                        Tags = order.Tag,
                        UnitPrice = order.Price,
                        TaxRate = order.TaxRate,
                        ExciseDuty = order.ExciseDuty,
                        Discount = order.Discount,
                        MRPPrice = order.MRPPrice,
                        TotalAmount = order.Price * order.Quantity,
                        UserId = order.CreatingUserName,
                        BranchId = order.BranchId,
                        CompanyCode = order.CompanyCode
                    };
                    screenOrderDetails.DepartmentId = order.DepartmentId;
                    screenOrderDetails.WarehouseId = order.WarehouseId;
                    screenOrderDetailsList.Add(screenOrderDetails);
                }
            }
            return screenOrderDetailsList;
        }
    }
}
