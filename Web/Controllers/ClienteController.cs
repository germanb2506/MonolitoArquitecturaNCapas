using App.Dto;
using App.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;
        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteService.ObtenerClientes();
            return View(clientes.Data);
        }
        [HttpPost]
        public async Task<JsonResult> Crear([FromBody] ClienteDto cliente )
        {

            if (cliente == null) return new JsonResult("Aca no llegaron datos");
            var creado = await _clienteService.CrearCliente(cliente);
            if (creado.IsExitoso == true)
            {
                return new JsonResult(creado);
            }
            else
            {
                return new JsonResult(creado);
            }
                return new JsonResult(cliente);
        }
    }
}
