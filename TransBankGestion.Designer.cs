namespace WindowsFormsApplication1
{
    partial class TransBankGestion
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
            this.BtnAnular = new System.Windows.Forms.Button();
            this.BtnCerrar = new System.Windows.Forms.Button();
            this.BtnInicializacion = new System.Windows.Forms.Button();
            this.BtnUltimaVenta = new System.Windows.Forms.Button();
            this.BtnCierreTerminal = new System.Windows.Forms.Button();
            this.BtnCargaLLaves = new System.Windows.Forms.Button();
            this.BtnRespInicializacion = new System.Windows.Forms.Button();
            this.BtnPooling = new System.Windows.Forms.Button();
            this.richTextPanel = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // BtnAnular
            // 
            this.BtnAnular.Location = new System.Drawing.Point(26, 12);
            this.BtnAnular.Name = "BtnAnular";
            this.BtnAnular.Size = new System.Drawing.Size(159, 38);
            this.BtnAnular.TabIndex = 0;
            this.BtnAnular.Text = "Anular Venta";
            this.BtnAnular.UseVisualStyleBackColor = true;
            this.BtnAnular.Click += new System.EventHandler(this.BtnAnular_Click);
            // 
            // BtnCerrar
            // 
            this.BtnCerrar.Location = new System.Drawing.Point(517, 13);
            this.BtnCerrar.Name = "BtnCerrar";
            this.BtnCerrar.Size = new System.Drawing.Size(157, 37);
            this.BtnCerrar.TabIndex = 1;
            this.BtnCerrar.Text = "Volver";
            this.BtnCerrar.UseVisualStyleBackColor = true;
            this.BtnCerrar.Click += new System.EventHandler(this.BtnCerrar_Click);
            // 
            // BtnInicializacion
            // 
            this.BtnInicializacion.Location = new System.Drawing.Point(189, 13);
            this.BtnInicializacion.Name = "BtnInicializacion";
            this.BtnInicializacion.Size = new System.Drawing.Size(159, 38);
            this.BtnInicializacion.TabIndex = 2;
            this.BtnInicializacion.Text = "Inicializacion";
            this.BtnInicializacion.UseVisualStyleBackColor = true;
            this.BtnInicializacion.Click += new System.EventHandler(this.BtnInicializacion_Click);
            // 
            // BtnUltimaVenta
            // 
            this.BtnUltimaVenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnUltimaVenta.Location = new System.Drawing.Point(352, 13);
            this.BtnUltimaVenta.Name = "BtnUltimaVenta";
            this.BtnUltimaVenta.Size = new System.Drawing.Size(159, 38);
            this.BtnUltimaVenta.TabIndex = 27;
            this.BtnUltimaVenta.Text = "Ult. Venta";
            this.BtnUltimaVenta.UseVisualStyleBackColor = true;
            this.BtnUltimaVenta.Click += new System.EventHandler(this.BtnUltimaVenta_Click);
            // 
            // BtnCierreTerminal
            // 
            this.BtnCierreTerminal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCierreTerminal.Location = new System.Drawing.Point(26, 69);
            this.BtnCierreTerminal.Name = "BtnCierreTerminal";
            this.BtnCierreTerminal.Size = new System.Drawing.Size(159, 38);
            this.BtnCierreTerminal.TabIndex = 30;
            this.BtnCierreTerminal.Text = "Cierre Terminal";
            this.BtnCierreTerminal.UseVisualStyleBackColor = true;
            this.BtnCierreTerminal.Click += new System.EventHandler(this.BtnCierreTerminal_Click);
            // 
            // BtnCargaLLaves
            // 
            this.BtnCargaLLaves.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCargaLLaves.Location = new System.Drawing.Point(352, 69);
            this.BtnCargaLLaves.Name = "BtnCargaLLaves";
            this.BtnCargaLLaves.Size = new System.Drawing.Size(159, 38);
            this.BtnCargaLLaves.TabIndex = 31;
            this.BtnCargaLLaves.Text = "Carga de Llaves";
            this.BtnCargaLLaves.UseVisualStyleBackColor = true;
            this.BtnCargaLLaves.Click += new System.EventHandler(this.BtnCargaLLaves_Click);
            // 
            // BtnRespInicializacion
            // 
            this.BtnRespInicializacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnRespInicializacion.Location = new System.Drawing.Point(515, 69);
            this.BtnRespInicializacion.Name = "BtnRespInicializacion";
            this.BtnRespInicializacion.Size = new System.Drawing.Size(159, 38);
            this.BtnRespInicializacion.TabIndex = 32;
            this.BtnRespInicializacion.Text = "Resp.Inicializacion";
            this.BtnRespInicializacion.UseVisualStyleBackColor = true;
            this.BtnRespInicializacion.Click += new System.EventHandler(this.BtnRespInicializacion_Click);
            // 
            // BtnPooling
            // 
            this.BtnPooling.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPooling.Location = new System.Drawing.Point(189, 69);
            this.BtnPooling.Name = "BtnPooling";
            this.BtnPooling.Size = new System.Drawing.Size(159, 38);
            this.BtnPooling.TabIndex = 33;
            this.BtnPooling.Text = "Pooling";
            this.BtnPooling.UseVisualStyleBackColor = true;
            this.BtnPooling.Click += new System.EventHandler(this.BtnPooling_Click);
            // 
            // richTextPanel
            // 
            this.richTextPanel.Location = new System.Drawing.Point(26, 126);
            this.richTextPanel.Name = "richTextPanel";
            this.richTextPanel.Size = new System.Drawing.Size(648, 286);
            this.richTextPanel.TabIndex = 34;
            this.richTextPanel.Text = "";
            // 
            // TransBankGestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 481);
            this.Controls.Add(this.richTextPanel);
            this.Controls.Add(this.BtnPooling);
            this.Controls.Add(this.BtnRespInicializacion);
            this.Controls.Add(this.BtnCargaLLaves);
            this.Controls.Add(this.BtnCierreTerminal);
            this.Controls.Add(this.BtnUltimaVenta);
            this.Controls.Add(this.BtnInicializacion);
            this.Controls.Add(this.BtnCerrar);
            this.Controls.Add(this.BtnAnular);
            this.Name = "TransBankGestion";
            this.Text = "TransBankGestion";
            this.Load += new System.EventHandler(this.TransBankGestion_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnAnular;
        private System.Windows.Forms.Button BtnCerrar;
        private System.Windows.Forms.Button BtnInicializacion;
        private System.Windows.Forms.Button BtnUltimaVenta;
        private System.Windows.Forms.Button BtnCierreTerminal;
        private System.Windows.Forms.Button BtnCargaLLaves;
        private System.Windows.Forms.Button BtnRespInicializacion;
        private System.Windows.Forms.Button BtnPooling;
        private System.Windows.Forms.RichTextBox richTextPanel;
    }
}