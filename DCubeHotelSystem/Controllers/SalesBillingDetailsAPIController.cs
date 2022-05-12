using DCubeHotelBusinessLayer.Sales;
using DCubeHotelDomain.Models.Menus;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class SalesBillingDetailsAPIController : BaseAPIController
    {
        private IDCubeRepository<MenuItemPortion> MenuItemPortionRepository;
        private IDCubeRepository<Order> OrderRepository;

        public SalesBillingDetailsAPIController()
        {
            this.MenuItemPortionRepository = (IDCubeRepository<MenuItemPortion>)new DCubeRepository<MenuItemPortion>();
            this.OrderRepository = (IDCubeRepository<Order>)new DCubeRepository<Order>();
        }

        [HttpGet]
        public HttpResponseMessage Get(int Id)
        {
            decimal Price = 0;
            Price = SaleBillingBusiness.GetSaleBillingRate(this.MenuItemPortionRepository, Id);
            return Request.CreateResponse(HttpStatusCode.OK, Price);

        }

        [HttpDelete]
        public HttpResponseMessage Delete(int Id)
        {
            int result = 0;
            result = SaleBillingBusiness.DeleteOrderDetails(this.OrderRepository, Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}