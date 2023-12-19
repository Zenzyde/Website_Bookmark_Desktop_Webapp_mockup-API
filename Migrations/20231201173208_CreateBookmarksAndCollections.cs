using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Website_Bookmark_Desktop_App_API.Migrations
{
    /// <inheritdoc />
    public partial class CreateBookmarksAndCollections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookmarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BookmarkURL = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    URLTitle = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    CollectionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CollectionTitle = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    OwningCollectionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookmarks");

            migrationBuilder.DropTable(
                name: "Collections");
        }
    }
}
