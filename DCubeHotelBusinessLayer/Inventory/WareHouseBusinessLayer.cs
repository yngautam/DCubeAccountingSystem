using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.HotelInventoryBusinessLayer
{
    public static class WareHouseBusinessLayer
    {
        public static List<Warehouse> GetWareHouse(
          IDCubeRepository<Warehouse> WarehouseRepository)
        {
            List<Warehouse> wareHouse = new List<Warehouse>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    wareHouse = WarehouseRepository.GetAllData().ToList<Warehouse>();
                    return wareHouse;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    wareHouse = (List<Warehouse>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return wareHouse;
        }

        public static Warehouse GetWareHouse(
          IDCubeRepository<Warehouse> WarehouseRepository,
          int Id)
        {
            Warehouse wareHouse = new Warehouse();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    wareHouse = WarehouseRepository.GetAllData().Where<Warehouse>((Func<Warehouse, bool>)(o => o.Id == Id)).FirstOrDefault<Warehouse>();
                    return wareHouse;
                }
                catch (Exception ex)
                {
                    wareHouse = (Warehouse)null;
                }
                unitOfWork.CommitTransaction();
            }
            return wareHouse;
        }

        public static int InsertWareHouse(
          IDCubeRepository<Warehouse> WarehouseRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          Warehouse value)
        {
            int num = 1;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    WarehouseRepository.Insert(value);
                    WarehouseRepository.Save();
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

        public static int UpdateWareHouse(
          IDCubeRepository<Warehouse> WarehouseRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          int id,
          Warehouse value)
        {
            int num = 1;
            if (id >= 1)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.StartTransaction();
                        WarehouseRepository.Update(value);
                        WarehouseRepository.Save();
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

        public static int DeleteWareHouse(
          IDCubeRepository<Warehouse> WarehouseRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          int id)
        {
            Warehouse warehouse = new Warehouse();
            int num = 1;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    WarehouseRepository.Delete((object)id);
                    WarehouseRepository.Save();
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

        public static List<WarehouseType> GetWareHouseType(
          IDCubeRepository<WarehouseType> WareHouseTypeRepository)
        {
            List<WarehouseType> wareHouseType = new List<WarehouseType>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    wareHouseType = WareHouseTypeRepository.GetAllData().ToList<WarehouseType>();
                    return wareHouseType;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    wareHouseType = (List<WarehouseType>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return wareHouseType;
        }

        public static WarehouseType GetWareHouseType(
          IDCubeRepository<WarehouseType> WareHouseTypeRepository,
          int id)
        {
            WarehouseType wareHouseType = new WarehouseType();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    wareHouseType = WareHouseTypeRepository.GetAllData().Where<WarehouseType>((Func<WarehouseType, bool>)(o => o.Id == id)).FirstOrDefault<WarehouseType>();
                    return wareHouseType;
                }
                catch (Exception ex)
                {
                    wareHouseType = (WarehouseType)null;
                }
                unitOfWork.CommitTransaction();
            }
            return wareHouseType;
        }

        public static int InsertWareHouseType(
          IDCubeRepository<WarehouseType> WareHouseTypeRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          WarehouseType value)
        {
            int num = 1;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    WareHouseTypeRepository.Insert(value);
                    WareHouseTypeRepository.Save();
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

        public static int UpdateWareHouseType(
          IDCubeRepository<WarehouseType> WareHouseTypeRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          int id,
          WarehouseType value)
        {
            int num = 0;
            if (id >= 1)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.StartTransaction();
                        WareHouseTypeRepository.Update(value);
                        WareHouseTypeRepository.Save();
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

        public static int DeleteWareHouseType(
          IDCubeRepository<WarehouseType> WareHouseTypeRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          int id)
        {
            int num = 1;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    WareHouseTypeRepository.Delete((object)id);
                    WareHouseTypeRepository.Save();
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