using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RemoteTestProject.Application.Context;
using RemoteTestProject.Application.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemoteTestProject.Application
{
    public class ServiceContext
    {
        private const string CompanyName = "Companies";
        private const string DepartmentName = "Departments";
        private const string StaffName = "Staffs";

        private readonly IMongoDatabase _database = null;
        public IMongoClient Client { get; }

        public ServiceContext(IOptions<ServiceSettings> settings)
        {
            Client = new MongoClient(settings.Value.ConnectionString);
            if (Client != null)
            {
                _database = Client.GetDatabase(settings.Value.DatabaseName);
                this.CreateIndex();
            }
        }

        void CreateIndex()
        {
            var names = _database.ListCollectionNames().ToList();
            var tables = new List<string>() { CompanyName, DepartmentName, StaffName };
            foreach (var item in tables)
            {
                if (!names.Any(x => x == item))
                {
                    _database.CreateCollection(item);
                }
            }
           
            this.Companies.Indexes.CreateOne(new CreateIndexModel<Company>(Builders<Company>.IndexKeys.Ascending(x => x.Id)));
            this.Companies.Indexes.CreateOne(new CreateIndexModel<Company>(Builders<Company>.IndexKeys.Ascending(x => x.Name)));
            this.Departments.Indexes.CreateOne(new CreateIndexModel<Department>(Builders<Department>.IndexKeys.Ascending(x => x.Id)));
            this.Staffs.Indexes.CreateOne(new CreateIndexModel<Staff>(Builders<Staff>.IndexKeys.Ascending(x => x.Id)));
        }


        public IMongoCollection<Company> Companies
        {
            get
            {
                return _database.GetCollection<Company>(CompanyName);
            }
        }
        public IMongoCollection<Department> Departments
        {
            get
            {
                return _database.GetCollection<Department>(DepartmentName);
            }
        }
        public IMongoCollection<Staff> Staffs
        {
            get
            {
                return _database.GetCollection<Staff>(StaffName);
            }
        }
    }
}
