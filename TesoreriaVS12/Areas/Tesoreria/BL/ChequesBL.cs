using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Vistas;
using TesoreriaVS12.Areas.Tesoreria.Models;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class ChequesBL
    {
        private ListaChequesImpresionDAL DALvListaCheques { get; set; }
        public ChequesBL()
        {
            if (DALvListaCheques == null) DALvListaCheques = new ListaChequesImpresionDAL();
        }

        public bool areConsecutive(short id_ctaBancaria, DateTime Fecha, List<vListaChequesImpresion> cheques, short no_cheques)
        {
            bool Respuesta = false;
            try
            {
                if (cheques.Count == 0)
                    return Respuesta;

                int NoCheques = cheques.Count;
                int primerCheque = cheques.First().No_Cheque;
                if (no_cheques < NoCheques)
                    cheques = cheques.Take((int)no_cheques).ToList();
                //checha que la fecha del cr sea mayor a la fecha del ejercido                
                List<vListaChequesImpresion> vlista = cheques;//DALvListaCheques.Get(reg => reg.Id_CtaBancaria == id_ctaBancaria).ToList();
                foreach (var auxv in vlista)
                {
                    if (Fecha < auxv.FechaCR) return false;
                }
                //Checa si son continuos
                List<int> chequesMas = new List<int>();
                chequesMas = vlista.OrderBy(reg => reg.No_Cheque).Select(reg => reg.No_Cheque).ToList();
                int UltCheque = Convert.ToInt32(chequesMas.Last());
                int i = primerCheque;
                int aux = 0;
                foreach (var t in chequesMas)
                {
                    if (i == t && i <= UltCheque)
                    {
                        i++;
                        aux++;
                        if (aux == NoCheques && NoCheques != 0) { return true; }
                    }
                    else
                        Respuesta = false;
                }
                i--;
                if (i == UltCheque)
                    Respuesta = true; //El cheque se puede Imprimir
                else
                    Respuesta = false; //Accion Cancelada. Los cheques no se encuentran consecutivos.
            }
            catch
            {
                Respuesta = false;
            }

            return Respuesta;
        }
    }
}