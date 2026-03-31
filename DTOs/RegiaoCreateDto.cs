using System.ComponentModel.DataAnnotations;

namespace RegioesApi.DTOs
{
    public class RegiaoCreateDto
    {
        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string UF { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;
    }
}
