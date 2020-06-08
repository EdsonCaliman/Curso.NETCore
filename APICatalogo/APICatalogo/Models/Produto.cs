using APICatalogo.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Models
{
    [Table("Produtos")]
    public class Produto : IValidatableObject
    {
        [Key]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage ="O nome é obrigatório")]
        [MaxLength(80)]
        [PrimeiraLetraMaiusculaAttribute]
        public string Nome { get; set; }
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [MaxLength(300)]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(1, 10000, ErrorMessage ="O preço deve estar entre {1} e {2}")]
        public decimal Preco { get; set; }
        [Required]
        [MaxLength(500)]
        public string ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.Nome))
            {
                var primeiraLetra = this.Nome[0].ToString();
                if (primeiraLetra != primeiraLetra.ToUpper())
                {
                    yield return new ValidationResult("A primeira letra do produto deve ser maiúscula.",
                    new[]
                    {
                        nameof(this.Nome)
                    }
                    );
                }
            }
        }
    }
}
