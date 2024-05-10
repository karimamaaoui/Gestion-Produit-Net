using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetGestionProduit.Migrations
{
    public partial class RolesSeedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "828ee7ba-cc75-4208-9638-5ce8d7c38482", "2", "Client", "Client" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "88416054-c551-4acc-a8dc-b8c634c93e3f", "1", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "828ee7ba-cc75-4208-9638-5ce8d7c38482");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88416054-c551-4acc-a8dc-b8c634c93e3f");
        }
    }
}
