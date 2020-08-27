using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SavioAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(name: "First Name", type: "varchar(80)", nullable: true),
                    LastName = table.Column<string>(name: "Last Name", type: "varchar(80)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Sex = table.Column<bool>(type: "bit", nullable: false),
                    Job = table.Column<string>(type: "varchar(100)", nullable: true),
                    CountryCode = table.Column<string>(type: "char(3)", maxLength: 3, nullable: true),
                    StateCode = table.Column<string>(type: "char(3)", maxLength: 3, nullable: true),
                    CivilState = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
