namespace OrganiseClientsMeetings.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageIntoMeetings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meetings", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Meetings", "Image");
        }
    }
}
