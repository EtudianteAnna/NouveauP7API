﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NouveauP7API.Data;
using NouveauP7API.Models;
using System.Data;

namespace NouveauP7API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidListsController : ControllerBase
    {
        private readonly LocalDbContext _context;

        public BidListsController(LocalDbContext context)
        {
            _context = context;
        }
              

        [HttpPost]
        [Authorize(Roles = "Admin, RH")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBidList([FromBody] BidList bidList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.BidLists.AddAsync(bidList);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBidList), new { id = bidList.BidListId }, bidList);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, RH, User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBidList(int id)
        {
            var bidList = await _context.BidLists.FindAsync(id);
            if (bidList == null)
            {
                // Si l'entrée n'existe pas, on crée une nouvelle entrée par défaut
                bidList = new BidList
                {
                    BidListId = id,
                    Account = "DefaultAccount",
                    BidType = "DefaultBidType",
                    BidQuantity = 0.0
                };

                _context.BidLists.Add(bidList);
                await _context.SaveChangesAsync();
            }

            return Ok(bidList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, RH, User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateBidList(
            [FromQuery] string account,
            [FromQuery] string bidType,
            [FromQuery] double? bidQuantity)
        {
            var newBidList = new BidList
            {
                Account = account,
                BidType = bidType,
                BidQuantity = bidQuantity
            };

            _context.BidLists.Add(newBidList);
            await _context.SaveChangesAsync();

            return Ok(newBidList);
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
