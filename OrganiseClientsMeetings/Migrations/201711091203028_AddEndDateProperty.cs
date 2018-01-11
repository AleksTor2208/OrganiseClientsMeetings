namespace OrganiseClientsMeetings.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEndDateProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meetings", "StartTime", c => c.String(nullable: false));
            AddColumn("dbo.Meetings", "EndTime", c => c.String(nullable: false));
            DropColumn("dbo.Meetings", "Time");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Meetings", "Time", c => c.String(nullable: false));
            DropColumn("dbo.Meetings", "EndTime");
            DropColumn("dbo.Meetings", "StartTime");
        }
    }
}
