using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Диплом2.Controllers;

namespace Диплом2.Models
{
    public class LettersContext :DbContext
    {
        public DbSet<Letter> Letters { get; set; }
    }
}
