using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.WebSockets
{
    class WebSocketManager
    {
        public static void StartListener()
        {
            var server = new WebSocketServer("ws://0.0.0.0:8181");
            server.Start(socket =>
            {
                socket.OnOpen = () => RavenEnvironment.GetGame().GetClientManager().registerSession(socket);
                socket.OnClose = () => RavenEnvironment.GetGame().GetClientManager().closeSession(socket);
                socket.OnBinary = message =>
                {
                    RavenEnvironment.GetGame().GetClientManager().sessionHandleMessage(socket, message);
                };
            });
        }
    }
}
