using DAL.Enteties;
using DAL.Repository;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using MongoDB.Bson;

using DAL.Neo4JRepository;

namespace DAL.Services
{



    public class UserServices
    {
        UserRepository repository;
        GraphRepository graphRepository;
        public UserServices()
        {
            repository = new UserRepository();
            graphRepository = new GraphRepository();
        }
        //
        public bool CheckPassword(string nickname, string password)
        {

            User user = new User();
            user = repository.GetUser(nickname);
            if (user != null)
            {
                if (user.Password == GetHashStringSHA256(password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public string GetHashStringSHA256(string str)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] hashPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
            string result = "";
            foreach (byte b in hashPassword)
            {
                result += b.ToString();
            }
            return result;
        }

        public bool CheckIndentityOfNickName(string nickname)
        {
            List<User> users = new List<User>();
            users = repository.GetUsers();
            foreach (var elem in users)
            {
                if (elem.NickName == nickname)
                {
                    return false;
                }
            }

            return true;
        }
        //
        public void NickNameWrite(string NickName)
        {
            var p = new NickInfo();

            p.NickName = NickName;
            if (p != null)
            {
                using (FileStream fs = new FileStream("NickInfo.json", FileMode.Create))
                {
                    DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(NickInfo));
                    jsonFormatter.WriteObject(fs, p);
                }
            }
            else
            {
                using (FileStream fs = new FileStream("NickInfo.json", FileMode.Create))
                {
                    p = new NickInfo { NickName = "" };
                    DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(NickInfo));
                    jsonFormatter.WriteObject(fs, p);
                }
            }


        }

