using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    [UserRoleAuthorize]
    public class TicketNoteAPIController : BaseAPIController
    {
        private IDCubeRepository<Ticket> TicketRepository;

        public TicketNoteAPIController()
        {
            this.TicketRepository = new DCubeRepository<Ticket>();
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri] string TicketId, [FromUri] string Note) => this.ToJson((object)TicketBusiness.TicketNote(this.TicketRepository, TicketId, Note));
    }
}