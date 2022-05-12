using DCubeHotelBusinessLayer;
using DCubeHotelDomain.Models;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class BranchAPIController : BaseAPIController
    {
        private IDCubeRepository<Branch> BranchRepository;

        public BranchAPIController()
        {
            this.BranchRepository = new DCubeRepository<Branch>();
        }
        [HttpGet]
        public HttpResponseMessage Get() => this.ToJson((object)BranchBusinessLayer.GetBranch(this.BranchRepository));

        [HttpGet]
        public HttpResponseMessage Get(int Id) => this.ToJson((object)BranchBusinessLayer.GetBranch(this.BranchRepository, Id));

        [HttpPost]
        public HttpResponseMessage Post(Branch value)
        {
            int result = 0;
            result = BranchBusinessLayer.PostBranch(BranchRepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        public HttpResponseMessage Put(int id, Branch value)
        {
            int result = 1;
            result = BranchBusinessLayer.UpdateBranch(BranchRepository, value, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public HttpResponseMessage DeleteBranch(int id)
        {
            int result = 1;
            result = BranchBusinessLayer.DeleteBranch(BranchRepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}