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
    public class CompaniesController : ControllerBase
    {
        IMediator _mediator;

        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cmd = new GetCompaniesQuery();
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }

        // GET: api/Companies/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var cmd = new GetCompanyByIdQuery(id);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }

        // POST: api/Companies
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IList<CompanyDTO> value)
        {
            var cmd = new InsertCompanyCommand(value);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }

        // PUT: api/Companies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CompanyDTO value)
        {
            var cmd = new UpdateCompanyCommand(id, value);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }

        // DELETE: api/Companies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cmd = new DeleteCompanyCommand(id);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }
    }
}
