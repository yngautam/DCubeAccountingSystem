using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCubeHotelSystem.Models
{
    public class SignalRNotification : Hub
    {
        public void Subscribe(string UserId)
        {
            Groups.Add(Context.ConnectionId, UserId);
        }

        public void Unsubscribe(string UserId)
        {
            Groups.Remove(Context.ConnectionId, UserId);
        }
        public void SendToAllClients(List<OrderNotificationKitchen> notifications)
        {
            Clients.All.displayMessages("ReceiveMessage", notifications);
        }
    }
}