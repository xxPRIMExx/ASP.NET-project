using Domain;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Domain.Entities;


namespace SC
{
    public class EFRepository
    {
        private EFDbContext context;
        //test

        public IEnumerable<Book> Test()
        {
            System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("@Id", "%2%");
            var b = context.Database.SqlQuery<Book>("Select * from Books WHERE Id LIKE @Id", param);
            return b;
        }

        
        /// <summary>
        /// Создаем подключение к БД
        /// </summary>
        public EFRepository()
        {
            context = new EFDbContext(ConfigurationManager.ConnectionStrings[0].ConnectionString);
        }

        /// <summary>
        /// Получаем все книги из БД
        /// </summary>
        public IEnumerable<Book> GetBooks()
        {
            return context.Books;
        }

        /// <summary>
        /// Получаем всех пользователей и БД
        /// </summary>
        public IEnumerable<User> GetUsers()
        {
            return context.Users;
        }

        /// <summary>
        /// Получаем всех авторов из БД
        /// </summary>
        public IEnumerable<Autor> GetAutors()
        {
            return context.Autors;
        }

        /// <summary>
        /// Получаем пользователя по ID
        /// </summary>
        /// <param name="id">Идентификационный номер пользователя</param>
        public User GetUser(int id)
        {
            return context.Users.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Получаем автора по ID
        /// </summary>
        /// <param name="id">Идентификаационный номер автора</param>
        public Autor GetAutor(int id)
        {
            return context.Autors.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Добавлеие пользователя
        /// </summary>
        /// <param name="u">Новый пользователь</param>
        public void AddUser(User u)
        {
            context.Users.Add(u);
            context.SaveChanges();
        }

        /// <summary>
        /// Добавление книги
        /// </summary>
        /// <param name="b">Новая книга</param>
        public void AddBook(Book b)
        {
            context.Books.Add(b);
            context.SaveChanges();
        }

        /// <summary>
        /// Получить книгу по ID
        /// </summary>
        /// <param name="id">Идентификатор книги</param>
        /// <returns></returns>
        public Book GetBook(int id)
        {
            return context.Books.FirstOrDefault(x => x.Id == id);
        }


        /// <summary>
        /// Добавление новой книги и связывание еще с автором
        /// </summary>
        /// <param name="authorName">Имя автора</param>
        /// <param name="authorSecondName">Фамилия автора</param>
        /// <param name="bookTitle">Название книги</param>
        public void AddBookByAuthor(string authorName, string authorSecondName, string bookTitle)
        {
            //IEnumerable<Book> tmpBook = 
            //    from b in context.Books
            //    where (b.Title == bookTitle) && ((from a in b.Autors where a.Name == authorName && a.LastName == authorSecondName select a) != null)
            //    select b;

            //if (tmpBook.Count() > 0)
            //{
            //    return;
            //}

            Book tmpBook = context.Books.FirstOrDefault(n => n.Title == bookTitle);
            Autor tmpAuthor = context.Autors.FirstOrDefault(n => n.Name == authorName && n.LastName == authorSecondName);

            // ------ Поиск автора в базе данных --------------------
            foreach (Autor b in context.Autors)
            {
                if (b.Name == authorName && b.LastName == authorSecondName)     // Имя и фамилия совпадают
                {
                    tmpAuthor = b;                                              // получаем ссылку на запись в бд
                    break;
                }
            }
            if (tmpAuthor == null)                                              // Автор не найден, создаем нового автора
            {
                context.Autors.Add(new Autor(authorName, authorSecondName));
                context.SaveChanges();
            }

            int tmpID = 0;
            foreach (Autor b in context.Autors)                                 // Находим присвоенный ID нового автора
            {
                if (b.Name == authorName && b.LastName == authorSecondName)
                {
                    tmpID = b.Id;                                               // помещаем присвоенный новый ID автора в переменную
                    break;
                }
            }

            tmpAuthor = GetAutor(tmpID);                                        // Получаем ссылку на автора по ID

            tmpBook = context.Books.FirstOrDefault(s => s.Title == bookTitle);  // Ищем книгу по названию
            if (tmpBook == null)
            {
                context.Books.Add(new Book(bookTitle));                         // Если книга не найдена, создаем книгу с новым названием
                context.SaveChanges();
            }

            tmpBook = context.Books.FirstOrDefault(t => t.Title == bookTitle);  // Получаем ссылку на только что добавленную книгу

            tmpBook.Autors.Add(tmpAuthor);                                      // Добавляем связи n-n для книги
            tmpAuthor.Books.Add(tmpBook);                                       // Добавляем связи n-n для автора

            context.SaveChanges();                                              // Сохраняем результат
        }


        /// <summary>
        /// Привязка книги к пользователю
        /// </summary>
        /// <param name="userID">ID Пользователя</param>
        /// <param name="bookID">ID книги</param>
        public void AddUserBook(int userID, int bookID)
        {
            GetUser(userID).Books.Add(GetBook(bookID));
            GetBook(bookID).Users.Add(GetUser(userID));
            context.SaveChanges();
        }

        /// <summary>
        /// Отвязка книги от пользователя
        /// </summary>
        /// <param name="userID">ID Пользователя</param>
        /// <param name="bookID">ID книги</param>
        public void RemoveBookFromUser(int userID, int BookID)
        {
            GetUser(userID).Books.Remove(GetBook(BookID));
            GetBook(BookID).Users.Remove(GetUser(userID));
            context.SaveChanges();
        }

        /// <summary>
        /// Удаление пользовтеля из БД
        /// </summary>
        /// <param name="userID"></param>
        public void RemoveUser(int userID)
        {
            context.Users.Remove(GetUser(userID));
            context.SaveChanges();
        }


    }
}