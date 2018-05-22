using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Tramiles.Utils
{
    public class Mapear
    {
        String usuario = String.Empty;
        String password = String.Empty;
        String servidor = String.Empty;


        public Mapear()
        {
            this.usuario = ConfigurationManager.AppSettings.Get("userMapeo");
            this.password = ConfigurationManager.AppSettings.Get("pwdMapeo");
            this.servidor = ConfigurationManager.AppSettings.Get("serverMapeo");
                       
        }

        public bool UploadFTP(HttpPostedFile archivo, ref string error, string nombreArchivo, bool Reemplaza)
        {
            byte[] array = new byte[archivo.InputStream.Length];
          
            error = "1";

            try
            {
                archivo.InputStream.Read(array, 0, (int)archivo.InputStream.Length);
                using (MemoryStream fs = new MemoryStream(array))
                {
                    //string url = RemotePath + "//" + archivo.FileName;
                    error = "2";
                    string url = servidor + '/' + nombreArchivo;
                    // Creo el objeto ftp
                    FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(url);

                    // Fijo las credenciales, usuario y contraseña
                    ftp.Credentials = new NetworkCredential(usuario, password);

                    // Le digo que no mantenga la conexión activa al terminar.
                    ftp.KeepAlive = false;

                    // Indicamos que la operación es subir un archivo...
                    ftp.Method = WebRequestMethods.Ftp.UploadFile;

                    // … en modo binario … (podria ser como ASCII)
                    ftp.UseBinary = true;

                    // Indicamos la longitud total de lo que vamos a enviar.
                    ftp.ContentLength = fs.Length;

                    //Indicamos que sea pasivo
                    ftp.UsePassive = true;

                    // Desactivo cualquier posible proxy http.
                    // Ojo pues de saltar este paso podría usar 
                    // un proxy configurado en iexplorer
                    ftp.Proxy = null;

                    // Pongo el stream al inicio
                    fs.Position = 0;

                    // Configuro el buffer a 2 KBytes
                    int buffLength = 2048;
                    byte[] buff = new byte[buffLength];
                    int contentLen;


                    string errInt = "";
                    if (Reemplaza)
                    {
                        if (ExisteArchivo(nombreArchivo, ref errInt))
                        {
                            error = errInt;
                            return false;
                        }
                    }
                    // obtener el stream del socket sobre el que se va a escribir.
                    error = "3";
                    using (Stream strm = ftp.GetRequestStream())
                    {
                        // Leer del buffer 2kb cada vez
                        contentLen = fs.Read(buff, 0, buffLength);

                        error = "4";
                        // mientras haya datos en el buffer ….
                        while (contentLen != 0)
                        {
                            // escribir en el stream de conexión
                            //el contenido del stream del fichero
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                            error = "5";
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }

        }

        public bool UploadFTP(Stream stream, ref string error, string nombreArchivo, bool Reemplaza, string newPath)
        {
            byte[] array = new byte[stream.Length];

            error = "1";

            try
            {
                stream.Read(array, 0, (int)stream.Length);

                using (MemoryStream fs = new MemoryStream(array))
                {
                    //string url = RemotePath + "//" + archivo.FileName;
                    error = "2";
                    string url ="";
                    if(!String.IsNullOrEmpty(newPath))
                        url = servidor + newPath + "/" + nombreArchivo;
                    else
                        url = servidor + nombreArchivo;

                    // Creo el objeto ftp
                    FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(url);

                    // Fijo las credenciales, usuario y contraseña
                    ftp.Credentials = new NetworkCredential(usuario, password);

                    // Le digo que no mantenga la conexión activa al terminar.
                    ftp.KeepAlive = false;

                    // Indicamos que la operación es subir un archivo...
                    ftp.Method = WebRequestMethods.Ftp.UploadFile;

                    // … en modo binario … (podria ser como ASCII)
                    ftp.UseBinary = true;

                    // Indicamos la longitud total de lo que vamos a enviar.
                    ftp.ContentLength = fs.Length;

                    //Indicamos que sea pasivo
                    ftp.UsePassive = true;

                    // Desactivo cualquier posible proxy http.
                    // Ojo pues de saltar este paso podría usar 
                    // un proxy configurado en iexplorer
                    ftp.Proxy = null;

                    // Pongo el stream al inicio
                    fs.Position = 0;

                    // Configuro el buffer a 2 KBytes
                    int buffLength = 2048;
                    byte[] buff = new byte[buffLength];
                    int contentLen;


                    string errInt = "";
                    if (!Reemplaza)
                    {
                        if (ExisteArchivo(nombreArchivo, ref errInt))
                        {
                            error = errInt;
                            return false;
                        }
                    }
                    // obtener el stream del socket sobre el que se va a escribir.
                    error = "3";
                    using (Stream strm = ftp.GetRequestStream())
                    {
                        // Leer del buffer 2kb cada vez
                        contentLen = fs.Read(buff, 0, buffLength);

                        error = "4";
                        // mientras haya datos en el buffer ….
                        while (contentLen != 0)
                        {
                            // escribir en el stream de conexión
                            //el contenido del stream del fichero
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                            error = "5";
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }

        }

        public Stream DownloadFTPDefensoria(String path, string nomFile, ref string error)
        {
            WebClient request = new WebClient();
          
            try
            {
                request.Credentials = new NetworkCredential(usuario, password);
                string uri = servidor + "/" + path + "/" + nomFile;
                byte[] datos = request.DownloadData(uri);
                Stream stream = new MemoryStream(datos);
                return stream;
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }

        public Stream DownloadFTP(string nomFile, ref string error)
        {
            WebClient request = new WebClient();
          
            try
            {
                request.Credentials = new NetworkCredential(usuario, password);
                string uri = servidor + "/" + nomFile;
                byte[] datos = request.DownloadData(uri);
                Stream stream = new MemoryStream(datos);
                return stream;
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }

        public byte[] DownloadFTPToByte(string nomFile, ref string error)
        {
            WebClient request = new WebClient();
        
            try
            {
                request.Credentials = new NetworkCredential(usuario, password);
                string uri = servidor + "/" + nomFile;
                byte[] datos = request.DownloadData(uri);
                return datos;
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }

        public Stream DownloadFTP_Rigo(string nomFile, ref string error)
        {
            WebClient request = new WebClient();
        
            try
            {
                request.Credentials = new NetworkCredential(usuario, password);
                string uri = servidor + "/" + nomFile;
                byte[] datos = request.DownloadData(uri);
                Stream stream = new MemoryStream(datos);
                return stream;
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }

        public Stream DownloadFTP_Verificacion(string nomFile, ref string error)
        {
            WebClient request = new WebClient();
          
            try
            {
                request.Credentials = new NetworkCredential(usuario, password);
                string uri = servidor + "/" + nomFile;
                byte[] datos = request.DownloadData(uri);
                Stream stream = new MemoryStream(datos);
                return stream;
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }

        public String ExisteDirectorio(String path)
        {
          
            //peticionFtp.KeepAlive = true;
            var request = (FtpWebRequest)WebRequest.Create(new Uri(servidor + path));
            request.KeepAlive = true;

            request.Credentials = new NetworkCredential(usuario, password);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;

            try
            {
                FtpWebResponse respuesta;
                respuesta = (FtpWebResponse)request.GetResponse();
                respuesta.Close();
                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool ExisteArchivo(string nomFile, ref string error)
        {
         
            var request = (FtpWebRequest)WebRequest.Create(servidor + "/" + nomFile);
            request.Credentials = new NetworkCredential(usuario, password);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false;
                }
                error = ex.Message;
                return false;
            }


            /*WebClient request = new WebClient();
            string usuario = ConfigurationManager.AppSettings.Get("userMapeo");
            string password = ConfigurationManager.AppSettings.Get("pwdMapeo");
            string servidor = ConfigurationManager.AppSettings.Get("svrMapeo");
            try
            {
                request.Credentials = new NetworkCredential(usuario, password);
                string uri = servidor + "sicerpi_docs/" + nomFile;
                byte[] datos = request.DownloadData(uri);
                Stream stream = new MemoryStream(datos);
                return stream;
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }*/
        }

        public bool EliminarArchivo(string nomFile, ref string error, string newPath)
        {
            FtpWebRequest request;
                if(newPath == "") 
                    request = (FtpWebRequest)WebRequest.Create(servidor + "/" + nomFile);
                else
                    request = (FtpWebRequest)WebRequest.Create(servidor + "/" + newPath + "/" + nomFile);
             
            request.Credentials = new NetworkCredential(usuario, password);
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false;
                }
                error = ex.Message;
                return false;
            }
        }

        public bool EliminarArchivo_Verificacion(string nomFile, ref string error)
        {
           
            var request = (FtpWebRequest)WebRequest.Create(servidor + "/" + nomFile);
            request.Credentials = new NetworkCredential(usuario, password);
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false;
                }
                error = ex.Message;
                return false;
            }
        }
    }
}