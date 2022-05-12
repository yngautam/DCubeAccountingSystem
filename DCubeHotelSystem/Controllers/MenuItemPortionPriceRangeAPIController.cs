using DCubeHotelBusinessLayer.HotelMenuBusinessLayer;
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
    public class MenuItemPortionPriceRangeAPIController : BaseAPIController
    {
        private IDCubeRepository<MenuItemPortionPriceRange> PriceRangeRepo;

        public MenuItemPortionPriceRangeAPIController()
        {
            this.PriceRangeRepo = new DCubeRepository<MenuItemPortionPriceRange>();
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            List<MenuItemPortionPriceRange> portionPriceRangeList = new List<MenuItemPortionPriceRange>();
            return this.ToJson((object)this.PriceRangeRepo.GetAllData().ToList<MenuItemPortionPriceRange>());
        }

        [HttpPut]
        public HttpResponseMessage Put(
          int id,
          Decimal MinQty,
          Decimal MaxQty,
          Decimal QPrice)
        {
            int result = 0;
            result = MenuItemBusinessLayer.UpdateMenuItemPositionRangePrice(this.PriceRangeRepo, id, MinQty, MaxQty, QPrice);
            return Request.CreateResponse(HttpStatusCode.OK, result);
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
                    this.PriceRangeRepo.Delete((object)id);
                    this.PriceRangeRepo.Save();
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
