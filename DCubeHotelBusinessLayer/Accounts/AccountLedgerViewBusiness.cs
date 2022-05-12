using DCubeHotelBusinessLayer.HotelReservationBL;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.Accounts
{
  public static class AccountLedgerViewBusiness
  {
    public static Decimal GetLedgerPreviousBalance(
      IDCubeRepository<Account> AccountRepository,
      IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
      int LedgerId,
      int TransactionId)
    {
      List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
      List<Account> accountList = new List<Account>();
      List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
      List<AccountTransactionValue> list2 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId == LedgerId && o.AccountTransactionId < TransactionId)).ToList<AccountTransactionValue>();
      Account account1 = new Account();
      Account account2 = list1.Find((Predicate<Account>) (o => o.Id == LedgerId));
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      Decimal num3 = 0M;
      Decimal num4 = num2 + list2.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Debit));
      Decimal num5 = num3 + list2.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Credit));
      Decimal ledgerPreviousBalance = num1 + num4 - num5;
      LedgerView ledgerView = new LedgerView();
      if (account2.DRCR == "Dr" || account2.DRCR == " " || account2.DRCR == "")
      {
        ledgerPreviousBalance += account2.Amount;
        ledgerView.Debit = ledgerPreviousBalance;
      }
      if (account2.DRCR == "Cr")
      {
        ledgerPreviousBalance -= account2.Amount;
        ledgerView.Credit = Math.Abs(ledgerPreviousBalance);
      }
      return ledgerPreviousBalance;
    }

    public static List<LedgerView> GetLedgerView(
      IDCubeRepository<Account> AccountRepository,
      IDCubeRepository<AccountTransactionDocument> AccountTransactionDocumentRepository,
      IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
      IDCubeRepository<AccountTransaction> AccountTransactionRepository,
      IDCubeRepository<Ticket> TicketRepository,
      IDCubeRepository<PurchaseDetails> purchaseDetailsRepository,
      IDCubeRepository<Order> orderRepository,
      DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepo,
      DCubeRepository<MenuItem> MenuItemRepo,
      DCubeRepository<MenuItemPortion> Menuportionrepo,
      DCubeRepository<MenuItemPortionPriceRange> MenuportionPriceRangeRepo,
      DCubeRepository<FinancialYear> FinancialYearRepo,
      int LedgerId,
      int BranchId,
      string fromDate,
      string toDate)
    {
      DateTime sFormDate = NepalitoEnglishDate.EnglishDate(fromDate);
      DateTime sToDate = NepalitoEnglishDate.EnglishDate(toDate);
      List<LedgerView> ledgerView1 = new List<LedgerView>();
      List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
      List<AccountTransactionValue> transactionValueList1 = new List<AccountTransactionValue>();
      List<AccountTransactionValue> transactionValueList2 = new List<AccountTransactionValue>();
      List<Account> accountList = new List<Account>();
      List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
      List<FinancialYear> financialYearList = new List<FinancialYear>();
      financialYearList = FinancialYearRepo.GetAllData().ToList<FinancialYear>();
      Account account1 = new Account();
      List<AccountTransactionValue> list2;
      List<AccountTransactionValue> list3;
      Account account2;
      if (BranchId == 0)
      {
        list2 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.Date < sFormDate && o.AccountId == LedgerId)).ToList<AccountTransactionValue>();
        list3 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.Date >= sFormDate && o.Date <= sToDate && o.AccountId == LedgerId)).ToList<AccountTransactionValue>();
        account2 = list1.Find((Predicate<Account>) (o => o.Id == LedgerId));
      }
      else
      {
        list2 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.Date < sFormDate && o.AccountId == LedgerId && o.BranchId == BranchId)).ToList<AccountTransactionValue>();
        list3 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.Date >= sFormDate && o.Date <= sToDate && o.AccountId == LedgerId && o.BranchId == BranchId)).ToList<AccountTransactionValue>();
        account2 = list1.Find((Predicate<Account>) (o => o.Id == LedgerId && o.BranchId == BranchId));
      }
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      Decimal num3 = 0M;
      Decimal num4 = num2 + list2.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Debit));
      Decimal num5 = num3 + list2.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Credit));
      Decimal num6 = num1 + num4 - num5;
      LedgerView ledgerView2 = new LedgerView();
      if (account2 != null)
      {
        if (account2.DRCR == "Dr" || account2.DRCR == " " || account2.DRCR == "")
        {
          num6 += account2.Amount;
          ledgerView2.Debit = num6;
        }
        if (account2.DRCR == "Cr")
        {
          num6 -= account2.Amount;
          ledgerView2.Credit = Math.Abs(num6);
        }
        ledgerView2.Id = account2.Id;
      }
      ledgerView2.Name = "Opening Balance";
      ledgerView2.VDate = "";
      ledgerView2.VNumber = "";
      ledgerView2.VType = "";
      ledgerView2.Balance = num6;
      ledgerView1.Add(ledgerView2);
      List<AccountTransaction> list4 = AccountTransactionRepository.GetAllData().ToList<AccountTransaction>();
      var datas = list4.Join((IEnumerable<AccountTransactionValue>) list3, (Func<AccountTransaction, int>) (a => a.Id), (Func<AccountTransactionValue, int>) (t => t.AccountTransactionId), (a, t) => new
      {
        Id = a.Id,
        Name = a.Name,
        Particular = t.Name,
        NVDate = t.NVDate,
        Credit = t.Credit,
        Debit = t.Debit,
        Date = t.Date
      }).Where(o => o.Credit.ToString() != "0.00" || o.Debit.ToString() != "0.00").OrderBy(o => o.Date);
      List<Ticket> ticketList = new List<Ticket>();
      List<Ticket> list5 = TicketRepository.GetAllData().ToList<Ticket>();
      List<Order> orderList1 = new List<Order>();
      List<Order> list6 = orderRepository.GetAllData().ToList<Order>();
      List<PurchaseDetails> source1 = new List<PurchaseDetails>();
      try
      {
        source1 = purchaseDetailsRepository.GetAllData().ToList<PurchaseDetails>();
      }
      catch (Exception ex)
      {
      }
      List<MenuItemWithPrice> menuItemWithPriceList = new List<MenuItemWithPrice>();
      List<MenuItemWithPrice> menuCategoryItem = MenuBusinessLayer.GetMenuCategoryItem(MenuCategoryRepo, (IDCubeRepository<MenuItem>) MenuItemRepo, (IDCubeRepository<MenuItemPortion>) Menuportionrepo, (IDCubeRepository<MenuItemPortionPriceRange>) MenuportionPriceRangeRepo);
      List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
      List<MenuItemPortion> list7 = Menuportionrepo.GetAllData().ToList<MenuItemPortion>();
      List<MenuItem> menuItemList = new List<MenuItem>();
      List<MenuItem> list8 = MenuItemRepo.GetAllData().ToList<MenuItem>();
      foreach (var data in datas)
      {
        var lat = data;
        List<AccountTransactionValue> source2 = new List<AccountTransactionValue>();
        Decimal num7;
        foreach (AccountTransactionValue transactionValue in list3.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == lat.Id)).ToList<AccountTransactionValue>())
        {
          num7 = transactionValue.Debit;
          int num8;
          if (!(num7.ToString() != "0.00"))
          {
            num7 = transactionValue.Credit;
            num8 = num7.ToString() != "0.00" ? 1 : 0;
          }
          else
            num8 = 1;
          if (num8 != 0)
            source2.Add(transactionValue);
        }
        AccountTransactionValue objListvoucher = source2.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId == LedgerId)).FirstOrDefault<AccountTransactionValue>();
        if (objListvoucher != null)
        {
          LedgerView ledgerView3 = new LedgerView();
          num7 = objListvoucher.Debit;
          int num9;
          if (!(num7.ToString() != "0.00"))
          {
            num7 = objListvoucher.Credit;
            num9 = num7.ToString() != "0.00" ? 1 : 0;
          }
          else
            num9 = 1;
          if (num9 != 0)
          {
            try
            {
              AccountTransaction objAccountTransaction = new AccountTransaction();
              objAccountTransaction = list4.Find((Predicate<AccountTransaction>) (o => o.Id == objListvoucher.AccountTransactionId));
              if (objAccountTransaction.Name.Contains("Sales") || objAccountTransaction.Name.Contains("Credit Note"))
              {
                Ticket objTicket = new Ticket();
                objTicket = list5.Find((Predicate<Ticket>) (o => o.AccountTransactionDocumentId == objAccountTransaction.AccountTransactionDocumentId));
                if (objTicket != null)
                {
                  List<Order> orderList2 = new List<Order>();
                  List<Order> list9 = list6.Where<Order>((Func<Order, bool>) (o => o.TicketId == objTicket.Id)).ToList<Order>();
                  List<ScreenOrderDetails> screenOrderDetailsList = new List<ScreenOrderDetails>();
                  foreach (Order order in list9)
                  {
                    Order objOrder = order;
                    try
                    {
                      MenuItemWithPrice menuItemWithPrice = menuCategoryItem.Where<MenuItemWithPrice>((Func<MenuItemWithPrice, bool>) (o => o.ItemId == objOrder.MenuItemId)).FirstOrDefault<MenuItemWithPrice>();
                      MenuItemPortion objMenuItemPortion = new MenuItemPortion();
                      MenuItem menuItem = new MenuItem();
                      if (menuItemWithPrice != null)
                      {
                        objMenuItemPortion = list7.Find((Predicate<MenuItemPortion>) (o => o.Id == objOrder.MenuItemId));
                        if (objMenuItemPortion != null)
                          menuItem = list8.Find((Predicate<MenuItem>) (o => o.Id == objMenuItemPortion.MenuItemPortionId));
                      }
                      screenOrderDetailsList.Add(new ScreenOrderDetails()
                      {
                        DepartmentId = menuItemWithPrice.DepartmentId,
                        ItemId = menuItemWithPrice.ItemId,
                        ItemName = menuItemWithPrice.Name,
                        Qty = objOrder.Quantity,
                        UnitType = menuItem.UnitType,
                        UnitPrice = objOrder.Price,
                        TotalAmount = objOrder.Quantity * objOrder.Price
                      });
                    }
                    catch (Exception ex)
                    {
                      string message = ex.Message;
                    }
                  }
                  ledgerView3.SalesOrderDetails = screenOrderDetailsList;
                }
              }
              if (objAccountTransaction.Name.Contains("Purchase") || objAccountTransaction.Name.Contains("Debit Note"))
              {
                List<PurchaseDetails> purchaseDetailsList = new List<PurchaseDetails>();
                List<PurchaseDetails> list10 = source1.Where<PurchaseDetails>((Func<PurchaseDetails, bool>) (o => o.AccountTransactionId == objAccountTransaction.Id)).ToList<PurchaseDetails>();
                ledgerView3.PurchaseDetails = list10;
              }
              num6 -= lat.Credit;
              ledgerView3.Credit = lat.Credit;
              num6 += lat.Debit;
              ledgerView3.Debit = lat.Debit;
              ledgerView3.Id = lat.Id;
              try
              {
                ledgerView3.Name = lat.Name.Substring(0, lat.Name.IndexOf("["));
              }
              catch
              {
              }
              ledgerView3.VDate = lat.NVDate;
              try
              {
                ledgerView3.VNumber = lat.Name.Substring(lat.Name.IndexOf("[")).Replace("#", "");
              }
              catch
              {
              }
              ledgerView3.VType = lat.Name.Substring(0, lat.Name.IndexOf("["));
              ledgerView3.Balance = num6;
              ledgerView1.Add(ledgerView3);
            }
            catch (Exception ex)
            {
              string message = ex.Message;
            }
          }
        }
      }
      return ledgerView1;
    }

    public static List<LedgerView> GetDayBook(
      IDCubeRepository<Account> AccountRepository,
      IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
      IDCubeRepository<AccountTransaction> AccountTransactionRepository,
      string FinancialYear)
    {
      List<LedgerView> dayBook = new List<LedgerView>();
      List<AccountTransactionValue> transactionValueList1 = new List<AccountTransactionValue>();
      List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
      List<AccountTransactionValue> transactionValueList2 = new List<AccountTransactionValue>();
      List<Account> accountList = new List<Account>();
      List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
      List<AccountTransactionValue> list2 = AccountTransactionValueRepository.GetAllData().ToList<AccountTransactionValue>();
      foreach (var data in AccountTransactionRepository.GetAllData().ToList<AccountTransaction>().Join((IEnumerable<AccountTransactionValue>) list2, (Func<AccountTransaction, int>) (a => a.Id), (Func<AccountTransactionValue, int>) (t => t.AccountTransactionId), (a, t) => new
      {
        Id = a.Id,
        Name = a.Name,
        Particular = t.Name,
        NVDate = t.NVDate,
        Credit = t.Credit,
        Debit = t.Debit,
        AccountId = t.AccountId
      }))
      {
        var lat = data;
        try
        {
          if (lat.Debit.ToString() != "0.00" || lat.Credit.ToString() != "0.00")
            dayBook.Add(new LedgerView()
            {
              Credit = lat.Credit,
              Debit = lat.Debit,
              Id = lat.Id,
              Name = list1.Where<Account>((Func<Account, bool>) (o => o.Id == lat.AccountId)).FirstOrDefault<Account>().Name,
              VDate = lat.NVDate,
              VNumber = lat.Name.Substring(lat.Name.IndexOf("[")).Replace("#", ""),
              VType = lat.Name.Substring(0, lat.Name.IndexOf("["))
            });
        }
        catch
        {
        }
      }
      return dayBook;
    }

    public static List<LedgerView> GetLedgerStatement(
      IDCubeRepository<Account> AccountRepository,
      IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository)
    {
      List<LedgerView> ledgerStatement = new List<LedgerView>();
      List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
      List<Account> accountList = new List<Account>();
      foreach (var data in AccountRepository.GetAllData().Where<Account>((Func<Account, bool>) (o => o.AccountTypeId == 18)).ToList<Account>().GroupJoin(AccountTransactionValueRepository.GetAllData().ToList<AccountTransactionValue>().GroupBy<AccountTransactionValue, int>((Func<AccountTransactionValue, int>) (e => e.AccountId)).Select(g => new
      {
        AccountId = g.Key,
        Debit = g.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (x => x.Debit)),
        Credit = g.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (x => x.Credit))
      }), (Func<Account, int>) (acc => acc.Id), bal => bal.AccountId, (acc, bal) => new
      {
        acc = acc,
        bal = bal
      }).SelectMany(x => x.bal.DefaultIfEmpty(), (la, lb) => new
      {
        Id = la.acc.Id,
        Name = la.acc.Name,
        OpeningBalance = la.acc.OpeningBalance,
        Debit = lb == null ? 0M : lb.Debit,
        Credit = lb == null ? 0M : lb.Credit
      }))
        ledgerStatement.Add(new LedgerView()
        {
          Balance = data.Credit + data.Debit + data.OpeningBalance,
          Credit = data.Credit,
          Debit = data.Debit,
          Id = data.Id,
          Name = data.Name,
          OpeningBalance = data.OpeningBalance
        });
      return ledgerStatement;
    }
  }
}
