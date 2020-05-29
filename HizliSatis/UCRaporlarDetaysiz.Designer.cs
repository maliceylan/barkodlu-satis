namespace HizliSatis
{
    partial class UCRaporlarDetaysiz
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCRaporlarDetaysiz));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnRaporDetaysizUCKapat = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblheader = new System.Windows.Forms.Label();
            this.btnSatisRaporExcel = new System.Windows.Forms.Button();
            this.tblDetaysizRapor = new System.Windows.Forms.DataGridView();
            this.urunAdi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.satisMiktari = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.satisFiyati = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toplamTutar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblDetaysizRapor)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRaporDetaysizUCKapat
            // 
            this.btnRaporDetaysizUCKapat.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnRaporDetaysizUCKapat.Image = ((System.Drawing.Image)(resources.GetObject("btnRaporDetaysizUCKapat.Image")));
            this.btnRaporDetaysizUCKapat.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRaporDetaysizUCKapat.Location = new System.Drawing.Point(540, 478);
            this.btnRaporDetaysizUCKapat.Name = "btnRaporDetaysizUCKapat";
            this.btnRaporDetaysizUCKapat.Size = new System.Drawing.Size(160, 50);
            this.btnRaporDetaysizUCKapat.TabIndex = 3;
            this.btnRaporDetaysizUCKapat.Text = "Kapat!";
            this.btnRaporDetaysizUCKapat.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRaporDetaysizUCKapat.UseVisualStyleBackColor = true;
            this.btnRaporDetaysizUCKapat.Click += new System.EventHandler(this.btnRaporDetaysizUCKapat_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SkyBlue;
            this.panel1.Controls.Add(this.lblheader);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(708, 42);
            this.panel1.TabIndex = 5;
            // 
            // lblheader
            // 
            this.lblheader.AutoSize = true;
            this.lblheader.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblheader.Location = new System.Drawing.Point(121, 4);
            this.lblheader.Name = "lblheader";
            this.lblheader.Size = new System.Drawing.Size(455, 34);
            this.lblheader.TabIndex = 0;
            this.lblheader.Text = "Ürün Bazlı Toplam Satış Raporu";
            // 
            // btnSatisRaporExcel
            // 
            this.btnSatisRaporExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSatisRaporExcel.Image = global::HizliSatis.Properties.Resources.excel;
            this.btnSatisRaporExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSatisRaporExcel.Location = new System.Drawing.Point(13, 478);
            this.btnSatisRaporExcel.Name = "btnSatisRaporExcel";
            this.btnSatisRaporExcel.Size = new System.Drawing.Size(295, 50);
            this.btnSatisRaporExcel.TabIndex = 71;
            this.btnSatisRaporExcel.Text = "Tabloyu Excel\'e Aktar";
            this.btnSatisRaporExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSatisRaporExcel.UseVisualStyleBackColor = true;
            this.btnSatisRaporExcel.Click += new System.EventHandler(this.btnSatisRaporExcel_Click);
            // 
            // tblDetaysizRapor
            // 
            this.tblDetaysizRapor.AllowUserToAddRows = false;
            this.tblDetaysizRapor.AllowUserToDeleteRows = false;
            this.tblDetaysizRapor.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tblDetaysizRapor.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tblDetaysizRapor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tblDetaysizRapor.BackgroundColor = System.Drawing.SystemColors.Control;
            this.tblDetaysizRapor.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tblDetaysizRapor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tblDetaysizRapor.ColumnHeadersHeight = 30;
            this.tblDetaysizRapor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.tblDetaysizRapor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.urunAdi,
            this.satisMiktari,
            this.satisFiyati,
            this.toplamTutar});
            this.tblDetaysizRapor.Location = new System.Drawing.Point(13, 61);
            this.tblDetaysizRapor.Name = "tblDetaysizRapor";
            this.tblDetaysizRapor.ReadOnly = true;
            this.tblDetaysizRapor.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.tblDetaysizRapor.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.tblDetaysizRapor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tblDetaysizRapor.Size = new System.Drawing.Size(687, 411);
            this.tblDetaysizRapor.TabIndex = 72;
            // 
            // urunAdi
            // 
            this.urunAdi.HeaderText = "Ürün Adı";
            this.urunAdi.Name = "urunAdi";
            this.urunAdi.ReadOnly = true;
            // 
            // satisMiktari
            // 
            this.satisMiktari.HeaderText = "Satış Miktarı";
            this.satisMiktari.Name = "satisMiktari";
            this.satisMiktari.ReadOnly = true;
            // 
            // satisFiyati
            // 
            this.satisFiyati.HeaderText = "Satış Fiyatı";
            this.satisFiyati.Name = "satisFiyati";
            this.satisFiyati.ReadOnly = true;
            // 
            // toplamTutar
            // 
            this.toplamTutar.HeaderText = "Toplam Tutar";
            this.toplamTutar.Name = "toplamTutar";
            this.toplamTutar.ReadOnly = true;
            // 
            // UCRaporlarDetaysiz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(49)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tblDetaysizRapor);
            this.Controls.Add(this.btnSatisRaporExcel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnRaporDetaysizUCKapat);
            this.Name = "UCRaporlarDetaysiz";
            this.Size = new System.Drawing.Size(706, 533);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblDetaysizRapor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRaporDetaysizUCKapat;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblheader;
        private System.Windows.Forms.Button btnSatisRaporExcel;
        private System.Windows.Forms.DataGridView tblDetaysizRapor;
        private System.Windows.Forms.DataGridViewTextBoxColumn urunAdi;
        private System.Windows.Forms.DataGridViewTextBoxColumn satisMiktari;
        private System.Windows.Forms.DataGridViewTextBoxColumn satisFiyati;
        private System.Windows.Forms.DataGridViewTextBoxColumn toplamTutar;
    }
}
