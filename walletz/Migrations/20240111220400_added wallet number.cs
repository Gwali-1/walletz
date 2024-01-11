using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace walletz.Migrations
{
    public partial class addedwalletnumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExistingWalletNumber",
                table: "users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExistingWalletNumber",
                table: "users");
        }
    }
}
