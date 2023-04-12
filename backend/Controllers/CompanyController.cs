using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Company;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyController : ControllerBase
	{
		private ApplicationDbContext _context { get; }
		private IMapper _mapper { get; }

		public CompanyController(ApplicationDbContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateDto dto)
		{
			/* map the passed in CompanyCreateDto object to Company object */
			var newCompany = _mapper.Map<Company>(dto);
			await _context.Companies.AddAsync(newCompany);
			await _context.SaveChangesAsync();

			return Ok("Company Created Sucessfully");
		}

		[HttpGet]
		[Route("get")]
		public async Task<ActionResult<IEnumerable<CompanyGetDto>>> GetCompanies()
		{
			var companies = _context.Companies.ToListAsync();
			var convertedCompanies = _mapper.Map<CompanyGetDto>(companies);

			return Ok(convertedCompanies);
		}
	}
}
