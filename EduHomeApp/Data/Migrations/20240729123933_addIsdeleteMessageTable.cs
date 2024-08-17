using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHomeApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class addIsdeleteMessageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Messages");
        }
    }
}
