using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Repository
{
    public interface IRepository<T> where T : Core.Models.BaseEntity
    {
        Task<T> Get(Guid Id);
        Task<IEnumerable<T>> GetAll();
        void Add(T model);
        void Update(T model);
        void Delete(T model);
        Task Complete();
    }
    public class Repository<T> : IRepository<T> where T : Core.Models.BaseEntity
    {
        public CrudDbContext _context;
        public Repository(CrudDbContext _context)
        {
            this._context = _context;
        }

        public void Add(T model)
        {
            _context.Set<T>().Add(model);
        }

        public async Task Complete()
        {
            var changeTracker = _context.ChangeTracker
                .Entries()
                .Where(o => o.State == EntityState.Modified || o.State == EntityState.Added);
            foreach (var item in changeTracker)
            {
                if (item.State == EntityState.Added)
                {
                    (item.Entity as BaseEntity).CreateDate = DateTime.Now;
                    (item.Entity as BaseEntity).UpdateDate = DateTime.Now;
                }
                if (item.State == EntityState.Modified)
                    (item.Entity as BaseEntity).UpdateDate = DateTime.Now;
            }
            await _context.SaveChangesAsync();
        }

        public void Delete(T model)
        {
            _context.Set<T>().Remove(model);
        }

        public async Task<T> Get(Guid Id)
        {
            return await _context.Set<T>().FindAsync(Id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public void Update(T model)
        {
            _context.Set<T>().Attach(model).State = EntityState.Modified;
        }
    }
}
