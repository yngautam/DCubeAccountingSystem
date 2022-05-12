using DCubeHotelBusinessLayer.TicketBusinessLayer;
using DCubeHotelDomain.Models.Tickets;
using DCubeHotelSystem.Models;
using DCubeHotelUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DCubeHotelSystem.Controllers
{
    public class OrderTableMoveController : BaseAPIController
    {
        private IDCubeRepository<Ticket> TicketRepository = null;
        public OrderTableMoveController()
        {
            this.TicketRepository = new DCubeRepository<Ticket>();
        }
        //[HttpPost]
        //public HttpResponseMessage Post(OrderTableMove objOrderTableMove)
        //{
        //    int result = 0;
        //    string baseTable = objOrderTableMove.baseTable;
        //    string baseTicketNumber = objOrderTableMove.baseTicketNumber;
        //    string distinationTable = objOrderTableMove.distinationTable;
        //    result = TicketBusiness.OrderTableMove(baseTable, baseTicketNumber, distinationTable, TicketRepository);
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}
    }
}
