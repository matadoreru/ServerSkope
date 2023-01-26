using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSkope
{
    public class Chat
    {
        int idChat;
        User user1r;
        User user2n;
        List<Message> chatMsgs;

        public Chat(int idChat, User user1r, User user2n, List<Message> chatMsgs)
        {
            this.IdChat = idChat;
            this.User1r = user1r;
            this.User2n = user2n;
            this.ChatMsgs = chatMsgs;
        }

        public int IdChat { get => idChat; set => idChat = value; }
        public User User1r { get => user1r; set => user1r = value; }
        public User User2n { get => user2n; set => user2n = value; }
        public List<Message> ChatMsgs { get => chatMsgs; set => chatMsgs = value; }

        public void AfegirMissatge(String txtMsg, User sender)
        {
            Message msgAdd = new Message(txtMsg, sender, 1);
            ChatMsgs.Add(msgAdd);
        }

        public override bool Equals(object? obj)
        {
            return obj is Chat chat &&
                   IdChat == chat.IdChat;
        }
    }
}
