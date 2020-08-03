using System.Collections.Generic;
using System.Linq;

namespace products.api.Models
{
    public static class BookStore
    {
        private static IEnumerable<Book> _books = new List<Book>
        {
            new Book(1, "ANGULAR 9 In Action", "Google Team", 49, 1),
            new Book(2, "REACT In Action", "Facebook Team", 39, 2),
            new Book(3, ".NET CORE 3.1 In Action", "Microsoft Team", 59, 1),
        };

        public  static IEnumerable<Book> GetAll(int ownerId)
        {
            return _books.Where(x=> x.OwnerId == ownerId);
        }
        public static Book GetById(int id)
        {
            return _books.FirstOrDefault(x => x.Id == id);
        }
    }
}
