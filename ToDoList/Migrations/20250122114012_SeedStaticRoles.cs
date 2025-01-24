using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToDoList.Migrations
{
    /// <inheritdoc />
    public partial class SeedStaticRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5d4909b1-078b-4ea1-bd8a-dd2f94786147");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63a7a28c-0437-42f0-aed1-b429c8895beb");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a6b6c5d4-1234-5678-abcd-1a2b3c4d5e6f", null, "User", "USER" },
                    { "d480e7d1-6f02-4f05-a3c5-470d5b6d8e3d", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6b6c5d4-1234-5678-abcd-1a2b3c4d5e6f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d480e7d1-6f02-4f05-a3c5-470d5b6d8e3d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5d4909b1-078b-4ea1-bd8a-dd2f94786147", null, "Admin", "ADMIN" },
                    { "63a7a28c-0437-42f0-aed1-b429c8895beb", null, "User", "USER" }
                });
        }
    }
}
