using DCubeHotelBusinessLayer.HotelMenuBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.MenuCategory;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class MenuConsumptionCategoryFilterAPIController : BaseAPIController
    {
        private IDCubeRepository<MenuItem> menuitemrepository = null;
        private IDCubeRepository<MenuItemPortion> menuportionrepository = null;
        private IDCubeRepository<MenuCategory> menucategoryrepository = null;
        private IDCubeRepository<ExceptionLog> exceptionrepo = null;
        private IDCubeRepository<MenuItemPortionPriceRange> PriceRangeRepo = null;
        private IDCubeRepository<MenuItemPhoto> MenuItemPhotoRepo = null;
        public MenuConsumptionCategoryFilterAPIController()
        {
            this.menuitemrepository = new DCubeRepository<MenuItem>();
            this.menuportionrepository = new DCubeRepository<MenuItemPortion>();
            this.menucategoryrepository = new DCubeRepository<MenuCategory>();
            this.exceptionrepo = new DCubeRepository<ExceptionLog>();
            this.PriceRangeRepo = new DCubeRepository<MenuItemPortionPriceRange>();
            this.MenuItemPhotoRepo = new DCubeRepository<MenuItemPhoto>();
        }
        [HttpGet]
        public HttpResponseMessage Get([FromUri] string CategoryId)
        {
            List<MenuItem> listMenuItem = new List<MenuItem>();
            listMenuItem = MenuItemBusinessLayer.GetMenuItems(menuitemrepository, menucategoryrepository, menuportionrepository, PriceRangeRepo, MenuItemPhotoRepo, exceptionrepo, int.Parse(CategoryId));
            listMenuItem = listMenuItem.Where(o => o.categoryId.ToString() == CategoryId).ToList();
            return ToJson(listMenuItem);
        }
    }
}
