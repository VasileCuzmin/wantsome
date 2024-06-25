using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;
using PublisherDomain.Models;

using PubContext _context = new();
//context.Database.EnsureCreated();//created the DB on the fly if it does not exist

//AddAuthor();
//GetAuthors();
//AddAuthorWithBook();
//GetAuthorsWithBooks();
//QueryFilters();
//RetrieveAndUpdateAuthor();

//AddSomeMoreAuthors();

//SkípAndTakeAuthors();

//SortAuthors();
LazyLoadBooksFromAnAuthor();

void AddAuthor()
{
    var author = new Author()//notice I'm not providing a value for ID - the DB is going to do that for me
    {
        FirstName = "John",
        LastName = "Smith"
    };

    using var context = new PubContext();
    context.Authors.Add(author);
    context.SaveChanges();
}

void GetAuthors()
{
    using var context = new PubContext();

    var authors = context.Authors;
    //var authors = context.Authors.ToList();//materializing the query - bring the result set in memory

    foreach (var author in authors)
    {
        //   Console.WriteLine(author.FirstName + " " + author.LastName);
    }
    Console.WriteLine("Executing Count()...");
    var activeUsersCount = authors.Count(user => user.FirstName == "John");
}


void AddAuthorWithBook()
{
    var author = new Author()
    {
        FirstName = "John",
        LastName = "Smith"
    };

    author.Books.Add(new Book()
    {
        Title = "Introducing to Entity Framework",
        PublishDate = new DateOnly(2010, 8, 1)
    });

    using var context = new PubContext();
    context.Authors.Add(author);
    context.SaveChanges();
    //After the context finished executing command, it fixes up the state of the EntityEntry that is tracking this
    //author - resets its state to Unchanged

}


void GetAuthorsWithBooks()
{
    using var context = new PubContext();
    var authors = context.Authors.Include(a => a.Books).ToList();
    foreach (var author in authors)
    {
        Console.WriteLine(author.FirstName + " " + author.LastName);
        foreach (var book in author.Books)
        {
            Console.WriteLine(book.Title);
        }
    }
}

void AddSomeMoreAuthors()
{
    _context.Authors.Add(new Author() { FirstName = "Tim", LastName = "Doe" });
    _context.Authors.Add(new Author() { FirstName = "Josef", LastName = "Popescu" });
    _context.Authors.Add(new Author() { FirstName = "John", LastName = "Doe" });
    _context.Authors.Add(new Author() { FirstName = "Paul", LastName = "Paul" });
    _context.SaveChanges();
}


void QueryFilters()
{
    //var firstName = "John";
    //var authors = _context.Authors.Where(a => a.FirstName == firstName).ToList();
    var authors = _context.Authors.Where(a => EF.Functions.Like(a.LastName, "L%"))
        .ToList();
}

void RetrieveAndUpdateAuthor()
{
    var author = _context.Authors.FirstOrDefault(a => a.FirstName == "John" && a.LastName == "Smith");
    if (author != null)
    {
        author.FirstName = "Julia";

        Console.WriteLine($"Before: {_context.ChangeTracker.DebugView.ShortView}");
        _context.ChangeTracker.DetectChanges();
        Console.WriteLine($"After: {_context.ChangeTracker.DebugView.ShortView}");


        _context.SaveChanges();
    }
}

void SkípAndTakeAuthors()
{
    var groupSize = 2;
    for (int i = 0; i < 5; i++)
    {
        var authors = _context.Authors.Skip(groupSize * i).Take(groupSize).ToList();
        Console.WriteLine($"Group {i}:");
        foreach (var author in authors)
        {
            Console.WriteLine(author.FirstName + " " + author.LastName);
        }
    }
}


