public class RotaAdimi
{
    public string Baslangic { get; set; }
    public string Bitis { get; set; }
    public string UlasimTipi { get; set; } // "Otobüs", "Tramvay", "Transfer"
    public int SureDakika { get; set; }
    public double Ucret { get; set; }
    public bool Ogrenci { get; set; }
    public bool OzelGunMu { get; set; }

    public override string ToString()
    {
        string emojiUlasim = "🚏";
        if (UlasimTipi == "Otobüs")
            emojiUlasim = "🚌";
        else if (UlasimTipi == "Tramvay")
            emojiUlasim = "🚋";
        else if (UlasimTipi == "Transfer")
            emojiUlasim = "🔄";

        string ucretBilgisi;
        if (OzelGunMu)
            ucretBilgisi = "0 TL (Özel Gün)";
        else if (Ogrenci)
            ucretBilgisi = $"{Ucret} TL (Öğrenci)";
        else
            ucretBilgisi = $"{Ucret} TL";

        return $"🔹 {Baslangic} → {Bitis} ({emojiUlasim} {UlasimTipi})\n⏳ Süre: {SureDakika} dk\n💰 Ücret: {ucretBilgisi}";
    }
}
