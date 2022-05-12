using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelUser;
using NepaliDate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.TicketBusinessLayer
{
  public class TicketSaleBillingBusiness
  {
    public static string TicketLastNumber(
      IDCubeRepository<Ticket> TicketRepository,
      AccountTransaction objAccountTransaction)
    {
      string s = "0";
      if (TicketRepository.GetAllData().ToList<Ticket>().Count > 0)
      {
        if (objAccountTransaction.Name != "Credit Note")
        {
          s = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>) (o => o.TicketStates != "Credit Note")).OrderByDescending<Ticket, int>((Func<Ticket, int>) (o => o.Id)).FirstOrDefault<Ticket>().TicketNumber;
        }
        else
        {
          try
          {
            s = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>) (o => o.TicketStates == "Credit Note")).OrderByDescending<Ticket, int>((Func<Ticket, int>) (o => o.Id)).FirstOrDefault<Ticket>().TicketNumber;
          }
          catch
          {
          }
        }
      }
      return (int.Parse(s) + 1).ToString();
    }

    public static int TicketSave(
      AccountTransaction objAccountTransaction,
      IDCubeRepository<Ticket> TicketRepository,
      string TicketNo,
      DateTime CurrentDate,
      int iAccountTransactionDocument)
    {
      string shortDateString = CurrentDate.ToShortDateString();
      CurrentDate.Year.ToString();
      string str1 = CurrentDate.Month.ToString();
      string str2 = CurrentDate.Day.ToString();
      if (str1.Length == 1)
      {
        string str3 = "0" + str1;
      }
      if (str2.Length == 1)
      {
        string str4 = "0" + str2;
      }
      string NDate = NepalitoEnglishDate.NepaliDate(DateTime.Parse(shortDateString));
      DateTime dateTime = NepalitoEnglishDate.EnglishDate(NDate);
      string nepaliMonth = NepalitoEnglish.englishToNepaliMonth(dateTime.Year, dateTime.Month, dateTime.Day);
      int num;
      try
      {
        TicketRepository.Insert(new Ticket()
        {
          Name = objAccountTransaction.Name,
          LastUpdateTime = CurrentDate,
          TicketNumber = TicketNo,
          Date = dateTime,
          LastOrderDate = CurrentDate,
          LastPaymentDate = CurrentDate,
          IsClosed = true,
          IsLocked = true,
          RemainingAmount = 0M,
          TotalAmount = objAccountTransaction.GrandAmount,
          DepartmentId = 1,
          TicketTypeId = 3,
          Table_Customer_Room = objAccountTransaction.SourceAccountTypeId,
          TicketStates = objAccountTransaction.Name,
          ExchangeRate = 1M,
          TaxIncluded = true,
          Printed_Time = DateTime.Now,
          AccountTransactionDocumentId = iAccountTransactionDocument,
          LastModifiedUserName = objAccountTransaction.UserName,
          FinancialYear = objAccountTransaction.FinancialYear,
          NepaliMonth = nepaliMonth,
          NVDate = NDate,
          WarehouseId = objAccountTransaction.WareHouseId,
          BranchCode = objAccountTransaction.BranchId
        });
        TicketRepository.Save();
        num = TicketRepository.GetAllData().ToList<Ticket>().LastOrDefault<Ticket>().Id;
      }
      catch (Exception ex)
      {
        num = 0;
      }
      return num;
    }

    public static int TicketUpdate(
      AccountTransaction objAccountTransaction,
      IDCubeRepository<Ticket> TicketRepository,
      string TicketNo,
      string CurrentDate)
    {
      string NDate = CurrentDate;
      DateTime dateTime = NepalitoEnglishDate.EnglishDate(NDate);
      string nepaliMonth = NepalitoEnglish.englishToNepaliMonth(dateTime.Year, dateTime.Month, dateTime.Day);
      int num;
      try
      {
        Ticket ticket1 = new Ticket();
        Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>) (o => o.TicketNumber == TicketNo)).FirstOrDefault<Ticket>();
        ticket2.Name = objAccountTransaction.Name;
        ticket2.LastUpdateTime = DateTime.Now;
        ticket2.TicketNumber = TicketNo;
        ticket2.Date = dateTime;
        ticket2.LastOrderDate = DateTime.Now;
        ticket2.LastPaymentDate = DateTime.Now;
        ticket2.IsClosed = true;
        ticket2.IsLocked = true;
        ticket2.RemainingAmount = 0M;
        ticket2.TotalAmount = objAccountTransaction.GrandAmount;
        ticket2.DepartmentId = 1;
        ticket2.TicketTypeId = 3;
        ticket2.Table_Customer_Room = 0;
        ticket2.TicketStates = "Sales";
        ticket2.ExchangeRate = 1M;
        ticket2.TaxIncluded = true;
        ticket2.Printed_Time = DateTime.Now;
        ticket2.LastModifiedUserName = objAccountTransaction.UserName;
        ticket2.FinancialYear = objAccountTransaction.FinancialYear;
        ticket2.NepaliMonth = nepaliMonth;
        ticket2.NVDate = NDate;
        TicketRepository.Update(ticket2);
        TicketRepository.Save();
        num = TicketRepository.GetAllData().ToList<Ticket>().LastOrDefault<Ticket>().Id;
      }
      catch (Exception ex)
      {
        num = 0;
      }
      return num;
    }

    public static List<TicketReference> GetCustomerTicket(
      int CustomerId,
      IDCubeRepository<Ticket> TicketRepository,
      IDCubeRepository<FinancialYear> FinancialYearRepository,
      string financialyear)
    {
      List<Ticket> ticketList = new List<Ticket>();
      List<TicketReference> customerTicket = new List<TicketReference>();
      FinancialYear objFinancialYear = FinancialYearRepository.GetAllData().Where<FinancialYear>((Func<FinancialYear, bool>) (o => o.Name == financialyear)).FirstOrDefault<FinancialYear>();
      try
      {
        if (CustomerId != 0)
        {
          foreach (Ticket ticket in TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>) (o => o.Table_Customer_Room == CustomerId && o.Date >= objFinancialYear.StartDate && o.Date <= objFinancialYear.EndDate)).ToList<Ticket>())
            customerTicket.Add(new TicketReference()
            {
              Id = int.Parse(ticket.TicketNumber),
              Name = ticket.TicketNumber
            });
        }
      }
      catch (Exception ex)
      {
      }
      return customerTicket;
    }

    public static Decimal TotalItemSale(
      IDCubeRepository<Order> OrderRepository,
      IDCubeRepository<FinancialYear> FinancialYearRepository,
      string financialyear)
    {
      FinancialYear objFinancialYear = FinancialYearRepository.GetAllData().Where<FinancialYear>((Func<FinancialYear, bool>) (o => o.Name == financialyear)).FirstOrDefault<FinancialYear>();
      return OrderRepository.GetAllData().ToList<Order>().Where<Order>((Func<Order, bool>) (o => o.CreatedDateTime >= objFinancialYear.StartDate && o.CreatedDateTime <= objFinancialYear.EndDate)).ToList<Order>().Select(o => new
      {
        TotalQuantity = o.Quantity
      }).Sum(o => o.TotalQuantity);
    }

    public static Decimal TotalSale(
      IDCubeRepository<Order> OrderRepository,
      IDCubeRepository<FinancialYear> FinancialYearRepository,
      string financialyear)
    {
      FinancialYear objFinancialYear = FinancialYearRepository.GetAllData().Where<FinancialYear>((Func<FinancialYear, bool>) (o => o.Name == financialyear)).FirstOrDefault<FinancialYear>();
      return OrderRepository.GetAllData().ToList<Order>().Where<Order>((Func<Order, bool>) (o => o.CreatedDateTime >= objFinancialYear.StartDate && o.CreatedDateTime <= objFinancialYear.EndDate)).ToList<Order>().Select(o => new
      {
        TotalPrice = o.Quantity * o.Price
      }).Sum(o => o.TotalPrice);
    }

    public static Decimal TotalMonthSale(
      IDCubeRepository<Order> OrderRepository,
      IDCubeRepository<FinancialYear> FinancialYearRepository,
      string financialyear)
    {
      FinancialYear objFinancialYear = FinancialYearRepository.GetAllData().Where<FinancialYear>((Func<FinancialYear, bool>) (o => o.Name == financialyear)).FirstOrDefault<FinancialYear>();
      DateTime now = DateTime.Now;
      int year = now.Year;
      now = DateTime.Now;
      int month = now.Month;
      now = DateTime.Now;
      int day = now.Day;
      string sNepaliMonth = NepalitoEnglish.englishToNepaliMonth(year, month, day);
      return OrderRepository.GetAllData().Where<Order>((Func<Order, bool>) (o => o.CreatedDateTime >= objFinancialYear.StartDate && o.CreatedDateTime <= objFinancialYear.EndDate && o.NepaliMonth == sNepaliMonth)).ToList<Order>().Select(o => new
      {
        TotalPrice = o.Quantity * o.Price
      }).Sum(o => o.TotalPrice);
    }

        public static List<MonthSale> TotalListMonthSale(IDCubeRepository<Order> OrderRepository, string FinancialYear)
        {
            List<MonthSale> listMonthSale = new List<MonthSale>();
            try
            {
                var ListOrder = OrderRepository.GetAllData().ToList().Where(o => o.FinancialYear == FinancialYear).ToList().Select(o => new { o.NepaliMonth, TotalPrice = o.Quantity * o.Price });
                var ListOrderMonth = from line in ListOrder
                                     group line by line.NepaliMonth into g
                                     select new MonthSale
                                     {
                                         Month = g.First().NepaliMonth,
                                         Amount = g.Sum(pc => pc.TotalPrice)
                                     };
                foreach (var lom in ListOrderMonth)
                {
                    MonthSale objMonthSale = new MonthSale();
                    objMonthSale.Amount = lom.Amount;
                    objMonthSale.Month = lom.Month;
                    listMonthSale.Add(objMonthSale);
                }
            }
            catch
            {

            }
            return listMonthSale;
        }
        public static List<YearSale> TotalListYearSale(IDCubeRepository<Order> OrderRepository)
        {
            List<YearSale> listYearSale = new List<YearSale>();
            var ListOrder = OrderRepository.GetAllData().ToList().Select(o => new { o.FinancialYear, TotalPrice = o.Quantity * o.Price });
            var ListOrderMonth = from line in ListOrder
                                 group line by line.FinancialYear into g
                                 select new YearSale
                                 {
                                     Year = g.First().FinancialYear,
                                     Amount = g.Sum(pc => pc.TotalPrice)
                                 };
            foreach (var lom in ListOrderMonth)
            {
                YearSale objYearSale = new YearSale();
                objYearSale.Amount = lom.Amount;
                objYearSale.Year = lom.Year;
                listYearSale.Add(objYearSale);
            }
            return listYearSale;
        }
    }
}
