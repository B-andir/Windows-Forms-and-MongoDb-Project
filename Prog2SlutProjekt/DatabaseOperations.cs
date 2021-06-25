using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Prog2SlutProjekt03
{
    class DatabaseOperations
    {
        MongoClient dbClient = new MongoClient("mongodb+srv://MongoDBUser:JwxaiesFPFkAPZci@cluster0.e0jur.mongodb.net?retryWrites=true&w=majority");

        public List<Player> createLeaderboard()
        {
            List<Player> playerList = new List<Player>();

            var database = dbClient.GetDatabase("leaderboard");
            var collection = database.GetCollection<BsonDocument>("entries");

            var sort = Builders<BsonDocument>.Sort.Ascending("time");

            var documents = collection.Find(new BsonDocument()).Sort(sort).ToList();

            foreach (var document in documents)
            {
                string name = document[1].ToString();
                string time = document[2].ToString();

                playerList.Add(new Player(name, time));
            }

            return (playerList);
        }

        public void updateLeaderboard(string name, string time)
        {
            var database = dbClient.GetDatabase("leaderboard");
            var collection = database.GetCollection<BsonDocument>("entries");

            var document = new BsonDocument 
            { 
                { "name", name }, 
                { "time", time } 
            };

            collection.InsertOne(document);
        }
    }
}
