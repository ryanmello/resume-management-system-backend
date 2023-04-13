using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Job;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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



	}
}
