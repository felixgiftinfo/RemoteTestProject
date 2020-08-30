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
    public class StaffsController : ControllerBase
    {
        IMediator _mediator;

        public StaffsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Staffs/{CompanyId}
        [HttpGet()]
        [Route("Company={companyId}")]
        public async Task<IActionResult> GetStaffsByCompany(Guid companyId)
        {
            var cmd = new GetStaffsByCompanyIdQuery(companyId);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }


        // GET: api/Staffs/{DepartmentId}
        [HttpGet()]
        [Route("Company={departmentId}")]
        public async Task<IActionResult> GetStaffsByDepartment(Guid departmentId)
        {
            var cmd = new GetStaffsByDepartmentIdQuery(departmentId);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }

        // GET: api/Staffs/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var cmd = new GetStaffByIdQuery(id);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }

        // POST: api/Staffs
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IList<StaffDTO> value)
        {
            var cmd = new InsertStaffCommand(value);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }

        // PUT: api/Staffs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] StaffDTO value)
        {
            var cmd = new UpdateStaffCommand(id, value);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }

        // DELETE: api/Staffs/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cmd = new DeleteStaffCommand(id);
            var result = await _mediator.Send(cmd);

            return Ok(result);
        }
    }
}
