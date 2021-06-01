using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Диплом2.Models
{
    public class Letter
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Укажите номер")]
        [Remote(action: "CheckTheme", controller: "Home", ErrorMessage = "Такой уже есть")]
        public string Theme { get; set; }
        [Required(ErrorMessage = "Укажите текст")]
        public string Text { get; set; }
        public int IdUser { get; set; }
        public string Status { get; set; }
    }
}
