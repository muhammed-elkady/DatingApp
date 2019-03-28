using DatingApp.Core.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.Core.Entities
{
    public class Message
    {
        public int Id { get; set; }

        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }

        public string RecipientId { get; set; }
        public ApplicationUser Recipient { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime? DateRead { get; set; }

        public DateTime MessageSentDate { get; set; }

        public bool SenderDeletedMessage { get; set; }

        public bool RecipientDeletedMessage { get; set; }

    }
}
