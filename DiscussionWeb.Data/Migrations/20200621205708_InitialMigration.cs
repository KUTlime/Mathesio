using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscussionWeb.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    NickName = table.Column<string>(maxLength: 50, nullable: false),
                    RegistrationEmail = table.Column<string>(maxLength: 100, nullable: false),
                    Registered = table.Column<DateTime>(nullable: false),
                    Permission = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Threads",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: false),
                    Posted = table.Column<DateTime>(nullable: false),
                    LastEdited = table.Column<DateTime>(nullable: false),
                    NumberOfEdits = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Threads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Threads_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: true),
                    ThreadId = table.Column<Guid>(nullable: true),
                    Message = table.Column<string>(maxLength: 2000, nullable: false),
                    Posted = table.Column<DateTime>(nullable: false),
                    LastEdited = table.Column<DateTime>(nullable: false),
                    NumberOfEdits = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Threads_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "Threads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "FirstName", "LastName", "NickName", "Permission", "Registered", "RegistrationEmail" },
                values: new object[] { new Guid("a28888e9-2ba9-473a-a40f-e38cb54f9b35"), "Tony", "Stark", "Ironman", (byte)1, new DateTime(2020, 6, 21, 20, 57, 8, 192, DateTimeKind.Utc).AddTicks(6349), "iron@man.cz" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "FirstName", "LastName", "NickName", "Permission", "Registered", "RegistrationEmail" },
                values: new object[] { new Guid("b28888e9-2ba9-473a-a40f-e38cb54f9b35"), "Bruce", "Wayne", "Batman", (byte)1, new DateTime(2020, 6, 21, 20, 57, 8, 192, DateTimeKind.Utc).AddTicks(6802), "bat@man.cz" });

            migrationBuilder.InsertData(
                table: "Threads",
                columns: new[] { "Id", "AuthorId", "Description", "LastEdited", "NumberOfEdits", "Posted", "Title" },
                values: new object[] { new Guid("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b"), new Guid("a28888e9-2ba9-473a-a40f-e38cb54f9b35"), "A general discussion about life universe and everything", new DateTime(2020, 6, 21, 20, 57, 8, 201, DateTimeKind.Utc).AddTicks(4335), 0L, new DateTime(2020, 6, 21, 20, 57, 8, 201, DateTimeKind.Utc).AddTicks(3929), "General Discussion" });

            migrationBuilder.InsertData(
                table: "Threads",
                columns: new[] { "Id", "AuthorId", "Description", "LastEdited", "NumberOfEdits", "Posted", "Title" },
                values: new object[] { new Guid("d173e20d-159e-4127-9ce9-b0ac2564ad97"), new Guid("b28888e9-2ba9-473a-a40f-e38cb54f9b35"), "A general discussion about C#.", new DateTime(2020, 6, 21, 20, 57, 8, 201, DateTimeKind.Utc).AddTicks(5162), 0L, new DateTime(2020, 6, 21, 20, 57, 8, 201, DateTimeKind.Utc).AddTicks(5146), "Csharp Discussion" });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "AuthorId", "LastEdited", "Message", "NumberOfEdits", "Posted", "ThreadId" },
                values: new object[] { new Guid("28c1db41-f104-46e6-8943-d31c0291e0e3"), null, new DateTime(2020, 6, 21, 20, 57, 8, 202, DateTimeKind.Utc).AddTicks(2457), "Hello world!", 0L, new DateTime(2020, 6, 21, 20, 57, 8, 202, DateTimeKind.Utc).AddTicks(1815), new Guid("5b1c2b4d-48c7-402a-80c3-cc796ad49c6b") });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "AuthorId", "LastEdited", "Message", "NumberOfEdits", "Posted", "ThreadId" },
                values: new object[] { new Guid("d94a64c2-2e8f-4162-9976-0ffe03d30767"), null, new DateTime(2020, 6, 21, 20, 57, 8, 202, DateTimeKind.Utc).AddTicks(3171), "Hello csharp!", 0L, new DateTime(2020, 6, 21, 20, 57, 8, 202, DateTimeKind.Utc).AddTicks(3158), new Guid("d173e20d-159e-4127-9ce9-b0ac2564ad97") });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ThreadId",
                table: "Posts",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_AuthorId",
                table: "Threads",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Threads");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
