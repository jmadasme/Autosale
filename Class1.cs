using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO.Ports;
using System.Threading;

namespace Biblioteca
{
    //public class Ptoductos
    // {
    //    public void cargaImagenPicFronDB()
    //    {
    //        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-CL"); //para representar la moneda
    //        lblCodigoProducto.Text = CodigoProducto.ToString().Trim();
    //        SqlConnection connection = new SqlConnection(cnstr.ToString());
    //        connection.Open();
    //        //   SqlCommand command1 = new SqlCommand("select top 1 imagen from [Productos] where CodigoProducto=" + IDProducto, connection);
    //        SqlCommand sqlCmd = new SqlCommand("select top 1 NombreProducto,Precio,imagen from [Productos] where CodigoProducto=@CodigoProducto", connection);
    //        sqlCmd.Parameters.Add(new SqlParameter("@CodigoProducto", CodigoProducto.ToString().Trim()));
    //        //  byte[] img = (byte[])sqlCmd.ExecuteScalar();
    //        byte[] img = null;
    //        SqlDataReader dr = sqlCmd.ExecuteReader();
    //        while (dr.Read())
    //        {

    //            lblNombreProducto.Text = dr["NombreProducto"].ToString();

    //            _precio = dr["Precio"].ToString();
    //            //  lblPrecio.Text =  dr["Precio"].ToString();
    //            lblPrecio.Text = String.Format("{0:C2}", dr["Precio"]);
    //            img = (byte[])dr["imagen"];



    //            MemoryStream ms = new MemoryStream(img);
    //            PictureProducto.Image = Image.FromStream(ms);
    //            HabilitaBotones(true);
    //        }

    //        connection.Close();
    //        dr.Close();
    //    }


    //}

    static class SincronizaVentas
    {

        public static string cnLocal = @"Data Source=.\SQLEXPRESS;Initial Catalog=DB;Integrated Security = True";
        public static string cnRemota = @"";




      //static  public  SincronizaVentas()
      //  {

      //      LeerParametros();


      //  }


