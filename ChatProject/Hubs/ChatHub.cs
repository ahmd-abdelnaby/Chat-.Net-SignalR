using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ChatProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatProject.Hubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        ApplicationDbContext applicationDbContext = new ApplicationDbContext();
        static HashSet<string> CurrentConnections = new HashSet<string>();
        static HashSet<string> Connections = new HashSet<string>();
        static List<ConnectionsViewModel> ConnectionsViewModel = new List<ConnectionsViewModel>();

        public override Task OnConnected()
        {
            var id = Context.ConnectionId;
            string name = Context.User.Identity.Name;
            CurrentConnections.Add(id);
            Connections.Add(name);
            ConnectionsViewModel connectionsViewModels = new ConnectionsViewModel { ConnectionID = id, Name = name };
            ConnectionsViewModel.Add(connectionsViewModels);
            Clients.All.NewConnection(connectionsViewModels.Name);
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool f)
        {
            //var connection = CurrentConnections.FirstOrDefault(x => x == Context.ConnectionId);
            var connection = Connections.FirstOrDefault(x => x == Context.User.Identity.Name);

            if (connection != null)
            {
                CurrentConnections.Remove(connection);
                Connections.Remove(connection);
                
                ConnectionsViewModel.Remove(ConnectionsViewModel
                    .Where(c => c.ConnectionID == Context.ConnectionId).FirstOrDefault());
                Clients.All.DisConnection(Context.User.Identity.Name);

            }

            return base.OnDisconnected(false);
        }


        //return list of all active connections
        [HubMethodName("getAllActiveConnections")]

        public List<ConnectionsViewModel> getAllActiveConnections()
        {
            
            return ConnectionsViewModel;
        }
        //public List<string> getAllActiveConnections()
        //{
        //     //return CurrentConnections.ToList();
        //    return Connections.ToList();

        //}
        [HubMethodName("SendNewMessage")]
        public void SendNewMessage(string msg)
        {
            string id=Context.User.Identity.GetUserId();
            string name = Context.User.Identity.Name;
            Clients.All.writeMessage(msg, name,DateTime.Now.ToShortTimeString(),DateTime.Now.Month);
        }
        

    }
}