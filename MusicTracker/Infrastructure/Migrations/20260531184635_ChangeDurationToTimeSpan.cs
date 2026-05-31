using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDurationToTimeSpan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 ALTER TABLE "Tracks"
                                 ALTER COLUMN "Duration"
                                 TYPE interval
                                 USING make_interval(secs => "Duration");
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 ALTER TABLE "Tracks"
                                 ALTER COLUMN "Duration"
                                 TYPE integer
                                 USING EXTRACT(EPOCH FROM "Duration")::integer;
                                 """);
        }
    }
}
