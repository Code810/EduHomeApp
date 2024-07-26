using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHomeApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class addCourseLanguageNamePropUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CourseLanguages_Name",
                table: "CourseLanguages",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CourseLanguages_Name",
                table: "CourseLanguages");
        }
    }
}
