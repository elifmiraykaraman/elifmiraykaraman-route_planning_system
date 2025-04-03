using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rota_Planlama_sistemi
{
    public enum RotaModu
    {
        SadeceTaksi,
        SadeceOtobus,
        TramvayOncelikli,
        EnAzAktarmali
        // Diğer modlar...
    }

    public class RotaHesaplayici
    {
        private readonly List<Durak> _durakListesi;
        private readonly decimal _taksiAcilisUcreti;
        private readonly decimal _taksiCostPerKm;

        // Kaç km üstünde taksi zorunlu diyecekseniz 
        private const double TAKSI_ESIK_KM = 3.0;

        public RotaHesaplayici(List<Durak> durakListesi, decimal taksiAcilisUcreti, decimal taksiCostPerKm)
        {
            _durakListesi = durakListesi;
            _taksiAcilisUcreti = taksiAcilisUcreti;
            _taksiCostPerKm = taksiCostPerKm;
        }

        /// <summary>
        /// BaslangicKonum'a en yakın durağı bulur
        /// </summary>
        public Durak EnYakinDurakBul(Konum kullaniciKonum)
        {
            Durak enYakin = null;
            double minMesafe = double.MaxValue;

            foreach (var durak in _durakListesi)
            {
                double mesafe = Konum.MesafeHesapla(kullaniciKonum, durak.Konum);
                if (mesafe < minMesafe)
                {
                    minMesafe = mesafe;
                    enYakin = durak;
                }
            }
            return enYakin;
        }

        /// <summary>
        /// Mesafe > 3 km ise taksi zorunlu sayalım (örnek).
        /// </summary>
        public bool TaksiyiZorunluMu(double mesafe)
        {
            return mesafe > TAKSI_ESIK_KM;
        }

        /// <summary>
        /// Taksi ücreti: acilis + (mesafe*costPerKm), 
        /// sonra yolcuya göre indirim uygula.
        /// </summary>
        public decimal TaksiUcretiHesapla(double mesafe, Yolcu yolcu)
        {
            decimal normalUcret = _taksiAcilisUcreti + (decimal)mesafe * _taksiCostPerKm;
            return yolcu.IndirimliUcret(normalUcret);
        }

        /// <summary>
        /// Basit bir rota bulan örnek metot.
        /// Mevcutta placeholder olarak otobüs/tram vs. koyuldu,
        /// isterseniz geliştirebilirsiniz.
        /// </summary>
        public List<string> RotaBul(Konum baslangicKonum, Konum hedefKonum, Yolcu yolcu, Odeme odeme)
        {
            List<string> ciktilar = new List<string>();
            decimal toplamUcret = 0m;

            // 1) Başlangıç -> en yakın durak
            Durak enYakinBaslangicDurak = EnYakinDurakBul(baslangicKonum);
            double mesafeBaslangic = Konum.MesafeHesapla(baslangicKonum, enYakinBaslangicDurak.Konum);

            if (TaksiyiZorunluMu(mesafeBaslangic))
            {
                decimal taksiUcret = TaksiUcretiHesapla(mesafeBaslangic, yolcu);
                ciktilar.Add($"(1) Başlangıç -> {enYakinBaslangicDurak.id} TAKSİ: {taksiUcret:F2} TL");
                toplamUcret += taksiUcret;
            }
            else
            {
                ciktilar.Add($"(1) Başlangıç -> {enYakinBaslangicDurak.id} YÜRÜME: {mesafeBaslangic:F2} km, 0 TL");
            }

            // 2) Otobüs/Tram - Basit placeholder
            decimal busCost = 5m;
            ciktilar.Add($"(2) Otobüs/Tram placeholder: {busCost:F2} TL");
            toplamUcret += busCost;

            // 3) Hedef durak -> hedef konum
            Durak enYakinHedefDurak = EnYakinDurakBul(hedefKonum);
            double mesafeHedef = Konum.MesafeHesapla(enYakinHedefDurak.Konum, hedefKonum);

            if (TaksiyiZorunluMu(mesafeHedef))
            {
                decimal taksiUcret = TaksiUcretiHesapla(mesafeHedef, yolcu);
                ciktilar.Add($"(3) {enYakinHedefDurak.id} -> Hedef KONUM TAKSİ: {taksiUcret:F2} TL");
                toplamUcret += taksiUcret;
            }
            else
            {
                ciktilar.Add($"(3) {enYakinHedefDurak.id} -> Hedef KONUM YÜRÜME: {mesafeHedef:F2} km, 0 TL");
            }

            decimal odemeTutari = odeme.Ode(toplamUcret);
            ciktilar.Add($"Toplam Ücret (ödemeden önce): {toplamUcret:F2} TL");
            ciktilar.Add($"Ödeme Yöntemi ({odeme.GetType().Name}) -> Nihai Tutar: {odemeTutari:F2} TL");

            return ciktilar;
        }

        /// <summary>
        /// Tüm seçenekleri (taksi, otobüs, tramvay, en az aktarma, yürüyüş) 
        /// topluca döndüren metot.
        /// Kısa mesafede yürüyüş, yoksa taksi/otobüs vs. dinamik oldu.
        /// </summary>
        public List<RotaSecenek> TumSecenekleriGetir(
     Konum basKonum,
     Konum hedefKonum,
     Yolcu yolcu,
     Odeme odeme,
     string rotaModuText)
        {
            var list = new List<RotaSecenek>();

            double tamMesafe = Konum.MesafeHesapla(basKonum, hedefKonum);

            if (tamMesafe <= 3.0)
            {
                double sureDakika = (tamMesafe / 4.0) * 60.0;
                list.Add(new RotaSecenek
                {
                    Aciklama = "Sadece Yürüyerek",
                    Maliyet = 0,
                    Sure = (int)Math.Round(sureDakika),
                    AktarmaSayisi = 0,
                    YolKoordinatlari = new List<PointLatLng>
            {
                new PointLatLng(basKonum.Lat, basKonum.Lon),
                new PointLatLng(hedefKonum.Lat, hedefKonum.Lon)
            },
                    RotaAdimlari = new List<RotaAdim>
            {
                new RotaAdim {
                    BaslangicDurak = "Konum",
                    BitisDurak = "Konum",
                    AracTipi = "Yürüyüş",
                    Sure = (int)Math.Round(sureDakika),
                    Ucret = 0
                }
            }
                });
            }

            void Ekle(string aciklama, double hizKmSaat, decimal ucret, string aracTipi, int aktarma)
            {
                double dakika = (tamMesafe / hizKmSaat) * 60.0;
                int sure = (int)Math.Round(dakika);
                decimal indirimli = yolcu.IndirimliUcret(ucret);

                list.Add(new RotaSecenek
                {
                    Aciklama = aciklama,
                    Maliyet = indirimli,
                    Sure = sure,
                    AktarmaSayisi = aktarma,
                    YolKoordinatlari = new List<PointLatLng>
            {
                new PointLatLng(basKonum.Lat, basKonum.Lon),
                new PointLatLng(hedefKonum.Lat, hedefKonum.Lon)
            },
                    RotaAdimlari = new List<RotaAdim>
            {
                new RotaAdim {
                    BaslangicDurak = "Başlangıç",
                    BitisDurak = "Hedef",
                    AracTipi = aracTipi,
                    Sure = sure,
                    Ucret = indirimli
                }
            }
                });
            }

            if (rotaModuText == "SadeceTaksi")
                Ekle("Sadece Taksi", 30.0, TaksiUcretiHesapla(tamMesafe, yolcu), "Taksi", 0);
            else if (rotaModuText == "SadeceOtobus")
                Ekle("Sadece Otobüs", 25.0, 10m, "Otobüs", 0);
            else if (rotaModuText == "TramvayOncelikli")
                Ekle("Tramvay Öncelikli", 20.0, 12m, "Tramvay", 1);
            else if (rotaModuText == "EnAzAktarmali")
                Ekle("En Az Aktarmalı", 22.0, 15m, "Transfer", 1);
            else if (rotaModuText == "Hepsi")
            {
                Ekle("Sadece Taksi", 30.0, TaksiUcretiHesapla(tamMesafe, yolcu), "Taksi", 0);
                Ekle("Sadece Otobüs", 25.0, 10m, "Otobüs", 0);
                Ekle("Tramvay Öncelikli", 20.0, 12m, "Tramvay", 1);
                Ekle("En Az Aktarmalı", 22.0, 15m, "Transfer", 1);
            }

            return list;
        }


        /// <summary>
        /// Örnek mod bazlı rota bulma (daha basit string cikti).
        /// </summary>
        public List<string> RotaBulAlternatif(Konum baslangicKonum, Konum hedefKonum, Yolcu yolcu, Odeme odeme, RotaModu mod)
        {
            List<string> ciktilar = new List<string>();

            switch (mod)
            {
                case RotaModu.SadeceTaksi:
                    {
                        double tamMesafe = Konum.MesafeHesapla(baslangicKonum, hedefKonum);
                        decimal taksiUcret = TaksiUcretiHesapla(tamMesafe, yolcu);
                        decimal taksiOdeme = odeme.Ode(taksiUcret);

                        // Süre ~30 km/h
                        double dak = (tamMesafe / 30.0) * 60.0;
                        int sure = (int)Math.Round(dak);

                        ciktilar.Add("🚖 SADECE TAKSİ ROTA");
                        ciktilar.Add($"Mesafe: {tamMesafe:F2} km");
                        ciktilar.Add($"Süre: {sure} dk");
                        ciktilar.Add($"Ücret (önce): {taksiUcret:F2} TL");
                        ciktilar.Add($"Ödeme ({odeme.GetType().Name}): {taksiOdeme:F2} TL");
                        break;
                    }
                case RotaModu.SadeceOtobus:
                    {
                        double tamMesafe = Konum.MesafeHesapla(baslangicKonum, hedefKonum);
                        double otobusDakika = (tamMesafe / 25.0) * 60.0;
                        int sureOtobus = (int)Math.Round(otobusDakika);

                        decimal otobusUcret = 10m;
                        otobusUcret = yolcu.IndirimliUcret(otobusUcret);
                        decimal otobusOdeme = odeme.Ode(otobusUcret);

                        ciktilar.Add("🚍 SADECE OTOBÜS ROTA");
                        ciktilar.Add($"Mesafe: {tamMesafe:F2} km");
                        ciktilar.Add($"Süre: {sureOtobus} dk");
                        ciktilar.Add($"Ücret (önce): {otobusUcret:F2} TL");
                        ciktilar.Add($"Ödeme ({odeme.GetType().Name}): {otobusOdeme:F2} TL");
                        break;
                    }
                case RotaModu.TramvayOncelikli:
                    {
                        double tamMesafe = Konum.MesafeHesapla(baslangicKonum, hedefKonum);
                        double tramDakika = (tamMesafe / 20.0) * 60.0;
                        int sureTram = (int)Math.Round(tramDakika);

                        decimal tramUcret = 12m;
                        tramUcret = yolcu.IndirimliUcret(tramUcret);
                        decimal tramOdeme = odeme.Ode(tramUcret);

                        ciktilar.Add("🚋 TRAMVAY ÖNCELİKLİ ROTA");
                        ciktilar.Add($"Mesafe: {tamMesafe:F2} km");
                        ciktilar.Add($"Süre: {sureTram} dk");
                        ciktilar.Add($"Ücret (önce): {tramUcret:F2} TL");
                        ciktilar.Add($"Ödeme ({odeme.GetType().Name}): {tramOdeme:F2} TL");
                        break;
                    }
                case RotaModu.EnAzAktarmali:
                    {
                        double tamMesafe = Konum.MesafeHesapla(baslangicKonum, hedefKonum);
                        // Basit varsayım: 22 km/h 
                        double dak = (tamMesafe / 22.0) * 60.0;
                        int sure = (int)Math.Round(dak);

                        decimal aktarmaUcret = 15m;
                        aktarmaUcret = yolcu.IndirimliUcret(aktarmaUcret);
                        decimal aktarmaOdeme = odeme.Ode(aktarmaUcret);

                        ciktilar.Add("🛑 EN AZ AKTARMALI ROTA");
                        ciktilar.Add($"Mesafe: {tamMesafe:F2} km");
                        ciktilar.Add($"Süre: {sure} dk");
                        ciktilar.Add($"Ücret (önce): {aktarmaUcret:F2} TL");
                        ciktilar.Add($"Ödeme ({odeme.GetType().Name}): {aktarmaOdeme:F2} TL");
                        break;
                    }
                default:
                    ciktilar.Add("Bilinmeyen rota modu.");
                    break;
            }

            return ciktilar;
        }
    }
}
