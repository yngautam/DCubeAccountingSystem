using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using System;

namespace DCubeHotelBusinessLayer.Accounts
{
  public static class AccountTransactionTypeBusiness
  {
    public static int Create(
      IDCubeRepository<AccountTransactionType> accounttransTyperepo,
      IDCubeRepository<ExceptionLog> exceptionRepository,
      AccountTransactionType value)
    {
      int num = 1;
      AccountTransactionType accountTransactionType = new AccountTransactionType();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          accounttransTyperepo.Insert(value);
          accounttransTyperepo.Save();
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
      IDCubeRepository<AccountTransactionType> accounttransTyperepo,
      IDCubeRepository<ExceptionLog> exceptionRepository,
      int id,
      AccountTransactionType value)
    {
      int num = 1;
      AccountTransactionType accountTransactionType = new AccountTransactionType();
      if (id >= 1)
      {
        using (UnitOfWork unitOfWork = new UnitOfWork())
        {
          try
          {
            unitOfWork.StartTransaction();
            accounttransTyperepo.Update(value);
            accounttransTyperepo.Save();
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

    public static int Delete(
      IDCubeRepository<AccountTransactionType> accounttransTyperepo,
      int id)
    {
      int num = 1;
      AccountTransactionType accountTransactionType = new AccountTransactionType();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          accounttransTyperepo.Delete((object) id);
          accounttransTyperepo.Save();
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
  }
}
