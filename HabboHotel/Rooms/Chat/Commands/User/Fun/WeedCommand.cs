﻿using Raven.Communication.Interfaces;
using Raven.Communication.Packets.Outgoing.Inventory.Purse;
using Raven.Communication.Packets.Outgoing.Rooms.Chat;
using System;
using System.Threading;

namespace Raven.HabboHotel.Rooms.Chat.Commands.User
{
    internal class WeedCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "command_sit";
            }
        }

        public string Parameters
        {
            get
            {
                return "%si%";
            }
        }

        public string Description
        {
            get
            {
                return "Fumando un bate, los costos de marihuana (5c).";
            }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendNotification("¿Te gustaría comprar marihuana?\n\n" +
                 "Para confirmar, escriba \":fumar si\". \n\n Después de haber hecho usted no puede volver!\n\n(Si no quieres comprar marihuana, ignora este mensaje! ;) )\n\n");
                return;
            }
            RoomUser roomUserByHabbo = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (roomUserByHabbo == null)
                return;
            if (Params.Length == 2 && Params[1].ToString() == "si")
            {
                roomUserByHabbo.GetClient().SendWhisper("¡Obtuvo Marihuana!");
                Thread.Sleep(1000);
                Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "* Enrolla el bate *", 0, 6), false);
                Thread.Sleep(2000);
                Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "*Encender y empezar a fumar*", 0, 6), false);
                Thread.Sleep(2000);
                roomUserByHabbo.ApplyEffect(53);
                Thread.Sleep(2000);
                switch (new Random().Next(1, 4))
                {
                    case 1:
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "Hehehe Veo muchas aves :D  ", 0, 6), false);
                        break;
                    case 2:
                        roomUserByHabbo.ApplyEffect(70);
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "Me siento un panda :D ", 0, 6), false);
                        break;
                    default:
                        Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "Hehehe voy volando por los aires :D ", 0, 6), false);
                        break;
                }
                Thread.Sleep(2000);
                Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "Veo a M00nlight y a Pussy de colores", 0, 6), false);
                Thread.Sleep(2000);
                roomUserByHabbo.ApplyEffect(0);
                Thread.Sleep(2000);
                Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "*que Marihuana buena que obtuve*", 0, 6), false);
                Thread.Sleep(2000);
            }

        }
    }
}