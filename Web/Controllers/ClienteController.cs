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

        [HttpGet]
        public async Task<JsonResult> ObtenerTodos()
        {
            var resultado = await _clienteService.ObtenerClientes();
            return Json(resultado);
        }

        [HttpPost]
        public async Task<JsonResult> Crear([FromBody] ClienteDto cliente)
        {
            if (cliente == null) 
                return Json(new { success = false, message = "No se recibieron datos" });

            var resultado = await _clienteService.CrearCliente(cliente);
            return Json(resultado);
        }

        [HttpPost]
        public async Task<JsonResult> Actualizar([FromBody] ClienteDto cliente)
        {
            if (cliente == null) 
                return Json(new { success = false, message = "No se recibieron datos" });

            var resultado = await _clienteService.ActualizarCliente(cliente);
            return Json(resultado);
        }

        [HttpPost]
        public async Task<JsonResult> Eliminar([FromBody] object data)
        {
            if (data == null) 
                return Json(new { success = false, message = "No se recibieron datos" });

            // Extraer el ID del objeto dinámico
            var idProperty = data.GetType().GetProperty("id");
            if (idProperty == null)
                return Json(new { success = false, message = "ID no encontrado en los datos" });

            var id = Convert.ToInt32(idProperty.GetValue(data));
            var resultado = await _clienteService.EliminarCliente(id);
            return Json(resultado);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerPorId(int id)
        {
            var resultado = await _clienteService.ObtenerClientePorId(id);
            return Json(resultado);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerPorNit(string nit)
        {
            var resultado = await _clienteService.ObtenerClientePorNit(nit);
            return Json(resultado);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerPorCorreo(string correo)
        {
            var resultado = await _clienteService.ObtenerClientePorCorreo(correo);
            return Json(resultado);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerActivos()
        {
            var resultado = await _clienteService.ObtenerClientesActivos();
            return Json(resultado);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerPorCiudad(string ciudad)
        {
            var resultado = await _clienteService.ObtenerClientesPorCiudad(ciudad);
            return Json(resultado);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerPorPais(string pais)
        {
            var resultado = await _clienteService.ObtenerClientesPorPais(pais);
            return Json(resultado);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerPorTipo(string tipoCliente)
        {
            var resultado = await _clienteService.ObtenerClientesPorTipo(tipoCliente);
            return Json(resultado);
        }

        [HttpGet]
        public async Task<JsonResult> BuscarPorRazonSocial(string razonSocial)
        {
            var resultado = await _clienteService.BuscarClientesPorRazonSocial(razonSocial);
            return Json(resultado);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerPorRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            var resultado = await _clienteService.ObtenerClientesPorRangoFechas(fechaInicio, fechaFin);
            return Json(resultado);
        }

        [HttpGet]
        public async Task<JsonResult> ValidarNitUnico(string nit, int? idExcluir = null)
        {
            var resultado = await _clienteService.ValidarNitUnico(nit, idExcluir);
            return Json(resultado);
        }

        [HttpGet]
        public async Task<JsonResult> ValidarCorreoUnico(string correo, int? idExcluir = null)
        {
            var resultado = await _clienteService.ValidarCorreoUnico(correo, idExcluir);
            return Json(resultado);
        }
    }
}
