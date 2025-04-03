using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Rota_Planlama_sistemi
{
    partial class Form1
    {
        private IContainer components = null;

        // Mevcut kontroller
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox txtBaslangicLat;
        private TextBox txtBaslangicLon;
        private TextBox txtHedefLat;
        private TextBox txtHedefLon;
        private ComboBox cmbYolcuTipi;
        private Button btnHesapla;
        private ListBox lstSonuc;
        private Button button1;

        // GMapControl
        private GMap.NET.WindowsForms.GMapControl gMapControl1;

        // Yeni eklenen buton alanlarınız:
        private Button btnSecim_Click;
        private Label label6;
        private ComboBox cmbOdeme;
        private ListView lvAlternatifRotalar;
        private ColumnHeader columnAciklama;
        private ColumnHeader columnSure;
        private ColumnHeader columnMaliyet;
        private ColumnHeader columnAktarma;

        /// <summary>
        /// Dispose
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// InitializeComponent
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBaslangicLat = new System.Windows.Forms.TextBox();
            this.txtBaslangicLon = new System.Windows.Forms.TextBox();
            this.txtHedefLat = new System.Windows.Forms.TextBox();
            this.txtHedefLon = new System.Windows.Forms.TextBox();
            this.cmbYolcuTipi = new System.Windows.Forms.ComboBox();
            this.btnHesapla = new System.Windows.Forms.Button();
            this.lstSonuc = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.btnSecim_Click = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbOdeme = new System.Windows.Forms.ComboBox();
            this.lvAlternatifRotalar = new System.Windows.Forms.ListView();
            this.columnAciklama = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSure = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnMaliyet = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnAktarma = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rtbSecilenRota = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Başlangıç Lat:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Başlangıç Lon:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Hedef Lat:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(73, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Hedef Lon:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(75, 255);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Yolcu Tipi:";
            // 
            // txtBaslangicLat
            // 
            this.txtBaslangicLat.Location = new System.Drawing.Point(206, 101);
            this.txtBaslangicLat.Name = "txtBaslangicLat";
            this.txtBaslangicLat.Size = new System.Drawing.Size(125, 22);
            this.txtBaslangicLat.TabIndex = 4;
            // 
            // txtBaslangicLon
            // 
            this.txtBaslangicLon.Location = new System.Drawing.Point(206, 136);
            this.txtBaslangicLon.Name = "txtBaslangicLon";
            this.txtBaslangicLon.Size = new System.Drawing.Size(125, 22);
            this.txtBaslangicLon.TabIndex = 5;
            // 
            // txtHedefLat
            // 
            this.txtHedefLat.Location = new System.Drawing.Point(206, 174);
            this.txtHedefLat.Name = "txtHedefLat";
            this.txtHedefLat.Size = new System.Drawing.Size(125, 22);
            this.txtHedefLat.TabIndex = 6;
            // 
            // txtHedefLon
            // 
            this.txtHedefLon.Location = new System.Drawing.Point(206, 212);
            this.txtHedefLon.Name = "txtHedefLon";
            this.txtHedefLon.Size = new System.Drawing.Size(125, 22);
            this.txtHedefLon.TabIndex = 7;
            // 
            // cmbYolcuTipi
            // 
            this.cmbYolcuTipi.FormattingEnabled = true;
            this.cmbYolcuTipi.Items.AddRange(new object[] {
            "Öğrenci",
            "Yaşlı",
            "Genel"});
            this.cmbYolcuTipi.Location = new System.Drawing.Point(206, 252);
            this.cmbYolcuTipi.Name = "cmbYolcuTipi";
            this.cmbYolcuTipi.Size = new System.Drawing.Size(125, 24);
            this.cmbYolcuTipi.TabIndex = 8;
            // 
            // btnHesapla
            // 
            this.btnHesapla.Location = new System.Drawing.Point(51, 351);
            this.btnHesapla.Name = "btnHesapla";
            this.btnHesapla.Size = new System.Drawing.Size(94, 23);
            this.btnHesapla.TabIndex = 9;
            this.btnHesapla.Text = "Hesapla";
            this.btnHesapla.UseVisualStyleBackColor = true;
            this.btnHesapla.Click += new System.EventHandler(this.btnHesapla_Click);
            // 
            // lstSonuc
            // 
            this.lstSonuc.FormattingEnabled = true;
            this.lstSonuc.ItemHeight = 16;
            this.lstSonuc.Location = new System.Drawing.Point(909, 487);
            this.lstSonuc.Name = "lstSonuc";
            this.lstSonuc.Size = new System.Drawing.Size(260, 212);
            this.lstSonuc.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 82);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // gMapControl1
            // 
            this.gMapControl1.Bearing = 0F;
            this.gMapControl1.CanDragMap = true;
            this.gMapControl1.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMapControl1.GrayScaleMode = false;
            this.gMapControl1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMapControl1.LevelsKeepInMemmory = 5;
            this.gMapControl1.Location = new System.Drawing.Point(431, 72);
            this.gMapControl1.MarkersEnabled = true;
            this.gMapControl1.MaxZoom = 20;
            this.gMapControl1.MinZoom = 1;
            this.gMapControl1.MouseWheelZoomEnabled = true;
            this.gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl1.Name = "gMapControl1";
            this.gMapControl1.NegativeMode = false;
            this.gMapControl1.PolygonsEnabled = true;
            this.gMapControl1.RetryLoadTile = 0;
            this.gMapControl1.RoutesEnabled = true;
            this.gMapControl1.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMapControl1.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapControl1.ShowTileGridLines = false;
            this.gMapControl1.Size = new System.Drawing.Size(673, 400);
            this.gMapControl1.TabIndex = 14;
            this.gMapControl1.Zoom = 12D;
            // 
            // btnSecim_Click
            // 
            this.btnSecim_Click.Location = new System.Drawing.Point(206, 351);
            this.btnSecim_Click.Name = "btnSecim_Click";
            this.btnSecim_Click.Size = new System.Drawing.Size(94, 23);
            this.btnSecim_Click.TabIndex = 15;
            this.btnSecim_Click.Text = "Seçim yap";
            this.btnSecim_Click.UseVisualStyleBackColor = true;
            this.btnSecim_Click.Click += new System.EventHandler(this.btnSecimYap_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label6.Location = new System.Drawing.Point(55, 294);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 20);
            this.label6.TabIndex = 16;
            this.label6.Text = "Ödeme Türü:";
            // 
            // cmbOdeme
            // 
            this.cmbOdeme.FormattingEnabled = true;
            this.cmbOdeme.Items.AddRange(new object[] {
            "Nakit",
            "Kredi Kartı",
            "KentKart"});
            this.cmbOdeme.Location = new System.Drawing.Point(206, 294);
            this.cmbOdeme.Name = "cmbOdeme";
            this.cmbOdeme.Size = new System.Drawing.Size(125, 24);
            this.cmbOdeme.TabIndex = 17;
            // 
            // lvAlternatifRotalar
            // 
            this.lvAlternatifRotalar.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnAciklama,
            this.columnSure,
            this.columnMaliyet,
            this.columnAktarma});
            this.lvAlternatifRotalar.FullRowSelect = true;
            this.lvAlternatifRotalar.HideSelection = false;
            this.lvAlternatifRotalar.Location = new System.Drawing.Point(16, 487);
            this.lvAlternatifRotalar.Name = "lvAlternatifRotalar";
            this.lvAlternatifRotalar.Size = new System.Drawing.Size(407, 212);
            this.lvAlternatifRotalar.TabIndex = 18;
            this.lvAlternatifRotalar.UseCompatibleStateImageBehavior = false;
            this.lvAlternatifRotalar.View = System.Windows.Forms.View.Details;
            // 
            // columnAciklama
            // 
            this.columnAciklama.Text = "Açıklama";
            this.columnAciklama.Width = 120;
            // 
            // columnSure
            // 
            this.columnSure.Text = "Süre (dk)";
            // 
            // columnMaliyet
            // 
            this.columnMaliyet.Text = "Maliyet (TL)";
            this.columnMaliyet.Width = 70;
            // 
            // columnAktarma
            // 
            this.columnAktarma.Text = "Aktarma";
            // 
            // rtbSecilenRota
            // 
            this.rtbSecilenRota.BackColor = System.Drawing.Color.White;
            this.rtbSecilenRota.Location = new System.Drawing.Point(451, 487);
            this.rtbSecilenRota.Name = "rtbSecilenRota";
            this.rtbSecilenRota.ReadOnly = true;
            this.rtbSecilenRota.Size = new System.Drawing.Size(422, 212);
            this.rtbSecilenRota.TabIndex = 19;
            this.rtbSecilenRota.Text = "";
            this.rtbSecilenRota.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 725);
            this.Controls.Add(this.rtbSecilenRota);
            this.Controls.Add(this.lvAlternatifRotalar);
            this.Controls.Add(this.cmbOdeme);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSecim_Click);
            this.Controls.Add(this.gMapControl1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lstSonuc);
            this.Controls.Add(this.btnHesapla);
            this.Controls.Add(this.cmbYolcuTipi);
            this.Controls.Add(this.txtHedefLon);
            this.Controls.Add(this.txtHedefLat);
            this.Controls.Add(this.txtBaslangicLon);
            this.Controls.Add(this.txtBaslangicLat);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RichTextBox rtbSecilenRota;
    }
}
