using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ecommerce_web_api.Migrations
{
public partial class updateFilesuploadwithguid : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Add a new column of type uuid
        migrationBuilder.AddColumn<Guid>(
            name: "NewId",
            table: "FilesUploads",
            type: "uuid",
            nullable: false,
            defaultValueSql: "gen_random_uuid()"); // Automatically generate UUIDs for existing rows

        // Optionally, if you want to keep the old Id, you can add a default value to NewId
        // migrationBuilder.Sql("UPDATE \"FilesUploads\" SET \"NewId\" = gen_random_uuid();");

        // Drop the original Id column (optional, if no longer needed)
        migrationBuilder.DropColumn(
            name: "Id",
            table: "FilesUploads");

        // Rename the new column to Id
        migrationBuilder.RenameColumn(
            name: "NewId",
            table: "FilesUploads",
            newName: "Id");

        // Recreate sequence (if needed, depending on how you handle UUIDs)
        // migrationBuilder.Sql("CREATE SEQUENCE \"FilesUploads_Id_seq\";");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Revert the changes if rolling back
        migrationBuilder.AddColumn<int>(
            name: "Id",
            table: "FilesUploads",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        // Remove the uuid column
        migrationBuilder.DropColumn(
            name: "Id",
            table: "FilesUploads");

        // Rename column back to original
        migrationBuilder.RenameColumn(
            name: "NewId",
            table: "FilesUploads",
            newName: "Id");

        // Drop sequence (if needed)
        // migrationBuilder.Sql("DROP SEQUENCE IF EXISTS \"FilesUploads_Id_seq\";");
    }
}




}
