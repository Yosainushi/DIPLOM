using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Диплом2.Models;

namespace Диплом2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public AppDBContext db;
        public static AppDBContext db1;
        IWebHostEnvironment _appEnvironment;
        public HomeController(ILogger<HomeController> logger, AppDBContext context, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            db = context;
            db1 = context;
            _appEnvironment = appEnvironment;
        }
       
        public IActionResult Index()
        {
           
            if (User.Identity.IsAuthenticated)
            {
                
                var LTs = db.Letter.ToList();
                User u = db.User.FirstOrDefault(u => u.Email == User.Identity.Name);
                if (u!=null && u.IsWorker==1)
                {
                    List<Letter> C = db.Letter.ToList();
                    ViewData["das"] = C;
                    return View(C);
                }
                else if(u!=null)
                {
                    List<Letter> C = db.Letter.Where(c => c.IdUser == u.Id).ToList();
                    ViewData["das"] = C;
                    return View(C);
                }
                
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Images/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
                db.Files.Add(file);
                db.SaveChanges();
            }

            return RedirectToAction("Ticket");
        }
        public IActionResult Work()
        {
            if (User.Identity.IsAuthenticated)
            {
                
                List<Letter> C = db.Letter.ToList();
                ViewData["ds"] = C;
                return View(C);
            }
            else
            {
                return View();
            }
            
        }
        public static int Proverka(string name)
        {
            var a = db1.User.ToList().Where(s => s.Email == name);
            if (a.First().IsWorker==1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        

        public ActionResult Send(string number, string text)
        {
            if (db.Letter.FirstOrDefault(c => c.Theme == number) == null || db.Letter.ToList().Count == 0)
            {
                var a = db.User.ToList().Where(s => s.Email == User.Identity.Name);
                Letter let = new Letter { Theme = number, Text = text, IdUser = a.First().Id };
                db.Letter.Add(let);
                db.SaveChanges();
                SendCodeAsync(let.Theme.ToString(), let.Text.ToString());
                
                return RedirectToAction("Index");
                //return Content($"{number} - {text}");
            }
            return RedirectToAction("Index");
        }
        public ActionResult Ready(string numberT, string valueR)
        {
            Letter let = db.Letter.First(l => l.Theme == numberT);
            let.Status = valueR;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult InProcess(string numberT, string valueNR )
        {
            Letter let = db.Letter.Where(l => l.Theme == numberT).First();
            let.Status = valueNR;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult SendMail(string number, string text)
        {
            var a = db.User.ToList().Where(s => s.Email == User.Identity.Name);
            db.Letter.Add(new Letter { Theme = number, Text = text, IdUser = a.First().Id });
            db.SaveChanges();
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        
        public async Task <IActionResult> Ticket(string id)
        {
           
            if (id!=null)
            {
                var u = db.User.ToList().Where(s => s.Email == User.Identity.Name);
                Letter C = db.Letter.Where(c => c.Theme == id).First();
                ViewData["ticketTheme"] = C.Theme;
                ViewData["ticketText"] = C.Text;
                
                int idsender = u.First().Id;
                ViewData["userid"] = idsender;
                ViewData["usermail"] = u.First().Email.ToString();
                ViewData["isworker"] = u.First().IsWorker.ToString();
                List <Chat> mails = db.Chat.Where(ch=>ch.IdWorker == int.Parse(C.Theme)).ToList();
                mails.OrderBy(ch => ch.Time);
                return View(mails);
            }
            else
            {
                return View();
            }
            
            
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
        private static async Task SendCodeAsync( string numberTicket, string textmessage)
        {
            MailAddress from = new MailAddress("Yosainushi.miner@gmail.com", "New ticket");
            MailAddress to = new MailAddress("Yosainushi.miner@gmail.com");
            MailMessage mail = new MailMessage(from, to);
            mail.Subject = "Новый тикет";
            var htmlDocument =
@"<!DOCTYPE html>
<html lang='en'>
<head>
<meta charset='UTF-8'>
</head>
<body>
<p> Поступил новый тикет.</p>
                       
                        <p>Номер заказа <b></b>" + numberTicket + @"</p>
<p>Текст <b></b>" + textmessage + @"</p>
</body>
</head>
</html>";



            mail.Body = htmlDocument;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("Yosainushi.miner@gmail.com", "Hentai228");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(mail);
        }
    }
}
