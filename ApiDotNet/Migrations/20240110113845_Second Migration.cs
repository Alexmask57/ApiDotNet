using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiDotNet.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "129b9d41-e2c9-4eb2-a859-8507846aa9ea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9ba5ddf3-c500-4d63-a6ec-7c866eeb16c6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a769c766-c6a3-4bfc-8d31-4ff349df0453");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a1f2965-1b83-4d1f-9187-6ebb4f6c739e", null, "Default", "DEFAULT" },
                    { "5e70b113-9ebc-4d1f-9ebb-6808faf2a392", null, "Admin", "ADMIN" },
                    { "dcf56798-52f5-4362-9e83-a1862cb30f0e", null, "Developer", "DEVELOPER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a1f2965-1b83-4d1f-9187-6ebb4f6c739e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e70b113-9ebc-4d1f-9ebb-6808faf2a392");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcf56798-52f5-4362-9e83-a1862cb30f0e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "129b9d41-e2c9-4eb2-a859-8507846aa9ea", null, "Default", "DEFAULT" },
                    { "9ba5ddf3-c500-4d63-a6ec-7c866eeb16c6", null, "Admin", "ADMIN" },
                    { "a769c766-c6a3-4bfc-8d31-4ff349df0453", null, "Developer", "DEVELOPER" }
                });
        }
    }
}
