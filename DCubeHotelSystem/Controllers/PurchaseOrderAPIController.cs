using DCubeHotelBusinessLayer.PurchaseOrderBusiness;
using DCubeHotelDomain.Models.Inventory;
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
    public class PurchaseOrderAPIController : BaseAPIController
    {
        private IDCubeRepository<PurchaseOrder> PurchaseOrderRepository;
        private IDCubeRepository<PurchaseOrderDetails> PurchaseOrderDetailRepository;

        public PurchaseOrderAPIController()
        {
            this.PurchaseOrderRepository = (IDCubeRepository<PurchaseOrder>)new DCubeRepository<PurchaseOrder>();
            this.PurchaseOrderDetailRepository = (IDCubeRepository<PurchaseOrderDetails>)new DCubeRepository<PurchaseOrderDetails>();
        }

        [HttpGet]
        public HttpResponseMessage Get(
          DateTime dFrom,
          DateTime dTo,
          string FinancialYear)
        {
            int BranchId = 0;
            List<PurchaseOrder> purchaseOrderList = new List<PurchaseOrder>();
            return this.ToJson((object)PurchaseOrderBusinessLayer.GetPurchaseOrders(this.PurchaseOrderRepository, this.PurchaseOrderDetailRepository, dFrom, dTo, FinancialYear, BranchId));
        }

        [HttpGet]
        public HttpResponseMessage Get(
          DateTime dFrom,
          DateTime dTo,
          string FinancialYear,
          string BranchId)
        {
            List<PurchaseOrder> purchaseOrderList = new List<PurchaseOrder>();
            return this.ToJson((object)PurchaseOrderBusinessLayer.GetPurchaseOrders(this.PurchaseOrderRepository, this.PurchaseOrderDetailRepository, dFrom, dTo, FinancialYear, int.Parse(BranchId)));
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            return this.ToJson((object)PurchaseOrderBusinessLayer.GetPurchaseOrder(this.PurchaseOrderRepository, this.PurchaseOrderDetailRepository, id));
        }

        [HttpPost]
        public HttpResponseMessage Post(PurchaseOrder value)
        {
            int result = 0;
            result = PurchaseOrderBusinessLayer.PostPurchaseOrder(this.PurchaseOrderRepository, this.PurchaseOrderDetailRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, PurchaseOrder value)
        {
            int result = 0;
            result = PurchaseOrderBusinessLayer.UpdatePurchaseOrder(this.PurchaseOrderRepository, this.PurchaseOrderDetailRepository, id, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 0;
            result = PurchaseOrderBusinessLayer.DeletePurchaseOrder(this.PurchaseOrderRepository, this.PurchaseOrderDetailRepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
