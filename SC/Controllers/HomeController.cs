using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entities;

namespace SC.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Инициализация репозитория
        /// </summary>
        EFRepository repositoty = new EFRepository();
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Отображение списка книг
        /// </summary>
        public ActionResult ShowBooks()
        {
            return View(repositoty.GetBooks());
        }


        /// <summary>
        /// Отображение списка пользователей
        /// </summary>
        public ActionResult ShowUsers()
        {
            return View(repositoty.GetUsers());
        }

        /// <summary>
        /// Отображение списка авторов
        /// </summary>
        public ActionResult ShowAutors()
        {
            return View(repositoty.GetAutors());
        }

        /// <summary>
        /// Отображение информации по искомому пользователю
        /// </summary>
        /// <param name="id"></param>
        public ActionResult FindUser(int id)
        {
            return View(repositoty.GetUser(id));
        }

        /// <summary>
        /// Найти все книги автора
        /// </summary>
        public ActionResult FindAutor(int id)
        {
            return View(repositoty.GetAutor(id));
        }

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public RedirectResult AddUser(User user)
        {
            repositoty.AddUser(user);
            return Redirect("/Home/ShowUsers");
        }

        /// <summary>
        /// Показ книг находящиеся у читателя
        /// </summary>
        /// <param name="id">ID читателя</param>
        /// <returns></returns>
        public ActionResult ShowReaders(int id)
        {
            return View(repositoty.GetBook(id));
        }

        [HttpGet]
        public ActionResult TakeBook(int id)
        {
            ViewBag.id = id;
            return View(repositoty.GetBooks());
        }


        public RedirectResult BookHasTaken(int id, int id2)
        {
            repositoty.AddUserBook(id, id2);
            return Redirect("/Home/FindUser/"+id);
        }

        /// <summary>
        ///  Удаление книги у пользователя
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <param name="id2">id книги</param>
        [HttpGet]
        public RedirectResult RemoveBookFromUser(int id, int id2)
        {
            repositoty.RemoveBookFromUser(id, id2);
            string path = "/Home/FindUser/" + id;
            return Redirect(path);
        }

        [HttpGet]
        public ActionResult AddBookByAuthor()
        {
            return View();
        }

        [HttpPost]
        public RedirectResult AddBookByAuthor(string authorName, string authorSecondName, string bookTitle)
        {
            if (authorName != "" && authorSecondName != "" && bookTitle != "")
            {
                repositoty.AddBookByAuthor(authorName, authorSecondName, bookTitle);
                return Redirect("/Home/ShowBooks/");

            }
            return Redirect("/Home/AddBookByAuthor/");
        }


        public ActionResult Test()
        {
            return View(repositoty.Test());
        }


    }
}