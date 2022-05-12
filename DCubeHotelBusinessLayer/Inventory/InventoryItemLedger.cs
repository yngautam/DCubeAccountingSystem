using DCubeHotelBusinessLayer.ExtraModel;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.Inventory
{
    public static class InventoryItemLedger
    {
        public static List<ViewInventoryItemLedger> ListInventoryItemLedger(
          IDCubeRepository<FinancialYear> FinancialYearRepo,
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransaction> AccountTransactionRepository,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<PurchaseDetails> PurchaseDetailRepository,
          DCubeRepository<MenuItemPortion> Menuportionrepo,
          DCubeRepository<MenuItem> MenuItemRepo,
          string ItemId,
          string financialyear)
        {
            List<FinancialYear> financialYearList = new List<FinancialYear>();
            List<FinancialYear> list1 = FinancialYearRepo.GetAllData().ToList<FinancialYear>();
            FinancialYear objFinancialYear = new FinancialYear();
            objFinancialYear = list1.Where<FinancialYear>((Func<FinancialYear, bool>)(o => o.Name == financialyear)).FirstOrDefault<FinancialYear>();
            List<ViewInventoryItemLedger> inventoryItemLedgerList = new List<ViewInventoryItemLedger>();
            Decimal num = 0M;
            List<AccountTransaction> accountTransactionList1 = new List<AccountTransaction>();
            List<AccountTransaction> list2 = AccountTransactionRepository.GetAllData().ToList<AccountTransaction>();
            List<AccountTransaction> accountTransactionList2 = new List<AccountTransaction>();
            List<Account> accountList = new List<Account>();
            List<Account> list3 = AccountRepository.GetAllData().ToList<Account>();
            List<AccountTransaction> list4 = list2.Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Name.Contains("Sales") || o.Name.Contains("Credit Note") || o.Name.Contains("Purchase") || o.Name.Contains("Debit Note"))).ToList<AccountTransaction>();
            if (objFinancialYear != null)
                accountTransactionList2 = list4.Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => DateTime.Parse(o.Date) > objFinancialYear.StartDate)).ToList<AccountTransaction>();
            List<AccountTransaction> list5 = list4.Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.FinancialYear == financialyear)).ToList<AccountTransaction>().OrderBy<AccountTransaction, string>((Func<AccountTransaction, string>)(o => o.Date)).ToList<AccountTransaction>();
            MenuItemPortion objMenuItemPortion = new MenuItemPortion();
            objMenuItemPortion = Menuportionrepo.GetAllData().Where<MenuItemPortion>((Func<MenuItemPortion, bool>)(o => o.Id.ToString() == ItemId)).FirstOrDefault<MenuItemPortion>();
            MenuItem menuItem1 = new MenuItem();
            MenuItem menuItem2 = MenuItemRepo.GetAllData().Where<MenuItem>((Func<MenuItem, bool>)(o => o.Id == objMenuItemPortion.MenuItemPortionId)).FirstOrDefault<MenuItem>();
            if (objMenuItemPortion != null)
            {
                num = objMenuItemPortion.OpeningStock - num;
                inventoryItemLedgerList.Add(new ViewInventoryItemLedger()
                {
                    Amount = objMenuItemPortion.OpeningStock * objMenuItemPortion.OpeningStockRate,
                    QtyBalance = num,
                    PartyName = "Opening Balance",
                    QtyIn = objMenuItemPortion.OpeningStock,
                    QtyOut = 0M,
                    Rate = objMenuItemPortion.OpeningStockRate,
                    TDate = "",
                    UnitType = menuItem2.UnitType
                });
            }
            List<Ticket> ticketList1 = new List<Ticket>();
            List<Ticket> ticketList2 = new List<Ticket>();
            List<Order> orderList1 = new List<Order>();
            List<Ticket> list6 = TicketRepository.GetAllData().ToList<Ticket>();
            if (objFinancialYear != null)
                ticketList2 = list6.Where<Ticket>((Func<Ticket, bool>)(o => o.Date > objFinancialYear.StartDate)).ToList<Ticket>().ToList<Ticket>();
            List<Ticket> list7 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.FinancialYear == financialyear)).ToList<Ticket>().ToList<Ticket>();
            List<Order> orderList2 = new List<Order>();
            Order objFirstOrder = new Order();
            Order objLastOrder = new Order();
            List<Order> orderList3 = new List<Order>();
            List<Order> list8 = OrderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o => o.MenuItemId.ToString() == ItemId)).ToList<Order>();
            objFirstOrder = list8.Where<Order>((Func<Order, bool>)(o => o.MenuItemId.ToString() == ItemId)).Where<Order>((Func<Order, bool>)(o => o.FinancialYear == financialyear)).FirstOrDefault<Order>();
            if (objFirstOrder != null)
                orderList3 = orderList2.Where<Order>((Func<Order, bool>)(o => o.Id > objFirstOrder.Id)).ToList<Order>();
            objLastOrder = OrderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o => o.MenuItemId.ToString() == ItemId)).Where<Order>((Func<Order, bool>)(o => o.FinancialYear == financialyear)).LastOrDefault<Order>();
            if (objFirstOrder != null && objLastOrder != null)
                orderList2 = list8.Where<Order>((Func<Order, bool>)(o => o.Id >= objFirstOrder.Id && o.Id <= objLastOrder.Id)).ToList<Order>();
            List<PurchaseDetails> purchaseDetailsList = new List<PurchaseDetails>();
            List<PurchaseDetails> list9 = PurchaseDetailRepository.GetAllData().Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.InventoryItemId.ToString() == ItemId)).Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.FinancialYear == financialyear)).ToList<PurchaseDetails>();
            var datas1 = list7.Join((IEnumerable<Order>)orderList2, (Func<Ticket, int>)(t => t.Id), (Func<Order, int>)(o => o.TicketId), (t, o) => new
            {
                t = t,
                o = o
            }).Join((IEnumerable<AccountTransaction>)list5, _param1 => _param1.t.AccountTransactionDocumentId, (Func<AccountTransaction, int>)(a => a.AccountTransactionDocumentId), (_param1, a) => new
            {
                ItemId = _param1.o.MenuItemId,
                Quantity = _param1.o.Quantity,
                Price = _param1.o.Price,
                TDate = _param1.t.Date,
                NVDate = _param1.t.NVDate,
                CustomerId = a.SourceAccountTypeId,
                UnitType = _param1.o.UnitType,
                Name = _param1.t.Name,
                Id = a.Id
            });
            var datas2 = list5.Join((IEnumerable<PurchaseDetails>)list9, (Func<AccountTransaction, int>)(at => at.Id), (Func<PurchaseDetails, int>)(p => p.AccountTransactionId), (at, p) => new
            {
                ItemId = p.InventoryItemId,
                Quantity = p.Quantity,
                Price = p.PurchaseRate,
                TDate = DateTime.Parse(at.Date),
                NVDate = p.NVDate,
                CustomerId = at.SourceAccountTypeId,
                UnitType = p.UnitType,
                Name = at.Name,
                Id = at.Id
            });
            List<ItemLedger> source = new List<ItemLedger>();
            foreach (var data in datas1)
                source.Add(new ItemLedger()
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    ItemId = data.ItemId,
                    Name = data.Name,
                    NVDate = data.NVDate,
                    Price = data.Price,
                    Quantity = data.Quantity,
                    TDate = data.TDate,
                    UnitType = data.UnitType
                });
            foreach (var data in datas2)
                source.Add(new ItemLedger()
                {
                    Id = data.Id,
                    CustomerId = data.CustomerId,
                    ItemId = data.ItemId,
                    Name = data.Name,
                    NVDate = data.NVDate,
                    Price = data.Price,
                    Quantity = data.Quantity,
                    TDate = data.TDate,
                    UnitType = data.UnitType
                });
            foreach (ItemLedger itemLedger in source.OrderBy<ItemLedger, int>((Func<ItemLedger, int>)(o => o.Id)).ToList<ItemLedger>())
            {
                ItemLedger oo = itemLedger;
                string str = string.Empty;
                Account account1 = new Account();
                Account account2 = list3.Find((Predicate<Account>)(o => o.Id == oo.CustomerId));
                if (account2 != null)
                    str = account2.Name;
                ViewInventoryItemLedger inventoryItemLedger = new ViewInventoryItemLedger();
                inventoryItemLedger.Amount = Math.Abs(oo.Quantity) * oo.Price;
                if (oo.Name.Contains("Sales"))
                {
                    num -= oo.Quantity;
                    inventoryItemLedger.QtyIn = 0M;
                    inventoryItemLedger.QtyOut = Math.Abs(oo.Quantity);
                }
                if (oo.Name.Contains("Credit Note"))
                {
                    str += "(Sales Return)";
                    num += Math.Abs(oo.Quantity);
                    inventoryItemLedger.QtyIn = Math.Abs(oo.Quantity);
                    inventoryItemLedger.QtyOut = 0M;
                }
                if (oo.Name.Contains("Debit Note"))
                {
                    str += "(Purchase Return)";
                    num -= Math.Abs(oo.Quantity);
                    inventoryItemLedger.QtyIn = 0M;
                    inventoryItemLedger.QtyOut = Math.Abs(oo.Quantity);
                }
                if (oo.Name.Contains("Purchase"))
                {
                    num += oo.Quantity;
                    inventoryItemLedger.QtyIn = Math.Abs(oo.Quantity);
                    inventoryItemLedger.QtyOut = 0M;
                }
                inventoryItemLedger.PartyName = str;
                inventoryItemLedger.QtyBalance = num;
                inventoryItemLedger.Rate = oo.Price;
                inventoryItemLedger.TDate = oo.NVDate;
                inventoryItemLedger.UnitType = oo.UnitType;
                inventoryItemLedgerList.Add(inventoryItemLedger);
            }
            return inventoryItemLedgerList;
        }
    }
}