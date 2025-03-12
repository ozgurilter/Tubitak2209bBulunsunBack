using System;
namespace DemoSc.Models
{
    public class Scooter
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public int battery_level { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsEnabled { get; set; }
    }
}
