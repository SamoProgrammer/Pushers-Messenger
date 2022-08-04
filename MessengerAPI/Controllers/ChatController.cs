using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Core.Interfaces;

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

        private static Dictionary<(int, int), WebSocket> _chatWS = new Dictionary<(int, int), WebSocket>();
        private static Dictionary<int, WebSocket> _chatsListWS = new Dictionary<int, WebSocket>();
        [Route("/chat/EnterRoom")]
        [HttpGet]
        [Authorize]
        public async Task EnterRoom()
        {
            int id = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int chatId = int.Parse(HttpContext.Request.Headers.Single(h => h.Key == "chatid").Value);
            try
            {
                using (var client = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    _chatWS.Add((id, chatId), client);

                    while (client.State == WebSocketState.Open)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            byte[] buffer = new byte[1024 * 4];
                            WebSocketReceiveResult receiveResult = await client.ReceiveAsync(
                                buffer, CancellationToken.None);
                            if (receiveResult.MessageType == WebSocketMessageType.Close)
                            {
                                _chatWS.Remove((id, chatId));
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
                                if (_chatWS.TryGetValue((contact.UserId, newMsg.ChatId), out var contactSocket))
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
                                    await contactSocket.SendAsync(memoryStream.ToArray(), WebSocketMessageType.Binary, true, CancellationToken.None);
                                }
                                else if (_chatsListWS.TryGetValue(contact.UserId, out var socket))
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
                                    await socket.SendAsync(memoryStream.ToArray(), WebSocketMessageType.Binary, true, CancellationToken.None);
                                    await _chatManager.SeenChat(new Core.DTOs.SeenCommand()
                                    {
                                        ChatId = newMsg.ChatId,
                                        ContactId = contact.Id,
                                        Seen = false
                                    });
                                }
                                else
                                {
                                    await _chatManager.SeenChat(new Core.DTOs.SeenCommand()
                                    {
                                        ChatId = newMsg.ChatId,
                                        ContactId = contact.Id,
                                        Seen = false
                                    });
                                }

                            }

                        }
                    }
                }
            }
            catch
            {
                _chatWS.Remove((id, chatId));
            }
        }
        [Route("/chat/Connect")]
        [HttpGet]
        [Authorize]
        public async Task Connect()
        {
            int id = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                using (var client = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    _chatsListWS.Add(id, client);

                    while (client.State == WebSocketState.Open)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            byte[] buffer = new byte[1024 * 4];
                            WebSocketReceiveResult receiveResult = await client.ReceiveAsync(
                                buffer, CancellationToken.None);
                            if (receiveResult.MessageType == WebSocketMessageType.Close)
                            {
                                _chatsListWS.Remove(id);
                                continue;
                            }
                        }
                    }
                }
            }
            catch
            {
                _chatsListWS.Remove(id);
            }
        }



        [Authorize]
        [HttpGet]
        [Route("/api/chat/seen/{contactChatId}/{contactId}/{seen}")]
        public async Task Seen([FromRoute] int contactChatId, [FromRoute] int contactId, [FromRoute] bool seen)
        {
            int Id = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if ((await _contactManager.GetContactByUserAsync(Id)).Id != contactId)
            {
                return;
            }
            await _chatManager.SeenChat(new Core.DTOs.SeenCommand()
            {
                ChatId = contactChatId,
                ContactId = contactId,
                Seen = seen
            });
        }
    }
    [ProtoContract]
    internal class Message
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
