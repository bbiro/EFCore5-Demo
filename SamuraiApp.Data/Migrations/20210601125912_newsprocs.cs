using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class newsprocs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
             @"CREATE DEFINER=`pomelo`@`localhost` PROCEDURE `SamuraisWhoSaidAWord`(text varchar(20))
                BEGIN
	                SELECT SamuraiId, Name
		                FROM Samurais INNER JOIN Quotes ON s.SamuraiId = Quotes.SamuraiId
		                WHERE Quotes.Text LIKE ('%' + text + '%');
                END");

            migrationBuilder.Sql(
              @"CREATE DEFINER=`pomelo`@`localhost` PROCEDURE `DeleteQuotesForSamurai`(samuraiId int)
                    BEGIN
	                    DELETE FROM Quotes
                                    WHERE Quotes.SamuraiId = samuraiId;
                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE `samuraiapp`.`SamuraisWhoSaidAWord`");
            migrationBuilder.Sql("DROP PROCEDURE `samuraiapp`.`DeleteQuotesForSamurai`");
        }
    }
}
