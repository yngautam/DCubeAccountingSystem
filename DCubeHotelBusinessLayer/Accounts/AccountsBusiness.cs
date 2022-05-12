using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Reservation;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.DCubeHotelAccount
{
  public static class AccountsBusiness
  {
    public static List<Account> GetAllParty(IDCubeRepository<Account> accountRepository)
    {
      List<Account> accountList = new List<Account>();
      return accountRepository.GetAllData().Where<Account>((Func<Account, bool>) (o => o.AccountTypeId == 6)).ToList<Account>();
    }

    public static List<Account> GetAllPartyAccount(
      IDCubeRepository<Account> accountRepository)
    {
      List<Account> accountList = new List<Account>();
      return accountRepository.GetAllData().Where<Account>((Func<Account, bool>) (o => o.AccountTypeId == 18 || o.AccountTypeId == 6)).ToList<Account>();
    }

    public static List<Account> GetAllCustomerAccount(
      IDCubeRepository<Account> accountRepository)
    {
      List<Account> accountList = new List<Account>();
      return accountRepository.GetAllData().Where<Account>((Func<Account, bool>) (o => o.AccountTypeId == 18)).ToList<Account>();
    }

    public static List<Account> GetAccount(IDCubeRepository<Account> accountRepository)
    {
      List<Account> accountList = new List<Account>();
      return accountRepository.GetAllData().Where<Account>((Func<Account, bool>) (o => o.AccountTypeId != 8 && o.AccountTypeId != 12 && o.AccountTypeId != 13 && o.AccountTypeId != 18)).ToList<Account>();
    }

    public static List<Account> GetAllAccountGeneral(
      IDCubeRepository<Account> accountRepository)
    {
      List<Account> accountList = new List<Account>();
      return accountRepository.GetAllData().Where<Account>((Func<Account, bool>) (o => o.AccountTypeId != 8 && o.AccountTypeId != 12 && o.AccountTypeId != 13)).ToList<Account>();
    }

    public static List<Account> GetAllAccount(
      IDCubeRepository<Account> accountRepository,
      string BankCash)
    {
      List<Account> accountList = new List<Account>();
      return accountRepository.GetAllData().Where<Account>((Func<Account, bool>) (o => o.AccountTypeId == 8 || o.AccountTypeId == 12 || o.AccountTypeId == 13)).ToList<Account>();
    }

    public static List<Account> GetAllAccountBankCash(
      IDCubeRepository<Account> accountRepository)
    {
      List<Account> accountList = new List<Account>();
      return accountRepository.GetAllData().Where<Account>((Func<Account, bool>) (o => o.AccountTypeId == 8 || o.AccountTypeId == 12 || o.AccountTypeId == 13)).ToList<Account>();
    }

    public static List<Customer> GetAllAccount(
      IDCubeRepository<Account> accountRepository)
    {
      List<Customer> allAccount = new List<Customer>();
      List<Account> accountList = new List<Account>();
      foreach (Account account in accountRepository.GetAllData().Where<Account>((Func<Account, bool>) (o => o.AccountTypeId == 18)).ToList<Account>())
        allAccount.Add(new Customer()
        {
          FirstName = account.Name,
          Title = "",
          MiddleName = "",
          LastName = "",
          Email = account.Email,
          MobileNumber = account.Telephone,
          Id = account.Id
        });
      return allAccount;
    }

    public static int Create(
      IDCubeRepository<Account> accountRepository,
      IDCubeRepository<ExceptionLog> exceptionRepository,
      Account value)
    {
      int num = 1;
      Account account = new Account();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          accountRepository.Insert(value);
          accountRepository.Save();
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

    public static int Update(
      IDCubeRepository<Account> accountRepository,
      IDCubeRepository<ExceptionLog> exceptionRepository,
      Account value,
      int Id)
    {
      int num = 1;
      Account account1 = new Account();
      if (accountRepository.GetAllData().Where<Account>((Func<Account, bool>) (o => o.Name == "Taxable Sales" && o.Name == "Non Taxable Sales" && o.Name == "Non Taxable Sales" && o.Name == "Taxable Purchase" && o.Name == "Non Taxable Purchase" && o.Name == "Excise Duty")).FirstOrDefault<Account>() != null)
        num = 0;
      else if (Id >= 1)
      {
        using (UnitOfWork unitOfWork = new UnitOfWork())
        {
          try
          {
            unitOfWork.StartTransaction();
            Account account2 = new Account();
            Account account3 = accountRepository.GetAllData().Where<Account>((Func<Account, bool>) (o => o.Id == Id)).FirstOrDefault<Account>();
            if (account3 != null)
            {
              account3.AcceptCard = value.AcceptCard;
              account3.AccountTypeId = value.AccountTypeId;
              account3.Address = value.Address;
              account3.AffectsStock = value.AffectsStock;
              account3.Agent = value.Agent;
              account3.AllowInMobile = value.AllowInMobile;
              account3.Amount = value.Amount;
              account3.Audited = value.Audited;
              account3.BankGuarentee = value.BankGuarentee;
              account3.BankName = value.BankName;
              account3.BranchId = value.BranchId;
              account3.City = value.City;
              account3.CreditDays = value.CreditDays;
              account3.CreditLimit = value.CreditLimit;
              account3.District = value.District;
              account3.DRCR = value.DRCR;
              account3.Email = value.Email;
              account3.ExciseDutyType = value.ExciseDutyType;
              account3.ExpireDate = value.ExpireDate;
              account3.ExpireMiti = value.ExpireMiti;
              account3.ForeignCurrencyId = value.ForeignCurrencyId;
              account3.ForPayRoll = value.ForPayRoll;
              account3.ForServiceTax = value.ForServiceTax;
              account3.GSTType = value.GSTType;
              account3.Id = Id;
              account3.IgnoreTDSExempt = value.IgnoreTDSExempt;
              account3.InterestOnBillWise = value.InterestOnBillWise;
              account3.InventoryValue = value.InventoryValue;
              account3.IsAbatementApplicable = value.IsAbatementApplicable;
              account3.IsAgent = value.IsAgent;
              account3.IsBillWiseOn = value.IsBillWiseOn;
              account3.IsCondensed = value.IsCondensed;
              account3.ISCostCentresOn = value.ISCostCentresOn;
              account3.IsExcise = value.IsExcise;
              account3.IsExempte = value.IsExempte;
              account3.IsFBTApplicable = value.IsFBTApplicable;
              account3.IsGSTApplicable = value.IsGSTApplicable;
              account3.IsInputCredit = value.IsInputCredit;
              account3.IsInterestOn = value.IsInterestOn;
              account3.IsTCSApplicable = value.IsTCSApplicable;
              account3.IsTDSApplicable = value.IsTDSApplicable;
              account3.IsVAT = value.IsVAT;
              account3.LedgerFBTCategory = value.LedgerFBTCategory;
              account3.MaintainBilByBill = value.MaintainBilByBill;
              account3.Name = value.Name;
              account3.OpeningBalance = value.OpeningBalance;
              account3.OverRideADVInterest = value.OverRideADVInterest;
              account3.OverRideInterest = value.OverRideInterest;
              account3.PanNo = value.PanNo;
              account3.RateofInterest = value.RateofInterest;
              account3.SecurityDeposit = value.SecurityDeposit;
              account3.ServiceCategory = value.ServiceCategory;
              account3.ShowInPaySlip = value.ShowInPaySlip;
              account3.SortPosition = value.SortPosition;
              account3.Street = value.Street;
              account3.TaxClassificationName = value.TaxClassificationName;
              account3.TaxRate = value.TaxRate;
              account3.TaxType = value.TaxType;
              account3.TDSDeducteeIsSpecialRate = value.TDSDeducteeIsSpecialRate;
              account3.TDSDeducteeType = value.TDSDeducteeType;
              account3.TDSRateName = value.TDSRateName;
              account3.Telephone = value.Telephone;
              account3.TraderLedNatureOfPurchase = value.TraderLedNatureOfPurchase;
              account3.UseForGratuity = value.UseForGratuity;
              account3.UseForVat = value.UseForVat;
              accountRepository.Update(account3);
              accountRepository.Save();
            }
          }
          catch (Exception ex)
          {
            ErrorLog.ErrorLogging(ex);
            num = 0;
          }
          unitOfWork.CommitTransaction();
        }
      }
      return num;
    }

    public static int Delete(IDCubeRepository<Account> accountRepository, int id)
    {
      int num = 1;
      Account account = new Account();
      if (accountRepository.GetAllData().Where<Account>((Func<Account, bool>) (o => o.Name == "Taxable Sales" || o.Name == "Non Taxable Sales" || o.Name == "Non Taxable Sales" || o.Name == "Taxable Purchase" || o.Name == "Non Taxable Purchase" || o.Name == "Excise Duty")).FirstOrDefault<Account>() != null)
      {
        num = 0;
      }
      else
      {
        using (UnitOfWork unitOfWork = new UnitOfWork())
        {
          try
          {
            unitOfWork.StartTransaction();
            accountRepository.Delete((object) id);
            accountRepository.Save();
          }
          catch (Exception ex)
          {
            ErrorLog.ErrorLogging(ex);
            num = 0;
          }
          unitOfWork.CommitTransaction();
        }
      }
      return num;
    }
  }
}
