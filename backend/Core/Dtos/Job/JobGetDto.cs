using backend.Core.Enums;

namespace backend.Core.Dtos.Job
{
	public class JobGetDto
	{
		public string Title { get; set; }
		public JobLevel Level { get; set; }
		public bool IsActive { get; set; } = true;
	}
}
