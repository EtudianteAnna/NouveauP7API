using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NouveauP7API.Data;
using NouveauP7API.Models;

namespace NouveauP7API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly ILogger<RatingController> _logger;
        private readonly LocalDbContext _localDbContext;

        public RatingController(ILogger<RatingController> logger, IRatingRepository ratingRepository, LocalDbContext localDbContext)
        {
            _logger = logger;
            _ratingRepository = ratingRepository;
            _localDbContext = localDbContext;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, RH, User")]
        [ProducesResponseType(StatusCodes.Status201Created)] // Created
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad Request
        public async Task<IActionResult> Post([FromBody] Rating rating)
        {
            _logger.LogInformation("Ajout d'une nouvelle note");
            if (rating.Id != 0)

            {
                _logger.LogWarning("L'ID de la note ne doit pas être défini lors de la création.");
                return BadRequest("L'ID de la note ne doit pas être défini lors de la création.");
            }
            // Ajouter la nouvelle note
             _localDbContext.Rating.Add(rating);
             await _localDbContext.SaveChangesAsync();
            _logger.LogInformation($"Note ajoutée avec succès. ID de la note : {rating.Id}");
                      

            return CreatedAtAction(nameof(Get), new { id = rating.Id }, rating);
        }

        [HttpGet("{id}")]
        [Authorize(Roles ="Admin,RH,User")]
        public async Task<IActionResult> Get(int id)
        {
            var rating = await _localDbContext.Rating.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (rating == null)
            {
                return NotFound();
            }

            return Ok(rating);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, RH, User")]
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
        [Authorize(Roles = "Admin, RH, User")]
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
} 
