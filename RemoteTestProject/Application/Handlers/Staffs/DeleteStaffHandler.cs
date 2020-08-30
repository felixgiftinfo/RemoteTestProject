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
    public class DeleteStaffHandler : IRequestHandler<DeleteStaffCommand, long>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<DeleteStaffHandler> _logger;

        public DeleteStaffHandler(
            ServiceContext context,
            ILogger<DeleteStaffHandler> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<long> Handle(DeleteStaffCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to DeleteStaffHandler made to delete Staff");
            try
            {
                var StaffId = request.Id;
                var StaffFilter = Builders<Domains.Staff>.Filter.Eq("Id", StaffId);

                DeleteResult result = await this._context.Staffs.DeleteOneAsync(StaffFilter);

                _logger.LogInformation("Call to DeleteStaffHandler completed.");

                return result.DeletedCount;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error deleting Staff.");
                throw new Exception("Error deleting Staff.");
            }
        }
    }
}
