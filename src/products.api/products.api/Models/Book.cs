namespace products.api.Models
{
    public class Book
    {
        public Book(int id, string title, string author, decimal price, int ownerId)
        {
            Id = id;
            Title = title;
            Author = author;
            Price = price;
            OwnerId = ownerId;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal  Price { get; set; }
        public int OwnerId { get; set; }
    }
}
