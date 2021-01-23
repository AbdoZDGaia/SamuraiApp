using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class SamuraiBattleStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Create FUNCTION dbo.EarliestBattleFoughtBySamurai(@SamuraiId int) 
                                   RETURNS NVARCHAR(MAX) AS
                                   BEGIN
	                               DECLARE @RET NVARCHAR(MAX)
	                               SELECT TOP 1 @RET=Name From Battles
	                               WHERE
	                               Battles.Id in (SELECT Id from SamuraiBattle WHERE SamuraiBattle.SamuraiId = @SamuraiId)
	                               ORDER BY StartDate
	                               RETURN @RET
                                   END");

            migrationBuilder.Sql(@"CREATE VIEW dbo.SamuraiBattleStats AS
                                   SELECT dbo.Samurais.Name,
                                   COUNT(dbo.SamuraiBattle.BattleId) AS NumberOfBattles,
                                   dbo.EarliestBattleFoughtBySamurai(MIN(dbo.Samurais.Id)) EarliestBattle
                                   FROM dbo.SamuraiBattle INNER JOIN dbo.Samurais
                                   ON dbo.SamuraiBattle.SamuraiId = dbo.Samurais.Id
                                   GROUP BY dbo.Samurais.Name,dbo.SamuraiBattle.BattleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW dbo.SamuraiBattleStats");
            migrationBuilder.Sql(@"DROP FUNCTION dbo.EarliestBattleFoughtBySamurai");
        }
    }
}
