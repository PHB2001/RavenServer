using Raven.Communication.Packets.Outgoing;
using Raven.Communication.Packets.Outgoing.Moderation;
using Raven.Communication.Packets.Outgoing.Rooms.Notifications;
using Raven.Communication.Packets.Outgoing.Rooms.Polls;
using Raven.HabboHotel.Rooms.Polls;

namespace Raven.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class MassPollCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_masspoll"; }
        }

        public string Parameters
        {
            get { return "%id%"; }
        }

        public string Description
        {
            get { return "Envia una encuesta a todo el hotel"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor introduzca la ID de la poll que desee enviar.");
                return;
            }

            RoomPoll poll = null;
            if (RavenEnvironment.GetGame().GetPollManager().TryGetPollForHotel(int.Parse(Params[1]), out poll))
            {
                if (poll.Type == RoomPollType.Poll)
                {
                    RavenEnvironment.GetGame().GetClientManager().SendMessage(new PollOfferComposer(poll));
                }
            }
            return;
        }
    }
}
