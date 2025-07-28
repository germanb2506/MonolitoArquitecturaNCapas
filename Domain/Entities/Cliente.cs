namespace Domain.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        public string RazonSocial { get; set; } = null!;
        public string Nit { get; set; } = null!;
        public string TipoCliente { get; set; } = null!;
        public string RepresentanteLegal { get; set; } = null!;
        public string CorreoContacto { get; set; } = null!;
        public string TelefonoContacto { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string Pais { get; set; } = null!;
        public string? PaginaWeb { get; set; }
        public string? Notas { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
