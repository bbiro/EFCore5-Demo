using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class SamuraiBattleStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE DEFINER=`pomelo`@`localhost` FUNCTION `EarliestBattleFoughtBySamurai`(samuraiId int) RETURNS char(30) CHARSET utf8mb4
                    DETERMINISTIC
                BEGIN
	                DECLARE ret char(30);
	                SELECT Name INTO ret
	                FROM Battles
	                WHERE Battles.BattleId IN (SELECT BattleId 
								                FROM BattleSamurai
								                WHERE SamuraiId = @samuraiId)
	                ORDER BY StartDate limit 1;
                RETURN ret;
                END");

            migrationBuilder.Sql(
                @"CREATE 
                    ALGORITHM = UNDEFINED 
                    DEFINER = `pomelo`@`localhost` 
                    SQL SECURITY DEFINER
                VIEW `samuraibattlesstats` AS
                    SELECT 
                        `samurais`.`Name` AS `Name`,
                        COUNT(`battlesamurai`.`BattleId`) AS `NumberOfBattles`,
                        EARLIESTBATTLEFOUGHTBYSAMURAI(MIN(`samurais`.`SamuraiId`)) AS `EarliestBattle`
                    FROM
                        (`battlesamurai`
                        JOIN `samurais` ON ((`battlesamurai`.`SamuraiId` = `samurais`.`SamuraiId`)))
                    GROUP BY `samurais`.`Name` , `battlesamurai`.`SamuraiId`");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW dbo.SamuraiBattlesStats");
            migrationBuilder.Sql("DROP FUNCTION dbo.EarliestBattleFoughtBySamurai");
        }
    }
}
