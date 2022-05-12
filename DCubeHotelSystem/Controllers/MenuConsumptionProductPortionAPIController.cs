using DCubeHotelBusinessLayer.HotelMenuBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class MenuConsumptionProductPortionAPIController : BaseAPIController
    {
        private IDCubeRepository<MenuItem> MenuItemRepo = null;
        private IDCubeRepository<MenuItemPortion> menuportionrepository = null;
        private IDCubeRepository<ExceptionLog> exceptionrepo = null;

        public MenuConsumptionProductPortionAPIController()
        {
            this.MenuItemRepo = new DCubeRepository<MenuItem>();
            this.menuportionrepository = new DCubeRepository<MenuItemPortion>();
            this.exceptionrepo = new DCubeRepository<ExceptionLog>();
        }
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var MenuProductPortion = MenuItemBusinessLayer.GetListItemPortions(MenuItemRepo, menuportionrepository, exceptionrepo);
            return ToJson(MenuProductPortion);
        }
    }
}
