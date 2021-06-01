using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Диплом2.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string TextLetter { get; set; }
        public int IdWorker { get; set; }
        public int IdSender { get; set; }
        public string Time { get; set; }

    }
}
