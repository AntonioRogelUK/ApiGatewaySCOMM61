namespace ApiGateway.Gateway.Dtos
{
    public class VentaDto
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid UsuarioId { get; set; }
        public string JuegoId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
