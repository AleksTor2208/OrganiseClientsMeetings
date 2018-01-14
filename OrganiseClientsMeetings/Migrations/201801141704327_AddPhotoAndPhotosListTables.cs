namespace OrganiseClientsMeetings.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhotoAndPhotosListTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Photos1",
                c => new
                    {
                        PhotoId = c.Int(nullable: false, identity: true),
                        Base64 = c.String(),
                    })
                .PrimaryKey(t => t.PhotoId);
            
            CreateTable(
                "dbo.PhotosLists",
                c => new
                    {
                        PhotosListId = c.Int(nullable: false, identity: true),
                        MeetingId = c.Int(nullable: false),
                        PhotoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PhotosListId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PhotosLists");
            DropTable("dbo.Photos1");
        }
    }
}
