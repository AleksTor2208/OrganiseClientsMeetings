namespace OrganiseClientsMeetings.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataAnnotationsMig1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "Name", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Meetings", "Date", c => c.String(nullable: false));
            AlterColumn("dbo.Meetings", "Time", c => c.String(nullable: false));
            AlterColumn("dbo.Meetings", "Payment", c => c.String(nullable: false));
            AlterColumn("dbo.Meetings", "Address", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Meetings", "Comment", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Meetings", "Comment", c => c.String());
            AlterColumn("dbo.Meetings", "Address", c => c.String());
            AlterColumn("dbo.Meetings", "Payment", c => c.String());
            AlterColumn("dbo.Meetings", "Time", c => c.String());
            AlterColumn("dbo.Meetings", "Date", c => c.String());
            AlterColumn("dbo.Clients", "Name", c => c.String());
        }
    }
}
