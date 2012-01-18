using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FacturaElectronica.Common.Contracts;
using FacturaElectronica.Ui.Win.Administrador.Code;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Common.Contracts.Search;
using System.Configuration;
using System.Threading;
using FacturaElectronica.Ui.Win.Administrador.Code.Ejecucion;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace FacturaElectronica.Ui.Win.Administrador
{
    public partial class FormEnviarArchivosAWeb : Form
    {
        private ISubidaArchivoService SubidaArchivoService = ServiceFactory.GetSubidaArchivoService();
        private DateTime fechaLog = default(DateTime);

        public FormEnviarArchivosAWeb()
        {
            InitializeComponent();
        }

        private void FormEnviarArchivosAWeb_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timerLog_Tick(object sender, EventArgs e)
        {
            try
            {
                MostrarLog();
            }
            catch (Exception ex)
            {
                timerLog.Stop();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (folderBrowserDialogFilesForUpLoad.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.txtDirectorio.Text = folderBrowserDialogFilesForUpLoad.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private CorridaSubidaArchivoDto CorridaSubidaArchivo = new CorridaSubidaArchivoDto();

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                timerLog.Stop();
                string sourceDirectory = this.txtDirectorio.Text;
                List<string> fileList = new List<string>();
                if (Directory.Exists(sourceDirectory))
                {
                    // Obtengo los archivos a procesar
                    fileList.AddRange(Directory.GetFiles(sourceDirectory, "*.*", SearchOption.TopDirectoryOnly).ToList());

                    if (fileList.Count > 0)
                    {
                        // Genero una corrida
                        MostrarMensajeEnLog("Creando Corrida...");
                        CorridaSubidaArchivo = SubidaArchivoService.CrearNuevaCorrida();

                        this.txtNroCorrida.Text = CorridaSubidaArchivo.Id.ToString();

                        // Copio los archivos al server de telefónica
                        MostrarMensajeEnLog("Copiando Archivos al Servidor...");

                        this.Refresh();

                        //fileList = CopiarArchivosParaProcesar(fileList);
                        fileList = CopiarArchivosParaProcesarPorFTP(fileList);

                        // Ejecuto
                        MostrarMensajeEnLog("Enviando mensaje de ejecución asincrónica...");

                        EjecucionSubidaArchivos ejecucionData = new EjecucionSubidaArchivos();
                        ejecucionData.CorridaId = CorridaSubidaArchivo.Id;
                        ejecucionData.FileList = fileList;

                        ThreadPool.QueueUserWorkItem(new WaitCallback(EjecutarCorridaCallBack), ejecucionData);

                        // Activo el timer porque a veces no retorna la siguiente llamada
                        timerLog.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EjecutarCorridaCallBack(object state)
        {
            EjecucionSubidaArchivos ejecucionData = (EjecucionSubidaArchivos)state;
            SubidaArchivoService.EjecutarCorrida(ejecucionData.CorridaId, ejecucionData.FileList);
        }

        private const int bufferLength = 2048;

        private List<string> CopiarArchivosParaProcesarPorFTP(List<string> fileList)
        {
            List<string> filesInServerList = new List<string>();

            string ftpAddress = ConfigurationManager.AppSettings["FTPAddress"];
            string ftpUser = ConfigurationManager.AppSettings["FTPUser"];
            string ftpPassword = ConfigurationManager.AppSettings["FTPPassword"];
            NetworkCredential networkCredential = new NetworkCredential(ftpUser, ftpPassword);
            FtpWebRequest fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpAddress + CorridaSubidaArchivo.Id.ToString()));// + "/AProcesar/"));
            fwr.Method = WebRequestMethods.Ftp.MakeDirectory;
            fwr.Credentials = networkCredential;
            // fwr.EnableSsl = true;
            // AsignarCertificado(fwr);

            // Creo el directorio en el servidor remoto
            using (FtpWebResponse response = (FtpWebResponse)fwr.GetResponse())
            {
                response.Close();
            }

            string addressForCorrida = ftpAddress + CorridaSubidaArchivo.Id.ToString() + "/AProcesar/";
            fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(addressForCorrida));
            fwr.Method = WebRequestMethods.Ftp.MakeDirectory;
            fwr.Credentials = networkCredential;
            // fwr.EnableSsl = true;
            // AsignarCertificado(fwr);

            // Creo el directorio en el servidor remoto
            using (FtpWebResponse response = (FtpWebResponse)fwr.GetResponse())
            {
                MostrarMensajeEnLog("Creación del directorio en el servidor remoto: " + response.StatusCode.ToString());
                response.Close();
            }

            // Por cada archivo lo subo a la carpeta en el servidor remoto
            foreach (string file in fileList)
            {
                string fileName = Path.GetFileName(file);
                fwr = (FtpWebRequest)FtpWebRequest.Create(new Uri(addressForCorrida + fileName));
                fwr.Method = WebRequestMethods.Ftp.UploadFile;
                fwr.Credentials = networkCredential;

                FileInfo fileInfo = new FileInfo(file);

                using (FileStream fileStream = fileInfo.OpenRead())
                {
                    // Escribo el archivo al request FTP
                    using (Stream requestStream = fwr.GetRequestStream())
                    {
                        // Leo el archivo
                        byte[] buffer = new byte[bufferLength];
                        int count = 0;
                        int readBytes = 0;
                        do
                        {
                            readBytes = fileStream.Read(buffer, 0, bufferLength);
                            requestStream.Write(buffer, 0, readBytes);
                            count += readBytes;
                        }
                        while (readBytes != 0);
                        requestStream.Flush();
                        requestStream.Close();
                    }

                    // obtengo la respuesta
                    using (FtpWebResponse response = (FtpWebResponse)fwr.GetResponse())
                    {
                        MostrarMensajeEnLog(string.Format("Subida de archivo {0}  en el servidor remoto: {1}", fileName, response.StatusCode.ToString()));
                        response.Close();
                    }
                }

                filesInServerList.Add(fileName);
            }

            return filesInServerList;
        }

        private void AsignarCertificado(FtpWebRequest fwr)
        {
            X509Certificate cer = null;
            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine); //StoreName.TrustedPeople, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            foreach (X509Certificate cert in store.Certificates)
            {
                if (cert.Subject.Contains("https://endesafews/"))
                {
                    cer = cert;
                    break;
                }
            }

            //X509Certificate2 objCert = null;

            //if (cer != null)
            //    objCert = new X509Certificate2(cer);
            //else
            //    throw new Exception("Certificado inexistente!");

            fwr.ClientCertificates.Add(cer);

        }

        private List<string> CopiarArchivosParaProcesar(List<string> fileList)
        {
            List<string> filesInServerList = new List<string>();
            string destinationPath = ConfigurationManager.AppSettings["PathDestinoArchivosFacturaConCAE"];
            destinationPath = Path.Combine(destinationPath, CorridaSubidaArchivo.Id.ToString());
            destinationPath = Path.Combine(destinationPath, "AProcesar");

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            foreach (string file in fileList)
            {
                string fileName = Path.GetFileName(file);
                string destFilePath = Path.Combine(destinationPath, fileName);
                File.Copy(file, destFilePath);
                filesInServerList.Add(destFilePath);
            }

            return filesInServerList;
        }

        private void MostrarMensajeEnLog(string text)
        {
            this.LogTextBox.Text += text + Environment.NewLine;
        }

        private void MostrarLog()
        {
            CorridaSubidaArchivoSearch search = new CorridaSubidaArchivoSearch();
            search.CorridaId = CorridaSubidaArchivo.Id;
            search.LoadLog = true;
            search.FechaDesde = fechaLog;

            var corrida = this.SubidaArchivoService.ObtenerCorridas(search).First();
            List<CorridaSubidaArchivoLogDto> logs = corrida.Log;
            if (logs != null && logs.Count() > 0)
            {
                foreach (CorridaSubidaArchivoLogDto log in logs)
                {
                    if (log.FinCorrida)
                    {
                        this.timerLog.Stop();
                        break;
                    }
                    else
                    {
                        this.LogTextBox.Text += log.Mensaje + Environment.NewLine;
                    }
                }

                fechaLog = logs.Last().Fecha;
            }
        }

        private void btnVerDetalleCorrida_Click(object sender, EventArgs e)
        {
            try
            {
                FormDetalleDeEnvioArhivoAWeb frmDetalle = new FormDetalleDeEnvioArhivoAWeb();
                frmDetalle.CorridaId = this.CorridaSubidaArchivo.Id;
                frmDetalle.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
