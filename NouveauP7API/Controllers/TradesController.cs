using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NouveauP7API.Models;
using NouveauP7API.Repositories;



[ApiController]
[Route("api/[controller]")]
public class TradeController : ControllerBase
{
    private readonly ITradeRepository _tradeRepository;
    private readonly ILogger<TradeController> _logger;


    public TradeController(ILogger<TradeController> logger, ITradeRepository tradeRepository)
    {
        _logger = logger;
        _tradeRepository = tradeRepository;
    }


    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, RH, User")]
    [ProducesResponseType(StatusCodes.Status200OK)] // OK
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] // Internal Server Error
    public async Task<IActionResult> Get(int id)
    {
        _logger.LogInformation($"Récupération de la transaction avec l'ID : {id}");

        try
        {
            var trade = await _tradeRepository.GetByIdAsync(id);
            if (trade == null)
            {
                _logger.LogWarning($"Transaction avec l'ID {id} non trouvée");
                return NotFound();
            }

            _logger.LogInformation($"Transaction avec l'ID {id} récupérée avec succès");
            return Ok(trade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erreur lors de la récupération de la transaction avec l'ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur est survenue lors de la récupération de la transaction.");
        }
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)] // Created
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad Request
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)] // Created
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad Request
    public async Task<IActionResult> Post([FromBody] Trade trade)
    {
        try
        {
            // Vérifier que l'objet trade n'est pas null
            if (trade == null)
            {
                return BadRequest("L'objet Trade ne peut pas être null.");
            }

            // Enregistrer la transaction dans la base de données
            await _tradeRepository.AddAsync(trade);

            _logger.LogInformation($"Transaction ajoutée avec succès. ID de la transaction : {trade.TradeId}");

            // Retourner une réponse HTTP 201 Created avec les informations de la transaction créée
            return CreatedAtAction(nameof(Trade), new { id = trade.TradeId }, trade);
        }
        catch (Exception ex)
        {
            // Gérer les exceptions
            _logger.LogError(ex, "Erreur lors de l'ajout de la transaction.");

            // Retourner une réponse HTTP 400 Bad Request avec un message d'erreur
            return BadRequest("Une erreur est survenue lors de l'ajout de la transaction.");
        }
    }



    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, RH, User")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] // No Content
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad Request
    public async Task<IActionResult> Put(int id, [FromBody] Trade trade)
    {
        _logger.LogInformation($"Mise à jour de la transaction avec l'ID : {id}");

        if (id != trade.TradeId)
        {
            _logger.LogError("Incompatibilité dans les ID de transaction. Requête incorrecte.");
            return BadRequest();
        }

        await _tradeRepository.UpdateAsync(trade);

        _logger.LogInformation($"Transaction avec l'ID {id} mise � jour avec succ�s");
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, RH, User")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] // No Content
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation($"Suppression de la transaction avec l'ID : {id}");

        await _tradeRepository.DeleteAsync(id);

        _logger.LogInformation($"Transaction avec l'ID {id} supprimée avec succès");
        return NoContent();
    }
}










