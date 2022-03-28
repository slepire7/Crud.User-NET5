using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class UserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PassWord { get; set; }
    }
    public class UserView
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
