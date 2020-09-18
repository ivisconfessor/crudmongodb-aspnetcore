using MongoDB.Driver;
using System;

namespace CrudMongoDB.DataBase
{
    public sealed class MongoDBDataBase
    {
        private static volatile MongoDBDataBase instance;
        private static object syncLock = new Object();
        private static IMongoDatabase db = null;

        private MongoDBDataBase()
        {
            try
            {
                // Conexão Servidor
                var client = new MongoClient("mongodb+srv://crudmongodb:crudmongodb@server01.5qo3v.mongodb.net/crudmongodb?retryWrites=true&w=majority");

                // Conexão/Criação Base de Dados
                db = client.GetDatabase("crudmongodb");
            }
            catch (MongoException ex)
            {
                Console.WriteLine(string.Format("ERRO AO ESTABELECE CONEXÃO COM BANCO DE DADOS: {0}", ex));
            }
        }

        public static IMongoDatabase GetConnect
        {
            get
            {
                if (instance == null)
                {
                    lock (syncLock)
                    {
                        if (instance == null)
                        {
                            instance = new MongoDBDataBase();
                            Console.WriteLine("\n\n\n:: Nova Conexão - Instância Criada");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\n\n\n:: Reutilizando Conexão");
                }

                return db;
            }
        }
    }
}
