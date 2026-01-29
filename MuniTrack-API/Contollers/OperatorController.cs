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
    [Authorize(Policy = "OnlySysAdmin")]
    public class OperatorController : ControllerBase
    {
        private readonly IOperatorService _operatorService;
        public OperatorController(IOperatorService operatorService)
        {
            _operatorService = operatorService;
        }


        [HttpPost]
        public IActionResult CreateOperator([FromBody] CreateOperatorDto Dto)
        {
            try
            {
                _operatorService.CreateOperator(Dto);
                return Ok(Dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
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
        [Route("{dniToDelete}")]
        public IActionResult DeleteOperator(int dniToDelete)
        {
            var deleteResult = _operatorService.DeleteOperator(dniToDelete);
            if (deleteResult)
                return Ok();

            return BadRequest($"La persona con el  siguiente DNI:{dniToDelete} no pudo ser eliminada");
        }

        [HttpPut("{dni}")]
        public IActionResult UpdateOperator(int dni, [FromBody] UpdateOperatorDto dto)
        {
            try
            {
                var updatedOperator = _operatorService.UpdateOperator(dni, dto);
                return Ok(updatedOperator);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
