using Microsoft.EntityFrameworkCore.Migrations;

namespace Sports.Alice.Migrations
{
    public partial class Add_CategoryName_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "NewsArticles",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "NewsArticles");
        }
    }
}
