namespace PromotionalWebsite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_conteact_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactDMs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FirstName = c.String(),
                        Email = c.String(),
                        Message = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ContactDMs");
        }
    }
}
