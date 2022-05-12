using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.CBMSAPI;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.Accounts
{
  public class MaterializedViewBusiness
  {
    public static List<MaterializedView> getMaterializedView(
      IDCubeRepository<AccountTransaction> AccountTransactionRepository,
      IDCubeRepository<AccountType> AccountTypeRepository,
      IDCubeRepository<Account> AccountRepository,
      IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
      IDCubeRepository<Ticket> TicketRepository,
      IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
      IDCubeRepository<AccountTransactionDocument> AccountTransactionDocumentRepository,
      string Month,
      string FinancialYear)
    {
      List<MaterializedView> materializedView1 = new List<MaterializedView>();
      string accounttypename = "Sales";
      List<AccountTransactionValue> transactionValueList1 = new List<AccountTransactionValue>();
      List<AccountTransactionValue> transactionValueList2 = new List<AccountTransactionValue>();
      List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
      List<Ticket> ticketList = new List<Ticket>();
            List<Account> source = new List<Account>();
            source = AccountRepository.GetAllData().ToList<Account>();
      List<AccountTransactionDocument> transactionDocumentList = new List<AccountTransactionDocument>();
      List<AccountTransactionType> accountTransactionTypeList = new List<AccountTransactionType>();
      accountTransactionTypeList = AccountTransactionTypeRepository.GetAllData().ToList<AccountTransactionType>();
      List<AccountTransactionDocument> list1 = AccountTransactionDocumentRepository.GetAllData().Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>) (o => o.Name.Contains(accounttypename))).ToList<AccountTransactionDocument>();
      accountTransactionList = AccountTransactionRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>) (o => o.Name.Contains(accounttypename))).ToList<AccountTransaction>();
      List<Ticket> list2 = TicketRepository.GetAllData().ToList<Ticket>().Where<Ticket>((Func<Ticket, bool>) (o => o.FinancialYear == FinancialYear && o.NepaliMonth == Month)).ToList<Ticket>();
      List<AccountTransactionValue> list3 = AccountTransactionValueRepository.GetAllData().ToList<AccountTransactionValue>();
      foreach (Ticket ticket in list2)
      {
        try
        {
          string str = accounttypename + " [#" + ticket.TicketNumber + "]";
          AccountTransactionDocument objAccountTransactionDocument = list1.Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>) (o => o.Name.Contains(accounttypename))).FirstOrDefault<AccountTransactionDocument>();
          List<AccountTransactionValue> list4 = list3.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionDocumentId == objAccountTransactionDocument.Id)).ToList<AccountTransactionValue>();
          AccountTransactionValue ObjAccountTransactionValue = new AccountTransactionValue();
          ObjAccountTransactionValue = list4.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId != 3 && o.Debit > 0M)).FirstOrDefault<AccountTransactionValue>();
          MaterializedView materializedView2 = new MaterializedView();
          materializedView2.Bill_Date = ticket.NVDate;
          materializedView2.Bill_no = ticket.TicketNumber;
          if (ObjAccountTransactionValue != null)
          {
            Account account1 = new Account();
            Account account2 = source.Where<Account>((Func<Account, bool>) (o => o.Id == ObjAccountTransactionValue.AccountId)).FirstOrDefault<Account>();
            materializedView2.Customer_name = account2.Name;
            materializedView2.Customer_Pan = "-";
            materializedView2.Entered_By = ObjAccountTransactionValue.UserName;
            if (account2.PanNo != null)
              materializedView2.Customer_Pan = account2.PanNo;
            materializedView2.Printed_by = ObjAccountTransactionValue.UserName;
          }
          else
          {
            materializedView2.Customer_name = "-";
            materializedView2.Customer_Pan = "-";
            materializedView2.Entered_By = ObjAccountTransactionValue.UserName;
            materializedView2.Printed_by = ObjAccountTransactionValue.UserName;
          }
          materializedView2.Fiscal_Year = ticket.FinancialYear;
          materializedView2.IS_Bill_Printed = ticket.IS_Bill_Printed;
          materializedView2.Is_bill_Active = ticket.IS_Bill_Active;
          materializedView2.Is_realtime = ticket.Real_Time;
          materializedView2.Printed_Time = ticket.Printed_Time;
          materializedView2.Sync_with_IRD = ticket.Sync_With_IRD;
          foreach (AccountTransactionValue transactionValue in list4)
          {
            if (transactionValue.AccountId == 3)
              materializedView2.Discount = transactionValue.Debit;
            if (transactionValue.AccountId == 8)
              materializedView2.Tax_Amount = transactionValue.Credit;
            if (transactionValue.AccountId != 3 && transactionValue.Debit > 0M)
              materializedView2.Total_Amount = transactionValue.Debit;
          }
          materializedView1.Add(materializedView2);
        }
        catch (Exception ex)
        {
          string message = ex.Message;
        }
      }
      return materializedView1;
    }
  }
}
