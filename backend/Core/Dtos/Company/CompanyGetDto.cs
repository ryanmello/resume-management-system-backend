using backend.Core.Enums;

namespace backend.Core.Dtos.Company
{
	public class CompanyGetDto
	{
		public long Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public CompanySize Size { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
	}
}
