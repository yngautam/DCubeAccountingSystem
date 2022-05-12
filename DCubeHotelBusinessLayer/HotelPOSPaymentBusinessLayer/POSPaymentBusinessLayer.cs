using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using NepaliDate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.HotelPOSPaymentBusinessLayer
{
    public static class POSPaymentBusinessLayer
    {
        public static ScreenTicket POSTPayment(
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          DateTime currentdate,
          PaymentSettle possettle)
        {
            ScreenTicket screenTicket = new ScreenTicket();
            try
            {
                AccountType objAccountType = new AccountType();
                objAccountType = AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Id == 20)).FirstOrDefault<AccountType>();
                if (objAccountType == null)
                    objAccountType = AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Name.Contains("Sales"))).FirstOrDefault<AccountType>();
                int num = 0;
                AccountTransactionType accountTransactionType = new AccountTransactionType();
                AccountTransactionType objAccountTransactionType = AccountTransactionTypeRepository.SelectDataById((object)4);
                Ticket objTicket = new Ticket();
                objTicket = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == possettle.TicketId)).FirstOrDefault<Ticket>();
                objTicket.TicketStates = "Paid";
                TicketRepository.Update(objTicket);
                TicketRepository.Save();
                List<AccountTransaction> accountTransactionList = new List<AccountTransaction>();
                List<AccountTransaction> list1 = AccountTranastionRepository.GetAllData().ToList<AccountTransaction>();
                int iTransactionDocument;
                if (list1.Count == 0)
                {
                    iTransactionDocument = num + 1;
                }
                else
                {
                    List<AccountTransaction> list2 = list1.Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.Name.Contains(possettle.PaymentMode))).ToList<AccountTransaction>();
                    if (list2.Count == 0)
                    {
                        iTransactionDocument = num + 1;
                    }
                    else
                    {
                        AccountTransaction accountTransaction1 = new AccountTransaction();
                        AccountTransaction accountTransaction2 = list2.OrderByDescending<AccountTransaction, int>((Func<AccountTransaction, int>)(o => o.Id)).FirstOrDefault<AccountTransaction>();
                        iTransactionDocument = int.Parse(accountTransaction2.Name.Substring(accountTransaction2.Name.IndexOf("[#")).Replace("]", "").Replace("[#", "")) + 1;
                    }
                }
                AccountTransactionDocument objAccountTransactionDocument = new AccountTransactionDocument();
                if (objAccountType != null && objTicket != null)
                    objAccountTransactionDocument = TransactionDocumentRepository.GetAllData().Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Name == objAccountType.Name + " [#" + objTicket.TicketNumber + "]")).FirstOrDefault<AccountTransactionDocument>();
                if (iTransactionDocument != 0)
                {
                    int iAccountTransaction = POSPaymentBusinessLayer.SaveAccountTransaction(objAccountTransactionType, AccountTranastionRepository, AccountTransactionTypeRepository, objAccountTransactionDocument.Id, iTransactionDocument, possettle);
                    if (iAccountTransaction == 0)
                        ;
                    if (POSPaymentBusinessLayer.SaveAccountTransactionValue(objAccountTransactionType, AccountTransactionValueRepository, TicketRepository, objAccountTransactionDocument.Id, iAccountTransaction, iTransactionDocument, currentdate, possettle) != 0)
                        ;
                    if (possettle.Discount == 0M)
                    {
                        List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
                        List<AccountTransactionValue> list3 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionDocumentId == objAccountTransactionDocument.Id)).ToList<AccountTransactionValue>();
                        AccountTransactionValue transactionValue1 = new AccountTransactionValue();
                        transactionValue1 = new AccountTransactionValue();
                        AccountTransactionValue transactionValue2 = list3.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 3));
                        if (transactionValue2 != null)
                            screenTicket.Discount = transactionValue2.Debit;
                    }
                    else
                        screenTicket.Discount = possettle.Discount;
                    screenTicket.CustomerId = objTicket.Table_Customer_Room;
                    screenTicket.Id = objTicket.Id;
                    screenTicket.IsActive = true;
                    screenTicket.isSubmitted = true;
                    screenTicket.Name = objTicket.Name;
                    screenTicket.Note = objTicket.Note;
                    screenTicket.PaymentHistory = (IEnumerable<PaymentHistory>)POSPaymentBusinessLayer.ListPaymentHistory(objAccountTransactionDocument.Id.ToString(), objAccountTransactionType.Name, AccountTransactionValueRepository, objAccountTransactionDocument.FinancialYear);
                    screenTicket.TableId = objTicket.Table_Customer_Room;
                    screenTicket.TicketId = int.Parse(objTicket.TicketNumber);
                    screenTicket.TicketOpeningTime = objTicket.Date;
                    screenTicket.TotalAmount = objTicket.TotalAmount;
                }
            }
            catch
            {
                screenTicket = (ScreenTicket)null;
            }
            return screenTicket;
        }

        public static ScreenTicket POSTDiscount(
          IDCubeRepository<Ticket> TicketRepository,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<AccountTransactionDocument> TransactionDocumentRepository,
          IDCubeRepository<AccountType> AccountTypeRepository,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          DateTime currentdate,
          PaymentSettle possettle)
        {
            ScreenTicket screenTicket = new ScreenTicket();
            try
            {
                AccountTransactionType accountTransactionType1 = new AccountTransactionType();
                AccountTransactionType accountTransactionType2 = AccountTransactionTypeRepository.SelectDataById((object)4);
                AccountType objAccountType = new AccountType();
                objAccountType = AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Id == 20)).FirstOrDefault<AccountType>();
                if (objAccountType == null)
                    objAccountType = AccountTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>)(o => o.Name.Contains("Sales"))).FirstOrDefault<AccountType>();
                Ticket objTicket = new Ticket();
                objTicket = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == possettle.TicketId)).FirstOrDefault<Ticket>();
                AccountTransactionDocument objAccountTransactionDocument = TransactionDocumentRepository.GetAllData().Where<AccountTransactionDocument>((Func<AccountTransactionDocument, bool>)(o => o.Name == objAccountType.Name + " [#" + objTicket.TicketNumber + "]")).FirstOrDefault<AccountTransactionDocument>();
                AccountTransaction accountTransaction1 = new AccountTransaction();
                AccountTransaction accountTransaction2 = AccountTranastionRepository.GetAllData().Where<AccountTransaction>((Func<AccountTransaction, bool>)(o => o.AccountTransactionDocumentId == objAccountTransactionDocument.Id)).FirstOrDefault<AccountTransaction>();
                List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
                List<AccountTransactionValue> list = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountTransactionDocumentId == objAccountTransactionDocument.Id)).ToList<AccountTransactionValue>();
                AccountTransactionValue objAccountTransactionValue = new AccountTransactionValue();
                objAccountTransactionValue = new AccountTransactionValue();
                objAccountTransactionValue = list.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 3));
                if (objAccountTransactionValue == null)
                {
                    if (possettle.Discount != 0M && accountTransaction2 != null)
                    {
                        DateTime dateTime = DateTime.Parse(accountTransaction2.Date);
                        accountTransactionType2 = AccountTransactionTypeRepository.SelectDataById((object)1);
                        AccountTransactionValueRepository.Insert(new AccountTransactionValue()
                        {
                            AccountTransactionId = accountTransaction2.Id,
                            AccountTransactionDocumentId = objAccountTransactionDocument.Id,
                            AccountTypeId = 1,
                            AccountId = 3,
                            Date = dateTime,
                            Debit = possettle.Discount,
                            entityLists = "Dr",
                            Credit = 0M,
                            Exchange = possettle.Discount,
                            Name = accountTransactionType2.Name + " [#" + objTicket.TicketNumber + "]",
                            Printed_Time = DateTime.Now,
                            FinancialYear = possettle.FinancialYear,
                            ref_invoice_number = "0",
                            NepaliMonth = NepalitoEnglish.englishToNepaliMonth(dateTime.Year, dateTime.Month, dateTime.Day),
                            NVDate = NepalitoEnglish.englishToNepali(dateTime.Year, dateTime.Month, dateTime.Day),
                            UserName = possettle.UserName
                        });
                        AccountTransactionValueRepository.Save();
                    }
                }
                else
                {
                    AccountTransactionValue transactionValue1 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue2 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Id == objAccountTransactionValue.Id)).FirstOrDefault<AccountTransactionValue>();
                    if (transactionValue2 != null)
                    {
                        transactionValue2.Debit = possettle.Discount;
                        transactionValue2.Exchange = possettle.Discount;
                        transactionValue2.UserName = possettle.UserName;
                        AccountTransactionValueRepository.Update(transactionValue2);
                        AccountTransactionValueRepository.Save();
                    }
                }
                objAccountTransactionValue = new AccountTransactionValue();
                objAccountTransactionValue = list.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 1));
                if (objAccountTransactionValue != null)
                {
                    AccountTransactionValue transactionValue3 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue4 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Id == objAccountTransactionValue.Id)).FirstOrDefault<AccountTransactionValue>();
                    if (transactionValue4 != null)
                    {
                        transactionValue4.Credit = possettle.PosSettle.TicketTotal;
                        transactionValue4.Exchange = possettle.PosSettle.TicketTotal;
                        transactionValue4.UserName = possettle.UserName;
                        AccountTransactionValueRepository.Update(transactionValue4);
                        AccountTransactionValueRepository.Save();
                    }
                }
                objAccountTransactionValue = new AccountTransactionValue();
                if (possettle.PosSettle.CustomerId != 0)
                    objAccountTransactionValue = list.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == possettle.PosSettle.CustomerId));
                if (possettle.PosSettle.CustomerId == 0)
                    objAccountTransactionValue = list.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 2));
                if (objAccountTransactionValue != null)
                {
                    AccountTransactionValue transactionValue5 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue6 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Id == objAccountTransactionValue.Id)).FirstOrDefault<AccountTransactionValue>();
                    if (transactionValue6 != null)
                    {
                        transactionValue6.Debit = possettle.PosSettle.GrandTotal;
                        transactionValue6.Exchange = possettle.PosSettle.GrandTotal;
                        transactionValue6.UserName = possettle.UserName;
                        AccountTransactionValueRepository.Update(transactionValue6);
                        AccountTransactionValueRepository.Save();
                    }
                }
                objAccountTransactionValue = new AccountTransactionValue();
                objAccountTransactionValue = list.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 8));
                if (objAccountTransactionValue != null)
                {
                    AccountTransactionValue transactionValue7 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue8 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Id == objAccountTransactionValue.Id)).FirstOrDefault<AccountTransactionValue>();
                    if (transactionValue8 != null)
                    {
                        transactionValue8.Credit = possettle.PosSettle.VatAmount;
                        transactionValue8.UserName = possettle.UserName;
                        AccountTransactionValueRepository.Update(transactionValue8);
                        AccountTransactionValueRepository.Save();
                    }
                }
                objAccountTransactionValue = new AccountTransactionValue();
                objAccountTransactionValue = list.Find((Predicate<AccountTransactionValue>)(o => o.AccountId == 9));
                if (objAccountTransactionValue != null)
                {
                    AccountTransactionValue transactionValue9 = new AccountTransactionValue();
                    AccountTransactionValue transactionValue10 = AccountTransactionValueRepository.GetAllData().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Id == objAccountTransactionValue.Id)).FirstOrDefault<AccountTransactionValue>();
                    if (transactionValue10 != null)
                    {
                        transactionValue10.Credit = possettle.PosSettle.ServiceCharge;
                        transactionValue10.UserName = possettle.UserName;
                        AccountTransactionValueRepository.Update(transactionValue10);
                        AccountTransactionValueRepository.Save();
                    }
                }
                screenTicket.CustomerId = objTicket.Table_Customer_Room;
                screenTicket.Id = objTicket.Id;
                screenTicket.IsActive = true;
                screenTicket.isSubmitted = true;
                screenTicket.Name = objTicket.Name;
                screenTicket.Note = objTicket.Note;
                screenTicket.PaymentHistory = (IEnumerable<PaymentHistory>)POSPaymentBusinessLayer.ListPaymentHistory(objAccountTransactionDocument.Id.ToString(), accountTransactionType2.Name, AccountTransactionValueRepository, objTicket.FinancialYear);
                screenTicket.Discount = possettle.Discount;
                screenTicket.TableId = objTicket.Table_Customer_Room;
                screenTicket.TicketId = int.Parse(objTicket.TicketNumber);
                screenTicket.TicketOpeningTime = objTicket.Date;
                screenTicket.TotalAmount = objTicket.TotalAmount;
            }
            catch (Exception ex)
            {
                screenTicket = (ScreenTicket)null;
            }
            return screenTicket;
        }

        public static List<PaymentHistory> ListPaymentHistory(
          string DocumentId,
          string TransactionType,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          string FinancialYear)
        {
            List<PaymentHistory> paymentHistoryList = new List<PaymentHistory>();
            List<AccountTransactionValue> transactionValueList = new List<AccountTransactionValue>();
            List<AccountTransactionValue> list = AccountTransactionValueRepository.GetAllData().ToList<AccountTransactionValue>().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Name.Contains(TransactionType))).ToList<AccountTransactionValue>().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.Name.Contains(TransactionType) && o.AccountTransactionDocumentId.ToString() == DocumentId && o.Debit > 0M)).ToList<AccountTransactionValue>().Where<AccountTransactionValue>((Func<AccountTransactionValue, bool>)(o => o.AccountId != 3)).ToList<AccountTransactionValue>();
            if (list != null)
            {
                foreach (AccountTransactionValue transactionValue in list)
                    paymentHistoryList.Add(new PaymentHistory()
                    {
                        Id = transactionValue.Id,
                        AmountPaid = transactionValue.Debit,
                        DateTime = transactionValue.Date.ToString(),
                        PaymentMode = transactionValue.Name.Replace(transactionValue.Name.Substring(0, TransactionType.Length), "")
                    });
            }
            return paymentHistoryList;
        }

        private static int SaveAccountTransaction(
          AccountTransactionType objAccountTransactionType,
          IDCubeRepository<AccountTransaction> AccountTranastionRepository,
          IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository,
          int iAccountTransactionDocument,
          int iTransactionDocument,
          PaymentSettle possettle)
        {
            int num;
            try
            {
                AccountTransaction accountTransaction = new AccountTransaction();
                accountTransaction.AccountTransactionDocumentId = iAccountTransactionDocument;
                if (!(possettle.Charged != 0M))
                    ;
                accountTransaction.Date = DateTime.Now.ToString();
                accountTransaction.Printed_Time = DateTime.Now;
                accountTransaction.Amount = possettle.Charged;
                accountTransaction.ExchangeRate = 1M;
                accountTransaction.AccountTransactionTypeId = 4;
                accountTransaction.SourceAccountTypeId = 5;
                accountTransaction.TargetAccountTypeId = 1;
                accountTransaction.IsReversed = false;
                accountTransaction.Reversable = true;
                accountTransaction.Name = objAccountTransactionType.Name + " [" + possettle.PaymentMode + "][#" + (object)iTransactionDocument + "]";
                accountTransaction.FinancialYear = possettle.FinancialYear;
                accountTransaction.ref_invoice_number = "0";
                accountTransaction.UserName = possettle.UserName;
                AccountTranastionRepository.Insert(accountTransaction);
                AccountTranastionRepository.Save();
                num = AccountTranastionRepository.GetAllData().ToList<AccountTransaction>().Last<AccountTransaction>().Id;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorLogging(ex);
                num = 0;
            }
            return num;
        }

        private static int SaveAccountTransactionValue(
          AccountTransactionType objAccountTransactionType,
          IDCubeRepository<AccountTransactionValue> AccountTransactionValueRepository,
          IDCubeRepository<Ticket> TicketRepository,
          int iAccountTransactionDocument,
          int iAccountTransaction,
          int iTransactionDocument,
          DateTime CurrentDate,
          PaymentSettle possettle)
        {
            int num = 0;
            try
            {
                Ticket ticket1 = new Ticket();
                Ticket ticket2 = TicketRepository.GetAllData().Where<Ticket>((Func<Ticket, bool>)(o => o.Id == possettle.TicketId)).FirstOrDefault<Ticket>();
                if (ticket2 != null)
                {
                    AccountTransactionValue transactionValue = new AccountTransactionValue();
                    transactionValue.Printed_Time = DateTime.Now;
                    transactionValue.AccountTransactionId = iAccountTransaction;
                    transactionValue.AccountTransactionDocumentId = iAccountTransactionDocument;
                    if (ticket2.TicketTypeId == 1)
                    {
                        transactionValue.AccountId = ticket2.Table_Customer_Room;
                        transactionValue.AccountTypeId = 5;
                    }
                    if (ticket2.TicketTypeId == 2)
                    {
                        transactionValue.AccountId = 2;
                        transactionValue.AccountTypeId = 1;
                    }
                    transactionValue.Date = CurrentDate;
                    transactionValue.Debit = 0M;
                    transactionValue.Credit = possettle.Charged;
                    transactionValue.Exchange = possettle.Charged;
                    transactionValue.Name = objAccountTransactionType.Name + " [" + possettle.PaymentMode + "][#" + (object)iTransactionDocument + "]";
                    transactionValue.FinancialYear = possettle.FinancialYear;
                    transactionValue.UserName = possettle.UserName;
                    transactionValue.ref_invoice_number = "0";
                    transactionValue.NepaliMonth = NepalitoEnglish.englishToNepaliMonth(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day);
                    transactionValue.NVDate = NepalitoEnglish.englishToNepali(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day);
                    AccountTransactionValueRepository.Insert(transactionValue);
                    AccountTransactionValueRepository.Save();
                    AccountTransactionValueRepository.Insert(new AccountTransactionValue()
                    {
                        AccountTransactionId = iAccountTransaction,
                        AccountTransactionDocumentId = iAccountTransactionDocument,
                        AccountTypeId = 3,
                        AccountId = 5,
                        Date = CurrentDate,
                        Debit = possettle.Charged,
                        Credit = 0M,
                        Exchange = possettle.Charged,
                        Name = objAccountTransactionType.Name + " [" + possettle.PaymentMode + "]",
                        Printed_Time = DateTime.Now,
                        FinancialYear = possettle.FinancialYear,
                        UserName = possettle.UserName,
                        ref_invoice_number = "0",
                        NepaliMonth = NepalitoEnglish.englishToNepaliMonth(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day),
                        NVDate = NepalitoEnglish.englishToNepali(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day)
                    });
                    AccountTransactionValueRepository.Save();
                }
            }
            catch (Exception ex)
            {
            }
            return num;
        }
    }
}
