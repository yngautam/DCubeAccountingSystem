using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.HotelMenuBusinessLayer
{
    public static class MenuItemBusinessLayer
    {
        public static List<MenuItemPortion> GetItemPortions(
          IDCubeRepository<MenuItemPortion> menuportionrepository,
          IDCubeRepository<MenuItemPortionPriceRange> MenuItemPortionPriceRangerepository,
          IDCubeRepository<ExceptionLog> exceptionrepo)
        {
            List<MenuItemPortion> itemPortions1 = new List<MenuItemPortion>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    List<MenuItemPortionPriceRange> list1 = MenuItemPortionPriceRangerepository.GetAllData().ToList<MenuItemPortionPriceRange>();
                    itemPortions1 = menuportionrepository.GetAllData().ToList<MenuItemPortion>();
                    List<MenuItemPortion> itemPortions2 = new List<MenuItemPortion>();
                    foreach (MenuItemPortion menuItemPortion in itemPortions1)
                    {
                        MenuItemPortion position = menuItemPortion;
                        List<MenuItemPortionPriceRange> portionPriceRangeList = new List<MenuItemPortionPriceRange>();
                        List<MenuItemPortionPriceRange> list2 = list1.Where<MenuItemPortionPriceRange>((Func<MenuItemPortionPriceRange, bool>)(o => o.PortionId == position.Id)).ToList<MenuItemPortionPriceRange>();
                        position.MenuItemPortionPriceRanges = (IEnumerable<MenuItemPortionPriceRange>)list2;
                        itemPortions2.Add(position);
                    }
                    return itemPortions2;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    itemPortions1 = (List<MenuItemPortion>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return itemPortions1;
        }
        public static List<MenuItemPortion> GetListItemPortions(IDCubeRepository<MenuItem> MenuItemrepository, IDCubeRepository<MenuItemPortion> menuportionrepository, IDCubeRepository<ExceptionLog> exceptionrepo)
        {
            List<MenuItemPortion> ListMenuItemPortion = new List<MenuItemPortion>();
            List<MenuItemPortion> listmenuitemportion = new List<MenuItemPortion>();
            List<MenuItem> listMenuItem = new List<MenuItem>();
            ListMenuItemPortion = menuportionrepository.GetAllData().ToList();
            listMenuItem = MenuItemrepository.GetAllData().ToList();
            var listmenuitemposition = from p in ListMenuItemPortion join i in listMenuItem on p.MenuItemPortionId equals i.Id select new { p.Id, p.Multiplier, p.Name, p.Price, p.ItemCode, p.OpeningStock, ItemPortionId = p.Id, ProductName = i.Name };

            foreach (var p in listmenuitemposition)
            {
                try
                {
                    MenuItemPortion objMenuItemPortion = new MenuItemPortion();
                    objMenuItemPortion.Id = p.Id;
                    objMenuItemPortion.MenuItemPortionId = p.ItemPortionId;
                    objMenuItemPortion.Multiplier = p.Multiplier;
                    objMenuItemPortion.Name = p.ProductName + " " + p.Name;
                    objMenuItemPortion.Price = p.Price;
                    objMenuItemPortion.ItemCode = p.ItemCode;
                    objMenuItemPortion.OpeningStock = p.OpeningStock;
                    listmenuitemportion.Add(objMenuItemPortion);
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    listmenuitemportion = null;
                }
            }
            return listmenuitemportion;
        }
        public static List<MenuItem> GetMenuItemList(
      IDCubeRepository<MenuItem> menuitemrepository,
      IDCubeRepository<MenuItemPhoto> MenuItemPhotorepository,
      IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> menucategoryrepository,
      IDCubeRepository<MenuItemPortion> menuportionrepository,
      IDCubeRepository<MenuItemPortionPriceRange> MenuItemPortionPriceRangerepository,
      IDCubeRepository<ExceptionLog> exceptionrepo)
        {
            List<MenuItem> source = new List<MenuItem>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    List<MenuItem> list1 = menuitemrepository.GetAllData().ToList<MenuItem>();
                    List<MenuItemPortionPriceRange> list2 = MenuItemPortionPriceRangerepository.GetAllData().ToList<MenuItemPortionPriceRange>();
                    List<DCubeHotelDomain.Models.MenuCategory.MenuCategory> list3 = menucategoryrepository.GetAllData().ToList<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
                    List<MenuItemPhoto> list4 = MenuItemPhotorepository.GetAllData().ToList<MenuItemPhoto>();
                    List<MenuItem> menuItemList = new List<MenuItem>();
                    foreach (var data in MenuItemBusinessLayer.LoadMenuItem(list1, menuportionrepository, MenuItemPortionPriceRangerepository).Join((IEnumerable<DCubeHotelDomain.Models.MenuCategory.MenuCategory>)list3, (Func<MenuItem, int>)(mi => mi.categoryId), (Func<DCubeHotelDomain.Models.MenuCategory.MenuCategory, int>)(mc => mc.Id), (mi, mc) => new
                    {
                        Id = mi.Id,
                        CostPrice = mi.CostPrice,
                        Name = mi.Name,
                        BranchId = mi.BranchId,
                        IsMenuItem = mi.IsMenuItem,
                        IsProduct = mi.IsProduct,
                        IsService = mi.IsService,
                        UnitDivided = mi.UnitDivided,
                        UnitTypeBase = mi.UnitTypeBase,
                        WareHouseId = mi.WareHouseId,
                        Barcode = mi.Barcode,
                        categoryId = mi.categoryId,
                        Tag = mi.Tag,
                        MenuItemPortions = mi.MenuItemPortions,
                        MenuItemPhotos = mi.MenuItemPhotos,
                        ExciseDuty = mi.ExciseDuty,
                        TaxRate = mi.TaxRate,
                        MarginRate = mi.MarginRate,
                        UnitType = mi.UnitType,
                        Description = mi.Description,
                        MetaDescription = mi.MetaDescription,
                        DepartmentId = mi.DepartmentId,
                        PhoteIdentity = mi.PhoteIdentity,
                        IdentityFileName = mi.IdentityFileName,
                        IdentityFileType = mi.IdentityFileType
                    }))
                    {
                        var menucategory = data;
                        MenuItem menuItem = new MenuItem();
                        menuItem.Id = menucategory.Id;
                        menuItem.CostPrice = menucategory.CostPrice;
                        menuItem.Name = menucategory.Name;
                        menuItem.categoryId = menucategory.categoryId;
                        menuItem.Barcode = menucategory.Barcode;
                        menuItem.Tag = menucategory.Tag;
                        menuItem.DepartmentId = menucategory.DepartmentId;
                        menuItem.Description = menucategory.Description;
                        menuItem.MetaDescription = menucategory.MetaDescription;
                        menuItem.ExciseDuty = menucategory.ExciseDuty;
                        menuItem.TaxRate = menucategory.TaxRate;
                        menuItem.MarginRate = menucategory.MarginRate;
                        menuItem.UnitType = menucategory.UnitType;
                        menuItem.BranchId = menucategory.BranchId;
                        menuItem.IsMenuItem = menucategory.IsMenuItem;
                        menuItem.IsProduct = menucategory.IsProduct;
                        menuItem.IsService = menucategory.IsService;
                        menuItem.UnitDivided = menucategory.UnitDivided;
                        menuItem.UnitTypeBase = menucategory.UnitTypeBase;
                        menuItem.WareHouseId = menucategory.WareHouseId;
                        menuItem.IdentityFileName = menucategory.IdentityFileName;
                        menuItem.IdentityFileType = menucategory.IdentityFileType;
                        List<MenuItemPhoto> menuItemPhotoList = new List<MenuItemPhoto>();
                        List<MenuItemPhoto> list5 = list4.Where<MenuItemPhoto>((Func<MenuItemPhoto, bool>)(o => o.MenuItemPortionId == menucategory.Id)).ToList<MenuItemPhoto>();
                        menuItem.MenuItemPhotos = (IEnumerable<MenuItemPhoto>)list5;
                        menuItem.PhoteIdentity = menucategory.PhoteIdentity;
                        List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
                        foreach (MenuItemPortion menuItemPortion in menucategory.MenuItemPortions)
                        {
                            MenuItemPortion position = menuItemPortion;
                            List<MenuItemPortionPriceRange> portionPriceRangeList = new List<MenuItemPortionPriceRange>();
                            List<MenuItemPortionPriceRange> list6 = list2.Where<MenuItemPortionPriceRange>((Func<MenuItemPortionPriceRange, bool>)(o => o.PortionId == position.Id)).ToList<MenuItemPortionPriceRange>();
                            position.MenuItemPortionPriceRanges = (IEnumerable<MenuItemPortionPriceRange>)list6;
                            menuItemPortionList.Add(position);
                        }
                        menuItem.MenuItemPortions = (IEnumerable<MenuItemPortion>)menuItemPortionList;
                        source.Add(menuItem);
                    }
                    source = source.OrderBy<MenuItem, string>((Func<MenuItem, string>)(o => o.Name)).ToList<MenuItem>();
                    return source;
                }
                catch (Exception ex)
                {
                    source = (List<MenuItem>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return source;
        }

        public static List<MenuItem> GetMenuItems(
          IDCubeRepository<MenuItem> menuitemrepository,
          IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> menucategoryrepository,
          IDCubeRepository<MenuItemPortion> menuportionrepository,
          IDCubeRepository<MenuItemPortionPriceRange> MenuItemPortionPriceRangerepository,
          IDCubeRepository<MenuItemPhoto> MenuItemPhotoRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          int id)
        {
            List<MenuItem> source = new List<MenuItem>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    List<MenuItem> list1 = menuitemrepository.GetAllData().Where<MenuItem>((Func<MenuItem, bool>)(o => o.Id == id)).ToList<MenuItem>();
                    List<DCubeHotelDomain.Models.MenuCategory.MenuCategory> list2 = menucategoryrepository.GetAllData().ToList<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
                    List<MenuItem> menuItemList = new List<MenuItem>();
                    List<MenuItem> outer = MenuItemBusinessLayer.LoadMenuItem(list1, menuportionrepository, MenuItemPortionPriceRangerepository);
                    List<MenuItemPortionPriceRange> list3 = MenuItemPortionPriceRangerepository.GetAllData().ToList<MenuItemPortionPriceRange>();
                    List<MenuItemPhoto> list4 = MenuItemPhotoRepository.GetAllData().ToList<MenuItemPhoto>();
                    foreach (var data in outer.Join((IEnumerable<DCubeHotelDomain.Models.MenuCategory.MenuCategory>)list2, (Func<MenuItem, int>)(mi => mi.categoryId), (Func<DCubeHotelDomain.Models.MenuCategory.MenuCategory, int>)(mc => mc.Id), (mi, mc) => new
                    {
                        Id = mi.Id,
                        CostPrice = mi.CostPrice,
                        Name = mi.Name,
                        BranchId = mi.BranchId,
                        IsMenuItem = mi.IsMenuItem,
                        IsProduct = mi.IsProduct,
                        IsService = mi.IsService,
                        UnitDivided = mi.UnitDivided,
                        UnitTypeBase = mi.UnitTypeBase,
                        WareHouseId = mi.WareHouseId,
                        Barcode = mi.Barcode,
                        categoryId = mi.categoryId,
                        Tag = mi.Tag,
                        MenuItemPortions = mi.MenuItemPortions,
                        MenuItemPhotos = mi.MenuItemPhotos,
                        ExciseDuty = mi.ExciseDuty,
                        TaxRate = mi.TaxRate,
                        MarginRate = mi.MarginRate,
                        UnitType = mi.UnitType,
                        Description = mi.Description,
                        MetaDescription = mi.MetaDescription,
                        DepartmentId = mi.DepartmentId,
                        PhoteIdentity = mi.PhoteIdentity,
                        IdentityFileName = mi.IdentityFileName,
                        IdentityFileType = mi.IdentityFileType
                    }))
                    {
                        var menucategory = data;
                        MenuItem menuItem = new MenuItem();
                        menuItem.Id = menucategory.Id;
                        menuItem.CostPrice = menucategory.CostPrice;
                        menuItem.Name = menucategory.Name;
                        menuItem.categoryId = menucategory.categoryId;
                        menuItem.Barcode = menucategory.Barcode;
                        menuItem.Tag = menucategory.Tag;
                        menuItem.DepartmentId = menucategory.DepartmentId;
                        menuItem.Description = menucategory.Description;
                        menuItem.MetaDescription = menucategory.MetaDescription;
                        menuItem.ExciseDuty = menucategory.ExciseDuty;
                        menuItem.TaxRate = menucategory.TaxRate;
                        menuItem.MarginRate = menucategory.MarginRate;
                        menuItem.UnitType = menucategory.UnitType;
                        menuItem.BranchId = menucategory.BranchId;
                        menuItem.IsMenuItem = menucategory.IsMenuItem;
                        menuItem.IsProduct = menucategory.IsProduct;
                        menuItem.IsService = menucategory.IsService;
                        menuItem.UnitDivided = menucategory.UnitDivided;
                        menuItem.UnitTypeBase = menucategory.UnitTypeBase;
                        menuItem.WareHouseId = menucategory.WareHouseId;
                        menuItem.IdentityFileName = menucategory.IdentityFileName;
                        menuItem.IdentityFileType = menucategory.IdentityFileType;
                        menuItem.PhoteIdentity = menucategory.PhoteIdentity;
                        List<MenuItemPhoto> menuItemPhotoList = new List<MenuItemPhoto>();
                        List<MenuItemPhoto> list5 = list4.Where<MenuItemPhoto>((Func<MenuItemPhoto, bool>)(o => o.MenuItemPortionId == menucategory.Id)).ToList<MenuItemPhoto>();
                        menuItem.MenuItemPhotos = (IEnumerable<MenuItemPhoto>)list5;
                        List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
                        foreach (MenuItemPortion menuItemPortion in menucategory.MenuItemPortions)
                        {
                            MenuItemPortion position = menuItemPortion;
                            List<MenuItemPortionPriceRange> portionPriceRangeList = new List<MenuItemPortionPriceRange>();
                            List<MenuItemPortionPriceRange> list6 = list3.Where<MenuItemPortionPriceRange>((Func<MenuItemPortionPriceRange, bool>)(o => o.PortionId == position.Id)).ToList<MenuItemPortionPriceRange>();
                            position.MenuItemPortionPriceRanges = (IEnumerable<MenuItemPortionPriceRange>)list6;
                            menuItemPortionList.Add(position);
                        }
                        menuItem.MenuItemPortions = (IEnumerable<MenuItemPortion>)menuItemPortionList;
                        source.Add(menuItem);
                    }
                    source = source.OrderBy<MenuItem, string>((Func<MenuItem, string>)(o => o.Name)).ToList<MenuItem>();
                    return source;
                }
                catch (Exception ex)
                {
                    source = (List<MenuItem>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return source;
        }

        private static List<MenuItem> LoadMenuItem(
          List<MenuItem> MenuItems,
          IDCubeRepository<MenuItemPortion> menuportionrepository,
          IDCubeRepository<MenuItemPortionPriceRange> MenuItemPortionPriceRangerepository)
        {
            List<MenuItemPortion> menuItemPortionList1 = new List<MenuItemPortion>();
            List<MenuItemPortion> list1 = menuportionrepository.GetAllData().ToList<MenuItemPortion>();
            List<MenuItem> menuItemList = new List<MenuItem>();
            List<MenuItemPortion.ScreenMenuItemPortion> screenMenuItemPortionList = new List<MenuItemPortion.ScreenMenuItemPortion>();
            List<MenuItemPortionPriceRange> list2 = MenuItemPortionPriceRangerepository.GetAllData().ToList<MenuItemPortionPriceRange>();
            foreach (MenuItem menuItem1 in MenuItems)
            {
                MenuItem ObjMenuItem = menuItem1;
                MenuItem menuItem2 = new MenuItem();
                menuItem2.BranchId = ObjMenuItem.BranchId;
                menuItem2.CostPrice = ObjMenuItem.CostPrice;
                menuItem2.IsProduct = ObjMenuItem.IsProduct;
                menuItem2.IsMenuItem = ObjMenuItem.IsMenuItem;
                menuItem2.IsService = ObjMenuItem.IsService;
                menuItem2.UnitDivided = ObjMenuItem.UnitDivided;
                menuItem2.UnitTypeBase = ObjMenuItem.UnitTypeBase;
                menuItem2.WareHouseId = ObjMenuItem.WareHouseId;
                menuItem2.Id = ObjMenuItem.Id;
                menuItem2.Name = ObjMenuItem.Name;
                menuItem2.Barcode = ObjMenuItem.Barcode;
                menuItem2.Tag = ObjMenuItem.Tag;
                menuItem2.categoryId = ObjMenuItem.categoryId;
                menuItem2.DepartmentId = ObjMenuItem.DepartmentId;
                menuItem2.Description = ObjMenuItem.Description;
                menuItem2.MetaDescription = ObjMenuItem.MetaDescription;
                menuItem2.ExciseDuty = ObjMenuItem.ExciseDuty;
                menuItem2.TaxRate = ObjMenuItem.TaxRate;
                menuItem2.MarginRate = ObjMenuItem.MarginRate;
                menuItem2.UnitType = ObjMenuItem.UnitType;
                menuItem2.IdentityFileName = ObjMenuItem.IdentityFileName;
                menuItem2.IdentityFileType = ObjMenuItem.IdentityFileType;
                menuItem2.PhoteIdentity = ObjMenuItem.PhoteIdentity;
                menuItem2.MenuItemPhotos = ObjMenuItem.MenuItemPhotos;
                List<MenuItemPortion> menuItemPortionList2 = new List<MenuItemPortion>();
                List<MenuItemPortion> list3 = list1.Where<MenuItemPortion>((Func<MenuItemPortion, bool>)(x => x.MenuItemPortionId == ObjMenuItem.Id)).ToList<MenuItemPortion>();
                List<MenuItemPortion> menuItemPortionList3 = new List<MenuItemPortion>();
                foreach (MenuItemPortion menuItemPortion in list3)
                {
                    MenuItemPortion position = menuItemPortion;
                    List<MenuItemPortionPriceRange> portionPriceRangeList = new List<MenuItemPortionPriceRange>();
                    List<MenuItemPortionPriceRange> list4 = list2.Where<MenuItemPortionPriceRange>((Func<MenuItemPortionPriceRange, bool>)(o => o.PortionId == position.Id)).ToList<MenuItemPortionPriceRange>();
                    position.MenuItemPortionPriceRanges = (IEnumerable<MenuItemPortionPriceRange>)list4;
                    menuItemPortionList3.Add(position);
                }
                menuItem2.MenuItemPortions = (IEnumerable<MenuItemPortion>)menuItemPortionList3;
                menuItemList.Add(menuItem2);
            }
            return menuItemList;
        }

        public static int PostMenuItem(
          IDCubeRepository<MenuItem> menuitemrepository,
          IDCubeRepository<MenuItemPortion> menuportionrepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          MenuItem value,
          IDCubeRepository<MenuItemPortionPriceRange> MenuItemPortionPriceRangerepository)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    MenuItem menuItem = new MenuItem();
                    List<MenuItem> menuItemList = new List<MenuItem>();
                    menuItem.Id = value.Id;
                    menuItem.CostPrice = value.CostPrice;
                    menuItem.Name = value.Name;
                    menuItem.categoryId = value.categoryId;
                    menuItem.Tag = value.Tag;
                    menuItem.Barcode = value.Barcode;
                    menuItem.UnitType = value.UnitType;
                    menuItem.DepartmentId = value.DepartmentId;
                    menuItem.Description = value.Description;
                    menuItem.MetaDescription = value.MetaDescription;
                    menuItem.ExciseDuty = value.ExciseDuty;
                    menuItem.TaxRate = value.TaxRate;
                    menuItem.MarginRate = value.MarginRate;
                    menuItem.BranchId = value.BranchId;
                    menuItem.IsProduct = value.IsProduct;
                    menuItem.IsMenuItem = value.IsMenuItem;
                    menuItem.IsService = value.IsService;
                    menuItem.UnitDivided = value.UnitDivided;
                    menuItem.UnitTypeBase = value.UnitTypeBase;
                    menuItem.WareHouseId = value.WareHouseId;
                    menuitemrepository.Insert(menuItem);
                    menuitemrepository.Save();
                    int id1 = menuitemrepository.GetAllData().OrderByDescending<MenuItem, int>((Func<MenuItem, int>)(x => x.Id)).FirstOrDefault<MenuItem>().Id;
                    foreach (MenuItemPortion menuItemPortion1 in value.MenuItemPortions)
                    {
                        MenuItemPortion menuItemPortion2 = new MenuItemPortion()
                        {
                            MenuItemPortionId = id1,
                            Name = menuItemPortion1.Name,
                            Multiplier = menuItemPortion1.Multiplier,
                            Price = menuItemPortion1.Price,
                            Discount = menuItemPortion1.Discount,
                            ItemCode = menuItemPortion1.ItemCode,
                            OpeningStock = menuItemPortion1.OpeningStock
                        };
                        menuItemPortion2.Multiplier = menuItemPortion1.Multiplier;
                        menuItemPortion2.OpeningStock = menuItemPortion1.OpeningStock;
                        menuItemPortion2.OpeningStockAmount = menuItemPortion1.OpeningStockAmount;
                        menuItemPortion2.OpeningStockRate = menuItemPortion1.OpeningStockRate;
                        menuItemPortion2.StockLimit = menuItemPortion1.StockLimit;
                        menuportionrepository.Insert(menuItemPortion2);
                        menuportionrepository.Save();
                        int id2 = menuportionrepository.GetAllData().OrderByDescending<MenuItemPortion, int>((Func<MenuItemPortion, int>)(x => x.Id)).FirstOrDefault<MenuItemPortion>().Id;
                        foreach (MenuItemPortionPriceRange portionPriceRange in menuItemPortion1.MenuItemPortionPriceRanges)
                        {
                            MenuItemPortionPriceRangerepository.Insert(new MenuItemPortionPriceRange()
                            {
                                Id = portionPriceRange.Id,
                                PortionId = id2,
                                Price = portionPriceRange.Price,
                                QtyMax = portionPriceRange.QtyMax,
                                QtyMin = portionPriceRange.QtyMin
                            });
                            MenuItemPortionPriceRangerepository.Save();
                        }
                        num = id1;
                    }
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    num = 0;
                }
            }
            return num;
        }

        public static int PostMenuItemPhoto(
          IDCubeRepository<MenuItemPhoto> MenuItemPhotoRepository,
          MenuItemPhoto value)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    MenuItemPhotoRepository.Insert(value);
                    MenuItemPhotoRepository.Save();
                    num = MenuItemPhotoRepository.GetAllData().OrderByDescending<MenuItemPhoto, int>((Func<MenuItemPhoto, int>)(x => x.Id)).FirstOrDefault<MenuItemPhoto>().Id;
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    num = 0;
                }
            }
            return num;
        }

        public static int UpdateMenuItemPhoto(
          IDCubeRepository<MenuItemPhoto> MenuItemPhotoRepository,
          MenuItemPhoto value,
          int Id)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    MenuItemPhoto menuItemPhoto1 = new MenuItemPhoto();
                    MenuItemPhoto menuItemPhoto2 = MenuItemPhotoRepository.GetAllData().Where<MenuItemPhoto>((Func<MenuItemPhoto, bool>)(o => o.Id == Id)).FirstOrDefault<MenuItemPhoto>();
                    menuItemPhoto2.Name = value.Name;
                    menuItemPhoto2.Sequence = value.Sequence;
                    MenuItemPhotoRepository.Update(menuItemPhoto2);
                    MenuItemPhotoRepository.Save();
                    num = Id;
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    num = 0;
                }
            }
            return num;
        }

        public static int UpdateMenuItem(
          IDCubeRepository<MenuItem> menuitemrepository,
          IDCubeRepository<MenuItemPortion> menuportionrepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          int id,
          MenuItem value,
          IDCubeRepository<MenuItemPortionPriceRange> MenuItemPortionPriceRangerepository)
        {
            int num = 0;
            if (id >= 1)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    try
                    {
                        unitOfWork.StartTransaction();
                        MenuItem menuItem = new MenuItem();
                        menuItem.CostPrice = value.CostPrice;
                        menuItem.Id = value.Id;
                        menuItem.Name = value.Name;
                        menuItem.categoryId = value.categoryId;
                        menuItem.Tag = value.Tag;
                        menuItem.Barcode = value.Barcode;
                        menuItem.UnitType = value.UnitType;
                        menuItem.DepartmentId = value.DepartmentId;
                        menuItem.Description = value.Description;
                        menuItem.MetaDescription = value.MetaDescription;
                        menuItem.ExciseDuty = value.ExciseDuty;
                        menuItem.TaxRate = value.TaxRate;
                        menuItem.MarginRate = value.MarginRate;
                        menuItem.BranchId = value.BranchId;
                        menuItem.IsProduct = value.IsProduct;
                        menuItem.IsMenuItem = value.IsMenuItem;
                        menuItem.IsService = value.IsService;
                        menuItem.UnitDivided = value.UnitDivided;
                        menuItem.UnitTypeBase = value.UnitTypeBase;
                        menuItem.WareHouseId = value.WareHouseId;
                        menuitemrepository.Update(menuItem);
                        menuitemrepository.Save();
                        foreach (MenuItemPortion menuItemPortion in value.MenuItemPortions)
                        {
                            if (menuItemPortion.Id == 0)
                            {
                                menuportionrepository.Insert(new MenuItemPortion()
                                {
                                    MenuItemPortionId = menuItem.Id,
                                    Name = menuItemPortion.Name,
                                    Multiplier = menuItemPortion.Multiplier,
                                    Price = menuItemPortion.Price,
                                    Discount = menuItemPortion.Discount,
                                    ItemCode = menuItemPortion.ItemCode,
                                    OpeningStock = menuItemPortion.OpeningStock,
                                    OpeningStockAmount = menuItemPortion.OpeningStockAmount,
                                    OpeningStockRate = menuItemPortion.OpeningStockRate,
                                    StockLimit = menuItemPortion.StockLimit
                                });
                                menuportionrepository.Save();
                                int id1 = menuportionrepository.GetAllData().OrderByDescending<MenuItemPortion, int>((Func<MenuItemPortion, int>)(x => x.Id)).FirstOrDefault<MenuItemPortion>().Id;
                                foreach (MenuItemPortionPriceRange portionPriceRange in menuItemPortion.MenuItemPortionPriceRanges)
                                {
                                    MenuItemPortionPriceRangerepository.Insert(new MenuItemPortionPriceRange()
                                    {
                                        Id = portionPriceRange.Id,
                                        PortionId = id1,
                                        Price = portionPriceRange.Price,
                                        QtyMax = portionPriceRange.QtyMax,
                                        QtyMin = portionPriceRange.QtyMin
                                    });
                                    MenuItemPortionPriceRangerepository.Save();
                                }
                                num = menuItem.Id;
                            }
                            else
                            {
                                menuportionrepository.Update(new MenuItemPortion()
                                {
                                    Id = menuItemPortion.Id,
                                    MenuItemPortionId = menuItem.Id,
                                    Name = menuItemPortion.Name,
                                    Multiplier = menuItemPortion.Multiplier,
                                    Price = menuItemPortion.Price,
                                    Discount = menuItemPortion.Discount,
                                    ItemCode = menuItemPortion.ItemCode,
                                    OpeningStock = menuItemPortion.OpeningStock,
                                    OpeningStockAmount = menuItemPortion.OpeningStockAmount,
                                    OpeningStockRate = menuItemPortion.OpeningStockRate,
                                    StockLimit = menuItemPortion.StockLimit
                                });
                                menuportionrepository.Save();
                                int id2 = menuItemPortion.Id;
                                foreach (MenuItemPortionPriceRange portionPriceRange in menuItemPortion.MenuItemPortionPriceRanges)
                                {
                                    if (portionPriceRange.Id == 0)
                                    {
                                        MenuItemPortionPriceRangerepository.Insert(new MenuItemPortionPriceRange()
                                        {
                                            Id = portionPriceRange.Id,
                                            PortionId = id2,
                                            Price = portionPriceRange.Price,
                                            QtyMax = portionPriceRange.QtyMax,
                                            QtyMin = portionPriceRange.QtyMin
                                        });
                                        MenuItemPortionPriceRangerepository.Save();
                                    }
                                    else
                                    {
                                        MenuItemPortionPriceRangerepository.Update(new MenuItemPortionPriceRange()
                                        {
                                            Id = portionPriceRange.Id,
                                            PortionId = id2,
                                            Price = portionPriceRange.Price,
                                            QtyMax = portionPriceRange.QtyMax,
                                            QtyMin = portionPriceRange.QtyMin
                                        });
                                        MenuItemPortionPriceRangerepository.Save();
                                    }
                                }
                                num = menuItem.Id;
                            }
                        }
                        unitOfWork.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.RollBackTransaction();
                        num = 0;
                    }
                }
            }
            else
                num = 0;
            return num;
        }

        public static int UpdateMenuItemPositionPrice(
          IDCubeRepository<MenuItemPortion> menuportionrepository,
          int id,
          Decimal Qty,
          Decimal QPrice)
        {
            int num = 0;
            if (id >= 1)
            {
                MenuItemPortion menuItemPortion1 = new MenuItemPortion();
                MenuItemPortion menuItemPortion2 = menuportionrepository.GetAllData().Where<MenuItemPortion>((Func<MenuItemPortion, bool>)(o => o.Id == id)).FirstOrDefault<MenuItemPortion>();
                if (menuItemPortion2 != null)
                {
                    menuItemPortion2.Multiplier = Qty;
                    menuItemPortion2.Price = QPrice;
                    menuportionrepository.Update(menuItemPortion2);
                    menuportionrepository.Save();
                    num = 1;
                }
            }
            else
                num = 0;
            return num;
        }

        public static int UpdateMenuItemPositionRangePrice(
          IDCubeRepository<MenuItemPortionPriceRange> MenuPortionPriceRangeRepository,
          int id,
          Decimal MinQty,
          Decimal MaxQty,
          Decimal QPrice)
        {
            int num = 0;
            if (id >= 1)
            {
                MenuItemPortionPriceRange portionPriceRange1 = new MenuItemPortionPriceRange();
                MenuItemPortionPriceRange portionPriceRange2 = MenuPortionPriceRangeRepository.GetAllData().Where<MenuItemPortionPriceRange>((Func<MenuItemPortionPriceRange, bool>)(o => o.Id == id)).FirstOrDefault<MenuItemPortionPriceRange>();
                if (portionPriceRange2 != null)
                {
                    portionPriceRange2.QtyMin = MinQty;
                    portionPriceRange2.QtyMax = MaxQty;
                    portionPriceRange2.Price = QPrice;
                    MenuPortionPriceRangeRepository.Update(portionPriceRange2);
                    MenuPortionPriceRangeRepository.Save();
                    num = 1;
                }
            }
            else
                num = 0;
            return num;
        }

        public static int DeleteMenuItem(
          IDCubeRepository<MenuItem> menuitemrepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          IDCubeRepository<MenuItemPortion> menuportionrepository,
          int id)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    menuitemrepository.Delete((object)id);
                    menuitemrepository.Save();
                    List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
                    foreach (MenuItemPortion menuItemPortion in menuportionrepository.GetAllData().Where<MenuItemPortion>((Func<MenuItemPortion, bool>)(o => o.MenuItemPortionId == id)).ToList<MenuItemPortion>())
                    {
                        menuportionrepository.Delete((object)menuItemPortion.Id);
                        menuportionrepository.Save();
                    }
                    num = 1;
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    num = 0;
                }
            }
            return num;
        }

        public static int DeleteMenuItemPhoto(
          IDCubeRepository<MenuItemPhoto> MenuItemPhotorepository,
          int id)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    MenuItemPhotorepository.Delete((object)id);
                    MenuItemPhotorepository.Save();
                    num = 1;
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    num = 0;
                }
            }
            return num;
        }

        public static int DeleteMenuItemPortionPriceRange(
          IDCubeRepository<MenuItemPortionPriceRange> MenuItemPortionPriceRangeRepository,
          int id)
        {
            int num = 0;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    MenuItemPortionPriceRangeRepository.Delete((object)id);
                    MenuItemPortionPriceRangeRepository.Save();
                    num = 1;
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    unitOfWork.RollBackTransaction();
                    num = 0;
                }
            }
            return num;
        }
    }
}
