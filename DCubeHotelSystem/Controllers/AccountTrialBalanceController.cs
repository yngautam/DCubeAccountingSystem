using DCubeHotelBusinessLayer.Accounts;
using DCubeHotelDomain.Models;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class AccountTrialBalanceController : BaseAPIController
    {
        private IDCubeRepository<AccountTransactionValue> accValueRepository;
        private IDCubeRepository<Account> AccountRepository;
        private IDCubeRepository<AccountType> AccountTypeRepository;
        private DCubeRepository<FinancialYear> FinancialYearrepo;

        public AccountTrialBalanceController()
        {
            this.accValueRepository = (IDCubeRepository<AccountTransactionValue>)new DCubeRepository<AccountTransactionValue>();
            this.AccountRepository = (IDCubeRepository<Account>)new DCubeRepository<Account>();
            this.AccountTypeRepository = (IDCubeRepository<AccountType>)new DCubeRepository<AccountType>();
            this.FinancialYearrepo = new DCubeRepository<FinancialYear>();
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string FinancialYear)
        {
            List<TrialBalance> source = new List<TrialBalance>();
            try
            {
                source = TrialBalanceBusiness.GetTrialBalance(this.AccountTypeRepository, this.AccountRepository, this.accValueRepository, this.FinancialYearrepo, FinancialYear);
            }
            catch (Exception ex)
            {
            }
            return this.ToJson((object)source.AsEnumerable<TrialBalance>());
        }
    }
}
