using DAL.Enteties;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class PostRepository
    {
        IMongoDatabase database;
        IMongoCollection<Post> collection;
        public PostRepository()
        {
            database = MongoConfigManager.GetDefaultDatabase();
            collection = database.GetCollection<Post>(GetTableName());
        }

        private string GetTableName()
        {
            return "post";
        }

        public void Add(Post post) =>
          collection.InsertOne(post);

        public void Add(IEnumerable<Post> posts) =>
            collection.InsertMany(posts);

        public void Update(ObjectId id, Post post) =>
            collection.ReplaceOne(p => p.Id == id, post);

        public void UpdatePost(ObjectId postId, string newTxt)
        {
            var filter = Builders<Post>.Filter.Eq("_id", postId);
            var update = Builders<Post>.Update.Set("Text", newTxt);
            collection.UpdateOne(filter, update);
            
        }

        public void AddLike(string UserNickname, ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("_id", postId);
            var update = Builders<Post>.Update.Inc("Like", 1);
            collection.UpdateOne(filter, update);

            update = Builders<Post>.Update.Push("PersonsWhoLike", UserNickname);
            collection.UpdateOne(filter, update);

        }

        public void DismissLike(string UserNickname, ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("_id", postId);
            var update = Builders<Post>.Update.Inc("Like", -1);
            collection.UpdateOne(filter, update);

            update = Builders<Post>.Update.Pull("PersonsWhoLike", UserNickname);
            collection.UpdateOne(filter, update);
        }
        //
        public void AddComment(Comment comment, ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("_id", postId);
            var update = Builders<Post>.Update.Push("Comments", comment);
            collection.UpdateOne(filter, update);
        }

        public void DeleteComment(Comment comment, ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("_id", postId);
            var update = Builders<Post>.Update.Pull("Comments", comment);
            collection.UpdateOne(filter, update);
        }
        //
        public void AddDislike(string UserNickname, ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("_id", postId);
            var update = Builders<Post>.Update.Inc("DisLike", 1);
            collection.UpdateOne(filter, update);

            update = Builders<Post>.Update.Push("PersonsWhoDisLike", UserNickname);

            collection.UpdateOne(filter, update);

        }

        public void DismissDisLike(string UserNickname, ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("_id", postId);
            var update = Builders<Post>.Update.Inc("DisLike", -1);
            collection.UpdateOne(filter, update);

            update = Builders<Post>.Update.Pull("PersonsWhoDisLike", UserNickname);
            collection.UpdateOne(filter, update);
        }
        //
        public List<string> GetPersonsWhoLiked(ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("_id", postId);
            var people = collection.Find(filter).Project(x => x.PersonsWhoLike).First();
            return people;

        }
        public List<string> GetPersonsWhoDisLiked(ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("_id", postId);
            var people = collection.Find(filter).Project(x => x.PersonsWhoDisLike).First();
            return people;

        }
        //
        public int GetLike(ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("_id", postId);
            var like = collection.Find(filter).Project(x => x.Like).First();
            return like;
        }

        public int GetDislike(ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("_id", postId);
            var dislike = collection.Find(filter).Project(x => x.DisLike).First();
            return dislike;
        }
        //
        public List<Comment> GetComments(ObjectId postId)
        {
            var filter = Builders<Post>.Filter.Eq("_id", postId);
            var people = collection.Find(filter).Project(x => x.Comments).First();
            return people;
        }
        //
        public List<Post> GetNewPosts(string TimeOfLastUserLogin, List<ObjectId> following)
        {
            var filter = Builders<Post>.Filter.Gte("Date", TimeOfLastUserLogin);
            filter = filter & Builders<Post>.Filter.In("PostOwnerId", following);
            var posts = collection.Find(filter).ToList();
            return posts;
        }

        public List<Post> GetPosts(ObjectId OwnerId) =>
            collection.Find(p => p.PostOwnerId == OwnerId).ToList();

        public Post GetPost(ObjectId id) =>
          collection.Find(p => p.Id == id).FirstOrDefault();
        //
        public void Delete(Post post) =>
             collection.DeleteOne(p => p.Id == post.Id);

        public void Delete(ObjectId postId) =>
           collection.DeleteOne(p => p.Id == postId);
    }

}
