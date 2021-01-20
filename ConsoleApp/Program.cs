using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
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
            QueryFilters();
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
