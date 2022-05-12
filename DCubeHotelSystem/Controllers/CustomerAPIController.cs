using DCubeHotelBusinessLayer.HotelCustomerBusinessLayer;
using DCubeHotelDomain.Models.Reservation;
using DCubeHotelUser;
using System.Net;
using System.Net.Http;

namespace DCubeHotelSystem.Controllers
{
    public class CustomerAPIController : BaseAPIController
    {
        private IDCubeRepository<Customer> Customerrepository;

        public CustomerAPIController()
        {
            this.Customerrepository = new DCubeRepository<Customer>();
        }
        public HttpResponseMessage Get() => this.ToJson((object)CustomerBusinessLayer.GetCustomer(this.Customerrepository));

        public HttpResponseMessage Post(Customer value)
        {
            int result = 0;
            result = CustomerBusinessLayer.PostCustomer(this.Customerrepository, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        public HttpResponseMessage Put(int id, Customer value)
        {
            int result = 0;
            result = CustomerBusinessLayer.UpdateCustomer(this.Customerrepository, id, value);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        public HttpResponseMessage Delete(int id)
        {
            int result = 0;
            result = CustomerBusinessLayer.DeleteCustomer(this.Customerrepository, id);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }
    }
}
