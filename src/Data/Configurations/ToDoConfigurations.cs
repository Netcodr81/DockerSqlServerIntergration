using DockerSqlServerIntegration.Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DockerSqlServerIntegration.Api.Data.Configurations;

public class ToDoConfigurations : IEntityTypeConfiguration<ToDo>
{
    public void Configure(EntityTypeBuilder<ToDo> builder)
    {
        builder.ToTable("ToDo", "Docker");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
        builder.Property(x => x.IsCompleted).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
    }
}