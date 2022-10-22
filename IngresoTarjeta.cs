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
    public partial class IngresoTarjeta : Form
    {
        string PuertoCOM = "COM7";//lector tarjetacliente convenio RFID
        string cnstr = @"Data Source=.\SQLEXPRESS;Initial Catalog=DB;Integrated Security = True";
        string _CodigoProducto;
        string _CodigoCliente;
        string _precio;
        string _NombreProducto;
        decimal _NuevoSaldo = 0;
        SerialPort _serialPort;
        string _NombreCliente = "";
        decimal _SaldoDisponible = 0;
        private delegate void SetTextDeleg(string text);
        private System.Windows.Forms.Timer timer;
        private int timeoutAutocerradoForm = 10000; //milisegundos

        public IngresoTarjeta()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        public IngresoTarjeta(string CodigoProducto, string NombreProducto, string precio)
        {
            InitializeComponent();

            //para configurar timeout
            timer = new System.Windows.Forms.Timer();
            timer.Interval = timeoutAutocerradoForm;
            timer.Tick += CloseForm;
            timer.Start();

            _CodigoProducto = CodigoProducto;
            _NombreProducto = NombreProducto;
            _precio = precio;

        }

        private void CloseForm(object sender, EventArgs e)
        {
            timer.Stop();
            timer.Dispose();
            _serialPort.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void IngresoTarjeta_Load(object sender, EventArgs e)
        {

            lblMEnsaje.Text = "INGRESE SU TARJETA";
            lblPrecio.Text = _precio;
            lblProducto.Text = _NombreProducto;

            //se redondea formulario
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, this.Width, this.Height);
            Region region = new Region(path);
            this.Region = region;


            //sender inicializa puerto
            _serialPort = new SerialPort(PuertoCOM, 9600, Parity.None, 8, StopBits.One);
            _serialPort.Handshake = Handshake.None;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceivedConvenio);
            _serialPort.ReadTimeout = 500;

            try
            {
                if (!_serialPort.IsOpen)
                    _serialPort.Open();
                    _serialPort.DiscardOutBuffer();
                //  _serialPort.Write("SI\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening/writing to serial port :: " + ex.Message, "Error!");
            }


        }


        void sp_DataReceivedConvenio(object sender, SerialDataReceivedEventArgs e)
        {

            Thread.Sleep(500);
            string RxString = _serialPort.ReadExisting();
            string data = RxString.Substring(1, 10);
       //     _serialPort.DiscardInBuffer();
            this.BeginInvoke(new SetTextDeleg(si_DataReceived), new object[] { data });
        }

        private void si_DataReceived(string data)
        {
            lblMEnsaje.Text = "Procesando";
            _CodigoCliente = data.Trim();
            getInformacionCliente();
          
            //timer.Stop();
            //timer.Dispose();
            //_serialPort.Close();
            //this.DialogResult = DialogResult.Cancel;

        }


        public string CodigoCliente
        {
            get
            {
                return _CodigoCliente;
            }
            set
            {
                _CodigoCliente = value;
            }
        }

        public string NombreCliente
        {
            get
            {
                return _NombreCliente;
            }
            set
            {
                _NombreCliente = value;
            }
        }


        public decimal SaldoDisponible
        {
            get
            {
                return _SaldoDisponible;
            }
            set
            {
                _SaldoDisponible = value;
            }
        }

        public decimal NuevoSaldo
        {
            get
            {
                return _NuevoSaldo;
            }
            set
            {
                _NuevoSaldo = value;
            }
        }





        private void getInformacionCliente()
        {



            string precio = _precio.ToString().Trim();

            SqlConnection connection = new SqlConnection(cnstr.ToString());
            connection.Open();
            //   SqlCommand command1 = new SqlCommand("select top 1 imagen from [Productos] where CodigoProducto=" + IDProducto, connection);
            SqlCommand sqlCmd = new SqlCommand("select top 1 RutCliente,NombreCliente,Covenio,Disponible from [Clientes] where CodigoCliente=@CodigoCliente", connection);
            sqlCmd.Parameters.Add(new SqlParameter("@CodigoCliente", _CodigoCliente.ToString().Trim()));
            SqlDataReader dr = sqlCmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    _NombreCliente = dr["NombreCliente"].ToString();
                    _SaldoDisponible = dr.GetDecimal(3);
                }
                dr.Close();
                _NuevoSaldo = _SaldoDisponible - Convert.ToDecimal(precio);

                //abre nueva ventana de despliegue cliente;
                // despliegaInformacionCliente(_NombreProducto, NombreCliente, SaldoDisponible,_precio, _NuevoSaldo);
                finalizar();
                this.DialogResult = DialogResult.OK; //codigo de cliente es valido
            }
            else //cliente no exite
            {
                finalizar();
                this.DialogResult = DialogResult.Cancel;

            }

        }

            private void finalizar()
              {
                    timer.Stop();
                    timer.Dispose();
                    _serialPort.Close();
        }

        private void lblPrecio_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblProducto_Click(object sender, EventArgs e)
        {

        }

        //private void updateSaldoInformacionCliente()
        //{


        //    SqlConnection connection = new SqlConnection(cnstr.ToString());
        //    connection.Open();
        //    //   SqlCommand command1 = new SqlCommand("select top 1 imagen from [Productos] where CodigoProducto=" + IDProducto, connection);
        //    SqlCommand sqlCmd = new SqlCommand("UPDATE  [Clientes] set Disponible=@Saldo where CodigoCliente=@CodigoCliente", connection);
        //    sqlCmd.Parameters.Add(new SqlParameter("@CodigoCliente", _CodigoCliente.ToString().Trim()));
        //    sqlCmd.Parameters.Add(new SqlParameter("@Saldo", Convert.ToDecimal(_NuevoSaldo)));

        //    sqlCmd.ExecuteNonQuery();
        //    connection.Close();
        //}

        //abre informacion de despliegue cliente.

        ////private void despliegaInformacionCliente(string nombreProducto,string nombreCliente,decimal saldo,string precio,decimal nuevoSaldo)
        ////{
        ////    this.Visible = true;
        ////    _serialPort.Close();
        ////    var Confirmacion = new Confirmacion(nombreProducto, nombreCliente, saldo, precio,nuevoSaldo);
        ////    DialogResult dr = Confirmacion.ShowDialog();
        ////    if (dr == DialogResult.Cancel)
        ////    {
        ////        Confirmacion.Close();
        ////        this.Close();
        ////    }
        ////    else if (dr == DialogResult.OK)
        ////    {
        ////        //textBox1.Text = frm2.getText();
        ////        Confirmacion.Close();
        ////        updateSaldoInformacionCliente();
        ////        this.Close();
        ////    }

        ////}
    }
}
