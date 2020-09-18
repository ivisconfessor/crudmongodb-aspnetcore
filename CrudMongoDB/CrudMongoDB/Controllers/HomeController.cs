using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CrudMongoDB.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using CrudMongoDB.MongoDAO;

namespace CrudMongoDB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(ListBooks());
        }

        public IActionResult FormBook(string bookId)
        {
            if (bookId == null)
                return View();
            else
            {
                // Obter Coleção Books - Coleção que contém os Livros Cadastrados (A Coleção é como se fosse uma tabela em um Banco de Dados Relacional)
                var booksCollection = BookDAO.Instance.GetBooksCollectionDAO();

                // Filtrar por Id o Livro que será carregado para Edição
                var filter = Builders<Book>.Filter.Where(b=>b.Id == ObjectId.Parse(bookId));
                var book = booksCollection.Find<Book>(filter).First();

                return View(book);
            }
        }

        public IActionResult InsertBook(Book book)
        {
            // Coleção Books, onde será salvo os dados do Livro (A Coleção em como se fosse uma tabela em um Banco de Dados Relacional)
            var booksCollection = BookDAO.Instance.GetBooksCollectionDAO();

            // Inserir Livro na Coleção Books
            BookDAO.Instance.InsertBookDAO(booksCollection, book);

            TempData["Sucesso"] = "Livro Cadastrado com Sucesso!";
            return RedirectToAction("Index");
        }

        public List<Book> ListBooks()
        {
            // Obter Coleção Books - Coleção que contém os Livros Cadastrados (A Coleção é como se fosse uma tabela em um Banco de Dados Relacional)
            var booksCollection = BookDAO.Instance.GetBooksCollectionDAO();

            // Listar todos os Livros Cadastrados
            var books = BookDAO.Instance.ListBooksDAO(booksCollection);

            return books;
        }

        public IActionResult UpdateBook(Book updated, string bookId)
        {
            // Obter Coleção Books - Coleção que contém os Livros Cadastrados (A Coleção é como se fosse uma tabela em um Banco de Dados Relacional)
            var booksCollection = BookDAO.Instance.GetBooksCollectionDAO();

            // Atualizar Livro na Coleção Books de acordo com o Id passado em parâmetro
            BookDAO.Instance.UpdateBookDAO(booksCollection, updated, bookId);
            
            TempData["Sucesso"] = "Livro Atualizado com Sucesso!";
            return RedirectToAction("Index");
        } 

        public IActionResult DeleteBook(string bookId)
        {
            // Obter Coleção Books - Coleção que contém os Livros Cadastrados (A Coleção é como se fosse uma tabela em um Banco Relacional)
            var booksCollection = BookDAO.Instance.GetBooksCollectionDAO();

            // Deleta Livro na Coleção Books de acordo com o Id passado em parâmetro
            BookDAO.Instance.DeleteBookDAO(booksCollection, bookId);

            TempData["Sucesso"] = "Livro Deletado com Sucesso!";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}