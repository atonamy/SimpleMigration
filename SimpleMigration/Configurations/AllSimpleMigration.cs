namespace SimpleMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllSimpleMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.simple_table",
                c => new
                    {
                        DataId = c.Int(nullable: false),
                        Content = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.DataId);
            
        }
        
        public override void Down()
        {
            DropTable("public.simple_table");
        }
    }
}
