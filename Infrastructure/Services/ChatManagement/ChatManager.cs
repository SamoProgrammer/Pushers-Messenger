using Core.Entities;
using Core.Interfaces;
using Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Services.ChatManagement
{
    public class ChatManager : IChatManager
    {
        private AppDbContext _context;

        public ChatManager(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Message> InsertMessage(Message message)
        {
            await _context.AddAsync<Message>(message);
            await _context.SaveChangesAsync();
            return await _context.Messages.Include(m => m.Sender).Include(m => m.Chat).ThenInclude(c=>c.Contacts).SingleOrDefaultAsync(m => m.Id == message.Id);
        }
    }
}
