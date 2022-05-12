using DCubeHotelBusinessLayer.Inventory;
using DCubeHotelDomain.Models.Inventory;
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
    public class CostDetailsAPIController : BaseAPIController
    {
        private IDCubeRepository<MenuItemPortion> MenuItemPortionRepository = null;

        public CostDetailsAPIController()
        {
            this.MenuItemPortionRepository = new DCubeRepository<MenuItemPortion>();
        }

        [HttpGet]
        public HttpResponseMessage Get(int Id)
        {
            decimal result = 1;
            result = PeriodicConsumptionItemBusiness.GetCost(MenuItemPortionRepository, Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
