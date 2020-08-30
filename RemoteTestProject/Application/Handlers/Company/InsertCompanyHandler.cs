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
    public class InsertCompanyHandler : IRequestHandler<InsertCompanyCommand, IEnumerable<Guid>>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<InsertCompanyHandler> _logger;
        private readonly IMapper _mapper;

        public InsertCompanyHandler(
            ServiceContext context,
            ILogger<InsertCompanyHandler> logger,
             IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Guid>> Handle(InsertCompanyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to InsertCompanyHandler made to add company record");

            using (var session = await _context.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();
                    var bulkOps = new List<WriteModel<Company>>();
                    List<Guid> ids = new List<Guid>();
                    foreach (var item in request.Models)
                    {
                        var company = await this._context.Companies.FindSync(session, Builders<Company>.Filter.Eq("Name", item.Name)).FirstOrDefaultAsync();
                        if (company != null)
                        {
                            _logger.LogInformation("Company name should be unique.");
                            throw new Exception("Company name should be unique.");
                        }

                        var model = _mapper.Map<Company>(item);
                        ids.Add(model.Id);

                        bulkOps.Add(new InsertOneModel<Company>(model));
                    }
                    await this._context.Companies.BulkWriteAsync(session, bulkOps).ConfigureAwait(false);
                    await session.CommitTransactionAsync();
                    _logger.LogInformation("Call to InsertCompanyHandler completed.");

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
