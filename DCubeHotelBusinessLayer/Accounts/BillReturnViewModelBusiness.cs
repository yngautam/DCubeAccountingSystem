using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.CBMSAPI;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.Accounts
{
  public class BillReturnViewModelBusiness
  {
    public static List<BillReturnViewModel> getBillReturnViewModel(
      IDCubeRepository<AccountType> AccountTypeRepository,
      IDCubeRepository<Account> AccountRepository,
      IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
      IDCubeRepository<AccountTransaction> AccountTransactionRepository,
      IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
      string FinancialYear)
    {
      List<BillReturnViewModel> billReturnViewModel1 = new List<BillReturnViewModel>();
      List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
      List<AccountTransaction> list1 = AccountTransactionRepository.GetAllData().ToList<AccountTransaction>();
      List<Account> accountList = new List<Account>();
      List<Account> list2 = AccountRepository.GetAllData().ToList<Account>();
      List<AccountType> accountTypeList = new List<AccountType>();
      accountTypeList = AccountTypeRepository.GetAllData().ToList<AccountType>();
      List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
      var source = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>) (o => o.FinancialYear == FinancialYear)).ToList<AccountTransactionValue>().Join((IEnumerable<AccountTransaction>) list1, (Func<AccountTransactionValue, int>) (v => v.AccountTransactionId), (Func<AccountTransaction, int>) (atr => atr.Id), (v, atr) => new
      {
        v = v,
        atr = atr
      }).Join((IEnumerable<Account>) list2, _param1 => _param1.v.AccountId, (Func<Account, int>) (a => a.Id), (_param1, a) => new
      {
        Id = _param1.v.Id,
        NVDate = _param1.v.NVDate,
        AccountTransactionTypeId = _param1.atr.AccountTransactionTypeId,
        Name = a.Name,
        AccountTransactionId = _param1.v.AccountTransactionId,
        VoucherNo = _param1.atr.Name.Substring(_param1.atr.Name.IndexOf("[")),
        AccountTransactionType = _param1.atr.Name.Substring(0, _param1.atr.Name.IndexOf("[")),
        Amount = _param1.atr.Amount,
        SourceAccountTypeId = _param1.atr.SourceAccountTypeId,
        Description = _param1.atr.Description,
        DebitAmount = _param1.v.Debit,
        CreditAmount = _param1.v.Credit,
        Date = _param1.v.Date,
        PhoteIdentity = _param1.atr.PhoteIdentity
      });
      var objaccounttransaction = source.Where(x => x.AccountTransactionTypeId == 11).FirstOrDefault();
      foreach (var data in source.Where(o => o.AccountTransactionTypeId == objaccounttransaction.AccountTransactionTypeId))
      {
        BillReturnViewModel billReturnViewModel2 = new BillReturnViewModel();
        billReturnViewModel2.amount_for_esf = (double) data.AccountTransactionId;
        billReturnViewModel2.credit_note_date = data.NVDate;
        billReturnViewModel2.buyer_name = data.Name;
        billReturnViewModel2.ref_invoice_number = data.VoucherNo;
        billReturnViewModel2.username = "Credit Note";
        BillReturnViewModel billReturnViewModel3 = billReturnViewModel2;
        Decimal num1 = data.CreditAmount;
        double num2 = double.Parse(num1.ToString());
        billReturnViewModel3.excisable_amount = num2;
        BillReturnViewModel billReturnViewModel4 = billReturnViewModel2;
        num1 = data.DebitAmount;
        double num3 = double.Parse(num1.ToString());
        billReturnViewModel4.export_sales = num3;
        billReturnViewModel1.Add(billReturnViewModel2);
      }
      return billReturnViewModel1;
    }
  }
}
