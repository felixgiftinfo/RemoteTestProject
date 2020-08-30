using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RemoteTestProject.Application.Commands;
using RemoteTestProject.Application.DTOs;
using RemoteTestProject.Application.Queries;

namespace RemoteTestProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BinaryStringsController : ControllerBase
    {


        // POST: api/Companies
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            string result = this.ValidateBinaryString(value);

            return Ok(result);
        }
        private string ValidateBinaryString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "Input value should not be empty";

            int zeroCount = 0;
            int oneCount = 0;

            foreach (var item in input)
            {
                if (item == '0')
                    zeroCount += 1;
                else if (item == '1')
                    oneCount += 1;
            }
            return zeroCount == oneCount ? "Valid" : "Invalid";
        }
    }
}
