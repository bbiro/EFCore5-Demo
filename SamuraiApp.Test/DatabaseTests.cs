using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
using SamutaiApp.Domain;
using System.Diagnostics;

namespace SamuraiApp.Test
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void CanInsertSamuraiIntoDatabase()
        {
            using (var context = new SamuraiContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var samurai = new Samurai();
                context.Samurais.Add(samurai);
                Debug.WriteLine($"Before save: {samurai.SamuraiId}");
                context.SaveChanges();
                Debug.WriteLine($"After save: {samurai.SamuraiId}");

                Assert.AreNotEqual(0, samurai.SamuraiId);
            }
        }
    }
}
