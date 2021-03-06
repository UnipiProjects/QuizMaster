using QuizMaster.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.ViewModels
{
    public class PlayerViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Score")]
        public int Score { get; set; }
        [Required]
        [Display(Name = "Questions")]
        public int Questions { get; set; }
        [Required]
        [Display(Name = "Rank")]
        public string Rank { get; set; }
        public Question Question { get; set; }
        [Required]
        [StringLength(100)]
        public string InputAnswer { get; set; }

    }
}
