using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetSitApp.Migrations
{
    /// <inheritdoc />
    public partial class FixedPetPictureRealtionship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetPictures_Pets_Id",
                table: "PetPictures");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PetPictures",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_PetPictures_PetId",
                table: "PetPictures",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetPictures_Pets_PetId",
                table: "PetPictures",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetPictures_Pets_PetId",
                table: "PetPictures");

            migrationBuilder.DropIndex(
                name: "IX_PetPictures_PetId",
                table: "PetPictures");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PetPictures",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_PetPictures_Pets_Id",
                table: "PetPictures",
                column: "Id",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
