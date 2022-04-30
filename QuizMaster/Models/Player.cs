using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class Player
    {
        public Player()
        {
            this.Questions = 100;
            this.Rank = "Bronze";
            this.Score = 0;
        }
        public int Id { get; set; }
        [Required]
        [Display(Name = "Score")]
        public int Score { get; set; }
        [Required]
        [Display(Name = "Questions")]
        public int Questions { get; set; }
        [Required]
        [Display(Name = "Rank")]
        public string Rank { get; set; }        
        
    }
}
