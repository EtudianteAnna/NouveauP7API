using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("ImportToMssql")]
        [Authorize(Roles = "Admin, RH")]
        public async Task<IActionResult> ImportBidListsToMssql()
        {
            try
            {
                using (var connection = new SqlConnection("YourConnectionString"))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var bidLists = _context.GetAllBidLists();

                        using (var tempTable = new DataTable())
                        {
                            tempTable.Columns.Add("BidListId", typeof(int));
                            tempTable.Columns.Add("Account", typeof(string));
                            tempTable.Columns.Add("BidType", typeof(string));
                            tempTable.Columns.Add("BidQuantity", typeof(double));
                            tempTable.Columns.Add("AskQuantity", typeof(double));
                            tempTable.Columns.Add("Bid", typeof(double));
                            tempTable.Columns.Add("Ask", typeof(double));
                            tempTable.Columns.Add("Benchmark", typeof(string));
                            tempTable.Columns.Add("BidListDate", typeof(DateTime));
                            tempTable.Columns.Add("Commentary", typeof(string));
                            tempTable.Columns.Add("BidSecurity", typeof(string));
                            tempTable.Columns.Add("BidStatus", typeof(string));
                            tempTable.Columns.Add("Trader", typeof(string));
                            tempTable.Columns.Add("Book", typeof(string));
                            tempTable.Columns.Add("CreationName", typeof(string));
                            tempTable.Columns.Add("CreationDate", typeof(DateTime));
                            tempTable.Columns.Add("RevisionName", typeof(string));
                            tempTable.Columns.Add("RevisionDate", typeof(DateTime));
                            tempTable.Columns.Add("DealName", typeof(string));
                            tempTable.Columns.Add("DealType", typeof(string));
                            tempTable.Columns.Add("SourceListId", typeof(string));
                            tempTable.Columns.Add("Side", typeof(string));
                            tempTable.Columns.Add("Name", typeof(string));

                            foreach (var bidList in bidLists)
                            {
                                tempTable.Rows.Add(
                                    bidList.BidListId,
                                    bidList.Account,
                                    bidList.BidType,
                                    bidList.BidQuantity,
                                    bidList.AskQuantity,
                                    bidList.Bid,
                                    bidList.Ask,
                                    bidList.Benchmark,
                                    bidList.BidListDate,
                                    bidList.Commentary,
                                    bidList.BidSecurity,
                                    bidList.BidStatus,
                                    bidList.Trader,
                                    bidList.Book,
                                    bidList.CreationName,
                                    bidList.CreationDate,
                                    bidList.RevisionName,
                                    bidList.RevisionDate,
                                    bidList.DealName,
                                    bidList.DealType,
                                    bidList.SourceListId,
                                    bidList.Side,
                                    bidList.Name
                                );
                            }

                            using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                            {
                                bulkCopy.DestinationTableName = "BidLists";
                                bulkCopy.WriteToServer(tempTable);
                            }

                            transaction.Commit();
                        }
                    }
                }

                return Ok("Les données ont été importées avec succès dans MSSQL.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Une erreur s'est produite lors de l'importation des données : {ex.Message}");
            }
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
                return NotFound();
            }

            return Ok(bidList);
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
