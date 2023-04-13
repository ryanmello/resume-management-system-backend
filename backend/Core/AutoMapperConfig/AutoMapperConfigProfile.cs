using AutoMapper;
using backend.Core.Dtos.Company;
using backend.Core.Dtos.Job;
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
			CreateMap<Job, JobCreateDto>().ReverseMap();
			// CreateMap<Job, JobGetDto>().ReverseMap();
			CreateMap<Job, JobGetDto>()
				.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
				.ReverseMap();

			// Candidate

		}
	}
}
