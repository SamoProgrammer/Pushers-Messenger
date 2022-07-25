using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class CreateGroupCommand
    {
        public List<int> ContactsIds { get; set; }
        public string Title { get; set; }
    }
}
