using DCubeHotelBusinessLayer.HotelMenuBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.MenuCategory;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
  public class MenuCategoryAPIController : BaseAPIController
  {
    private IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory> MenuRepository;
    private IDCubeRepository<ExceptionLog> exceptionrepo;

    public MenuCategoryAPIController()
    {
      this.MenuRepository = (IDCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory>) new DCubeRepository<DCubeHotelDomain.Models.MenuCategory.MenuCategory>();
      this.exceptionrepo = (IDCubeRepository<ExceptionLog>) new DCubeRepository<ExceptionLog>();
    }

    [HttpGet]
    public HttpResponseMessage Get() => this.ToJson((object) MenuCategoryBusinessLayer.GetMenuCategory(this.MenuRepository, this.exceptionrepo));

    [HttpGet]
    public HttpResponseMessage Get(int id) => this.ToJson((object) MenuCategoryBusinessLayer.GetMenuCategory(this.MenuRepository, this.exceptionrepo, id));

        [HttpPost]
        public HttpResponseMessage Post(MenuCategory value)
        {
            int result = 0;
            result = MenuCategoryBusinessLayer.PostMenu(MenuRepository, exceptionrepo, value);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPut]
        public HttpResponseMessage Put(int id, MenuCategory value)
        {
            int result = 0;
            result = MenuCategoryBusinessLayer.UpdateMenuCategory(MenuRepository, exceptionrepo, id, value);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 0;
            result = MenuCategoryBusinessLayer.DeleteMenuCategory(MenuRepository, exceptionrepo, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPost]
        public HttpResponseMessage DeleteMenuCategory(int id)
        {
            int result = 0;
            result = MenuCategoryBusinessLayer.DeleteMenuCategory(MenuRepository, exceptionrepo, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}