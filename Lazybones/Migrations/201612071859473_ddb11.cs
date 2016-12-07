namespace Lazybones.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ddb11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "First_Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Gig_Poster", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Go_Getter", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Last_Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Mobile_Phone", c => c.String());
            AddColumn("dbo.AspNetUsers", "Preferred_Contact_Method", c => c.String());
            AddColumn("dbo.AspNetUsers", "State", c => c.String());
            AddColumn("dbo.AspNetUsers", "Zip", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Zip");
            DropColumn("dbo.AspNetUsers", "State");
            DropColumn("dbo.AspNetUsers", "Preferred_Contact_Method");
            DropColumn("dbo.AspNetUsers", "Mobile_Phone");
            DropColumn("dbo.AspNetUsers", "Last_Name");
            DropColumn("dbo.AspNetUsers", "Go_Getter");
            DropColumn("dbo.AspNetUsers", "Gig_Poster");
            DropColumn("dbo.AspNetUsers", "First_Name");
        }
    }
}
