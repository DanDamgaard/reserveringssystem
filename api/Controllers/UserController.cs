using api.Data;
using api.Model;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserData _userdata;
        private readonly IConfiguration _configuration;

        public UserController(IUserData userdata, IConfiguration configuration)
        {
            _userdata = userdata;
            _configuration = configuration;
        }
        // GET: api/<UserController>
        [HttpGet, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserModel>> Get()
        {
            try
            {
                return Ok(await _userdata.GetUsers());
            }
            catch (Exception ex)
            {
                Log.Error("GetUsers error: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        // GET api/<UserController>/5
        [HttpGet("{id}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<UserModel>> Get(int id)
        {
            try
            {

                return Ok(await _userdata.GetUserById(id));
            }
            catch(Exception ex)
            {
                Log.Error("GetUsersById error: " + ex.Message);
                return NotFound("Ugyldig bruger id");
            }
        }

        [HttpGet("ByName/{name}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<UserModel>> Get(string name)
        {
            try
            {
                return Ok(await _userdata.GetUserByName(name));
            }
            catch(Exception ex)
            {
                Log.Error("GetUsersByName error: " + ex.Message);
                return NotFound("Ugyldig brugenavn");
            }
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<UserModel>> Get([FromBody] UserModel value)
        {
            try
            {
                UserModel u = await _userdata.GetUserByName(value.Username);
                if (!BCrypt.Net.BCrypt.Verify(value.Password, u.Password))
                {
                    return NotFound("Kunne ikke finde bruger med login");
                }
                string token = CreateToken(u);
                u.Token = token;
                return Ok(u);
            }
            catch(Exception ex)
            {
                Log.Error("Login error: " + ex.Message);
                return NotFound("Kunne ikke finde bruger med login");
            }
        }



        // POST api/<UserController>
        [HttpPost, Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserModel>> Post([FromBody] UserModel value)
        {
            try
            {
                string hash = BCrypt.Net.BCrypt.HashPassword(value.Password);

                value.Password = hash;

                await _userdata.InsertUser(value);


                return Ok("Bruger blev oprettet");
            }
            catch(Exception ex)
            {
                Log.Error("CreateUser error: " + ex.Message);
                return BadRequest("Kunne ikke oprette bruger");
            }

        }

        // PUT api/<UserController>/5
        [HttpPut, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserModel>> Put([FromBody] UserModel value)
        {
            if (value == null)
            {
                Log.Error("UpdateUsers error: invalid user");
                return BadRequest("Skal sende en bruger!");
            };
            if (value.Id <= 0)
            {
                Log.Error("UpdateUsers error: Id");
                return BadRequest("Ugyldig bruger id!");
            }

            try
            {
               UserModel? user = await _userdata.GetUserById(value.Id);
            }
            catch
            {
                Log.Error("UpdateUsers error: Invalid Id");
                return BadRequest("Ugyldig bruger id!");
            }



            if (string.IsNullOrEmpty(value.Username))
            {
                Log.Error("UpdateUsers error: Invalid username");
                return BadRequest("Skal sende brugernavn!");
            };

            if (!string.IsNullOrEmpty(value.Password))
            {
                try
                {
                    string hash = BCrypt.Net.BCrypt.HashPassword(value.Password);

                    value.Password = hash;

                    await _userdata.UpdateUser(value);

                    return Ok("Bruger blev opdateret");
                }
                catch(Exception ex)
                {
                    Log.Error("UpdateUsers error: " + ex.Message);
                    return Problem("Kunne ikke opdater bruger");
                }
            }
            else
            {
                try
                {
                    await _userdata.UpdateUserNoPass(value);
                    return Ok("Bruger blev opdateret");
                }
                catch(Exception ex)
                {
                    Log.Error("UpdateUsers error: " + ex.Message);
                    return Problem("Kunne ikke opdater bruger");
                }
            }
            
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                Log.Error("DeleteUsers error: Invalid Id");
                return BadRequest("Ugyldig bruger id");
            }


            try
            {
               UserModel? user = await _userdata.GetUserById(id);
            }
            catch
            {
                Log.Error("DeleteUsers error: Invalid Id");
                return BadRequest("Ugyldig bruger id!");
            }

            try
            {
                await _userdata.DeleteUser(id);

                return Ok("Bruger blev slettet");
            }
            catch(Exception ex)
            {
                Log.Error("DeleteUsers error: " + ex.Message);
                return Problem("Kunne ikke slette bruger");
            }
        }

        private string CreateToken(UserModel user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Tokken").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            string JWT = new JwtSecurityTokenHandler().WriteToken(token);

            return JWT;
        }

    }
}
