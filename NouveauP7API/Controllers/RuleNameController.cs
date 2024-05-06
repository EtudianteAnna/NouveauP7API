using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NouveauP7API.Models;
using NouveauP7API.Repositories;

namespace NouveauP7API.Controllers
{

    [ApiController]
[Route("api/[controller]")]
public class RuleNameController : ControllerBase
{
    private readonly IRuleNameRepository _ruleNameRepository;
    private readonly ILogger<RuleNameController> _logger;

    public RuleNameController(ILogger<RuleNameController> logger, IRuleNameRepository ruleNameRepository)
    {
        _logger = logger;
        _ruleNameRepository = ruleNameRepository;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, RH, User")]
    [ProducesResponseType(StatusCodes.Status200OK)] // OK
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found
    public async Task<IActionResult> Get(int id)
    {
        _logger.LogInformation($"Récupération du nom de règle avec l'ID : {id}");

        var ruleName = await _ruleNameRepository.GetByIdAsync(id);
            if (ruleName == null)
        {
            _logger.LogWarning($"Nom de règle avec l'ID {id} non trouvé");
            return NotFound();
        }

        _logger.LogInformation($"Nom de règle avec l'ID {id} récupère avec succès");
        return Ok(ruleName);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, RH, User")]
    [ProducesResponseType(StatusCodes.Status201Created)] // Created
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad Request
    public async Task<IActionResult> Post([FromBody] RuleName ruleName)
    {
            _logger.LogInformation("Ajout d'un nouveau nom de règle");

        await _ruleNameRepository.AddAsync(ruleName);

        _logger.LogInformation($"Nom de règle ajout� avec succès. ID du nom de règle : {ruleName.Id}");

        return CreatedAtAction(nameof(Get), new { id = ruleName.Id }, ruleName);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, RH, User")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] // No Content
    [ProducesResponseType(StatusCodes.Status400BadRequest)] // Bad Request
    public async Task<IActionResult> Put(int id, [FromBody] RuleName ruleName)
    {
        _logger.LogInformation($"Mise à jour du nom de règle avec l'ID : {id}");

        if (id != ruleName.Id)
        {
            _logger.LogError("Incompatibilit� dans les ID de nom de règle. Requête incorrecte.");
            return BadRequest();
        }

        await _ruleNameRepository.UpdateAsync(ruleName);

        _logger.LogInformation($"Nom de règle avec l'ID {id} mis à jour avec succès");
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, RH, User")]
    [ProducesResponseType(StatusCodes.Status204NoContent)] // No Content
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Not Found
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation($"Suppression du nom de règle avec l'ID : {id}");

        await _ruleNameRepository.DeleteAsync(id);

        _logger.LogInformation($"Nom de règle avec l'ID {id} supprimé avec succès");
        return NoContent();
    }
}
}