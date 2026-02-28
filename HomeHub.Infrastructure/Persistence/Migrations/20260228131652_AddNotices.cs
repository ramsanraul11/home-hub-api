using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNotices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HouseholdId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    ScheduledForUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    ArchivedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notice", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notice_HouseholdId_CreatedAtUtc",
                table: "Notice",
                columns: new[] { "HouseholdId", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Notice_HouseholdId_IsArchived",
                table: "Notice",
                columns: new[] { "HouseholdId", "IsArchived" });

            migrationBuilder.CreateIndex(
                name: "IX_Notice_HouseholdId_ScheduledForUtc",
                table: "Notice",
                columns: new[] { "HouseholdId", "ScheduledForUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Notice_HouseholdId_Severity",
                table: "Notice",
                columns: new[] { "HouseholdId", "Severity" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notice");
        }
    }
}
