using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ListeningHistoryConfiguration : IEntityTypeConfiguration<ListeningHistory>
{
    public void Configure(EntityTypeBuilder<ListeningHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new
        {
            x.TrackId,
            x.UserName,
            x.PlayedAt
        }).IsUnique();
    }
}