using EfCoreApp.Models;

namespace EfCoreApp.Servicies.Interfaces
{
    public interface IDataService
    {
        Task<IEnumerable<DataViewModel>> GetAsync();
    }
}
