using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Quartz;//Install-Package Quartz -Version 2.3.3  //https://www.nuget.org/packages/Quartz/2.3.3
using Quartz.Impl;
using System.IO.Ports;
using System.Net;
using Newtonsoft.Json;
using System.Net.Mail;

namespace Herramientas
{


    public class Casdro
    {
        public int code { get; set; }
        public string data { get; set; }
    }


    public  class funciones
    {


        private string  cnLocal = @"Data Source=.\SQLEXPRESS;Initial Catalog=DB;Integrated Security = True";
        public string cnRemota = @"";
        public string idCash = "";
        public string cron = "";
        public string PortLectorBarras = "";
        public string PortImpresora = "";
        public string PortLectorTBK = "";
        public string cronZ = "";
        public string IPCashDro = "";
        public string UserCasdro = "";
        public string PassCashdro = "";
        public string TimeOutCashdro = "";
        public string posIdCasdro = "";
        public string posUser = "";
        public string CodigoCajaImpresor = "";
        public string ListaDeNotificacionInforme = "";
        public string EnviarCorreoHabilitado = "";
        public string ListaDeNotificacionGeneral = "";
        public string RutaAttachCorreoInforme = "";
        public string RutaAttachCorreoGeneral = "";





        public Boolean ExistePort(string port)
        {
            var portExists = SerialPort.GetPortNames().Any(x => x == port);
            return portExists;
        }






        public void enviarCorreo(string pathFileName)
          {

            ///////////
            string NombreInfDetatVtaCsv = "";
            using (SqlConnection connection = new SqlConnection(cnLocal))
            {
     
                    connection.Open();
                    SqlCommand command = new SqlCommand("getLastFileNameZ", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    // command.Parameters.Add(new SqlParameter("@EmployeeID", employeeID));
                    command.CommandTimeout = 5;

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                NombreInfDetatVtaCsv = dr["NombreInfDetatVtaCsv"].ToString();
                            }
                        }

                    }
             }
            

            
            pathFileName = pathFileName + NombreInfDetatVtaCsv;
            /////////////

            using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"))
            {

                string addresses = "dionisiocataldo@outlook.com;dionisiocataldo@gmail.com";

                using (MailMessage mail = new MailMessage())
                { //code goes here } 

                    foreach (var address in addresses.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mail.To.Add(address);
                    }

                    mail.From = new MailAddress("notificacion.aramark@gmail.com");
                   // mail.To.Add("dionisiocataldo@outlook.com,dionisiocataldo@gmail.com");
                    mail.Subject = "aramark test 2";
                    mail.Body = "mail with attachment";
                    System.Net.Mail.Attachment attachment;
                   
                    SmtpServer.Port = 587;
                    SmtpServer.EnableSsl = true;
                    attachment = new System.Net.Mail.Attachment(pathFileName);
                    mail.Attachments.Add(attachment);
                    SmtpServer.Credentials = new System.Net.NetworkCredential("notificacion.aramark@gmail.com", "notificacion.totem");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
            }
        }


