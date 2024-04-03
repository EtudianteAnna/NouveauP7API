using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NouveauP7API.Models;

namespace NouveauP7API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly ILogger<RatingController> _logger;

        public RatingController(ILogger<RatingController> logger, IRatingRepository ratingRepository)
        {
            _logger = logger;
            _ratingRepository = ratingRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, RH")]
        [ProducesResponseType(StatusCodes.Status201Created)] // Created
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad Request
        public async Task<IActionResult> Post([FromBody] Rating rating)
        {
            _logger.LogInformation("Ajout d'une nouvelle note");
            rating.Id = 0;

            await _ratingRepository.AddAsync(rating);

            _logger.LogInformation($"Note ajoutée avec succès. ID de la note : {rating.Id}");

            return CreatedAtAction(nameof(Post), new { id = rating.Id }, rating);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, RH")]
        [ProducesResponseType(StatusCodes.Status201Created)] // Success
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad Request
        public async Task<IActionResult> Put(int id, [FromBody] Rating rating)
        {
            _logger.LogInformation($"Mise à jour de la note avec l'ID : {id}");

            if (id != rating.Id)
            {
                _logger.LogError("Incompatibilité dans les ID de note. Requête incorrecte.");
                return BadRequest();
            }

            await _ratingRepository.UpdateAsync(rating);

            _logger.LogInformation($"Note avec l'ID {id} mise à jour avec succès");
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, RH")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // No Content
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Suppression de la note avec l'ID : {id}");

            await _ratingRepository.DeleteAsync(id);

            _logger.LogInformation($"Note avec l'ID {id} supprimée avec succès");
            return NoContent();
        }
    }
} // Fermeture de la classe RatingController
