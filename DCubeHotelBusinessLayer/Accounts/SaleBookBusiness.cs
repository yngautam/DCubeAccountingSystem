using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.Accounts
{
    public class SaleBookBusiness
    {
        public static List<SaleBook> GetSaleBook(
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionDocument> AccountTransactionDocumentRepository,
          string Month,
          string FinancialYear)
        {
            List<SaleBook> saleBook1 = new List<SaleBook>();
            List<AccountTransactionValue> transactionValueList1 = new List<AccountTransactionValue>();
            List<AccountTransactionValue> transactionValueList2 = new List<AccountTransactionValue>();
            List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
            List<Ticket> ticketList = new List<Ticket>();
            AccountType accountType = new AccountType();
            List<AccountTransactionDocument> transactionDocumentList = new List<AccountTransactionDocument>();
            List<AccountTransactionType> accountTransactionTypeList = new List<AccountTransactionType>();
            List<Account> source = new List<Account>();
            source = AccountRepository.GetAllData().ToList<Account>();
            accountTransactionTypeList = AccountTransactionTypeRepository.GetAllData().ToList<AccountTransactionType>();
            string accounttype = "Sales";
            if (AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Id == 20)).FirstOrDefault<AccountType>() == null)
                accountType = AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Name.Contains("Sales"))).FirstOrDefault<AccountType>();
            List<AccountTransactionDocument> list1 = AccountTransactionDocumentRepository.GetAllData().Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Name.Contains(accounttype))).ToList<AccountTransactionDocument>();
            List<Ticket> list2 = TicketRepository.GetAllData().ToList<Ticket>().Where<Ticket>((Func<Ticket, bool>)(o => o.FinancialYear == FinancialYear && o.NepaliMonth == Month)).ToList<Ticket>();
            List<AccountTransactionValue> list3 = AccountTransactionValueRepository.GetAllData().ToList<AccountTransactionValue>();
            foreach (Ticket ticket in list2)
            {
                try
                {
                    string curretaccounttype = accounttype + " [#" + ticket.TicketNumber + "]";
                    AccountTransactionDocument objAccountTransactionDocument = list1.Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Name.Contains(curretaccounttype))).FirstOrDefault<AccountTransactionDocument>();
                    if (objAccountTransactionDocument != null)
                    {
                        List<AccountTransactionValue> list4 = list3.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionDocumentId == objAccountTransactionDocument.Id)).ToList<AccountTransactionValue>();
                        Decimal num = 0M;
                        SaleBook saleBook2 = new SaleBook();
                        saleBook2.BillNo = ticket.TicketNumber;
                        saleBook2.BuyerName = "-";
                        saleBook2.BuyerPAN = "-";
                        saleBook2.ExportSale = 0M;
                        saleBook2.NonTaxableSale = 0M;
                        saleBook2.VDate = ticket.NVDate;
                        AccountTransactionValue ObjAccountTransactionValue = new AccountTransactionValue();
                        ObjAccountTransactionValue = list4.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId != 3 && o.Debit > 0M)).FirstOrDefault<AccountTransactionValue>();
                        if (ObjAccountTransactionValue != null)
                        {
                            Account account1 = new Account();
                            Account account2 = source.Where<Account>((Func<Account, bool>)(o => o.Id == ObjAccountTransactionValue.AccountId)).FirstOrDefault<Account>();
                            saleBook2.BuyerName = account2.Name;
                            if (account2.PanNo != null)
                                saleBook2.BuyerPAN = account2.PanNo;
                        }
                        foreach (AccountTransactionValue transactionValue in list4)
                        {
                            if (transactionValue.AccountId == 3)
                            {
                                saleBook2.Discount = transactionValue.Debit;
                                num -= transactionValue.Debit;
                            }
                            if (transactionValue.AccountId == 8)
                                saleBook2.Tax = transactionValue.Credit;
                            if (transactionValue.AccountId != 3 && transactionValue.Debit > 0M)
                            {
                                saleBook2.TotalSale = transactionValue.Debit;
                                num += transactionValue.Debit;
                            }
                        }
                        saleBook2.TaxableAmount = num;
                        saleBook1.Add(saleBook2);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return saleBook1;
        }

        public static List<SaleBillingBook> GetSaleBillingBook(
          IDCubeRepository<Table> TableRepository,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<AccountTransactionDocument> AccountTransactionDocumentRepository,
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<SaleBillingBook> SaleBillingBookRepository,
          IDCubeRepository<ScreenOrderItemRequest> ScreenOrderItemRequestRepo,
          string FromDate,
          string ToDate,
          int TransactionTypeId)
        {
            DateTime sFormDate = NepalitoEnglishDate.EnglishDate(FromDate);
            DateTime sToDate = NepalitoEnglishDate.EnglishDate(ToDate);
            List<Table> tableList = new List<Table>();
            List<Account> accountList = new List<Account>();
            List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
            List<SaleBillingBook> saleBillingBook1 = new List<SaleBillingBook>();
            List<AccountTransactionValue> transactionValueList1 = new List<AccountTransactionValue>();
            List<AccountTransactionValue> source = new List<AccountTransactionValue>();
            List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
            List<Ticket> ticketList = new List<Ticket>();
            AccountType objAccountType = new AccountType();
            List<AccountTransactionDocument> transactionDocumentList = new List<AccountTransactionDocument>();
            List<AccountTransactionType> accountTransactionTypeList = new List<AccountTransactionType>();
            accountTransactionTypeList = AccountTransactionTypeRepository.GetAllData().ToList<AccountTransactionType>();
            List<Table> list2 = TableRepository.GetAllData().ToList<Table>();
            objAccountType = AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Id == 20)).FirstOrDefault<AccountType>();
            if (objAccountType == null)
                objAccountType = AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Name.Contains("Sales"))).FirstOrDefault<AccountType>();
            if (objAccountType == null)
                objAccountType = AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Name.Contains("Sales"))).FirstOrDefault<AccountType>();
            List<AccountTransactionDocument> list3 = AccountTransactionDocumentRepository.GetAllData().Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Name.Contains(objAccountType.Name))).ToList<AccountTransactionDocument>();
            List<Ticket> list4 = TicketRepository.GetAllData().ToList<Ticket>().Where<Ticket>((Func<Ticket, bool>)(o => o.Date.Date >= sFormDate && o.Date.Date <= sToDate)).ToList<Ticket>();
            List<AccountTransactionValue> list5 = AccountTransactionValueRepository.GetAllData().ToList<AccountTransactionValue>();
            foreach (Ticket ticket in list4)
            {
                Ticket objTicket = ticket;
                if (objTicket.AccountTransactionDocumentId != 0)
                {
                    source = list5.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionDocumentId == objTicket.AccountTransactionDocumentId)).ToList<AccountTransactionValue>();
                }
                else
                {
                    AccountTransactionDocument document = list3.Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Name.Replace(objAccountType.Name, "").Replace("[#", "").Replace("]", "").Trim() == objTicket.TicketNumber)).FirstOrDefault<AccountTransactionDocument>();
                    if (document != null)
                        source = list5.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionDocumentId == document.Id)).ToList<AccountTransactionValue>();
                }
                AccountTransactionValue transactionValue1 = new AccountTransactionValue();
                AccountTransactionValue transactionValue2 = source.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == 3)).FirstOrDefault<AccountTransactionValue>();
                Account account1 = new Account();
                Account account2 = list1.Find((Predicate<Account>)(o => o.Name == "Taxable Sales"));
                int iTaxableAccountId = 0;
                if (account2 != null)
                    iTaxableAccountId = account2.Id;
                Account account3 = new Account();
                Account account4 = list1.Find((Predicate<Account>)(o => o.Name == "Non Taxable Sales"));
                int inonTaxableAccountId = 0;
                if (account4 != null)
                    inonTaxableAccountId = account4.Id;
                List<AccountTransactionValue> transactionValueList2 = new List<AccountTransactionValue>();
                List<AccountTransactionValue> list6 = source.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == iTaxableAccountId || o.AccountId == inonTaxableAccountId || o.AccountId == 1)).ToList<AccountTransactionValue>();
                AccountTransactionValue transactionValue3 = new AccountTransactionValue();
                AccountTransactionValue transactionValue4 = source.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Debit == o.Debit)).FirstOrDefault<AccountTransactionValue>();
                AccountTransactionValue transactionValue5 = new AccountTransactionValue();
                AccountTransactionValue transactionValue6 = source.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == 8)).FirstOrDefault<AccountTransactionValue>();
                AccountTransactionValue transactionValue7 = new AccountTransactionValue();
                AccountTransactionValue transactionValue8 = source.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == 9)).FirstOrDefault<AccountTransactionValue>();
                Decimal num = 0M;
                SaleBillingBook saleBillingBook2 = new SaleBillingBook();
                saleBillingBook2.BillNo = objTicket.TicketNumber;
                saleBillingBook2.VDate = objTicket.NVDate;
                if (objTicket.TicketTypeId == 1)
                {
                    Account account5 = new Account();
                    Account account6 = list1.Find((Predicate<Account>)(o => o.Id == objTicket.Table_Customer_Room));
                    if (account6 != null)
                        saleBillingBook2.Name = account6.Name;
                }
                if (objTicket.TicketTypeId == 2)
                {
                    Table table1 = new Table();
                    Table table2 = list2.Find((Predicate<Table>)(o => o.Id == objTicket.Table_Customer_Room));
                    if (table2 != null)
                        saleBillingBook2.Name = table2.Name;
                }
                saleBillingBook2.Discount = transactionValue2 == null ? 0M : transactionValue2.Debit;
                saleBillingBook2.ServiceCharge = transactionValue8 == null ? 0M : transactionValue8.Credit;
                if (transactionValue6 != null)
                    saleBillingBook2.Tax = transactionValue6.Credit;
                if (list6 != null)
                {
                    foreach (AccountTransactionValue transactionValue9 in list6)
                        num += transactionValue9.Credit;
                    saleBillingBook2.GrandTotal = num;
                }
                saleBillingBook2.BillTotal = transactionValue4.Debit;
                saleBillingBook1.Add(saleBillingBook2);
            }
            return saleBillingBook1;
        }

        public static List<SalesBillItem> GetSaleBookItemWise(
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<MenuItemPortion> MenuItemPortionepository,
          DateTime FromDate,
          DateTime ToDate)
        {
            List<SalesBillItem> saleBookItemWise = new List<SalesBillItem>();
            List<Ticket> ticketList = new List<Ticket>();
            List<Order> orderList = new List<Order>();
            List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
            List<Ticket> list1 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Date >= FromDate && o.Date <= ToDate)).ToList<Ticket>();
            List<Order> list2 = OrderRepository.GetAllData().ToList<Order>();
            List<MenuItemPortion> list3 = MenuItemPortionepository.GetAllData().ToList<MenuItemPortion>();
            foreach (var data in list1.Join((IEnumerable<Order>)list2, (Func<Ticket, int>)(t => t.Id), (Func<Order, int>)(o => o.TicketId), (t, o) => new
            {
                t = t,
                o = o
            }).Join((IEnumerable<MenuItemPortion>)list3, _param1 => _param1.o.MenuItemId, (Func<MenuItemPortion, int>)(m => m.Id), (_param1, m) => new
            {
                Quantity = _param1.o.Quantity,
                Price = _param1.o.Price,
                Amount = _param1.o.Quantity * _param1.o.Price,
                Name = m.Name
            }).GroupBy(l => new { Name = l.Name, Price = l.Price }).Select(cl => new
            {
                Name = cl.First().Name,
                Quantity = cl.Sum(c => c.Quantity),
                Price = cl.First().Price
            }))
            {
                if (data.Quantity > 0M)
                    saleBookItemWise.Add(new SalesBillItem()
                    {
                        Amount = data.Quantity * data.Price,
                        ItemName = data.Name,
                        Quantity = data.Quantity,
                        Rate = data.Price
                    });
            }
            return saleBookItemWise;
        }

        public static List<SaleBookCustomer> GetSaleBookCustomerWise(
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<MenuItemPortion> MenuItemPortionepository,
          IDCubeRepository<Account> AccountRepository,
          string CustomerId,
          string FinancialYear)
        {
            List<SaleBookCustomer> bookCustomerWise = new List<SaleBookCustomer>();
            List<Ticket> ticketList = new List<Ticket>();
            List<Order> orderList = new List<Order>();
            List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
            Account account1 = new Account();
            Account account2 = AccountRepository.GetAllData().Where<Account>((Func<Account, bool>)(o => o.Id == int.Parse(CustomerId))).FirstOrDefault<Account>();
            if (account2 != null)
            {
                SaleBookCustomer saleBookCustomer = new SaleBookCustomer();
                saleBookCustomer.BillNo = "Opening Balance";
                saleBookCustomer.TotalSale = account2.Amount;
                List<SalesBillItem> salesBillItemList = new List<SalesBillItem>();
                SalesBillItem salesBillItem = new SalesBillItem();
                saleBookCustomer.SalesBillItems = (IEnumerable<SalesBillItem>)salesBillItemList;
                bookCustomerWise.Add(saleBookCustomer);
            }
            List<Ticket> list1 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.TicketTypeId == 1 && o.Table_Customer_Room == int.Parse(CustomerId) && o.FinancialYear == FinancialYear)).ToList<Ticket>();
            var datas = list1.Select(o => new
            {
                Id = o.Id,
                TicketNumber = o.TicketNumber,
                TotalAmount = o.TotalAmount,
                NVDate = o.NVDate
            });
            List<Order> list2 = OrderRepository.GetAllData().ToList<Order>();
            List<MenuItemPortion> list3 = MenuItemPortionepository.GetAllData().ToList<MenuItemPortion>();
            var source = list1.Join((IEnumerable<Order>)list2, (Func<Ticket, int>)(t => t.Id), (Func<Order, int>)(o => o.TicketId), (t, o) => new
            {
                t = t,
                o = o
            }).Join((IEnumerable<MenuItemPortion>)list3, _param1 => _param1.o.MenuItemId, (Func<MenuItemPortion, int>)(m => m.Id), (_param1, m) => new
            {
                TicketId = _param1.o.TicketId,
                Quantity = _param1.o.Quantity,
                Price = _param1.o.Price,
                Amount = _param1.o.Quantity * _param1.o.Price,
                Name = m.Name
            });
            foreach (var data1 in datas)
            {
                var t = data1;
                List<SalesBillItem> salesBillItemList = new List<SalesBillItem>();
                SaleBookCustomer saleBookCustomer = new SaleBookCustomer();
                saleBookCustomer.BillNo = t.TicketNumber;
                saleBookCustomer.TotalSale = t.TotalAmount;
                saleBookCustomer.VDate = t.NVDate;
                foreach (var data2 in source.Where(o => o.TicketId == t.Id))
                    salesBillItemList.Add(new SalesBillItem()
                    {
                        Amount = data2.Quantity * data2.Price,
                        ItemName = data2.Name,
                        Quantity = data2.Quantity,
                        Rate = data2.Price
                    });
                saleBookCustomer.SalesBillItems = (IEnumerable<SalesBillItem>)salesBillItemList;
                bookCustomerWise.Add(saleBookCustomer);
            }
            return bookCustomerWise;
        }

        public static List<SaleBookDate> GetSaleBookDateWise(
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<MenuItemPortion> MenuItemPortionepository,
          DateTime FromDate,
          DateTime ToDate)
        {
            List<SaleBookDate> saleBookDateWise = new List<SaleBookDate>();
            List<Ticket> ticketList = new List<Ticket>();
            List<Order> orderList = new List<Order>();
            List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
            List<Account> accountList = new List<Account>();
            List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
            List<Ticket> list2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Date >= FromDate && o.Date <= ToDate)).ToList<Ticket>();
            list2.Where<Ticket>((Func<Ticket, bool>)(o => o.TicketTypeId == 1));
            IEnumerable<Ticket> source1 = list2.Where<Ticket>((Func<Ticket, bool>)(o => o.TicketTypeId == 2));
            var second = list2.Join((IEnumerable<Account>)list1, (Func<Ticket, int>)(t => t.Table_Customer_Room), (Func<Account, int>)(a => a.Id), (t, a) => new
            {
                Id = t.Id,
                TicketNumber = t.TicketNumber,
                TotalAmount = t.TotalAmount,
                NVDate = t.NVDate,
                Name = a.Name,
                PanNo = a.PanNo
            });
            var datas = source1.Select(o => new
            {
                Id = o.Id,
                TicketNumber = o.TicketNumber,
                TotalAmount = o.TotalAmount,
                NVDate = o.NVDate,
                Name = "Counter Sales",
                PanNo = ""
            }).Union(second);
            List<Order> list3 = OrderRepository.GetAllData().ToList<Order>();
            List<MenuItemPortion> list4 = MenuItemPortionepository.GetAllData().ToList<MenuItemPortion>();
            var source2 = list2.Join((IEnumerable<Order>)list3, (Func<Ticket, int>)(t => t.Id), (Func<Order, int>)(o => o.TicketId), (t, o) => new
            {
                t = t,
                o = o
            }).Join((IEnumerable<MenuItemPortion>)list4, _param1 => _param1.o.MenuItemId, (Func<MenuItemPortion, int>)(m => m.Id), (_param1, m) => new
            {
                TicketId = _param1.o.TicketId,
                Quantity = _param1.o.Quantity,
                Price = _param1.o.Price,
                Amount = _param1.o.Quantity * _param1.o.Price,
                Name = m.Name
            });
            foreach (var data1 in datas)
            {
                var t = data1;
                List<SalesBillItem> salesBillItemList = new List<SalesBillItem>();
                SaleBookDate saleBookDate = new SaleBookDate();
                saleBookDate.BillNo = t.TicketNumber;
                saleBookDate.TotalSale = t.TotalAmount;
                saleBookDate.VDate = t.NVDate;
                saleBookDate.BuyerName = t.Name;
                saleBookDate.BuyerPAN = t.PanNo;
                foreach (var data2 in source2.Where(o => o.TicketId == t.Id))
                    salesBillItemList.Add(new SalesBillItem()
                    {
                        Amount = data2.Quantity * data2.Price,
                        ItemName = data2.Name,
                        Quantity = data2.Quantity,
                        Rate = data2.Price
                    });
                saleBookDate.SalesBillItems = (IEnumerable<SalesBillItem>)salesBillItemList;
                saleBookDateWise.Add(saleBookDate);
            }
            return saleBookDateWise;
        }
    }
}
