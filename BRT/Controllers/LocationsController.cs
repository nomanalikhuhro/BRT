using BRT.Models;
using BRT.Models.Locations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BRT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : Controller
    {
        private readonly BRTDBContext _context;

        public LocationsController(BRTDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("allLocations")]
        public async Task<IActionResult> GetAllLocations()
        {
            
            var allLocations = await (
               from bl in _context.BRTSUBLOCATIONS
               join cl in _context.ContainerLocation.Where(c => c.Status == 1)
               on bl.id equals cl.locationId into tempLocations
               from cl in tempLocations.DefaultIfEmpty() // LEFT JOIN with condition
               select new
               {
                   bl.id,
                   bl.name,
                   Status = cl != null ? cl.Status : 0,  // Ensuring 'Status' defaults to 0 if there's no match
                   ContainerLocationName = cl != null ? cl.ContainerNo : null  // Handling nulls for 'ContainerNo'
               }
                ).Distinct()
                .OrderBy(bl => bl.name) // Order by bl.name
                .ToListAsync();

            if (allLocations == null || !allLocations.Any())
            {
                return NotFound("No location found.");
            }
            return Ok(allLocations);
        }


        [HttpGet("sublocations/{mainLocation}/{locationid}")]
        public async Task<IActionResult> GetSubLocations(string mainLocation, int locationid)
        {
            if (string.IsNullOrEmpty(mainLocation))
            {
                return BadRequest("Invalid main location.");
            }
            var subLocations = await (
                from bl in _context.BRTSUBLOCATIONS
                join cl in _context.ContainerLocation.Where(c => c.Status == 1)
                on bl.id equals cl.locationId into tempLocations
                from cl in tempLocations.DefaultIfEmpty() // LEFT JOIN with condition
                where bl.name.StartsWith(mainLocation)
                select new
                {
                    bl.id,
                    bl.name,
                    Status = cl != null ? cl.Status : 0,  // Ensuring 'Status' defaults to 0 if there's no match
                    ContainerLocationName = cl != null ? cl.ContainerNo : null  // Handling nulls for 'ContainerNo'
                }
            ).Distinct().OrderBy(bl => bl.name).ToListAsync();

            if (subLocations == null || !subLocations.Any())
            {
                return NotFound("No sublocations found.");
            }
            return Ok(subLocations);
        }
   
    }
}
