using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public User User { get; set; }
    }
}
