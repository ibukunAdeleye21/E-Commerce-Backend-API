using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Backend.Entity
{
    public class Rating
    {
        public decimal Rate { get; set; }
        public int Count { get; set; }
    }
}
