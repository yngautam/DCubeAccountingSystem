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
    public class PeriodicConsumptionItemAPIController : BaseAPIController
    {
        private IDCubeRepository<PeriodicConsumptionItem> PeriodicConsumptionItemRepository = null;
        //private IDCubeRepository<PeriodicConsumption> PeriodicConsumptionRepository = null;
        private IDCubeRepository<InventoryReceiptDetails> InventReceiptDetailsRepository = null;
        private IDCubeRepository<MenuItemPortion> MenuItemPortionRepository = null;


        public PeriodicConsumptionItemAPIController()
        {
            this.PeriodicConsumptionItemRepository = new DCubeRepository<PeriodicConsumptionItem>();
            //this.PeriodicConsumptionRepository = new DCubeRepository<PeriodicConsumption>();
            this.InventReceiptDetailsRepository = new DCubeRepository<InventoryReceiptDetails>();
            this.MenuItemPortionRepository = new DCubeRepository<MenuItemPortion>();
        }

        [HttpGet]
        public HttpResponseMessage Get(int Id)
        {
            decimal result = 1;
            result = PeriodicConsumptionItemBusiness.GetReceiptSumQuantity(MenuItemPortionRepository, Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public HttpResponseMessage Post(PeriodicConsumptionItem value)
        {
            int result = 1;
            PeriodicConsumptionItem objPerodicConsumptionItem = new PeriodicConsumptionItem();
            result = PeriodicConsumptionItemBusiness.Create(PeriodicConsumptionItemRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int PeriodicConsumptionId)
        {
            int result = 1;
            result = PeriodicConsumptionBusines.DeletePeriodicConsumption(PeriodicConsumptionItemRepository, PeriodicConsumptionId);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
