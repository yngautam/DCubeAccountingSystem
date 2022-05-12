using DCubeHotelBusinessLayer.HotelMenuBusinessLayer;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    public class MenuItemPhotoAPIController : BaseAPIController
    {
        private IDCubeRepository<MenuItemPhoto> MenuItemPhotoRepository;

        public MenuItemPhotoAPIController()
        {
            this.MenuItemPhotoRepository = new DCubeRepository<MenuItemPhoto>();
        }

        [HttpPost]
        public HttpResponseMessage Post(MenuItemPhoto value)
        {
            int result = 0;
            result = MenuItemBusinessLayer.PostMenuItemPhoto(this.MenuItemPhotoRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, MenuItemPhoto value)
        {
            int result = 0;
            result = MenuItemBusinessLayer.UpdateMenuItemPhoto(this.MenuItemPhotoRepository, value, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 0;
            result = MenuItemBusinessLayer.DeleteMenuItemPhoto(this.MenuItemPhotoRepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}