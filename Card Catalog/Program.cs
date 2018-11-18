using System;
using System.Collections.Generic;
using System.IO;

namespace Card_Catalog
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Library Card Catalog console application");
            string filename = GetCatalogFile();
            List<Book> books = LoadDataFromCatalogFile(filename);
            CardCatalog cardCatalog = new CardCatalog(filename);
            string menuOption = "";
            while (menuOption != "3")
            {
                menuOption = GetMenuOption();
                if (menuOption == "1")
                {
                    cardCatalog.ListBooks(books);
                }
                else if (menuOption == "2")
                {
                    cardCatalog.AddBook(books);
                }
                else
                {
                    cardCatalog.Save(books,filename);
                }
            }
        }

        private static string GetCatalogFile()
        {
            string filename;
            Console.WriteLine();
            Console.Write("Please input the name of an existing Card Catalog file: ");
            filename = Console.ReadLine();
            while (FoundFile(filename) == false)
            {
                Console.WriteLine();
                Console.Write("Please input the name of an existing Card Catalog file: ");
                filename = Console.ReadLine();
            }
            return filename;
        }

        private static bool FoundFile(string filename)
        {            
            try
            {
                StreamReader inFile = new StreamReader(@filename);
                inFile.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }            
        }

        private static string GetMenuOption()
        {
            Console.WriteLine();
            Console.WriteLine("1.  List all books");
            Console.WriteLine("2.  Add a Book");
            Console.WriteLine("3.  Save and Exit");
            string menuOption = "";
            while (menuOption != "1" && menuOption != "2" && menuOption != "3")
            {
                Console.WriteLine();
                Console.Write("Please select an option from the menu above by inputting 1, 2 or 3: ");
                menuOption = Console.ReadLine();
                Console.WriteLine();
            }
            return menuOption;
        }

        private static List<Book> LoadDataFromCatalogFile(string filename)
        {
            StreamReader textIn = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read));
            List<Book> books = new List<Book>();
            while (textIn.Peek() !=-1)
            {
                string row = textIn.ReadLine();
                string[] columns = row.Split('|');
                Book book = new Book(columns[0],columns[1]);                
                books.Add(book);
            }
            textIn.Close();
            return books;
        }
    }

    public class Book
    {
        private string _title;
        private string _author;

        public Book(string author, string title)
        {
            _author = author;
            _title = title;
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }
    }

    public class CardCatalog
    {
        private string _filename;
        private List<Book> _books;       

        public CardCatalog(string filename)
        {       
            _filename = filename;
        }
  
        public void ListBooks(List<Book> books)
        {
            _books = books;
            foreach (Book book in _books)
            {
                Console.WriteLine(book.Title + " written by " + book.Author + ".");
            }           
        }

        public void AddBook(List<Book> books)
        {
            _books = books;
            string author;
            string title;

            Console.Write("Please input the author's last name: ");
            author = Console.ReadLine();
            Console.Write("Please input the title of the book: ");
            title = Console.ReadLine();
            Book book = new Book(author, title);
            _books.Add(book);
            Console.WriteLine();
            Console.WriteLine("Thank you. The book " + title + " written by " + author + " has been added to the card catalog.");
        }

        public void Save(List<Book> books, string filename)
        {
            _books = books;
            _filename = filename;            
            StreamWriter textOut = new StreamWriter(new FileStream(_filename, FileMode.Create, FileAccess.Write));
            foreach (Book book in _books)
            {
                textOut.Write(book.Author + "|");
                textOut.WriteLine(book.Title);
            }
            textOut.Close();
        }
    }

}
