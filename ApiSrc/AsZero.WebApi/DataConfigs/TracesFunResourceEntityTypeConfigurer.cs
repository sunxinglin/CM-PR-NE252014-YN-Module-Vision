using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StdUnit.One.FuncResources;

namespace AsZero.WebApi.DataConfigs
{
    public class TracesFunResourceEntityTypeConfigurer : IEntityTypeConfiguration<FuncResource>
    {
        public void Configure(EntityTypeBuilder<FuncResource> builder)
        {
            builder.HasData(new FuncResource() {
                Id = 1000,
                UniqueName = "打螺丝NG-拧紧枪反转",
                AllowedClaims = [],
                Configurable = true,
                Description = string.Empty,
                IsDeleted = false,
                Kind = 0,
                ParentId = 1,
                Tag = string.Empty,
                Url = string.Empty,
            });

            builder.HasData(new FuncResource()
            {
                Id = 1001,
                UniqueName = "打螺丝NG-免除反转",
                AllowedClaims = [],
                Configurable = true,
                Description = string.Empty,
                IsDeleted = false,
                Kind = 0,
                ParentId = 1,
                Tag = string.Empty,
                Url = string.Empty,
            });
        }
    }
}
