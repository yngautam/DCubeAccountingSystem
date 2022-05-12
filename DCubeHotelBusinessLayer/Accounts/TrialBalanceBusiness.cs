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

namespace DCubeHotelBusinessLayer.Accounts
{
    public static class TrialBalanceBusiness
    {
        public static List<TrialBalance> GetTrialBalance(
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          DCubeRepository<FinancialYear> FinancialYearRepo,
          string FinancialYear)
        {
            List<TrialBalance> trialBalanceList = new List<TrialBalance>();
            return TrialBalanceBusiness.CurrentFinancialYear(AccountTypeRepository, AccountRepository, accValueRepository, FinancialYearRepo, FinancialYear);
        }

        public static List<ProfitAndLoss> GetTotalIncomeExpense(
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          DCubeRepository<FinancialYear> FinancialYearRepo,
          string FinancialYear)
        {
            List<ProfitAndLoss> source = new List<ProfitAndLoss>();
            List<Account> accountList = new List<Account>();
            List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
            List<AccountType> accountTypeList = new List<AccountType>();
            List<AccountType> list2 = AccountTypeRepository.GetAllData().ToList<AccountType>().Where<AccountType>((Func<AccountType, bool>)(o => o.NatureofGroup == "Income" || o.NatureofGroup == "Expenses")).ToList<AccountType>();
            var inner = list1.Join((IEnumerable<AccountType>)list2, (Func<Account, int>)(a => a.AccountTypeId), (Func<AccountType, int>)(at => at.Id), (a, at) => new
            {
                Id = a.Id,
                Name = a.Name,
                NatureofGroup = at.NatureofGroup
            });
            List<TrialBalance> trialBalanceList = new List<TrialBalance>();
            var datas = TrialBalanceBusiness.CurrentFinancialYear(AccountTypeRepository, AccountRepository, accValueRepository, FinancialYearRepo, FinancialYear).Join(inner, (Func<TrialBalance, int>)(l => l.Id), a => a.Id, (l, a) => new
            {
                AccountId = l.Id,
                Credit = l.Credit,
                Debit = l.Debit,
                Name = a.Name,
                NatureofGroup = a.NatureofGroup
            }).GroupBy(pl => pl.NatureofGroup).Select(plGroup => new
            {
                NatureofGroup = plGroup.Key,
                Credit = plGroup.Sum(x => x.Credit),
                Debit = plGroup.Sum(x => x.Debit)
            });
            Decimal num1 = 0M;
            Decimal num2 = 0M;
            foreach (var data in datas)
            {
                ProfitAndLoss profitAndLoss = new ProfitAndLoss();
                if (data.NatureofGroup == "Expenses")
                {
                    Decimal num3 = data.Debit - data.Credit;
                    profitAndLoss.Name = data.NatureofGroup;
                    profitAndLoss.Amount = Math.Abs(num3);
                    profitAndLoss.SortOrder = 1;
                    profitAndLoss.NatureofGroup = "Expenses";
                    num1 += num3;
                    profitAndLoss.Bold = "No";
                    source.Add(profitAndLoss);
                }
                if (data.NatureofGroup == "Income")
                {
                    Decimal num4 = data.Credit - data.Debit;
                    profitAndLoss.Name = data.NatureofGroup;
                    profitAndLoss.Amount = Math.Abs(num4);
                    profitAndLoss.SortOrder = 4;
                    profitAndLoss.NatureofGroup = "Income";
                    num2 += num4;
                    profitAndLoss.Bold = "No";
                    source.Add(profitAndLoss);
                }
            }
            source.Add(new ProfitAndLoss()
            {
                Name = "Expenses",
                Amount = 0M,
                SortOrder = 0,
                Bold = "Yes"
            });
            source.Add(new ProfitAndLoss()
            {
                Name = "Total",
                Amount = num1,
                SortOrder = 2,
                Bold = "Yes"
            });
            source.Add(new ProfitAndLoss()
            {
                Name = "Total",
                Amount = num2,
                SortOrder = 5,
                Bold = "Yes"
            });
            source.Add(new ProfitAndLoss()
            {
                Name = "Income",
                Amount = 0M,
                SortOrder = 3,
                Bold = "Yes"
            });
            source.Add(new ProfitAndLoss()
            {
                Name = "Net Profit / Loss",
                Amount = num2 - num1,
                SortOrder = 6,
                Bold = "Yes"
            });
            return source.OrderBy<ProfitAndLoss, int>((Func<ProfitAndLoss, int>)(o => o.SortOrder)).ToList<ProfitAndLoss>();
        }

