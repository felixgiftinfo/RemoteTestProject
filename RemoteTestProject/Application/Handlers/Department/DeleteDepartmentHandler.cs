using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RemoteTestProject.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Handlers
{
    public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, long>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<DeleteDepartmentHandler> _logger;

        public DeleteDepartmentHandler(
            ServiceContext context,
            ILogger<DeleteDepartmentHandler> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<long> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to DeleteDepartmentHandler made to delete department");
            using (var session = await _context.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();

                    var departmentId = request.Id;
                    var departmentFilter = Builders<Domains.Department>.Filter.Eq("Id", departmentId);
                    var staffFilter = Builders<Domains.Staff>.Filter.Eq("DepartmentId", departmentId);

                    DeleteResult result = await this._context.Departments.DeleteOneAsync(session, departmentFilter);
                    await this._context.Staffs.DeleteManyAsync(session, staffFilter);

                    await session.CommitTransactionAsync();
                    _logger.LogInformation("Call to DeleteDepartmentHandler completed.");

                    return result.DeletedCount;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    _logger.LogInformation("Error deleting department.");
                    throw new Exception("Error deleting department.");
                }
            }
        }
    }
}
