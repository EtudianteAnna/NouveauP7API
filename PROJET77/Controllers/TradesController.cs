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
    public async Task<IActionResult> Get(int id)
    {
        _logger.LogInformation($"Récupération de la transaction avec l'ID : {id}");

        var trade = await _tradeRepository.GetByIdAsync(id);
        if (trade == null)
        {
            _logger.LogWarning($"Transaction avec l'ID {id} non trouvée");
            return NotFound();
        }

        _logger.LogInformation($"Transaction avec l'ID {id} récupérée avec succès");
        return Ok(trade);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)] // Created
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad Request
    public async Task<IActionResult> Post([FromBody] Trade trade)
    {
        _logger.LogInformation("Ajout d'une nouvelle transaction");

        await _tradeRepository.AddAsync(trade);

        _logger.LogInformation($"Transaction ajout�e avec succès. ID de la transaction : {trade.TradeId}");

        return CreatedAtAction(nameof(Get), new { id = trade.TradeId }, trade);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, RH")]
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
    [Authorize(Roles = "Admin, RH")]
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



