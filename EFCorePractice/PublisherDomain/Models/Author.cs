﻿namespace PublisherDomain.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<Book> Books
        { get; set; } = new List<Book>();
    }
}
