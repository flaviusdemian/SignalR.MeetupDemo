using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace SignalRMVC5.Hubs
{
    [HubName("Chat")]
    public class ChatHub : Hub
    {
        [HubMethodName("Hello")]
        public void Hello()
        {
            Clients.All.hello();
        }

        public void Send(string name, string message)
        {
            if (message.Contains("<script>"))
            {
                throw new HubException("This message will flow to the client", new { user = Context.User.Identity.Name, message = message });
            }

            string userName = Clients.Caller.userName;
            string computerName = Clients.Caller.computerName;
            //string userName = Clients.CallerState.userName;
            //string computerName = Clients.CallerState.computerName;

            Clients.All.addNewMessageToPage(name, message);

            // all
            // Clients.All.

            // caller
            // Clients.Caller

            // others
            // Clients.Others

            // specific client
            // Clients.Client(Context.ConnectionId)
            
            // all except
            // Clients.AllExcept(connectionId1, connectionId2)

            // all in a group
            // Clients.Group(groupName)

            // all in a group except clients
            // Clients.Group(groupName, connectionId1, connectionId2)

            // others in group, than calling client
            // Clients.OthersInGroup(groupName)

            // specific user -> IPrincipal.Identity.Name
            // Clients.User(userid)

            // list clients
            // Clients.Clients(ConnectionIds)

            // list of groups
            // Clients.Groups(GroupIds)

            // A list of user names
            // Clients.Users(new string[] { "myUser", "myUser2" })

            var headers = Context.Request.Headers;

            var queryString = Context.Request.QueryString;
            string parameterValue = queryString["parametername"];
        }

        public async Task<string> DoLongRunningThing(IProgress<int> progress)
        {
            for (int i = 0; i <= 100; i += 5)
            {
                await Task.Delay(200);
                progress.Report(i);
            }
            return "Job complete!";
        }

        public override Task OnConnected()
        {
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // Add your own code here.
            // For example: in a chat application, mark the user as offline, 
            // delete the association between the current connection id and user name.
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            // Add your own code here.
            // For example: in a chat application, you might have marked the
            // user as offline after a period of inactivity; in that case 
            // mark the user as online again.
            return base.OnReconnected();
        }
    }
}