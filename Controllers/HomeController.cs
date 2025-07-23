using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BeygirMuhendisi.Web.Models;
using BeygirMuhendisi.Web.Data;


namespace BeygirMuhendisi.Web.Controllers;

public class HomeController : Controller
{
     private readonly ILogger<HomeController> _logger;
    private readonly UygulamaDbContext _context;

    public HomeController(ILogger<HomeController> logger, UygulamaDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        // Son 5 tahmini çek
        var sonTahminler = _context.Tahminler
            .OrderByDescending(t => t.Tarih)
            .Take(5)
            .ToList();

        var tutanTahminler = _context.Tahminler
            .Where(t => t.TutanTahmin)
            .OrderByDescending(t => t.Tarih)
            .Take(5)
            .ToList();

        ViewBag.SonTahminler = sonTahminler;
        ViewBag.TutanTahminler = tutanTahminler;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // GET: /Home/Iletisim
    public IActionResult Iletisim()
    {
        return View();
    }

    // POST: /Home/Iletisim
    [HttpPost]
    public IActionResult Iletisim(string Ad, string Email, string Mesaj)
    {
        if (!string.IsNullOrEmpty(Ad) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Mesaj))
        {
            try
            {
                var config = HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
                if (config == null)
                {
                    ViewBag.Hata = "Yapılandırma okunamadı!";
                    return View();
                }

                var smtpHost = config["Smtp:Host"] ?? "";
                var smtpPortStr = config["Smtp:Port"];
                var smtpUser = config["Smtp:User"] ?? "";
                var smtpPass = config["Smtp:Pass"] ?? "";
                var smtpSslStr = config["Smtp:EnableSsl"];

                if (string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(smtpPortStr) ||
                    string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPass) || string.IsNullOrWhiteSpace(smtpSslStr))
                {
                    ViewBag.Hata = "SMTP ayarları eksik!";
                    return View();
                }

                int smtpPort = int.TryParse(smtpPortStr, out var port) ? port : 587;
                bool smtpSsl = bool.TryParse(smtpSslStr, out var ssl) ? ssl : true;

                using (var client = new SmtpClient(smtpHost, smtpPort))
                {
                    client.EnableSsl = smtpSsl;
                    client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);

                    var mail = new MailMessage();
                    mail.From = new MailAddress(smtpUser, "Beygir Mühendisi İletişim");
                    mail.To.Add(smtpUser); // Kendine gönder
                    mail.Subject = "İletişim Formu Mesajı";
                    mail.Body = $"Ad: {Ad}\nE-posta: {Email}\nMesaj: {Mesaj}";

                    client.Send(mail);
                }

                ViewBag.Basarili = "Mesajınız başarıyla gönderildi!";
            }
            catch (Exception ex)
            {
                ViewBag.Hata = "Mesaj gönderilemedi: " + ex.Message;
            }
        }
        else
        {
            ViewBag.Hata = "Lütfen tüm alanları doldurun.";
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
