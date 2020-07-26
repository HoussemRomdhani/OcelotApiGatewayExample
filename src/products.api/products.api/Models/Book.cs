namespace products.api.Models
{
    public class Book
    {
        public Book(int id, string title, string author, decimal price)
        {
            Id = id;
            Title = title;
            Author = author;
            Price = price;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal  Price { get; set; }
    }
}
