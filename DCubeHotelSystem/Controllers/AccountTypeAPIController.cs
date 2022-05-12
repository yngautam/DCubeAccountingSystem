using DCubeHotelBusinessLayer.Accounts;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
  [UserRoleAuthorize]
  public class AccountTypeAPIController : BaseAPIController
  {
    private IDCubeRepository<AccountType> accTypeRepository;
    private IDCubeRepository<ExceptionLog> exceptionRepository;

    public AccountTypeAPIController()
    {
      this.accTypeRepository = (IDCubeRepository<AccountType>) new DCubeRepository<AccountType>();
      this.exceptionRepository = (IDCubeRepository<ExceptionLog>) new DCubeRepository<ExceptionLog>();
    }

    [HttpGet]
    public HttpResponseMessage Get() => this.ToJson((object) this.accTypeRepository.GetAllData());

    [HttpGet]
    public HttpResponseMessage Get(int id) => this.ToJson((object) this.accTypeRepository.GetAllData().Where<AccountType>((Func<AccountType, bool>) (o => o.Id == id)).FirstOrDefault<AccountType>());

        [HttpPost]
        public HttpResponseMessage Post(AccountType value)
        {
            int result = 1;
            AccountType objAccountType = new AccountType();
            result = AccountTypeBusiness.Create(accTypeRepository, exceptionRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, AccountType value)
        {
            int result = 1;
            AccountType objAccountType = new AccountType();
            result = AccountTypeBusiness.Update(accTypeRepository, exceptionRepository, id, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 1;
            AccountType objAccountType = new AccountType();
            result = AccountTypeBusiness.Delete(accTypeRepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}