using DCubeHotelBusinessLayer.Purchases;
using DCubeHotelDomain.Models.Inventory;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class PurchaseDetailsAPIController : BaseAPIController
    {
        private IDCubeRepository<PurchaseDetails> purchaseDetailsRepository;

        public PurchaseDetailsAPIController()
        {
            this.purchaseDetailsRepository = new DCubeRepository<PurchaseDetails>();
        }
        [HttpDelete]
        public HttpResponseMessage Delete(int Id)
        {
            int result = 0;
            result = PurchaseBusiness.DeletePurchaseDetails(this.purchaseDetailsRepository, Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
