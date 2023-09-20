using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetSitApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedPkToPet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetPictures_Pets_PetId",
                table: "PetPictures");

            migrationBuilder.DropIndex(
                name: "IX_PetPictures_PetId",
                table: "PetPictures");

            migrationBuilder.AlterColumn<string>(
                name: "MedicalInformation",
                table: "Pets",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PetPictures",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                table: "Owners",
                type: "varbinary(max)",
                nullable: false,
                defaultValueSql: "(0x)",
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MedicalInformation",
                table: "Pets",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PetPictures",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                table: "Owners",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldDefaultValueSql: "(0x)");

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
    }
}
