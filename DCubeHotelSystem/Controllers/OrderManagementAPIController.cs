using DCubeHotelBusinessLayer.OrderManagementBusiness;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class OrderManagementAPIController : BaseAPIController
    {
        private IDCubeRepository<OrderManagement> OrderManagementRepository;
        private IDCubeRepository<OrderManagementDetail> OrderManagementDetailRepository;

        public OrderManagementAPIController()
        {
            this.OrderManagementRepository = (IDCubeRepository<OrderManagement>)new DCubeRepository<OrderManagement>();
            this.OrderManagementDetailRepository = (IDCubeRepository<OrderManagementDetail>)new DCubeRepository<OrderManagementDetail>();
        }

        [HttpGet]
        public HttpResponseMessage Get(
          DateTime dFrom,
          DateTime dTo,
          string FinancialYear)
        {
            int BranchId = 0;
            List<OrderManagement> orderManagementList = new List<OrderManagement>();
            return this.ToJson((object)OrderManagementBusinesslayer.GetOrderManagements(this.OrderManagementRepository, this.OrderManagementDetailRepository, dFrom, dTo, FinancialYear, BranchId));
        }

        [HttpGet]
        public HttpResponseMessage Get(
          DateTime dFrom,
          DateTime dTo,
          string FinancialYear,
          string BranchId)
        {
            List<OrderManagement> orderManagementList = new List<OrderManagement>();
            return this.ToJson((object)OrderManagementBusinesslayer.GetOrderManagements(this.OrderManagementRepository, this.OrderManagementDetailRepository, dFrom, dTo, FinancialYear, int.Parse(BranchId)));
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            OrderManagement orderManagement = new OrderManagement();
            return this.ToJson((object)OrderManagementBusinesslayer.GetOrderManagement(this.OrderManagementRepository, this.OrderManagementDetailRepository, id));
        }

        [HttpPost]
        public HttpResponseMessage Post(OrderManagement value)
        {
            int result = 0;
            result = OrderManagementBusinesslayer.PostOrderManagement(this.OrderManagementRepository, this.OrderManagementDetailRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, OrderManagement value)
        {
            int result = 0;
            result = OrderManagementBusinesslayer.UpdateOrderManagement(this.OrderManagementRepository, this.OrderManagementDetailRepository, id, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 0;
            result = OrderManagementBusinesslayer.DeleteOrderManagement(this.OrderManagementRepository, this.OrderManagementDetailRepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}