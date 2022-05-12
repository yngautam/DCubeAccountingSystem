using DCubeHotelBusinessLayer.HotelMenuBusinessLayer;
using DCubeHotelDomain.Models;
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
    public class MenuItemAPIController : BaseAPIController
    {
        private IDCubeRepository<MenuItem> menuitemrepository;
        private IDCubeRepository<MenuItemPortion> menuportionrepository;
        private IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> menucategoryrepository;
        private IDCubeRepository<ExceptionLog> exceptionrepo;
        private IDCubeRepository<MenuItemPortionPriceRange> PriceRangeRepo;
        private IDCubeRepository<MenuItemPhoto> MenuItemPhotoRepo;

        public MenuItemAPIController()
        {
            this.menuitemrepository = (IDCubeRepository<MenuItem>)new DCubeRepository<MenuItem>();
            this.menuportionrepository = (IDCubeRepository<MenuItemPortion>)new DCubeRepository<MenuItemPortion>();
            this.menucategoryrepository = (IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory>)new DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
            this.exceptionrepo = (IDCubeRepository<ExceptionLog>)new DCubeRepository<ExceptionLog>();
            this.PriceRangeRepo = (IDCubeRepository<MenuItemPortionPriceRange>)new DCubeRepository<MenuItemPortionPriceRange>();
            this.MenuItemPhotoRepo = (IDCubeRepository<MenuItemPhoto>)new DCubeRepository<MenuItemPhoto>();
        }

        [HttpGet]
        public HttpResponseMessage Get() => this.ToJson((object)MenuItemBusinessLayer.GetMenuItemList(this.menuitemrepository, this.MenuItemPhotoRepo, this.menucategoryrepository, this.menuportionrepository, this.PriceRangeRepo, this.exceptionrepo));

        [HttpGet]
        public HttpResponseMessage Get(int id) => this.ToJson((object)MenuItemBusinessLayer.GetMenuItems(this.menuitemrepository, this.menucategoryrepository, this.menuportionrepository, this.PriceRangeRepo, this.MenuItemPhotoRepo, this.exceptionrepo, id));

        private List<MenuItem> LoadMenuItem(List<MenuItem> MenuItems)
        {
            List<MenuItemPortion> menuItemPortionList1 = new List<MenuItemPortion>();
            List<MenuItemPortion> list1 = this.menuportionrepository.GetAllData().ToList<MenuItemPortion>();
            List<MenuItem> menuItemList = new List<MenuItem>();
            foreach (MenuItem menuItem1 in MenuItems)
            {
                MenuItem ObjMenuItem = menuItem1;
                MenuItem menuItem2 = new MenuItem();
                menuItem2.Id = ObjMenuItem.Id;
                menuItem2.Name = ObjMenuItem.Name;
                MenuItem menuItem3 = menuItem2;
                menuItem3.categoryId = menuItem3.categoryId;
                menuItem2.Barcode = ObjMenuItem.Barcode;
                menuItem2.Tag = ObjMenuItem.Tag;
                menuItem2.MarginRate = ObjMenuItem.MarginRate;
                List<MenuItemPortion> menuItemPortionList2 = new List<MenuItemPortion>();
                List<MenuItemPortion> list2 = list1.Where<MenuItemPortion>((Func<MenuItemPortion, bool>)(x => x.MenuItemPortionId == ObjMenuItem.Id)).ToList<MenuItemPortion>();
                menuItem2.MenuItemPortions = (IEnumerable<MenuItemPortion>)list2;
                menuItemList.Add(menuItem2);
            }
            return menuItemList;
        }
        [HttpPost]
        public HttpResponseMessage Post(MenuItem value)
        {
            int result = 0;
            result = MenuItemBusinessLayer.PostMenuItem(menuitemrepository, menuportionrepository, exceptionrepo, value, PriceRangeRepo);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPut]
        public HttpResponseMessage Put(int id, MenuItem value)
        {
            int result = 0;
            result = MenuItemBusinessLayer.UpdateMenuItem(menuitemrepository, menuportionrepository, exceptionrepo, id, value, PriceRangeRepo);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 0;
            result = MenuItemBusinessLayer.DeleteMenuItem(menuitemrepository, exceptionrepo, menuportionrepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
