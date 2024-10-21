using DAL.Enteties;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UserRepository
    {
        IMongoDatabase database;
        IMongoCollection<User> collection;
        public UserRepository()
        {
            database = MongoConfigManager.GetDefaultDatabase();
            collection = database.GetCollection<User>(GetTableName());
        }

        private string GetTableName()
        {
            return "users";
        }
        // 
        public void Add(User user) =>
           collection.InsertOne(user);

        public void Add(IEnumerable<User> entities) =>
            collection.InsertMany(entities);
        // 
        public void Update(string nickname, User user) =>
            collection.ReplaceOne(entity => entity.NickName == nickname, user);

        public void Update(ObjectId id, User user) =>
            collection.ReplaceOne(entity => entity.Id == id, user);

        public void UpdateField(string nickname , string FieldToEdit , string FieldValue)
        {
            var filter = Builders<User>.Filter.Eq("NickName", nickname);
            var update = Builders<User>.Update.Set(FieldToEdit, FieldValue);
            collection.UpdateOne(filter, update);
        }

        public void UpdateDate(string nickname)
        {
            var filter = Builders<User>.Filter.Eq("NickName", nickname);
            var update = Builders<User>.Update.Set("Date", DateTime.Now.ToString());
            collection.UpdateOne(filter, update);
        }
        //
        public void addFollower(string nickname,string newFollower)
        {
            var filter = Builders<User>.Filter.Eq("NickName", nickname);
            var update = Builders<User>.Update.Push("Followers",newFollower);
            collection.UpdateOne(filter, update);
            
        }
        public void unFollow(string nickname, string follower)
        {
            var filter = Builders<User>.Filter.Eq("NickName", nickname);
            var update = Builders<User>.Update.Pull("Following", follower);
            collection.UpdateOne(filter, update);


            filter = Builders<User>.Filter.Eq("NickName", follower);
            update = Builders<User>.Update.Pull("Followers", nickname);
            collection.UpdateOne(filter, update);
        }

        public void addFollowing(string nickname, string newFollowing)
        {
            var filter = Builders<User>.Filter.Eq("NickName", nickname);
            var update = Builders<User>.Update.Push("Following", newFollowing);
            collection.UpdateOne(filter, update);
           
        }     
        //
        public List<string> GetFollowers(string nickname)
        {
            var filter = Builders<User>.Filter.Eq("NickName", nickname);
            var people = collection.Find(filter).Project(x => x.Followers).First();
            return people;
        }

        public List<string> GetFollowing(string nickname)
        {
            var filter = Builders<User>.Filter.Eq("NickName", nickname);
            var people = collection.Find(filter).Project(x => x.Following).First();
            return people;
        }
        //
        public ObjectId GetUserId(string nickname)
        {
            var user = collection.Find(entity => entity.NickName == nickname).FirstOrDefault();
            return user.Id;
        }
        //
        public List<User> GetUsers() =>
            collection.Find(entity => true).ToList();

        public User GetUser(string nickname) =>
          collection.Find(entity => entity.NickName == nickname).FirstOrDefault();

        public User GetUser(ObjectId id) =>
         collection.Find(entity => entity.Id == id).FirstOrDefault();
        //
       
        public void Delete(User user) =>
             collection.DeleteOne(u => u.Id == user.Id);

        public void Delete(ObjectId userId) =>
            collection.DeleteOne(u => u.Id == userId);

        public void Delete(string nickname) =>
            collection.DeleteOne(u => u.NickName == nickname);

    }
}
