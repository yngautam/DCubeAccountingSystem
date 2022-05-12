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
    public class AccountTransactionTypeAPIController : BaseAPIController
    {
        private IDCubeRepository<AccountTransactionType> accounttransTyperepo;
        private IDCubeRepository<ExceptionLog> exceptionRepository;

        public AccountTransactionTypeAPIController()
        {
            this.accounttransTyperepo = (IDCubeRepository<AccountTransactionType>)new DCubeRepository<AccountTransactionType>();
            this.exceptionRepository = (IDCubeRepository<ExceptionLog>)new DCubeRepository<ExceptionLog>();
        }

        [HttpGet]
        public HttpResponseMessage Get() => this.ToJson((object)this.accounttransTyperepo.GetAllData());

        [HttpGet]
        public HttpResponseMessage Get(int id) => this.ToJson((object)this.accounttransTyperepo.GetAllData().Where<AccountTransactionType>((Func<AccountTransactionType, bool>)(o => o.Id == id)).FirstOrDefault<AccountTransactionType>());

        [HttpPost]
        public HttpResponseMessage Post(AccountTransactionType value)
        {
            int result = 1;
            AccountTransactionType objaccoutTransType = new AccountTransactionType();
            result = AccountTransactionTypeBusiness.Create(accounttransTyperepo, exceptionRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpPut]
        public HttpResponseMessage Put(int id, AccountTransactionType value)
        {
            int result = 1;
            AccountTransactionType objaccoutTransType = new AccountTransactionType();
            result = AccountTransactionTypeBusiness.Update(accounttransTyperepo, exceptionRepository, id, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 1;
            AccountTransactionType objaccoutTransType = new AccountTransactionType();
            result = AccountTransactionTypeBusiness.Delete(accounttransTyperepo, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
