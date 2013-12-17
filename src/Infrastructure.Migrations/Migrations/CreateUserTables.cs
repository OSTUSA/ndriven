using FluentMigrator;
using System;

namespace Infrastructure.Migrations.Migrations
{
    [Migration(6)]
    public class CreateUserTables : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                  .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Email").AsString(255).NotNullable().Unique()
                  .WithColumn("Name").AsString(255).NotNullable()
                  .WithColumn("Password").AsString(255).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("User");
        }
    }
}
