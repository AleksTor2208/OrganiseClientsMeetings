namespace OrganiseClientsMeetings.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyMeetingModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meetings", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Meetings", "Address");
        }
    }
}
