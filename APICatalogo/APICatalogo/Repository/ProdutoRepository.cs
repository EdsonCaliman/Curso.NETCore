using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {

        }
        public async Task<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return await Get().OrderBy(c => c.Preco).ToListAsync();
        }
    }
}
