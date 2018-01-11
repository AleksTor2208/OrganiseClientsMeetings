namespace OrganiseClientsMeetings.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewmigrationAfterCreateEditView : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MeetingViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.String(),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        Payment = c.String(),
                        Address = c.String(),
                        Comment = c.String(),
                        PhotoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);            
        }
        
        public override void Down()
        {
            DropTable("dbo.MeetingViewModels");
        }
    }
}
