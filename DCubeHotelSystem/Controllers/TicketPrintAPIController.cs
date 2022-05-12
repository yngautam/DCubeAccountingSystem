using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class TicketPrintAPIController : BaseAPIController
    {
        private IDCubeRepository<Ticket> TicketRepository;

        public TicketPrintAPIController()
        {
            this.TicketRepository = new DCubeRepository<Ticket>();
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string TicketId) => this.ToJson((object)TicketBusiness.PrintTicket(this.TicketRepository, TicketId));
    }
}