using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Assignment.ViewModels
{
    public class EditUserViewModel
    {
        public void EditRoleViewModel()
        {
            
            Roles = new List<string>();
        }
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required][EmailAddress]
        public string Email { get; set;}

        [Required]       
        public string FirstName { get; set; }

        [Required]        
        public string LastName { get; set; }

        [Required]        
        public string Address { get; set; }

        [Required]        
        public int PostalCode { get; set; }
        
        public IList<string> Roles { get; set; }
    }
}
