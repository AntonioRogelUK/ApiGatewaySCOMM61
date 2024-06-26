using ApiGateway.Gateway.Dtos;
using ApiGateway.Gateway.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace ApiGateway.Gateway.Controllers
{
    [ApiController]
    [Route("gateway/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public VentasController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<List<VentaDto>>> ObtenerTodos()
        {
            var clienteSqlServer = _httpClientFactory.CreateClient(ApiClients.SqlServer.ToString());
            var response = await clienteSqlServer.GetAsync($"api/Ventas/");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            var result = JsonSerializer.Deserialize<List<VentaDto>>(content, options);

            return Ok(result);
        }



        [HttpPost]
        public async Task<ActionResult> Crear([FromBody]VentaDto venta)
        {
            var clienteSqlServer = _httpClientFactory.CreateClient(ApiClients.SqlServer.ToString());
            var clienteMondoDB = _httpClientFactory.CreateClient(ApiClients.MongoDB.ToString());

            var juegoResponse = await clienteMondoDB.GetAsync($"api/Juegos/{venta.JuegoId}");
            var usuarioResponse = await clienteSqlServer.GetAsync($"api/Usuarios/{venta.UsuarioId}");

            if (!juegoResponse.IsSuccessStatusCode)
            {
                return NotFound("No se encontró juego");
            } else if (!usuarioResponse.IsSuccessStatusCode)
            {
                return NotFound("No se encontró usuario");
            }



            var ventaContent = new StringContent(
                JsonSerializer.Serialize(venta), Encoding.UTF8, "application/json");
            var response = await clienteSqlServer.PostAsync("api/Ventas", ventaContent);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
