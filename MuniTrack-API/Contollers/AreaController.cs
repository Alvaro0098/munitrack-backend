using Application.Dtos;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MuniTrack_API.Contollers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;
        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }

        [HttpPost]
        public IActionResult CreateArea([FromBody] CreateAndUpdateAreaDTO Dto)
        {
            try
            {
                _areaService.CreateArea(Dto);
                return Ok(Dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = "AnyRole")]
        public IActionResult GetAllAreas()
        {
            var areas = _areaService.GetAreas();
            return Ok(areas);
        }
        [HttpGet("{id}")]
        public IActionResult GetAreaById(int id)
        {
            var searchArea = _areaService.GetAreaById(id);
            if (searchArea == null)
                return NotFound($"No se encontró el area con el id {id}");

            return Ok(searchArea);
        }

        [HttpDelete]
        [Route("{idToDelete}")]
        public IActionResult DeleteArea(int idToDelete)
        {
            var deleteResult = _areaService.DeleteArea(idToDelete);
            if (deleteResult)
                return Ok();

            return BadRequest($"El area con el  siguiente id:{idToDelete} no pudo ser eliminada");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateArea(int id, [FromBody] CreateAndUpdateAreaDTO dto)
        {
            try
            {
                var updatedArea = _areaService.UpdateArea(id, dto);
                return Ok(updatedArea);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