void SortAuthors()
{
    //var authorsByLastName = _context.Authors.OrderBy(a => a.LastName).ToList();
    //authorsByLastName.ForEach(a => Console.WriteLine(a.LastName + " " + a.FirstName));

    ////What happens if we have multiple OrderBy calls?
    //var authorsByLastName = _context.Authors
    //    .OrderBy(a => a.LastName).
    //    OrderBy(a=> a.FirstName);


    var authorsByLastName = _context.Authors
        .OrderBy(a => a.LastName).
         ThenBy(a => a.FirstName)
        .ToList();


    authorsByLastName.ForEach(a => Console.WriteLine(a.LastName + " " + a.FirstName));
}


void DeleteAnAuthor()
{
    var extra = _context.Authors.Find(1);
    //var extra = _context.Authors.FirstOrDefault(a => a.Id == 1);

    if (extra != null)
    {
        _context.Authors.Remove(extra);
        _context.SaveChanges();
    }
}

void InsertMultipleAuthors()
{
    var newAuthors = new Author[]{
        new Author { FirstName = "Ruth", LastName = "Ozeki" },
        new Author { FirstName = "Sofia", LastName = "Segovia" },
        new Author { FirstName = "Ursula K.", LastName = "LeGuin" },
        new Author { FirstName = "Hugh", LastName = "Howey" },
        new Author { FirstName = "Isabelle", LastName = "Allende" }
    };
    _context.AddRange(newAuthors);
    _context.SaveChanges();
}

void InsertMultipleAuthorsPassedIn(List<Author> listOfAuthors)
{
    _context.Authors.AddRange(listOfAuthors);
    _context.SaveChanges();
}


void ExecuteDelete()
{
    var deleteId = 9;
    _context.Authors.Where(a => a.AuthorId == deleteId).ExecuteDelete();
    var startswith = "H";
    var count = _context.Authors.Where(a => a.LastName.StartsWith(startswith)).ExecuteDelete();
}


void ExecuteUpdate()
{
    var tenYearsAgo = DateOnly.FromDateTime(DateTime.Now).AddYears(-10);
    ////change price of books older than 10 years to $1.50
    var oldbookprice = 1.50m;
    _context.Books.Where(b => b.PublishDate < tenYearsAgo)
        .ExecuteUpdate(setters => setters.SetProperty(b => b.Price, oldbookprice));

    ////change all last names to lower case
    _context.Authors
        .ExecuteUpdate(setters => setters.SetProperty(a => a.LastName, a => a.LastName.ToLower()));

    //change all last names back to title case
    //(note:May look funky but LINQ can't translate efforts like ToUpperInvariant and TextInfo)
    _context.Authors
        .ExecuteUpdate(setters => setters.SetProperty(
            a => a.LastName,
            a => a.LastName.Substring(0, 1).ToUpper() + a.LastName.Substring(1).ToLower()));
}



void InsertNewAuthorWithBook()
{

}

void InsertNewAuthorWith2NewBooks()
{

}

void AddNewBookToExistingAuthorInMemory()
{

}


void EagerLoadBooksWithAuthors()
{

}

void EagerLoadBooksThatWerePublishedSince2010WithAuthors()
{

}

void Projections()
{
    var unknownTypes = _context.Authors
        .Select(a => new
        {
            a.AuthorId,
            Name = a.FirstName.First() + "" + a.LastName,
            a.Books  //.Where(b => b.PublishDate.Year < 2000).Count()
        })
        .ToList();
    var debugview = _context.ChangeTracker.DebugView.ShortView;
}


void LazyLoadBooksFromAnAuthor()
{
    //requires lazy loading to be set up in your app
    var author = _context.Authors.FirstOrDefault(a => a.LastName == "Lerman");
    if (author != null)
    {
        foreach (var book in author.Books)
        {
            Console.WriteLine(book.Title);
        }
    }

}

//FilterUsingRelatedData();
// Notice that Books are not loaded into memory !!
void FilterUsingRelatedData()
{
    var recentAuthors = _context.Authors
        .Where(a => a.Books.Any(b => b.PublishDate.Year >= 2015))
        .ToList();
}
