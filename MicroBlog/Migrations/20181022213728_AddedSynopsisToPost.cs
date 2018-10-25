using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroBlog.Migrations
{
    public partial class AddedSynopsisToPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Synopsis",
                table: "Post",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Synopsis",
                table: "Post");
        }
    }
}
