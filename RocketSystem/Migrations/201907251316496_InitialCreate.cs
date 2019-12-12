namespace RocketSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        userId = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        membershipNo = c.String(),
                        dateOfBirth = c.DateTime(nullable: false),
                        telephoneNo = c.String(),
                        mobileNo = c.String(),
                        postalCode = c.String(),
                        katakanaName = c.String(),
                        nickName = c.String(),
                        gender = c.String(),
                        addressOne = c.String(),
                        addressTwo = c.String(),
                        addressThree = c.String(),
                        faxNo = c.String(),
                        webEmail = c.String(),
                        mobileEmail = c.String(),
                        accountName = c.String(),
                        accountNameKatakana = c.String(),
                        bankNameKatakana = c.String(),
                        transferDestinationBank = c.String(),
                        branchNameKatakana = c.String(),
                        transferDestinationBranchCode = c.String(),
                        accountClassification = c.String(),
                        transferAccountNumber = c.String(),
                        userName = c.String(),
                        password = c.String(),
                    })
                .PrimaryKey(t => t.userId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
        }
    }
}
