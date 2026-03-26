using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MuniTrack_API.Contollers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OperatorController : ControllerBase
    {
        private readonly IOperatorService _operatorService;
        public OperatorController(IOperatorService operatorService)
        {
            _operatorService = operatorService;
        }


        [HttpPost]
        [Authorize(Policy = "OnlySysAdmin")]
        public IActionResult CreateOperator([FromBody] CreateOperatorDto Dto)
        {
            try
            {
                _operatorService.CreateOperator(Dto);
                Console.WriteLine($"✅ Operador creado exitosamente. DNI: {Dto.DNI}, NLegajo: {Dto.NLegajo}");
                return Ok(new { message = "Operador registrado correctamente.", data = Dto });
            }
            catch (InvalidOperationException ex)
            {
                // Excepciones de validación - mensaje limpio al cliente (string pelado)
                Console.WriteLine($"⚠️ Validación en CreateOperator: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en CreateOperator: {ex.GetType().Name} - {ex.Message}");
                return BadRequest("No se pudo registrar el operador. Verifique los datos e intente nuevamente.");
            }
        }

        [HttpGet]
        [Authorize(Policy = "AnyRole")]
        public IActionResult GetAllOperator()
        {
            var operators = _operatorService.GetOperators();
            return Ok(operators);
        }
        [HttpGet("{dni}")]
        public IActionResult GetOperatorByDni(int dni) 
        {
            var searchOperator = _operatorService.GetOperatorByDni(dni);
            if(searchOperator == null)
            return NotFound($"No se encontró el operador con DNI {dni}");

            return Ok(searchOperator);
        }

        [HttpDelete]
        [Authorize(Policy = "OnlySysAdmin")]
        [Route("{dniToDelete}")]
        public IActionResult DeleteOperator(int dniToDelete)
        {
            try
            {
                var deleteResult = _operatorService.DeleteOperator(dniToDelete);
                if (deleteResult)
                    return Ok(new { message = "Operador eliminado correctamente." });

                return BadRequest($"No se encontró el operador con DNI: {dniToDelete}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"⚠️ Validación fallida en DeleteOperator: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en DeleteOperator: {ex.GetType().Name} - {ex.Message}");
                return BadRequest("No se pudo eliminar el operador. Intente nuevamente más tarde.");
            }
        }

        [HttpPut("{dni}")]
        [Authorize(Policy = "OnlySysAdmin")]
        public IActionResult UpdateOperator(int dni, [FromBody] UpdateOperatorDto dto)
        {
            try
            {
                var updatedOperator = _operatorService.UpdateOperator(dni, dto);
                return Ok(new { message = "Operador actualizado correctamente.", data = updatedOperator });
            }
            catch (InvalidOperationException ex)
            {
                // Excepciones de validación conocidas
                Console.WriteLine($"⚠️ Validación fallida en UpdateOperator: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Excepciones inesperadas
                Console.WriteLine($"❌ Error en UpdateOperator: {ex.GetType().Name} - {ex.Message}");
                return BadRequest("No se pudo actualizar el operador. Verifique los datos e intente nuevamente.");
            }
        }

    }
}
