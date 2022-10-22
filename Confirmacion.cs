using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Confirmacion : Form
    {
        string _nombreCliente;
        string _precio;
        string _NombreProducto;
        decimal _saldo;
        decimal _nuevoSaldo;


     
        public Confirmacion()
        {
            InitializeComponent();
        }
        public Confirmacion(string NombreProducto,string nombreCliente,decimal saldo,string precio,decimal nuevoSaldo)
        {
             InitializeComponent();
            _nombreCliente = nombreCliente;
            _NombreProducto = NombreProducto;
            _precio = precio;
            _saldo = saldo;
            _nuevoSaldo = nuevoSaldo;

        }


        private void Convenio_Load(object sender, EventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, this.Width, this.Height);
            Region region = new Region(path);
            this.Region = region;

            despliegaInformacionCliente();



        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
                       
        }


        private void despliegaInformacionCliente()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-CL"); //para representar la moneda
            String.Format("{0:C2}", _saldo);
            lblProducto.Text = _NombreProducto;
            txtCliente.Text = _nombreCliente;
            //txtSaldo.Text = Convert.ToString(_saldo);
            //txtPrecio.Text = _precio.ToString().Trim();
            //txtNuevoSaldo.Text = Convert.ToString( _nuevoSaldo);

            txtSaldo.Text = String.Format("{0:C2}", _saldo);
            txtPrecio.Text = String.Format("{0:C2}", _precio);
            txtNuevoSaldo.Text = String.Format("{0:C2}", _nuevoSaldo);


        }

        private void txtSaldo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNuevoSaldo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCliente_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void lblProducto_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {

        }
    }
}
