using Microsoft.EntityFrameworkCore.Migrations;

namespace APICatalogo.Migrations
{
    public partial class Populadb : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("insert into Categorias(Nome, ImagemUrl) values ('Bebidas', 'http://macoratti.net/imagens/1.jpg')");
            mb.Sql("insert into Categorias(Nome, ImagemUrl) values ('Lanches', 'http://macoratti.net/imagens/2.jpg')");
            mb.Sql("insert into Categorias(Nome, ImagemUrl) values ('Sobremesas', 'http://macoratti.net/imagens/3.jpg')");


            mb.Sql("insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) values " +
                "('Coca-Cola Diet', 'Refrigerante de cola', '5.45', 'http://macoratti.net/imagens/coca.jpg', 50, now(), " +
                "(select CategoriaId from Categorias where Nome='Bebidas'))");
            mb.Sql("insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) values " +
                "('Lanche de Atum', 'Lanche de Atum com maionese', '8.50', 'http://macoratti.net/imagens/atum.jpg', 10, now(), " +
                "(select CategoriaId from Categorias where Nome='Lanches'))");
            mb.Sql("insert into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) values " +
                "('Pudim 100 g', 'Pudim de leite condensado 100g', '6.75', 'http://macoratti.net/imagens/pudim.jpg', 20, now(), " +
                "(select CategoriaId from Categorias where Nome='Sobremesas'))");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("delete from produtos");
            mb.Sql("delete from categorias");
        }
    }
}
