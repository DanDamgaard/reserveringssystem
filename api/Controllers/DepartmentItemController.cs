using api.Data;
using api.Model;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentItemController : ControllerBase
    {
        private readonly IDepartmentItemData _departmentItemData;

        public DepartmentItemController(IDepartmentItemData departmentItemData)
        {
            _departmentItemData = departmentItemData;
        }

        // GET api/<DepartmentItemController>/5
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<DepartmentItemModel>> Get()
        {
            try
            {
                return Ok(await _departmentItemData.GetDepartmentItems());
            }
            catch(Exception ex)
            {
                Log.Error("GetDepartmentItem error: " + ex.Message);
                return NotFound("Ugyldig afdelings id");
            }
        }

        // GET api/<DepartmentItemController>/5
        [HttpGet("ByDepartmentId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<DepartmentItemModel>> Get(int id)
        {
            try
            {
                return Ok(await _departmentItemData.GetDepartmentItemsByDepartmentId(id));
            }
            catch(Exception ex)
            {
                Log.Error("GetDepartmentItemById error: " + ex.Message);
                return NotFound("Ugyldig afdelings id");
            }
        }

        // POST api/<DepartmentItemController>
        [HttpPost("{ItemId}/{DepartmentId}/{Count}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(int ItemId, int DepartmentId, int Count)
        {
            try
            {
                await _departmentItemData.InsertDepartmentItem(ItemId, DepartmentId, Count);
                return Ok("Afdeling varer blev oprettet");
            }
            catch(Exception ex)
            {
                Log.Error("CreateDepartmentItem error: " + ex.Message);
                return BadRequest("Kunne ikke oprette afdeling varer");
            }

        }

        // PUT api/<DepartmentItemController>/5
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentItemModel>> Put([FromBody] DepartmentItemModel value)
        {
            if (value == null)
            {
                Log.Error("UpdateDepartmentItem error: Invalid item sent");
                return BadRequest("Skal sende en afdelings vare!");
            };
            if (value.Id <= 0)
            {
                Log.Error("UpdateDepartmentItem error: Invalid Id");
                return BadRequest("Ugyldig id!");
            }

            try
            {
                await _departmentItemData.GetDepartmentItemsById(value.Id);
            }
            catch
            {
                Log.Error("UpdateDepartmentItem error: Invalid Id");
                return BadRequest("Ugyldig afdelings vare id!");
            }

            try
            {
                await _departmentItemData.UpdateDepartmentItem(value);

                return Ok("Afdelings vare blev opdateret");
            }
            catch(Exception ex)
            {
                Log.Error("UpdateDepartmentItem error: " + ex.Message);
                return Problem("Kunne ikke opdater afdelings vare");
            }
        }

        // DELETE api/<DepartmentItemController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                Log.Error("DeleteDepartmentItem error: Invalid Id");
                return BadRequest("Ugyldig afdelings vare id");
            }


            try
            {
                await _departmentItemData.GetDepartmentItemsById(id);
            }
            catch
            {
                Log.Error("DeleteDepartmentItem error: Invalid Id");
                return BadRequest("Ugyldig afdelings vare id!");
            }

            try
            {
                await _departmentItemData.DeleteDepartmentItem(id);

                return Ok("Afdeling vare blev slettet");
            }
            catch(Exception ex)
            {
                Log.Error("DeleteDepartmentItem error: " + ex.Message);
                return Problem("Kunne ikke slette afdeling vare");
            }
        }
    }
}
