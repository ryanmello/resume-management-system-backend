using AutoMapper;
using backend.Core.Context;
using backend.Core.Dtos.Candidate;
using backend.Core.Dtos.Company;
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

		// get candidate by id
		[HttpGet]
		[Route("get/{id}")]
		public async Task<ActionResult<CandidateGetDto>> GetCandidateById(long id)
		{
			var candidate = await _context.Candidates.FindAsync(id);
			var convertedCandidate = _mapper.Map<CandidateGetDto>(candidate);
			if (convertedCandidate == null)
			{
				return NotFound("Invalid id input");
			}
			return Ok(convertedCandidate);
		}

		// update candidate by id
		[HttpPut]
		[Route("edit/{id}")]
		public async Task<ActionResult<CandidateGetDto>> EditCompanyById(long id, [FromBody] CandidateCreateDto candidate)
		{
			var currentCandidate = await _context.Candidates.FindAsync(id);
			if (currentCandidate is null)
			{
				return NotFound("Invalid id");
			}

			currentCandidate.FirstName = candidate.FirstName;
			currentCandidate.LastName = candidate.LastName;
			currentCandidate.Email = candidate.Email;
			currentCandidate.Phone = candidate.Phone;	
			currentCandidate.CoverLetter = candidate.CoverLetter;
			currentCandidate.JobId = candidate.JobId;
			currentCandidate.UpdatedAt = DateTime.Now;

			await _context.SaveChangesAsync();

			return Ok(currentCandidate);
		}

		// delete candidate by id
		[HttpDelete]
		[Route("delete")]
		public async Task<ActionResult> DeleteCandidateById(long id)
		{
			var candidate = await _context.Candidates.FindAsync(id);
			if (candidate is null)
			{
				return NotFound("Candidate not found");
			}

			_context.Candidates.Remove(candidate);
			await _context.SaveChangesAsync();

			return Ok("Candidate deleted sucessfully");
		}

	}
}