        public static List<ProfitAndLoss> GetProfitLoss(
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          DCubeRepository<FinancialYear> FinancialYearRepo,
          string FinancialYear)
        {
            List<ProfitAndLoss> source = new List<ProfitAndLoss>();
            List<Account> accountList = new List<Account>();
            List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
            List<AccountType> accountTypeList = new List<AccountType>();
            List<AccountType> list2 = AccountTypeRepository.GetAllData().ToList<AccountType>().Where<AccountType>((Func<AccountType, bool>)(o => o.NatureofGroup == "Income" || o.NatureofGroup == "Expenses")).ToList<AccountType>();
            var inner = list1.Join((IEnumerable<AccountType>)list2, (Func<Account, int>)(a => a.AccountTypeId), (Func<AccountType, int>)(at => at.Id), (a, at) => new
            {
                Id = a.Id,
                Name = a.Name,
                NatureofGroup = at.NatureofGroup
            });
            List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
            var datas = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.FinancialYear == FinancialYear)).ToList<AccountTransactionValue>().Select(x => new
            {
                AccountTypeId = x.AccountTypeId,
                AccountId = x.AccountId,
                Debit = x.Debit,
                Credit = x.Credit
            }).GroupBy(s => new { AccountId = s.AccountId }).Select(g => new
            {
                AccountId = g.Key.AccountId,
                AccountTypeId = g.FirstOrDefault().AccountTypeId,
                Debit = g.Sum(x => Math.Round(Convert.ToDecimal(x.Debit), 2)),
                Credit = g.Sum(x => Math.Round(Convert.ToDecimal(x.Credit), 2))
            }).Join(inner, l => l.AccountId, a => a.Id, (l, a) => new
            {
                AccountId = l.AccountId,
                Credit = l.Credit,
                Debit = l.Debit,
                Name = a.Name,
                NatureofGroup = a.NatureofGroup
            });
            Decimal num1 = 0M;
            Decimal num2 = 0M;
            foreach (var data in datas)
            {
                ProfitAndLoss profitAndLoss = new ProfitAndLoss();
                if (data.NatureofGroup == "Expenses")
                {
                    Decimal num3 = data.Debit - data.Credit;
                    profitAndLoss.Name = data.Name;
                    profitAndLoss.Amount = Math.Abs(num3);
                    profitAndLoss.SortOrder = 1;
                    profitAndLoss.NatureofGroup = "Expenses";
                    num1 += Math.Abs(num3);
                    profitAndLoss.Bold = "No";
                    source.Add(profitAndLoss);
                }
                if (data.NatureofGroup == "Income")
                {
                    Decimal num4 = data.Credit - data.Debit;
                    profitAndLoss.Name = data.Name;
                    profitAndLoss.Amount = Math.Abs(num4);
                    profitAndLoss.SortOrder = 4;
                    profitAndLoss.NatureofGroup = "Income";
                    num2 += num4;
                    profitAndLoss.Bold = "No";
                    source.Add(profitAndLoss);
                }
            }
            source.Add(new ProfitAndLoss()
            {
                Name = "Expenses",
                Amount = 0M,
                SortOrder = 0,
                Bold = "Yes"
            });
            source.Add(new ProfitAndLoss()
            {
                Name = "Total",
                Amount = num1,
                SortOrder = 2,
                Bold = "Yes"
            });
            source.Add(new ProfitAndLoss()
            {
                Name = "Total",
                Amount = num2,
                SortOrder = 5,
                Bold = "Yes"
            });
            source.Add(new ProfitAndLoss()
            {
                Name = "Income",
                Amount = 0M,
                SortOrder = 3,
                Bold = "Yes"
            });
            source.Add(new ProfitAndLoss()
            {
                Name = "Net Profit / Loss",
                Amount = num2 - num1,
                SortOrder = 6,
                Bold = "Yes"
            });
            return source.OrderBy<ProfitAndLoss, int>((Func<ProfitAndLoss, int>)(o => o.SortOrder)).ToList<ProfitAndLoss>();
        }

        public static List<ProfitAndLoss> GetProfitLoss(
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransaction> AccountTransactionRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          IDCubeRepository<MenuItemPortion> MenuItemPortionRepository,
          IDCubeRepository<PurchaseDetails> PurchaseDetailsRepository,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          string fromDate,
          string toDate,
          int BranchId)
        {
            DateTime sFormDate = NepalitoEnglishDate.EnglishDate(fromDate);
            DateTime sToDate = NepalitoEnglishDate.EnglishDate(toDate);
            sToDate = sToDate.AddHours(23);
            sToDate = sToDate.AddMinutes(59);
            sToDate = sToDate.AddSeconds(59);

            string OpeningDate = "2018.01.10";

            List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
            List<MenuItemPortion> list1 = MenuItemPortionRepository.GetAllData().ToList<MenuItemPortion>();
            list1.Select(o => new
            {
                InventoryItemId = o.Id,
                Qty = o.OpeningStock
            });
            List<ProfitAndLoss> source1 = new List<ProfitAndLoss>();
            List<Account> accountList = new List<Account>();
            List<Account> list2 = AccountRepository.GetAllData().ToList<Account>();
            List<Ticket> ticketList1 = new List<Ticket>();
            List<Ticket> list3 = TicketRepository.GetAllData().ToList<Ticket>();
            if (BranchId != 0)
                list3 = list3.Where<Ticket>((Func<Ticket, bool>)(o => o.BranchCode == BranchId)).ToList<Ticket>();
            List<AccountTransaction> accountTransactionList1 = new List<AccountTransaction>();
            List<AccountTransaction> list4 = AccountTransactionRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => Convert.ToDateTime(o.Date) <= sToDate)).ToList<AccountTransaction>();
            if (BranchId != 0)
                list4 = list4.Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.BranchId == BranchId)).ToList<AccountTransaction>();
            List<AccountTransaction> accountTransactionList2 = new List<AccountTransaction>();
            List<AccountTransaction> list5 = list4.Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => Convert.ToDateTime(o.Date) <= sFormDate)).ToList<AccountTransaction>();
            List<AccountTransaction> accountTransactionList3 = new List<AccountTransaction>();
            List<AccountTransaction> list6 = list4.Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => Convert.ToDateTime(o.Date) <= sToDate)).ToList<AccountTransaction>();
            List<Ticket> ticketList2 = new List<Ticket>();
            List<Ticket> list7 = list3.Where<Ticket>((Func<Ticket, bool>)(o => Convert.ToDateTime(o.Date) <= sFormDate)).ToList<Ticket>();
            List<Ticket> ticketList3 = new List<Ticket>();
            List<Ticket> list8 = list3.Where<Ticket>((Func<Ticket, bool>)(o => Convert.ToDateTime(o.Date) <= sToDate)).ToList<Ticket>();
            List<Order> orderList = new List<Order>();
            List<Order> list9 = OrderRepository.GetAllData().ToList<Order>();
            var source2 = list7.Join((IEnumerable<Order>)list9, (Func<Ticket, int>)(t => t.Id), (Func<Order, int>)(o => o.TicketId), (t, o) => new
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
                UnitType = _param1.o.UnitType,
                Name = _param1.t.Name,
                ref_invoice_number = a.ref_invoice_number,
                BillNo = _param1.t.TicketNumber,
                Id = a.Id
            });
            var source3 = list8.Join((IEnumerable<Order>)list9, (Func<Ticket, int>)(t => t.Id), (Func<Order, int>)(o => o.TicketId), (t, o) => new
            {
                t = t,
                o = o
            }).Join((IEnumerable<AccountTransaction>)list4, _param1 => _param1.t.AccountTransactionDocumentId, (Func<AccountTransaction, int>)(a => a.AccountTransactionDocumentId), (_param1, a) => new
            {
                ItemId = _param1.o.MenuItemId,
                Quantity = _param1.o.Quantity,
                Price = _param1.o.Price,
                TDate = _param1.t.Date,
                NVDate = _param1.t.NVDate,
                UnitType = _param1.o.UnitType,
                Name = _param1.t.Name,
                ref_invoice_number = a.ref_invoice_number,
                BillNo = _param1.t.TicketNumber,
                Id = a.Id
            });
            List<PurchaseDetails> purchaseDetailsList = new List<PurchaseDetails>();
            List<PurchaseDetails> list10 = PurchaseDetailsRepository.GetAllData().ToList<PurchaseDetails>();
            var source4 = list5.Join((IEnumerable<PurchaseDetails>)list10, (Func<AccountTransaction, int>)(at => at.Id), (Func<PurchaseDetails, int>)(p => p.AccountTransactionId), (at, p) => new
            {
                ItemId = p.InventoryItemId,
                Quantity = p.Quantity,
                Price = p.PurchaseRate,
                TDate = DateTime.Parse(at.Date),
                NVDate = p.NVDate,
                CustomerId = at.SourceAccountTypeId,
                UnitType = p.UnitType,
                Name = at.Name,
                ref_invoice_number = at.ref_invoice_number,
                BillNo = at.ref_invoice_number,
                Id = at.Id
            });
            var source5 = list6.Join((IEnumerable<PurchaseDetails>)list10, (Func<AccountTransaction, int>)(at => at.Id), (Func<PurchaseDetails, int>)(p => p.AccountTransactionId), (at, p) => new
            {
                ItemId = p.InventoryItemId,
                Quantity = p.Quantity,
                Price = p.PurchaseRate,
                TDate = DateTime.Parse(at.Date),
                NVDate = p.NVDate,
                CustomerId = at.SourceAccountTypeId,
                UnitType = p.UnitType,
                Name = at.Name,
                ref_invoice_number = at.ref_invoice_number,
                BillNo = at.ref_invoice_number,
                Id = at.Id
            });
            List<AccountType> accountTypeList = new List<AccountType>();
            List<AccountType> list11 = AccountTypeRepository.GetAllData().ToList<AccountType>().Where<AccountType>((Func<AccountType, bool>)(o => o.NatureofGroup == "Income" || o.NatureofGroup == "Expenses")).ToList<AccountType>();
            List<PeriodicConsumption> periodicConsumptionList = new List<PeriodicConsumption>();
            List<PeriodicConsumptionItem> periodicConsumptionItemList = new List<PeriodicConsumptionItem>();
            Decimal num1 = 0M;
            Decimal num2 = 0M;
            foreach (MenuItemPortion menuItemPortion in list1)
            {
                MenuItemPortion currentitem = menuItemPortion;
                List<ViewInventoryItemLedger> source6 = new List<ViewInventoryItemLedger>();
                Decimal num3 = 0M;
                Decimal openingStockRate = currentitem.OpeningStockRate;
                var datas1 = source3.Where(o => o.ItemId == currentitem.Id);
                var datas2 = source5.Where(o => o.ItemId == currentitem.Id);
                source4.Where(o => o.ItemId == currentitem.Id).Sum(o => o.Quantity);
                source2.Where(o => o.ItemId == currentitem.Id).Sum(o => o.Quantity);
                DateTime dateTime = sFormDate;
                List<ItemLedger> source7 = new List<ItemLedger>();
                source7.Add(new ItemLedger()
                {
                    ItemId = currentitem.Id,
                    Name = "Opening",
                    NVDate = fromDate,
                    Price = currentitem.OpeningStockRate,
                    Quantity = currentitem.OpeningStock,
                    TDate = DateTime.Parse(OpeningDate),
                    UnitType = "",
                    RefNumber = "0",
                    BillNumber = "0"
                });
                foreach (var data in datas1)
                    source7.Add(new ItemLedger()
                    {
                        ItemId = data.ItemId,
                        Name = data.Name,
                        NVDate = data.NVDate,
                        Price = data.Price,
                        Quantity = data.Quantity,
                        TDate = data.TDate,
                        UnitType = data.UnitType,
                        RefNumber = data.ref_invoice_number,
                        BillNumber = data.BillNo.ToString()
                    });
                foreach (var data in datas2)
                    source7.Add(new ItemLedger()
                    {
                        ItemId = data.ItemId,
                        Name = data.Name,
                        NVDate = data.NVDate,
                        Price = data.Price,
                        Quantity = data.Quantity,
                        TDate = data.TDate,
                        UnitType = data.UnitType,
                        RefNumber = data.ref_invoice_number,
                        BillNumber = data.BillNo
                    });
                List<ItemLedger> list12 = source7.OrderBy<ItemLedger, int>((Func<ItemLedger, int>)(o => o.Id)).ToList<ItemLedger>();
                Decimal num4 = currentitem.OpeningStockRate;
                Decimal num5 = currentitem.OpeningStock;
                int num6 = 1;
                foreach (ItemLedger itemLedger in list12)
                {
                    ItemLedger oo = itemLedger;
                    string str = string.Empty;
                    Account account1 = new Account();
                    Account account2 = list2.Find((Predicate<Account>)(o => o.Id == oo.CustomerId));
                    if (account2 != null)
                        str = account2.Name;
                    ViewInventoryItemLedger inventoryItemLedger = new ViewInventoryItemLedger();
                    inventoryItemLedger.Id = num6;
                    inventoryItemLedger.Amount = Math.Abs(oo.Quantity) * oo.Price;
                    inventoryItemLedger.PartyName = str;
                    if (oo.Name.Contains("Opening"))
                    {
                        num3 += Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyIn = Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyOut = 0M;
                        inventoryItemLedger.TType = "I";
                    }
                    if (oo.Name.Contains("Sales"))
                    {
                        num3 -= Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyIn = 0M;
                        inventoryItemLedger.QtyOut = Math.Abs(oo.Quantity);
                        inventoryItemLedger.TType = "O";
                    }
                    if (oo.Name.Contains("Credit Note"))
                    {
                        num3 += Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyIn = Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyOut = 0M;
                        inventoryItemLedger.TType = "IR";
                    }
                    if (oo.Name.Contains("Debit Note"))
                    {
                        num3 -= Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyIn = 0M;
                        inventoryItemLedger.QtyOut = Math.Abs(oo.Quantity);
                        inventoryItemLedger.TType = "PR";
                    }
                    if (oo.Name.Contains("Purchase"))
                    {
                        num3 += Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyIn = Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyOut = 0M;
                        inventoryItemLedger.TType = "I";
                        num4 = oo.Price;
                        num5 = oo.Quantity;
                    }
                    inventoryItemLedger.BillNumber = oo.BillNumber;
                    inventoryItemLedger.RefNumber = oo.RefNumber;
                    inventoryItemLedger.QtyBalance = num3;
                    inventoryItemLedger.Rate = oo.Price;
                    inventoryItemLedger.TDate = oo.TDate.ToString();
                    inventoryItemLedger.pRate = num4;
                    inventoryItemLedger.UnitType = oo.UnitType;
                    inventoryItemLedger.LastQtyIn = num5;
                    source6.Add(inventoryItemLedger);
                    ++num6;
                }
                List<ViewInventoryItemLedger> list13 = source6.OrderBy<ViewInventoryItemLedger, int>((Func<ViewInventoryItemLedger, int>)(o => o.Id)).ToList<ViewInventoryItemLedger>();
                list13 = list13.OrderBy(o=> DateTime.Parse(o.TDate)).ToList();

                ViewInventoryItemLedger inventoryItemLedger1 = new ViewInventoryItemLedger();
                List<ViewInventoryItemLedger> inventoryItemLedgerList1 = new List<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> inventoryItemLedgerList2 = new List<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> inventoryItemLedgerList3 = new List<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> inventoryItemLedgerList4 = new List<ViewInventoryItemLedger>();
                inventoryItemLedgerList1 = list13.Where<ViewInventoryItemLedger>((Func<ViewInventoryItemLedger, bool>)(o => o.TType == "O")).ToList<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> list14 = list13.Where<ViewInventoryItemLedger>((Func<ViewInventoryItemLedger, bool>)(o => o.TType == "I")).ToList<ViewInventoryItemLedger>().OrderByDescending<ViewInventoryItemLedger, DateTime>((Func<ViewInventoryItemLedger, DateTime>)(o => DateTime.Parse(o.TDate))).ToList<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> list15 = list13.Where<ViewInventoryItemLedger>((Func<ViewInventoryItemLedger, bool>)(o => o.TType == "PR")).ToList<ViewInventoryItemLedger>();
                inventoryItemLedgerList2 = list13.Where<ViewInventoryItemLedger>((Func<ViewInventoryItemLedger, bool>)(o => o.TType == "IR")).ToList<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> inventoryItemLedgerList5 = new List<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> inventoryItemLedgerList6 = new List<ViewInventoryItemLedger>();
                Decimal qtyBalance = source6.LastOrDefault<ViewInventoryItemLedger>().QtyBalance;
                Decimal num7 = -1M;
                Decimal num8 = qtyBalance;
                foreach (ViewInventoryItemLedger inventoryItemLedger2 in list14)
                {
                    ViewInventoryItemLedger p = inventoryItemLedger2;
                    bool flag = true;
                    if (qtyBalance > num7)
                    {
                        if (num7 == -1M)
                            num7 = 0M;
                        List<ViewInventoryItemLedger> inventoryItemLedgerList7 = new List<ViewInventoryItemLedger>();
                        Decimal num9 = 0M;
                        if (p.BillNumber != null)
                        {
                            foreach (ViewInventoryItemLedger inventoryItemLedger3 in list15.Where<ViewInventoryItemLedger>((Func<ViewInventoryItemLedger, bool>)(o => o.RefNumber == p.BillNumber)).ToList<ViewInventoryItemLedger>())
                            {
                                num9 += inventoryItemLedger3.QtyOut;
                                list15.Remove(inventoryItemLedger3);
                            }
                        }
                        if (num9 > 0M)
                        {
                            Decimal num10 = p.QtyIn - num9;
                            if (num10 <= num8 & flag)
                            {
                                Decimal num11 = (p.QtyIn - num9) * p.pRate;
                                num2 += num11;
                                num7 += num10;
                                num8 -= num10;
                                flag = false;
                            }
                            if (num10 > num8 & flag)
                            {
                                if (num8 < num10)
                                {
                                    Decimal num12 = num8 * p.pRate;
                                    num2 += num12;
                                    num8 -= num10;
                                }
                                else
                                {
                                    Decimal num13 = num10 - num8;
                                    Decimal num14 = num13 * p.pRate;
                                    num2 += num14;
                                    num8 -= num13;
                                }
                                num7 += num10;
                            }
                        }
                        else if (num8 > p.QtyIn & flag)
                        {
                            Decimal num15 = p.QtyIn * p.pRate;
                            num2 += num15;
                            num7 += p.QtyIn;
                            num8 -= p.QtyIn;
                        }
                        else
                        {
                            Decimal num16 = num8 * p.pRate;
                            num2 += num16;
                            num7 += p.QtyIn;
                            num8 -= num8;
                        }
                    }
                }
            }
            source1.Add(new ProfitAndLoss()
            {
                Name = "Closing Stock",
                Amount = num2,
                SortOrder = 999,
                NatureofGroup = "Income",
                Bold = "No"
            });

            //Start Opening Stock
            foreach (MenuItemPortion menuItemPortion in list1)
            {
                MenuItemPortion currentitem = menuItemPortion;
                List<ViewInventoryItemLedger> source8 = new List<ViewInventoryItemLedger>();
                Decimal num17 = 0M;
                Decimal openingStockRate = currentitem.OpeningStockRate;
                var datas3 = source2.Where(o => o.ItemId == currentitem.Id);
                var datas4 = source4.Where(o => o.ItemId == currentitem.Id);
                List<ItemLedger> source9 = new List<ItemLedger>();
                source9.Add(new ItemLedger()
                {
                    ItemId = currentitem.Id,
                    Name = "Opening",
                    NVDate = fromDate,
                    Price = currentitem.OpeningStockRate,
                    Quantity = currentitem.OpeningStock,
                    TDate = DateTime.Parse(OpeningDate),
                    UnitType = "",
                    RefNumber = "0",
                    BillNumber = "0"
                });
                foreach (var data in datas3)
                    source9.Add(new ItemLedger()
                    {
                        ItemId = data.ItemId,
                        Name = data.Name,
                        NVDate = data.NVDate,
                        Price = data.Price,
                        Quantity = data.Quantity,
                        TDate = data.TDate,
                        UnitType = data.UnitType,
                        RefNumber = data.ref_invoice_number,
                        BillNumber = data.BillNo.ToString()
                    });
                foreach (var data in datas4)
                    source9.Add(new ItemLedger()
                    {
                        ItemId = data.ItemId,
                        Name = data.Name,
                        NVDate = data.NVDate,
                        Price = data.Price,
                        Quantity = data.Quantity,
                        TDate = data.TDate,
                        UnitType = data.UnitType,
                        RefNumber = data.ref_invoice_number,
                        BillNumber = data.BillNo
                    });
                List<ItemLedger> list16 = source9.OrderBy<ItemLedger, DateTime>((Func<ItemLedger, DateTime>)(o => o.TDate)).ToList<ItemLedger>();
                Decimal num18 = currentitem.OpeningStockRate;
                Decimal num19 = currentitem.OpeningStock;
                int num20 = 1;
                foreach (ItemLedger itemLedger in list16)
                {
                    ItemLedger oo = itemLedger;
                    string str = string.Empty;
                    Account account3 = new Account();
                    Account account4 = list2.Find((Predicate<Account>)(o => o.Id == oo.CustomerId));
                    if (account4 != null)
                        str = account4.Name;
                    ViewInventoryItemLedger inventoryItemLedger = new ViewInventoryItemLedger();
                    inventoryItemLedger.Id = num20;
                    inventoryItemLedger.Amount = Math.Abs(oo.Quantity) * oo.Price;
                    inventoryItemLedger.PartyName = str;
                    if (oo.Name.Contains("Opening"))
                    {
                        num17 += Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyIn = Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyOut = 0M;
                        inventoryItemLedger.TType = "I";
                    }
                    if (oo.Name.Contains("Sales"))
                    {
                        num17 -= Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyIn = 0M;
                        inventoryItemLedger.QtyOut = Math.Abs(oo.Quantity);
                        inventoryItemLedger.TType = "O";
                    }
                    if (oo.Name.Contains("Credit Note"))
                    {
                        num17 += Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyIn = Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyOut = 0M;
                        inventoryItemLedger.TType = "IR";
                    }
                    if (oo.Name.Contains("Debit Note"))
                    {
                        num17 -= Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyIn = 0M;
                        inventoryItemLedger.QtyOut = Math.Abs(oo.Quantity);
                        inventoryItemLedger.TType = "PR";
                    }
                    if (oo.Name.Contains("Purchase"))
                    {
                        num17 += Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyIn = Math.Abs(oo.Quantity);
                        inventoryItemLedger.QtyOut = 0M;
                        inventoryItemLedger.TType = "I";
                        num18 = oo.Price;
                        num19 = oo.Quantity;
                    }
                    inventoryItemLedger.BillNumber = oo.BillNumber;
                    inventoryItemLedger.RefNumber = oo.RefNumber;
                    inventoryItemLedger.QtyBalance = num17;
                    inventoryItemLedger.Rate = oo.Price;
                    inventoryItemLedger.TDate = oo.TDate.ToString();
                    inventoryItemLedger.pRate = num18;
                    inventoryItemLedger.UnitType = oo.UnitType;
                    inventoryItemLedger.LastQtyIn = num19;
                    source8.Add(inventoryItemLedger);
                    ++num20;
                }
                List<ViewInventoryItemLedger> list17 = source8.OrderByDescending<ViewInventoryItemLedger, int>((Func<ViewInventoryItemLedger, int>)(o => o.Id)).ToList<ViewInventoryItemLedger>();
                list17 = list17.OrderBy(o => DateTime.Parse(o.TDate)).ToList();
                source8 = source8.OrderBy(o => DateTime.Parse(o.TDate)).ToList();
                ViewInventoryItemLedger inventoryItemLedger4 = new ViewInventoryItemLedger();
                List<ViewInventoryItemLedger> inventoryItemLedgerList8 = new List<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> inventoryItemLedgerList9 = new List<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> inventoryItemLedgerList10 = new List<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> inventoryItemLedgerList11 = new List<ViewInventoryItemLedger>();
                inventoryItemLedgerList8 = list17.Where<ViewInventoryItemLedger>((Func<ViewInventoryItemLedger, bool>)(o => o.TType == "O")).ToList<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> list18 = list17.Where<ViewInventoryItemLedger>((Func<ViewInventoryItemLedger, bool>)(o => o.TType == "I")).ToList<ViewInventoryItemLedger>().OrderByDescending<ViewInventoryItemLedger, DateTime>((Func<ViewInventoryItemLedger, DateTime>)(o => DateTime.Parse(o.TDate))).ToList<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> list19 = list17.Where<ViewInventoryItemLedger>((Func<ViewInventoryItemLedger, bool>)(o => o.TType == "PR")).ToList<ViewInventoryItemLedger>();
                inventoryItemLedgerList9 = list17.Where<ViewInventoryItemLedger>((Func<ViewInventoryItemLedger, bool>)(o => o.TType == "IR")).ToList<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> inventoryItemLedgerList12 = new List<ViewInventoryItemLedger>();
                List<ViewInventoryItemLedger> inventoryItemLedgerList13 = new List<ViewInventoryItemLedger>();
                Decimal qtyBalance = source8.LastOrDefault<ViewInventoryItemLedger>().QtyBalance;
                Decimal num21 = -1M;
                Decimal num22 = qtyBalance;
                foreach (ViewInventoryItemLedger inventoryItemLedger5 in list18)
                {
                    ViewInventoryItemLedger p = inventoryItemLedger5;
                    bool flag = true;
                    if (qtyBalance > num21)
                    {
                        if (num21 == -1M)
                            num21 = 0M;
                        List<ViewInventoryItemLedger> inventoryItemLedgerList14 = new List<ViewInventoryItemLedger>();
                        Decimal num23 = 0M;
                        if (p.BillNumber != null)
                        {
                            foreach (ViewInventoryItemLedger inventoryItemLedger6 in list19.Where<ViewInventoryItemLedger>((Func<ViewInventoryItemLedger, bool>)(o => o.RefNumber == p.BillNumber)).ToList<ViewInventoryItemLedger>())
                            {
                                num23 += inventoryItemLedger6.QtyOut;
                                list19.Remove(inventoryItemLedger6);
                            }
                        }
                        if (num23 > 0M)
                        {
                            Decimal num24 = p.QtyIn - num23;
                            if (num24 <= num22 & flag)
                            {
                                Decimal num25 = (p.QtyIn - num23) * p.pRate;
                                num1 += num25;
                                num21 += num24;
                                num22 -= num24;
                                flag = false;
                            }
                            if (num24 > num22 & flag)
                            {
                                if (num22 < num24)
                                {
                                    Decimal num26 = num22 * p.pRate;
                                    num1 += num26;
                                    num22 -= num24;
                                }
                                else
                                {
                                    Decimal num27 = num24 - num22;
                                    Decimal num28 = num27 * p.pRate;
                                    num1 += num28;
                                    num22 -= num27;
                                }
                                num21 += num24;
                            }
                        }
                        else if (num22 > p.QtyIn & flag)
                        {
                            Decimal num29 = p.QtyIn * p.pRate;
                            num1 += num29;
                            num21 += p.QtyIn;
                            num22 -= p.QtyIn;
                        }
                        else
                        {
                            Decimal num30 = num22 * p.pRate;
                            num1 += num30;
                            num21 += p.QtyIn;
                            num22 -= num22;
                        }
                    }
                }
            }
            source1.Add(new ProfitAndLoss()
            {
                Name = "Opening Stock",
                Amount = num1,
                SortOrder = 0,
                NatureofGroup = "Expenses",
                Bold = "No"
            });
            var inner = list2.Join((IEnumerable<AccountType>)list11, (Func<Account, int>)(a => a.AccountTypeId), (Func<AccountType, int>)(at => at.Id), (a, at) => new
            {
                Id = a.Id,
                Name = a.Name,
                NatureofGroup = at.NatureofGroup,
                SortOrder = at.SortOrder,
                SORTPOSITION = at.SORTPOSITION,
                AccountTypeId = a.AccountTypeId
            });
            List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
            List<AccountTransactionValue> list20 = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Date >= sFormDate && o.Date <= sToDate)).ToList<AccountTransactionValue>();
            if (BranchId != 0)
                list20 = list20.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.BranchId == BranchId)).ToList<AccountTransactionValue>();
            var datas = list20.Select(x => new
            {
                Name = x.Name.Substring(0, x.Name.IndexOf("[#")),
                AccountTypeId = x.AccountTypeId,
                AccountId = x.AccountId,
                Debit = x.Debit,
                Credit = x.Credit
            }).GroupBy(s => new
            {
                AccountId = s.AccountId,
                Name = s.Name
            }).Select(g => new
            {
                Name = g.FirstOrDefault().Name,
                AccountId = g.Key.AccountId,
                AccountTypeId = g.FirstOrDefault().AccountTypeId,
                Debit = g.Sum(x => Math.Round(Convert.ToDecimal(x.Debit), 2)),
                Credit = g.Sum(x => Math.Round(Convert.ToDecimal(x.Credit), 2))
            }).Join(inner, l => l.AccountId, a => a.Id, (l, a) => new
            {
                BusinesType = l.Name,
                AccountId = l.AccountId,
                Credit = l.Credit,
                Debit = l.Debit,
                Name = a.Name,
                NatureofGroup = a.NatureofGroup,
                SortOrder = a.SortOrder,
                AccountTypeId = a.AccountTypeId
            });
            Decimal num31 = 0M;
            Decimal num32 = 0M;
            foreach (var data in datas)
            {
                ProfitAndLoss profitAndLoss = new ProfitAndLoss();
                if (data.NatureofGroup == "Expenses")
                {
                    Decimal num33 = data.Debit - data.Credit;
                    profitAndLoss.Name = data.Name;
                    profitAndLoss.Amount = num33;
                    profitAndLoss.SortOrder = data.SortOrder;
                    profitAndLoss.NatureofGroup = data.NatureofGroup;
                    num31 += Math.Abs(num33);
                    profitAndLoss.Bold = "No";
                    profitAndLoss.AccountTypeId = data.AccountTypeId;
                    profitAndLoss.BusinesType = data.BusinesType;
                    source1.Add(profitAndLoss);
                }
                if (data.NatureofGroup == "Income")
                {
                    Decimal num34 = data.Credit - data.Debit;
                    profitAndLoss.Name = data.Name;
                    profitAndLoss.Amount = num34;
                    profitAndLoss.SortOrder = data.SortOrder;
                    profitAndLoss.NatureofGroup = data.NatureofGroup;
                    num32 += num34;
                    profitAndLoss.Bold = "No";
                    profitAndLoss.AccountTypeId = data.AccountTypeId;
                    profitAndLoss.BusinesType = data.BusinesType;
                    source1.Add(profitAndLoss);
                }
            }
            return source1.OrderBy<ProfitAndLoss, int>((Func<ProfitAndLoss, int>)(o => o.SortOrder)).OrderBy<ProfitAndLoss, int>((Func<ProfitAndLoss, int>)(o => o.AccountTypeId)).OrderBy<ProfitAndLoss, string>((Func<ProfitAndLoss, string>)(o => o.Name)).ToList<ProfitAndLoss>();
        }

        public static List<BalanceSheet> GetBalanceSheet(
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          DCubeRepository<FinancialYear> FinancialYearRepo,
          string FinancialYear)
        {
            List<BalanceSheet> balanceSheet1 = new List<BalanceSheet>();
            List<Account> accountList = new List<Account>();
            List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
            List<AccountType> accountTypeList = new List<AccountType>();
            List<AccountType> list2 = AccountTypeRepository.GetAllData().ToList<AccountType>().Where<AccountType>((Func<AccountType, bool>)(o => o.NatureofGroup == "Assets" || o.NatureofGroup == "Liabilities")).ToList<AccountType>();
            var source1 = list1.Join((IEnumerable<AccountType>)list2, (Func<Account, int>)(a => a.AccountTypeId), (Func<AccountType, int>)(at => at.Id), (a, at) => new
            {
                Id = a.Id,
                Name = a.Name,
                NatureofGroup = at.NatureofGroup,
                Amount = a.Amount,
                DRCR = a.DRCR,
                AccountTypeId = a.AccountTypeId
            });
            List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
            var source2 = accValueRepository.GetAllData().ToList<AccountTransactionValue>().Select(x => new
            {
                AccountTypeId = x.AccountTypeId,
                AccountId = x.AccountId,
                Debit = x.Debit,
                Credit = x.Credit
            }).GroupBy(s => new { AccountId = s.AccountId }).Select(g => new
            {
                AccountId = g.Key.AccountId,
                AccountTypeId = g.FirstOrDefault().AccountTypeId,
                Debit = g.Sum(x => Math.Round(Convert.ToDecimal(x.Debit), 2)),
                Credit = g.Sum(x => Math.Round(Convert.ToDecimal(x.Credit), 2))
            });
            BalanceSheet balanceSheet2 = new BalanceSheet();
            List<BalanceSheetDetail> source3 = new List<BalanceSheetDetail>();
            List<BalanceSheetDetail> source4 = new List<BalanceSheetDetail>();
            foreach (var data1 in source1.Distinct())
            {
                var objbalancesheet = data1;
                Decimal num = 0M;
                if (objbalancesheet.DRCR == "Dr" || objbalancesheet.DRCR == " " || objbalancesheet.DRCR == "")
                    num += objbalancesheet.Amount;
                if (objbalancesheet.DRCR == "Cr")
                    num -= objbalancesheet.Amount;
                var data2 = source2.Where(o => o.AccountId == objbalancesheet.Id).FirstOrDefault();
                if (data2 != null)
                    num = num + data2.Debit - data2.Credit;
                if (num > 0M)
                    source4.Add(new BalanceSheetDetail()
                    {
                        AccountId = objbalancesheet.Id,
                        AccountTypeId = objbalancesheet.AccountTypeId,
                        Amount = Math.Abs(num),
                        Bold = "No",
                        Name = objbalancesheet.Name,
                        NatureofGroup = objbalancesheet.NatureofGroup,
                        SortOrder = 0
                    });
                if (num < 0M)
                    source3.Add(new BalanceSheetDetail()
                    {
                        AccountId = objbalancesheet.Id,
                        AccountTypeId = objbalancesheet.AccountTypeId,
                        Amount = Math.Abs(num),
                        Bold = "No",
                        Name = objbalancesheet.Name,
                        NatureofGroup = objbalancesheet.NatureofGroup,
                        SortOrder = 0
                    });
            }
            List<ProfitAndLoss> profitAndLossList = new List<ProfitAndLoss>();
            ProfitAndLoss profitAndLoss = TrialBalanceBusiness.GetProfitLoss(AccountTypeRepository, AccountRepository, accValueRepository, FinancialYearRepo, FinancialYear).Find((Predicate<ProfitAndLoss>)(o => o.SortOrder == 6));
            Decimal num1 = 0M;
            if (profitAndLoss != null)
                num1 = profitAndLoss.Amount;
            source3.Add(new BalanceSheetDetail()
            {
                AccountId = 9999,
                AccountTypeId = 9999,
                Amount = Math.Abs(num1),
                Bold = "Yes",
                Name = "Profit & Loss",
                NatureofGroup = "",
                SortOrder = 0
            });
            List<BalanceSheetDetail> list3 = source3.Distinct<BalanceSheetDetail>().ToList<BalanceSheetDetail>();
            balanceSheet2.Lability = list3;
            List<BalanceSheetDetail> list4 = source4.Distinct<BalanceSheetDetail>().ToList<BalanceSheetDetail>();
            balanceSheet2.Asset = list4;
            balanceSheet1.Add(balanceSheet2);
            return balanceSheet1;
        }

        private static List<TrialBalance> CurrentFinancialYear(
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          DCubeRepository<FinancialYear> FinancialYearRepo,
          string FinancialYear)
        {
            List<TrialBalance> source1 = new List<TrialBalance>();
            List<AccountType> accountTypeList1 = new List<AccountType>();
            List<AccountType> accountTypeList2 = new List<AccountType>();
            List<AccountType> list1 = AccountTypeRepository.GetAllData().ToList<AccountType>();
            List<AccountType> list2 = list1.Where<AccountType>((Func<AccountType, bool>)(o => o.NatureofGroup == "Assets" || o.NatureofGroup == "Liabilities")).ToList<AccountType>();
            List<Account> accountList = new List<Account>();
            List<Account> list3 = AccountRepository.GetAllData().ToList<Account>();
            var source2 = list3.Join((IEnumerable<AccountType>)list2, (Func<Account, int>)(a => a.AccountTypeId), (Func<AccountType, int>)(at => at.Id), (a, at) => new
            {
                Id = a.Id,
                Name = a.Name,
                NatureofGroup = at.NatureofGroup,
                Amount = a.Amount,
                DRCR = a.DRCR,
                AccountTypeId = a.AccountTypeId
            });
            List<AccountTransactionValue> transactionValueList1 = new List<AccountTransactionValue>();
            List<AccountTransactionValue> list4 = accValueRepository.GetAllData().ToList<AccountTransactionValue>();
            var source3 = list4.Select(x => new
            {
                AccountTypeId = x.AccountTypeId,
                AccountId = x.AccountId,
                Debit = x.Debit,
                Credit = x.Credit
            }).GroupBy(s => new { AccountId = s.AccountId }).Select(g => new
            {
                AccountId = g.Key.AccountId,
                AccountTypeId = g.FirstOrDefault().AccountTypeId,
                Debit = g.Sum(x => Math.Round(Convert.ToDecimal(x.Debit), 2)),
                Credit = g.Sum(x => Math.Round(Convert.ToDecimal(x.Credit), 2))
            });
            foreach (var data1 in source2.Distinct())
            {
                var objtrial = data1;
                TrialBalance trialBalance = new TrialBalance();
                Decimal num = 0M;
                if (objtrial.DRCR == "Dr" || objtrial.DRCR == " " || objtrial.DRCR == "")
                    num += objtrial.Amount;
                if (objtrial.DRCR == "Cr")
                    num -= objtrial.Amount;
                var data2 = source3.Where(o => o.AccountId == objtrial.Id).FirstOrDefault();
                if (data2 != null)
                    num = num + data2.Debit - data2.Credit;
                trialBalance.Id = objtrial.Id;
                trialBalance.AccountTypeId = objtrial.AccountTypeId;
                if (num != 0M)
                {
                    if (num < 0M)
                        trialBalance.Credit = Math.Abs(num);
                    else
                        trialBalance.Debit = num;
                    trialBalance.Name = objtrial.Name;
                    source1.Add(trialBalance);
                }
            }
            List<AccountType> accountTypeList3 = new List<AccountType>();
            List<AccountType> list5 = list1.Where<AccountType>((Func<AccountType, bool>)(o => o.NatureofGroup == "Income" || o.NatureofGroup == "Expenses")).ToList<AccountType>();
            var datas = list3.Join((IEnumerable<AccountType>)list5, (Func<Account, int>)(a => a.AccountTypeId), (Func<AccountType, int>)(at => at.Id), (a, at) => new
            {
                Id = a.Id,
                Name = a.Name,
                NatureofGroup = at.NatureofGroup,
                Amount = a.Amount,
                DRCR = a.DRCR,
                AccountTypeId = a.AccountTypeId
            });
            List<AccountTransactionValue> transactionValueList2 = new List<AccountTransactionValue>();
            var source4 = list4.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.FinancialYear == FinancialYear)).ToList<AccountTransactionValue>().Select(x => new
            {
                AccountTypeId = x.AccountTypeId,
                AccountId = x.AccountId,
                Debit = x.Debit,
                Credit = x.Credit
            }).GroupBy(s => new { AccountId = s.AccountId }).Select(g => new
            {
                AccountId = g.Key.AccountId,
                AccountTypeId = g.FirstOrDefault().AccountTypeId,
                Debit = g.Sum(x => Math.Round(Convert.ToDecimal(x.Debit), 2)),
                Credit = g.Sum(x => Math.Round(Convert.ToDecimal(x.Credit), 2))
            });
            foreach (var data3 in datas)
            {
                var objtrial = data3;
                TrialBalance trialBalance = new TrialBalance();
                Decimal num = 0M;
                var data4 = source4.Where(o => o.AccountId == objtrial.Id).FirstOrDefault();
                if (data4 != null)
                    num = num + data4.Debit - data4.Credit;
                trialBalance.AccountTypeId = objtrial.AccountTypeId;
                if (num != 0M)
                {
                    if (num < 0M)
                        trialBalance.Credit = Math.Abs(num);
                    else
                        trialBalance.Debit = num;
                    trialBalance.Name = objtrial.Name;
                    source1.Add(trialBalance);
                }
            }
            return source1.Distinct<TrialBalance>().ToList<TrialBalance>().OrderBy<TrialBalance, string>((Func<TrialBalance, string>)(o => o.Name)).ToList<TrialBalance>();
        }

        public static List<DayWiseSales> TotalDayWiseSale(
          IDCubeRepository<Order> OrderRepository,
          string fromDate,
          string toDate)
        {
            List<DayWiseSales> dayWiseSalesList = new List<DayWiseSales>();
            DateTime sFormDate = NepalitoEnglishDate.EnglishDate(fromDate);
            DateTime sToDate = NepalitoEnglishDate.EnglishDate(toDate);
            foreach (var data in OrderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o => o.CreatedDateTime >= sFormDate && o.CreatedDateTime <= sToDate)).ToList<Order>().Select(cl => new
            {
                InventoryItemId = cl.MenuItemId,
                Qty = cl.Quantity,
                NVDate = cl.NVDate,
                Price = cl.Price,
                CreatedDateTime = cl.CreatedDateTime
            }).GroupBy(data => new
            {
                InventoryItemId = data.InventoryItemId,
                NVDate = data.NVDate
            }).Select(cl => new
            {
                InventoryItemId = cl.Key.InventoryItemId,
                NDate = cl.FirstOrDefault().NVDate,
                Amount = cl.LastOrDefault().Price * cl.Sum(c => c.Qty),
                CreatedDateTime = cl.FirstOrDefault().CreatedDateTime
            }).GroupBy(data => new { NDate = data.NDate }).Select(cl => new
            {
                NDate = cl.Key.NDate,
                Amount = cl.Sum(o => o.Amount),
                CreatedDateTime = cl.FirstOrDefault().CreatedDateTime
            }).OrderBy(o => o.CreatedDateTime))
                dayWiseSalesList.Add(new DayWiseSales()
                {
                    Amount = data.Amount,
                    NDate = data.NDate
                });
            return dayWiseSalesList;
        }
    }
}
