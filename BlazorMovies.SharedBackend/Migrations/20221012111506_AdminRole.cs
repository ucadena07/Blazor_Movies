using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorMovies.SharedBackend.Migrations
{
    public partial class AdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"insert into AspNetRoles (Id, [Name], NormalizedName)
                                    values('2c0660b6-2225-455e-af0d-551361453be3','Admin','Admin')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
