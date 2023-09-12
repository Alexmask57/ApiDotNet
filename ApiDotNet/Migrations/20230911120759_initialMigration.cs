using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiDotNet.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8589191f-3fa0-474d-821a-21f474cf9220");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f54413b-ff82-4d32-8798-a8cb29579cd1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5d018b2-f3f7-4f74-abc8-a055fc3dd152");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "198bf318-a3ec-49e9-b453-2b4d7760a9ae", null, "Developer", "DEVELOPER" },
                    { "5c920d6c-c736-4321-998e-28f185ddc7fc", null, "Admin", "ADMIN" },
                    { "d5cb9553-adc7-4772-94e8-ca1708b378c1", null, "Default", "DEFAULT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "198bf318-a3ec-49e9-b453-2b4d7760a9ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5c920d6c-c736-4321-998e-28f185ddc7fc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d5cb9553-adc7-4772-94e8-ca1708b378c1");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8589191f-3fa0-474d-821a-21f474cf9220", null, "Admin", "ADMIN" },
                    { "8f54413b-ff82-4d32-8798-a8cb29579cd1", null, "Default", "DEFAULT" },
                    { "a5d018b2-f3f7-4f74-abc8-a055fc3dd152", null, "Developer", "DEVELOPER" }
                });
        }
    }
}
