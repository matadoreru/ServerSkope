using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSkope
{
    public class Skope
    {
        List<User> users;
        List<String> usernames;
        int nextChat = 0;
        public List<User> Users { get => users; set => users = value; }
        public List<string> Usernames { get => usernames; set => usernames = value; }
        public int NextChat { get => nextChat; set => nextChat = value; }

        public Skope()
        {
            Users = new List<User>();
            Usernames = new List<string>();
        }

        public bool RegistrarUsuari(User userAdd)
        {
            bool resultat = false;
            if(!Users.Contains(userAdd))
            {
                resultat = true;
                Users.Add(userAdd);
                Usernames.Add(userAdd.Username);
            }
            return resultat;
        }

        public bool LogIn(String username, String password)
        {
            bool resultat = false;
            if (Usernames.Contains(username))
            {
                User userData = Users[Usernames.IndexOf(username)];
                if (userData.Password == password)
                    resultat = true;
            }
            return resultat;
        }

        public bool EnviarSolicitud(String actualUsername, String addUser)
        {
            bool resultat = false;
            User actualAddUser = GetUser(addUser);
            User actualAddUsername = GetUser(actualUsername);
            if (actualAddUser != null && actualAddUsername != null)
            {
                if(!actualAddUser.Friends.Contains(actualAddUsername) && !actualAddUser.FriendRequest.Contains(actualAddUsername)) {
                    Users[Usernames.IndexOf(actualUsername)].FriendRequest.Add(actualAddUser);
                    Users[Usernames.IndexOf(addUser)].FriendRequest.Add(actualAddUsername);
                    resultat = true;
                }
            }
            return resultat;
        }

        public bool EnviarMissatge(String usernameEnviar, String usernameReceive, String message)
        {
            bool resultat = false;
            User localUser = GetUser(usernameEnviar);
            User sendUser = GetUser(usernameReceive);
            if (localUser != null && sendUser != null)
            {
                foreach (Chat chat in localUser.Chats)
                {
                    if ((chat.User1r == localUser || chat.User1r == sendUser) && (chat.User2n == localUser || chat.User2n == sendUser))
                    {
                        chat.AfegirMissatge(message, localUser);
                        break;
                    }
                }

                resultat = true;
            }
            return resultat;
        }

        public bool AceptarSolicitud(string? userName, string? aceptUser)
        {
            bool resultat = false;
            User user1 = GetUser(userName);
            User user2 = GetUser(aceptUser);

            if(user1 != null && user2 != null)
            {
                resultat = true;
                user1.Friends.Add(user2);
                user2.Friends.Add(user1);
                user1.FriendRequest.Remove(user2);
                user2.FriendRequest.Remove(user1);             
            }
            return resultat;
        }

        internal User GetUser(String username)
        {
            User resultat = null;
            if (Usernames.Contains(username))
            {
                resultat = Users[Usernames.IndexOf(username)];
            }
            return resultat;
        }
    }
}
