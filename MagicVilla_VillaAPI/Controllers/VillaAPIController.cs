using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.LoggerConfiguration;
using MagicVilla_VillaApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaApi.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogging _logger;

        public VillaApiController(AppDbContext dbContext, ILogging logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.Log("Getting all villas", "");
            var villas = await _dbContext.Villas.ToListAsync();

            return Ok(villas);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            
            _logger.Log("Get Villa Error with Id" + id, "error");
            
            var villa = await _dbContext.Villas.FindAsync(id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _dbContext.Villas.AnyAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower()))
            {
                ModelState.AddModelError("CustomError", "Villa already exists!");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
            {
                return BadRequest("Villa data cannot be null");
            }

            var villa = new Villa
            {
                Name = villaDTO.Name,
                Sqft = villaDTO.Sqft,
                Occupancy = villaDTO.Occupancy,
                CreateDate = DateTime.UtcNow
            };

            await _dbContext.Villas.AddAsync(villa);
            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbContext.Villas.FindAsync(id);

            if (villa == null)
            {
                return NotFound();
            }

            _dbContext.Villas.Remove(villa);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }

            var villa = await _dbContext.Villas.FindAsync(id);

            if (villa == null)
            {
                return NotFound();
            }

            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy = villaDTO.Occupancy;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbContext.Villas.FindAsync(id);

            if (villa == null)
            {
                return NotFound();
            }

            var villaDTO = new VillaDTO
            {
                Id = villa.Id,
                Name = villa.Name,
                Sqft = villa.Sqft,
                Occupancy = villa.Occupancy,
            };

            patchDTO.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy = villaDTO.Occupancy;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
