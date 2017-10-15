namespace OrganiseClientsMeetings.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDateTimeOnMeeting : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Meetings", "DateTime");
            AddColumn("dbo.Meetings", "DateTime", c => c.String());
        }
        
        public override void Down()
        {
            
        }
    }
}
