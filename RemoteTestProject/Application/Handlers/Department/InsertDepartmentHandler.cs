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
    public class InsertDepartmentHandler : IRequestHandler<InsertDepartmentCommand, IEnumerable<Guid>>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<InsertDepartmentHandler> _logger;
        private readonly IMapper _mapper;

        public InsertDepartmentHandler(
            ServiceContext context,
            ILogger<InsertDepartmentHandler> logger,
             IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Guid>> Handle(InsertDepartmentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to InsertDepartmentHandler made to add department record");

            using (var session = await _context.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    var bulkOps = new List<WriteModel<Department>>();
                    List<Guid> ids = new List<Guid>();

                    var comapny = await this._context.Companies.FindSync(session, Builders<Company>.Filter.Eq("Id", request)).FirstOrDefaultAsync();
                    if (comapny == null)
                    {
                        await session.AbortTransactionAsync();
                        _logger.LogInformation("Company in department not found.");
                        throw new Exception("Company in department not found.");
                    }

                    foreach (var item in request.Models)
                    {
                        var company = await this._context.Companies.FindSync(session, Builders<Company>.Filter.Eq("Id", item.CompanyId)).FirstOrDefaultAsync();
                        if (company == null)
                        {
                            _logger.LogInformation("Company in department not found.");
                            throw new Exception("Company in department not found.");
                        }

                        var model = _mapper.Map<Department>(item);
                        ids.Add(model.Id);

                        bulkOps.Add(new InsertOneModel<Department>(model));
                    }
                    await this._context.Departments.BulkWriteAsync(session, bulkOps).ConfigureAwait(false);
                    await session.CommitTransactionAsync();
                    _logger.LogInformation("Call to InsertDepartmentHandler completed.");

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
