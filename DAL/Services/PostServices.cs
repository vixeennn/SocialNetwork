using DAL.Enteties;
using DAL.Repository;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class PostServices
    {
        PostRepository repository;
        UserRepository userRepository;
        UserServices userServices;
        public PostServices()
        {
            repository = new PostRepository();
            userServices = new UserServices();
            userRepository = new UserRepository();
        }
        //
        public void InsertPost(string text)
        {
            Post post = new Post();
            post.Text = text;
            post.PostOwnerId = userRepository.GetUserId(userServices.NickNameRead());
            post.Date = DateTime.Now.ToString();
            repository.Add(post);
        }
        //
        public void EditPost(string newText, ObjectId postId)
        {
            try
            {
                repository.UpdatePost(postId, newText);
            }
            catch
            {

            }

        }
        //
        public bool DeletePost(ObjectId postId)
        {
            try
            {
                repository.Delete(postId);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //
        public bool CheckIfUserDisLikePost(string UserNickname, ObjectId postId)
        {

            Post post = new Post();
            post = repository.GetPost(postId);
            if (post != null)
            {
                if (post.PersonsWhoDisLike != null && post.PersonsWhoDisLike.Count > 0)
                {
                    foreach (var nick in post.PersonsWhoDisLike)
                    {
                        if (nick == UserNickname)
                        {
                            return true;
                        }
                    }
                }
            }



            return false;
        }

        public bool CheckIfUserLikePost(string UserNickname, ObjectId postId)
        {

            Post post = new Post();
            post = repository.GetPost(postId);
            if (post != null)
            {
                if (post.PersonsWhoLike != null && post.PersonsWhoLike.Count > 0)
                {
                    foreach (var nick in post.PersonsWhoLike)
                    {
                        if (nick == UserNickname)
                        {
                            return true;
                        }
                    }
                }
            }



            return false;
        }
        //
        public void AddDislike(string UserNickname, ObjectId postId)
        {
            try
            {
                repository.AddDislike(UserNickname, postId);
            }
            catch
            {

            }

        }

        public void DismissDisLike(string UserNickname, ObjectId postId)
        {
            try
            {
                repository.DismissDisLike(UserNickname, postId);
            }
            catch
            {

            }
        }
        //
        public void AddLike(string UserNickname, ObjectId postId)
        {
            try
            {
                repository.AddLike(UserNickname, postId);
            }
            catch
            {

            }

        }

        public void DismissLike(string UserNickname, ObjectId postId)
        {
            try
            {
                repository.DismissLike(UserNickname, postId);
            }
            catch
            {

            }
        }
        //
        public int GetLikes(ObjectId postID)
        {
            try
            {
                return repository.GetLike(postID);
            }
            catch
            {
                return 0;
            }

        }

        public int GetDislikes(ObjectId postID)
        {
            try
            {
                return repository.GetDislike(postID);
            }
            catch
            {
                return 0;
            }

        }
        //
        public bool AddComment(string text, ObjectId postId)
        {

            Comment comment = new Comment();
            comment.Text = text;
            comment.CommentOwnerId = userRepository.GetUser(userServices.NickNameRead()).Id;
            comment.Date = DateTime.Now.ToString();
            try
            {
                repository.AddComment(comment, postId);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool DeleteComment(string text, ObjectId postId)
        {

            Comment comment = new Comment();
            comment.Text = text;
            comment.CommentOwnerId = userRepository.GetUser(userServices.NickNameRead()).Id;
            comment.Date = DateTime.Now.ToString();
            try
            {
                repository.DeleteComment(comment, postId);
                return true;
            }
            catch
            {
                return false;
            }

        }
        //
        public List<Post> GetNewPosts(string TimeOfLastUserLogin, List<string> following)
        {
            List<ObjectId> ids = new List<ObjectId>();
            if (following != null)
            {
                foreach (var el in following)
                {
                    ids.Add(userRepository.GetUserId(el));
                }
                return repository.GetNewPosts(TimeOfLastUserLogin, ids);
            }

            return new List<Post>();

        }

        public List<Post> GetPosts(string nickname)
        {
            List<Post> posts = new List<Post>();
            try
            {
                posts = repository.GetPosts(userRepository.GetUserId(nickname));
                return posts;
            }
            catch
            {
                return posts;
            }

        }

        public Post GetPost(ObjectId postId)
        {
            Post post = new Post();
            try
            {
                post = repository.GetPost(postId);
                return post;
            }
            catch
            {
                return post;
            }

        }
        //
        public List<string> GetPersonWhoComment(ObjectId postId)
        {
            List<Comment> comments = new List<Comment>();
            try
            {
                comments = repository.GetComments(postId);
                if (comments != null)
                {
                    List<string> res = new List<string>();
                    foreach (var el in comments)
                    {
                        res.Add(userRepository.GetUser(el.CommentOwnerId).NickName + "\n" + el.Text + "\n" + el.Date);

                    }
                    return res;
                }
            }
            catch
            {
                return new List<string>();
            }
            return new List<string>();
        }

        public List<string> GetPersonsWhoLiked(ObjectId postId)
        {
            List<string> ls = new List<string>();
            try
            {
                ls = repository.GetPersonsWhoLiked(postId);
                return ls;
            }
            catch
            {
                return ls;
            }

        }

        public List<string> GetPersonsWhoDisLiked(ObjectId postId)
        {
            List<string> ls = new List<string>();
            try
            {
                ls = repository.GetPersonsWhoDisLiked(postId);
                return ls;
            }
            catch
            {
                return ls;
            }

        }
        //


    }


}
