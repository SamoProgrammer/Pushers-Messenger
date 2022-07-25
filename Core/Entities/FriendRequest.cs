using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FriendRequest
    {
        [Key]
        public int Id { get; set; }
        public Contact? ToContact { get; set; }
        [ForeignKey("ToContact")]
        public int? ToContactId { get; set; }

        public Contact? FromContact { get; set; }
        [ForeignKey("FromContact")]
        public int FromContactId { get; set; }
        public string Text { get; set; }
    }
}
