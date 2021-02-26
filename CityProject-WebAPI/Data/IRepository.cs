using System.Threading.Tasks;
using CityProject_WebAPI.Models;

namespace CityProject_WebAPI.Data
{
    public interface IRepository
    {
        //Padrão
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        //Cidade
        Task<Cidade[]> GetAllCidadesAsync(bool includeEstados);
        Task<Cidade> GetCidadesAsyncById(int cidadeId, bool includeEstado);
    
        //Estado
        Task<Estado[]> GetAllEstadosAsync(bool includeCidades);
        Task<Estado> GetEstadosAsyncById(int estadoId, bool includeCidades);
        void UpdateEstado(Estado estado);
        
        //ParametroCusto
        Task<ParametroCusto> GetParametroCusto();

        //Execel
        Task<Cidade> GetCidadeAsyncByNome(string cidadeNome);
        Task<Estado> GetEstadoAsyncByNome(string estadoNome);
    }
}