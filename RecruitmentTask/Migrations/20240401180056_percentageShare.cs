using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentTask.Migrations
{
    /// <inheritdoc />
    public partial class percentageShare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "percentageShare",
                table: "Tags",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "percentageShare",
                table: "Tags");
        }
    }
}
