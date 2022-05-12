using DCubeHotelDomain.Models.Inventory;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.Inventory
{
    public static class PeriodicConsumptionBusines
    {
        public static List<PeriodicConsumption> GetPeriodicConsumptions(
          IDCubeRepository<PeriodicConsumption> PeriodicConsumptionRepository,
          IDCubeRepository<PeriodicConsumptionItem> PeriodicConsumptionItemRepository)
        {
            List<PeriodicConsumption> periodicConsumptionList = new List<PeriodicConsumption>();
            List<PeriodicConsumption> periodicConsumptions = new List<PeriodicConsumption>();
            List<PeriodicConsumptionItem> periodicConsumptionItemList1 = new List<PeriodicConsumptionItem>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    if (periodicConsumptionList != null)
                        periodicConsumptionList = PeriodicConsumptionRepository.GetAllData().Where<PeriodicConsumption>((Func<PeriodicConsumption, bool>)(o => o.Name == "Stock Damage")).ToList<PeriodicConsumption>();
                    List<PeriodicConsumptionItem> list1 = PeriodicConsumptionItemRepository.GetAllData().ToList<PeriodicConsumptionItem>();
                    foreach (PeriodicConsumption periodicConsumption in periodicConsumptionList)
                    {
                        PeriodicConsumption objPeriodicConsumption = periodicConsumption;
                        List<PeriodicConsumptionItem> periodicConsumptionItemList2 = new List<PeriodicConsumptionItem>();
                        List<PeriodicConsumptionItem> list2 = list1.Where<PeriodicConsumptionItem>((Func<PeriodicConsumptionItem, bool>)(o => o.PeriodicConsumptionId == objPeriodicConsumption.Id)).ToList<PeriodicConsumptionItem>();
                        objPeriodicConsumption.PeriodicConsumptionItems = (IList<PeriodicConsumptionItem>)list2;
                        periodicConsumptions.Add(objPeriodicConsumption);
                    }
                    return periodicConsumptions;
                }
                catch (Exception ex)
                {
                    periodicConsumptions = (List<PeriodicConsumption>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return periodicConsumptions;
        }

        public static PeriodicConsumption GetPeriodicConsumption(
          IDCubeRepository<PeriodicConsumption> PeriodiConsumptionRepository,
          IDCubeRepository<PeriodicConsumptionItem> PeriodicConsumptionItemRepository,
          int Id)
        {
            PeriodicConsumption ObjPeriodicConsumption = new PeriodicConsumption();
            List<PeriodicConsumptionItem> periodicConsumptionItemList1 = new List<PeriodicConsumptionItem>();
            try
            {
                ObjPeriodicConsumption = PeriodiConsumptionRepository.GetAllData().Where<PeriodicConsumption>((Func<PeriodicConsumption, bool>)(o => o.Id == Id)).FirstOrDefault<PeriodicConsumption>();
                if (ObjPeriodicConsumption != null)
                {
                    List<PeriodicConsumptionItem> list1 = PeriodicConsumptionItemRepository.GetAllData().ToList<PeriodicConsumptionItem>();
                    List<PeriodicConsumptionItem> periodicConsumptionItemList2 = new List<PeriodicConsumptionItem>();
                    List<PeriodicConsumptionItem> list2 = list1.Where<PeriodicConsumptionItem>((Func<PeriodicConsumptionItem, bool>)(o => o.PeriodicConsumptionId == ObjPeriodicConsumption.Id)).ToList<PeriodicConsumptionItem>();
                    ObjPeriodicConsumption.PeriodicConsumptionItems = (IList<PeriodicConsumptionItem>)list2;
                }
                else
                    ObjPeriodicConsumption = (PeriodicConsumption)null;
            }
            catch (Exception ex)
            {
                ObjPeriodicConsumption = (PeriodicConsumption)null;
            }
            return ObjPeriodicConsumption;
        }

        public static int Create(
          IDCubeRepository<PeriodicConsumption> PeriodiConsumptionRepository,
          IDCubeRepository<PeriodicConsumptionItem> PeriodicConsumptionItemRepository,
          IDCubeRepository<WarehouseConsumption> WarehouseConsumptionRepository,
          PeriodicConsumption value)
        {
            int num = 1;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                unitOfWork.StartTransaction();
                try
                {
                    PeriodicConsumption periodicConsumption = new PeriodicConsumption();
                    periodicConsumption.Id = periodicConsumption.Id;
                    periodicConsumption.Name = value.Name;
                    periodicConsumption.StartDate = value.StartDate;
                    periodicConsumption.FinancialYear = value.FinancialYear;
                    periodicConsumption.UserName = value.UserName;
                    periodicConsumption.CompanyCode = value.CompanyCode;
                    PeriodiConsumptionRepository.Insert(periodicConsumption);
                    PeriodiConsumptionRepository.Save();
                    int id = PeriodiConsumptionRepository.GetAllData().OrderByDescending<PeriodicConsumption, int>((Func<PeriodicConsumption, int>)(x => x.Id)).FirstOrDefault<PeriodicConsumption>().Id;
                    foreach (PeriodicConsumptionItem periodicConsumptionItem in (IEnumerable<PeriodicConsumptionItem>)value.PeriodicConsumptionItems)
                    {
                        PeriodicConsumptionItemRepository.Insert(new PeriodicConsumptionItem()
                        {
                            Consumption = periodicConsumptionItem.Consumption,
                            Cost = periodicConsumptionItem.Cost,
                            InventoryItemId = periodicConsumptionItem.InventoryItemId,
                            InStock = periodicConsumptionItem.InStock,
                            PhysicalInventory = periodicConsumptionItem.PhysicalInventory,
                            PeriodicConsumptionId = id
                        });
                        PeriodicConsumptionItemRepository.Save();
                        num = 1;
                    }
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    ErrorLog.ErrorLogging(ex);
                    num = 0;
                }
            }
            return num;
        }

        public static int Edit(
          IDCubeRepository<PeriodicConsumption> PeriodicConsumptionRepository,
          IDCubeRepository<PeriodicConsumptionItem> PeriodicConsumptionItemRepository,
          PeriodicConsumption value)
        {
            int num = 1;
            DateTime now = DateTime.Now;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                unitOfWork.StartTransaction();
                try
                {
                    PeriodicConsumptionRepository.Update(new PeriodicConsumption()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        StartDate = value.StartDate,
                        LastUpdateTime = now
                    });
                    PeriodicConsumptionRepository.Save();
                    foreach (PeriodicConsumptionItem periodicConsumptionItem in (IEnumerable<PeriodicConsumptionItem>)value.PeriodicConsumptionItems)
                    {
                        if (periodicConsumptionItem.Id >= 1)
                        {
                            PeriodicConsumptionItemRepository.Update(new PeriodicConsumptionItem()
                            {
                                Id = periodicConsumptionItem.Id,
                                InventoryItemId = periodicConsumptionItem.InventoryItemId,
                                PeriodicConsumptionId = periodicConsumptionItem.PeriodicConsumptionId,
                                InStock = periodicConsumptionItem.InStock,
                                Consumption = periodicConsumptionItem.Consumption,
                                PhysicalInventory = periodicConsumptionItem.PhysicalInventory,
                                Cost = periodicConsumptionItem.Cost
                            });
                            PeriodicConsumptionItemRepository.Save();
                        }
                        else if (periodicConsumptionItem.Id == 0)
                        {
                            PeriodicConsumptionItemRepository.Insert(new PeriodicConsumptionItem()
                            {
                                Id = periodicConsumptionItem.Id,
                                PeriodicConsumptionId = value.Id,
                                InventoryItemId = periodicConsumptionItem.InventoryItemId,
                                InStock = periodicConsumptionItem.InStock,
                                Consumption = periodicConsumptionItem.Consumption,
                                PhysicalInventory = periodicConsumptionItem.PhysicalInventory,
                                Cost = periodicConsumptionItem.Cost
                            });
                            PeriodicConsumptionItemRepository.Save();
                        }
                    }
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    num = 0;
                }
                return num;
            }
        }

        public static int Delete(
          IDCubeRepository<PeriodicConsumption> PeriodicConsumptionRepository,
          IDCubeRepository<PeriodicConsumptionItem> PeriodicConsumptionItemRepository,
          int Id)
        {
            int num = 1;
            PeriodicConsumption periodicConsumption = new PeriodicConsumption();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                unitOfWork.StartTransaction();
                try
                {
                    PeriodicConsumptionRepository.Delete((object)Id);
                    PeriodicConsumptionRepository.Save();
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    throw ex;
                }
                return num;
            }
        }

        public static int DeletePeriodicConsumption(
          IDCubeRepository<PeriodicConsumptionItem> PeriodicConsumptionItemRepository,
          int PeriodicConsumptionId)
        {
            int num = 1;
            PeriodicConsumptionItem periodicConsumptionItem = new PeriodicConsumptionItem();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    unitOfWork.InventoryReceiptDetailsRepo.DeleteReceipts(PeriodicConsumptionId);
                    PeriodicConsumptionItemRepository.Save();
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
