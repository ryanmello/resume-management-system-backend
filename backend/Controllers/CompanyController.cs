using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Company;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
			var companies = await _context.Companies.OrderByDescending(q => q.CreatedAt).ToListAsync();
			var convertedCompanies = _mapper.Map<IEnumerable<CompanyGetDto>>(companies);

			return Ok(convertedCompanies);
		}

		// get company by id
		[HttpGet]
		[Route("get/{id}")]
		public async Task<ActionResult<CompanyGetDto>> GetCompanyById(long id)
		{
			var company = await _context.Companies.FindAsync(id);
			var convertedCompany = _mapper.Map<CompanyGetDto>(company);
			if (convertedCompany == null)
			{
				return NotFound("Invalid id input");
			}
			return Ok(convertedCompany);
		}

		// update company by id
		[HttpPut]
		[Route("edit/{id}")]
		public async Task<ActionResult<CompanyGetDto>> EditCompanyById(long id, [FromBody] CompanyCreateDto company)
		{
			var currentCompany = await _context.Companies.FindAsync(id);
			if(currentCompany is null)
			{
				return NotFound("Invalid id");
			}

			currentCompany.Name = company.Name;
			currentCompany.Size = company.Size;
			currentCompany.UpdatedAt = DateTime.Now;

			await _context.SaveChangesAsync();

			return Ok(currentCompany);
		}

		// delete company by id
		[HttpDelete]
		[Route("delete")]
		public async Task<ActionResult> DeleteCompanyById(long id)
		{
			var company = await _context.Companies.FindAsync(id);
			if (company is null)
			{
				return NotFound("Company not found");
			}

			_context.Companies.Remove(company);
			await _context.SaveChangesAsync();

			return Ok("Company deleted sucessfully");
		}
	}
}
