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
                // Conexão Servidor
                var client = new MongoClient("mongodb+srv://crudmongodb:crudmongodb@server01.5qo3v.mongodb.net/crudmongodb?retryWrites=true&w=majority");

                // Conexão/Criação Base de Dados
                var database = client.GetDatabase("crudmongodb");

                // Criando Coleção Books, onde será salvo os dados do Livro (A Coleção em como se fosse uma tabela em um Banco Relacional)
                IMongoCollection<Book> booksCollection = database.GetCollection<Book>("Books");

                var filter = Builders<Book>.Filter.Where(b=>b.Id == ObjectId.Parse(bookId));
                var book = booksCollection.Find<Book>(filter).First();

                return View(book);
            }
        }

        public IActionResult InsertBook(Book book)
        {
            // Conexão Servidor
            var client = new MongoClient("mongodb+srv://crudmongodb:crudmongodb@server01.5qo3v.mongodb.net/crudmongodb?retryWrites=true&w=majority");

            // Conexão/Criação Base de Dados
            var database = client.GetDatabase("crudmongodb");

            // Criando Coleção Books, onde será salvo os dados do Livro (A Coleção em como se fosse uma tabela em um Banco Relacional)
            IMongoCollection<Book> booksCollection = database.GetCollection<Book>("Books");

            booksCollection.InsertOne(book);

            TempData["Sucesso"] = "Livro Cadastrado!";
            return RedirectToAction("Index");
        }

        public List<Book> ListBooks()
        {
            // Conexão Servidor
            var client = new MongoClient("mongodb+srv://crudmongodb:crudmongodb@server01.5qo3v.mongodb.net/crudmongodb?retryWrites=true&w=majority");

            // Conexão/Criação Base de Dados
            var database = client.GetDatabase("crudmongodb");

            // Criando Coleção Books, onde será salvo os dados do Livro (A Coleção em como se fosse uma tabela em um Banco Relacional)
            IMongoCollection<Book> booksCollection = database.GetCollection<Book>("Books");

            var filter = Builders<Book>.Filter.Empty;
            var books = booksCollection.Find<Book>(filter).ToList();

            return books;
        }

        public IActionResult UpdateBook(Book updated, string bookId)
        {
            // Conexão Servidor
            var client = new MongoClient("mongodb+srv://crudmongodb:crudmongodb@server01.5qo3v.mongodb.net/crudmongodb?retryWrites=true&w=majority");

            // Conexão/Criação Base de Dados
            var database = client.GetDatabase("crudmongodb");

            // Criando Coleção Books, onde será salvo os dados do Livro (A Coleção em como se fosse uma tabela em um Banco Relacional)
            IMongoCollection<Book> booksCollection = database.GetCollection<Book>("Books");

            // Filtro para Buscar apenas o Livro que está sendo atualizado
            var filter = Builders<Book>.Filter.Eq(b => b.Id, ObjectId.Parse(bookId));

            // Populando Atualização 
            var bookUpdate = Builders<Book>.Update
                .Set(b => b.YearPublish, updated.YearPublish)
                .Set(b => b.Description, updated.Description);

            // Efetivar Atualização do Livro
            booksCollection.UpdateOne(filter, bookUpdate);

            TempData["Sucesso"] = "Livro Atualizado com Sucesso!";
            return RedirectToAction("Index");
        } 

        public IActionResult DeleteBook()
        {
            TempData["Sucesso"] = "Livro Deletado com Sucesso!";
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
