using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class Question
    {
        public int id { get; set; }
        public string answer { get; set; }
        public string question { get; set; }
        public int value { get; set; }
        public DateTime airdate { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int category_id { get; set; }
        public object game_id { get; set; }
        public object invalid_count { get; set; }
        public Category category { get; set; }
    }
}
