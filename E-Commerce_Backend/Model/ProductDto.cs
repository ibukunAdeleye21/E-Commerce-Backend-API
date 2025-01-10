namespace E_Commerce_Backend.Model
{
    public class ProductDto
    {
        public int id { get; set; }
        public string title { get; set; } = string.Empty;
        public decimal price { get; set; }
        public string description { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public string image {  get; set; } = string.Empty;
        public RatingDto? rating { get; set; } 

    }

    public class RatingDto
    {
        public decimal Rate { get; set; }
        public int Count { get; set; }
    }
}
