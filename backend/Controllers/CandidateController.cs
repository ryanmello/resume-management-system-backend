using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Candidate;
using backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
		public async Task<IActionResult> CreateCandidate([FromForm] CandidateCreateDto candidate, IFormFile pdf)
		{
			var fiveMegaByte = 5 * 1024 * 1024;
			var pdfType = "application/pdf";

			if (pdf.Length > fiveMegaByte || pdf.ContentType != pdfType)
			{
				return BadRequest("Invalid file");
			}

			var resumeUrl = Guid.NewGuid().ToString() + ".pdf";
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Documents", "Pdf", resumeUrl);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await pdf.CopyToAsync(stream);
			}

			var newCandidate = _mapper.Map<Candidate>(candidate);
			newCandidate.ResumeUrl = resumeUrl;
			await _context.Candidates.AddAsync(newCandidate);
			await _context.SaveChangesAsync();

			return Ok("Candidate Created Successfully");
		}

		[HttpGet]
		[Route("get")]
		public async Task<ActionResult<IEnumerable<CandidateGetDto>>> GetCandidates()
		{
			var candidates = await _context.Candidates.Include(candidate => candidate.Job).ToListAsync();
			var convertedCandidates = _mapper.Map<IEnumerable<CandidateGetDto>>(candidates);

			return Ok(convertedCandidates);
		}

		[HttpGet]
		[Route("download/{url}")]
		public IActionResult DownloadPdfFile(string url)
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Documents", "Pdf", url);

			if (!System.IO.File.Exists(filePath))
			{
				return NotFound("File does not exist");
			}

			var pdfBytes = System.IO.File.ReadAllBytes(filePath);
			var file = File(pdfBytes, "application/pdf", url);
			return file;

		}

	}
}
