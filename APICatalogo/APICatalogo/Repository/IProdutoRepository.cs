using APICatalogo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosPorPreco();
    }
}
