using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.HotelReservationBL
{
    public static class MenuBusinessLayer
    {
        public static List<Menu> GetMenu(
          IDCubeRepository<Menu> MenuRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo)
        {
            List<Menu> menu = new List<Menu>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    menu = MenuRepository.GetAllData().ToList<Menu>();
                    return menu;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    menu = (List<Menu>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return menu;
        }

        public static int PostMenu(
          IDCubeRepository<Menu> MenuRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          Menu value)
        {
            int num = 1;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    MenuRepository.Insert(value);
                    MenuRepository.Save();
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

        public static int UpdateMenu(
          IDCubeRepository<Menu> MenuRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          int id,
          Menu value)
        {
            int num = 1;
            if (id >= 1)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.StartTransaction();
                        MenuRepository.Update(value);
                        MenuRepository.Save();
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

        public static int DeleteMenu(
          IDCubeRepository<Menu> MenuRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          int id)
        {
            int num = 1;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    MenuRepository.Delete((object)id);
                    MenuRepository.Save();
                    num = 1;
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    num = 0;
                }
            }
            return num;
        }

        public static List<MenuItemWithPrice> GetMenuCategoryItem(
          DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepository,
          IDCubeRepository<MenuItem> MenuItemRepo,
          IDCubeRepository<MenuItemPortion> Menuportionrepo,
          IDCubeRepository<MenuItemPortionPriceRange> MenuportionPriceRangerepo)
        {
            List<MenuItemWithPrice> source = new List<MenuItemWithPrice>();
            List<MenuItemWithPrice> menuCategoryItem;
            try
            {
                IEnumerable<DCubeHotelDomain.Models.MenuCategory.MenuCategory> allData1 = MenuCategoryRepository.GetAllData();
                IEnumerable<MenuItem> allData2 = MenuItemRepo.GetAllData();
                IEnumerable<MenuItemPortion> allData3 = Menuportionrepo.GetAllData();
                IEnumerable<MenuItemPortionPriceRange> allData4 = MenuportionPriceRangerepo.GetAllData();
                foreach (var data in allData2.Join(allData1, (Func<MenuItem, int>)(MCI => MCI.categoryId), (Func<DCubeHotelDomain.Models.MenuCategory.MenuCategory, int>)(mc => mc.Id), (MCI, mc) => new
                {
                    Id = mc.Id,
                    Name = mc.Name,
                    ItemId = MCI.Id
                }).Join(allData2, mc => mc.ItemId, (Func<MenuItem, int>)(MI => MI.Id), (mc, MI) => new
                {
                    Id = mc.Id,
                    Name = MI.Name,
                    ItemId = mc.ItemId,
                    categoryId = mc.Id,
                    DepartmentId = MI.DepartmentId,
                    MarginRate = MI.MarginRate,
                    TaxRate = MI.TaxRate,
                    ExciseDuty = MI.ExciseDuty,
                    CostPrice = MI.CostPrice,
                    IdentityFileName = MI.IdentityFileName,
                    IdentityFileType = MI.IdentityFileType,
                    PhoteIdentity = MI.PhoteIdentity,
                    UnitTypeBase = MI.UnitTypeBase,
                    UnitType = MI.UnitType,
                    UnitDivided = MI.UnitDivided
                }).Join(allData3, mci => mci.ItemId, (Func<MenuItemPortion, int>)(mp => mp.MenuItemPortionId), (mci, mp) => new
                {
                    Name = mci.Name + " " + mp.Name + " - " + mp.ItemCode,
                    Id = mp.Id,
                    ItemId = mp.Id,
                    CategoryId = mci.categoryId,
                    UnitPrice = mp.Price,
                    Qty = mp.Multiplier,
                    DepartmentId = mci.DepartmentId,
                    MarginRate = mci.MarginRate,
                    TaxRate = mci.TaxRate,
                    ExciseDuty = mci.ExciseDuty,
                    CostPrice = mci.CostPrice,
                    IdentityFileName = mci.IdentityFileName,
                    IdentityFileType = mci.IdentityFileType,
                    PhoteIdentity = mci.PhoteIdentity,
                    UnitTypeBase = mci.UnitTypeBase,
                    UnitType = mci.UnitType,
                    UnitDivided = mci.UnitDivided
                }))
                {
                    var menuposition = data;
                    List<MenuItemPortionPriceRange> portionPriceRangeList = new List<MenuItemPortionPriceRange>();
                    List<MenuItemPortionPriceRange> list = allData4.Where<MenuItemPortionPriceRange>((Func<MenuItemPortionPriceRange, bool>)(o => o.PortionId == menuposition.Id)).ToList<MenuItemPortionPriceRange>();
                    if (list.Count < 1)
                        source.Add(new MenuItemWithPrice()
                        {
                            CategoryId = menuposition.CategoryId,
                            Id = menuposition.Id,
                            Name = menuposition.Name,
                            ItemId = menuposition.ItemId,
                            Qty = menuposition.Qty,
                            CostPrice = menuposition.CostPrice,
                            UnitPrice = menuposition.UnitPrice,
                            DepartmentId = menuposition.DepartmentId,
                            IdentityFileName = menuposition.IdentityFileName,
                            IdentityFileType = menuposition.IdentityFileType,
                            MarginRate = menuposition.MarginRate,
                            TaxRate = menuposition.TaxRate,
                            ExciseDuty = menuposition.ExciseDuty,
                            PhoteIdentity = menuposition.PhoteIdentity,
                            UnitDivided = menuposition.UnitDivided,
                            UnitType = menuposition.UnitType,
                            UnitTypeBase = menuposition.UnitTypeBase
                        });
                    foreach (MenuItemPortionPriceRange portionPriceRange in list)
                        source.Add(new MenuItemWithPrice()
                        {
                            CategoryId = menuposition.CategoryId,
                            Id = menuposition.Id,
                            Name = menuposition.Name,
                            ItemId = menuposition.ItemId,
                            Qty = portionPriceRange.QtyMin,
                            CostPrice = menuposition.CostPrice,
                            UnitPrice = portionPriceRange.Price,
                            DepartmentId = menuposition.DepartmentId,
                            IdentityFileName = menuposition.IdentityFileName,
                            IdentityFileType = menuposition.IdentityFileType,
                            MarginRate = menuposition.MarginRate,
                            TaxRate = menuposition.TaxRate,
                            ExciseDuty = menuposition.ExciseDuty,
                            PhoteIdentity = menuposition.PhoteIdentity
                        });
                }
                return source.OrderBy<MenuItemWithPrice, string>((Func<MenuItemWithPrice, string>)(o => o.Name)).ToList<MenuItemWithPrice>();
            }
            catch (Exception ex)
            {
                menuCategoryItem = (List<MenuItemWithPrice>)null;
            }
            return menuCategoryItem;
        }

        public static List<MenuItemWithPrice> GetMenuCategoryItem(
          IDCubeRepository<MenuCategoryItem> Menucatrepo,
          DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepository,
          IDCubeRepository<MenuItem> MenuItemRepo,
          IDCubeRepository<MenuItemPortion> Menuportionrepo)
        {
            List<MenuItemWithPrice> menuCategoryItem1 = new List<MenuItemWithPrice>();
            List<MenuItemWithPrice> menuCategoryItem2;
            try
            {
                IEnumerable<DCubeHotelDomain.Models.MenuCategory.MenuCategory> allData1 = MenuCategoryRepository.GetAllData();
                IEnumerable<MenuCategoryItem> allData2 = Menucatrepo.GetAllData();
                IEnumerable<MenuItem> allData3 = MenuItemRepo.GetAllData();
                IEnumerable<MenuItemPortion> allData4 = Menuportionrepo.GetAllData();
                foreach (var data in allData2.Join(allData1, (Func<MenuCategoryItem, int>)(MCI => MCI.categoryId), (Func<DCubeHotelDomain.Models.MenuCategory.MenuCategory, int>)(mc => mc.Id), (MCI, mc) => new
                {
                    Id = mc.Id,
                    Name = mc.Name,
                    ItemId = MCI.ItemId
                }).Join(allData3, mc => mc.ItemId, (Func<MenuItem, int>)(MI => MI.Id), (mc, MI) => new
                {
                    Id = mc.Id,
                    Name = MI.Name,
                    ItemId = mc.ItemId,
                    categoryId = mc.Id,
                    DepartmentId = MI.DepartmentId
                }).Join(allData4, mci => mci.ItemId, (Func<MenuItemPortion, int>)(mp => mp.MenuItemPortionId), (mci, mp) => new
                {
                    Name = mci.Name + " " + mp.Name,
                    Id = mp.Id,
                    ItemId = mp.Id,
                    CategoryId = mci.categoryId,
                    UnitPrice = mp.Price,
                    Qty = mp.Multiplier,
                    DepartmentId = mci.DepartmentId
                }))
                    menuCategoryItem1.Add(new MenuItemWithPrice()
                    {
                        CategoryId = data.CategoryId,
                        Id = data.Id,
                        Name = data.Name,
                        ItemId = data.ItemId,
                        Qty = data.Qty,
                        UnitPrice = data.UnitPrice,
                        DepartmentId = data.DepartmentId
                    });
                return menuCategoryItem1;
            }
            catch (Exception ex)
            {
                menuCategoryItem2 = (List<MenuItemWithPrice>)null;
            }
            return menuCategoryItem2;
        }

        public static List<MenuItemWithPrice> GetMenuCategoryItem(
          DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepository,
          IDCubeRepository<MenuItem> MenuItemRepo,
          IDCubeRepository<MenuItemPortion> Menuportionrepo)
        {
            List<MenuItemWithPrice> menuCategoryItem1 = new List<MenuItemWithPrice>();
            List<MenuItemWithPrice> menuCategoryItem2;
            try
            {
                IEnumerable<DCubeHotelDomain.Models.MenuCategory.MenuCategory> allData1 = MenuCategoryRepository.GetAllData();
                IEnumerable<MenuItem> allData2 = MenuItemRepo.GetAllData();
                IEnumerable<MenuItemPortion> allData3 = Menuportionrepo.GetAllData();
                foreach (var data in allData2.Join(allData1, (Func<MenuItem, int>)(MCI => MCI.categoryId), (Func<DCubeHotelDomain.Models.MenuCategory.MenuCategory, int>)(mc => mc.Id), (MCI, mc) => new
                {
                    Id = mc.Id,
                    Name = mc.Name,
                    ItemId = MCI.Id
                }).Join(allData2, mc => mc.ItemId, (Func<MenuItem, int>)(MI => MI.Id), (mc, MI) => new
                {
                    Id = mc.Id,
                    Name = MI.Name,
                    ItemId = mc.ItemId,
                    categoryId = mc.Id,
                    DepartmentId = MI.DepartmentId
                }).Join(allData3, mci => mci.ItemId, (Func<MenuItemPortion, int>)(mp => mp.MenuItemPortionId), (mci, mp) => new
                {
                    Name = mci.Name + " " + mp.Name,
                    Id = mp.Id,
                    ItemId = mp.Id,
                    CategoryId = mci.categoryId,
                    UnitPrice = mp.Price,
                    Qty = mp.Multiplier,
                    DepartmentId = mci.DepartmentId
                }))
                    menuCategoryItem1.Add(new MenuItemWithPrice()
                    {
                        CategoryId = data.CategoryId,
                        Id = data.Id,
                        Name = data.Name,
                        ItemId = data.ItemId,
                        Qty = data.Qty,
                        UnitPrice = data.UnitPrice,
                        DepartmentId = data.DepartmentId
                    });
                return menuCategoryItem1;
            }
            catch (Exception ex)
            {
                menuCategoryItem2 = (List<MenuItemWithPrice>)null;
            }
            return menuCategoryItem2;
        }
    }
}
