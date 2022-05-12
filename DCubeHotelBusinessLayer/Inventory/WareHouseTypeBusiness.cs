using DCubeHotelDomain.Models.Inventory;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using System;

namespace DCubeHotelBusinessLayer.Inventory
{
    public static class WareHouseTypeBusiness
    {
        public static int Create(IDCubeRepository<WarehouseType> warehouseTyperepo, WarehouseType value)
        {
            int num = 1;
            WarehouseType warehouseType = new WarehouseType();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    warehouseTyperepo.Insert(value);
                    warehouseTyperepo.Save();
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
          IDCubeRepository<WarehouseType> warehouseTyperepo,
          int id,
          WarehouseType value)
        {
            int num = 1;
            WarehouseType warehouseType = new WarehouseType();
            if (id >= 1)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.StartTransaction();
                        warehouseTyperepo.Update(value);
                        warehouseTyperepo.Save();
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

        public static int Delete(IDCubeRepository<WarehouseType> warehouseTyperepo, int id)
        {
            int num = 1;
            WarehouseType warehouseType = new WarehouseType();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    warehouseTyperepo.Delete((object)id);
                    warehouseTyperepo.Save();
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
