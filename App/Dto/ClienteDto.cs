namespace App.Dto
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? DocumentoIdentidad { get; set; }
        public string? TipoCliente { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string? Empresa { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Pais { get; set; }
        public string? SitioWeb { get; set; }
        public string? NotasInternas { get; set; }
        public DateTime CreadoEn { get; set; }
    }
}
