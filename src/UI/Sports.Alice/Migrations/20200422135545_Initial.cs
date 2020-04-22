using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sports.Alice.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsArticles",
                columns: table => new
                {
                    NewsArticleId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    PublishedDate = table.Column<DateTimeOffset>(nullable: true),
                    ExternalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsArticles", x => x.NewsArticleId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsArticles");
        }
    }
}
