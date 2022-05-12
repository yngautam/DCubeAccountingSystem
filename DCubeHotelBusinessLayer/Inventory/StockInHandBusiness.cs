using DCubeHotelDomain.Models.Inventory;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.Inventory
{
    public static class StockInHandBusiness
    {
        public static List<ViewInventoryItem> GetStockQuantity(
          IDCubeRepository<MenuItem> MenuItemRepository,
          IDCubeRepository<PeriodicConsumptionItem> PeriodicConsumptionItemRepository,
          IDCubeRepository<PurchaseDetails> PurchaseDetailsRepository,
          IDCubeRepository<Order> OrderRepository,
          IDCubeRepository<MenuItemPortion> MenuItemPortionRepository,
          IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepo,
          string FinancialYear)
        {
            List<ViewInventoryItem> stockQuantity = new List<ViewInventoryItem>();
            List<MenuItem> menuItemList = new List<MenuItem>();
            List<MenuItem> list1 = MenuItemRepository.GetAllData().ToList<MenuItem>();
            List<PeriodicConsumptionItem> periodicConsumptionItemList = new List<PeriodicConsumptionItem>();
            periodicConsumptionItemList = PeriodicConsumptionItemRepository.GetAllData().ToList<PeriodicConsumptionItem>();
            List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
            List<MenuItemPortion> list2 = MenuItemPortionRepository.GetAllData().ToList<MenuItemPortion>();
            List<DCubeHotelDomain.Models.MenuCategory.MenuCategory> menuCategoryList = new List<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
            List<DCubeHotelDomain.Models.MenuCategory.MenuCategory> list3 = MenuCategoryRepo.GetAllData().ToList<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
            List<PurchaseDetails> purchaseDetailsList = new List<PurchaseDetails>();
            List<PurchaseDetails> list4 = PurchaseDetailsRepository.GetAllData().Where<PurchaseDetails>((Func<PurchaseDetails, bool>)(o => o.FinancialYear == FinancialYear)).ToList<PurchaseDetails>();
            List<Order> orderList = new List<Order>();
            List<Order> list5 = OrderRepository.GetAllData().Where<Order>((Func<Order, bool>)(o => o.FinancialYear == FinancialYear)).ToList<Order>();
            var source = list4.GroupBy<PurchaseDetails, int>((Func<PurchaseDetails, int>)(data => data.InventoryItemId)).Select(cl => new
            {
                InventoryItemId = cl.Key,
                Rate = cl.Max<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(c => c.PurchaseRate))
            });
            var list6 = list4.GroupBy<PurchaseDetails, int>((Func<PurchaseDetails, int>)(data => data.InventoryItemId)).Select(cl => new
            {
                InventoryItemId = cl.Key,
                Qty = cl.Sum<PurchaseDetails>((Func<PurchaseDetails, Decimal>)(c => c.Quantity))
            }).Union(list5.GroupBy<Order, int>((Func<Order, int>)(data => data.MenuItemId)).Select(cl => new
            {
                InventoryItemId = cl.Key,
                Qty = -cl.Sum<Order>((Func<Order, Decimal>)(c => c.Quantity))
            })).GroupBy(l => l.InventoryItemId).Select(cl => new
            {
                InventoryItemId = cl.Key,
                Qty = cl.Sum(c => c.Qty)
            }).ToList();
            foreach (var data1 in list2.Join((IEnumerable<MenuItem>)list1, (Func<MenuItemPortion, int>)(c => c.MenuItemPortionId), (Func<MenuItem, int>)(mi => mi.Id), (c, mi) => new
            {
                c = c,
                mi = mi
            }).Join((IEnumerable<DCubeHotelDomain.Models.MenuCategory.MenuCategory>)list3, _param1 => _param1.mi.categoryId, (Func<DCubeHotelDomain.Models.MenuCategory.MenuCategory, int>)(mc => mc.Id), (_param1, mc) => new
            {
                Id = _param1.c.Id,
                Item = _param1.mi.Name,
                UnitType = _param1.mi.UnitType,
                MenuItemName = _param1.c.Name,
                ItemCode = _param1.c.ItemCode,
                Qty = _param1.c.OpeningStock,
                MenuCategory = mc.Name
            }))
            {
                var itemname = data1;
                ViewInventoryItem viewInventoryItem = new ViewInventoryItem();
                viewInventoryItem.Id = itemname.Id;
                viewInventoryItem.CategoryName = itemname.MenuCategory;
                viewInventoryItem.Name = itemname.Item + " " + itemname.MenuItemName;
                viewInventoryItem.ItemCode = itemname.ItemCode;
                Decimal num1 = 0M;
                Decimal num2 = 0M;
                var data2 = source.Where(o => o.InventoryItemId == itemname.Id).FirstOrDefault();
                if (data2 != null)
                {
                    num2 = data2.Rate;
                }
                else
                {
                    MenuItemPortion menuItemPortion = list2.Where<MenuItemPortion>((Func<MenuItemPortion, bool>)(o => o.Id == itemname.Id)).FirstOrDefault<MenuItemPortion>();
                    if (menuItemPortion != null)
                        num2 = menuItemPortion.OpeningStockRate;
                }
                if (list6.Count != 0)
                {
                    try
                    {
                        var data3 = list6.First(o => o.InventoryItemId == itemname.Id);
                        if (data3 != null)
                            num1 = data3.Qty;
                    }
                    catch
                    {
                    }
                }
                if (num1 != 0M)
                {
                    viewInventoryItem.Qty = itemname.Qty + num1;
                    viewInventoryItem.Rate = num2;
                    viewInventoryItem.Amount = (itemname.Qty + num1) * num2;
                }
                else
                {
                    viewInventoryItem.Rate = num2;
                    viewInventoryItem.Qty = itemname.Qty;
                    viewInventoryItem.Amount = itemname.Qty * num2;
                }
                viewInventoryItem.UnitType = itemname.UnitType;
                stockQuantity.Add(viewInventoryItem);
            }
            return stockQuantity;
        }
    }
}