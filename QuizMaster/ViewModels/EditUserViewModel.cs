using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.ViewModels
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
        
        public IList<string> Roles { get; set; }
    }
}
