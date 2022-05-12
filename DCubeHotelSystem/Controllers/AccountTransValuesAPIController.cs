using DCubeHotelBusinessLayer.Accounts;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
  [UserRoleAuthorize]
  public class AccountTransValuesAPIController : BaseAPIController
  {
    private IDCubeRepository<AccountTransactionValue> accountValuesRepository;
        public AccountTransValuesAPIController()
        {
            this.accountValuesRepository = new DCubeRepository<AccountTransactionValue>();
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int Id)
        {
            int result = 1;
            result = AccountTransactionBusiness.DeleteAccountTransValues(accountValuesRepository, Id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
