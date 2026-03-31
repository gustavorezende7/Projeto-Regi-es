namespace RegioesApi.DTOs
{
    public class RegiaoReadDto
    {
        public int Id { get; set; }
        public string UF { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
