using Microsoft.AspNetCore.Mvc;
using Microsoft.Net;
using Microsoft.Net.Http.Headers;
using System;
using System.Text;
using System.Text.Encodings;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using ProtoBuf;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Infrastructure.Services.ContactManagement;
using Core.Interfaces;
using Infrastructure.Services.ChatManagement;

namespace MessengerAPI.Controllers
{
    public class ChatController : Controller
    {
        private IContactManager _contactManager;
        private IChatManager _chatManager;


        public ChatController(IContactManager contactManager, IChatManager chatManager)
        {
            _contactManager = contactManager;
            _chatManager = chatManager;
        }

        private static Dictionary<int, WebSocket> WebSockets = new Dictionary<int, WebSocket>();
        [Route("/chat/Connect")]
        [HttpGet]
        [Authorize]
        public async Task Connect()
        {
            int Id = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                using (var client = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    WebSockets.Add(Id, client);

                    while (client.State == WebSocketState.Open)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            byte[] buffer = new byte[1024 * 4];
                            WebSocketReceiveResult receiveResult = await client.ReceiveAsync(
                                buffer, CancellationToken.None);
                            if (receiveResult.MessageType == WebSocketMessageType.Close)
                            {
                                WebSockets.Remove(Id);
                                continue;
                            }
                            var msg = Serializer.Deserialize<Message>(new MemoryStream(buffer, 0, receiveResult.Count));
                            var test = DateTime.Now.AddTicks(0).ToString("yyyy-MM-dd H:mm:ss.FFFFFFF");
                            var newMsg = await _chatManager.InsertMessage(new Core.Entities.Message()
                            {
                                ChatId = int.Parse(msg.ChatId),
                                SenderId = int.Parse(msg.SenderId),
                                SentDate = DateTime.Now,
                                Text = msg.Text,
                            });


                            foreach (var contact in newMsg.Chat.Contacts)
                            {
                                Serializer.Serialize<Message>(memoryStream, new Message()
                                {

                                    ChatId = newMsg.ChatId.ToString(),
                                    Id = newMsg.Id.ToString(),
                                    SenderId = newMsg.SenderId.ToString(),
                                    SentDate = newMsg.SentDate.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFF"),
                                    Text = msg.Text
                                });
                                memoryStream.Capacity = int.Parse(memoryStream.Length.ToString());
                                var contactSocket = WebSockets.GetValueOrDefault(contact.UserId);
                                if (contactSocket != null)
                                    await contactSocket?.SendAsync(memoryStream.ToArray(), WebSocketMessageType.Binary, true, CancellationToken.None);
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                WebSockets.Remove(Id);
            }
        }
    }
    [ProtoContract]
    class Message
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public string Text { get; set; }
        [ProtoMember(3)]
        public string SentDate { get; set; }
        [ProtoMember(4)]
        public string ChatId { get; set; }
        [ProtoMember(5)]
        public string SenderId { get; set; }
    }

}