            public bool hayconexionInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool hayconexionCasdro(string IP)
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("https://" + IP +"/Cashdro3WS"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        //public string hayconexionInternetCashdro()
        //{
        //    try
        //    {
        //        using (var client = new WebClient())
        //        using (var stream = client.OpenRead("https://10.10.20.57/&name=manager&password=2"))
        //        {
        //            using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
        //        {
        //            var page = sr.ReadToEnd();
        //hay que validar qye venga {"code":1,"data":""}

        //                // MessageBox.Show("resultado id operacion(enqueue): {0}", idOperation);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return page;
        //    }
        //}


        public Boolean LeerParametros()
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
                                idCash = dr["idCash"].ToString();
                                cron = dr["cron"].ToString();
                                PortLectorBarras = dr["PortLectorBarras"].ToString();
                                PortImpresora = dr["PortImpresora"].ToString();
                                PortLectorTBK = dr["PortLectorTBK"].ToString();
                                cronZ = dr["cronZ"].ToString();
                                IPCashDro = dr["IPCashDro"].ToString();
                                UserCasdro = dr["UserCasdro"].ToString();
                                TimeOutCashdro = dr["TimeOutCashdro"].ToString();
                                PassCashdro = dr["PassCashdro"].ToString();
                                posIdCasdro = dr["posIdCasdro"].ToString();
                                posUser = dr["posUser"].ToString();
                                CodigoCajaImpresor = dr["CodigoCajaImpresor"].ToString();

                                ListaDeNotificacionInforme = dr["ListaDeNotificacionInforme"].ToString();
                                EnviarCorreoHabilitado = dr["EnviarCorreoHabilitado"].ToString();
                                ListaDeNotificacionGeneral = dr["ListaDeNotificacionGeneral"].ToString();
                                RutaAttachCorreoInforme = dr["RutaAttachCorreoInforme"].ToString();
                                RutaAttachCorreoGeneral = dr["RutaAttachCorreoGeneral"].ToString();
                            }
}

                    }
                    return true;
                }
                catch (Exception)
                {
                    if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                    return false;
                }

            }

        }

       


    }


    public class Target
    {
        public int code;
        public string data;
    }

    //public class CierreZ
    //{
    //    string comando; string extension; string NumZeta;
    //    public  CierreZ()
    //        {
    //        }
    //}




        public class SincronizaVentas
    {

        public  string cnLocal = @"Data Source=.\SQLEXPRESS;Initial Catalog=DB;Integrated Security = True";
        public  string cnRemota = @"";
        public string idCash = "";
        public string cron = "";
        public string PortLectorBarras = "";
        public string PortImpresora = "";
        public string PortLectorTBK = "";




        public SincronizaVentas()
        {

            LeerParametros();
          

        }


         public void LeerParametros()
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
                                idCash = dr["idCash"].ToString();
                                cron = dr["cron"].ToString();
                                PortLectorBarras = dr["PortLectorBarras"].ToString();
                                PortImpresora = dr["PortImpresora"].ToString();
                                PortLectorTBK = dr["PortLectorTBK"].ToString();
                     


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



         public void SincronizaTabla(string StrcnOrigen, string StrcnDestino, string nombreTabla,string idCash ,string accion)
        {


            try { 
            using (SqlConnection connSource = new SqlConnection(StrcnOrigen))
            {
                using (SqlConnection connDestino = new SqlConnection(StrcnDestino))
                {
                    string queryOrigen;

                    if (idCash != "") // si viene el id lo agrego a la query
                    {queryOrigen = "SELECT " + "'"+ idCash + "'" + " as idCash,* FROM " + nombreTabla;
                      
                    }
                    else {     queryOrigen = "SELECT * FROM " + nombreTabla; }

                  


                    SqlCommand cmdDestino = new SqlCommand("DELETE FROM " + nombreTabla, connDestino);
                   

                    SqlCommand cmdOrigen = new SqlCommand(queryOrigen, connSource);
                    connSource.Open();
                    connDestino.Open();


                    if (accion == "TruncateDestino") { cmdDestino.ExecuteNonQuery(); } //trunca tabla de destino.


                    SqlBulkCopy bcp = new SqlBulkCopy(StrcnDestino, SqlBulkCopyOptions.KeepIdentity);
                    bcp.DestinationTableName = nombreTabla;


                    using (SqlDataReader reader = cmdOrigen.ExecuteReader())
                    {
                        bcp.WriteToServer(reader);
                    }

                    if (accion == "TruncaOrigen") {
                        cmdOrigen.CommandText = "DELETE FROM " + nombreTabla;
                            cmdOrigen.ExecuteNonQuery();
                                        } //trunca tabla de destino.
                    
                }
                
            }
            } //fin del try

            catch (Exception ex)
            {
                //  exception
                ResgistroError.insertarLog(ex.HResult.ToString(),ex.Source,ex.Message, "SincronizaTabla:"  + nombreTabla );

            }
                        finally
            {
                // final

            }

        }
        

    }



    #region Clase Job Calendario
    public class Jobcalendario
    {
        public string cron = "";
        public string cnLocal = @"Data Source=.\SQLEXPRESS;Initial Catalog=DB;Integrated Security = True";


        public Jobcalendario()
        {

            LeerParametros();
            Calendarionzacion();


        }


        public void LeerParametros()
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
                              
                                cron = dr["cron"].ToString();

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

        public void Calendarionzacion()
        {
          
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler, start the schedular before triggers or anything else
            IScheduler sched = schedFact.GetScheduler();
            sched.Start();

            // create job
            IJobDetail job = JobBuilder.Create<JobDeSincronizacion>()
                    .WithIdentity("job1", "group1")
                    .Build();



            ITrigger trigger = TriggerBuilder.Create()
                     .WithIdentity("trigger1", "group1")
                     .WithCronSchedule(cron)
                     .ForJob(job)
                     .Build();

            // Schedule the job using the job and trigger 
            sched.ScheduleJob(job, trigger);

        }





    }
    #endregion JobCalendario
    #region clase JOB

    public class JobDeSincronizacion : IJob
    {
        //  private TBPESContext db = new TBPESContext();
        public void Execute(IJobExecutionContext context)
        {
            Herramientas.SincronizaVentas sv = new Herramientas.SincronizaVentas();

            //  sv.LeerParametros();
            string origen = sv.cnRemota;
            string destino = sv.cnLocal;
            string tabla = "Productos";


            sv.SincronizaTabla(origen, destino, tabla, "", "TruncateDestino");

            tabla = "Clientes";
            sv.SincronizaTabla(origen, destino, tabla, "", "TruncateDestino");

            tabla = "ClienteAudit";
            origen = sv.cnLocal;
            destino = sv.cnRemota;
            sv.SincronizaTabla(origen, destino, tabla, sv.idCash, "TruncaOrigen");



        }
    }


    public class JobDeCierreZ : IJob
    {
        //  private TBPESContext db = new TBPESContext();
        public void Execute(IJobExecutionContext context)
        {
            Herramientas.SincronizaVentas sv = new Herramientas.SincronizaVentas();

            //  sv.LeerParametros();
            string origen = sv.cnRemota;
            string destino = sv.cnLocal;
            string tabla = "Productos";


            sv.SincronizaTabla(origen, destino, tabla, "", "TruncateDestino");

            tabla = "Clientes";
            sv.SincronizaTabla(origen, destino, tabla, "", "TruncateDestino");

            tabla = "ClienteAudit";
            origen = sv.cnLocal;
            destino = sv.cnRemota;
            sv.SincronizaTabla(origen, destino, tabla, sv.idCash, "TruncaOrigen");



        }
    }





    #endregion clase JOB



    public static class ResgistroError
    {

        public static void insertarLog(string numero, string fuente,string descripcion,string metodo)
        {
            string cnLocal = @"Data Source=.\SQLEXPRESS;Initial Catalog=DB;Integrated Security = True";
            using (SqlConnection connection = new SqlConnection(cnLocal))
            {
                // int employeeID = findEmployeeID();
                try
                {

                    connection.Open();
                    SqlCommand command = new SqlCommand("InsertLog", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@error", numero));
                    command.Parameters.Add(new SqlParameter("@origen", fuente));
                    command.Parameters.Add(new SqlParameter("@mensaje", descripcion));
                    command.Parameters.Add(new SqlParameter("@metodo", metodo));

                    // command.Parameters.Add(new SqlParameter("@EmployeeID", employeeID));
                    command.CommandTimeout = 5;
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    if (connection.State == System.Data.ConnectionState.Open) connection.Close();
                }

            }


        }

      
    }



    //*******************
public delegate string MensajeDeEstado(string mensaje);


    public class MensajesTbk : EventArgs
    {

        public MensajesTbk(string  mensaje)
        { Mensaje = mensaje; }
        public string Mensaje { get; set; }
    }



    public  class TransBank : IDisposable
    {
      //  public delegate void  MensajeDeEstado();
        public event EventHandler<MensajesTbk> OnMensajeEstado;
     //   public event MensajeDeEstado OnMensajeEstado;
        string cnstr = @"Data Source=.\SQLEXPRESS;Initial Catalog=DB;Integrated Security = True";
        string PuertoCOM = "COM6";//lector tarjeta transbank
 //       string _CodigoProducto;
 //       string _precio;
//        string _NombreProducto;
        SerialPort _serialPort;
        public string _voucherRX;
        public string _mensajeRetorno;

        private delegate void SetTextDeleg(string text);

//        string _ClearData = "";
//        string _OriginalData = "";

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
              
            }
            // free native resources if there are any.
        }

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



        public  void EjecutaComando(string comando)
        {

            try
            {
                char stx = System.Convert.ToChar(0x02);
                char eox = System.Convert.ToChar(0x03);
                char separador = System.Convert.ToChar(0x7C);
                char sumar = System.Convert.ToChar(0x00);
                String Comando;
                //0200l35800l002380l1l1
                Comando = string.Concat(comando, eox);
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
                //   MessageBox.Show("No se puedo enviar la información", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public  void EjecutaVenta(string precio)
        {

            try
            {
                char stx = System.Convert.ToChar(0x02);
                char eox = System.Convert.ToChar(0x03);
                char separador = System.Convert.ToChar(0x7C);
                char sumar = System.Convert.ToChar(0x00);
                string Ticket = "";
                String Comando_Venta;
                Ticket = GetTimestamp(DateTime.Now);
                Comando_Venta = string.Concat("200", separador, precio.Trim(), separador, Ticket, separador, "1", separador, "0", eox);
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
                //   MessageBox.Show("No se puedo enviar la información", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        public TransBank(string precio)
        {


            _serialPort = new SerialPort(PuertoCOM, 115200, Parity.None, 8, StopBits.One);
            _serialPort.Handshake = Handshake.None;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(this.sp_DataReceivedTransbank);
            _serialPort.BaudRate = 115200;
            _serialPort.ReadTimeout = 120000;
            //   _serialPort.Encoding = Encoding.GetEncoding(28591);
          //  _serialPort.Close();

            try
            {
                if (!_serialPort.IsOpen)
                    _serialPort.Open();
                pooling(); //chequeo de dispositivo OK
                _serialPort.DiscardOutBuffer();


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

                OnMensajeEstado(this, new MensajesTbk("rror"));
                // MessageBox.Show("No se puedo enviar la información", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void sp_DataReceivedTransbank(object sender, SerialDataReceivedEventArgs e)
        {
            //MensajesTbk x = new MensajesTbk("EXITO!!!");
            //OnMensajeEstado(this, x);
            //   Thread.Sleep(500);
            //    char ack = System.Convert.ToChar(0x06);
            // System.Threading.Thread.Sleep(1000);
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

            if (datorx[0] == 0x06) { _mensajeRetorno = "ACK desde Terminal"; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); }
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
                if (resultadoRX.Contains("02")) { _mensajeRetorno = "Autorizador no responde......."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 1; }
                if (resultadoRX.Contains("03")) { _mensajeRetorno = "Conexion fallo......."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 1; }
                if (resultadoRX.Contains("04")) { _mensajeRetorno = "Transaccion ya fue anulada......."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 0; } //anulacion fue ok
                if (resultadoRX.Contains("05")) { _mensajeRetorno = "No existe Transaccion para anular.......";  OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno));isOK = 1; }
                if (resultadoRX.Contains("06")) { _mensajeRetorno = "Tarjeta no soportada......."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 1; }
                if (resultadoRX.Contains("07")) { _mensajeRetorno = "Transaccion cancelada ......."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 1; }
                if ((resultadoRX.Contains("08") &&  (OnMensajeEstado != null))) { _mensajeRetorno = "No puede Anular Transaccion Debito.......";OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno));


                      isOK = 1; }
                if (resultadoRX.Contains("09")) { _mensajeRetorno = "Error Lectura Tarjeta......."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 1; }
                if (resultadoRX.Contains("10")) { _mensajeRetorno = "Monto menor al minimo permitido......."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 1; }
                if (resultadoRX.Contains("11")) { _mensajeRetorno = "No existe venta......."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 1; }
                if (resultadoRX.Contains("12")) { _mensajeRetorno = "Transaccion no soportada......."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 1; }
                if (resultadoRX.Contains("13")) { _mensajeRetorno = "Debe ejecutar Cierre......."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 1; }
                if (resultadoRX.Contains("14")) { _mensajeRetorno = "Error Encriptado PAN (BCYCLE......."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 1; }
                if (resultadoRX.Contains("15")) { _mensajeRetorno = "Error Operando con Debito......."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 1; }
                if (resultadoRX.Contains("80")) { _mensajeRetorno = "Solicita confirmar Monto......."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 2; }

                if (resultadoRX.Contains("81")) { _mensajeRetorno = "Solicita ingreso de Clave......"; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 2; }
                if (resultadoRX.Contains("82"))
                { _mensajeRetorno = "Enviando Transaccion..........."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 2; }
                if (resultadoRX.Contains("90")) { _mensajeRetorno = "Inicializacion exitosa........."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 2; }



                if (resultadoRX.Contains("91")) { _mensajeRetorno = "Inicializacion fallida........."; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 1; }
                if (resultadoRX.Contains("92")) { _mensajeRetorno = "Lector no conectado............"; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 1; }
                //===============Identificacion de Comando de respuesta==============================================
                if (datorx.Contains("0210|"))
                { _mensajeRetorno = "Venta en proceso"; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 2; }
                if (datorx.Contains("0810|")) { _mensajeRetorno = "Carga de Llaves en proceso"; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 2; }
                if (datorx.Contains("0510|")) { _mensajeRetorno = "Cierre en proceso"; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 2; }
                if (datorx.Contains("1210|")) { _mensajeRetorno = "Anulacion en proceso"; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 2; }
                if (datorx.Contains("1080|")) { _mensajeRetorno = "Respuesta de Inicializacion en proceso"; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 2; }
                if (datorx.Contains("0410|")) { _mensajeRetorno = "Pan Encriptado en proceso"; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 2; }
                if (datorx.Contains("0260|")) { _mensajeRetorno = "Ultima Venta en proceso"; OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno)); isOK = 2; }
                //=========================Procesamiento de datos Recibidos=======================================
                if (datorx.Contains("0410|"))
                {
                    encrippanRX = datorx.Substring(9, 48);
                  //  OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno));
                    // textBox2.Text = string.Concat("PAN Encriptado 3DES : " + encrippanRX); //textBox2.Text: aqui se formatea el voucher
                    isOK = 2;
                }
                if (datorx.Contains("0810|"))
                {
               
                    comercioRX = datorx.Substring(9, 12);   //12 dig //pos
                    terminalRX = datorx.Substring(22, 8);   //8 dig //pos
                    _mensajeRetorno = string.Concat("Carga de Llaves " + "\r\nCodigo Comercio : " + comercioRX + "\r\nID Terminal: " + terminalRX);
                    OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno));
                    isOK = 2;                                       //    textBox2.Text = string.Concat("Carga de Llaves " + "\r\nCodigo Comercio : " + comercioRX + "\r\nID Terminal: " + terminalRX);
                }
                if (datorx.Contains("1080|"))
                {
                    fechaRX = datorx.Substring(9, 8); ;                //8 dig //pos
                    horaRX = datorx.Substring(18, 6); ;
                    _mensajeRetorno = string.Concat("Inicializacion " + "\r\nFecha de Inicializacion : " + fechaRX + "Hora de Inicializacion : " + horaRX);
                    OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno));
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

                    OnMensajeEstado(this, new MensajesTbk("Cierre Terminal"));
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
                    OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno));
                    isOK = 1;
                }
                if (datorx.Contains("0210|02|"))
                {
                    _mensajeRetorno = "Transaccion Fallo............";
                    //    textBox2.Text = "Autorizador no responde...........";
                   
                    OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno));
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
                    OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno));
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
                    OnMensajeEstado(this, new MensajesTbk(_mensajeRetorno));
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
