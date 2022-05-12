using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.HotelReservationBL
{
    public static class DepartmentBusinessLayer
    {
        public static List<Department> GetDepartment(
          IDCubeRepository<Department> DepartmentRepository)
        {
            List<Department> department = new List<Department>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    department = DepartmentRepository.GetAllData().ToList<Department>();
                    return department;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    department = (List<Department>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return department;
        }

        public static Department GetDepartment(
          IDCubeRepository<Department> DepartmentRepository,
          int Id)
        {
            Department department = new Department();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    department = DepartmentRepository.GetAllData().Where<Department>((Func<Department, bool>)(o => o.Id == Id)).FirstOrDefault<Department>();
                    return department;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    department = (Department)null;
                }
                unitOfWork.CommitTransaction();
            }
            return department;
        }

        public static int PostDepartment(
          IDCubeRepository<Department> DepartmentRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          Department value)
        {
            int num = 1;
            Department department = new Department();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    DepartmentRepository.Insert(value);
                    DepartmentRepository.Save();
                    num = 1;
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

        public static int UpdateDepartment(
          IDCubeRepository<Department> DepartmentRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          Department value,
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
                        DepartmentRepository.Update(value);
                        DepartmentRepository.Save();
                        num = 1;
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.ErrorLogging(ex);
                        num = 0;
                    }
                    unitOfWork.CommitTransaction();
                }
            }
            else
                num = 0;
            return num;
        }

        public static int DeleteDepartment(
          IDCubeRepository<Department> DepartmentRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          int id)
        {
            int num = 1;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    DepartmentRepository.Delete((object)id);
                    DepartmentRepository.Save();
                    num = 1;
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
