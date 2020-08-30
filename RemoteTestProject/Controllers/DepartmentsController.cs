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
    public class DepartmentsController : ControllerBase
    {
        IMediator _mediator;

        public DepartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Departments/{CompanyId}
        [HttpGet()]
        [Route("Company={companyId}")]
        public async Task<IActionResult> GetDepartmentsByCompany(Guid companyId)
        {
            var cmd = new GetDepartmentsByCompanyIdQuery(companyId);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }

        // GET: api/Departments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var cmd = new GetDepartmentByIdQuery(id);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IList<DepartmentDTO> value)
        {
            var cmd = new InsertDepartmentCommand(value);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] DepartmentDTO value)
        {
            var cmd = new UpdateDepartmentCommand(id, value);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }

        // DELETE: api/Departments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cmd = new DeleteDepartmentCommand(id);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }
    }
}
