using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MittClick.Migrations
{
    /// <inheritdoc />
    public partial class bilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectLeader = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrivateProfile = table.Column<bool>(type: "bit", nullable: false),
                    Information = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Resume = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount" },
                values: new object[,]
                {
            { "user1", "user1", "user1", "user1@example.com", "user1@example.com", false, "AQAAAAEAACcQAAAAEB8LrT+O9kFkMZHZQqA6nGPKxJ8/HUW1eD+0quxKQvPpN/fQUCwWgKHZqeJ9bAYMFA==", "67A64458-C6CD-4E53-BA97-2E67A3F15353", "C8DDA27E-3C1B-4F93-BE41-731EF78A663F", null, false, false, null, false, 0 },
            { "user2", "user2", "user2", "user2@example.com", "user2@example.com", false, "AQAAAAEAACcQAAAAEB8LrT+O9kFkMZHZQqA6nGPKxJ8/HUW1eD+0quxKQvPpN/fQUCwWgKHZqeJ9bAYMFA==", "C8DDA27E-3C1B-4F93-BE41-731EF78A663F", "9FA7C645-6F58-42F0-8C2E-7861A9943B9E", null, false, false, null, false, 0 },
            { "user3", "user3", "user3", "user3@example.com", "user3@example.com", false, "AQAAAAEAACcQAAAAEB8LrT+O9kFkMZHZQqA6nGPKxJ8/HUW1eD+0quxKQvPpN/fQUCwWgKHZqeJ9bAYMFA==", "C8DDA27E-3C1B-4F93-BE41-731EF78A663F", "9FA7C645-6F58-42F0-8C2E-7861A9943B9E", null, false, false, null, false, 0 },
            { "user4", "user4", "user4", "user4@example.com", "user4@example.com", false, "AQAAAAEAACcQAAAAEB8LrT+O9kFkMZHZQqA6nGPKxJ8/HUW1eD+0quxKQvPpN/fQUCwWgKHZqeJ9bAYMFA==", "C8DDA27E-3C1B-4F93-BE41-731EF78A663F", "9FA7C645-6F58-42F0-8C2E-7861A9943B9E", null, false, false, null, false, 0 },
            { "user5", "user5", "user5", "user5@example.com", "user5@example.com", false, "AQAAAAEAACcQAAAAEB8LrT+O9kFkMZHZQqA6nGPKxJ8/HUW1eD+0quxKQvPpN/fQUCwWgKHZqeJ9bAYMFA==", "C8DDA27E-3C1B-4F93-BE41-731EF78A663F", "9FA7C645-6F58-42F0-8C2E-7861A9943B9E", null, false, false, null, false, 0 },
            { "user6", "user6", "user6", "user6@example.com", "user6@example.com", false, "AQAAAAEAACcQAAAAEB8LrT+O9kFkMZHZQqA6nGPKxJ8/HUW1eD+0quxKQvPpN/fQUCwWgKHZqeJ9bAYMFA==", "C8DDA27E-3C1B-4F93-BE41-731EF78A663F", "9FA7C645-6F58-42F0-8C2E-7861A9943B9E", null, false, false, null, false, 0 },
            { "user7", "user7", "user7", "user7@example.com", "user7@example.com", false, "AQAAAAEAACcQAAAAEB8LrT+O9kFkMZHZQqA6nGPKxJ8/HUW1eD+0quxKQvPpN/fQUCwWgKHZqeJ9bAYMFA==", "C8DDA27E-3C1B-4F93-BE41-731EF78A663F", "9FA7C645-6F58-42F0-8C2E-7861A9943B9E", null, false, false, null, false, 0 },
            { "user8", "user8", "user8", "user8@example.com", "user8@example.com", false, "AQAAAAEAACcQAAAAEB8LrT+O9kFkMZHZQqA6nGPKxJ8/HUW1eD+0quxKQvPpN/fQUCwWgKHZqeJ9bAYMFA==", "C8DDA27E-3C1B-4F93-BE41-731EF78A663F", "9FA7C645-6F58-42F0-8C2E-7861A9943B9E", null, false, false, null, false, 0 },
            { "user9", "user9", "user9", "user9@example.com", "user9@example.com", false, "AQAAAAEAACcQAAAAEB8LrT+O9kFkMZHZQqA6nGPKxJ8/HUW1eD+0quxKQvPpN/fQUCwWgKHZqeJ9bAYMFA==", "C8DDA27E-3C1B-4F93-BE41-731EF78A663F", "9FA7C645-6F58-42F0-8C2E-7861A9943B9E", null, false, false, null, false, 0 },
            { "user10", "user10", "user10", "user10@example.com", "user10@example.com", false, "AQAAAAEAACcQAAAAEB8LrT+O9kFkMZHZQqA6nGPKxJ8/HUW1eD+0quxKQvPpN/fQUCwWgKHZqeJ9bAYMFA==", "C8DDA27E-3C1B-4F93-BE41-731EF78A663F", "9FA7C645-6F58-42F0-8C2E-7861A9943B9E", null, false, false, null, false, 0 },
            { "user11", "user11", "user11", "user11@example.com", "user11@example.com", false, "AQAAAAEAACcQAAAAEB8LrT+O9kFkMZHZQqA6nGPKxJ8/HUW1eD+0quxKQvPpN/fQUCwWgKHZqeJ9bAYMFA==", "C8DDA27E-3C1B-4F93-BE41-731EF78A663F", "9FA7C645-6F58-42F0-8C2E-7861A9943B9E", null, false, false, null, false, 0 },
            { "user12", "user12", "user12", "user12@example.com", "user12@example.com", false, "AQAAAAEAACcQAAAAEB8LrT+O9kFkMZHZQqA6nGPKxJ8/HUW1eD+0quxKQvPpN/fQUCwWgKHZqeJ9bAYMFA==", "C8DDA27E-3C1B-4F93-BE41-731EF78A663F", "9FA7C645-6F58-42F0-8C2E-7861A9943B9E", null, false, false, null, false, 0 },
            
            // ... (Add more users as needed)
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "ProfileId", "FirstName", "LastName", "PrivateProfile", "Information", "ProfileImage", "Resume", "UserId", "UserName" },
                values: new object[,]
                {
            { 1, "John", "Doe", false, null, null, null, "user1", "user1" },
            { 2, "Jane", "Smith", false, null, null, null, "user2", "user2" },
            { 3, "Alice", "Johnson", false, null, null, null, "user3", "user3" },
            { 4, "Bob", "Williams", false, null, null, null, "user4", "user4" },
            { 5, "Eva", "Anderson", false, null, null, null, "user5", "user5" },
            { 6, "Chris", "Miller", false, null, null, null, "user6", "user6" },
            { 7, "Sophia", "Miller", false, null, null, null, "user7", "user7" },
            { 8, "Jackson", "Wilson", false, null, null, null, "user8", "user8" },
            { 9, "Olivia", "Davis", false, null, null, null, "user9", "user9" },
            { 10, "Samuel", "Taylor", false, null, null, null, "user10", "user10" },
            { 11, "Emma", "Jones", false, null, null, null, "user11", "user11" },
            { 12, "William", "Brown", false, null, null, null, "user12", "user12" },
            
            // ... (Add more profiles as needed)
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
