namespace OrganiseClientsMeetings.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateTime = c.String(nullable: true),
                        Payment = c.String(),
                        isRemote = c.Boolean(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id);            
        }
        
        public override void Down()
        {
            DropTable("dbo.Clients");
        }
    }
}
