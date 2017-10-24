namespace OrganiseClientsMeetings.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimeToMeetings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meetings", "Date", c => c.String());
            AddColumn("dbo.Meetings", "Time", c => c.String());
            DropColumn("dbo.Meetings", "DateTime");
            DropColumn("dbo.Meetings", "isRemote");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Meetings", "isRemote", c => c.String());
            AddColumn("dbo.Meetings", "DateTime", c => c.String());
            DropColumn("dbo.Meetings", "Time");
            DropColumn("dbo.Meetings", "Date");
        }
    }
}
