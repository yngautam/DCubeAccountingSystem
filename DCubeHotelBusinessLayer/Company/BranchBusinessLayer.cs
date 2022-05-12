using DCubeHotelDomain.Models;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer
{
  public class BranchBusinessLayer
  {
    public static List<Branch> GetBranch(IDCubeRepository<Branch> BranchRepository)
    {
      List<Branch> branch = new List<Branch>();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          branch = BranchRepository.GetAllData().ToList<Branch>();
          return branch;
        }
        catch (Exception ex)
        {
          branch = (List<Branch>) null;
        }
        unitOfWork.CommitTransaction();
      }
      return branch;
    }

    public static Branch GetBranch(IDCubeRepository<Branch> BranchRepository, int Id)
    {
      Branch branch = new Branch();
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          branch = BranchRepository.GetAllData().Where<Branch>((Func<Branch, bool>) (o => o.Id == Id)).FirstOrDefault<Branch>();
          return branch;
        }
        catch (Exception ex)
        {
          branch = (Branch) null;
        }
        unitOfWork.CommitTransaction();
      }
      return branch;
    }

    public static int PostBranch(IDCubeRepository<Branch> BranchRepository, Branch value)
    {
      int num = 1;
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          BranchRepository.Insert(value);
          BranchRepository.Save();
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

    public static int UpdateBranch(IDCubeRepository<Branch> BranchRepository, Branch value, int id)
    {
      int num = 1;
      if (id >= 1)
      {
        using (UnitOfWork unitOfWork = new UnitOfWork())
        {
          try
          {
            unitOfWork.StartTransaction();
            BranchRepository.Update(value);
            BranchRepository.Save();
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

    public static int DeleteBranch(IDCubeRepository<Branch> BranchRepository, int id)
    {
      int num = 1;
      using (UnitOfWork unitOfWork = new UnitOfWork())
      {
        try
        {
          unitOfWork.StartTransaction();
          BranchRepository.Delete((object) id);
          BranchRepository.Save();
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
