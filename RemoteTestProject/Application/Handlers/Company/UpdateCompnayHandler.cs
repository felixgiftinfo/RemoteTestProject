using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
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
    public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, bool>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<UpdateCompanyHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateCompanyHandler(
            ServiceContext context,
            ILogger<UpdateCompanyHandler> logger,
             IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to UpdateCompanyHandler made to update company record");
            using (var session = await _context.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();

                    var companyId = request.Id;

                    var model = await this._context.Companies.FindSync(session, Builders<Company>.Filter.Eq("Id", companyId)).FirstOrDefaultAsync();
                    if (model == null)
                    {
                        _logger.LogInformation("Company not found.");
                        throw new Exception("Company not found.");
                    }

                    _mapper.Map(request.Model, model);

                    var filter = Builders<Company>.Filter.Eq("Id", companyId);
                    var result = await this._context.Companies.ReplaceOneAsync(session, filter, model, new ReplaceOptions() { IsUpsert = true });


                    await session.CommitTransactionAsync();
                    _logger.LogInformation("Call to UpdateCompanyHandler completed.");
                    return result.IsAcknowledged && result.ModifiedCount > 0;
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
