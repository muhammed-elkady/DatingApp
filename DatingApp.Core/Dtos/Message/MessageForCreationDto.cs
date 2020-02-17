using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.Dtos.Message
{
    public class MessageForCreationDto
    {
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public DateTime MessageSentDate { get; set; }
        public string Content { get; set; }

        public MessageForCreationDto()
        {
            MessageSentDate = DateTime.Now;
        }
    }
}
