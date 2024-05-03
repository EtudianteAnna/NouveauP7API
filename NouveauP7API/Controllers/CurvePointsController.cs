using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NouveauP7API.Models;
using NouveauP7API.Repositories;

namespace P7CreateRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurvePointsController : ControllerBase
    {
        private readonly ICurvePointRepository _curvePointRepository;
        private readonly ILogger<CurvePointsController> _logger;

public CurvePointsController(ILogger<CurvePointsController> logger, ICurvePointRepository curvePointRepository)
        {
            _logger = logger;
            _curvePointRepository = curvePointRepository;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CurvePoints>> GetCurvePoint(int id)
        {
            var curvePoint = await _curvePointRepository.GetByIdAsync(id);

            if (curvePoint == null)
            {
                return NotFound();
            }

            return Ok (curvePoint); //code 200
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCurvePoint(int id, CurvePoints curvePoint)
        {
            if (id != curvePoint.Id)
            {
                return BadRequest();
            }
            var curvePointExist = await _curvePointRepository.GetByIdAsync(id);

            if (curvePointExist == null)
            {
                return NotFound();
            }
            await _curvePointRepository.UpdateAsync(curvePoint);

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CurvePoints>> PostCurvePoint(CurvePoints curvePoint)
        {
            if (curvePoint == null)
            {
                return BadRequest(); // Retourne un résultat BadRequest si curvePoint est null
            }

            try
            {
                await _curvePointRepository.AddAsync(curvePoint);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Une erreur est survenue lors de l'ajout de la ressource : {ex.Message}");
                return Problem("Une erreur est survenue lors de l'ajout de la ressource.", statusCode: 500);
            }

            return CreatedAtAction("GetCurvePoint", new { id = curvePoint.Id }, curvePoint);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCurvePoint(int id)
        {
            var curvePoint = await _curvePointRepository.GetByIdAsync(id);

            if (curvePoint == null)
            {
                return NotFound();
            }

            await _curvePointRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
