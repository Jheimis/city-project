using System;
using System.Linq;
using System.Threading.Tasks;
using CityProject_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CityProject_WebAPI.Data
{
    public class Repository : IRepository
    {
        private const EntityState detached = EntityState.Detached;
        private readonly DataContext _context;

         public Repository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        
        public void Update<T>(T entity) where T : class
        {
            // var local = _context.Set<T>().Local.Where(x => x.Equals(entity)).FirstOrDefault();
            // if(local != null )
            // {
            //     _context.Entry(local).State = EntityState.Detached;
            // }
            _context.Update(entity);
                    
            
        }

        public void DetachLocal<T>(Func<T,bool> predicate)  where T : class
        {
            var local = _context.Set<T>().Local.Where(predicate).FirstOrDefault();
            if(local != null )
            {
                _context.Entry(local).State = EntityState.Detached;
            }
        }

        public async Task<Cidade[]> GetAllCidadesAsync(bool includeEstado)
        {

            IQueryable<Cidade> query = _context.Cidades;

             if (includeEstado)
            {
                query = query.Include(ce => ce.Estado);
                             
            }

            query = query.AsNoTracking()
                         .OrderBy(c => c.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Cidade[]> GetCidadesAsyncByEstadoId(int estadoId){
            
            IQueryable<Cidade> query = _context.Cidades;

            query = query.AsNoTracking()
                         .OrderBy(cidade => cidade.Id)
                         .Where(cidade => cidade.EstadoId == estadoId);

            return await query.ToArrayAsync();
        }

        public async Task<Estado[]> GetAllEstadosAsync(bool includeCidade)
        {
            IQueryable<Estado> query = _context.Estados;

            if (includeCidade)
            {
                query = query.Include(ce => ce.Cidades);

            }

            query = query.AsNoTracking()
                         .OrderBy(c => c.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Cidade> GetCidadesAsyncById(int cidadeId, bool includeEstado)
        {
            IQueryable<Cidade> query = _context.Cidades;

            if (includeEstado)
            {
                query = query.Include(ce => ce.Estado);

            }

            query = query.AsNoTracking()
                        .Where(c => c.Id == cidadeId)
                        .OrderBy(c => c.Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Estado> GetEstadosAsyncById(int estadoId, bool includeCidades)
        {
            IQueryable<Estado> query = _context.Estados;

               if (includeCidades)
            {
                query = query.Include(ce => ce.Cidades);
                             
            }

            query = query.AsNoTracking()
                        .Where(c => c.Id == estadoId)
                        .OrderBy(c => c.Id);

            return await query.FirstOrDefaultAsync();
        }


        public async Task<ParametroCusto> GetParametroCusto()
        {
           IQueryable<ParametroCusto> query = _context.ParametroCustos;

            query = query.AsNoTracking();  

            return await query.FirstOrDefaultAsync();
        }

       

        public async Task<Cidade> GetCidadeAsyncByNome(string cidadeNome)
        {
            IQueryable<Cidade> query = _context.Cidades;

            query = query.AsNoTracking()
                        .Where(c => c.Nome == cidadeNome)
                        .OrderBy(c => c.Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Cidade[]> GetCidadesAsyncByEstadoId(int estadoId, bool includeEstado)
        {
            IQueryable<Cidade> query = _context.Cidades;

              if (includeEstado)
            {
                query = query.Include(ce => ce.Estado);
                             
            }

            query = query.AsNoTracking()
                         .OrderBy(e => e.Id)
                         .Where(e => e.EstadoId ==estadoId);

            return await query.ToArrayAsync();
        }

        public async Task<Estado> GetEstadoAsyncByNome(string estadoNome)
        {
            IQueryable<Estado> query = _context.Estados;

            query = query.AsNoTracking()
                        .Where(c => c.Nome == estadoNome)
                        .OrderBy(c => c.Id);

            return await query.FirstOrDefaultAsync();
        }
           
    }
}