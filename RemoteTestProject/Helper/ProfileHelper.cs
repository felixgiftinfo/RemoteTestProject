using RemoteTestProject.Application.Domains;
using RemoteTestProject.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;


namespace RemoteTestProject.Helper
{
    public class ProfileHelper : Profile
    {
        public ProfileHelper()
        {
            CreateMap<Company, CompanyDTO>().ReverseMap();
            CreateMap<Company, CompanyReadDTO>().ReverseMap();


            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<Department, DepartmentReadDTO>().ReverseMap();


            CreateMap<Staff, StaffDTO>().ReverseMap();
            CreateMap<Staff, StaffReadDTO>().ReverseMap();
        }
    }
}
