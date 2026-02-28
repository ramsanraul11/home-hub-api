using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddShopping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HouseholdId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShoppingListId = table.Column<Guid>(type: "uuid", nullable: false),
                    HouseholdId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsBought = table.Column<bool>(type: "boolean", nullable: false),
                    BoughtAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BoughtByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingListItem_ShoppingList_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingList_HouseholdId_IsArchived",
                table: "ShoppingList",
                columns: new[] { "HouseholdId", "IsArchived" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingList_HouseholdId_Name",
                table: "ShoppingList",
                columns: new[] { "HouseholdId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItem_HouseholdId_ShoppingListId_IsBought",
                table: "ShoppingListItem",
                columns: new[] { "HouseholdId", "ShoppingListId", "IsBought" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItem_ShoppingListId_CreatedAtUtc",
                table: "ShoppingListItem",
                columns: new[] { "ShoppingListId", "CreatedAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingListItem");

            migrationBuilder.DropTable(
                name: "ShoppingList");
        }
    }
}
