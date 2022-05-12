using DCubeHotelBusinessLayer.PurchaseOrderBusiness;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class PurchaseOrderDetailAPIController : BaseAPIController
    {
        private IDCubeRepository<PurchaseOrderDetails> PurchaseOrderDetailRepository;
        public PurchaseOrderDetailAPIController()
        {
            this.PurchaseOrderDetailRepository = (IDCubeRepository<PurchaseOrderDetails>)new DCubeRepository<PurchaseOrderDetails>();
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 0;
            result = PurchaseOrderBusinessLayer.DeletePurchaseOrderDetailDetail(this.PurchaseOrderDetailRepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}