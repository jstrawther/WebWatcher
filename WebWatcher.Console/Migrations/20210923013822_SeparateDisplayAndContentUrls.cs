using Microsoft.EntityFrameworkCore.Migrations;

namespace WebWatcher.Console.Migrations
{
    public partial class SeparateDisplayAndContentUrls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Websites",
                newName: "DisplayUrl");

            migrationBuilder.AddColumn<string>(
                name: "ContentUrl",
                table: "Websites",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentUrl",
                table: "Websites");

            migrationBuilder.RenameColumn(
                name: "DisplayUrl",
                table: "Websites",
                newName: "Url");
        }
    }
}
