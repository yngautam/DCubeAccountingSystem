using DCubeHotelDomain.Models.Inventory;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelUser;
using System;
using System.Linq;

namespace DCubeHotelBusinessLayer.Inventory
{
    public static class PeriodicConsumptionItemBusiness
    {
        public static int Create(
          IDCubeRepository<PeriodicConsumptionItem> periodicConsumptionItemRepo,
          PeriodicConsumptionItem value)
        {
            int num = 1;
            PeriodicConsumptionItem periodicConsumptionItem = new PeriodicConsumptionItem();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    periodicConsumptionItem.PeriodicConsumptionId = value.PeriodicConsumptionId;
                    periodicConsumptionItem.InventoryItemId = value.InventoryItemId;
                    periodicConsumptionItem.InStock = value.InStock;
                    periodicConsumptionItem.Consumption = value.Consumption;
                    periodicConsumptionItem.Cost = value.Cost;
                    periodicConsumptionItem.PhysicalInventory = value.PhysicalInventory;
                    periodicConsumptionItemRepo.Insert(periodicConsumptionItem);
                    periodicConsumptionItemRepo.Save();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    num = 0;
                }
                unitOfWork.CommitTransaction();
            }
            return num;
        }

        public static Decimal GetReceiptSumQuantity(
          IDCubeRepository<MenuItemPortion> MenuItemPortionRepository,
          int Id)
        {
            return MenuItemPortionRepository.GetAllData().Where<MenuItemPortion>((Func<MenuItemPortion, bool>)(x => x.MenuItemPortionId == Id)).Sum<MenuItemPortion>((Func<MenuItemPortion, Decimal>)(x => x.Multiplier));
        }

        public static int GetConsumptionSumQuantity(
          IDCubeRepository<PeriodicConsumptionItem> periodicConsumptionItemRepo,
          int Id)
        {
            return Convert.ToInt32(periodicConsumptionItemRepo.GetAllData().Where<PeriodicConsumptionItem>((Func<PeriodicConsumptionItem, bool>)(x => x.InventoryItemId == Id)).Sum<PeriodicConsumptionItem>((Func<PeriodicConsumptionItem, Decimal>)(x => x.Consumption)));
        }

        public static Decimal GetCost(
          IDCubeRepository<MenuItemPortion> MenuItemPortionRepository,
          int Id)
        {
            return MenuItemPortionRepository.GetAllData().Where<MenuItemPortion>((Func<MenuItemPortion, bool>)(x => x.MenuItemPortionId == Id)).Sum<MenuItemPortion>((Func<MenuItemPortion, Decimal>)(x => x.Price));
        }
    }
}
