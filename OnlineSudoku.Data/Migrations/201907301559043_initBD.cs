namespace OnlineSudoku.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initBD : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TopUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        CountMoves = c.Int(nullable: false),
                        Time = c.Time(nullable: false, precision: 7),
                        ConectionId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TopUsers");
        }
    }
}
