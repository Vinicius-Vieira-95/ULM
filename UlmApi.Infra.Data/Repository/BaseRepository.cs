using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UlmApi.Domain.Interfaces;

namespace UlmApi.Infra.Data.Repository
{
    public class BaseRepository<TEntity, TType> : IBaseRepository<TEntity, TType> where TEntity : class
    {
        protected readonly Context _context;
        public BaseRepository(Context context)
        {
            _context = context;
        }

        public virtual async Task Insert(TEntity obj)
        {
            await _context.Set<TEntity>().AddAsync(obj);
            await Save();
        }

        public void Update(TEntity obj)
        {
            _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(TType id)
        {
            _context.Set<TEntity>().Remove(Select(id).Result);
            _context.SaveChanges();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IList<TEntity>> Select() => await _context.Set<TEntity>().ToListAsync();

        public virtual async Task<TEntity> Select(TType id) => await _context.Set<TEntity>().FindAsync(id);

    }
}
