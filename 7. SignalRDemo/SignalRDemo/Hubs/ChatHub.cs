using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using System.Timers;

namespace SignalRDemo.Hubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            if (message.Contains("<script>"))
            {
                throw new HubException("This message will flow to the client", new { user = Context.User.Identity.Name, message = message });
            }

            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);

            //Clients.All.addContosoChatMessageToPage(name, message);
            //Clients.Caller.addContosoChatMessageToPage(name, message); <=> Clients.Client(Context.ConnectionId).addContosoChatMessageToPage(name, message);
            //Clients.Others.addContosoChatMessageToPage(name, message);
            //Clients.AllExcept(connectionId1, connectionId2).addContosoChatMessageToPage(name, message);
            //Clients.Group(groupName).addContosoChatMessageToPage(name, message);
            //Clients.Group(groupName, connectionId1, connectionId2).addContosoChatMessageToPage(name, message);
            //Clients.OthersInGroup(groupName).addContosoChatMessageToPage(name, message);
            //Clients.Clients(ConnectionIds).broadcastMessage(name, message);
            //Clients.Groups(GroupIds).broadcastMessage(name, message);
            
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.Remove(Context.ConnectionId, groupName);
        }


        public override Task OnConnected()
        {
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.

            Clients.All.addNewMessageToPage("### System message ###", "New connection established. Connection id: " + Context.ConnectionId);
            if (DateAndTimeProvider.Instance.TimerStarted == false) DateAndTimeProvider.Instance.StartTimer();

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            // Add your own code here.
            // For example: in a chat application, mark the user as offline, 
            // delete the association between the current connection id and user name.
            Clients.All.addNewMessageToPage("### System message ###", "Connection closed. Connection id: " + Context.ConnectionId);

            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            // Add your own code here.
            // For example: in a chat application, you might have marked the
            // user as offline after a period of inactivity; in that case 
            // mark the user as online again.

            Clients.All.addNewMessageToPage("### System message ###", "Connection reestablished. Connection id: " + Context.ConnectionId);

            return base.OnReconnected();
        }
    }


    public class DateAndTimeProvider
    {
        private static DateAndTimeProvider _instance = null;
        public static DateAndTimeProvider Instance 
        {
            get
            {
                if (_instance == null) _instance = new DateAndTimeProvider(GlobalHost.ConnectionManager.GetHubContext<ChatHub>());
                return _instance;
            }
        }

        public bool TimerStarted { get; set; }
        private IHubContext _context;
        private Timer _timer = null;

        private DateAndTimeProvider(IHubContext context)
        {
            _context = context;
            TimerStarted = false;
        }

        public void StartTimer()
        {
            if (TimerStarted == false && _timer == null)
            {
                _timer = new Timer(5000);
                _timer.Elapsed += _timer_Elapsed;
                _timer.Start();
                TimerStarted = true;
            }
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _context.Clients.All.addNewMessageToPage("### System time update ###", "Server time is: " + DateTime.Now.ToString());
        }
    }
}