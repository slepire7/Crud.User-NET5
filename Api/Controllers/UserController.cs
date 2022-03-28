using Infra.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> get()
        {
            return Ok(await _userService.GetAllUserView());
        }
        [HttpGet("{Userid}")]
        public async Task<IActionResult> get([FromRoute] Guid Userid)
        {
            return Ok(await _userService.GetUser(Userid));
        }
        [HttpPut("{Userid}")]
        public async Task<IActionResult> Put([FromRoute] Guid Userid, [FromBody] Core.DTO.UserDTO userDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.Select(o => o.Value.Errors));

                await _userService.UpdateUser(userDTO, Userid);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Core.DTO.UserDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Select(o => o.Value.Errors));
            await _userService.NewUser(user);
            return Ok();
        }
        [HttpDelete("{Userid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid Userid)
        {
            await _userService.DeleteUser(Userid);
            return Ok();
        }
    }
}
