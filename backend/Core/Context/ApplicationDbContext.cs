using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Core.Context
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

        public DbSet<Company> Companies { get; set; }
		public DbSet<Job> Jobs { get; set; }
		public DbSet<Candidate> Candidates { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			/* each job has one company entity, each company has many
			 * jobs, and each job has a corresponding company id */
			modelBuilder.Entity<Job>()
				.HasOne(job => job.Company)
				.WithMany(company => company.Jobs)
				.HasForeignKey(job => job.CompanyId);

			/* each candidate has one job, each job has many candidates,
			 * and each candidate has an associated job id */
			modelBuilder.Entity<Candidate>()
				.HasOne(candidate => candidate.Job)
				.WithMany(job => job.Candidates)
				.HasForeignKey(candidate => candidate.JobId);

			/* converting the company size enumeration to its corresponding string value */
			modelBuilder.Entity<Company>().Property(company => company.Size).HasConversion<string>();
			modelBuilder.Entity<Job>().Property(job => job.Level).HasConversion<string>();

		}
	}
}
