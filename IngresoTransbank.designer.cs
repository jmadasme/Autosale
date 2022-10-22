namespace WindowsFormsApplication1
{
    partial class IngresoTransbank
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IngresoTransbank));
            this.lblMEnsaje = new System.Windows.Forms.Label();
            this.lblPrecio = new System.Windows.Forms.Label();
            this.lblProducto = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox23 = new System.Windows.Forms.PictureBox();
            this.lblatatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox23)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMEnsaje
            // 
            this.lblMEnsaje.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMEnsaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMEnsaje.ForeColor = System.Drawing.Color.Red;
            this.lblMEnsaje.Location = new System.Drawing.Point(165, 197);
            this.lblMEnsaje.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMEnsaje.Name = "lblMEnsaje";
            this.lblMEnsaje.Size = new System.Drawing.Size(435, 39);
            this.lblMEnsaje.TabIndex = 5;
            this.lblMEnsaje.Text = "Resultado Transaccion";
            this.lblMEnsaje.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMEnsaje.Click += new System.EventHandler(this.lblMEnsaje_Click);
            // 
            // lblPrecio
            // 
            this.lblPrecio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPrecio.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrecio.ForeColor = System.Drawing.Color.Navy;
            this.lblPrecio.Location = new System.Drawing.Point(348, 129);
            this.lblPrecio.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new System.Drawing.Size(343, 39);
            this.lblPrecio.TabIndex = 4;
            this.lblPrecio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPrecio.Click += new System.EventHandler(this.lblPrecio_Click);
            // 
            // lblProducto
            // 
            this.lblProducto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblProducto.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.875F);
            this.lblProducto.Location = new System.Drawing.Point(146, 53);
            this.lblProducto.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(450, 39);
            this.lblProducto.TabIndex = 3;
            this.lblProducto.Text = "PAGO CON TRANSBANK";
            this.lblProducto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(107, 123);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 39);
            this.label1.TabIndex = 39;
            this.label1.Text = "TOTAL";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox23
            // 
            this.pictureBox23.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox23.Image")));
            this.pictureBox23.Location = new System.Drawing.Point(276, 123);
            this.pictureBox23.Name = "pictureBox23";
            this.pictureBox23.Size = new System.Drawing.Size(46, 45);
            this.pictureBox23.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox23.TabIndex = 38;
            this.pictureBox23.TabStop = false;
            // 
            // lblatatus
            // 
            this.lblatatus.BackColor = System.Drawing.Color.Yellow;
            this.lblatatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblatatus.Location = new System.Drawing.Point(133, 296);
            this.lblatatus.Name = "lblatatus";
            this.lblatatus.Size = new System.Drawing.Size(500, 50);
            this.lblatatus.TabIndex = 41;
            // 
            // IngresoTransbank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(751, 438);
            this.ControlBox = false;
            this.Controls.Add(this.lblatatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox23);
            this.Controls.Add(this.lblMEnsaje);
            this.Controls.Add(this.lblPrecio);
            this.Controls.Add(this.lblProducto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "IngresoTransbank";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IngresoTransbank";
            this.Load += new System.EventHandler(this.IngresoTransbank_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox23)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMEnsaje;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox23;
        private System.Windows.Forms.Label lblatatus;
    }
}