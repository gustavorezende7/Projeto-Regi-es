using System.ComponentModel.DataAnnotations;

namespace RegioesApi.Models
{
    public class Regiao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo UF é obrigatório.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "UF deve ter exatamente 2 caracteres.")]
        public string UF { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome da região é obrigatório.")]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
