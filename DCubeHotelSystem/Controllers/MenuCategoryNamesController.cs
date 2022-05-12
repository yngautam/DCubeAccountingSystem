using DCubeHotelBusinessLayer.HotelMenuBusinessLayer;
using DCubeHotelDomain.Models.MenuCategory;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    public class MenuCategoryNamesController : BaseAPIController
    {
        private DCubeRepository<MenuCategory> MenuCategoryRepository = null;
        private DCubeRepository<Menu> MenuRepo = null;

        public MenuCategoryNamesController()
        {
            this.MenuCategoryRepository = new DCubeRepository<MenuCategory>();
            this.MenuRepo = new DCubeRepository<Menu>();
        }
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var listmenucategory = MenuCategoryBusinessLayer.ListMenuCategory(MenuCategoryRepository, MenuRepo);
            return ToJson(listmenucategory);
        }
    }
}
