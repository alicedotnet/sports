using Microsoft.EntityFrameworkCore.Migrations;

namespace Sports.Alice.Migrations
{
    public partial class NewsArticleNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentsCount",
                table: "NewsArticles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsHotContent",
                table: "NewsArticles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentsCount",
                table: "NewsArticles");

            migrationBuilder.DropColumn(
                name: "IsHotContent",
                table: "NewsArticles");
        }
    }
}
