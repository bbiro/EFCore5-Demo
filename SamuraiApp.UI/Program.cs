using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamutaiApp.Domain;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new();
        private static SamuraiContextNoTracking _contextNT = new();

        private static void Main(string[] args)
        {
            //GetSamurais("Before Add:");
            //AddSamurai();
            //AddSamuraisByName("Shimada", "OkaMoto", "Kikuchio", "Hayashida");
            //AddVariousTypes();
            //GetSamurais("After Add:");
            //RetrieveAndUpdateSamurai();
            //RetreiveAndUpdateMultipleSamurais();
            //MultipleDatabaseOperation();
            //InsertNewSamuraiWithAQuote();
            //InsertNewSamuraiWithManyQuotes();
            //AddQuoteToExistingSamuraiWhileTracked();
            //Simpler_AddQuoteToExistingSamuraiNotTracked(1);
            //EagerLoadSamuraiWithQuotes();
            //ProjectSamuraisWithQuotes();
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        //private static void AddSamurai()
        //{
        //    var samurai = new Samurai { Name = "Sampson" };
        //    _context.Samurais.Add(samurai);
        //    _context.SaveChanges();
        //}

        private static void AddSamuraisByName(params string[] names)
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            _context.SaveChanges();
        }

        private static void AddVariousTypes()
        {
            _context.AddRange(new Samurai { Name = "Shimada2" },
                              new Samurai { Name = "Okamoto2" },
                              new Battle { Name = "Battle of Anegawa" },
                              new Battle { Name = "Battle of Nagashino" });
            _context.SaveChanges();
        }

        private static void QueryFilters()
        {
            //var samurais = _context.Samurais.Where(s => s.Name == "Sampson").ToList();
            var samurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name, "%J")).ToList();
        }

        private static void QueryAggregate()
        {
            //var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Sampson");
            var samurai = _context.Samurais.Find(2);
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Sampson");
            samurai.Name += "San";
            _context.SaveChanges();
        }

        private static void RetreiveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += " San");
            _context.SaveChanges();
        }

        private static void MultipleDatabaseOperation()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += " San";
            _context.Samurais.Add(new Samurai { Name = "Shino" });
            _context.SaveChanges();
        }

        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais
                .TagWith("ConsoleApp.Program.GetSamurai method")
                .ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void QueryAndUpdateBattles_Disconnected()
        {
            List<Battle> disconnectedBatles;
            using (var context1 = new SamuraiContext())
            {
                disconnectedBatles = _context.Battles.ToList();
            }//context1 is disconnected;
            disconnectedBatles.ForEach(b =>
              {
                  b.StartDate = new DateTime(1570, 01, 01);
                  b.EndDate = new DateTime(1570, 12, 1);
              });

            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconnectedBatles);
                context2.SaveChanges();
            }
        }

        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "I've come to save you"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai
            {
                Name = "Kyuzo",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "Watch out for may sharp sword!"},
                    new Quote { Text = "I told you to watch out for the sharp sword! Oh well!"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I've saved you!"
            });
            _context.SaveChanges();
        }
        private static void AddQuoteToExistingSamuraiNotTracked(int SamuraiId)
        {
            var samurai = _context.Samurais.Find(SamuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that I haved you, will you feed me dinner?"
            });

            using (var newContext = new SamuraiContext())
            {
                //newContext.Samurais.Update(samurai);
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }
        private static void Simpler_AddQuoteToExistingSamuraiNotTracked(int SamuraiId)
        {
            var quote = new Quote { Text = "Thanks for dinner!", SamuraiId = SamuraiId };
            using var newContext = new SamuraiContext();
            newContext.Quotes.Add(quote);
            newContext.SaveChanges();
        }
        private static void EagerLoadSamuraiWithQuotes()
        {
            //var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
            //var samuraiWithQuotes = _context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToList();
            var filteredWithInclude = _context.Samurais.Include(s => s.Quotes.Where(q => q.Text.Contains("Thanks"))).ToList();
            var filterPrimaryEntityWithInclude = _context.Samurais.Where(s => s.Name.Contains("Sampson")).Include(s => s.Quotes).FirstOrDefault();
        }
        private static void ProjectSamuraisWithQuotes()
        {
            var somePropsWithQuotes = _context.Samurais.Select(s => new { s.SamuraiId, s.Name, NumberOfQuotes = s.Quotes.Count() }).ToList();
            var somePropsWithQuotes2 = _context.Samurais.Select(s => new { s.SamuraiId, s.Name, HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy")) }).ToList();
        }
        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new { s.SamuraiId, s.Name }).ToList();
            var idAndNames = _context.Samurais.Select(s => new IdAndName(s.SamuraiId, s.Name)).ToList();
        }

        public struct IdAndName
        {
            public IdAndName(int Id, string name)
            {
                this.Id = Id;
                this.Name = name;
            }

            public int Id;
            public string Name;
        }

        private static void ExplicitLoadQuotes()
        {
            // make sure there's a horse in the DB, then clear the context's change tracker
            _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr. Ed" });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            var samurai = _context.Samurais.Find(1);
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }
    }
}
