using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AsZero.WebApi.Models.DBModels
{
    public class SystemLogEntityConfiguration : IEntityTypeConfiguration<SystemLog>
    {
        public void Configure(EntityTypeBuilder<SystemLog> builder)
        {
            builder.ToTable("system_log");
            builder.HasKey(c => c.Id);
        }
    }
}
