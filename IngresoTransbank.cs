using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace WindowsFormsApplication1
{
    public partial class IngresoTransbank : Form
    {
        string cnstr = @"Data Source=.\SQLEXPRESS;Initial Catalog=DB;Integrated Security = True";
        string PuertoCOM = "";//lector tarjeta transbank
        string _CodigoProducto;
        string _precio;
        string _NombreProducto;
//==========Variables GCR agregados=================
        string _PortName;
        string _Monto;
        string _ticket;
        string datorx = "";
        char ack = System.Convert.ToChar(0x06);
        char stx = System.Convert.ToChar(0x02);
        char eox = System.Convert.ToChar(0x03);
        char separador = System.Convert.ToChar(0x7C);
        int contador_trama = 0;
        int cant_bytes = 0;
        string tramaRX1 = ""; //richTextBox2
        string tramaRX2 = ""; //richTextBox3
        string tramaRX3 = ""; //richTextBox4
        string tramaRX4 = ""; //richTextBox5
        string tramaRX5 = ""; //richTextBox6
        string tramaRX6 = ""; //richTextBox7
        string tramaRX7 = ""; //richTextBox8
        string tramaRX8 = ""; //richTextBox9
        string tramaRX9 = ""; //richTextBox10
        string tramaRX10 = ""; //richTextBox11
        int _largoRX;
        string formato_voucher = "";// textBox2.Text = "------ TRAMA RECIBIDA COMPLETA -------";   // Para imprimir voucher
//================variable solo para mostrar=============================
        string procesoRX = ""; //textBox3.Text = "Acciones de Cliente"; // proceso en curso de la comunicacion POS  // Para mostrar GCR
        string result_codigoRX = "";//textBox6.Text = "Solicita confirmar Monto.......";                            // Para mostrar GCR
        string ver_pos_stx = ""; //textBox9.Text = string.Concat(pos_stx);  // Para mostrar GCR
        string ver_pos_eox = ""; //textBox10.Text = string.Concat(pos_eox);  // Para mostrar GCR
        string ver_pos_ult_sep = ""; //textBox11.Text = string.Concat(pos_ult_sep);  // Para mostrar GCR
        string ver_pos_largoRX = ""; //textBox5.Text = largoRX;  // Para mostrar GCR
        string ver_comercioRX = "";   //         textBox7.Text = comercioRX;  // Para mostrar GCR
        string ver_terminalRX = "";   //         textBox8.Text = terminalRX;  // Para mostrar GCR
        string ver_contador_trama = ""; //textBox4.Text = "Cant.TRAMAS :" + contador_trama;  // Para mostrar GCR
//======================================================================================

//=============Fin variables GCR agregadas =====================
        SerialPort _serialPort;
        public string _voucherRX;
        public string _mensajeRetorno;

        private delegate void SetTextDeleg(string text);
        private System.Windows.Forms.Timer timer;
        private int timeoutAutocerradoForm = 120000; //milisegundos
        string _ClearData = "";
        string _OriginalData = "";
        
        Dictionary<string, string> CodigosRespuesta = new Dictionary<string, string>(); 

        public IngresoTransbank(string Puerto)
        {
            InitializeComponent();
            PuertoCOM = Puerto;
            _serialPort = new SerialPort(PuertoCOM, 115200, Parity.None, 8, StopBits.One);
            _serialPort.Handshake = Handshake.None;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceivedTransbank);
            _serialPort.ReadTimeout = 120000;
            _serialPort.Close();
           // if (!_serialPort.IsOpen) _serialPort.Open();

        }

        private void CloseForm(object sender, EventArgs e)
        {
            timer.Stop();
            timer.Dispose();
            _serialPort.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        public IngresoTransbank(string monto, string _nboleta,string Puerto)
        {
            InitializeComponent();

            //Para configurar tie out
            timer = new System.Windows.Forms.Timer();
            timer.Interval = timeoutAutocerradoForm;
            timer.Tick += CloseForm;
            timer.Start();

            _Monto = monto;
            _ticket = _nboleta;


            PuertoCOM = Puerto;
           _serialPort = new SerialPort(PuertoCOM, 115200, Parity.None, 8, StopBits.One);
            _serialPort.Handshake = Handshake.None;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceivedTransbank);
            _serialPort.ReadTimeout = 120000;
            _serialPort.Close();
       
        }
        private void IngresoTransbank_Load(object sender, EventArgs e)
        {
    
            try
            {
                if (!_serialPort.IsOpen) _serialPort.Open();
                if (!poolling())
                {
                    MessageBox.Show("POS TRANSBANK Desconectado"); _serialPort.Close();

                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }

                else
                {
                    if (!_serialPort.IsOpen) _serialPort.Open();
                    _serialPort.DiscardOutBuffer();
    //=======================Orden de Pago Transbank ====================================
                    char stx = System.Convert.ToChar(0x02);
                    char eox = System.Convert.ToChar(0x03);
                    char separador = System.Convert.ToChar(0x7C);
                    char sumar = System.Convert.ToChar(0x00);
                    lblPrecio.Text = _Monto;
                    String Comando_Venta;
                    //0200l35800l002380l1l1
                    Comando_Venta = string.Concat("0200", separador, _Monto, separador, _ticket, separador, "1", separador, "0", eox);
                    //===========Checksum==XOR========================
                    for (int i = 0; i < Comando_Venta.Length; i++)
                        {
                            sumar ^= System.Convert.ToChar(Comando_Venta[i]);
                        }
                    //========================================
                    _serialPort.Write(string.Concat(stx, Comando_Venta, sumar));
                }
                //====================================================================================
            }           
            catch (Exception ex)
            {
                MessageBox.Show( "Error Catch - IngresoTransbank_Load No se puedo enviar la información (Port)" + ex.Message, "Error!");
            }
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

       bool ChequeoPooling ;


      public  Boolean poolling()
        {
            ChequeoPooling = false;
            if (!_serialPort.IsOpen) _serialPort.Open();
            try
            {
                char stx = System.Convert.ToChar(0x02);
                char eox = System.Convert.ToChar(0x03);
                char separador = System.Convert.ToChar(0x7C);
                char sumar = System.Convert.ToChar(0x00);
                String Comando;
                //0200l35800l002380l1l1
                Comando = string.Concat("0100", eox);
                //===========Checksum==XOR========================

                for (int i = 0; i < Comando.Length; i++)
                {
                    sumar ^= System.Convert.ToChar(Comando[i]);
                }
                //========================================
                _serialPort.Write(string.Concat(stx, Comando, sumar));

                //txtTx.Clear();
                DateTime dt = DateTime.Now + TimeSpan.FromSeconds(1);
                do { } while (DateTime.Now < dt);
                _serialPort.Close();
                return (ChequeoPooling);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se puedo enviar la información", "Error Catch pooling", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (false);
            }
        }

        void sp_DataReceivedTransbank(object sender, SerialDataReceivedEventArgs e)
        {
//============ Recepcion de Trama ============================
//========================RECIBIR TRAMA MODO 1================================
//          datorx = serialPort1.ReadExisting();   ///////
//========================RECIBIR TRAMA MODO 2 OK ================================
            int bytes = _serialPort.BytesToRead;
            byte[] buffer = new byte[bytes];
            _serialPort.Read(buffer, 0, bytes);
            var str = System.Text.Encoding.Default.GetString(buffer);
            datorx = str;
            cant_bytes = bytes;
            contador_trama++;
                 if (contador_trama == 1) { tramaRX1 = datorx; }// textBox1.Text = ":" + cant_bytes; }
            else if (contador_trama == 2) { tramaRX2 = datorx; }// textBox13.Text = ":" + cant_bytes; }
            else if (contador_trama == 3) { tramaRX3 = datorx; }// textBox14.Text = ":" + cant_bytes; }
            else if (contador_trama == 4) { tramaRX4 = datorx; }// textBox15.Text = ":" + cant_bytes; }
            else if (contador_trama == 5) { tramaRX5 = datorx; }// textBox16.Text = ":" + cant_bytes; }
            else if (contador_trama == 6) { tramaRX6 = datorx; }// textBox17.Text = ":" + cant_bytes; }
            else if (contador_trama == 7) { tramaRX7 = datorx; }// textBox18.Text = ":" + cant_bytes; }
            else if (contador_trama == 8) { tramaRX8 = datorx; }// textBox19.Text = ":" + cant_bytes; }
            else if (contador_trama == 9) { tramaRX9 = datorx; }// textBox20.Text = ":" + cant_bytes; }
            else if (contador_trama == 10){ tramaRX10 = datorx; }// textBox21.Text = ":" + cant_bytes; }

            ver_contador_trama = "Cant.TRAMAS :" + contador_trama;

            if (datorx[0] == 0x06)
                {
                    procesoRX = "ACK recibido desde Terminal"; contador_trama = 0; //textBox1.Text = procesoRX;
                ChequeoPooling = true;
                return;
                }

            if (datorx[0] == stx)
                {
                    _serialPort.Write(string.Concat(ack));
                    procesoRX = "ACK enviado a Terminal"; //textBox1.Text = procesoRX;
            }
            if (datorx.Contains(string.Concat(eox)))
                {

                    procesoRX = "Fin de trama " + contador_trama; //textBox1.Text = procesoRX;
                if (contador_trama == 1)  datorx = tramaRX1;// richTextBox2.Text;
                    else if (contador_trama == 2)  datorx = tramaRX1 + tramaRX2;//richTextBox2.Text + richTextBox3.Text;
                    else if (contador_trama == 3)  datorx = tramaRX1 + tramaRX2 + tramaRX3;//;//richTextBox2.Text + richTextBox3.Text + richTextBox4.Text;
                    else if (contador_trama == 4)  datorx = tramaRX1 + tramaRX2 + tramaRX3 + tramaRX4;//;//;//richTextBox2.Text + richTextBox3.Text + richTextBox4.Text + richTextBox5.Text;
                    else if (contador_trama == 5)  datorx = tramaRX1 + tramaRX2 + tramaRX3 + tramaRX4 + tramaRX5;//;//;//;//richTextBox2.Text + richTextBox3.Text + richTextBox4.Text + richTextBox5.Text + richTextBox6.Text;
                    else if (contador_trama == 6)  datorx = tramaRX1 + tramaRX2 + tramaRX3 + tramaRX4 + tramaRX5 + tramaRX6;//;//;//;//;//richTextBox2.Text + richTextBox3.Text + richTextBox4.Text + richTextBox5.Text + richTextBox6.Text + richTextBox7.Text;
                    else if (contador_trama == 7)  datorx = tramaRX1 + tramaRX2 + tramaRX3 + tramaRX4 + tramaRX5 + tramaRX6 + tramaRX7;//;//;//;//;//;//richTextBox2.Text + richTextBox3.Text + richTextBox4.Text + richTextBox5.Text + richTextBox6.Text + richTextBox7.Text + richTextBox8.Text;
                    else if (contador_trama == 8)  datorx = tramaRX1 + tramaRX2 + tramaRX3 + tramaRX4 + tramaRX5 + tramaRX6 + tramaRX7 + tramaRX8;//;//;//;//;//;//;//richTextBox2.Text + richTextBox3.Text + richTextBox4.Text + richTextBox5.Text + richTextBox6.Text + richTextBox7.Text + richTextBox8.Text + richTextBox9.Text;
                    else if (contador_trama == 9)  datorx = tramaRX1 + tramaRX2 + tramaRX3 + tramaRX4 + tramaRX5 + tramaRX6 + tramaRX7 + tramaRX8 + tramaRX9;//;//;//;//;//;//;//;//richTextBox2.Text + richTextBox3.Text + richTextBox4.Text + richTextBox5.Text + richTextBox6.Text + richTextBox7.Text + richTextBox8.Text + richTextBox9.Text + richTextBox10.Text;
                    else if (contador_trama == 10) datorx = tramaRX1 + tramaRX2 + tramaRX3 + tramaRX4 + tramaRX5 + tramaRX6 + tramaRX7 + tramaRX8 + tramaRX9 + tramaRX10;//;//;//;//;//;//;//;//;//richTextBox2.Text + richTextBox3.Text + richTextBox4.Text + richTextBox5.Text + richTextBox6.Text + richTextBox7.Text + richTextBox8.Text + richTextBox9.Text + richTextBox10.Text + richTextBox11.Text;
                    //richTextBox1.Text = datorx;
                }
//====================FIN de Recepcion Trama GCR========================================
            string data = datorx;
            this.BeginInvoke(new SetTextDeleg(si_DataReceived), new object[] { data });
        }

        private void si_DataReceived(string data)
        {
            int isOK = 2; // 0:exito 1:error 2:no hace nada o falta definicion de que hacer en caso de que ocurra (la accion)
            datorx = data;
//==========================INICIO Procesamiento de Trama RX Serial===============================
            _largoRX = datorx.Length;

            if (datorx.Contains("0900|80|"))
                {
                    procesoRX = "Acciones de Cliente"; //textBox1.Text = procesoRX;
                    result_codigoRX = "Solicita confirmar Monto......."; lblatatus.Text = result_codigoRX;
                    return;
                }
            else if (datorx.Contains("0900|81|"))
                {
                    procesoRX = "Acciones de Cliente"; //textBox1.Text = procesoRX;
                    result_codigoRX = "Solicita ingreso de Clave......"; lblatatus.Text = result_codigoRX;
                    return;
                }
            else if (datorx.Contains("0900|82|"))
                {
                    procesoRX = "Acciones de Cliente"; //textBox1.Text = procesoRX;
                result_codigoRX = "Enviando Transaccion..........."; lblatatus.Text = result_codigoRX;
                    return;
                }
            else if (datorx[0] == stx && datorx[_largoRX - 2] == eox)
                {
                    formato_voucher = "------ TRAMA RECIBIDA COMPLETA -------";
                    string comandoRX = "";              //4 dig //pos
                    string resultadoRX = "";            //2 dig //pos
                    string comercioRX = "";             //12 dig //pos
                    string terminalRX = "";             //8 dig //pos
                    string boletaRX = "";               //06 dig //pos
                    string autorizacionRX = "";         //max 6 dig //pos
                    string montolRX = "";               //max 9 dig //pos
                    string numTarjRX = "";             //4 dig //pos
                    string operacionRX = "";            //max 6 dig //pos
                    string tipoTarjRX = "";             //2 dig //pos      credito o debito
                    string fechacontableRX = "";        //6 dig //pos
                    string cuentaRX = "0";               //19 dig //pos
                    string abrevtarjRX = "";            //2 dig //pos
                    string fechaRX = "";                //8 dig //pos
                    string horaRX = "";                 //6 dig //pos
                    string voucherRX = "";              //2 dig //pos
                    string largoRX = "";
                    string encrippanRX = "";
                    int pos_stx = datorx.IndexOf(stx);  //indicar indice de eox
                    int pos_eox = datorx.IndexOf(eox); // 770
                    int pos_ult_sep = datorx.LastIndexOf(separador);  //129
                    string espacioMonto = "                     ";

                    ver_pos_stx = string.Concat(pos_stx);
                    ver_pos_eox = string.Concat(pos_eox);
                    ver_pos_ult_sep = string.Concat(pos_ult_sep);
                    ver_pos_largoRX = largoRX;
                    largoRX = datorx.Length.ToString();
                    comandoRX = datorx.Substring(1, 4);    //Comando//se indica indice inicio,cantidad
                    resultadoRX = datorx.Substring(6, 2);  //Resultado
                //====================== Identificar Codigo de respuesta ===============
                    if (resultadoRX.Contains("03")) { result_codigoRX = "Conexion fallo......."; lblatatus.Text = result_codigoRX; isOK = 1; }
                    if (resultadoRX.Contains("06")) { result_codigoRX = "Tarjeta no soportada......."; lblatatus.Text = result_codigoRX; isOK = 1; }
                    if (resultadoRX.Contains("07")) { result_codigoRX = "Transaccion cancelada ......."; lblatatus.Text = result_codigoRX; isOK = 3; }
                    if (resultadoRX.Contains("09")) { result_codigoRX = "Error Lectura Tarjeta......."; lblatatus.Text = result_codigoRX; isOK = 1; }
                    if (resultadoRX.Contains("10")) { result_codigoRX = "Monto menor al minimo permitido......."; lblatatus.Text = result_codigoRX; isOK = 1; }
                    if (resultadoRX.Contains("12")) { result_codigoRX = "Transaccion no soportada......."; lblatatus.Text = result_codigoRX; isOK = 1; }
                    if (resultadoRX.Contains("14")) { result_codigoRX = "Error Encriptado PAN (BCYCLE......."; lblatatus.Text = result_codigoRX; isOK = 1; }
                    if (resultadoRX.Contains("15")) { result_codigoRX = "Error Operando con Debito......."; lblatatus.Text = result_codigoRX; isOK = 1; }
                    if (resultadoRX.Contains("92")) { result_codigoRX = "Lector no conectado............"; lblatatus.Text = result_codigoRX; isOK = 1; }
                //===============Identificacion de Comando de respuesta==============================================
                    if (datorx.Contains("0410|"))
                        {
                            procesoRX = "Pan Encriptado en proceso"; //textBox1.Text = procesoRX;
                    encrippanRX = datorx.Substring(9, 48);
                            //_nlineas = 2;
                            formato_voucher = string.Concat("PAN Encriptado 3DES : -" + encrippanRX + "-        ");
                            _voucherRX = formato_voucher;//
                            isOK = 2;
                        }
                    if (datorx.Contains("0810|00|"))
                        {
                            //"\u00020810|00|597029414300|70000595\u0003x"
                            result_codigoRX = "Carga de llaves exitosa.........";lblatatus.Text = result_codigoRX;
                            procesoRX = "Carga de Llaves en proceso"; //textBox1.Text = procesoRX;
                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                            terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                                                                    //_nlineas = 3;
                            formato_voucher = string.Concat("------- Carga de Llaves exitosa ------- " +
                                "Codigo Comercio : " + comercioRX + "          " +
                                "ID Terminal : " + terminalRX + "                    ");
                            _voucherRX = formato_voucher;//
                            ver_comercioRX = comercioRX;
                            ver_terminalRX = terminalRX;
                            isOK = 2;
                        }
                    else if (datorx.Contains("0810|02|"))
                        {
                            //"\u00020810|02|597029414300|70000595\u0003z"
                            result_codigoRX = "Carga de llaves fallida.........";lblatatus.Text = result_codigoRX;
                            procesoRX = "Carga de Llaves en proceso"; //textBox1.Text = procesoRX;
                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                            terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                                                                    //_nlineas = 3;
                            formato_voucher = string.Concat("------- Carga de Llaves fallida ------- " +
                                "Codigo Comercio : " + comercioRX + "          " +
                                "ID Terminal : " + terminalRX + "                    ");
                            _voucherRX = formato_voucher;//
                            ver_comercioRX = comercioRX;
                            ver_terminalRX = terminalRX;
                            isOK = 2;
                        }
                    if (datorx.Contains("1080|91|"))
                        {
                            //            {02}1080|91|17042018|131018{ 03}
                            //fallida "\u00021080|91|22032018|171857\u0003y"
                            result_codigoRX = "Inicializacion fallida.........";lblatatus.Text = result_codigoRX;
                            procesoRX = "Respuesta de Inicializacion en proceso"; //textBox1.Text = procesoRX;
                    fechaRX = datorx.Substring(9, 8);               //8 dig //pos
                            horaRX = datorx.Substring(18, 6);                //6 dig //pos
                                                                             //_nlineas = 3;
                            formato_voucher = string.Concat("-------- Inicializacion Fallida --------" +
                            "Fecha de Inicializacion : " + fechaRX + "      " +
                            "Hora de Inicializacion : " + horaRX + "         ");
                            _voucherRX = formato_voucher;//
                            isOK = 2;
                        }
                    else if (datorx.Contains("1080|90|"))
                        {
                            result_codigoRX = "Inicializacion exitosa.........";lblatatus.Text = result_codigoRX;
                            procesoRX = "Respuesta de Inicializacion en proceso"; //textBox1.Text = procesoRX;
                    fechaRX = datorx.Substring(9, 8);                 //8 dig //pos
                            horaRX = datorx.Substring(18, 6);                //6 dig //pos
                                                                             //_nlineas = 3;
                            formato_voucher = string.Concat("-------- Inicializacion Exitosa --------" +
                            "Fecha de Inicializacion : " + fechaRX + "      " +
                            "Hora de Inicializacion : " + horaRX + "         ");
                            _voucherRX = formato_voucher;//
                            isOK = 2;
                        }
                    else if (datorx.Contains("1080|13|"))
                        {
                            result_codigoRX = "Debe ejecutar Cierre.......";lblatatus.Text = result_codigoRX;
                            procesoRX = "Respuesta de Inicializacion en proceso"; //textBox1.Text = procesoRX;
                    fechaRX = datorx.Substring(9, 8);                //8 dig //pos
                            horaRX = datorx.Substring(18, 6);                //6 dig //pos
                                                                             //_nlineas = 3;
                            formato_voucher = string.Concat("------ Inicializacion incompleta -------" +
                            "Fecha de Inicializacion : " + fechaRX + "      " +
                            "Hora de Inicializacion : " + horaRX + "         ");
                            _voucherRX = formato_voucher;//
                            isOK = 2;
                        }
                    if (datorx.Contains("1210|08|"))
                        {
                            //"\u00021210|08|597029414300|70000595|800863|000343\u0003z"
                            result_codigoRX = "No puede Anular Transaccion Debito.......";lblatatus.Text = result_codigoRX;
                            procesoRX = "Anulacion en proceso"; //textBox1.Text = procesoRX;
                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                            terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                            autorizacionRX = datorx.Substring(31, 6);         //max 6 dig //pos
                            operacionRX = datorx.Substring(38, 6);            //max 6 dig //pos
                                                                              //_nlineas = 5;
                            formato_voucher = string.Concat("-- No puede Anular Transaccion Debito --" +
                            "Codigo Comercio : " + comercioRX + "          " +
                            "ID Terminal : " + terminalRX + "                  " +
                            "ID Autorizacion : " + autorizacionRX + "                " +
                            "Operacion : " + operacionRX + "                      ");
                            _voucherRX = formato_voucher;//
                            ver_comercioRX = comercioRX;
                            ver_terminalRX = terminalRX;
                            isOK = 2;
                        }

                    else if (datorx.Contains("1210|05|"))
                        {
                            //"\u00021210|05|597029414300|70000595||000346\u0003w"
                            result_codigoRX = "No existe Transaccion para anular.......";lblatatus.Text = result_codigoRX;
                            procesoRX = "Anulacion en proceso"; //textBox1.Text = procesoRX;
                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                            terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                            autorizacionRX = datorx.Substring(31, 6);         //max 6 dig //pos
                                                                              //========================================================================
                            ver_comercioRX = comercioRX;
                            ver_terminalRX = terminalRX;
                            //======================================================================== 
                            //operacionRX = datorx.Substring(32, 6);            //max 6 dig //pos
                            //_nlineas = 3;
                            formato_voucher = string.Concat("-- No existe Transaccion para anular ---" +
                            "Codigo Comercio : " + comercioRX + "          " +
                            "ID Terminal : " + terminalRX + "                  ");
                            _voucherRX = formato_voucher;//
                            isOK = 2;
                        }
                    else if (datorx.Contains("1210|04|"))
                        {
                            //"\u00021210|05|597029414300|70000595||000346\u0003w"
                            result_codigoRX = "Transaccion ya fue anulada.......";lblatatus.Text = result_codigoRX;
                            procesoRX = "Anulacion en proceso"; //textBox1.Text = procesoRX;
                            comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                            terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                            autorizacionRX = datorx.Substring(31, 6);         //max 6 dig //pos
                                                                              //========================================================================
                            ver_comercioRX = comercioRX;
                            ver_terminalRX = terminalRX;
                            //======================================================================== 
                            //operacionRX = datorx.Substring(32, 6);            //max 6 dig //pos
                            //_nlineas = 3;
                            formato_voucher = string.Concat("-- No existe Transaccion para anular ---" +
                            "Codigo Comercio : " + comercioRX + "          " +
                            "ID Terminal : " + terminalRX + "                  ");
                            _voucherRX = formato_voucher;//
                            isOK = 2;
                        }
                //Agregar  1210 00  GCR
                    else if (datorx.Contains("1210|00|"))
                        {
                            //       1210|00|597029414300|70000595|197627|000662}
                            //"\u00021210|05|597029414300|70000595||000346\u0003w"
                            result_codigoRX = "Anulacion Transaccion Venta exitosa.......";
                            procesoRX = "Anulacion en proceso";
                            comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                            terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                            autorizacionRX = datorx.Substring(31, 6);         //max 6 dig //pos
                            operacionRX = datorx.Substring(38, 6);            //max 6 dig //pos
                                                                              //========================================================================
                            ver_comercioRX = comercioRX;
                            ver_terminalRX = terminalRX;
                            //======================================================================== 
                            //_nlineas = 3;
                            formato_voucher = string.Concat("------ Anulacion ult Venta exitosa -----" +
                            "Codigo Comercio : " + comercioRX + "          " +
                            "ID Terminal : " + terminalRX + "                  " +
                            "N° Autorizacion Anulada : " + autorizacionRX + "        " +
                            "N° Operacion : " + operacionRX + "                   ");
                            _voucherRX = formato_voucher;//
                        }
                    if (datorx.Contains("0510|00|") && datorx.Contains("TERMINAL"))
                        {
                            //"\u00020510|00|597029414300|70000595|
                            //REPORTE DE CIERRE DEL TERMINAL
                            //Huerfanos 770 Piso 8
                            //Santiago
                            //597029414300   -RS 15.12.1       
                            //FECHA            HORA           TERMINAL
                            //21 /03/18        17:40:49        70000595                                                         
                            //
                            //NUMERO            TOTAL
                            //VISA               000               $ 0
                            //AMEX               000               $ 0
                            //MASTERCARD         000               $ 0
                            //DINERS             000               $ 0
                            //DEBITO             000               $ 0
                            //----------------------------------------
                            //TOTAL CAPTURAS     000               $ 0
                            //\u0003\f"
                            result_codigoRX = "Cierre Terminal con Voucher";lblatatus.Text = result_codigoRX;
                            procesoRX = "Cierre en proceso"; //textBox1.Text = procesoRX;
                    voucherRX = datorx.Substring(pos_ult_sep + 1, (pos_eox - 1) - pos_ult_sep);
                            comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                            terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                            formato_voucher = voucherRX;
                            ver_comercioRX = comercioRX;
                            ver_terminalRX = terminalRX;
                            //_nlineas = 15;
                            _voucherRX = formato_voucher;// voucherRX;  //voucher formateado
                            isOK = 0;
                        }
                    else if (datorx.Contains("0510|00|"))
                        {
                            result_codigoRX = "Cierre Terminal sin Voucher";lblatatus.Text = result_codigoRX;
                            procesoRX = "Cierre en proceso"; //textBox1.Text = procesoRX;
                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                            terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                            ver_comercioRX = comercioRX;
                            ver_terminalRX = terminalRX;
                            //_nlineas = 3;
                            formato_voucher = string.Concat("---------- Cierre sin voucher ----------" +
                            "Codigo Comercio : " + comercioRX + "          " +
                            "ID Terminal : " + terminalRX + "                  ");
                            _voucherRX = formato_voucher;//
                            isOK = 0;
                        }
                //=========================Cierre rechazado=======================
                // {02}0510|02|597029414300|{03}y
                //================================================================
                    else if (datorx.Contains("0510|02|"))
                        {
                            result_codigoRX = "Cierre Terminal sin Voucher";lblatatus.Text = result_codigoRX;
                            procesoRX = "Cierre en proceso"; //textBox1.Text = procesoRX;
                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                            ver_comercioRX = comercioRX;
                            //_nlineas = 3;
                            formato_voucher = string.Concat("----------- Cierre Rechazado -----------" +
                            "Codigo Comercio : " + comercioRX + "          ");// +
                                                                              //"ID Terminal : " + terminalRX + "                  ");
                            _voucherRX = formato_voucher;//
                            isOK = 2;
                        }

                    if (datorx.Contains("0210|00|") && datorx.Contains("TERMINAL"))
                        {
                            procesoRX = "Venta en proceso"; //textBox1.Text = procesoRX;
                    result_codigoRX = "Transaccion exitosa con Voucher...........";lblatatus.Text = result_codigoRX;
                            voucherRX = datorx.Substring(pos_ult_sep + 1, (pos_eox - 1) - pos_ult_sep);   //
                            comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                            terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                            boletaRX = datorx.Substring(31, 20);               //06 dig ????  //pos 
                            autorizacionRX = datorx.Substring(52, 6);         //max 6 dig //pos
                            montolRX = datorx.Substring(59, 9);               //max 9 dig //pos
                            numTarjRX = datorx.Substring(69, 4);             //4 dig //pos
                            operacionRX = datorx.Substring(74, 6);            //max 6 dig //pos
                            tipoTarjRX = datorx.Substring(81, 2);             //2 dig //pos      credito o debito
                            fechacontableRX = datorx.Substring(84, 6);        //6 dig //pos
                                                                              //_nlineas = 16;
                            formato_voucher = voucherRX;
                            _voucherRX = formato_voucher;// 
                            ver_comercioRX = comercioRX;
                            ver_terminalRX = terminalRX;
                            isOK = 0;
                        }
                    else if (datorx.Contains("0210|01|"))
                        {
                            result_codigoRX = "Transaccion Rechazada............";lblatatus.Text = result_codigoRX;
                            //_nlineas = 1;
                            formato_voucher = "-------- Transaccion Rechazada ---------";
                            _voucherRX = formato_voucher;//
                            isOK = 4;
                        }
                    else if (datorx.Contains("0210|02|"))
                        {
                            result_codigoRX = "Transaccion Fallo............";lblatatus.Text = result_codigoRX;
                            //_nlineas = 1;
                            formato_voucher = "------- Autorizador no responde --------";
                            _voucherRX = formato_voucher;//
                            isOK = 5;
                        }

                    if (datorx.Contains("0260|00|") && datorx.Contains("TERMINAL"))
                        {
                            procesoRX = "Ultima Venta en proceso con Voucher"; //textBox1.Text = procesoRX;
                    voucherRX = datorx.Substring(pos_ult_sep + 1, (pos_eox - 1) - pos_ult_sep);   //
                            comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                            terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                            boletaRX = datorx.Substring(31, 20);               //06 dig ????  //pos 
                            autorizacionRX = datorx.Substring(52, 6);         //max 6 dig //pos
                            montolRX = datorx.Substring(59, 9);               //max 9 dig //pos
                            numTarjRX = datorx.Substring(69, 4);             //4 dig //pos
                            operacionRX = datorx.Substring(74, 6);            //max 6 dig //pos
                            tipoTarjRX = datorx.Substring(81, 2);             //2 dig //pos      credito o debito
                            fechacontableRX = datorx.Substring(84, 6);        //6 dig //pos
                            ver_comercioRX = comercioRX;
                            ver_terminalRX = terminalRX;
                            //_nlineas = 16;
                            formato_voucher = voucherRX;
                            _voucherRX = formato_voucher;//_voucherRX = voucherRX;
                            isOK = 0;
                        }
                    else if (datorx.Contains("0260|11|"))
                        {
                            // Despues de un cierre  "\u00020260|11|597029414300|70000595\u0003u"
                            result_codigoRX = "No existe venta.......";lblatatus.Text = result_codigoRX;
                            procesoRX = "Ultima Venta en proceso"; //textBox1.Text = procesoRX;
                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                            terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                            ver_comercioRX = comercioRX;
                            ver_terminalRX = terminalRX;
                            //_nlineas = 3;
                            voucherRX = string.Concat("----------- No existe venta ------------" +
                            "Codigo Comercio : " + comercioRX + "          " +
                            "ID Terminal : " + terminalRX + "                  ");
                            formato_voucher = voucherRX;
                            _voucherRX = formato_voucher;//
                            isOK = 2;
                        }
                    else if (datorx.Contains("0260|00|"))
                        {
//Solo Debito "\u00020260|00|597029414300|70000595|00201803211140157524|800863|000001300|8881|000343|DB|000000|3000000000000000000|VI|21032018|113821|0\u0003Y"
//                {02}0260|00|597029414300|70000595|00000000000000002380|311973|000035800|3331|000570|DB|000000|                   |DB|17042018|132922|0{03}\
                            comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                            terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                            boletaRX = datorx.Substring(31, 20);               //06 dig ????  //pos 
                            autorizacionRX = datorx.Substring(52, 6);         //max 6 dig //pos
                            montolRX = datorx.Substring(59, 9);               //max 9 dig //pos
                            numTarjRX = datorx.Substring(69, 4);             //4 dig //pos
                            operacionRX = datorx.Substring(74, 6);            //max 6 dig //pos
                            tipoTarjRX = datorx.Substring(81, 2);             //2 dig //pos      credito o debito
                            if (tipoTarjRX == "DB")
                                {
                                    procesoRX = "Ultima Venta en proceso"; //textBox1.Text = procesoRX;
                        result_codigoRX = "Ultima Venta Debito en sin voucher";lblatatus.Text = result_codigoRX;
                                    abrevtarjRX = datorx.Substring(111, 2);            //2 dig //pos
                                    fechaRX = datorx.Substring(114, 8);                //8 dig //pos
                                    horaRX = datorx.Substring(123, 6);                 //6 dig //pos
                                                                                       //_nlineas = 4;
                                    ver_comercioRX = comercioRX;
                                    ver_terminalRX = terminalRX;
                                    //_nlineas = 6;
                                    espacioMonto = "                     ";
                                    formato_voucher = string.Concat("---- Ult. Venta Debito sin Voucher -----" +
                                    "Codigo Comercio : " + comercioRX + "          " +
                                    "ID Terminal : " + terminalRX + "                  " +
                                    "ID Autorizacion: " + autorizacionRX + "                " +
                                     "Operacion : " + operacionRX + "                      " +
                                     "Boleta : " + boletaRX + "           " +
                                     "Monto : " + espacioMonto + "$ " + montolRX);
                                    _voucherRX = formato_voucher;//
                                    isOK = 2;
                                }
                            else if (tipoTarjRX == "CR")
                                {
                                    //Credito     "\u00020260|00|597029414300|70000595|00201803211622202533|429956|000000750|9480|000344|CR|||MC|21032018|162025|0\u0003k"  
                                    //               {02}0260|00|597029414300|70000595|00000000000000002380|408834|000035800|9480|000569|CR|||MC|17042018|131310|0{03}i
                                    procesoRX = "Ultima Venta en proceso"; //textBox1.Text = procesoRX;
                        result_codigoRX = "Ultima Venta Credito sin voucher";lblatatus.Text = result_codigoRX;
                                    abrevtarjRX = datorx.Substring(86, 2);            //2 dig //pos
                                    fechaRX = datorx.Substring(89, 8);                //8 dig //pos
                                    horaRX = datorx.Substring(98, 6);                 //6 dig //pos
                                    ver_comercioRX = comercioRX;
                                    ver_terminalRX = terminalRX;
                                    //_nlineas = 6;
                                    espacioMonto = "                     ";
                                    formato_voucher = string.Concat("---- Ult. Venta Credito sin Voucher ----" +
                                    "Codigo Comercio : " + comercioRX + "          " +
                                    "ID Terminal : " + terminalRX + "                  " +
                                    "ID Autorizacion : " + autorizacionRX + "                " +
                                     "Operacion : " + operacionRX + "                      " +
                                     "Boleta : " + boletaRX + "           " +
                                     "Monto : " + espacioMonto + "$ " + montolRX);
                                    _voucherRX = formato_voucher;//
                                    isOK = 2;
                                }
                        }
//                    else
//                        {
//                        }
                //======================FIN DE PROCESAMIENTO DE TRAMA RECIBIDA=====================================
                    switch (isOK)
                    {
                        case 0:
                            InsertaTrx(comercioRX, terminalRX, boletaRX, autorizacionRX, montolRX, numTarjRX, operacionRX, tipoTarjRX, fechacontableRX, horaRX, cuentaRX, voucherRX);
                            _serialPort.Close();
                            _voucherRX = voucherRX;
                        var mensaje = new mensajesPantalla("TRANSACCION EXITOSA");
                        mensaje.ShowDialog();
                        mensaje.Close();
                        this.DialogResult = DialogResult.OK;
                            break;
                        case 1:
                            _serialPort.Close();
                        mensaje = new mensajesPantalla(result_codigoRX);
                        mensaje.ShowDialog();
                        mensaje.Close();
                        this.DialogResult = DialogResult.Cancel;
                            break;
                        case 3:
                            _serialPort.Close();

                        mensaje = new mensajesPantalla("ERROR DE USUARIO - TRANSACCION CANCELADA");
                        mensaje.ShowDialog();
                        mensaje.Close();

                        this.DialogResult = DialogResult.Cancel;
                            break;
                    case 4:
                        _serialPort.Close();

                        mensaje = new mensajesPantalla("TRANSACCION RECHAZADA");
                        mensaje.ShowDialog();
                        mensaje.Close();

                        this.DialogResult = DialogResult.Cancel;
                        break;
                    case 5:
                        _serialPort.Close();

                        mensaje = new mensajesPantalla("TRANSACCION FALLÓ");
                        mensaje.ShowDialog();
                        mensaje.Close();

                        this.DialogResult = DialogResult.Cancel;
                        break;
                    case 2:
                            _serialPort.DiscardInBuffer();
                            break;


                    }
                }
            else
                {
                    //_nlineas = 1;
                    formato_voucher = "------ TRAMA RECIBIDA INCOMPLETA -------";
                    _voucherRX = formato_voucher;//
                    isOK = 2;
                }


        }
        //==============================Fin Funcion analisis de trama ===============================
        private void InsertaTrx(string comercioRX, string terminalRX, string boletaRX,
            string vautorizacionRX, string montolRX, string numTarjRX, string operacionRX,
            string tipoTarjRX, string fechacontableRX, string horaRX, string cuentaRX, string voucherRX)
        {
            SqlConnection connection = new SqlConnection(cnstr.ToString());
            connection.Open();
            //   SqlCommand command1 = new SqlCommand("select top 1 imagen from [Productos] where CodigoProducto=" + IDProducto, connection);
            SqlCommand sqlCmd = new SqlCommand(@"Insertatrx   @comercioRX , @terminalRX, @boletaRX,@vautorizacionRX,@montolRX, @numTarjRX,@operacionRX,@tipoTarjRX,
                                                @fechacontableRX,
                                                @horaRX,
                                                @cuentaRX,
                                                @voucherRX", connection);
            sqlCmd.Parameters.Add(new SqlParameter("@comercioRX", comercioRX.ToString().Trim()));
            sqlCmd.Parameters.Add(new SqlParameter("@terminalRX", terminalRX.ToString().Trim()));
            sqlCmd.Parameters.Add(new SqlParameter("@boletaRX", boletaRX.ToString().Trim()));
            sqlCmd.Parameters.Add(new SqlParameter("@vautorizacionRX", vautorizacionRX.ToString().Trim()));
            sqlCmd.Parameters.Add(new SqlParameter("@montolRX", montolRX.ToString().Trim()));
            sqlCmd.Parameters.Add(new SqlParameter("@numTarjRX", numTarjRX.ToString().Trim()));
            sqlCmd.Parameters.Add(new SqlParameter("@operacionRX", operacionRX.ToString().Trim()));
            sqlCmd.Parameters.Add(new SqlParameter("@tipoTarjRX", tipoTarjRX.ToString().Trim()));
            sqlCmd.Parameters.Add(new SqlParameter("@fechacontableRX", fechacontableRX.ToString().Trim()));
            sqlCmd.Parameters.Add(new SqlParameter("@horaRX", horaRX.ToString().Trim()));
            sqlCmd.Parameters.Add(new SqlParameter("@cuentaRX", cuentaRX.ToString().Trim()));
            sqlCmd.Parameters.Add(new SqlParameter("@voucherRX", voucherRX.ToString()));

            sqlCmd.ExecuteNonQuery();
            connection.Close();
        }


        public string ClearData
        {
            get
            {
                return _ClearData;
            }
            set
            {
                _ClearData = value;
            }
        }

        public string OriginalData
        {
            get
            {
                return _OriginalData;
            }
            set
            {
                _OriginalData = value;
            }
        }

        private void lblPrecio_Click(object sender, EventArgs e)
        {

        }

        private void lblMEnsaje_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
