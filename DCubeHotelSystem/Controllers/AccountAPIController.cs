using DCubeHotelBusinessLayer.DCubeHotelAccount;
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
  public class AccountAPIController : BaseAPIController
  {
    private IDCubeRepository<Account> accountRepository;
    private IDCubeRepository<ExceptionLog> exceptionRepository;

    public AccountAPIController()
    {
      this.accountRepository = (IDCubeRepository<Account>) new DCubeRepository<Account>();
      this.exceptionRepository = (IDCubeRepository<ExceptionLog>) new DCubeRepository<ExceptionLog>();
    }

    [HttpGet]
    public HttpResponseMessage Get() => this.ToJson((object) this.accountRepository.GetAllData());

    [HttpGet]
    public HttpResponseMessage GetAll() => this.ToJson((object) this.accountRepository.GetAllData());

    [HttpGet]
    public HttpResponseMessage GetCurrentAccount(int? id) => id.HasValue ? this.ToJson((object) this.accountRepository.GetAllData().Where<Account>((Func<Account, bool>) (o => o.Id == id.Value)).FirstOrDefault<Account>()) : this.ToJson((object) this.accountRepository.GetAllData());

    [HttpGet]
    public HttpResponseMessage Get(
      [FromUri] string AccountTypeId,
      [FromUri] string AccountGeneral,
      [FromUri] string CustomerId,
      [FromUri] string CustomerType,
      [FromUri] string PartyAccount,
      [FromUri] string PartyType,
      [FromUri] string PartyTypeBankCash)
    {
      return this.ToJson((object) AccountsBusiness.GetAllAccountBankCash(this.accountRepository));
    }

    [HttpGet]
    public HttpResponseMessage Get(
      [FromUri] string AccountTypeId,
      [FromUri] string AccountGeneral,
      [FromUri] string CustomerId,
      [FromUri] string CustomerType,
      [FromUri] string PartyAccount,
      [FromUri] string PartyType)
    {
      return this.ToJson((object) AccountsBusiness.GetAllPartyAccount(this.accountRepository));
    }

    [HttpGet]
    public HttpResponseMessage Get(
      [FromUri] string AccountTypeId,
      [FromUri] string AccountGeneral,
      [FromUri] string CustomerId,
      [FromUri] string CustomerType,
      [FromUri] string PartyAccount,
      [FromUri] string PartyType,
      [FromUri] string PartyTypeName,
      [FromUri] string PartyLst)
    {
      return this.ToJson((object) AccountsBusiness.GetAllParty(this.accountRepository));
    }

    [HttpGet]
    public HttpResponseMessage GetVendorParty() => this.ToJson((object) AccountsBusiness.GetAllPartyAccount(this.accountRepository));

    [HttpGet]
    public HttpResponseMessage Get(
      [FromUri] string AccountTypeId,
      [FromUri] string AccountGeneral,
      [FromUri] string CustomerId,
      [FromUri] string CustomerType,
      [FromUri] string PartyAccount)
    {
      return this.ToJson((object) AccountsBusiness.GetAccount(this.accountRepository));
    }

    [HttpGet]
    public HttpResponseMessage GetParyMiscAccount() => this.ToJson((object) AccountsBusiness.GetAccount(this.accountRepository));

    [HttpGet]
    public HttpResponseMessage Get(
      [FromUri] string AccountTypeId,
      [FromUri] string AccountGeneral,
      [FromUri] string CustomerId,
      [FromUri] string CustomerType)
    {
      return this.ToJson((object) AccountsBusiness.GetAccount(this.accountRepository));
    }

    [HttpGet]
    public HttpResponseMessage GetMisAccount() => this.ToJson((object) AccountsBusiness.GetAccount(this.accountRepository));

    [HttpGet]
    public HttpResponseMessage Get(
      [FromUri] string AccountTypeId,
      [FromUri] string AccountGeneral,
      [FromUri] string CustomerId)
    {
      return this.ToJson((object) AccountsBusiness.GetAllCustomerAccount(this.accountRepository));
    }

    [HttpGet]
    public HttpResponseMessage GetPartyAccount() => this.ToJson((object) AccountsBusiness.GetAllCustomerAccount(this.accountRepository));

    [HttpGet]
    public HttpResponseMessage Get([FromUri] string AccountTypeId, [FromUri] string AccountGeneral) => this.ToJson((object) AccountsBusiness.GetAllAccountGeneral(this.accountRepository));

    [HttpGet]
    public HttpResponseMessage GetMiscAccount() => this.ToJson((object) AccountsBusiness.GetAllAccountGeneral(this.accountRepository));

    [HttpGet]
    public HttpResponseMessage GetBankCash([FromUri] string AccountTypeId) => this.ToJson((object) AccountsBusiness.GetAllAccountBankCash(this.accountRepository));

        [HttpPost]
        public HttpResponseMessage Post(Account value)
        {
            int result = 1;
            Account objaccount = new Account();
            result = AccountsBusiness.Create(accountRepository, exceptionRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, Account value)
        {
            int result = 1;
            Account objaccount = new Account();
            result = AccountsBusiness.Update(accountRepository, exceptionRepository, value, id);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            int result = 1;
            Account objaccount = new Account();
            result = AccountsBusiness.Delete(accountRepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
