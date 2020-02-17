using DatingApp.Core.Entities;
using DatingApp.Core.Helpers;
using DatingApp.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {

        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Message entity)
        {
            _context.Add(entity);
        }

        public void Delete(Message entity)
        {
            _context.Remove(entity);
        }

        public async Task<Message> GetMessage(int id)
        {
            // id: is messageId
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public Task<PagedList<Message>> GetMessagesForUser()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Message>> GetMessagesThread(string userId, string recipientId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
