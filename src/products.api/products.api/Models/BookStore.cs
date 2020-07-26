using System.Collections.Generic;
using System.Linq;

namespace products.api.Models
{
    public static class BookStore
    {
        private static IEnumerable<Book> _books = new List<Book>
        {
            new Book(1, "ANGULAR 9 In Action", "Google Team", 49),
            new Book(2, "REACT In Action", "Facebook Team", 39),
            new Book(3, ".NET CORE 3.1 In Action", "Microsoft Team", 59),
        };

        public  static IEnumerable<Book> GetAll()
        {
            return _books;
        }
        public static Book GetById(int id)
        {
            return _books.FirstOrDefault(x => x.Id == id);
        }
    }
}
