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
    public class DeleteCompanyHandler : IRequestHandler<DeleteCompanyCommand, long>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<DeleteCompanyHandler> _logger;

        public DeleteCompanyHandler(
            ServiceContext context,
            ILogger<DeleteCompanyHandler> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<long> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to DeleteCompanyHandler made to delete company");
            using (var session = await _context.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();

                    var companyId = request.Id;
                    var companyFilter = Builders<Domains.Company>.Filter.Eq("Id", companyId);
                    var departmentFilter = Builders<Domains.Department>.Filter.Eq("CompanyId", companyId);
                    var staffFilter = Builders<Domains.Staff>.Filter.Eq("CompanyId", companyId);

                    DeleteResult result = await this._context.Companies.DeleteOneAsync(session, companyFilter);
                    await this._context.Departments.DeleteManyAsync(session, departmentFilter);
                    await this._context.Staffs.DeleteManyAsync(session, staffFilter);

                    await session.CommitTransactionAsync();
                    _logger.LogInformation("Call to DeleteCompanyHandler completed.");

                    return result.DeletedCount;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    _logger.LogInformation("Error deleting company.");
                    throw new Exception("Error deleting company.");
                }
            }
        }
    }
}
