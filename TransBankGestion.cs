using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Herramientas;
using System.Threading;

namespace WindowsFormsApplication1
{
    delegate void StringArgReturningVoidDelegate(string text);
    public partial class TransBankGestion : Form
    {
    //    TransBank _Tbk = new TransBank("1");
        public TransBankGestion()
        {
            InitializeComponent();

        }

        private void TransBankGestion_Load(object sender, EventArgs e)
        {
         //   _Tbk.OnMensajeEstado += tbk_mensaje;
        }

        private void BtnAnular_Click(object sender, EventArgs e)
        {
            TransBank Tbk = new TransBank("1");
            Tbk.OnMensajeEstado += tbk_mensaje;
            richTextPanel.Text = "Anulando Ultima venta....\n";
            Tbk.EjecutaComando("1200"); //anulacion Venta

         //   richTextPanel.Text = richTextPanel.Text + "Fin comando Ultima venta....\n";
       //     Tbk = null;
        }

        private void tbk_mensaje(object sender, MensajesTbk e)
        {
            // MessageBox.Show(e.Mensaje);
            string x = e.Mensaje.ToString();

  

            this.Invoke((MethodInvoker)delegate ()
            {
                // text = richTextPanel.Text = ;
                richTextPanel.Text = richTextPanel.Text + x + "\n" ;
            });
          //  this.SetText(x);

        }

      

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.richTextPanel.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richTextPanel.Text = text;
            }
        }




        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnInicializacion_Click(object sender, EventArgs e)
        {
          using(TransBank Tbk = new TransBank("1"))
            {        Tbk.OnMensajeEstado += tbk_mensaje;
                richTextPanel.Text = "Inicializacion....\n";
                Tbk.EjecutaComando("0070");
            }
                
         //Inicializacion
                                            //   richTextPanel.Text= richTextPanel.Text+Tbk._voucherRX;
             //   Tbk = null;



                //richTextPanel.Text = "Inicializacion....\n";
                //_Tbk.EjecutaComando("0070"); //Inicializacion
                //                            //   richTextPanel.Text= richTextPanel.Text+Tbk._voucherRX;


            

        }

        private void BtnUltimaVenta_Click(object sender, EventArgs e)
        {
           // try
           // {
           //     TransBank Tbk = new TransBank("1");
           //     Tbk.OnMensajeEstado += tbk_mensaje;
           //     richTextPanel.Text = "Ultima Venta....\n";
           //     Tbk.EjecutaComando("0250");
           ////     Tbk = null;
           // }
           // catch (Exception ex)
           // {
           //     MessageBox.Show("No se puedo enviar la información", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
           // }


            using (TransBank Tbk = new TransBank("1"))
            {
                Tbk.OnMensajeEstado += tbk_mensaje;
                richTextPanel.Text = "Ultima Venta....\n";
                Tbk.EjecutaComando("0250");
            }
        }

        private void BtnCierreTerminal_Click(object sender, EventArgs e)
        {
            try
            {
                TransBank Tbk = new TransBank("1");
                Tbk.OnMensajeEstado += tbk_mensaje;

                Tbk.EjecutaComando("0500");
                Tbk = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se puedo enviar la información", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCargaLLaves_Click(object sender, EventArgs e)
        {
            try
            {
                TransBank Tbk = new TransBank("1");
                Tbk.OnMensajeEstado += tbk_mensaje;

                Tbk.EjecutaComando("0800");
                Tbk = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se puedo enviar la información", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRespInicializacion_Click(object sender, EventArgs e)
        {
            try
            {
                TransBank Tbk = new TransBank("1");
                Tbk.OnMensajeEstado += tbk_mensaje;

                Tbk.EjecutaComando("0080");
                  Tbk = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se puedo enviar la información", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

       

        private void BtnPooling_Click(object sender, EventArgs e)
        {
            try
            {
                TransBank Tbk = new TransBank("1");
                Tbk.OnMensajeEstado += tbk_mensaje;

                Tbk.EjecutaComando("0100");
                //    Tbk = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se puedo enviar la información", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

      
    }
    }

