using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodOperator.Migrations
{
    /// <inheritdoc />
    public partial class AddIsToppingToIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTopping",
                table: "Ingredients",
                type: "bit",
                nullable: false,
                defaultValue: false);


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTopping",
                table: "Ingredients"
                );




        }
    }
}
