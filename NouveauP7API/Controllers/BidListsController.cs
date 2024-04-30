using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NouveauP7API.Data;
using NouveauP7API.Models;
using NouveauP7API.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NouveauP7API.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class BidListsController : ControllerBase
    {
        private readonly LocalDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly IBidListRepository _bidListRepository;

        public BidListsController(LocalDbContext context, JwtSettings jwtSettings, IBidListRepository bidListRepository )
        {
            _context = context;
            _jwtSettings = jwtSettings;
            _bidListRepository = bidListRepository;
            
        }

        [HttpPost]
        [Authorize(Roles = "Admin, RH, User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBidList([FromBody] BidList bidList)
        {
            if (_context == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "La base de données n'est pas accessible.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bidListRepository.AddAsync(bidList);
            

            return CreatedAtAction(nameof(GetBidList), new { id = bidList.BidListId }, bidList);
        }

        /*[HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBidLists()
        {
            var bidLists = await _context.BidLists.ToListAsync();
            return Ok(bidLists);
        }*/

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBidList(int id)
        {
            // Valider le jeton JWT
            var userId = ValidateToken(HttpContext.Request.Headers["Authorization"]);
            if (userId == null)
            {
                return Unauthorized();
            }

            var bidList = await _bidListRepository.GetByIdAsync(id);
            if (bidList == null)
            {
                return NotFound();
            }

            return Ok(bidList);
        }

        // Autres actions du contrôleur BidListController

        private string? ValidateToken(string authorizationHeader)
        {
            // Vérifier si l'en-tête d'autorisation est présent et commence par "Bearer "
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return null;
            }

            // Extraire le jeton JWT de l'en-tête d'autorisation
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            // Créer un gestionnaire de jetons JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Configurer les paramètres de validation du jeton
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero
                };

                // Valider le jeton JWT
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validateToken);

                // Extraire l'ID de l'utilisateur à partir des revendications (claims) du jeton
                var jwtToken = (JwtSecurityToken)validateToken;
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                return userId;
            }
            catch
            {
                // En cas d'erreur de validation, retourner null
                return null;
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, RH")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBidList(int id, [FromBody] BidList bidList)
        {
            if (id != bidList.BidListId)
            {
                return BadRequest();
            }

            _context.BidLists.Update(bidList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, RH")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBidList(int id)
        {
            var bidList = await _context.BidLists.FindAsync(id);
            if (bidList == null)
            {
                return NotFound();
            }

            _context.BidLists.Remove(bidList);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}








