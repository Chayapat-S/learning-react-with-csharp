//using api.Helper;
using api.Models;
//using api.Models.Building;
//using api.Services.Building;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Repositories.Abstraction;
//using api.Infrastructure.Middlewares;

namespace api.Controllers
{
	public class UserController : ControllerBase
	{

		[HttpPost("insertupdate")]
		public async Task<IActionResult> InsertUpdate([FromBody] InsertUpdateUserRequest user)
		{
			try
			{

				return StatusCode(200, "");
            }
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}
	}
}

