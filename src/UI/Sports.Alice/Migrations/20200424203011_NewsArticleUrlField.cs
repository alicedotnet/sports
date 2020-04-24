using Microsoft.EntityFrameworkCore.Migrations;

namespace Sports.Alice.Migrations
{
    public partial class NewsArticleUrlField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "NewsArticles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "NewsArticles");
        }
    }
}
