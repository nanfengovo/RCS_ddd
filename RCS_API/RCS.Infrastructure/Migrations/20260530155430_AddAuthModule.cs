using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RCS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sys_permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sys_roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sys_users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sys_role_permissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_role_permissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_sys_role_permissions_sys_permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "sys_permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sys_role_permissions_sys_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "sys_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sys_user_roles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_user_roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_sys_user_roles_sys_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "sys_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sys_user_roles_sys_users_UserId",
                        column: x => x.UserId,
                        principalTable: "sys_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sys_permissions_Code",
                table: "sys_permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sys_role_permissions_PermissionId",
                table: "sys_role_permissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_sys_roles_Name",
                table: "sys_roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sys_user_roles_RoleId",
                table: "sys_user_roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_sys_users_Username",
                table: "sys_users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sys_role_permissions");

            migrationBuilder.DropTable(
                name: "sys_user_roles");

            migrationBuilder.DropTable(
                name: "sys_permissions");

            migrationBuilder.DropTable(
                name: "sys_roles");

            migrationBuilder.DropTable(
                name: "sys_users");
        }
    }
}
