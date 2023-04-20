using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Company;
using backend.Core.Dtos.Job;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class JobController : ControllerBase
	{
		private ApplicationDbContext _context { get; }
		private IMapper _mapper { get; }

		public JobController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> CreateJob([FromBody] JobCreateDto job)
		{
			var newJob = _mapper.Map<Job>(job);
			await _context.Jobs.AddAsync(newJob);
			await _context.SaveChangesAsync();

			return Ok("Job Created Sucessfully");
		}

		[HttpGet]
		[Route("get")]
		public async Task<ActionResult<IEnumerable<JobGetDto>>> GetJobs()
		{
			var jobs = await _context.Jobs.OrderByDescending(q => q.CreatedAt).ToListAsync();
			var convertedJobs = _mapper.Map<IEnumerable<JobGetDto>>(jobs);

			return Ok(convertedJobs);
		}

		// get job by id
		[HttpGet]
		[Route("get/{id}")]
		public async Task<ActionResult<JobGetDto>> GetJobById(long id)
		{
			var job = await _context.Jobs.FindAsync(id);
			var convertedJob = _mapper.Map<JobGetDto>(job);
			if (convertedJob == null)
			{
				return NotFound("Invalid id input");
			}
			return Ok(convertedJob);
		}

		// update company by id
		[HttpPut]
		[Route("edit/{id}")]
		public async Task<ActionResult<JobGetDto>> EditJobById(long id, [FromBody] JobCreateDto job)
		{
			var currentJob = await _context.Jobs.FindAsync(id);
			if (currentJob is null)
			{
				return NotFound("Invalid id");
			}

			currentJob.Title = job.Title;
			currentJob.Level = job.Level;
			currentJob.CompanyId = job.CompanyId;
			currentJob.UpdatedAt = DateTime.Now;

			await _context.SaveChangesAsync();

			return Ok(currentJob);
		}

		// delete company by id
		[HttpDelete]
		[Route("delete")]
		public async Task<ActionResult> DeleteJobById(long id)
		{
			var job = await _context.Jobs.FindAsync(id);
			if (job is null)
			{
				return NotFound("Job not found");
			}

			_context.Jobs.Remove(job);
			await _context.SaveChangesAsync();

			return Ok("Company deleted sucessfully");
		}
	}
}
