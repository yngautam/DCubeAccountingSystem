using DCubeHotelBusinessLayer.HotelReservationBL;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelUser;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
  public class MenuCategoryItemAPIController : BaseAPIController
  {
    private DCubeRepository<MenuCategoryItem> Menucatrepo;
    private DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuCategoryRepo;
    private DCubeRepository<MenuItem> MenuItemRepo;
    private DCubeRepository<MenuItemPortion> Menuportionrepo;
    private DCubeRepository<MenuItemPortionPriceRange> MenuportionPriceRangerepo;

    public MenuCategoryItemAPIController()
    {
      this.Menucatrepo = new DCubeRepository<MenuCategoryItem>();
      this.MenuCategoryRepo = new DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
      this.MenuItemRepo = new DCubeRepository<MenuItem>();
      this.Menuportionrepo = new DCubeRepository<MenuItemPortion>();
      this.MenuportionPriceRangerepo = new DCubeRepository<MenuItemPortionPriceRange>();
    }

    [HttpGet]
    public HttpResponseMessage Get() => this.ToJson((object) MenuBusinessLayer.GetMenuCategoryItem(this.MenuCategoryRepo, (IDCubeRepository<MenuItem>) this.MenuItemRepo, (IDCubeRepository<MenuItemPortion>) this.Menuportionrepo, (IDCubeRepository<MenuItemPortionPriceRange>) this.MenuportionPriceRangerepo));
  }
}
