using DCubeHotelDomain.Models;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.Accounts
{
  public class FinancialYearBusinessLayer
  {
    public static List<FinancialYear> GetFinancialYear(
      IDCubeRepository<FinancialYear> FinancialYearRepository)
    {
      List<FinancialYear> source = new List<FinancialYear>();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          source = FinancialYearRepository.GetAllData().ToList<FinancialYear>();
          source = source.OrderByDescending<FinancialYear, int>((Func<FinancialYear, int>) (o => o.Id)).ToList<FinancialYear>();
          return source;
        }
        catch (Exception ex)
        {
          source = (List<FinancialYear>) null;
        }
        unitOfWork.CommitTransaction();
      }
      return source;
    }

    public static FinancialYear GetFinancialYear(
      IDCubeRepository<FinancialYear> FinancialYearRepository,
      int Id)
    {
      FinancialYear financialYear = new FinancialYear();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          financialYear = FinancialYearRepository.GetAllData().Where<FinancialYear>((Func<FinancialYear, bool>) (o => o.Id == Id)).FirstOrDefault<FinancialYear>();
          return financialYear;
        }
        catch (Exception ex)
        {
          financialYear = (FinancialYear) null;
        }
        unitOfWork.CommitTransaction();
      }
      return financialYear;
    }

    public static int PostFinancialYear(
      IDCubeRepository<FinancialYear> FinancialYearRepository,
      FinancialYear value)
    {
      int num = 1;
      FinancialYear financialYear = new FinancialYear();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          FinancialYearRepository.Insert(value);
          FinancialYearRepository.Save();
          num = 1;
        }
        catch (Exception ex)
        {
          num = 0;
        }
        unitOfWork.CommitTransaction();
      }
      return num;
    }

    public static int UpdateFinancialYear(
      IDCubeRepository<FinancialYear> FinancialYearRepository,
      FinancialYear value,
      int id)
    {
      int num = 1;
      if (id >= 1)
      {
        using (UnitOfWork unitOfWork = new UnitOfWork())
        {
          try
          {
            unitOfWork.StartTransaction();
            FinancialYearRepository.Update(value);
            FinancialYearRepository.Save();
            num = 1;
          }
          catch (Exception ex)
          {
            num = 0;
          }
          unitOfWork.CommitTransaction();
        }
      }
      else
        num = 0;
      return num;
    }

    public static int DeleteFinancialYear(
      IDCubeRepository<FinancialYear> FinancialYearRepository,
      int id)
    {
      int num = 1;
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          FinancialYearRepository.Delete((object) id);
          FinancialYearRepository.Save();
          num = 1;
        }
        catch (Exception ex)
        {
          num = 0;
        }
        unitOfWork.CommitTransaction();
      }
      return num;
    }
  }
}
