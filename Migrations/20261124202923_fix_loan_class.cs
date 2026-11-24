using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class fix_loan_class : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "BookCopies");

            migrationBuilder.CreateSequence<int>(
                name: "CopyNumber",
                startValue: 5000L);

            migrationBuilder.AddColumn<string>(
                name: "CopyCode",
                table: "Loans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CopyNumber",
                table: "BookCopies",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR CopyNumber");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "BookCopies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_BookCopies_CopyNumber",
                table: "BookCopies",
                column: "CopyNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookCopies_CopyNumber",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "CopyCode",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "CopyNumber",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "BookCopies");

            migrationBuilder.DropSequence(
                name: "CopyNumber");

            migrationBuilder.AddColumn<int>(
                name: "Barcode",
                table: "BookCopies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
