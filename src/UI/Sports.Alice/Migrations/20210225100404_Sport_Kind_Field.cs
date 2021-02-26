using Microsoft.EntityFrameworkCore.Migrations;

namespace Sports.Alice.Migrations
{
    public partial class Sport_Kind_Field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "NewsArticles");

            migrationBuilder.AddColumn<int>(
                name: "SportKind",
                table: "NewsArticles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SportKind",
                table: "NewsArticles");

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "NewsArticles",
                type: "TEXT",
                nullable: true);
        }
    }
}
