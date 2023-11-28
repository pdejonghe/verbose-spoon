using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProceedixDemoApp.Models;

namespace ProceedixDemoApp.Data
{
    public class LogMessageConfiguration : IEntityTypeConfiguration<LogMessage>
    {
        public void Configure(EntityTypeBuilder<LogMessage> builder)
        {
            builder
                .HasOne(x => x.Application)
                .WithMany(x => x.LogMessages)
                .HasForeignKey(x => x.ApplicationId)
            ;
        }
    }
}