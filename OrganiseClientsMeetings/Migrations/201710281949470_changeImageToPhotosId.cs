namespace OrganiseClientsMeetings.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeImageToPhotosId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meetings", "PhotosId", c => c.Int(nullable: false));
            DropColumn("dbo.Meetings", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Meetings", "Image", c => c.String());
            DropColumn("dbo.Meetings", "PhotosId");
        }
    }
}
