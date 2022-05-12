using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using NepaliDate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.Accounts
{
  public static class AccountTransactionBusiness
  {
    public static AccountTransaction ScreenAccountTransaction(
      IDCubeRepository<AccountTransaction> accRepository,
      string TransactionId)
    {
      AccountTransaction accountTransaction = new AccountTransaction();
      return accRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>) (o => o.Id.ToString() == TransactionId)).FirstOrDefault<AccountTransaction>();
    }

    public static AccountScreenValue ScrenAccountTransaction(
      IDCubeRepository<AccountTransaction> accRepository,
      IDCubeRepository<AccountTransactionValue> accValueRepository,
      IDCubeRepository<Ticket> TicketRepository,
      IDCubeRepository<FinancialYear> FinancialYearRepository,
      string TransactionId)
    {
      AccountScreenValue accountScreenValue = new AccountScreenValue();
      AccountTransaction objAccountTransaction = new AccountTransaction();
      List<AccountTransactionValue> source = new List<AccountTransactionValue>();
      objAccountTransaction = accRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>) (o => o.Id.ToString() == TransactionId)).FirstOrDefault<AccountTransaction>();
      if (objAccountTransaction != null)
      {
        List<TicketReference> ticketReferenceList = new List<TicketReference>();
        List<TicketReference> customerTicket = TicketSaleBillingBusiness.GetCustomerTicket(objAccountTransaction.SourceAccountTypeId, TicketRepository, FinancialYearRepository, objAccountTransaction.FinancialYear);
        if (objAccountTransaction.Name.Substring(0, objAccountTransaction.Name.IndexOf("[")) == "Receipt Transaction ")
          source = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == objAccountTransaction.Id && o.Credit > 0M)).ToList<AccountTransactionValue>();
        if (objAccountTransaction.Name.Substring(0, objAccountTransaction.Name.IndexOf("[")) == "Receipt")
          source = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == objAccountTransaction.Id && o.Credit > 0M)).ToList<AccountTransactionValue>();
        if (objAccountTransaction.Name.Substring(0, objAccountTransaction.Name.IndexOf("[")) == "Payment")
          source = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == objAccountTransaction.Id && o.Debit > 0M)).ToList<AccountTransactionValue>();
        if (objAccountTransaction.Name.Substring(0, objAccountTransaction.Name.IndexOf("[")) == "Journal")
          source = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == objAccountTransaction.Id)).ToList<AccountTransactionValue>();
        if (objAccountTransaction.Name.Substring(0, objAccountTransaction.Name.IndexOf("[")) == "Debit Note")
          source = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == objAccountTransaction.Id)).ToList<AccountTransactionValue>();
        if (objAccountTransaction.Name.Substring(0, objAccountTransaction.Name.IndexOf("[")) == "Credit Note")
          source = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == objAccountTransaction.Id && o.Debit > 0M)).ToList<AccountTransactionValue>();
        if (objAccountTransaction.Name.Substring(0, objAccountTransaction.Name.IndexOf("[")) == "Contra")
          source = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == objAccountTransaction.Id && o.Debit > 0M)).ToList<AccountTransactionValue>();
        accountScreenValue.AccountTransactionDocumentId = objAccountTransaction.AccountTransactionDocumentId;
        accountScreenValue.AccountTransactionType = objAccountTransaction.Name.Substring(0, objAccountTransaction.Name.IndexOf("["));
        accountScreenValue.AccountTransactionTypeId = objAccountTransaction.AccountTransactionTypeId;
        accountScreenValue.AccountTransactionValues = (IEnumerable<AccountTransactionValue>) source;
        accountScreenValue.Amount = objAccountTransaction.Amount.ToString();
        accountScreenValue.CreditAmount = source.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Credit));
        accountScreenValue.crTotal = source.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Credit)).ToString();
        accountScreenValue.DebitAmount = source.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Debit));
        accountScreenValue.Description = objAccountTransaction.Description;
        accountScreenValue.drTotal = source.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Debit)).ToString();
        accountScreenValue.Id = objAccountTransaction.Id;
        accountScreenValue.IsReversed = objAccountTransaction.IsReversed;
        accountScreenValue.Name = objAccountTransaction.Name;
        accountScreenValue.Reversable = objAccountTransaction.Reversable;
        accountScreenValue.SourceAccountTypeId = objAccountTransaction.SourceAccountTypeId;
        accountScreenValue.TargetAccountTypeId = objAccountTransaction.TargetAccountTypeId;
        accountScreenValue.TicketReferences = (IEnumerable<TicketReference>) customerTicket;
        accountScreenValue.BranchId = objAccountTransaction.BranchId;
        accountScreenValue.CompanyCode = objAccountTransaction.CompanyCode;
        accountScreenValue.DepartmentId = objAccountTransaction.DepartmentId;
        accountScreenValue.WareHouseId = objAccountTransaction.WareHouseId;
        accountScreenValue.ref_invoice_number = objAccountTransaction.ref_invoice_number == null ? string.Empty : objAccountTransaction.ref_invoice_number;
      }
      return accountScreenValue;
    }

    public static List<AccountScreen> GetScrenAccountTransaction(
      IDCubeRepository<AccountType> AccountTypeRepository,
      IDCubeRepository<Account> AccountRepository,
      IDCubeRepository<AccountTransaction> accRepository,
      IDCubeRepository<AccountTransactionValue> accValueRepository,
      string FromDate,
      string ToDate,
      int TransactionTypeId,
      int BranchId)
    {
      DateTime sFormDate = NepalitoEnglishDate.EnglishDate(FromDate);
      DateTime sToDate = NepalitoEnglishDate.EnglishDate(ToDate);
      List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
      List<AccountTransaction> list1 = accRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>) (o => o.AccountTransactionTypeId == TransactionTypeId)).ToList<AccountTransaction>();
      if (BranchId != 0)
        list1 = list1.Where<AccountTransaction>((Func<AccountTransaction, bool>) (o => o.BranchId == BranchId)).ToList<AccountTransaction>();
      List<AccountType> accountTypeList = new List<AccountType>();
      accountTypeList = AccountTypeRepository.GetAllData().ToList<AccountType>();
      List<Account> accountList = new List<Account>();
      List<Account> list2 = AccountRepository.GetAllData().ToList<Account>();
      List<AccountTransactionValue> transactionValueList1 = new List<AccountTransactionValue>();
      List<AccountTransactionValue> list3 = accValueRepository.GetAllData().ToList<AccountTransactionValue>().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.Date.Date >= sFormDate && o.Date.Date <= sToDate)).ToList<AccountTransactionValue>();
      if (BranchId != 0)
        list3 = list3.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.BranchId == BranchId)).ToList<AccountTransactionValue>();
      var source = list3.Join((IEnumerable<AccountTransaction>) list1, (Func<AccountTransactionValue, int>) (v => v.AccountTransactionId), (Func<AccountTransaction, int>) (atr => atr.Id), (v, atr) => new
      {
        v = v,
        atr = atr
      }).Join((IEnumerable<Account>) list2, _param1 => _param1.v.AccountId, (Func<Account, int>) (a => a.Id), (_param1, a) => new
      {
        Id = _param1.v.Id,
        Name = a.Name,
        AccountTransactionId = _param1.v.AccountTransactionId,
        VoucherNo = _param1.atr.Name.Substring(_param1.atr.Name.IndexOf("[")),
        AccountTransactionType = _param1.atr.Name.Substring(0, _param1.atr.Name.IndexOf("[")),
        Amount = _param1.atr.Amount,
        SourceAccountTypeId = _param1.atr.SourceAccountTypeId,
        Description = _param1.atr.Description,
        DebitAmount = _param1.v.Debit,
        CreditAmount = _param1.v.Credit,
        NVDate = _param1.v.NVDate,
        PhoteIdentity = _param1.atr.PhoteIdentity
      });
      var list4 = source.Select(o => new
      {
        AccountTransactionId = o.AccountTransactionId
      }).Distinct().ToList().GroupBy(a => a.AccountTransactionId).Select(g => g.First()).ToList();
      List<AccountScreen> accountTransaction = new List<AccountScreen>();
      foreach (var data1 in list4)
      {
        var accounttransaction = data1;
        var objaccounttransaction = source.Where(o => o.AccountTransactionId == accounttransaction.AccountTransactionId).FirstOrDefault();
        AccountScreen accountScreen = new AccountScreen();
        accountScreen.Id = accounttransaction.AccountTransactionId;
        accountScreen.VDate = objaccounttransaction.NVDate;
        accountScreen.VType = objaccounttransaction.AccountTransactionType;
        accountScreen.VoucherNo = objaccounttransaction.VoucherNo;
        accountScreen.IdentityFile = objaccounttransaction.PhoteIdentity != null;
        var datas = source.Where(o => o.VoucherNo == objaccounttransaction.VoucherNo);
        List<AccountScreenValue> accountScreenValueList = new List<AccountScreenValue>();
        foreach (var data2 in datas)
        {
          var screenvalue = data2;
          AccountScreenValue accountScreenValue = new AccountScreenValue();
          accountScreenValue.AccountTransactionType = screenvalue.AccountTransactionType;
          accountScreenValue.Amount = screenvalue.Amount.ToString();
          accountScreenValue.CreditAmount = screenvalue.CreditAmount;
          accountScreenValue.DebitAmount = screenvalue.DebitAmount;
          accountScreenValue.Description = screenvalue.Description;
          accountScreenValue.Id = screenvalue.Id;
          accountScreenValue.Name = screenvalue.Name;
          accountScreenValue.SourceAccountTypeId = screenvalue.SourceAccountTypeId;
          List<AccountTransactionValue> transactionValueList2 = new List<AccountTransactionValue>();
          List<AccountTransactionValue> list5 = list3.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == screenvalue.Id)).ToList<AccountTransactionValue>();
          accountScreenValue.AccountTransactionValues = (IEnumerable<AccountTransactionValue>) list5;
          accountScreenValueList.Add(accountScreenValue);
        }
        accountScreen.AccountTransactionValues = (IList<AccountScreenValue>) accountScreenValueList;
        accountTransaction.Add(accountScreen);
      }
      return accountTransaction;
    }

    public static int Create(
      IDCubeRepository<Account> AccountRepository,
      IDCubeRepository<AccountTransaction> accRepository,
      IDCubeRepository<AccountTransactionType> accTransTypeRepository,
      IDCubeRepository<AccountTransactionValue> accValueRepository,
      IDCubeRepository<AccountTransactionDocument> accTransDocRepository,
      AccountTransaction value)
    {
      string date = value.Date;
      DateTime dateTime = NepalitoEnglishDate.EnglishDate(date);
      string nepaliMonth = NepalitoEnglish.englishToNepaliMonth(dateTime.Year, dateTime.Month, dateTime.Day);
      int num1 = 0;
      AccountTransaction accountTransaction1 = new AccountTransaction();
      DateTime now = DateTime.Now;
      string empty = string.Empty;
      int num2 = 0;
      List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
      List<AccountTransactionType> accountTransactionTypeList = new List<AccountTransactionType>();
      List<AccountTransactionType> list2 = accTransTypeRepository.GetAllData().ToList<AccountTransactionType>();
      AccountTransactionType accountTransactionType1 = new AccountTransactionType();
      AccountTransactionType accountTransactionType2 = list2.Find((Predicate<AccountTransactionType>) (o => o.Name.Contains(value.Name)));
      List<AccountTransactionDocument> transactionDocumentList = new List<AccountTransactionDocument>();
      List<AccountTransactionDocument> list3 = accTransDocRepository.GetAllData().ToList<AccountTransactionDocument>();
      int num3;
      if (list3.Count == 0)
      {
        num3 = num2 + 1;
      }
      else
      {
        List<AccountTransactionDocument> list4 = list3.Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>) (o => o.Name.Contains(value.Name))).ToList<AccountTransactionDocument>();
        if (list4.Count == 0)
        {
          num3 = num2 + 1;
        }
        else
        {
          AccountTransactionDocument transactionDocument1 = new AccountTransactionDocument();
          AccountTransactionDocument transactionDocument2 = list4.OrderByDescending<AccountTransactionDocument, int>((Func<AccountTransactionDocument, int>) (o => o.Id)).FirstOrDefault<AccountTransactionDocument>();
          num3 = int.Parse(transactionDocument2.Name.Substring(transactionDocument2.Name.IndexOf("[#")).Replace("]", "").Replace("[#", "")) + 1;
        }
      }
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        unitOfWork.StartTransaction();
        try
        {
          string str = accountTransactionType2.Name + " [#" + (object) num3 + "]";
          accTransDocRepository.Insert(new AccountTransactionDocument()
          {
            Date = now,
            DocumentTypeId = 0,
            Name = str,
            Printed_Time = DateTime.Now
          });
          accTransDocRepository.Save();
          int id = accTransDocRepository.GetAllData().ToList<AccountTransactionDocument>().Last<AccountTransactionDocument>().Id;
          AccountTransaction accountTransaction2 = new AccountTransaction();
          accountTransaction2.AccountTransactionDocumentId = id;
          if (value.Name == "Receipt")
            accountTransaction2.SourceAccountTypeId = value.SourceAccountTypeId;
          if (value.Name == "Payment")
            accountTransaction2.SourceAccountTypeId = value.SourceAccountTypeId;
          if (value.Name == "Contra")
            accountTransaction2.SourceAccountTypeId = value.SourceAccountTypeId;
          if (value.Name == "Journal")
            accountTransaction2.SourceAccountTypeId = accountTransactionType2.SourceAccountTypeId;
          if (value.Name == "Credit Note")
            accountTransaction2.SourceAccountTypeId = value.SourceAccountTypeId;
          accountTransaction2.Name = value.Name + "[#" + (object) num3 + "]";
          accountTransaction2.Amount = value.AccountTransactionValues.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId != 0)).Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Credit));
          accountTransaction2.Description = value.Description;
          accountTransaction2.ExchangeRate = value.ExchangeRate;
          accountTransaction2.AccountTransactionTypeId = accountTransactionType2.Id;
          accountTransaction2.TargetAccountTypeId = accountTransactionType2.TargetAccountTypeId;
          accountTransaction2.Date = dateTime.ToShortDateString();
          accountTransaction2.ref_invoice_number = value.ref_invoice_number;
          accountTransaction2.IsReversed = false;
          accountTransaction2.Reversable = true;
          accountTransaction2.Reversable = true;
          accountTransaction2.Printed_Time = DateTime.Now;
          accountTransaction2.FinancialYear = value.FinancialYear;
          accountTransaction2.UserName = value.UserName;
          accountTransaction2.BranchId = value.BranchId;
          accountTransaction2.DepartmentId = value.DepartmentId;
          accountTransaction2.CompanyCode = value.CompanyCode;
          accountTransaction2.WareHouseId = value.WareHouseId;
          accRepository.Insert(accountTransaction2);
          accRepository.Save();
          num1 = accRepository.GetAllData().OrderByDescending<AccountTransaction, int>((Func<AccountTransaction, int>) (x => x.Id)).FirstOrDefault<AccountTransaction>().Id;
          foreach (AccountTransactionValue transactionValue in value.AccountTransactionValues)
          {
            AccountTransactionValue objAccountTrans = transactionValue;
            if (objAccountTrans.AccountId != 0)
            {
              accValueRepository.Insert(new AccountTransactionValue()
              {
                AccountTransactionId = num1,
                AccountTransactionDocumentId = id,
                Credit = objAccountTrans.Credit,
                Debit = objAccountTrans.Debit,
                entityLists = objAccountTrans.entityLists,
                AccountId = objAccountTrans.AccountId,
                Name = value.Name + "[#" + (object) num3 + "]",
                ref_invoice_number = value.ref_invoice_number,
                Description = objAccountTrans.Description,
                AccountTypeId = list1.Find((Predicate<Account>) (o => o.Id == objAccountTrans.AccountId)).AccountTypeId,
                Date = dateTime,
                NVDate = date,
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
          }
          if (value.Name == "Receipt")
          {
            accValueRepository.Insert(new AccountTransactionValue()
            {
              AccountTransactionId = num1,
              AccountTransactionDocumentId = id,
              Credit = 0M,
              Debit = value.AccountTransactionValues.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId != 0)).Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Credit)),
              entityLists = "Dr",
              AccountId = value.SourceAccountTypeId,
              Name = value.Name + "[#" + (object) num3 + "]",
              ref_invoice_number = value.ref_invoice_number,
              AccountTypeId = list1.Find((Predicate<Account>) (o => o.Id == value.SourceAccountTypeId)).AccountTypeId,
              Description = value.Description,
              Date = dateTime,
              Printed_Time = DateTime.Now,
              NVDate = date,
              FinancialYear = value.FinancialYear,
              NepaliMonth = nepaliMonth,
              UserName = value.UserName,
              BranchId = value.BranchId,
              DepartmentId = value.DepartmentId,
              CompanyCode = value.CompanyCode,
              WareHouseId = value.WareHouseId
            });
            accValueRepository.Save();
          }
          if (value.Name == "Payment")
          {
            accValueRepository.Insert(new AccountTransactionValue()
            {
              AccountTransactionId = num1,
              AccountTransactionDocumentId = id,
              Credit = value.AccountTransactionValues.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId != 0)).Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Debit)),
              Debit = 0M,
              entityLists = "Cr",
              AccountId = value.SourceAccountTypeId,
              Name = value.Name + "[#" + (object) num3 + "]",
              ref_invoice_number = value.ref_invoice_number,
              AccountTypeId = list1.Find((Predicate<Account>) (o => o.Id == value.SourceAccountTypeId)).AccountTypeId,
              Description = value.Description,
              Date = dateTime,
              Printed_Time = DateTime.Now,
              NVDate = date,
              FinancialYear = value.FinancialYear,
              NepaliMonth = nepaliMonth,
              UserName = value.UserName,
              BranchId = value.BranchId,
              DepartmentId = value.DepartmentId,
              CompanyCode = value.CompanyCode,
              WareHouseId = value.WareHouseId
            });
            accValueRepository.Save();
          }
          if (value.Name == "Contra")
          {
            accValueRepository.Insert(new AccountTransactionValue()
            {
              AccountTransactionId = num1,
              AccountTransactionDocumentId = id,
              Credit = value.AccountTransactionValues.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId != 0)).Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Debit)),
              Debit = 0M,
              entityLists = "Cr",
              AccountId = value.SourceAccountTypeId,
              Name = value.Name + "[#" + (object) num3 + "]",
              ref_invoice_number = value.ref_invoice_number,
              AccountTypeId = list1.Find((Predicate<Account>) (o => o.Id == value.SourceAccountTypeId)).AccountTypeId,
              Description = value.Description,
              Date = dateTime,
              Printed_Time = DateTime.Now,
              NVDate = date,
              FinancialYear = value.FinancialYear,
              NepaliMonth = nepaliMonth,
              UserName = value.UserName,
              BranchId = value.BranchId,
              DepartmentId = value.DepartmentId,
              CompanyCode = value.CompanyCode,
              WareHouseId = value.WareHouseId
            });
            accValueRepository.Save();
          }
          if (value.Name == "Credit Note")
          {
            accValueRepository.Insert(new AccountTransactionValue()
            {
              AccountTransactionId = num1,
              AccountTransactionDocumentId = id,
              Credit = value.AccountTransactionValues.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId != 0)).Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Debit)),
              Debit = 0M,
              entityLists = "Cr",
              AccountId = value.SourceAccountTypeId,
              Name = value.Name + "[#" + (object) num3 + "]",
              AccountTypeId = list1.Find((Predicate<Account>) (o => o.Id == value.SourceAccountTypeId)).AccountTypeId,
              Description = value.Description,
              Date = dateTime,
              Printed_Time = DateTime.Now,
              ref_invoice_number = value.ref_invoice_number,
              NVDate = date,
              FinancialYear = value.FinancialYear,
              NepaliMonth = nepaliMonth,
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
        catch (Exception ex)
        {
          unitOfWork.RollBackTransaction();
          ErrorLog.ErrorLogging(ex);
          num1 = 0;
        }
      }
      return num1;
    }

    public static int Edit(
      IDCubeRepository<Account> AccountRepository,
      IDCubeRepository<AccountTransaction> accRepository,
      IDCubeRepository<AccountTransactionValue> accValueRepository,
      AccountTransaction value)
    {
      string date = value.Date;
      DateTime dateTime = NepalitoEnglishDate.EnglishDate(date);
      string nepaliMonth = NepalitoEnglish.englishToNepaliMonth(dateTime.Year, dateTime.Month, dateTime.Day);
      List<Account> list = AccountRepository.GetAllData().ToList<Account>();
      int num = value.Id;
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        unitOfWork.StartTransaction();
        try
        {
          AccountTransaction accountTransaction1 = new AccountTransaction();
          AccountTransaction accountTransaction2 = accRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>) (o => o.Id == value.Id)).FirstOrDefault<AccountTransaction>();
          if (accountTransaction2 != null)
          {
            accountTransaction2.AccountTransactionDocumentId = value.AccountTransactionDocumentId;
            accountTransaction2.SourceAccountTypeId = value.SourceAccountTypeId;
            accountTransaction2.Name = value.Name;
            accountTransaction2.Amount = value.AccountTransactionValues.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId != 0)).Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Debit));
            accountTransaction2.Description = value.Description;
            accountTransaction2.Date = dateTime.ToShortDateString();
            accountTransaction2.ref_invoice_number = value.ref_invoice_number;
            accountTransaction2.UserName = value.UserName;
            accountTransaction2.BranchId = value.BranchId;
            accountTransaction2.DepartmentId = value.DepartmentId;
            accountTransaction2.CompanyCode = value.CompanyCode;
            accountTransaction2.WareHouseId = value.WareHouseId;
            accRepository.Update(accountTransaction2);
            accRepository.Save();
          }
          foreach (AccountTransactionValue transactionValue1 in value.AccountTransactionValues)
          {
            AccountTransactionValue objAccountTrans = transactionValue1;
            if (objAccountTrans.Id > 0 && objAccountTrans.AccountId != 0)
            {
              AccountTransactionValue transactionValue2 = new AccountTransactionValue();
              AccountTransactionValue transactionValue3 = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.Id == objAccountTrans.Id)).FirstOrDefault<AccountTransactionValue>();
              if (transactionValue3 != null)
              {
                transactionValue3.AccountTransactionId = objAccountTrans.AccountTransactionId;
                transactionValue3.AccountTransactionDocumentId = objAccountTrans.AccountTransactionDocumentId;
                transactionValue3.Name = objAccountTrans.Name;
                transactionValue3.entityLists = objAccountTrans.entityLists;
                transactionValue3.AccountTypeId = list.Find((Predicate<Account>) (o => o.Id == objAccountTrans.AccountId)).AccountTypeId;
                transactionValue3.AccountId = objAccountTrans.AccountId;
                transactionValue3.Description = objAccountTrans.Description;
                transactionValue3.Date = dateTime;
                transactionValue3.ref_invoice_number = value.ref_invoice_number;
                transactionValue3.Debit = objAccountTrans.Debit;
                transactionValue3.Credit = objAccountTrans.Credit;
                transactionValue3.Exchange = objAccountTrans.Exchange;
                transactionValue3.UserName = value.UserName;
                transactionValue3.NVDate = date;
                transactionValue3.NepaliMonth = nepaliMonth;
                transactionValue3.BranchId = value.BranchId;
                transactionValue3.DepartmentId = value.DepartmentId;
                transactionValue3.CompanyCode = value.CompanyCode;
                transactionValue3.WareHouseId = value.WareHouseId;
                accValueRepository.Update(transactionValue3);
                accValueRepository.Save();
              }
            }
            else if (objAccountTrans.Id == 0 && objAccountTrans.AccountId != 0)
            {
              accValueRepository.Insert(new AccountTransactionValue()
              {
                AccountTransactionId = value.Id,
                AccountTransactionDocumentId = value.AccountTransactionDocumentId,
                Name = value.Name,
                entityLists = objAccountTrans.entityLists,
                AccountTypeId = list.Find((Predicate<Account>) (o => o.Id == objAccountTrans.AccountId)).AccountTypeId,
                AccountId = objAccountTrans.AccountId,
                Description = objAccountTrans.Description,
                ref_invoice_number = objAccountTrans.ref_invoice_number,
                Date = dateTime,
                Debit = objAccountTrans.Debit,
                Credit = objAccountTrans.Credit,
                Exchange = objAccountTrans.Exchange,
                Printed_Time = DateTime.Now,
                NVDate = date,
                FinancialYear = value.FinancialYear,
                NepaliMonth = nepaliMonth,
                UserName = value.UserName,
                BranchId = value.BranchId,
                DepartmentId = value.DepartmentId,
                CompanyCode = value.CompanyCode,
                WareHouseId = value.WareHouseId
              });
              accValueRepository.Save();
            }
          }
          value.Name.Substring(0, value.Name.IndexOf("[")).Replace("[", "");
          if (value.Name.Substring(0, value.Name.IndexOf("[")).Replace("[", "") == "Receipt Transaction ")
          {
            new AccountTransactionValue().AccountId = value.SourceAccountTypeId;
            AccountTransactionValue transactionValue = accValueRepository.GetAllData().ToList<AccountTransactionValue>().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == value.Id && o.Debit > 0M)).FirstOrDefault<AccountTransactionValue>();
            transactionValue.Debit = value.AccountTransactionValues.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId != 0)).Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Credit));
            transactionValue.NVDate = date;
            transactionValue.NepaliMonth = nepaliMonth;
            transactionValue.UserName = value.UserName;
            transactionValue.BranchId = value.BranchId;
            transactionValue.DepartmentId = value.DepartmentId;
            transactionValue.CompanyCode = value.CompanyCode;
            transactionValue.WareHouseId = value.WareHouseId;
            accValueRepository.Update(transactionValue);
            accValueRepository.Save();
          }
          if (value.Name.Substring(0, value.Name.IndexOf("[")).Replace("[", "") == "Receipt")
          {
            new AccountTransactionValue().AccountId = value.SourceAccountTypeId;
            AccountTransactionValue transactionValue = accValueRepository.GetAllData().ToList<AccountTransactionValue>().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == value.Id && o.Debit > 0M)).FirstOrDefault<AccountTransactionValue>();
            transactionValue.Debit = value.AccountTransactionValues.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId != 0)).Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Credit));
            transactionValue.NVDate = date;
            transactionValue.NepaliMonth = nepaliMonth;
            transactionValue.UserName = value.UserName;
            transactionValue.BranchId = value.BranchId;
            transactionValue.DepartmentId = value.DepartmentId;
            transactionValue.CompanyCode = value.CompanyCode;
            transactionValue.WareHouseId = value.WareHouseId;
            accValueRepository.Update(transactionValue);
            accValueRepository.Save();
          }
          if (value.Name.Substring(0, value.Name.IndexOf("[")).Replace("[", "") == "Payment")
          {
            new AccountTransactionValue().AccountId = value.SourceAccountTypeId;
            AccountTransactionValue transactionValue = accValueRepository.GetAllData().ToList<AccountTransactionValue>().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == value.Id && o.Credit > 0M)).FirstOrDefault<AccountTransactionValue>();
            transactionValue.Credit = value.AccountTransactionValues.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId != 0)).Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Debit));
            transactionValue.NVDate = date;
            transactionValue.NepaliMonth = nepaliMonth;
            transactionValue.UserName = value.UserName;
            transactionValue.BranchId = value.BranchId;
            transactionValue.DepartmentId = value.DepartmentId;
            transactionValue.CompanyCode = value.CompanyCode;
            transactionValue.WareHouseId = value.WareHouseId;
            accValueRepository.Update(transactionValue);
            accValueRepository.Save();
          }
          if (value.Name.Substring(0, value.Name.IndexOf("[")).Replace("[", "") == "Contra")
          {
            new AccountTransactionValue().AccountId = value.SourceAccountTypeId;
            AccountTransactionValue transactionValue = accValueRepository.GetAllData().ToList<AccountTransactionValue>().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == value.Id && o.Credit > 0M)).FirstOrDefault<AccountTransactionValue>();
            transactionValue.Credit = value.AccountTransactionValues.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId != 0)).Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Debit));
            transactionValue.NVDate = date;
            transactionValue.NepaliMonth = nepaliMonth;
            transactionValue.UserName = value.UserName;
            transactionValue.BranchId = value.BranchId;
            transactionValue.DepartmentId = value.DepartmentId;
            transactionValue.CompanyCode = value.CompanyCode;
            transactionValue.WareHouseId = value.WareHouseId;
            accValueRepository.Update(transactionValue);
            accValueRepository.Save();
          }
          if (value.Name.Substring(0, value.Name.IndexOf("[")).Replace("[", "") == "Credit Note")
          {
            new AccountTransactionValue().AccountId = value.SourceAccountTypeId;
            AccountTransactionValue transactionValue = accValueRepository.GetAllData().ToList<AccountTransactionValue>().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionId == value.Id && o.Credit > 0M)).FirstOrDefault<AccountTransactionValue>();
            transactionValue.Credit = value.AccountTransactionValues.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountId != 0)).Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>) (o => o.Debit));
            transactionValue.ref_invoice_number = value.ref_invoice_number;
            transactionValue.NVDate = date;
            transactionValue.NepaliMonth = nepaliMonth;
            transactionValue.UserName = value.UserName;
            transactionValue.BranchId = value.BranchId;
            transactionValue.DepartmentId = value.DepartmentId;
            transactionValue.CompanyCode = value.CompanyCode;
            transactionValue.WareHouseId = value.WareHouseId;
            accValueRepository.Update(transactionValue);
            accValueRepository.Save();
          }
          unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
          unitOfWork.RollBackTransaction();
          ErrorLog.ErrorLogging(ex);
          num = 0;
        }
      }
      return num;
    }

    public static int Edit(
      IDCubeRepository<AccountTransaction> accRepository,
      AccountTransaction value)
    {
      int num = 0;
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        unitOfWork.StartTransaction();
        try
        {
          unitOfWork.AccountTransactionRepository.UpdateTransactionReferenceDocument(value);
          num = value.Id;
          unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
          unitOfWork.RollBackTransaction();
          ErrorLog.ErrorLogging(ex);
          num = 0;
        }
      }
      return num;
    }

    public static int Delete(
      IDCubeRepository<AccountTransactionValue> accValueRepository,
      IDCubeRepository<AccountTransaction> accRepository,
      IDCubeRepository<AccountTransactionDocument> accTransDocRepository,
      IDCubeRepository<AccountType> AccountTypeRepository,
      AccountTransaction value)
    {
      int num = 1;
      List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        unitOfWork.StartTransaction();
        try
        {
          AccountType accountType = new AccountType();
          accountType = AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>) (o => o.Id == 2)).FirstOrDefault<AccountType>();
          AccountTransactionDocument transactionDocument = accTransDocRepository.GetAllData().Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>) (o => o.Id == value.AccountTransactionDocumentId)).FirstOrDefault<AccountTransactionDocument>();
          if (transactionDocument.Name.Substring(0, transactionDocument.Name.IndexOf("[")).Replace("[", "").Trim() == "Sales Accounts" && value.Name.Substring(0, value.Name.IndexOf("[")).Replace("[", "").Trim() == "Receipt Transaction")
          {
            List<AccountTransaction> list = accRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>) (o => o.AccountTransactionDocumentId == value.AccountTransactionDocumentId)).ToList<AccountTransaction>();
            if (list != null)
            {
              AccountTransaction accountTransaction1 = new AccountTransaction();
              AccountTransaction accountTransaction2 = list.Where<AccountTransaction>((Func<AccountTransaction, bool>) (o => o.Name.Substring(0, o.Name.IndexOf("[")).Replace("[", "").Trim() == "Receipt Transaction")).FirstOrDefault<AccountTransaction>();
              if (accountTransaction2 != null)
              {
                accRepository.Delete((object) accountTransaction2.Id);
                accRepository.Save();
              }
            }
          }
          else
          {
            accTransDocRepository.Delete((object) value.AccountTransactionDocumentId);
            accTransDocRepository.Save();
          }
          List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
          foreach (AccountTransactionValue transactionValue in accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.AccountTransactionDocumentId == value.AccountTransactionDocumentId)).ToList<AccountTransactionValue>())
          {
            accValueRepository.Delete((object) transactionValue.Id);
            accValueRepository.Save();
          }
          unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
          unitOfWork.RollBackTransaction();
          throw ex;
        }
        return num;
      }
    }

    public static int DeleteAccountTransValues(
      IDCubeRepository<AccountTransactionValue> accValueRepository,
      int Id)
    {
      int num = 1;
      try
      {
        accValueRepository.Delete((object) Id);
        accValueRepository.Save();
      }
      catch (Exception ex)
      {
        ErrorLog.ErrorLogging(ex);
        num = 0;
      }
      return num;
    }
  }
}
