using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelErrorLog;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DCubeHotelBusinessLayer.HotelMenuBusinessLayer
{
    public static class MenuCategoryBusinessLayer
    {
        public static List<DCubeHotelDomain.Models.MenuCategory.MenuCategory> GetMenuCategory(
          IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo)
        {
            List<DCubeHotelDomain.Models.MenuCategory.MenuCategory> menuCategory = new List<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    menuCategory = MenuRepository.GetAllData().ToList<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
                    return menuCategory;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    menuCategory = (List<DCubeHotelDomain.Models.MenuCategory.MenuCategory>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return menuCategory;
        }

        public static DCubeHotelDomain.Models.MenuCategory.MenuCategory GetMenuCategory(
          IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          int Id)
        {
            DCubeHotelDomain.Models.MenuCategory.MenuCategory menuCategory = new DCubeHotelDomain.Models.MenuCategory.MenuCategory();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    menuCategory = MenuRepository.GetAllData().Where<DCubeHotelDomain.Models.MenuCategory.MenuCategory>((Func<DCubeHotelDomain.Models.MenuCategory.MenuCategory, bool>)(o => o.Id == Id)).FirstOrDefault<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
                    return menuCategory;
                }
                catch (Exception ex)
                {
                    menuCategory = (DCubeHotelDomain.Models.MenuCategory.MenuCategory)null;
                }
                unitOfWork.CommitTransaction();
            }
            return menuCategory;
        }

        public static int PostMenu(
          IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          DCubeHotelDomain.Models.MenuCategory.MenuCategory value)
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

        public static int UpdateMenuCategory(
          IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuRepository,
          IDCubeRepository<ExceptionLog> exceptionrepo,
          int id,
          DCubeHotelDomain.Models.MenuCategory.MenuCategory value)
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

        public static int DeleteMenuCategory(
          IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuRepository,
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

        public static List<DCubeHotelDomain.Models.MenuCategory.MenuCategory> ListMenuCategory(
          DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepository,
          DCubeRepository<Menu> MenuRepo)
        {
            List<DCubeHotelDomain.Models.MenuCategory.MenuCategory> menuCategoryList = new List<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    IEnumerable<DCubeHotelDomain.Models.MenuCategory.MenuCategory> allData = MenuCategoryRepository.GetAllData();
                    int id = MenuRepo.GetAllData().FirstOrDefault<Menu>().Id;
                    foreach (DCubeHotelDomain.Models.MenuCategory.MenuCategory menuCategory in allData)
                        menuCategoryList.Add(new DCubeHotelDomain.Models.MenuCategory.MenuCategory()
                        {
                            Id = menuCategory.Id,
                            Name = menuCategory.Name
                        });
                    return menuCategoryList;
                }
                catch (Exception ex)
                {
                    ErrorLog.ErrorLogging(ex);
                    menuCategoryList = (List<DCubeHotelDomain.Models.MenuCategory.MenuCategory>)null;
                }
                unitOfWork.CommitTransaction();
            }
            return menuCategoryList;
        }
    }
}