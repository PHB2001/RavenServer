using System;
using log4net;
using Raven.HabboHotel;

using Raven.Communication.Packets.Outgoing.Moderation;
using System.Threading;
using System.Threading.Tasks;
using Raven.Communication.Packets.Outgoing.Notifications;
using System.Globalization;

namespace Raven.Core
{
    public class ConsoleCommandHandler
    {
        private static readonly ILog log = LogManager.GetLogger("Raven.Core.ConsoleCommandHandler");

        public static void InvokeCommand(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
                return;

            try
            {
                #region Command parsing
                string[] parameters = inputData.Split(' ');

                switch (parameters[0].ToLower())
                {
                    #region stop
                    case "shutdown":
                        {
                            string time = parameters[1];
                            string time2 = parameters[2];

                            int total_time = int.Parse(time) * 60 * 1000;
                            Logging.WriteLine("The server will be close in " + time + " minutes.", ConsoleColor.Yellow);
                            RavenEnvironment.GetGame().GetClientManager().SendMessage(new HotelWillCloseInMinutesAndBackInComposer(int.Parse(time), int.Parse(time2)));
                            RavenStaticGameSettings.IsGoingToBeClose = true;
                            Task t = Task.Factory.StartNew(() => ShutdownIn(total_time));
                            break;
                        }
                    #endregion

                    #region stop
                    case "open":
                        {
                            RavenStaticGameSettings.HotelOpenForUsers = true;
                            Logging.WriteLine("Now users can enter.", ConsoleColor.Yellow);
                            break;
                        }
                    #endregion

                    #region alert
                    case "alert":
                        {
                            string Notice = inputData.Substring(6);

                            RavenEnvironment.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(RavenEnvironment.GetGame().GetLanguageLocale().TryGetValue("console.noticefromadmin") + "\n\n" + Notice));

                            log.Info(">> [SEND] Alerta enviada satisfactoriamente");
                            break;
                        }
                    #endregion

                    default:
                        {
                            log.Error(parameters[0].ToLower() + "? No se ha conseguido ese comando, escribe Help para mas Informacion.");
                            break;
                        }
                }
                #endregion
            }
            catch (Exception e)
            {
                log.Error("Error en el comando [" + inputData + "]: " + e);
            }
        }

        public static void ShutdownIn(int time)
        {
            Thread.Sleep(time);
            
            Logging.DisablePrimaryWriting(true);
            Logging.WriteLine("The server is saving users furniture, rooms, etc. WAIT FOR THE SERVER TO CLOSE, DO NOT EXIT THE PROCESS IN TASK MANAGER!!", ConsoleColor.Yellow);
            RavenEnvironment.PerformShutDown();
        }
    }
}