using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Assignment.ViewModels
{
    public class DiplomaViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Teacher Name")]
        public string TeacherName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }
        public int Status { get; set; }
        [Display(Name = "File")]
        public IFormFile FilePath { get; set; }
    }
}
