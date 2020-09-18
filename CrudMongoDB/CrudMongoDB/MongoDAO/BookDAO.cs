using CrudMongoDB.DataBase;
using CrudMongoDB.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudMongoDB.MongoDAO
{
    public sealed class BookDAO
    {
        private static BookDAO _instace;

        public static BookDAO Instance
        {
            get
            {
                if (_instace == null) _instace = new BookDAO();
                return _instace;
            }
        }

        public IMongoCollection<Book> GetBooksCollectionDAO()
        {
            IMongoCollection<Book> booksCollection = MongoDBDataBase.GetConnect.GetCollection<Book>("Books");

            return booksCollection;
        }

        public void InsertBookDAO(IMongoCollection<Book> booksCollection, Book infoBook)
        {
            booksCollection.InsertOne(infoBook);
        }

        public List<Book> ListBooksDAO(IMongoCollection<Book> booksCollection)
        {
            var filter = Builders<Book>.Filter.Empty;
            var books = booksCollection.Find<Book>(filter).ToList();
            return books;
        }

        public void UpdateBookDAO(IMongoCollection<Book> booksCollection, Book infoBook, string id)
        {
            // Filtro para Buscar apenas o Livro que está sendo atualizado
            var filter = Builders<Book>.Filter.Eq(b => b.Id, ObjectId.Parse(id));

            // Populando Atualização 
            var bookUpdate = Builders<Book>.Update
                .Set(b => b.YearPublish, infoBook.YearPublish)
                .Set(b => b.Description, infoBook.Description);

            // Efetivar Atualização do Livro
            booksCollection.UpdateOne(filter, bookUpdate);

        }

        public void DeleteBookDAO(IMongoCollection<Book> booksCollection, string id)
        {
            // Filtro para excluir apenas o Livro que contém o id passando em parâmetro
            var filter = Builders<Book>.Filter.Where(b => b.Id == ObjectId.Parse(id));

            booksCollection.DeleteOne(filter);
        }
    }
}
