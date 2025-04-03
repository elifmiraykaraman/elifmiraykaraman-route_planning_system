using GMap.NET;
using Rota_Planlama_sistemi;
using System.Collections.Generic;

public class RotaSecenek
{
    public string Aciklama { get; set; }
    public decimal Maliyet { get; set; }
    public int Sure { get; set; }
    public int AktarmaSayisi { get; set; }
    public List<PointLatLng> YolKoordinatlari { get; set; } = new List<PointLatLng>();

    // Her rota seçeneği içindeki "adımlar" (otobüs, tramvay, yürüyüş vb.) listesi:
    public List<RotaAdim> RotaAdimlari { get; set; } = new List<RotaAdim>();
}

