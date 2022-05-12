using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using NepaliDate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.Purchases
{
    public static class PurchaseBusiness
    {
        public static AccountScreenValue ScrenAccountTransaction(
          IDCubeRepository<AccountTransaction> accRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          IDCubeRepository<PurchaseDetails> purchaseDetailsRepository,
          IDCubeRepository<MenuItemPortion> menuportionrepository,
          string TransactionId)
        {
            AccountScreenValue accountScreenValue = new AccountScreenValue();
            AccountTransaction objAccountTransaction = new AccountTransaction();
            List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
            objAccountTransaction = accRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Id.ToString() == TransactionId)).FirstOrDefault<AccountTransaction>();
            if (objAccountTransaction != null)
            {
                List<PurchaseDetails> source = new List<PurchaseDetails>();
                try
                {
                    source = purchaseDetailsRepository.GetAllData().ToList<PurchaseDetails>();
                }
                catch (Exception ex)
                {
                }
                List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
                List<MenuItemPortion> list1 = menuportionrepository.GetAllData().ToList<MenuItemPortion>();
                List<PurchaseDetails> purchaseDetailsList1 = new List<PurchaseDetails>();
                List<PurchaseDetails> list2 = source.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.AccountTransactionId == objAccountTransaction.Id)).ToList<PurchaseDetails>();
                List<PurchaseDetails> purchaseDetailsList2 = new List<PurchaseDetails>();
                foreach (PurchaseDetails purchaseDetails in list2)
                {
                    PurchaseDetails objPurchaseDetail = purchaseDetails;
                    MenuItemPortion menuItemPortion1 = new MenuItemPortion();
                    MenuItemPortion menuItemPortion2 = list1.Find((Predicate<MenuItemPortion>)(o => o.Id == objPurchaseDetail.InventoryItemId));
                    if (menuItemPortion2 != null)
                        objPurchaseDetail.MRPPrice = menuItemPortion2.Price;
                    purchaseDetailsList2.Add(objPurchaseDetail);
                }
                accountScreenValue.PurchaseDetails = (IEnumerable<PurchaseDetails>)purchaseDetailsList2;
                List<AccountTransactionValue> list3 = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionId == objAccountTransaction.Id)).ToList<AccountTransactionValue>();
                accountScreenValue.AccountTransactionDocumentId = objAccountTransaction.AccountTransactionDocumentId;
                accountScreenValue.AccountTransactionType = objAccountTransaction.Name.Substring(0, objAccountTransaction.Name.IndexOf("["));
                accountScreenValue.AccountTransactionTypeId = objAccountTransaction.AccountTransactionTypeId;
                accountScreenValue.AccountTransactionValues = (IEnumerable<AccountTransactionValue>)list3;
                accountScreenValue.Amount = objAccountTransaction.Amount.ToString();
                accountScreenValue.Date = objAccountTransaction.Date;
                accountScreenValue.CreditAmount = list3.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>)(o => o.Credit));
                accountScreenValue.crTotal = list3.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>)(o => o.Credit)).ToString();
                accountScreenValue.DebitAmount = list3.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>)(o => o.Debit));
                accountScreenValue.Description = objAccountTransaction.Description;
                accountScreenValue.drTotal = list3.Sum<AccountTransactionValue>((Func<AccountTransactionValue, Decimal>)(o => o.Debit)).ToString();
                accountScreenValue.Id = objAccountTransaction.Id;
                accountScreenValue.IsReversed = objAccountTransaction.IsReversed;
                accountScreenValue.Name = objAccountTransaction.Name;
                accountScreenValue.Reversable = objAccountTransaction.Reversable;
                accountScreenValue.SourceAccountTypeId = objAccountTransaction.SourceAccountTypeId;
                accountScreenValue.TargetAccountTypeId = objAccountTransaction.TargetAccountTypeId;
                accountScreenValue.ref_invoice_number = objAccountTransaction.ref_invoice_number;
                accountScreenValue.BranchId = objAccountTransaction.BranchId;
                accountScreenValue.CompanyCode = objAccountTransaction.CompanyCode;
                accountScreenValue.DepartmentId = objAccountTransaction.DepartmentId;
                accountScreenValue.WareHouseId = objAccountTransaction.WareHouseId;
                accountScreenValue.UserName = objAccountTransaction.UserName;
                accountScreenValue.FinancialYear = objAccountTransaction.FinancialYear;
            }
            return accountScreenValue;
        }

        public static List<AccountScreen> GetScrenAccountTransaction(
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransaction> accRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          IDCubeRepository<PurchaseDetails> purchaseDetailsRepository,
          string FromDate,
          string ToDate,
          int TransactionTypeId,
          int BranchId)
        {
            DateTime sFormDate = NepalitoEnglishDate.EnglishDate(FromDate);
            DateTime sToDate = NepalitoEnglishDate.EnglishDate(ToDate);
            List<PurchaseDetails> source1 = new List<PurchaseDetails>();
            try
            {
                source1 = purchaseDetailsRepository.GetAllData().ToList<PurchaseDetails>();
            }
            catch (Exception ex)
            {
            }
            List<AccountTransaction> accountTransactionList1 = new List<AccountTransaction>();
            List<AccountTransaction> accountTransactionList2 = TransactionTypeId != 11 ? accRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Name.Contains("Purchase"))).ToList<AccountTransaction>() : accRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Name.Contains("Debit Note"))).ToList<AccountTransaction>();
            if (BranchId != 0)
                accountTransactionList2 = accountTransactionList2.Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.BranchId == BranchId)).ToList<AccountTransaction>();
            List<AccountType> accountTypeList = new List<AccountType>();
            accountTypeList = AccountTypeRepository.GetAllData().ToList<AccountType>();
            List<Account> accountList = new List<Account>();
            List<Account> list1 = AccountRepository.GetAllData().ToList<Account>();
            List<AccountTransactionValue> transactionValueList1 = new List<AccountTransactionValue>();
            List<AccountTransactionValue> transactionValueList2 = TransactionTypeId != 11 ? accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Date.Date >= sFormDate && o.Date.Date <= sToDate)).ToList<AccountTransactionValue>() : accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Date.Date >= sFormDate && o.Date.Date <= sToDate && o.Name.Contains("Debit Note"))).ToList<AccountTransactionValue>();
            if (BranchId != 0)
                transactionValueList2 = transactionValueList2.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.BranchId == BranchId)).ToList<AccountTransactionValue>();
            var source2 = transactionValueList2.Join((IEnumerable<AccountTransaction>)accountTransactionList2, (Func<AccountTransactionValue, int>)(v => v.AccountTransactionId), (Func<AccountTransaction, int>)(atr => atr.Id), (v, atr) => new
            {
                v = v,
                atr = atr
            }).Join((IEnumerable<Account>)list1, _param1 => _param1.v.AccountId, (Func<Account, int>)(a => a.Id), (_param1, a) => new
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
                BranchId = _param1.v.BranchId,
                CompanyCode = _param1.v.CompanyCode,
                DepartmentId = _param1.v.DepartmentId,
                WareHouseId = _param1.v.WareHouseId
            });
            var list2 = source2.Select(o => new
            {
                AccountTransactionId = o.AccountTransactionId,
                VoucherNo = o.VoucherNo,
                NVDate = o.NVDate,
                AccountTransactionType = o.AccountTransactionType
            }).Distinct().ToList().GroupBy(a => a.AccountTransactionId).Select(g => g.First()).ToList();
            List<AccountScreen> accountTransaction = new List<AccountScreen>();
            foreach (var data1 in list2)
            {
                var accounttransaction = data1;
                AccountScreen accountScreen = new AccountScreen();
                accountScreen.Id = accounttransaction.AccountTransactionId;
                accountScreen.VDate = accounttransaction.NVDate;
                accountScreen.VType = accounttransaction.AccountTransactionType;
                accountScreen.VoucherNo = accounttransaction.VoucherNo;
                List<PurchaseDetails> purchaseDetailsList = new List<PurchaseDetails>();
                List<PurchaseDetails> list3 = source1.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.AccountTransactionId == accounttransaction.AccountTransactionId)).ToList<PurchaseDetails>();
                accountScreen.PurchaseDetails = list3;
                var datas = source2.Where(o => o.VoucherNo == accounttransaction.VoucherNo);
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
                    accountScreenValue.BranchId = screenvalue.BranchId;
                    accountScreenValue.CompanyCode = screenvalue.CompanyCode;
                    accountScreenValue.DepartmentId = screenvalue.DepartmentId;
                    accountScreenValue.WareHouseId = screenvalue.WareHouseId;
                    List<AccountTransactionValue> transactionValueList3 = new List<AccountTransactionValue>();
                    List<AccountTransactionValue> list4 = transactionValueList2.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionId == screenvalue.Id)).ToList<AccountTransactionValue>();
                    accountScreenValue.AccountTransactionValues = (IEnumerable<AccountTransactionValue>)list4;
                    accountScreenValueList.Add(accountScreenValue);
                }
                accountScreen.AccountTransactionValues = (IList<AccountScreenValue>)accountScreenValueList;
                accountTransaction.Add(accountScreen);
            }
            return accountTransaction;
        }

        public static int Create(
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransactionType> accTransTypeRepository,
          IDCubeRepository<AccountTransaction> accRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          IDCubeRepository<PurchaseDetails> purchaseDetailsRepository,
          IDCubeRepository<AccountTransactionDocument> accTransDocRepository,
          IDCubeRepository<MenuItemPortion> menuportionrepository,
          AccountTransaction value)
        {
            int num1 = 1;
            if (value.PurchaseDetails != null)
            {
                string date = value.Date;
                DateTime dateTime1 = NepalitoEnglishDate.EnglishDate(date);
                DateTime now1 = DateTime.Now;
                DateTime dateTime2 = dateTime1.AddHours((double)now1.Hour).AddMinutes((double)now1.Minute).AddSeconds((double)now1.Second);
                string nepaliMonth = NepalitoEnglish.englishToNepaliMonth(dateTime2.Year, dateTime2.Month, dateTime2.Day);
                AccountTransaction accountTransaction1 = new AccountTransaction();
                DateTime now2 = DateTime.Now;
                now2.AddHours((double)now1.Hour);
                now2.AddMinutes((double)now1.Minute);
                now2.AddSeconds((double)now1.Second);
                string empty = string.Empty;
                int num2 = 0;
                List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
                List<MenuItemPortion> list1 = menuportionrepository.GetAllData().ToList<MenuItemPortion>();
                List<Account> list2 = AccountRepository.GetAllData().ToList<Account>();
                List<AccountTransactionType> accountTransactionTypeList = new List<AccountTransactionType>();
                List<AccountTransactionType> list3 = accTransTypeRepository.GetAllData().ToList<AccountTransactionType>();
                AccountTransactionType accountTransactionType1 = new AccountTransactionType();
                AccountTransactionType accountTransactionType2 = list3.Find((Predicate<AccountTransactionType>)(o => o.Name.Trim() == "Purchase Transaction"));
                List<AccountTransactionDocument> transactionDocumentList = new List<AccountTransactionDocument>();
                List<AccountTransactionDocument> list4 = accTransDocRepository.GetAllData().ToList<AccountTransactionDocument>();
                int num3;
                if (list4.Count == 0)
                {
                    num3 = num2 + 1;
                }
                else
                {
                    List<AccountTransactionDocument> list5 = list4.Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Name.Contains("Purchase Transaction"))).ToList<AccountTransactionDocument>();
                    if (list5.Count == 0)
                    {
                        num3 = num2 + 1;
                    }
                    else
                    {
                        AccountTransactionDocument transactionDocument1 = new AccountTransactionDocument();
                        AccountTransactionDocument transactionDocument2 = list5.OrderByDescending<AccountTransactionDocument, int>((Func<AccountTransactionDocument, int>)(o => o.Id)).FirstOrDefault<AccountTransactionDocument>();
                        num3 = int.Parse(transactionDocument2.Name.Substring(transactionDocument2.Name.IndexOf("[#")).Replace("]", "").Replace("[#", "")) + 1;
                    }
                }
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    unitOfWork.StartTransaction();
                    try
                    {
                        string str = accountTransactionType2.Name + " [#" + (object)num3 + "]";
                        accTransDocRepository.Insert(new AccountTransactionDocument()
                        {
                            Date = now2,
                            DocumentTypeId = 0,
                            Name = str,
                            Printed_Time = DateTime.Now
                        });
                        accTransDocRepository.Save();
                        int id1 = accTransDocRepository.GetAllData().ToList<AccountTransactionDocument>().Last<AccountTransactionDocument>().Id;
                        AccountTransaction accountTransaction2 = new AccountTransaction()
                        {
                            AccountTransactionDocumentId = id1,
                            Name = value.Name + "[#" + (object)num3 + "]",
                            Amount = value.Amount,
                            Description = value.Description,
                            ExchangeRate = value.ExchangeRate,
                            AccountTransactionTypeId = accountTransactionType2.Id,
                            SourceAccountTypeId = !value.Name.Contains("Debit Note") ? value.SourceAccountTypeId : value.SourceAccountTypeId,
                            TargetAccountTypeId = accountTransactionType2.TargetAccountTypeId,
                            Date = dateTime2.ToString(),
                            IsReversed = false,
                            Reversable = true
                        };
                        accountTransaction2.Reversable = true;
                        accountTransaction2.Printed_Time = DateTime.Now;
                        accountTransaction2.FinancialYear = value.FinancialYear;
                        accountTransaction2.ref_invoice_number = value.ref_invoice_number;
                        accountTransaction2.UserName = value.UserName;
                        accountTransaction2.BranchId = value.BranchId;
                        accountTransaction2.CompanyCode = value.CompanyCode;
                        accountTransaction2.DepartmentId = value.DepartmentId;
                        accountTransaction2.WareHouseId = value.WareHouseId;
                        accRepository.Insert(accountTransaction2);
                        accRepository.Save();
                        int id2 = accRepository.GetAllData().OrderByDescending<AccountTransaction, int>((Func<AccountTransaction, int>)(x => x.Id)).FirstOrDefault<AccountTransaction>().Id;
                        num1 = id2;
                        foreach (AccountTransactionValue transactionValue in value.AccountTransactionValues)
                        {
                            AccountTransactionValue objAccountTrans = transactionValue;
                            if (objAccountTrans.AccountId != 0)
                            {
                                accValueRepository.Insert(new AccountTransactionValue()
                                {
                                    AccountTransactionId = id2,
                                    AccountTransactionDocumentId = id1,
                                    Description = objAccountTrans.Description,
                                    Credit = objAccountTrans.Credit,
                                    Debit = objAccountTrans.Debit,
                                    entityLists = objAccountTrans.entityLists,
                                    AccountId = objAccountTrans.AccountId,
                                    Name = value.Name + "[#" + (object)num3 + "]",
                                    AccountTypeId = list2.Find((Predicate<Account>)(o => o.Id == objAccountTrans.AccountId)).AccountTypeId,
                                    Date = dateTime2,
                                    Printed_Time = DateTime.Now,
                                    NVDate = date,
                                    FinancialYear = value.FinancialYear,
                                    BranchId = value.BranchId,
                                    CompanyCode = value.CompanyCode,
                                    DepartmentId = value.DepartmentId,
                                    WareHouseId = value.WareHouseId,
                                    NepaliMonth = nepaliMonth,
                                    UserName = value.UserName
                                });
                                accValueRepository.Save();
                            }
                        }
                        AccountTransactionValue transactionValue1 = new AccountTransactionValue();
                        transactionValue1.AccountTransactionId = id2;
                        transactionValue1.AccountTransactionDocumentId = id1;
                        if (value.Name == "Debit Note")
                        {
                            transactionValue1.Credit = 0M;
                            transactionValue1.entityLists = "Dr";
                            transactionValue1.Debit = value.PurchaseDetails.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.InventoryItemId != 0)).Sum<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(o => o.Discount));
                        }
                        else
                        {
                            transactionValue1.Debit = 0M;
                            transactionValue1.entityLists = "Cr";
                            transactionValue1.Credit = value.PurchaseDetails.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.InventoryItemId != 0)).Sum<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(o => o.Discount));
                        }
                        transactionValue1.AccountId = 3;
                        transactionValue1.Name = value.Name + "[#" + (object)num3 + "]";
                        transactionValue1.AccountTypeId = list2.Find((Predicate<Account>)(o => o.Id == 3)).AccountTypeId;
                        transactionValue1.Date = dateTime2;
                        transactionValue1.Printed_Time = DateTime.Now;
                        transactionValue1.NVDate = date;
                        transactionValue1.FinancialYear = value.FinancialYear;
                        transactionValue1.NepaliMonth = nepaliMonth;
                        transactionValue1.UserName = value.UserName;
                        transactionValue1.BranchId = value.BranchId;
                        transactionValue1.CompanyCode = value.CompanyCode;
                        transactionValue1.DepartmentId = value.DepartmentId;
                        transactionValue1.WareHouseId = value.WareHouseId;
                        accValueRepository.Insert(transactionValue1);
                        accValueRepository.Save();
                        AccountTransactionValue transactionValue2 = new AccountTransactionValue();
                        transactionValue2.AccountTransactionId = id2;
                        transactionValue2.AccountTransactionDocumentId = id1;
                        if (value.Name == "Debit Note")
                        {
                            transactionValue2.Credit = value.VATAmount;
                            transactionValue2.entityLists = "Cr";
                            transactionValue2.Debit = 0M;
                        }
                        else
                        {
                            transactionValue2.Debit = value.VATAmount;
                            transactionValue2.entityLists = "Dr";
                            transactionValue2.Credit = 0M;
                        }
                        transactionValue2.AccountId = 8;
                        transactionValue2.Name = value.Name + "[#" + (object)num3 + "]";
                        transactionValue2.AccountTypeId = list2.Find((Predicate<Account>)(o => o.Id == 8)).AccountTypeId;
                        transactionValue2.Date = dateTime2;
                        transactionValue2.Printed_Time = DateTime.Now;
                        transactionValue2.NVDate = date;
                        transactionValue2.FinancialYear = value.FinancialYear;
                        transactionValue2.NepaliMonth = nepaliMonth;
                        transactionValue2.UserName = value.UserName;
                        transactionValue2.BranchId = value.BranchId;
                        transactionValue2.CompanyCode = value.CompanyCode;
                        transactionValue2.DepartmentId = value.DepartmentId;
                        transactionValue2.WareHouseId = value.WareHouseId;
                        accValueRepository.Insert(transactionValue2);
                        accValueRepository.Save();
                        Account account1 = new Account();
                        Account account2 = list2.Find((Predicate<Account>)(o => o.Name == "Excise Duty"));
                        int iExciseDutyId = 0;
                        iExciseDutyId = account2.Id;
                        AccountTransactionValue transactionValue3 = new AccountTransactionValue();
                        transactionValue3.AccountTransactionId = id2;
                        transactionValue3.AccountTransactionDocumentId = id1;
                        if (value.Name == "Debit Note")
                        {
                            transactionValue3.Credit = value.ExciseDuty;
                            transactionValue3.entityLists = "Cr";
                            transactionValue3.Debit = 0M;
                        }
                        else
                        {
                            transactionValue3.Debit = value.ExciseDuty;
                            transactionValue3.entityLists = "Cr";
                            transactionValue3.Credit = 0M;
                        }
                        transactionValue3.AccountId = iExciseDutyId;
                        transactionValue3.Name = value.Name + "[#" + (object)num3 + "]";
                        transactionValue3.AccountTypeId = list2.Find((Predicate<Account>)(o => o.Id == iExciseDutyId)).AccountTypeId;
                        transactionValue3.Date = dateTime2;
                        transactionValue3.Printed_Time = DateTime.Now;
                        transactionValue3.NVDate = date;
                        transactionValue3.FinancialYear = value.FinancialYear;
                        transactionValue3.NepaliMonth = nepaliMonth;
                        transactionValue3.UserName = value.UserName;
                        transactionValue3.BranchId = value.BranchId;
                        transactionValue3.CompanyCode = value.CompanyCode;
                        transactionValue3.DepartmentId = value.DepartmentId;
                        transactionValue3.WareHouseId = value.WareHouseId;
                        accValueRepository.Insert(transactionValue3);
                        accValueRepository.Save();
                        Account account3 = new Account();
                        Account account4 = list2.Find((Predicate<Account>)(o => o.Name == "Taxable Purchase"));
                        int iTaxableAccountId = 0;
                        if (account4 != null)
                            iTaxableAccountId = account4.Id;
                        AccountTransactionValue transactionValue4 = new AccountTransactionValue();
                        transactionValue4.AccountTransactionId = id2;
                        transactionValue4.AccountTransactionDocumentId = id1;
                        if (value.Name == "Debit Note")
                        {
                            transactionValue4.Credit = value.PurchaseDetails.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.InventoryItemId != 0 && o.TaxRate != 0M)).Sum<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(o => o.PurchaseAmount));
                            transactionValue4.entityLists = "Cr";
                            transactionValue4.Debit = 0M;
                        }
                        else
                        {
                            transactionValue4.Debit = value.PurchaseDetails.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.InventoryItemId != 0 && o.TaxRate != 0M)).Sum<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(o => o.PurchaseAmount));
                            transactionValue4.entityLists = "Dr";
                            transactionValue4.Credit = 0M;
                        }
                        transactionValue4.AccountId = iTaxableAccountId;
                        transactionValue4.Name = value.Name + "[#" + (object)num3 + "]";
                        transactionValue4.AccountTypeId = list2.Find((Predicate<Account>)(o => o.Id == iTaxableAccountId)).AccountTypeId;
                        transactionValue4.Date = dateTime2;
                        transactionValue4.Printed_Time = DateTime.Now;
                        transactionValue4.NVDate = date;
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
                        Account account6 = list2.Find((Predicate<Account>)(o => o.Name == "Non Taxable Purchase"));
                        int inonTaxableAccountId = 0;
                        if (account6 != null)
                            inonTaxableAccountId = account6.Id;
                        AccountTransactionValue transactionValue5 = new AccountTransactionValue();
                        transactionValue5.AccountTransactionId = id2;
                        transactionValue5.AccountTransactionDocumentId = id1;
                        if (value.Name.Contains("Debit Note"))
                        {
                            transactionValue5.Credit = value.PurchaseDetails.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.InventoryItemId != 0 && o.TaxRate == 0M)).Sum<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(o => o.PurchaseAmount));
                            transactionValue5.Debit = 0M;
                            transactionValue5.entityLists = "Cr";
                        }
                        else
                        {
                            transactionValue5.Credit = 0M;
                            transactionValue5.Debit = value.PurchaseDetails.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.InventoryItemId != 0 && o.TaxRate == 0M)).Sum<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(o => o.PurchaseAmount));
                            transactionValue5.entityLists = "Dr";
                        }
                        transactionValue5.AccountId = inonTaxableAccountId;
                        transactionValue5.Name = value.Name + " [#" + (object)num3 + "]";
                        transactionValue5.AccountTypeId = list2.Find((Predicate<Account>)(o => o.Id == inonTaxableAccountId)).AccountTypeId;
                        transactionValue5.Date = dateTime2;
                        transactionValue5.NVDate = date;
                        transactionValue5.NepaliMonth = nepaliMonth;
                        transactionValue5.UserName = value.UserName;
                        transactionValue5.FinancialYear = value.FinancialYear;
                        transactionValue5.BranchId = value.BranchId;
                        transactionValue5.CompanyCode = value.CompanyCode;
                        transactionValue5.DepartmentId = value.DepartmentId;
                        transactionValue5.WareHouseId = value.WareHouseId;
                        accValueRepository.Insert(transactionValue5);
                        accValueRepository.Save();
                        AccountTransactionValue transactionValue6 = new AccountTransactionValue();
                        transactionValue6.AccountTransactionId = id2;
                        transactionValue6.AccountTransactionDocumentId = id1;
                        if (value.Name == "Debit Note")
                        {
                            transactionValue6.Credit = 0M;
                            transactionValue6.Debit = value.Amount;
                            transactionValue6.entityLists = "Dr";
                        }
                        else
                        {
                            transactionValue6.Credit = value.Amount;
                            transactionValue6.Debit = 0M;
                            transactionValue6.entityLists = "Cr";
                        }
                        transactionValue6.AccountId = value.SourceAccountTypeId;
                        transactionValue6.Name = value.Name + " [#" + (object)num3 + "]";
                        transactionValue6.AccountTypeId = list2.Find((Predicate<Account>)(o => o.Id == value.SourceAccountTypeId)).AccountTypeId;
                        transactionValue6.Date = dateTime2;
                        transactionValue6.Printed_Time = DateTime.Now;
                        transactionValue6.NVDate = date;
                        transactionValue6.NepaliMonth = nepaliMonth;
                        transactionValue6.FinancialYear = value.FinancialYear;
                        transactionValue6.UserName = value.UserName;
                        transactionValue6.BranchId = value.BranchId;
                        transactionValue6.CompanyCode = value.CompanyCode;
                        transactionValue6.DepartmentId = value.DepartmentId;
                        transactionValue6.WareHouseId = value.WareHouseId;
                        accValueRepository.Insert(transactionValue6);
                        accValueRepository.Save();
                        foreach (PurchaseDetails purchaseDetail in value.PurchaseDetails)
                        {
                            if (purchaseDetail.PurchaseId == 0 && purchaseDetail.InventoryItemId != 0 && purchaseDetail.FinancialYear != "")
                            {
                                purchaseDetailsRepository.Insert(new PurchaseDetails()
                                {
                                    AccountTransactionId = id2,
                                    AccountTransactionDocumentId = id1,
                                    InventoryItemId = purchaseDetail.InventoryItemId,
                                    PurchaseRate = purchaseDetail.PurchaseRate,
                                    CostPrice = purchaseDetail.CostPrice,
                                    Quantity = !value.Name.Contains("Debit Note") ? purchaseDetail.Quantity : -purchaseDetail.Quantity,
                                    PurchaseAmount = purchaseDetail.PurchaseAmount,
                                    Discount = purchaseDetail.Discount,
                                    TaxRate = purchaseDetail.TaxRate,
                                    ExciseDuty = purchaseDetail.ExciseDuty,
                                    NVDate = date,
                                    FinancialYear = value.FinancialYear,
                                    NepaliMonth = nepaliMonth,
                                    UserName = value.UserName,
                                    BranchId = value.BranchId,
                                    CompanyCode = value.CompanyCode,
                                    DepartmentId = value.DepartmentId,
                                    WareHouseId = value.WareHouseId
                                });
                                purchaseDetailsRepository.Save();
                            }
                            if (value.Name.Contains("Debit Note"))
                            {
                                purchaseDetailsRepository.Insert(new PurchaseDetails()
                                {
                                    AccountTransactionId = id2,
                                    AccountTransactionDocumentId = id1,
                                    InventoryItemId = purchaseDetail.InventoryItemId,
                                    PurchaseRate = purchaseDetail.PurchaseRate,
                                    CostPrice = purchaseDetail.CostPrice,
                                    Quantity = !value.Name.Contains("Debit Note") ? purchaseDetail.Quantity : -purchaseDetail.Quantity,
                                    Discount = purchaseDetail.Discount,
                                    TaxRate = purchaseDetail.TaxRate,
                                    ExciseDuty = purchaseDetail.ExciseDuty,
                                    PurchaseAmount = purchaseDetail.PurchaseAmount,
                                    NVDate = date,
                                    FinancialYear = value.FinancialYear,
                                    NepaliMonth = nepaliMonth,
                                    UserName = value.UserName,
                                    BranchId = value.BranchId,
                                    CompanyCode = value.CompanyCode,
                                    DepartmentId = value.DepartmentId,
                                    WareHouseId = value.WareHouseId
                                });
                                purchaseDetailsRepository.Save();
                            }
                        }
                        foreach (PurchaseDetails purchaseDetail in value.PurchaseDetails)
                        {
                            PurchaseDetails objPurchaseDetails = purchaseDetail;
                            if (objPurchaseDetails.PurchaseId == 0 && objPurchaseDetails.InventoryItemId != 0)
                            {
                                MenuItemPortion menuItemPortion1 = new MenuItemPortion();
                                if (!value.Name.Contains("Debit Note"))
                                {
                                    MenuItemPortion menuItemPortion2 = list1.Find((Predicate<MenuItemPortion>)(o => o.Id == objPurchaseDetails.InventoryItemId));
                                    if (menuItemPortion2 != null)
                                    {
                                        menuItemPortion2.Price = objPurchaseDetails.MRPPrice;
                                        menuportionrepository.Update(menuItemPortion2);
                                        menuportionrepository.Save();
                                    }
                                }
                            }
                        }
                        unitOfWork.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.ErrorLogging(ex);
                        num1 = 0;
                    }
                }
            }
            else
                num1 = 0;
            return num1;
        }

        public static int Edit(
          IDCubeRepository<Account> AccountRepository,
          IDCubeRepository<AccountTransaction> accRepository,
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          IDCubeRepository<PurchaseDetails> purchaseDetailsRepository,
          IDCubeRepository<MenuItemPortion> menuportionrepository,
          AccountTransaction value,
          int id)
        {
            string date = value.Date;
            DateTime dateTime1 = NepalitoEnglishDate.EnglishDate(date);
            DateTime now = DateTime.Now;
            DateTime dateTime2 = dateTime1.AddHours((double)now.Hour).AddMinutes((double)now.Minute).AddSeconds((double)now.Second);
            string nepaliMonth = NepalitoEnglish.englishToNepaliMonth(dateTime2.Year, dateTime2.Month, dateTime2.Day);
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;
            List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
            List<MenuItemPortion> list1 = menuportionrepository.GetAllData().ToList<MenuItemPortion>();
            List<Account> list2 = AccountRepository.GetAllData().ToList<Account>();
            int num4 = id;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                unitOfWork.StartTransaction();
                try
                {
                    AccountTransaction accountTransaction1 = new AccountTransaction();
                    AccountTransaction accountTransaction2 = accRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Id == id)).FirstOrDefault<AccountTransaction>();
                    if (accountTransaction2 != null)
                    {
                        num1 = int.Parse(accountTransaction2.Name.Substring(accountTransaction2.Name.IndexOf("[#")).Replace("]", "").Replace("[#", ""));
                        num2 = accountTransaction2.AccountTransactionDocumentId;
                        num3 = accountTransaction2.Id;
                        accountTransaction2.SourceAccountTypeId = value.SourceAccountTypeId;
                        accountTransaction2.Amount = value.Amount;
                        accountTransaction2.Description = value.Description;
                        accountTransaction2.Date = dateTime2.ToString();
                        accountTransaction2.FinancialYear = value.FinancialYear;
                        accountTransaction2.ref_invoice_number = value.ref_invoice_number;
                        accountTransaction2.UserName = value.UserName;
                        if (value.Name.Contains("Debit Note"))
                        {
                            accountTransaction2.BranchId = accountTransaction2.BranchId;
                            accountTransaction2.CompanyCode = accountTransaction2.CompanyCode;
                            accountTransaction2.DepartmentId = accountTransaction2.DepartmentId;
                            accountTransaction2.WareHouseId = accountTransaction2.WareHouseId;
                        }
                        else
                        {
                            accountTransaction2.BranchId = value.BranchId;
                            accountTransaction2.CompanyCode = value.CompanyCode;
                            accountTransaction2.DepartmentId = value.DepartmentId;
                            accountTransaction2.WareHouseId = value.WareHouseId;
                        }
                        accRepository.Update(accountTransaction2);
                        accRepository.Save();
                    }
                    foreach (PurchaseDetails purchaseDetail in value.PurchaseDetails)
                    {
                        PurchaseDetails objPurchaseDetails = purchaseDetail;
                        if (objPurchaseDetails.PurchaseId > 0 && objPurchaseDetails.InventoryItemId != 0)
                        {
                            PurchaseDetails purchaseDetails = new PurchaseDetails();
                            purchaseDetails.PurchaseId = objPurchaseDetails.PurchaseId;
                            purchaseDetails.AccountTransactionId = objPurchaseDetails.AccountTransactionId;
                            purchaseDetails.AccountTransactionDocumentId = objPurchaseDetails.AccountTransactionDocumentId;
                            purchaseDetails.InventoryItemId = objPurchaseDetails.InventoryItemId;
                            purchaseDetails.PurchaseRate = objPurchaseDetails.PurchaseRate;
                            purchaseDetails.CostPrice = objPurchaseDetails.CostPrice;
                            purchaseDetails.Quantity = !value.Name.Contains("Debit Note") ? objPurchaseDetails.Quantity : -objPurchaseDetails.Quantity;
                            purchaseDetails.Discount = objPurchaseDetails.Discount;
                            purchaseDetails.TaxRate = objPurchaseDetails.TaxRate;
                            purchaseDetails.ExciseDuty = objPurchaseDetails.ExciseDuty;
                            purchaseDetails.PurchaseAmount = objPurchaseDetails.PurchaseAmount;
                            purchaseDetails.NVDate = date;
                            purchaseDetails.FinancialYear = value.FinancialYear;
                            purchaseDetails.NepaliMonth = nepaliMonth;
                            purchaseDetails.UserName = value.UserName;
                            if (value.Name.Contains("Debit Note"))
                            {
                                purchaseDetails.BranchId = accountTransaction2.BranchId;
                                purchaseDetails.CompanyCode = accountTransaction2.CompanyCode;
                                purchaseDetails.DepartmentId = accountTransaction2.DepartmentId;
                                purchaseDetails.WareHouseId = accountTransaction2.WareHouseId;
                            }
                            else
                            {
                                purchaseDetails.BranchId = value.BranchId;
                                purchaseDetails.CompanyCode = value.CompanyCode;
                                purchaseDetails.DepartmentId = value.DepartmentId;
                                purchaseDetails.WareHouseId = value.WareHouseId;
                            }
                            purchaseDetailsRepository.Update(purchaseDetails);
                            purchaseDetailsRepository.Save();
                        }
                        else if (objPurchaseDetails.PurchaseId == 0 && objPurchaseDetails.InventoryItemId != 0)
                        {
                            PurchaseDetails purchaseDetails = new PurchaseDetails();
                            purchaseDetails.AccountTransactionId = value.Id;
                            purchaseDetails.AccountTransactionDocumentId = value.AccountTransactionDocumentId;
                            purchaseDetails.InventoryItemId = objPurchaseDetails.InventoryItemId;
                            purchaseDetails.PurchaseRate = objPurchaseDetails.PurchaseRate;
                            purchaseDetails.CostPrice = objPurchaseDetails.CostPrice;
                            purchaseDetails.Quantity = objPurchaseDetails.Quantity;
                            purchaseDetails.Discount = objPurchaseDetails.Discount;
                            purchaseDetails.TaxRate = objPurchaseDetails.TaxRate;
                            purchaseDetails.ExciseDuty = objPurchaseDetails.ExciseDuty;
                            purchaseDetails.PurchaseAmount = objPurchaseDetails.PurchaseAmount;
                            purchaseDetails.NVDate = date;
                            purchaseDetails.FinancialYear = value.FinancialYear;
                            purchaseDetails.NepaliMonth = nepaliMonth;
                            purchaseDetails.UserName = value.UserName;
                            if (value.Name.Contains("Debit Note"))
                            {
                                purchaseDetails.BranchId = accountTransaction2.BranchId;
                                purchaseDetails.CompanyCode = accountTransaction2.CompanyCode;
                                purchaseDetails.DepartmentId = accountTransaction2.DepartmentId;
                                purchaseDetails.WareHouseId = accountTransaction2.WareHouseId;
                            }
                            else
                            {
                                purchaseDetails.BranchId = value.BranchId;
                                purchaseDetails.CompanyCode = value.CompanyCode;
                                purchaseDetails.DepartmentId = value.DepartmentId;
                                purchaseDetails.WareHouseId = value.WareHouseId;
                            }
                            purchaseDetailsRepository.Insert(purchaseDetails);
                            purchaseDetailsRepository.Save();
                            MenuItemPortion menuItemPortion1 = new MenuItemPortion();
                            MenuItemPortion menuItemPortion2 = list1.Find((Predicate<MenuItemPortion>)(o => o.Id == objPurchaseDetails.InventoryItemId));
                            if (menuItemPortion2 != null)
                            {
                                menuItemPortion2.Price = objPurchaseDetails.MRPPrice;
                                menuportionrepository.Update(menuItemPortion2);
                                menuportionrepository.Save();
                            }
                        }
                    }
                    foreach (PurchaseDetails purchaseDetail in value.PurchaseDetails)
                    {
                        PurchaseDetails objPurchaseDetails = purchaseDetail;
                        if (objPurchaseDetails.PurchaseId > 0 && objPurchaseDetails.InventoryItemId != 0)
                        {
                            MenuItemPortion menuItemPortion3 = new MenuItemPortion();
                            MenuItemPortion menuItemPortion4 = list1.Find((Predicate<MenuItemPortion>)(o => o.Id == objPurchaseDetails.InventoryItemId));
                            if (menuItemPortion4 != null)
                            {
                                menuItemPortion4.Price = objPurchaseDetails.MRPPrice;
                                menuportionrepository.Update(menuItemPortion4);
                                menuportionrepository.Save();
                            }
                        }
                    }
                    List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
                    List<AccountTransactionValue> list3 = accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionId == id)).ToList<AccountTransactionValue>();
                    AccountTransactionValue transactionValue1 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue2 = list3.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 3));
                    if (transactionValue2 != null)
                    {
                        if (value.Name.Contains("Debit Note"))
                        {
                            transactionValue2.Credit = 0M;
                            transactionValue2.entityLists = "Dr";
                            transactionValue2.Debit = value.PurchaseDetails.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.InventoryItemId != 0)).Sum<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(o => o.Discount));
                        }
                        else
                        {
                            transactionValue2.Debit = 0M;
                            transactionValue2.entityLists = "Cr";
                            transactionValue2.Credit = value.PurchaseDetails.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.InventoryItemId != 0)).Sum<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(o => o.Discount));
                        }
                        transactionValue2.Date = dateTime2;
                        transactionValue2.Printed_Time = DateTime.Now;
                        transactionValue2.NVDate = date;
                        transactionValue2.FinancialYear = value.FinancialYear;
                        transactionValue2.NepaliMonth = nepaliMonth;
                        transactionValue2.UserName = value.UserName;
                        if (value.Name.Contains("Debit Note"))
                        {
                            transactionValue2.BranchId = accountTransaction2.BranchId;
                            transactionValue2.CompanyCode = accountTransaction2.CompanyCode;
                            transactionValue2.DepartmentId = accountTransaction2.DepartmentId;
                            transactionValue2.WareHouseId = accountTransaction2.WareHouseId;
                        }
                        else
                        {
                            transactionValue2.BranchId = value.BranchId;
                            transactionValue2.CompanyCode = value.CompanyCode;
                            transactionValue2.DepartmentId = value.DepartmentId;
                            transactionValue2.WareHouseId = value.WareHouseId;
                        }
                        accValueRepository.Update(transactionValue2);
                        accValueRepository.Save();
                    }
                    AccountTransactionValue transactionValue3 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue4 = list3.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 8));
                    if (transactionValue4 != null)
                    {
                        if (value.Name.Contains("Debit Note"))
                        {
                            transactionValue4.Credit = value.VATAmount;
                            transactionValue4.entityLists = "Cr";
                            transactionValue4.Debit = 0M;
                        }
                        else
                        {
                            transactionValue4.Debit = value.VATAmount;
                            transactionValue4.entityLists = "Dr";
                            transactionValue4.Credit = 0M;
                        }
                        transactionValue4.Date = dateTime2;
                        transactionValue4.Printed_Time = DateTime.Now;
                        transactionValue4.NVDate = date;
                        transactionValue4.FinancialYear = value.FinancialYear;
                        transactionValue4.NepaliMonth = nepaliMonth;
                        transactionValue4.UserName = value.UserName;
                        if (value.Name.Contains("Debit Note"))
                        {
                            transactionValue4.BranchId = accountTransaction2.BranchId;
                            transactionValue4.CompanyCode = accountTransaction2.CompanyCode;
                            transactionValue4.DepartmentId = accountTransaction2.DepartmentId;
                            transactionValue4.WareHouseId = accountTransaction2.WareHouseId;
                        }
                        else
                        {
                            transactionValue4.BranchId = value.BranchId;
                            transactionValue4.CompanyCode = value.CompanyCode;
                            transactionValue4.DepartmentId = value.DepartmentId;
                            transactionValue4.WareHouseId = value.WareHouseId;
                        }
                        accValueRepository.Update(transactionValue4);
                        accValueRepository.Save();
                    }
                    Account account1 = new Account();
                    Account account2 = list2.Find((Predicate<Account>)(o => o.Name == "Excise Duty"));
                    int iExciseDutyAccountId = 0;
                    if (account2 != null)
                        iExciseDutyAccountId = account2.Id;
                    AccountTransactionValue transactionValue5 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue6 = list3.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == iExciseDutyAccountId));
                    if (transactionValue6 != null)
                    {
                        if (value.Name.Contains("Debit Note"))
                        {
                            transactionValue6.Credit = value.ExciseDuty;
                            transactionValue6.entityLists = "Cr";
                            transactionValue6.Debit = 0M;
                        }
                        else
                        {
                            transactionValue6.Debit = value.ExciseDuty;
                            transactionValue6.entityLists = "Dr";
                            transactionValue6.Credit = 0M;
                        }
                        transactionValue6.Date = dateTime2;
                        transactionValue6.Printed_Time = DateTime.Now;
                        transactionValue6.NVDate = date;
                        transactionValue6.FinancialYear = value.FinancialYear;
                        transactionValue6.NepaliMonth = nepaliMonth;
                        transactionValue6.UserName = value.UserName;
                        if (value.Name.Contains("Debit Note"))
                        {
                            transactionValue6.BranchId = accountTransaction2.BranchId;
                            transactionValue6.CompanyCode = accountTransaction2.CompanyCode;
                            transactionValue6.DepartmentId = accountTransaction2.DepartmentId;
                            transactionValue6.WareHouseId = accountTransaction2.WareHouseId;
                        }
                        else
                        {
                            transactionValue6.BranchId = value.BranchId;
                            transactionValue6.CompanyCode = value.CompanyCode;
                            transactionValue6.DepartmentId = value.DepartmentId;
                            transactionValue6.WareHouseId = value.WareHouseId;
                        }
                        accValueRepository.Update(transactionValue6);
                        accValueRepository.Save();
                    }
                    Account account3 = new Account();
                    Account account4 = list2.Find((Predicate<Account>)(o => o.Name == "Taxable Purchase"));
                    int iTaxableAccountId = 0;
                    if (account4 != null)
                        iTaxableAccountId = account4.Id;
                    AccountTransactionValue transactionValue7 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue8 = list3.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == iTaxableAccountId));
                    if (transactionValue8 != null)
                    {
                        if (value.Name.Contains("Debit Note"))
                        {
                            transactionValue8.Credit = value.PurchaseDetails.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.InventoryItemId != 0 && o.TaxRate != 0M)).Sum<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(o => o.PurchaseAmount));
                            transactionValue8.entityLists = "Cr";
                            transactionValue8.Debit = 0M;
                        }
                        else
                        {
                            transactionValue8.Debit = value.PurchaseDetails.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.InventoryItemId != 0 && o.TaxRate != 0M)).Sum<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(o => o.PurchaseAmount));
                            transactionValue8.entityLists = "Dr";
                            transactionValue8.Credit = 0M;
                        }
                        transactionValue8.Date = dateTime2;
                        transactionValue8.Printed_Time = DateTime.Now;
                        transactionValue8.NVDate = date;
                        transactionValue8.FinancialYear = value.FinancialYear;
                        transactionValue8.NepaliMonth = nepaliMonth;
                        transactionValue8.UserName = value.UserName;
                        if (value.Name.Contains("Debit Note"))
                        {
                            transactionValue8.BranchId = accountTransaction2.BranchId;
                            transactionValue8.CompanyCode = accountTransaction2.CompanyCode;
                            transactionValue8.DepartmentId = accountTransaction2.DepartmentId;
                            transactionValue8.WareHouseId = accountTransaction2.WareHouseId;
                        }
                        else
                        {
                            transactionValue8.BranchId = value.BranchId;
                            transactionValue8.CompanyCode = value.CompanyCode;
                            transactionValue8.DepartmentId = value.DepartmentId;
                            transactionValue8.WareHouseId = value.WareHouseId;
                        }
                        accValueRepository.Update(transactionValue8);
                        accValueRepository.Save();
                    }
                    Account account5 = new Account();
                    Account account6 = list2.Find((Predicate<Account>)(o => o.Name == "Non Taxable Purchase"));
                    int inonTaxableAccountId = 0;
                    if (account6 != null)
                        inonTaxableAccountId = account6.Id;
                    AccountTransactionValue transactionValue9 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue10 = list3.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == inonTaxableAccountId));
                    if (value.Name.Contains("Debit Note"))
                    {
                        transactionValue10.Credit = value.PurchaseDetails.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.InventoryItemId != 0 && o.TaxRate == 0M)).Sum<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(o => o.PurchaseAmount));
                        transactionValue10.Debit = 0M;
                        transactionValue10.entityLists = "Cr";
                    }
                    else
                    {
                        transactionValue10.Credit = 0M;
                        transactionValue10.Debit = value.PurchaseDetails.Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.InventoryItemId != 0 && o.TaxRate == 0M)).Sum<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(o => o.PurchaseAmount));
                        transactionValue10.entityLists = "Dr";
                    }
                    transactionValue10.Date = dateTime2;
                    transactionValue10.NVDate = date;
                    transactionValue10.NepaliMonth = nepaliMonth;
                    transactionValue10.UserName = value.UserName;
                    if (value.Name.Contains("Debit Note"))
                    {
                        transactionValue10.BranchId = accountTransaction2.BranchId;
                        transactionValue10.CompanyCode = accountTransaction2.CompanyCode;
                        transactionValue10.DepartmentId = accountTransaction2.DepartmentId;
                        transactionValue10.WareHouseId = accountTransaction2.WareHouseId;
                    }
                    else
                    {
                        transactionValue10.BranchId = value.BranchId;
                        transactionValue10.CompanyCode = value.CompanyCode;
                        transactionValue10.DepartmentId = value.DepartmentId;
                        transactionValue10.WareHouseId = value.WareHouseId;
                    }
                    accValueRepository.Update(transactionValue10);
                    accValueRepository.Save();
                    AccountTransactionValue transactionValue11 = new AccountTransactionValue();
                    List<AccountTransactionValue> list4 = (!value.Name.Contains("Debit Note") ? (IEnumerable<AccountTransactionValue>)list3.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.entityLists == "Cr")).ToList<AccountTransactionValue>() : (IEnumerable<AccountTransactionValue>)list3.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.entityLists == "Dr")).ToList<AccountTransactionValue>()).Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId != 3)).ToList<AccountTransactionValue>().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId != 8)).ToList<AccountTransactionValue>().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId != iTaxableAccountId)).ToList<AccountTransactionValue>().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId != inonTaxableAccountId)).ToList<AccountTransactionValue>().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId != iExciseDutyAccountId)).ToList<AccountTransactionValue>();
                    AccountTransactionValue transactionValue12 = list4.FirstOrDefault<AccountTransactionValue>();
                    if (value.Name.Contains("Debit Note"))
                    {
                        transactionValue12.Credit = 0M;
                        transactionValue12.Debit = value.Amount;
                        transactionValue12.entityLists = "Dr";
                    }
                    else
                    {
                        transactionValue12.Credit = value.Amount;
                        transactionValue12.Debit = 0M;
                        transactionValue12.entityLists = "Cr";
                    }
                    transactionValue12.AccountId = value.SourceAccountTypeId;
                    transactionValue12.AccountTypeId = list2.Find((Predicate<Account>)(o => o.Id == value.SourceAccountTypeId)).AccountTypeId;
                    transactionValue12.Description = value.Description;
                    transactionValue12.Date = dateTime2;
                    transactionValue12.Printed_Time = DateTime.Now;
                    transactionValue12.NVDate = date;
                    transactionValue12.NepaliMonth = nepaliMonth;
                    transactionValue12.FinancialYear = value.FinancialYear;
                    transactionValue12.UserName = value.UserName;
                    if (value.Name.Contains("Debit Note"))
                    {
                        transactionValue12.BranchId = accountTransaction2.BranchId;
                        transactionValue12.CompanyCode = accountTransaction2.CompanyCode;
                        transactionValue12.DepartmentId = accountTransaction2.DepartmentId;
                        transactionValue12.WareHouseId = accountTransaction2.WareHouseId;
                    }
                    else
                    {
                        transactionValue12.BranchId = value.BranchId;
                        transactionValue12.CompanyCode = value.CompanyCode;
                        transactionValue12.DepartmentId = value.DepartmentId;
                        transactionValue12.WareHouseId = value.WareHouseId;
                    }
                    accValueRepository.Update(transactionValue12);
                    accValueRepository.Save();
                    foreach (AccountTransactionValue transactionValue13 in value.AccountTransactionValues)
                    {
                        AccountTransactionValue objAccountTrans = transactionValue13;
                        if (objAccountTrans.Id > 0 && objAccountTrans.AccountId != 0)
                        {
                            AccountTransactionValue transactionValue14 = new AccountTransactionValue();
                            AccountTransactionValue transactionValue15 = list4.Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Id == objAccountTrans.Id)).FirstOrDefault<AccountTransactionValue>();
                            if (transactionValue15 != null)
                            {
                                transactionValue15.AccountTransactionId = objAccountTrans.AccountTransactionId;
                                transactionValue15.AccountTransactionDocumentId = objAccountTrans.AccountTransactionDocumentId;
                                transactionValue15.Name = objAccountTrans.Name;
                                transactionValue15.Description = objAccountTrans.Description;
                                transactionValue15.entityLists = objAccountTrans.entityLists;
                                transactionValue15.AccountTypeId = list2.Find((Predicate<Account>)(o => o.Id == objAccountTrans.AccountId)).AccountTypeId;
                                transactionValue15.AccountId = objAccountTrans.AccountId;
                                transactionValue15.Date = dateTime2;
                                transactionValue15.Debit = objAccountTrans.Debit;
                                transactionValue15.Credit = objAccountTrans.Credit;
                                transactionValue15.Exchange = objAccountTrans.Exchange;
                                transactionValue15.NVDate = date;
                                transactionValue15.FinancialYear = value.FinancialYear;
                                transactionValue15.NepaliMonth = nepaliMonth;
                                transactionValue15.UserName = value.UserName;
                                if (value.Name.Contains("Debit Note"))
                                {
                                    transactionValue15.BranchId = accountTransaction2.BranchId;
                                    transactionValue15.CompanyCode = accountTransaction2.CompanyCode;
                                    transactionValue15.DepartmentId = accountTransaction2.DepartmentId;
                                    transactionValue15.WareHouseId = accountTransaction2.WareHouseId;
                                }
                                else
                                {
                                    transactionValue15.BranchId = value.BranchId;
                                    transactionValue15.CompanyCode = value.CompanyCode;
                                    transactionValue15.DepartmentId = value.DepartmentId;
                                    transactionValue15.WareHouseId = value.WareHouseId;
                                }
                                accValueRepository.Update(transactionValue15);
                                accValueRepository.Save();
                            }
                        }
                        else if (objAccountTrans.Id == 0)
                        {
                            AccountTransactionValue transactionValue16 = new AccountTransactionValue();
                            transactionValue16.AccountTransactionId = num3;
                            transactionValue16.AccountTransactionDocumentId = num2;
                            transactionValue16.Credit = objAccountTrans.Credit;
                            transactionValue16.Debit = objAccountTrans.Debit;
                            transactionValue16.entityLists = objAccountTrans.entityLists;
                            transactionValue16.AccountId = objAccountTrans.AccountId;
                            transactionValue16.Name = value.Name + "[#" + (object)num1 + "]";
                            transactionValue16.AccountTypeId = list2.Find((Predicate<Account>)(o => o.Id == objAccountTrans.AccountId)).AccountTypeId;
                            transactionValue16.Date = dateTime2;
                            transactionValue16.Printed_Time = DateTime.Now;
                            transactionValue16.NVDate = date;
                            transactionValue16.FinancialYear = value.FinancialYear;
                            transactionValue16.NepaliMonth = nepaliMonth;
                            transactionValue16.UserName = value.UserName;
                            if (value.Name.Contains("Debit Note"))
                            {
                                transactionValue16.BranchId = accountTransaction2.BranchId;
                                transactionValue16.CompanyCode = accountTransaction2.CompanyCode;
                                transactionValue16.DepartmentId = accountTransaction2.DepartmentId;
                                transactionValue16.WareHouseId = accountTransaction2.WareHouseId;
                            }
                            else
                            {
                                transactionValue16.BranchId = value.BranchId;
                                transactionValue16.CompanyCode = value.CompanyCode;
                                transactionValue16.DepartmentId = value.DepartmentId;
                                transactionValue16.WareHouseId = value.WareHouseId;
                            }
                            accValueRepository.Insert(transactionValue16);
                            accValueRepository.Save();
                        }
                    }
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    num4 = 0;
                }
            }
            return num4;
        }

        public static int Delete(
          IDCubeRepository<AccountTransactionValue> accValueRepository,
          IDCubeRepository<AccountTransaction> accRepository,
          IDCubeRepository<AccountTransactionDocument> accTransDocRepository,
          IDCubeRepository<PurchaseDetails> purchaseDetailsRepository,
          AccountTransaction value)
        {
            int num = 1;
            AccountTransaction accountTransaction = new AccountTransaction();
            List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                unitOfWork.StartTransaction();
                try
                {
                    accTransDocRepository.Delete((object)value.AccountTransactionDocumentId);
                    accTransDocRepository.Save();
                    foreach (AccountTransactionValue transactionValue in accValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionDocumentId == value.AccountTransactionDocumentId)).ToList<AccountTransactionValue>())
                    {
                        accValueRepository.Delete((object)transactionValue.Id);
                        accValueRepository.Save();
                    }
                    unitOfWork.PurchaseAccountsRepository.DeleteAccountTransactionDocumentId(value.AccountTransactionDocumentId);
                    purchaseDetailsRepository.Save();
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

        public static int DeletePurchaseDetails(
          IDCubeRepository<PurchaseDetails> purchaseDetailsRepository,
          int Id)
        {
            int num = 1;
            PurchaseDetails purchaseDetails = new PurchaseDetails();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    purchaseDetailsRepository.Delete((object)Id);
                    purchaseDetailsRepository.Save();
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

        public static Decimal TotalPurchase(
          IDCubeRepository<PurchaseDetails> PurchaseDetailRepository,
          IDCubeRepository<AccountTransaction> AccountTransactionRepository,
          IDCubeRepository<FinancialYear> FinancialYearRepository,
          string financialyear)
        {
            FinancialYear objFinancialYear = FinancialYearRepository.GetAllData().Where<FinancialYear>((Func<FinancialYear, bool>)(o => o.Name == financialyear)).FirstOrDefault<FinancialYear>();
            List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
            return AccountTransactionRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => DateTime.Parse(o.Date) >= objFinancialYear.StartDate && DateTime.Parse(o.Date) <= objFinancialYear.EndDate)).ToList<AccountTransaction>().Join(PurchaseDetailRepository.GetAllData().ToList<PurchaseDetails>().Select(o => new
            {
                TotalPrice = o.Quantity * o.PurchaseRate,
                AccountTransactionId = o.AccountTransactionId
            }), (Func<AccountTransaction, int>)(t => t.Id), p => p.AccountTransactionId, (t, p) => new
            {
                TotalPrice = p.TotalPrice
            }).Sum(o => o.TotalPrice);
        }

        public static List<TicketReference> GetCustomerInvoiceNo(
          int CustomerId,
          IDCubeRepository<AccountTransaction> AccountTransactionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          string FinancialYear)
        {
            List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
            List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
            List<TicketReference> customerInvoiceNo = new List<TicketReference>();
            try
            {
                if (CustomerId != 0)
                {
                    List<AccountTransactionValue> list = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId == CustomerId && o.FinancialYear == FinancialYear & o.Name.Contains("Purchase"))).ToList<AccountTransactionValue>();
                    if (list != null)
                    {
                        foreach (var data in AccountTransactionRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.FinancialYear == FinancialYear & o.Name.Contains("Purchase"))).ToList<AccountTransaction>().Join((IEnumerable<AccountTransactionValue>)list, (Func<AccountTransaction, int>)(at => at.Id), (Func<AccountTransactionValue, int>)(atv => atv.AccountTransactionId), (at, atv) => new
                        {
                            ref_invoice_number = at.ref_invoice_number
                        }))
                        {
                            try
                            {
                                customerInvoiceNo.Add(new TicketReference()
                                {
                                    Id = int.Parse(data.ref_invoice_number),
                                    Name = data.ref_invoice_number
                                });
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return customerInvoiceNo;
        }

        public static List<PurchaseDetails> CustomerScreenOrderDetail(
          IDCubeRepository<AccountTransaction> AccountTransactionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<PurchaseDetails> PurchaseDetailsRepository,
          string CustomerId,
          string InvoiceNo,
          string FinancialYear)
        {
            List<PurchaseDetails> purchaseDetailsList = new List<PurchaseDetails>();
            List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
            List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
            List<AccountTransaction> list1 = AccountTransactionRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.ref_invoice_number == InvoiceNo && o.FinancialYear == FinancialYear)).ToList<AccountTransaction>();
            List<AccountTransactionValue> list2 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId.ToString() == CustomerId && o.FinancialYear == FinancialYear & o.Name.Contains("Purchase"))).ToList<AccountTransactionValue>();
            if (list1 != null && list2 != null)
            {
                var listAccountTransaction = list1.Join((IEnumerable<AccountTransactionValue>)list2, (Func<AccountTransaction, int>)(at => at.Id), (Func<AccountTransactionValue, int>)(atv => atv.AccountTransactionId), (at, atv) => new
                {
                    at = at,
                    atv = atv
                }).Where(_param1 => _param1.at.ref_invoice_number == InvoiceNo && _param1.atv.AccountId.ToString() == CustomerId).Select(_param1 => new
                {
                    Id = _param1.at.Id
                });
                if (listAccountTransaction != null)
                    purchaseDetailsList = PurchaseDetailsRepository.GetAllData().Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.AccountTransactionId == listAccountTransaction.FirstOrDefault().Id)).ToList<PurchaseDetails>();
            }
            return purchaseDetailsList;
        }
    }
}