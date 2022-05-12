using DCubeHotelDomain.Models;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer
{
  public class CompanyBusinessLayer
  {
    public static List<Company> GetCompany(IDCubeRepository<Company> CompanyRepository)
    {
      List<Company> company = new List<Company>();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          company = CompanyRepository.GetAllData().ToList<Company>();
          return company;
        }
        catch (Exception ex)
        {
          company = (List<Company>) null;
        }
        unitOfWork.CommitTransaction();
      }
      return company;
    }

    public static Company GetCompany(IDCubeRepository<Company> CompanyRepository, int Id)
    {
      Company company = new Company();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          company = CompanyRepository.GetAllData().Where<Company>((Func<Company, bool>) (o => o.Id == Id)).FirstOrDefault<Company>();
          return company;
        }
        catch (Exception ex)
        {
          company = (Company) null;
        }
        unitOfWork.CommitTransaction();
      }
      return company;
    }

    public static int PostCompany(IDCubeRepository<Company> CompanyRepository, Company value)
    {
      int num = 1;
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          CompanyRepository.Insert(value);
          CompanyRepository.Save();
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

    public static int UpdateCompany(
      IDCubeRepository<Company> CompanyRepository,
      Company value,
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
            CompanyRepository.Update(value);
            CompanyRepository.Save();
            num = value.Id;
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

    public static int DeleteCompany(IDCubeRepository<Company> CompanyRepository, int id)
    {
      int num = 1;
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          CompanyRepository.Delete((object) id);
          CompanyRepository.Save();
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
