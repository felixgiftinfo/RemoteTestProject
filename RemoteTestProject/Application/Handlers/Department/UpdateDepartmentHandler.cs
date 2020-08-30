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
    public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, bool>
    {
        private readonly ServiceContext _context;
        private readonly ILogger<UpdateDepartmentHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateDepartmentHandler(
            ServiceContext context,
            ILogger<UpdateDepartmentHandler> logger,
             IMapper mapper
            )
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Call to UpdateDepartmentHandler made to update department record");
            using (var session = await _context.Client.StartSessionAsync())
            {
                try
                {
                    session.StartTransaction();

                    var departmentId = request.Id;

                    var model = await this._context.Departments.FindSync(session, Builders<Department>.Filter.Eq("Id", departmentId)).FirstOrDefaultAsync();
                    if (model == null)
                    {
                        _logger.LogInformation("Department not found.");
                        throw new Exception("Department not found.");
                    }

                    var comapny = await this._context.Companies.FindSync(session, Builders<Company>.Filter.Eq("Id", request.Model.CompanyId)).FirstOrDefaultAsync();
                    if (comapny == null)
                    {
                        _logger.LogInformation("Company in department not found.");
                        throw new Exception("Company in department not found.");
                    }


                    _mapper.Map(request.Model, model);

                    var filter = Builders<Department>.Filter.Eq("Id", departmentId);
                    var result = await this._context.Departments.ReplaceOneAsync(session, filter, model, new ReplaceOptions() { IsUpsert = true });


                    await session.CommitTransactionAsync();
                    _logger.LogInformation("Call to UpdateDepartmentHandler completed.");
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
