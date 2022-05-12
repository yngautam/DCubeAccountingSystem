using DCubeHotelBusinessLayer.Accounts;
using DCubeHotelDomain.Models.Accounts;
using DCubeHotelDomain.Models.CBMSAPI;
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
    public class BillReturnViewController : BaseAPIController
    {
        // GET: BillReturnView
        private IDCubeRepository<BillReturnViewModel> BillReturnViewModelRepository = null;
        private IDCubeRepository<AccountTransactionValue> accValueRepository = null;
        private IDCubeRepository<Account> AccountRepository = null;
        private IDCubeRepository<AccountType> AccountTypeRepository = null;
        private IDCubeRepository<AccountTransaction> AccountTransactionRepository = null;
        private IDCubeRepository<AccountTransactionType> AccountTransactionTypeRepository = null;
        public BillReturnViewController()
        {
            this.BillReturnViewModelRepository = new DCubeRepository<BillReturnViewModel>();
            this.accValueRepository = new DCubeRepository<AccountTransactionValue>();
            this.AccountRepository = new DCubeRepository<Account>();
            this.AccountTypeRepository = new DCubeRepository<AccountType>();
            this.AccountTransactionRepository = new DCubeRepository<AccountTransaction>();
            this.AccountTransactionTypeRepository = new DCubeRepository<AccountTransactionType>();
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string FinancialYear)
        {
            List<BillReturnViewModel> ListBillReturnViewModel = new List<BillReturnViewModel>();
            try
            {
                ListBillReturnViewModel = BillReturnViewModelBusiness.getBillReturnViewModel(AccountTypeRepository, AccountRepository, accValueRepository, AccountTransactionRepository, AccountTransactionTypeRepository, FinancialYear);
            }
            catch (Exception ex)
            {

            }
            return ToJson(ListBillReturnViewModel.AsEnumerable());
        }
    }
}