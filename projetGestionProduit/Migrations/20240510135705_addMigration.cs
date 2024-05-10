using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetGestionProduit.Migrations
{
    public partial class addMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "828ee7ba-cc75-4208-9638-5ce8d7c38482");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88416054-c551-4acc-a8dc-b8c634c93e3f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2b9eaef9-a5cc-4693-91c7-be9ac33a3394", "2", "Client", "Client" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fcf8559c-d8e9-4bda-b990-2c2c382edaa0", "1", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b9eaef9-a5cc-4693-91c7-be9ac33a3394");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fcf8559c-d8e9-4bda-b990-2c2c382edaa0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "828ee7ba-cc75-4208-9638-5ce8d7c38482", "2", "Client", "Client" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "88416054-c551-4acc-a8dc-b8c634c93e3f", "1", "Admin", "Admin" });
        }
    }
}
