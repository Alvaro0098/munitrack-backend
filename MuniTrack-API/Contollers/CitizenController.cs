using Microsoft.AspNetCore.Mvc;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Application.Services;
using Microsoft.AspNetCore.Authorization;


namespace MuniTrack_API.Contollers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AnyRole")]
    public class CitizenController : Controller
    {

        private readonly ICitizenService _citizenService;

        public CitizenController(ICitizenService citizenService)
        {
            _citizenService = citizenService;
        }

        [HttpPost] //post
        public IActionResult CreateCitizen([FromBody] CreateCitizenDto Dto)
        {
            try
            {
                _citizenService.CreateCitizen(Dto);
                return Ok(Dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAllCitizens()
        {
            var citizen = _citizenService.GetCitizen();
            return Ok(citizen);
        }
        [HttpGet("{dni}")]
        public IActionResult GetCitizenByDni(int dni)
        {
            var searchCitizen = _citizenService.GetCitizenByDni(dni);
            if (searchCitizen== null)
                return NotFound($"No se encontró el ciudadano con DNI {dni}");

            return Ok(searchCitizen);
        }

        [HttpDelete]
        [Route("{dniToDelete}")]
        public IActionResult DeleteCitizen(int dniToDelete)
        {
            var deleteResult = _citizenService.DeleteCitizen(dniToDelete);
            if (deleteResult)
                return Ok();

            return BadRequest($"La persona con el  siguiente DNI:{dniToDelete} no pudo ser eliminada");
        }
    }
}
