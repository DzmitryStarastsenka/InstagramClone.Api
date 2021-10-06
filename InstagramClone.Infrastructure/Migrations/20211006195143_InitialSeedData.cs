using InstagramClone.Application.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstagramClone.Infrastructure.Migrations
{
    public partial class InitialSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string adminPassword = "Jaw-15F-aqA-Kvg";
            PasswordHashHelper.CreatePasswordHash(adminPassword, out byte[] passwordHash, out byte[] passwordSalt);

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "FirstName", "LastName", "UserName", "PasswordHash", "PasswordSalt" },
                values: new object[,]
                {
                    {
                        "Admin", "Test", "admin@gmail.com", passwordHash, passwordSalt
                    }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