        static public void LeerParametros()
        {

            using (SqlConnection connection = new SqlConnection(cnLocal))
            {
                // int employeeID = findEmployeeID();
                try
                {

                    connection.Open();
                    SqlCommand command = new SqlCommand("GetParametros", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    // command.Parameters.Add(new SqlParameter("@EmployeeID", employeeID));
                    command.CommandTimeout = 5;

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                cnRemota = dr["BdRemota"].ToString();

                            }
                        }

                    }
                }
                catch (Exception)
                {
                    if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                }

            }

        }



        static public void SyncProducto(string StrcnOrigen, string StrcnDestino, string nombreTabla)
    {

          

            using (SqlConnection connSource = new SqlConnection(StrcnOrigen))
            {
                using (SqlConnection connDestino = new SqlConnection(StrcnDestino))
                {



                    SqlCommand cmdDestino = new SqlCommand("DELETE FROM " + nombreTabla, connDestino);
                    SqlCommand cmdOrigen = new SqlCommand("SELECT * FROM " + nombreTabla, connSource);

                    SqlBulkCopy bcp = new SqlBulkCopy(connDestino);
                    bcp.DestinationTableName = nombreTabla;

                    using (SqlDataReader reader = cmdOrigen.ExecuteReader())
                    {
                        bcp.WriteToServer(reader);
                    }



                }




            }
                //using (SqlCommand cmd = connSource.CreateCommand())
                //using (SqlBulkCopy bcp = new SqlBulkCopy(cnDestino))
                //{
                //    bcp.DestinationTableName = nombreTabla; ;
                //    cmd.CommandText = "SELECT* FROM " + nombreTabla;
                ////    cmd.CommandType = CommandType.StoredProcedure;
                //    connSource.Open();
                //    using (SqlDataReader reader = cmd.ExecuteReader())
                //    {
                //        bcp.WriteToServer(reader);
                //    }
                //}

                ///////////////////////////////////////////////////////////

      


            //SqlCommand cmd = new SqlCommand("DELETE FROM " + nombreTabla, cnDestino);
            //cmd = new SqlCommand("SELECT * FROM " + nombreTabla, cnOrigen);



            //SqlBulkCopy bcp = new SqlBulkCopy(cnDestino);

            //bcp.DestinationTableName = nombreTabla;

            //using (SqlDataReader reader = cmd.ExecuteReader())
            //{
            //    bcp.WriteToServer(reader);
            //}

            //bcp.Close();
            //cnDestino.Close();
            //cnOrigen.Close();
            ////////////////////////
            //SqlCommand command = new SqlCommand("SycnProducto", cnOrigen);
            //command.CommandType = CommandType.StoredProcedure;


        }


        
}


    public class TransBank
    {

        string cnstr = @"Data Source=.\SQLEXPRESS;Initial Catalog=DB;Integrated Security = True";
        string PuertoCOM = "COM6";//lector tarjeta transbank
//        string _CodigoProducto;
        string _precio;
        //string _NombreProducto;
        SerialPort _serialPort;
        public string _voucherRX;
        public string _mensajeRetorno;

        private delegate void SetTextDeleg(string text);

        //string _ClearData = "";
        //string _OriginalData = "";



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





        public TransBank(string precio)
        {


            _serialPort = new SerialPort(PuertoCOM, 115200, Parity.None, 8, StopBits.One);
            _serialPort.Handshake = Handshake.None;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceivedTransbank);
            _serialPort.ReadTimeout = 120000;
            //   _serialPort.Encoding = Encoding.GetEncoding(28591);
            _serialPort.Close();

            try
            {
                if (!_serialPort.IsOpen)
                    _serialPort.Open();
                pooling(); //chequeo de dispositivo OK
                _serialPort.DiscardOutBuffer();

                //string mensajeEnviado = "@CMD:Venta* @Valor:"+ _precio.Trim() + "* @Ticket:150001 */";

                //_serialPort.Write(mensajeEnviado);
                char stx = System.Convert.ToChar(0x02);
                char eox = System.Convert.ToChar(0x03);
                char separador = System.Convert.ToChar(0x7C);
                char sumar = System.Convert.ToChar(0x00);
                string Ticket = "150001";
                String Comando_Venta;
                //0200l35800l002380l1l1

                Ticket = GetTimestamp(DateTime.Now);


                Comando_Venta = string.Concat("200", separador, _precio.Trim(), separador, Ticket, separador, "1", separador, "0", eox);




                //===========Checksum==XOR========================

                for (int i = 0; i < Comando_Venta.Length; i++)
                {
                    sumar ^= System.Convert.ToChar(Comando_Venta[i]);
                }
                //========================================


                _serialPort.Write(string.Concat(stx, Comando_Venta, sumar));



            }
            catch (Exception ex)
            {
              //  MessageBox.Show("No se puedo enviar la información (Port)" + ex.Message, "Error!");
            }

        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }


        void pooling()
        {
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
            }
            catch (Exception ex)
            {
               // MessageBox.Show("No se puedo enviar la información", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        void sp_DataReceivedTransbank(object sender, SerialDataReceivedEventArgs e)
        {

            //   Thread.Sleep(500);
            //    char ack = System.Convert.ToChar(0x06);
            Thread.Sleep(1000);
            string datorx = _serialPort.ReadExisting();
            //  Thread.Sleep(1000);
            // string RxString = _serialPort.ReadExisting();
            string data = datorx;
            //_serialPort.DiscardInBuffer();

            char ack = System.Convert.ToChar(0x06);
            // string datorx = serialPort1.ReadExisting();
            int isOK = 2; // o:exito 1:error 2:no hace nada o falta definicion de que hacer en caso de que ocurra (la accion)


//            string procesoRX = "";                //2 dig //pos
            string comandoRX = "";              //4 dig //pos
            string resultadoRX = "";            //2 dig //pos
            string comercioRX = "";             //12 dig //pos
            string terminalRX = "";             //8 dig //pos
                                                //===========================================================
            string boletaRX = "";               //06 dig //pos
            string autorizacionRX = "";         //max 6 dig //pos
            string montolRX = "";               //max 9 dig //pos
            string numTarjRX = "";             //4 dig //pos
            string operacionRX = "";            //max 6 dig //pos
            string tipoTarjRX = "";             //2 dig //pos      credito o debito
            string fechacontableRX = "";        //6 dig //pos
            string cuentaRX = "";               //19 dig //pos
            string abrevtarjRX = "";            //2 dig //pos
            string fechaRX = "";                //8 dig //pos
            string horaRX = "";                 //6 dig //pos
            string voucherRX = "";              //2 dig //pos
            string largoRX = "";
            string encrippanRX = "";

            //  _serialPort.Write(string.Concat(ack)); //si no se manda el transbank pregunta tres veces.

            if (datorx[0] == 0x06) { _mensajeRetorno = "ACK desde Terminal"; }
            else
            {
                _serialPort.Write(string.Concat(ack));


                comandoRX = datorx.Substring(1, 4);    //Comando//se indica indice inicio,cantidad
                //textBox3.Text = comandoRX;
                resultadoRX = datorx.Substring(6, 2);  //Resultado
                                                       //  textBox4.Text = resultadoRX;
                largoRX = datorx.Length.ToString();
                //  textBox5.Text = largoRX;

                //======================Identificar Codigo de respuesta===============
                //MessageBox.Show(str.Length.ToString());
                if (resultadoRX.Contains("02")) { _mensajeRetorno = "Autorizador no responde......."; isOK = 1; }
                if (resultadoRX.Contains("03")) { _mensajeRetorno = "Conexion fallo......."; isOK = 1; }
                if (resultadoRX.Contains("04")) { _mensajeRetorno = "Transaccion ya fue anulada......."; isOK = 0; } //anulacion fue ok
                if (resultadoRX.Contains("05")) { _mensajeRetorno = "No existe Transaccion para anular......."; isOK = 1; }
                if (resultadoRX.Contains("06")) { _mensajeRetorno = "Tarjeta no soportada......."; isOK = 1; }
                if (resultadoRX.Contains("07")) { _mensajeRetorno = "Transaccion cancelada ......."; isOK = 1; }
                if (resultadoRX.Contains("08")) { _mensajeRetorno = "No puede Anular Transaccion Debito......."; isOK = 1; }
                if (resultadoRX.Contains("09")) { _mensajeRetorno = "Error Lectura Tarjeta......."; isOK = 1; }
                if (resultadoRX.Contains("10")) { _mensajeRetorno = "Monto menor al minimo permitido......."; isOK = 1; }
                if (resultadoRX.Contains("11")) { _mensajeRetorno = "No existe venta......."; isOK = 1; }
                if (resultadoRX.Contains("12")) { _mensajeRetorno = "Transaccion no soportada......."; isOK = 1; }
                if (resultadoRX.Contains("13")) { _mensajeRetorno = "Debe ejecutar Cierre......."; isOK = 1; }
                if (resultadoRX.Contains("14")) { _mensajeRetorno = "Error Encriptado PAN (BCYCLE......."; isOK = 1; }
                if (resultadoRX.Contains("15")) { _mensajeRetorno = "Error Operando con Debito......."; isOK = 1; }
                if (resultadoRX.Contains("80")) { _mensajeRetorno = "Solicita confirmar Monto......."; isOK = 2; }

                if (resultadoRX.Contains("81")) { _mensajeRetorno = "Solicita ingreso de Clave......"; isOK = 2; }
                if (resultadoRX.Contains("82"))
                { _mensajeRetorno = "Enviando Transaccion..........."; isOK = 2; }
                if (resultadoRX.Contains("90")) { _mensajeRetorno = "Inicializacion exitosa........."; isOK = 2; }



                if (resultadoRX.Contains("91")) { _mensajeRetorno = "Inicializacion fallida........."; isOK = 1; }
                if (resultadoRX.Contains("92")) { _mensajeRetorno = "Lector no conectado............"; isOK = 1; }
                //===============Identificacion de Comando de respuesta==============================================
                if (datorx.Contains("0210|"))
                { _mensajeRetorno = "Venta en proceso"; isOK = 2; }
                if (datorx.Contains("0810|")) { _mensajeRetorno = "Carga de Llaves en proceso"; isOK = 2; }
                if (datorx.Contains("0510|")) { _mensajeRetorno = "Cierre en proceso"; isOK = 2; }
                if (datorx.Contains("1210|")) { _mensajeRetorno = "Anulacion en proceso"; isOK = 2; }
                if (datorx.Contains("1080|")) { _mensajeRetorno = "Respuesta de Inicializacion en proceso"; isOK = 2; }
                if (datorx.Contains("0410|")) { _mensajeRetorno = "Pan Encriptado en proceso"; isOK = 2; }
                if (datorx.Contains("0260|")) { _mensajeRetorno = "Ultima Venta en proceso"; isOK = 2; }
                //=========================Procesamiento de datos Recibidos=======================================
                if (datorx.Contains("0410|"))
                {
                    encrippanRX = datorx.Substring(9, 48);
                    // textBox2.Text = string.Concat("PAN Encriptado 3DES : " + encrippanRX); //textBox2.Text: aqui se formatea el voucher
                    isOK = 2;
                }
                if (datorx.Contains("0810|"))
                {
                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                    terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                    isOK = 2;                                       //    textBox2.Text = string.Concat("Carga de Llaves " + "\r\nCodigo Comercio : " + comercioRX + "\r\nID Terminal: " + terminalRX);
                }
                if (datorx.Contains("1080|"))
                {
                    fechaRX = datorx.Substring(9, 8); ;                //8 dig //pos
                    horaRX = datorx.Substring(18, 6); ;                //6 dig //pos
                    isOK = 2;                                          //    textBox2.Text = string.Concat("Inicializacion " + "\r\nFecha de Inicializacion : " + fechaRX + "Hora de Inicializacion : " + horaRX);

                }
                if (datorx.Contains("1210|04"))
                {
                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                    terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                    autorizacionRX = datorx.Substring(31, 6);         //max 6 dig //pos
                    operacionRX = datorx.Substring(38, 6);            //max 6 dig //pos
                    isOK = 2;

                }
                if (datorx.Contains("0510|") && datorx.Contains("TERMINAL")) //Cierre de terminal//cmabio turno
                {

                    char stx = System.Convert.ToChar(0x02);
                    char eox = System.Convert.ToChar(0x03);
                    char separador = System.Convert.ToChar(0x7C);
                    int pos_stx = datorx.IndexOf(stx);  //indicar indice de eox
                    int pos_eox = datorx.IndexOf(eox);
                    int pos_ult_sep = datorx.LastIndexOf(separador);
                    //textBox9.Text = string.Concat(pos_stx);
                    //textBox10.Text = string.Concat(pos_eox);
                    //textBox11.Text = string.Concat(pos_ult_sep);
                    voucherRX = datorx.Substring(pos_ult_sep + 1, (pos_eox - 1) - pos_ult_sep);

                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                    terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                                                            //  textBox3.Text = "Cierre en proceso";
                                                            //textBox2.Text = voucherRX;
                                                            //textBox7.Text = comercioRX;
                                                            //textBox8.Text = terminalRX;

                    isOK = 0;
                }
                else if (datorx.Contains("0510|"))
                {
                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                    terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                    isOK = 0;
                    //   textBox2.Text = string.Concat("Cierre sin voucher\r\n" + "Codigo Comercio : " + comercioRX + "\r\nID Terminal: " + terminalRX);
                }
                if (datorx.Contains("0210|01|"))
                {
                    _mensajeRetorno = "Transaccion Rechazada............";
                    //    textBox2.Text = "Transaccion Rechazada...........";
                    isOK = 1;
                }
                if (datorx.Contains("0210|02|"))
                {
                    _mensajeRetorno = "Transaccion Fallo............";
                    //    textBox2.Text = "Autorizador no responde...........";
                    isOK = 1;
                }
                //========================Falta extraer datos================= //2000 venta
                //     MessageBox.Show(datorx);

                if (datorx.Contains("0210|00|") && datorx.Contains("TERMINAL"))
                {
                    char stx = System.Convert.ToChar(0x02);
                    char eox = System.Convert.ToChar(0x03);
                    char separador = System.Convert.ToChar(0x7C);
                    int pos_stx = datorx.IndexOf(stx);  //indicar indice de eox
                    int pos_eox = datorx.IndexOf(eox); // 770
                    int pos_ult_sep = datorx.LastIndexOf(separador);  //129
                                                                      //textBox9.Text = string.Concat(pos_stx);
                                                                      //textBox10.Text = string.Concat(pos_eox);
                                                                      //textBox11.Text = string.Concat(pos_ult_sep);

                    //insertar datos en tablaBD
                    //
                    voucherRX = datorx.Substring(pos_ult_sep + 1, (pos_eox - 1) - pos_ult_sep);
                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                    terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                    boletaRX = datorx.Substring(31, 20);               //06 dig ????  //pos 
                    autorizacionRX = datorx.Substring(52, 6);         //max 6 dig //pos
                    montolRX = datorx.Substring(59, 9);               //max 9 dig //pos
                    numTarjRX = datorx.Substring(69, 4);             //4 dig //pos
                    operacionRX = datorx.Substring(74, 6);            //max 6 dig //pos
                    tipoTarjRX = datorx.Substring(81, 2);             //2 dig //pos      credito o debito
                                                                      //Hasta aqui es igual si es credito o debito, despues cambian los campos.

                    switch (tipoTarjRX)
                    {
                        case "DB":
                            fechacontableRX = datorx.Substring(95, 8);
                            horaRX = datorx.Substring(104, 6);
                            cuentaRX = datorx.Substring(84, 6);//6 dig //pos
                            abrevtarjRX = datorx.Substring(92, 2);

                            break;
                        case "CR":
                            fechacontableRX = datorx.Substring(89, 8);
                            horaRX = datorx.Substring(98, 6);
                            cuentaRX = "";
                            abrevtarjRX = datorx.Substring(86, 2);

                            break;

                    }

                    _mensajeRetorno = "Transaccion exitosa...........";
                    isOK = 0;
                }

                //========================Falta extraer datos=================
                if (datorx.Contains("0260|") && datorx.Contains("TERMINAL")) //respuesta a la re imprimir ultima venta
                {
                    char stx = System.Convert.ToChar(0x02);
                    char eox = System.Convert.ToChar(0x03);
                    char separador = System.Convert.ToChar(0x7C);
                    int pos_stx = datorx.IndexOf(stx);  //indicar indice de eox
                    int pos_eox = datorx.IndexOf(eox); // 770
                    int pos_ult_sep = datorx.LastIndexOf(separador);  //129
                    //textBox9.Text = string.Concat(pos_stx);
                    //textBox10.Text = string.Concat(pos_eox);
                    //textBox11.Text = string.Concat(pos_ult_sep

                    //insertar en tabla TransbankTrx
                    voucherRX = datorx.Substring(pos_ult_sep + 1, (pos_eox - 1) - pos_ult_sep);   //

                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                    terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                    boletaRX = datorx.Substring(31, 20);               //06 dig ????  //pos 
                    autorizacionRX = datorx.Substring(52, 6);         //max 6 dig //pos
                    montolRX = datorx.Substring(59, 9);               //max 9 dig //pos
                    numTarjRX = datorx.Substring(69, 4);             //4 dig //pos
                    operacionRX = datorx.Substring(74, 6);            //max 6 dig //pos
                    tipoTarjRX = datorx.Substring(81, 2);

                    switch (tipoTarjRX)
                    {
                        case "DB":
                            fechacontableRX = datorx.Substring(95, 8);
                            horaRX = datorx.Substring(104, 6);
                            cuentaRX = datorx.Substring(84, 6);//6 dig //pos
                            abrevtarjRX = datorx.Substring(92, 2);

                            break;
                        case "CR":
                            fechacontableRX = datorx.Substring(89, 8);
                            horaRX = datorx.Substring(98, 6);
                            cuentaRX = "";
                            abrevtarjRX = datorx.Substring(86, 2);

                            break;

                    }

                    _mensajeRetorno = "Ultima Venta...........";
                    isOK = 0;
                }

                // lblatatus.BackColor = Color.White;//FromArgb(150, 0, 0);// 
                //  _serialPort.Close();

                //   Thread.Sleep(2000);
                switch (isOK)
                {
                    case 0:
                        InsertaTrx(comercioRX, terminalRX, boletaRX, autorizacionRX, montolRX, numTarjRX, operacionRX, tipoTarjRX, fechacontableRX, horaRX, cuentaRX, voucherRX);
                        //     _serialPort.Close();

                        _voucherRX = voucherRX;
                  //      this.DialogResult = DialogResult.OK;
                        break;
                    case 1:
                        //    _serialPort.Close();
                    //    this.DialogResult = DialogResult.Cancel;
                        break;
                    case 2:
                        //    Thread.Sleep(500);
                        _serialPort.DiscardInBuffer();
                        break;


                }




            }

        }




    }



}
