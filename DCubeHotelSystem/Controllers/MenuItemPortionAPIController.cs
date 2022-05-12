using DCubeHotelBusinessLayer.HotelMenuBusinessLayer;
using DCubeHotelDomain.Models;
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
    public class MenuItemPortionAPIController : BaseAPIController
    {
        private IDCubeRepository<MenuItemPortion> menuportionrepository;
        private IDCubeRepository<ExceptionLog> exceptionrepo;
        private IDCubeRepository<MenuItemPortionPriceRange> PriceRangeRepo;

        public MenuItemPortionAPIController()
        {
            this.menuportionrepository = (IDCubeRepository<MenuItemPortion>)new DCubeRepository<MenuItemPortion>();
            this.exceptionrepo = (IDCubeRepository<ExceptionLog>)new DCubeRepository<ExceptionLog>();
            this.PriceRangeRepo = (IDCubeRepository<MenuItemPortionPriceRange>)new DCubeRepository<MenuItemPortionPriceRange>();
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
            return this.ToJson((object)this.menuportionrepository.GetAllData().ToList<MenuItemPortion>());
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string ItemId)
        {
            List<MenuItemPortion> menuItemPortionList = new List<MenuItemPortion>();
            return this.ToJson((object)MenuItemBusinessLayer.GetItemPortions(this.menuportionrepository, this.PriceRangeRepo, this.exceptionrepo).Where<MenuItemPortion>((Func<MenuItemPortion, bool>)(o => o.MenuItemPortionId.ToString() == ItemId)).ToList<MenuItemPortion>());
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, Decimal Qty, Decimal QPrice)
        {
            int num = 1;
            num = MenuItemBusinessLayer.UpdateMenuItemPositionPrice(this.menuportionrepository, id, Qty, QPrice);
            return Request.CreateResponse(HttpStatusCode.OK, num);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int num = 1;
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                try
                {
                    unitOfWork.StartTransaction();
                    this.menuportionrepository.Delete((object)id);
                    this.menuportionrepository.Save();
                    num = 1;
                    unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, num);
        }
    }
}