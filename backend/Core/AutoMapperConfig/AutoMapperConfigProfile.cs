using AutoMapper;
using backend.Core.Dtos.Company;
using backend.Core.Entities;

namespace backend.Core.AutoMapperConfig
{
	public class AutoMapperConfigProfile : Profile
	{
        public AutoMapperConfigProfile()
        {
            /* convert company to company create dto and compnay create dto to company */
            CreateMap<Company, CompanyCreateDto>().ReverseMap();
            CreateMap<Company, CompanyGetDto>().ReverseMap();

            // Job


            // Candidate

        }
    }
}
