using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SentDate { get; set; }
        public Chat Chat { get; set; }
        [ForeignKey("Chat")]
        public int ChatId { get; set; }
        public Contact Sender { get; set; }
        [ForeignKey("Sender")]
        public int SenderId { get; set; }
    }
}
