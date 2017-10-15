namespace OrganiseClientsMeetings.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientsAndRenameMeetings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Meetings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: true),
                        Payment = c.String(),
                        isRemote = c.String(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Clients", "DateTime");
            DropColumn("dbo.Clients", "Payment");
            DropColumn("dbo.Clients", "isRemote");
            DropColumn("dbo.Clients", "Comment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "Comment", c => c.String());
            AddColumn("dbo.Clients", "isRemote", c => c.Boolean(nullable: false));
            AddColumn("dbo.Clients", "Payment", c => c.String());
            AddColumn("dbo.Clients", "DateTime", c => c.DateTime(nullable: false));
            DropTable("dbo.Meetings");
        }
    }
}
