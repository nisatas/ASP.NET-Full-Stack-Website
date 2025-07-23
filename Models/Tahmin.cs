using System;

namespace BeygirMuhendisi.Web.Models
{
    public class Tahmin
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public string? Baslik { get; set; }
        public string? Pist { get; set; }
        public string? Gun { get; set; }
        public string? Tur { get; set; }
        public string? Aciklama { get; set; }
        public string? SheetsUrl { get; set; }
        public bool Aktif { get; set; }
        public bool TutanTahmin { get; set; }
    }
}