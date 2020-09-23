using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext _contexto;

        public Repository(AppDbContext contexto)
        {
            _contexto = contexto;
        }
        public void Add(T entity)
        {
            _contexto.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _contexto.Set<T>().Remove(entity);
        }

        public IQueryable<T> Get()
        {
            return _contexto.Set<T>().AsNoTracking();
        }

        public async Task<T> GetById(Expression<Func<T, bool>> predicate)
        {
            return await _contexto.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public int GetTotalRegistros()
        {
            return _contexto.Set<T>().AsNoTracking().Count();
        }

        public List<T> LocalizaPagina<Tipo>(int pagina, int tamanhoPagina) where Tipo : class
        {
            return _contexto.Set<T>()
                .Skip(tamanhoPagina * (pagina - 1))
                .Take(tamanhoPagina).ToList();
        }

        public void Update(T entity)
        {
            _contexto.Entry(entity).State = EntityState.Modified;
            _contexto.Set<T>().Update(entity);
        }
    }
}
