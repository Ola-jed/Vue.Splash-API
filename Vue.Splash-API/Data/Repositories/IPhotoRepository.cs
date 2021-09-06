using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vue.Splash_API.Models;

namespace Vue.Splash_API.Data.Repositories
{
    public interface IPhotoRepository
    {
        Task CreatePhoto(Photo photo);
        Task<Photo> GetPhoto(int id);
        IEnumerable<Photo> Find(Func<Photo, bool> func);
        void UpdatePhoto(int id);
        void DeletePhoto(Photo photo);
        Task SaveChanges();
    }
}