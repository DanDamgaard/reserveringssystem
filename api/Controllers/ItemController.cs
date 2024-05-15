using api.Data;
using api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IItemData _itemData;

        public ItemController(IItemData itemData)
        {
            _itemData = itemData;
        }

        // GET: api/<ItemController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ItemModel>> Get()
        {
            try
            {
                return Ok(await _itemData.GetItems());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpGet("Types/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<string>> GetTypes()
        {
            try
            {
                return Ok(await _itemData.GetItemTypes());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("Brands/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<string>> GetBrands()
        {
            try
            {
                return Ok(await _itemData.GetItemBrands());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<ItemController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<ItemModel>> Get(int id)
        {
            try
            {

                return Ok(await _itemData.GetItemById(id));
            }
            catch
            {
                return NotFound("Ugyldig bruger id");
            }
        }

        // POST api/<ItemController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ItemModel>> Post([FromBody] ItemModel value)
        {
            try
            {
                await _itemData.InsertItem(value);
                return Ok("Vare blev oprettet");
            }
            catch
            {
                return BadRequest("Kunne ikke oprette vare");
            }

        }

        // PUT api/<ItemController>/5
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ItemModel>> Put([FromBody] ItemModel value)
        {
            if (value == null)
            {
                return BadRequest("Skal sende en Vare!");
            };
            if (value.Id <= 0)
            {
                return BadRequest("Ugyldig Vare id!");
            }

            try
            {
                await _itemData.GetItemById(value.Id);
            }
            catch
            {
                return BadRequest("Ugyldig Vare id!");
            }

            try
            {
                await _itemData.UpdateItem(value);

                return Ok("Vare blev opdateret");
            }
            catch
            {
                return Problem("Kunne ikke opdater vare");
            }
        }

        // DELETE api/<ItemController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Ugyldig vare id");
            }

            try
            {
                await _itemData.GetItemById(id);
            }
            catch
            {
                return BadRequest("Ugyldig Vare id!");
            }

            try
            {
                await _itemData.DeleteItem(id);

                return Ok("Vare blev slettet");
            }
            catch
            {
                return Problem("Kunne ikke slette vare");
            }
        }
    }
}
