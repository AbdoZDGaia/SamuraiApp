using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        private static SamuraiContext _ctx = new SamuraiContext();
        static void Main(string[] args)
        {
            //_ctx.Database.EnsureCreated();
            //GetSamurais("Before Add:");
            //AddMulti();
            //InsertVariousTypes();
            //AddSamurai();
            //GetSamurais("After Add:");
            //Console.Write("Press any key...");
            //Console.ReadKey();
            //QueryFilters();
            //UpdateSamurai();
            //UpdateSamurais();
            //InsertBattle();
            //QueryAndUpdateBattleDisconnected();
            //InsertSamuraiWithAQuote();
            //UpdateSamuraiWithQuote();
            //UpdateSamuraiWithQuote_Attach();
            //EagerLoadSamuraiWithQuote();
            //ProjectSamuraiWithQuotes();
            //ExplicitLoadQuotes();
            //FilteringWithRelatedData();
            //ModifyingRelatedDataWhenConnected();
            //ModifyingRelatedDataWhenNotConnected();
            //GetSamuraiWithBattles();
            //AddNewSamuraiWithHorse();
            //AddNewHorseToSamuraiUsingId();
            //AddNewHorseToSamuraiObject();
            //AddNewHorseToDisconnectedSamuraiObject();
            //GetHorseWithoutSamurai();
            //GetHorseWithSamurai();
            //QueryView();
            QueryUsingRawSQL();
        }

        private static void QueryUsingRawSQL()
        {
            var samurais = _ctx.Samurais.FromSqlRaw("SELECT * FROM Samurais").ToList();
        }

        private static void QueryView()
        {
            var battlesWithStats = _ctx.SamuraiBattleStats.ToList();
        }

        private static void GetHorseWithSamurai()
        {
            var horseWithSamurai = _ctx.Samurais.Include(s => s.Horse).FirstOrDefault(s => s.Horse.Id == 3);

            var horseWithSamurais = _ctx.Samurais.Where(s=>s.Horse != null).Select(s=>new {
                Samurai = s,
                Horse = s.Horse
            }).ToList();

        }

        private static void GetHorseWithoutSamurai()
        {
            var horseWithoutSamurai = _ctx.Set<Horse>().Find(2);
        }

        private static void AddNewHorseToDisconnectedSamuraiObject()
        {
            var samurai = _ctx.Samurais.Find(4);
            samurai.Horse = new Horse { Name = "YELOLOLEELE" };
            using (var _discCtx = new SamuraiContext())
            {
                _discCtx.Attach(samurai);
                _discCtx.SaveChanges();
            }
        }

        private static void AddNewHorseToSamuraiObject()
        {
            var samurai = _ctx.Samurais.Find(3);
            samurai.Horse = new Horse { Name = "YEEEHAW" };
            _ctx.SaveChanges();
        }

        private static void AddNewHorseToSamuraiUsingId()
        {
            var horse = new Horse { Name = "Horzey", SamuraiId = 2 };
            _ctx.Add(horse);
            _ctx.SaveChanges();
        }

        private static void AddNewSamuraiWithHorse()
        {
            var samurai = new Samurai { Name = "DGaia" };
            samurai.Horse = new Horse { Name = "Lightning Bolt" };
            _ctx.Add(samurai);
            _ctx.SaveChanges();
        }

        private static void GetSamuraiWithBattles()
        {
            var samuraiWithBattle = _ctx.Samurais
                .Include(s => s.SamuraiBattles).ThenInclude(sb => sb.Battle)
                .Where(s => s.SamuraiBattles.Count > 0).FirstOrDefault();

            var samuraiWithBattleCleaner = _ctx.Samurais.Select(s => new
            {
                SamuraiId = s.Id,
                Battle = s.SamuraiBattles.Select(sb => sb.Battle)
            });

        }

        private static void ModifyingRelatedDataWhenNotConnected()
        {
            var samurai = _ctx.Samurais.Include(s => s.Quotes).FirstOrDefault();
            var quote = samurai.Quotes[0];
            quote.Text = "loleeen";
            using (var _discCtx = new SamuraiContext())
            {
                _discCtx.Entry(quote).State = EntityState.Modified;
                _discCtx.SaveChanges();
            }
        }

        private static void ModifyingRelatedDataWhenConnected()
        {
            var samurai = _ctx.Samurais.Include(s => s.Quotes).FirstOrDefault();
            samurai.Quotes[0].Text = "lol";
            _ctx.SaveChanges();
        }

        private static void FilteringWithRelatedData()
        {
            var samurai = _ctx.Samurais.Where(s => s.Quotes.Any(q => q.Text.Length < 10)).ToList();
        }

        private static void ExplicitLoadQuotes()
        {
            var samurai = _ctx.Samurais.FirstOrDefault();
            _ctx.Entry(samurai).Collection(s => s.Quotes).Load();
            _ctx.Entry(samurai).Reference(s => s.Horse).Load();
        }

        private static void ProjectSamuraiWithQuotes()
        {
            var samurais = _ctx.Samurais.Select(s => new { s.Name, s.Quotes, ShortQuotes = s.Quotes.Where(q => q.Text.Length < 10) }).Take(10).ToList();
        }

        private static void EagerLoadSamuraiWithQuote()
        {
            var samurais = _ctx.Samurais.Include(s => s.Quotes).ToList();
        }

        private static void UpdateSamuraiWithQuote()
        {
            var samurai = _ctx.Samurais.FirstOrDefault();
            samurai.Quotes = new List<Quote>() { new Quote { Text = "hellow" } };
            using (var _discCtx = new SamuraiContext())
            {
                _discCtx.Samurais.Update(samurai);
                _discCtx.SaveChanges();
            }
        }

        private static void UpdateSamuraiWithQuote_Attach()
        {
            var samurai = _ctx.Samurais.FirstOrDefault();
            samurai.Quotes = new List<Quote>() { new Quote { Text = "hellow" } };
            using (var _discCtx = new SamuraiContext())
            {
                _discCtx.Samurais.Attach(samurai);
                _discCtx.SaveChanges();
            }
        }

        private static void InsertSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Clan = new Clan { ClanName = "The Kayles" },
                Horse = new Horse { Name = "HORSEY" },
                Name = "AbdoZ The Quoting Champ",
                Quotes = new List<Quote>()
                {
                    new Quote {Text="HEY YOOO!"}
                },

            };
            _ctx.Samurais.Add(samurai);
            _ctx.SaveChanges();
        }

        private static void QueryAndUpdateBattleDisconnected()
        {
            var battle = _ctx.Battles.AsNoTracking().FirstOrDefault();
            battle.EndDate = new DateTime(2200, 10, 1);
            using (var disconnectedCtx = new SamuraiContext())
            {
                disconnectedCtx.Battles.Update(battle);
                disconnectedCtx.SaveChanges();
            }
        }

        private static void InsertBattle()
        {
            var battle = new Battle
            {
                Name = "7arb embaba",
                StartDate = new DateTime(2120, 11, 30),
                EndDate = new DateTime(3000, 10, 20)
            };
            _ctx.Battles.Add(battle);
            _ctx.SaveChanges();
        }

        private static void UpdateSamurais()
        {
            var samurais = _ctx.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += " San");
            _ctx.SaveChanges();
        }

        private static void UpdateSamurai()
        {
            var samurai = _ctx.Samurais.FirstOrDefault();
            samurai.Name += " San";
            _ctx.SaveChanges();
        }

        private static void QueryFilters()
        {
            var name = "AbdoZ";
            //var samurais = _ctx.Samurais.OrderBy(s=>s.Name).LastOrDefault(s =>EF.Functions.Like(s.Name,"A%"));
            var samurais = _ctx.Samurais.Find(2);
            //var samurais = _ctx.Samurais.Where(s => s.Name == name).ToList();
        }

        private static void InsertVariousTypes()
        {
            var clan = new Clan { ClanName = "The Kayles" };
            var samurai = new Samurai { Name = "Kayle", Clan = clan };
            _ctx.AddRange(samurai, clan);
            _ctx.SaveChanges();
        }

        private static void AddMulti()
        {
            var samurai1 = new Samurai { Name = "Bodz" };
            var samurai2 = new Samurai { Name = "AbdoZ" };
            var samurai3 = new Samurai { Name = "AbdoZ1" };
            var samurai4 = new Samurai { Name = "AbdoZ2" };
            _ctx.Samurais.AddRange(samurai1, samurai2, samurai3, samurai4);
            _ctx.SaveChanges();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "Shimo" };
            _ctx.Samurais.Add(samurai);
            _ctx.SaveChanges();
        }

        private static void GetSamurais(string v)
        {
            var samurais = _ctx.Samurais.ToList();
            Console.WriteLine($"{v} Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }
    }
}
