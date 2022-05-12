using DCubeHotelDomain.Models.Reservation;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.HotelCustomerBusinessLayer
{
    public static class CustomerBusinessLayer
    {
        public static List<Customer> GetCustomer(
          IDCubeRepository<Customer> Customerrepository)
        {
            List<Customer> customer = new List<Customer>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    customer = Customerrepository.GetAllData().ToList<Customer>();
                    return customer;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                }
                unitOfWork.CommitTransaction();
            }
            return customer;
        }

        public static int PostCustomer(IDCubeRepository<Customer> RoomBookRepository, Customer value)
        {
            int num = 1;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    RoomBookRepository.Insert(value);
                    RoomBookRepository.Save();
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

        public static int UpdateCustomer(
          IDCubeRepository<Customer> Customerrepository,
          int id,
          Customer value)
        {
            int num = 1;
            if (id >= 1)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.StartTransaction();
                        Customerrepository.Update(value);
                        Customerrepository.Save();
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

        public static int DeleteCustomer(IDCubeRepository<Customer> Customerrepository, int id)
        {
            int num = 1;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    Customerrepository.Delete((object)id);
                    Customerrepository.Save();
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
