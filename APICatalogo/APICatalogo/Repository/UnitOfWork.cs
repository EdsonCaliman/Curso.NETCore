using APICatalogo.Context;
using System.Threading.Tasks;

namespace APICatalogo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProdutoRepository _produtoRepository;
        private CategoriaRepository _categoriaRepository;
        public AppDbContext _contexto;
        public UnitOfWork(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_contexto);
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_contexto);
            }
        }

        public async Task Commit()
        {
            await _contexto.SaveChangesAsync();
        }

        public void Dispose()
        {
            _contexto.Dispose();
        }

    }
}