        public string NickNameRead()
        {
            var p = new NickInfo();
            using (FileStream fs = new FileStream("NickInfo.json", FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(NickInfo));
                if (fs.Length != 0)
                {
                    p = (NickInfo)jsonFormatter.ReadObject(fs);
                }

            }

            return p.NickName;
        }
        //
        public bool CheckAlreadyFollow(string nickname, string usernickname)
        {
            User user = new User();
            user = repository.GetUser(nickname);
            if (user != null && user.Following != null)
            {
                foreach (var el in user.Following)
                {
                    if (el == usernickname)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        public bool CheckIsUserInDatabase(string nickname)
        {
            User user = new User();
            user = repository.GetUser(nickname);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //
        public bool UpdatePassword(string oldpasssword, string newpassword)
        {
            if (CheckPassword(NickNameRead(), (oldpasssword)))
            {
                repository.UpdateField(NickNameRead(), "Password", GetHashStringSHA256(newpassword));
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool UpdateField(string nickname, string fieldToEdit, string fieldValue)
        {
            try
            {
                repository.UpdateField(nickname, fieldToEdit, fieldValue);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public void UpdateDate()
        {
            try
            {
                repository.UpdateDate(NickNameRead());
            }
            catch
            {

            }

        }
        //
        public void InsertUser(string usName, string usSurname, string usMail, string usNickName, string usPassword)
        {
            User user = new User();
            user.Name = usName;
            user.Surname = usSurname;
            user.Mail = usMail;
            user.NickName = usNickName;
            user.Password = GetHashStringSHA256(usPassword);
            DateTime date1 = new DateTime();
            user.Date = date1.ToString();
            repository.Add(user);

            graphRepository.CreatePerson(new Person()
            {
                Surname = usSurname,
                Name = usName,
                Mail = usMail,
                NickName = usNickName
            });
        }
        //
        public User GetUser()
        {
            try
            {
                return repository.GetUser(NickNameRead());
            }
            catch
            {
                return new User();
            }

        }

        public ObjectId GetUserId()
        {
            try
            {
                return repository.GetUserId(NickNameRead());
            }
            catch
            {
                return new ObjectId();
            }

        }

        public User GetUser(string nickname)
        {
            try
            {
                return repository.GetUser(nickname);
            }
            catch
            {
                return new User();
            }

        }

        public User GetUser(ObjectId id)
        {
            try
            {
                return repository.GetUser(id);
            }
            catch
            {
                return new User();
            }

        }
        //
        public List<string> GetFollowers()
        {
            List<string> ls = new List<string>();
            try
            {
                ls = repository.GetFollowers(NickNameRead());
                return ls;
            }
            catch
            {
                return ls;
            }
        }

        public List<string> GetFollowing()
        {
            List<string> ls = new List<string>();
            try
            {
                ls = repository.GetFollowing(NickNameRead());
                return ls;
            }
            catch
            {
                return ls;
            }
        }
        //

        public List<Person> GetFriendsOfFriend()
        {
            List<Person> res = new List<Person>();
            User user = new User();

            user = GetUser(NickNameRead());

            var people = graphRepository.FriendsOfAFriend(new Person()
            {

                Surname = user.Surname,
                Name = user.Name,
                Mail = user.Mail,
                NickName = user.NickName
            });

            foreach (var elem in people)
            {
                bool temp = true;
                foreach (var el in res)
                {
                    if (el.NickName == elem.NickName)
                    {
                        temp = false;
                    }
                }
                if (temp)
                {
                    res.Add(elem);
                }
            }

            return res;

        }

        public List<string> GetConnectingPaths(string nickname)
        {
            try
            {
                List<string> res = new List<string>();

                User user1 = new User();
                user1 = GetUser(NickNameRead());

                User user2 = new User();
                user2 = GetUser(nickname);

                var temp = graphRepository.ConnectingPaths(new Person()
                {
                    Surname = user1.Surname,
                    Name = user1.Name,
                    NickName = user1.NickName,
                    Mail = user1.Mail
                },
                new Person()
                {
                    Surname = user2.Surname,
                    Name = user2.Name,
                    NickName = user2.NickName,
                    Mail = user2.Mail
                });

                foreach (var elem in temp)
                {
                    res.Add(elem);
                }

                return res;
            }
            catch
            {
                return new List<string>();
            }
        }

        public string GetConnectingPathsNumber(string nickname)
        {
            try
            {
                List<string> res = new List<string>();

                User user1 = new User();
                user1 = GetUser(NickNameRead());

                User user2 = new User();
                user2 = GetUser(nickname);

                var temp = graphRepository.ConnectingPaths(new Person()
                {
                    Surname = user1.Surname,
                    Name = user1.Name,
                    NickName = user1.NickName,
                    Mail = user1.Mail
                },
                new Person()
                {
                    Surname = user2.Surname,
                    Name = user2.Name,
                    NickName = user2.NickName,
                    Mail = user2.Mail
                });

                foreach (var elem in temp)
                {
                    res.Add(elem);
                }
                if (res.Count == 0)
                {
                    return "No connection";
                }
                else if (res.Count == 2)
                {
                    return "following";
                }
                else if (res.Count - 1 > 1)
                {
                    return "Connection : " + (res.Count - 1).ToString();
                }
                else
                {
                    return "No connection";
                }
            }
            catch (Exception e)
            {
                return " ";
            }
        }

        public void AddFollower(string nickname, string newFollower)
        {
            repository.addFollower(nickname, newFollower);
        }

        public void UnFollow(string nickname, string follower)
        {
            repository.unFollow(nickname, follower);
            User user1 = new User();
            user1 = GetUser(nickname);

            User user2 = new User();
            user2 = GetUser(follower);

            graphRepository.DeleteRelationShip(new Person()
            {
                Surname = user1.Surname,
                Name = user1.Name,
                Mail = user1.Mail,
                NickName = user1.NickName
            }, new Person()
            {
                Surname = user2.Surname,
                Name = user2.Name,
                NickName = user2.NickName,
                Mail = user2.Mail
            });
        }

        public void AddFollowing(string nickname, string newFollowing)
        {
            repository.addFollowing(nickname, newFollowing);

            User user1 = new User();
            user1 = GetUser(nickname);

            User user2 = new User();
            user2 = GetUser(newFollowing);

            graphRepository.CreatRelationShip(new Person()
            {
                Surname = user1.Surname,
                Name = user1.Name,
                Mail = user1.Mail,
                NickName = user1.NickName
            }, new Person()
            {
                Surname = user2.Surname,
                Name = user2.Name,
                NickName = user2.NickName,
                Mail = user2.Mail
            });
        }

        public List<string> GetCommonFriends(string nickname)
        {
            try
            {
                User user1 = new User();
                user1 = GetUser(NickNameRead());

                User user2 = new User();
                user2 = GetUser(nickname);

                var temp = graphRepository.CommonFriends(new Person()
                {
                    Surname = user1.Surname,
                    Name = user1.Name,
                    Mail = user1.Mail,
                    NickName = user1.NickName
                }, new Person()
                {
                    Surname = user2.Surname,
                    Name = user2.Name,
                    NickName = user2.NickName,
                    Mail = user2.Mail
                });

                List<string> res = new List<string>();


                foreach (var elem in temp)
                {
                    bool t = true;
                    foreach (var el in res)
                    {
                        if (el == elem.NickName)
                        {
                            t = false;
                        }
                    }
                    if (t)
                    {
                        res.Add(elem.NickName);
                    }
                }

                return res;
            }
            catch (Exception er)
            {
                return new List<string>();
            }

        }
    }
}
