using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProceedixDemoApp.Models;

namespace ProceedixDemoApp.Data
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasData(
                new Application() { Id = 1, Name = "px_demo_app" }
            );
        }
    }
}