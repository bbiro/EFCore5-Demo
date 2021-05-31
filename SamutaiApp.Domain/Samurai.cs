using System.Collections.Generic;

namespace SamutaiApp.Domain
{
    public class Samurai
    {
        public int SamuraiId { get; set; }
        public string Name { get; set; }
        public List<Quote> Quotes { get; set; } = new List<Quote>();
        public List<Battle> Battles { get; set; } = new List<Battle>();
        public Horse Horse { get; set; }
    }
}
