using AutoMapper;
using backend.Core.Dtos.Candidate;
using backend.Core.Dtos.Company;
using backend.Core.Dtos.Job;
using backend.Core.Entities;

namespace backend.Core.AutoMapperConfig
{
	public class AutoMapperConfigProfile : Profile
	{
        public AutoMapperConfigProfile()
        {
			/* Company -- CompanyCreateDto -- CompanyGetDto */
			CreateMap<Company, CompanyCreateDto>().ReverseMap();
            CreateMap<Company, CompanyGetDto>().ReverseMap();

			/* Job -- JobCreateDto -- JobGetDto */
			CreateMap<Job, JobCreateDto>().ReverseMap();
			CreateMap<Job, JobGetDto>()
				/* include the name of the company when mapping */
				.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
				.ReverseMap();

			/* Candidate */
			CreateMap<Candidate, CandidateCreateDto>().ReverseMap();
			CreateMap<Candidate, CandidateGetDto>()
				/* include the title of the job when mapping */
				.ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Job.Title))
				.ReverseMap();
		}
	}
}
