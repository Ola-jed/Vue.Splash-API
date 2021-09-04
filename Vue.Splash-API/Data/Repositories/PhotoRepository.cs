using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vue.Splash_API.Data.Context;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Data.Repositories
{
    public class PhotoRepository: IPhotoRepository
    {
        private readonly SplashContext _context;

        public PhotoRepository(SplashContext context)
        {
            _context = context;
        }

        public async Task CreatePhoto(Photo photo)
        {
            if (photo == null)
            {
                throw new ArgumentNullException(nameof(photo));
            }
            await _context.Photos.AddAsync(photo);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public IEnumerable<Photo> Find(Func<Photo, bool> func)
        {
            return _context
                .Photos
                .Where(func);
        }

        public void UpdatePhoto(int id)
        {
            // Euh not yet
        }

        public void DeletePhoto(Photo photo)
        {
            if (photo == null)
            {
                throw new ArgumentNullException(nameof(photo));
            }
            _context.Photos.Remove(photo);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}