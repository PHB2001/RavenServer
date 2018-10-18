using System;
using Raven.HabboHotel.GameClients;
using Raven.Communication.Packets.Outgoing.Rooms.Chat;

namespace Raven.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    class MatarCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_sit"; }
        }
        public string Parameters
        {
            get { return "[nick]"; }
        }
        public string Description
        {
            get { return "Mata a alguien."; }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduzca el nick de quien desea matar.");
                return;
            }
            GameClient TargetClient = RavenEnvironment.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Esta persona no se encuentra en la habitación o no está en línea.");
                return;
            }
            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Se ha producido un error, este usuario no se ha encontrado.");
            }
            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("¿Está loco queriendo matarse? ¡Su Nutella!");
                return;
            }
            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            if (!(Math.Abs(TargetUser.X - ThisUser.X) >= 2) || (Math.Abs(TargetUser.Y - ThisUser.Y) >= 2))
            {

                ThisUser.ApplyEffect(101);
                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Pow Pow, Te maté " + Params[1] + ", se jode ahì tirado en el suelo*", 0, ThisUser.LastBubble));
                System.Threading.Thread.Sleep(1000);
                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*No esperaba eso de usted* :(", 0, ThisUser.LastBubble));
                TargetUser.Statusses.Add("lay", "0.1");
                TargetUser.isLying = true;
                TargetUser.UpdateNeeded = true;
            }
            else
            {
                Session.SendWhisper("Llega más cerca de la persona o esperas más tiempo para hacer de nuevo.");
                return;
            }
        }
    }
}