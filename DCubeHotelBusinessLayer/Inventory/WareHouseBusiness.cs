using DCubeHotelDomain.Models.Inventory;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using System;

namespace DCubeHotelBusinessLayer.Inventory
{
    public static class WareHouseBusiness
    {
        public static int Create(IDCubeRepository<Warehouse> warehouserepo, Warehouse value)
        {
            int num = 1;
            Warehouse warehouse = new Warehouse();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    warehouserepo.Insert(value);
                    warehouserepo.Save();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    ErrorLog.ErrorLogging(ex);
                    num = 0;
                }
                unitOfWork.CommitTransaction();
            }
            return num;
        }

        public static int Update(IDCubeRepository<Warehouse> warehouserepo, int id, Warehouse value)
        {
            int num = 1;
            Warehouse warehouse = new Warehouse();
            if (value.Id != 0)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.StartTransaction();
                        warehouserepo.Update(value);
                        warehouserepo.Save();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.RollBackTransaction();
                        ErrorLog.ErrorLogging(ex);
                        num = 0;
                    }
                    unitOfWork.CommitTransaction();
                }
            }
            return num;
        }

        public static int Delete(IDCubeRepository<Warehouse> warehouserepo, int id)
        {
            int num = 1;
            Warehouse warehouse = new Warehouse();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    warehouserepo.Delete((object)id);
                    warehouserepo.Save();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    ErrorLog.ErrorLogging(ex);
                    num = 0;
                }
                unitOfWork.CommitTransaction();
            }
            return num;
        }
    }
}
