using DatingApp.Core.Entities;
using DatingApp.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Infrastructure.Repositories
{
    public class LikeRepository : ILikeRepository
    {

        private readonly ApplicationDbContext _context;

        public LikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public void Add(Like entity)
        {
            _context.Add(entity);
        }

        public void Delete(Like entity)
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
