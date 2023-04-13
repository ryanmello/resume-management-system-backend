using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Candidate;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CandidateController : ControllerBase
	{
		private ApplicationDbContext _context;
		private IMapper _mapper;

		public CandidateController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> CreateCandidate([FromBody] CandidateCreateDto candidate)
		{
			var newCandidate = _mapper.Map<Candidate>(candidate);
			await _context.Candidates.AddAsync(newCandidate);
			await _context.SaveChangesAsync();

			return Ok("Candidate Created Successfully");
		}
	}
}
