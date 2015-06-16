namespace DrinkDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialmodels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drinks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 128),
                        Instructions = c.String(),
                        Glass = c.String(maxLength: 128),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.DrinkIngredients",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IngredientID = c.Int(nullable: false),
                        DrinkID = c.Int(nullable: false),
                        Amount = c.String(),
                        Brand = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Ingredients", t=>t.IngredientID, cascadeDelete:true)
                .ForeignKey("dbo.Drinks", t => t.DrinkID, cascadeDelete: true)
                .Index(t => t.DrinkID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DrinkIngredients", "DrinkID", "dbo.Drinks");
            DropIndex("dbo.DrinkIngredients", new[] { "DrinkID" });
            DropTable("dbo.DrinkIngredients");
            DropTable("dbo.Ingredients");
            DropTable("dbo.Drinks");
        }
    }
}
