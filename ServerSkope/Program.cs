using ServerSkope;
using System.Net;
using System.Text;

ListenAsync();
Console.ReadKey();


async static void ListenAsync()
{
    var skope = new Skope();
    HttpListener listener = new HttpListener();
    listener.Prefixes.Add("http://localhost:5111/");
    listener.Start();

    while (true)
    {
        HttpListenerContext context = await listener.GetContextAsync();
        var url = context.Request.RawUrl;
        string msg = "";
        context.Response.StatusCode = (int)HttpStatusCode.OK;

        if (url == null)
        {
            msg = "The URL is null";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
        else
        {
            if (url.StartsWith("/register"))
            {
                var username = context.Request.QueryString.Get("username");
                var password = context.Request.QueryString.Get("password");
                var name = context.Request.QueryString.Get("name");
                var age = Convert.ToInt32(context.Request.QueryString.Get("age"));
                
                User userTemp = new User(username, password, name, age);
                if(skope.RegistrarUsuari(userTemp))
                {
                    msg = userTemp.Username+";"+ userTemp.Password + ";" + userTemp.Name + ";" + userTemp.Age + ";";
                    Console.WriteLine(msg);
                    Console.WriteLine(userTemp.Username, userTemp.Password);
                }
                else
                {
                    msg = "Error";
                    Console.WriteLine(msg);
                }
            }
            else if(url.StartsWith("/llistarUsuaris"))
            {
                foreach(User user in skope.Users)
                {
                    msg += user.Username + ";" + user.Password + ";" + user.Name + ";" + user.Age + ";\n";
                    Console.WriteLine(user.Username);
                }
                msg = msg.Remove(msg.Length - 2);
            }
            else if (url.StartsWith("/llistarFriends"))
            {
                var userName = context.Request.QueryString.Get("userName");

                User u = skope.GetUser(userName);

                foreach (User user in u.Friends)
                {
                    msg += user.Username + ";" + user.Password + ";" + user.Name + ";" + user.Age + ";\n";
                    Console.WriteLine(user.Username);
                }
                if (msg.Length != 0)
                    msg = msg.Remove(msg.Length - 2);
            }
            else if (url.StartsWith("/llistarAmigosRequest"))
            {
                var userName = context.Request.QueryString.Get("userName");

                User u = skope.GetUser(userName);

                foreach (User user in u.FriendRequest)
                {
                    msg += user.Username + ";" + user.Password + ";" + user.Name + ";" + user.Age + ";\n";
                    Console.WriteLine(user.Username);
                }
                if (msg.Length != 0)
                    msg = msg.Remove(msg.Length - 2);
            }

            else if (url.StartsWith("/enviarSolicitud"))
            {
                var userName = context.Request.QueryString.Get("userName");
                var addUser = context.Request.QueryString.Get("addUser");

                if (skope.EnviarSolicitud(userName, addUser))
                {
                    msg = "Correct";
                }
                else
                {
                    msg = "Error";
                }
            }
            else if (url.StartsWith("/enviarMsg"))
            {
                var userName = context.Request.QueryString.Get("localUser");
                var addUser = context.Request.QueryString.Get("sendUser");
                var msgChat = context.Request.QueryString.Get("msg");

                if (skope.EnviarMissatge(userName, addUser, msgChat))
                {
                    msg = "Correct";
                }
                else
                {
                    msg = "Error";
                }
            }

            else if (url.StartsWith("/aceptarSolicitud"))
            {
                var userName = context.Request.QueryString.Get("userName1");
                var addUser = context.Request.QueryString.Get("userName2");

                if (skope.AceptarSolicitud(userName, addUser))
                {
                    msg = "Correct";
                }
                else
                {
                    msg = "Error";
                }
            }
            else if(url.StartsWith("/getChat")) {
                var userName = context.Request.QueryString.Get("userName1");
                var addUser = context.Request.QueryString.Get("userName2");

                bool trobat = false;
                User user1 = skope.GetUser(userName);
                User user2 = skope.GetUser(addUser);

                if(user1 != null && user2 != null)
                {
                    foreach (Chat chat in user1.Chats)
                    {
                        if((chat.User1r == user1 || chat.User1r == user2) && (chat.User2n == user1 || chat.User2n == user2))
                        {
                            trobat = true;
                            foreach(Message message in chat.ChatMsgs)
                            {
                                msg += message.TxtMessage + ";" + message.SendUser.Username + ";" + message.Date + ";\n";
                            }
                            break;
                        }
                    }
                    if(!trobat)
                    {
                        msg = "NEW CHAT";
                        Chat chatAdd = new Chat(skope.NextChat, user1, user2, new List<Message>());
                        user1.Chats.Add(chatAdd);
                        user2.Chats.Add(chatAdd);
                        skope.NextChat = skope.NextChat++;
                    }
                }
            }
            else if (url.StartsWith("/login"))
            {
                var userName = context.Request.QueryString.Get("userName");
                var password = context.Request.QueryString.Get("password");

                if (skope.LogIn(userName, password))
                {
                    msg = "Correct";
                }
                else
                {
                    msg = "Error";
                }
            }
        }
        context.Response.ContentLength64 = Encoding.UTF8.GetByteCount(msg);
        using (Stream s = context.Response.OutputStream)
        using (StreamWriter writer = new StreamWriter(s))
            await writer.WriteAsync(msg);
    }
    listener.Stop();
}
