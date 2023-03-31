using GenshinTheoryCrafting.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenshinTheoryCrafting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private static List<Characters> characters = new List<Characters>
        {
            new Characters {
                Id = 0,
                Name = "Diluc",
                Vision = CHRVision.Pyro,
                Class = CHRCLass.Claymore
            },
            new Characters
            {
                Id = 1,
                Name = "Tighnari",
                Vision = CHRVision.Dendro,
                Class = CHRCLass.Bow
            }
        };

        [HttpGet]
        public async Task<ActionResult<List<Characters>>> Get()
        {
            return Ok(characters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Characters>>> Get(int id)
        {
            var character = characters.Find(x => x.Id == id);
            if (id < 0)
                throw new ArgumentException("Invalid Character ID", nameof(id));

            if (character == null)
                return NotFound();

            return Ok(character);
        }

        [HttpPost("addCharacter")]
        public async Task<ActionResult<List<Characters>>> AddCharacter(Characters character)
        {
            characters.Add(character);

            return Ok(characters);
        }

        [HttpPut("editCharacter")]
        public async Task<ActionResult<List<Characters>>> EditCharacter(Characters request)
        {
            var character = characters.Find(c => c.Id == request.Id);
            if (request.Id < 0)
                throw new ArgumentException("Invalid Character ID", nameof(request.Id));

            if (character == null)
                return NotFound();

            return Ok(characters);
        }
    }
}
