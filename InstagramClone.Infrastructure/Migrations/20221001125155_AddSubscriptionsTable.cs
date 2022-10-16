using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstagramClone.Infrastructure.Migrations
{
    public partial class AddSubscriptionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriberId = table.Column<int>(type: "int", nullable: false),
                    PublisherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => new { x.SubscriberId, x.PublisherId });
                    table.ForeignKey(
                        name: "FK_Subscriptions_UserProfiles_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subscriptions_UserProfiles_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PublisherId",
                table: "Subscriptions",
                column: "PublisherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");
        }
    }
}
