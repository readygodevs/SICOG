using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.Models;

namespace TesoreriaVS12.Areas.Tesoreria.BL
{
    public class TipoCompromisoBL
    {
        private TipoCompromisosDAL DALTipoCompromisos;
        public TipoCompromisoBL()
        {
            if (DALTipoCompromisos == null)
                DALTipoCompromisos = new TipoCompromisosDAL();
        }
        public int getNextId()
        {
            int i= 0 ;
            try
            {
                i= DALTipoCompromisos.Get().Max(max => max.Id_TipoCompromiso) + 1;
            }
            catch (Exception)
            {
                i= 1;
            }
            return i;
        }
        public string obtenerFechaLetra(Ca_TipoCompromisos compromiso)
        {
            string pagarse = "";
            if (compromiso.Sin_Fecha == true)
            {
                pagarse = "AL MISMO DIA";
            }
            else
            {
                //Checkbox Dias
                if (compromiso.Num_Dia == true)
                {
                    if (compromiso.Dia_Semana > 0)
                    {

                        if (compromiso.Dia_Semana == 1)
                        {
                            if (compromiso.Antes == true)
                            {
                                pagarse = compromiso.Num_Dias.ToString() + " ANTES DEL LUNES";
                            }
                            if (compromiso.Despues == true)
                            {
                                pagarse = compromiso.Num_Dias.ToString() + " DESPUES DEL LUNES";
                            }
                            if (compromiso.Despues == false && compromiso.Antes == false)
                            {
                                pagarse = "DEL LUNES EN " + compromiso.Num_Dias.ToString() + " DIAS";
                            }
                        }

                        if (compromiso.Dia_Semana == 2)
                        {
                            if (compromiso.Antes == true)
                            {
                                pagarse = compromiso.Num_Dias.ToString() + " ANTES DEL MARTES";
                            }
                            if (compromiso.Despues == true)
                            {
                                pagarse = compromiso.Num_Dias.ToString() + " DESPUES DEL MARTES";
                            }
                            if (compromiso.Despues == false && compromiso.Antes == false)
                            {
                                pagarse = "DEL MARTES EN " + compromiso.Num_Dias.ToString() + " DIAS";
                            }
                        }

                        if (compromiso.Dia_Semana == 3)
                        {
                            if (compromiso.Antes == true)
                            {
                                pagarse = compromiso.Num_Dias.ToString() + " ANTES DEL MIERCOLES";
                            }
                            if (compromiso.Despues == true)
                            {
                                pagarse = compromiso.Num_Dias.ToString() + " DESPUES DEL MIERCOLES";
                            }
                            if (compromiso.Despues == false && compromiso.Antes == false)
                            {
                                pagarse = "DEL MIERCOLES EN " + compromiso.Num_Dias.ToString() + " DIAS";
                            }
                        }

                        if (compromiso.Dia_Semana == 4)
                        {
                            if (compromiso.Antes == true)
                            {
                                pagarse = compromiso.Num_Dias.ToString() + " ANTES DEL JUEVES";
                            }
                            if (compromiso.Despues == true)
                            {
                                pagarse = compromiso.Num_Dias.ToString() + " DESPUES DEL JUEVES";
                            }
                            if (compromiso.Despues == false && compromiso.Antes == false)
                            {
                                pagarse = "DEL JUEVES EN " + compromiso.Num_Dias.ToString() + " DIAS";
                            }
                        }

                        if (compromiso.Dia_Semana == 5)
                        {
                            if (compromiso.Antes == true)
                            {
                                pagarse = compromiso.Num_Dias.ToString() + " ANTES DEL VIERNES";
                            }
                            if (compromiso.Despues == true)
                            {
                                pagarse = compromiso.Num_Dias.ToString() + " DESPUES DEL VIERNES";
                            }
                            if (compromiso.Despues == false && compromiso.Antes == false)
                            {
                                pagarse = "DEL VIERNES EN " + compromiso.Num_Dias.ToString() + " DIAS";
                            }
                        }

                    }
                    else
                    {
                        pagarse = compromiso.Num_Dias.ToString() + " DIAS";
                        if (compromiso.Antes == true)
                        {
                            pagarse = pagarse + " ANTES";
                        }
                        if (compromiso.Despues == true)
                        {
                            pagarse = pagarse + " DESPUES";
                        }
                    }
                }
                //

                //Checkbox Num_Semana
                if (compromiso.Num_Semana == true)
                {
                    if (compromiso.Num_Semanas > 0)
                    {

                        if (compromiso.Num_Semanas == 1)
                        {
                            if (compromiso.Dia_Semana == 1)
                            {
                                pagarse = "PRIMERA SEMANA EL DIA LUNES";
                            }
                            if (compromiso.Dia_Semana == 2)
                            {
                                pagarse = "PRIMERA SEMANA EL DIA MARTES";
                            }
                            if (compromiso.Dia_Semana == 3)
                            {
                                pagarse = "PRIMERA SEMANA EL DIA MIERCOLES";
                            }
                            if (compromiso.Dia_Semana == 4)
                            {
                                pagarse = "PRIMERA SEMANA EL DIA JUEVES";
                            }
                            if (compromiso.Dia_Semana == 5)
                            {
                                pagarse = "PRIMERA SEMANA EL DIA VIERNES";
                            }
                        }

                        if (compromiso.Num_Semanas == 2)
                        {
                            if (compromiso.Dia_Semana == 1)
                            {
                                pagarse = "SEGUNDA SEMANA EL DIA LUNES";
                            }
                            if (compromiso.Dia_Semana == 2)
                            {
                                pagarse = "SEGUNDA SEMANA EL DIA MARTES";
                            }
                            if (compromiso.Dia_Semana == 3)
                            {
                                pagarse = "SEGUNDA SEMANA EL DIA MIERCOLES";
                            }
                            if (compromiso.Dia_Semana == 4)
                            {
                                pagarse = "SEGUNDA SEMANA EL DIA JUEVES";
                            }
                            if (compromiso.Dia_Semana == 5)
                            {
                                pagarse = "SEGUNDA SEMANA EL DIA VIERNES";
                            }
                        }

                        if (compromiso.Num_Semanas == 3)
                        {
                            if (compromiso.Dia_Semana == 1)
                            {
                                pagarse = "TERCERA SEMANA EL DIA LUNES";
                            }
                            if (compromiso.Dia_Semana == 2)
                            {
                                pagarse = "TERCERA SEMANA EL DIA MARTES";
                            }
                            if (compromiso.Dia_Semana == 3)
                            {
                                pagarse = "TERCERA SEMANA EL DIA MIERCOLES";
                            }
                            if (compromiso.Dia_Semana == 4)
                            {
                                pagarse = "TERCERA SEMANA EL DIA JUEVES";
                            }
                            if (compromiso.Dia_Semana == 5)
                            {
                                pagarse = "TERCERA SEMANA EL DIA VIERNES";
                            }
                        }

                        if (compromiso.Num_Semanas == 4)
                        {
                            if (compromiso.Dia_Semana == 1)
                            {
                                pagarse = "CUARTA SEMANA EL DIA LUNES";
                            }
                            if (compromiso.Dia_Semana == 2)
                            {
                                pagarse = "CUARTA SEMANA EL DIA MARTES";
                            }
                            if (compromiso.Dia_Semana == 3)
                            {
                                pagarse = "CUARTA SEMANA EL DIA MIERCOLES";
                            }
                            if (compromiso.Dia_Semana == 4)
                            {
                                pagarse = "CUARTA SEMANA EL DIA JUEVES";
                            }
                            if (compromiso.Dia_Semana == 5)
                            {
                                pagarse = "CUARTA SEMANA EL DIA VIERNES";
                            }
                        }

                        if (compromiso.Num_Semanas == 5)
                        {
                            if (compromiso.Dia_Semana == 1)
                            {
                                pagarse = "QUINTA SEMANA EL DIA LUNES";
                            }
                            if (compromiso.Dia_Semana == 2)
                            {
                                pagarse = "QUINTA SEMANA EL DIA MARTES";
                            }
                            if (compromiso.Dia_Semana == 3)
                            {
                                pagarse = "QUINTA SEMANA EL DIA MIERCOLES";
                            }
                            if (compromiso.Dia_Semana == 4)
                            {
                                pagarse = "QUINTA SEMANA EL DIA JUEVES";
                            }
                            if (compromiso.Dia_Semana == 5)
                            {
                                pagarse = "QUINTA SEMANA EL DIA VIERNES";
                            }
                        }

                        if (compromiso.Antes == true)
                        {
                            pagarse = pagarse + " (ANTES)";
                        }

                        if (compromiso.Despues == true)
                        {
                            pagarse = pagarse + " (DESPUES)";
                        }
                    }
                }
                //

                //A la quincena
                if (compromiso.A_Qna == true)
                {

                    if (compromiso.Dia_Antes_Q > 0)
                    {
                        pagarse = compromiso.Dia_Antes_Q.ToString() + " DIAS ANTES DE LA QUINCENA";
                    }

                    if (compromiso.Dia_Despues_Q > 0)
                    {
                        pagarse = compromiso.Dia_Despues_Q.ToString() + " DIAS DESPUES DE LA QUINCENA";
                    }

                    if (compromiso.Antes == true)
                    {
                        pagarse = pagarse + " (ANTES)";
                    }

                    if (compromiso.Despues == true)
                    {
                        pagarse = pagarse + " (DESPUES)";
                    }

                }
                //

                //Al mes
                if (compromiso.Dia_Mes == true)
                {
                    if (compromiso.Dia1 > 0)
                    {
                        if (compromiso.Dia2 > 0)
                        {
                            pagarse = compromiso.Dia1.ToString() + " Y " + compromiso.Dia2.ToString() + " DE CADA MES";
                        }
                        else
                        {
                            if (compromiso.Dia_Semana == 1)
                            {
                                pagarse = "LUNES CERCANO AL DIA " + compromiso.Dia1.ToString();
                            }
                            if (compromiso.Dia_Semana == 2)
                            {
                                pagarse = "MARTES CERCANO AL DIA " + compromiso.Dia1.ToString();
                            }
                            if (compromiso.Dia_Semana == 3)
                            {
                                pagarse = "MIERCOLES CERCANO AL DIA " + compromiso.Dia1.ToString();
                            }
                            if (compromiso.Dia_Semana == 4)
                            {
                                pagarse = "JUEVES CERCANO AL DIA " + compromiso.Dia1.ToString();
                            }
                            if (compromiso.Dia_Semana == 5)
                            {
                                pagarse = "VIERNES CERCANO AL DIA " + compromiso.Dia1.ToString();
                            }
                        }
                        if (compromiso.Antes == true)
                        {
                            pagarse = pagarse + " (ANTES)";
                        }
                        if (compromiso.Despues == true)
                        {
                            pagarse = pagarse + " (DESPUES)";
                        }
                    }
                }
            }
            return pagarse;
        }
    }
}