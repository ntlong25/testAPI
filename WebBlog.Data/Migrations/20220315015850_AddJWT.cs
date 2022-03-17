using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebBlog.Data.Migrations
{
    public partial class AddJWT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", maxLength: 20, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    parentId = table.Column<long>(type: "bigint", maxLength: 20, nullable: true),
                    title = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    metaTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_parentId",
                        column: x => x.parentId,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    roleStatus = table.Column<int>(type: "int", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", maxLength: 20, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    metaTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    content = table.Column<string>(type: "TEXT", nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", maxLength: 20, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    middleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    lastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    phoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    passwordHash = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    registeredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    intro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    profile = table.Column<string>(type: "TEXT", nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_Role_roleId",
                        column: x => x.roleId,
                        principalTable: "Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", maxLength: 20, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    authorId = table.Column<long>(type: "bigint", nullable: false),
                    parentId = table.Column<long>(type: "bigint", maxLength: 20, nullable: true),
                    title = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    metaTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    published = table.Column<byte>(type: "TINYINT", maxLength: 1, nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    publishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Posts_Posts_parentId",
                        column: x => x.parentId,
                        principalTable: "Posts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Users_authorId",
                        column: x => x.authorId,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonRevoked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostCategories",
                columns: table => new
                {
                    postId = table.Column<long>(type: "bigint", maxLength: 20, nullable: false),
                    categoryId = table.Column<long>(type: "bigint", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategories", x => new { x.postId, x.categoryId });
                    table.ForeignKey(
                        name: "FK_PostCategories_Categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostCategories_Posts_postId",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostComments",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", maxLength: 20, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postId = table.Column<long>(type: "bigint", maxLength: 20, nullable: false),
                    parentId = table.Column<long>(type: "bigint", maxLength: 20, nullable: true),
                    userId = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    published = table.Column<byte>(type: "TINYINT", maxLength: 1, nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    publishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    content = table.Column<string>(type: "TEXT", nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PostComments_PostComments_parentId",
                        column: x => x.parentId,
                        principalTable: "PostComments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostComments_Posts_postId",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostComments_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostMetas",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", maxLength: 20, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postId = table.Column<long>(type: "bigint", maxLength: 20, nullable: false),
                    key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    content = table.Column<string>(type: "TEXT", nullable: true),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostMetas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PostMetas_Posts_postId",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTags",
                columns: table => new
                {
                    postId = table.Column<long>(type: "bigint", maxLength: 20, nullable: false),
                    tagId = table.Column<long>(type: "bigint", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTags", x => new { x.postId, x.tagId });
                    table.ForeignKey(
                        name: "FK_PostTags_Posts_postId",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTags_Tags_tagId",
                        column: x => x.tagId,
                        principalTable: "Tags",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_parentId",
                table: "Categories",
                column: "parentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCategories_categoryId",
                table: "PostCategories",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_parentId",
                table: "PostComments",
                column: "parentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_postId",
                table: "PostComments",
                column: "postId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_userId",
                table: "PostComments",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_PostMetas_postId",
                table: "PostMetas",
                column: "postId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_authorId",
                table: "Posts",
                column: "authorId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_parentId",
                table: "Posts",
                column: "parentId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_slug",
                table: "Posts",
                column: "slug",
                unique: true,
                filter: "[slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_tagId",
                table: "PostTags",
                column: "tagId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserID",
                table: "RefreshToken",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_phoneNumber",
                table: "Users",
                column: "phoneNumber",
                unique: true,
                filter: "[phoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_roleId",
                table: "Users",
                column: "roleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostCategories");

            migrationBuilder.DropTable(
                name: "PostComments");

            migrationBuilder.DropTable(
                name: "PostMetas");

            migrationBuilder.DropTable(
                name: "PostTags");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
