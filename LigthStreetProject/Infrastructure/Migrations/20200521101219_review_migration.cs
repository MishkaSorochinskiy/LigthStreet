using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class review_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    State = table.Column<string>(nullable: false),
                    CreatedById = table.Column<int>(nullable: true),
                    PointId = table.Column<int>(nullable: false),
                    ApplyOnId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_ApplyOnId",
                        column: x => x.ApplyOnId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Points_PointId",
                        column: x => x.PointId,
                        principalTable: "Points",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ApplyOnId",
                table: "Reviews",
                column: "ApplyOnId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CreatedById",
                table: "Reviews",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PointId",
                table: "Reviews",
                column: "PointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
