using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RemoteTestProject.Application.Commands;
using RemoteTestProject.Application.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteTestProject.Application.Handlers
{
    public class InsertStaffHandler : IRequestHandler<InsertStaffCommand, IEnumerable<Guid>>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<InsertStaffHandler> _logger;
        private readonly IMapper _mapper;

        public InsertStaffHandler(
            ServiceContext context,
            ILogger<InsertStaffHandler> logger,
             IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Guid>> Handle(InsertStaffCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to InsertStaffHandler made to add staff record");

            using (var session = await _context.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    var bulkOps = new List<WriteModel<Staff>>();
                    List<Guid> ids = new List<Guid>();
                    foreach (var item in request.Models)
                    {
                        var department = await this._context.Departments.FindSync(session, Builders<Department>.Filter.Eq("Id", item.DepartmentId)).FirstOrDefaultAsync();
                        if (department == null)
                        {
                            _logger.LogInformation("Department in staff not found.");
                            throw new Exception("Department in staff not found.");
                        }

                        var model = _mapper.Map<Staff>(item);
                        ids.Add(model.Id);

                        model.CompanyId = department.CompanyId;
                        bulkOps.Add(new InsertOneModel<Staff>(model));
                    }
                    await this._context.Staffs.BulkWriteAsync(session, bulkOps).ConfigureAwait(false);
                    await session.CommitTransactionAsync();
                    _logger.LogInformation("Call to InsertStaffHandler completed.");

                    return ids;
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
