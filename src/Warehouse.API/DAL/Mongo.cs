using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Mongo {
public class PersonsRepository
    {

        protected FooDBContext Context => new FooDBContext();

        public void Insert(Person person)
        {
            Context.Persons.InsertOne(person);
        }
        public List<Person> Find(Func<Person, bool> predicate)
        {
            return Context.Persons.AsQueryable().Where(predicate).ToList();
        }
    }


    public class FooDBContext
    {
        //public static string ConnectionString => "mongodb://172.17.0.3:27017";
        //public static string ConnectionString => "mongodb://localhost:27017";
        public static string ConnectionString=> Environment.GetEnvironmentVariable("mongo_connection_string");
        static FooDBContext(){
            BsonClassMap.RegisterClassMap<Person>(p =>
            {
                p.AutoMap();
                p.MapIdMember(c => c.Id);//.SetIdGenerator(CombGuidGenerator.Instance);
            });
        }
        protected MongoClient Client => new MongoClient(ConnectionString);
        protected IMongoDatabase DataBase => Client.GetDatabase("foo");
        public IMongoCollection<Person> Persons => DataBase.GetCollection<Person>("bar");
    }



    public class Person
    {
        //[BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
    }

    public class SomeTests
    {
        public static void Main1(string[] args)
        {

            BsonClassMap.RegisterClassMap<Person>(p =>
            {
                p.AutoMap();
                p.MapIdMember(c => c.Id);//.SetIdGenerator(CombGuidGenerator.Instance);
            });
            //Test2().Wait();
            Test3();
        }


        public static void Test3()
        {
            var repo = new PersonsRepository();
            repo.Insert(new Person { Name = "John" });

            var persons = repo.Find(x => x.Name == "John").ToList();

            persons.ForEach(p =>
            {
                Console.WriteLine(p.Name);
            });
        }

        private static async Task Test2()
        {
            var context = new FooDBContext();
            await context.Persons.InsertOneAsync(new Person { Name = "John" });

            var persons = context.Persons.AsQueryable().Where(x => x.Name == "John").ToList();

            persons.ForEach(p =>
               {
                   Console.WriteLine(p.Name);
               });
        }

        private static async Task Test()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("foo");
            var collection = database.GetCollection<Person>("bar");

            await collection.InsertOneAsync(new Person { Name = "Jacks" });

            var list = await collection.Find(x => x.Name == "Jack")
                .ToListAsync();

            foreach (var person in list)
            {
                Console.WriteLine(person.Name);
            }
        }
    }
}