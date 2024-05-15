﻿using api.Data;
using api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentData _departmentData;

        // GET: api/<DepartmentController>
        public DepartmentController(IDepartmentData departmentData)
        {
            _departmentData = departmentData;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentModel>> Get()
        {
            try
            {
                return Ok(await _departmentData.GetDepartment());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        // POST api/<DepartmentController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DepartmentModel>> Post([FromBody] DepartmentModel value)
        {
            try
            {
                await _departmentData.InsertDepartment(value);
                return Ok("Afdeling blev oprettet");
            }
            catch
            {
                return BadRequest("Kunne ikke oprette Afdeling");
            }

        }

        // PUT api/<DepartmentController>/5
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentModel>> Put([FromBody] DepartmentModel value)
        {
            if (value == null)
            {
                return BadRequest("Skal sende en afdeling!");
            };
            if (value.Id <= 0)
            {
                return BadRequest("Ugyldig afdelings id!");
            }

            try
            {
                await _departmentData.GetDepartmentById(value.Id);
            }
            catch
            {
                return BadRequest("Ugyldig afdelings id!");
            }

            try
            {
                await _departmentData.UpdateDepartment(value);

                return Ok("Afdeling blev opdateret");
            }
            catch
            {
                return Problem("Kunne ikke opdater afdeling");
            }
        }

        // DELETE api/<DepartmentController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Ugyldig afdelings id");
            }


            try
            {
                await _departmentData.GetDepartmentById(id);
            }
            catch
            {
                return BadRequest("Ugyldig afdelings id!");
            }

            try
            {
                await _departmentData.DeleteDepartment(id);

                return Ok("Afdeling blev slettet");
            }
            catch
            {
                return Problem("Kunne ikke slette afdeling");
            }
        }
    }
}
