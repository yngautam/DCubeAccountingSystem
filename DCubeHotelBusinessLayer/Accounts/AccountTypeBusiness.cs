using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using System;

namespace DCubeHotelBusinessLayer.Accounts
{
  public static class AccountTypeBusiness
  {
    public static int Create(
      IDCubeRepository<AccountType> accTypeRepository,
      IDCubeRepository<ExceptionLog> exceptionRepository,
      AccountType value)
    {
      int num = 1;
      AccountType accountType = new AccountType();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          accTypeRepository.Insert(value);
          accTypeRepository.Save();
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
      IDCubeRepository<AccountType> accTypeRepository,
      IDCubeRepository<ExceptionLog> exceptionRepository,
      int id,
      AccountType value)
    {
      int num = 1;
      AccountType accountType = new AccountType();
      if (value.Id >= 1)
      {
        using (UnitOfWork unitOfWork = new UnitOfWork())
        {
          try
          {
            unitOfWork.StartTransaction();
            accTypeRepository.Update(value);
            accTypeRepository.Save();
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

    public static int Delete(IDCubeRepository<AccountType> accTypeRepository, int id)
    {
      int num = 1;
      AccountType accountType = new AccountType();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          accTypeRepository.Delete((object) id);
          accTypeRepository.Save();
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
