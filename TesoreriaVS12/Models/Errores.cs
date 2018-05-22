using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.DAL;

namespace TesoreriaVS12.Models
{
    public class Errores
    {
        private String _Mensaje;
        private ErroresDAL _Erorres;

        public ErroresDAL ErroresDal
        {
            get { return _Erorres; }
            set { _Erorres = value;  }
        }
        public String Mensaje
        {
            get { return _Mensaje; }
            set { _Mensaje = value; }
        }

        public Errores() {
            Mensaje = "";
        }

        public Errores(long Code, String msg = "")
        {
            if (ErroresDal == null) ErroresDal = new ErroresDAL();
            Ca_MensajesError e = ErroresDal.GetByID(x => x.CodeError == Code);
            if (e != null)
                Mensaje = e.Mensaje;
            else
            {
                e = new Ca_MensajesError();
                e.CodeError = Code;
                e.Mensaje = msg;
                e.Exception = msg;
                ErroresDal.Insert(e);
                ErroresDal.Save();
                Mensaje = "Ocurrió un error";
            }
            
        }
    }
}