using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Models;
using TesoreriaVS12.Utils;

namespace TesoreriaVS12.Areas.Tesoreria.Models
{
    public class ListaErrores
    {
        public List<String> lista { get; set; }
    }
    public static class StringID
    {

        public static string Exceptions = "Ocurrio un error : ";
        public static string Yaexiste = "Ya existe éste ";
        public static string Yaexista = "Ya existe ésta ";
        public static string YaAsendente(string campo, string ascdesc) { return campo + " que usted intenta guardar no tiene un registro " + ascdesc + ", favor de verificarlo."; }
        public static string YaAsendentelvl(string campo, string ascdesc) { return campo + " que usten intenta guardar tiene un registro " + ascdesc + " de ultimo nivel, favor de verificarlo."; }
        public static string IsValid = "Información Invalida";
        public static string Agregado = "Registro Guardado Exitosamente";
        public static string EditarUltimoNivel(string ascdesc) { return "No se puede editar el último nivel porque tiene registros " + ascdesc; }


        public static string IdAccion(string Id_Accion)
        {
            return string.Format("{0:000}", Id_Accion);
        }

        public static string IdCRecaudador(byte Id_URI, byte Id_UEI, byte Id_UPI)
        {
            return string.Format("{0:00}{1:00}{2:00}", Id_URI, Id_UEI, Id_UPI);
        }

        //public static string IdCRecaudador(string Id_URI, string Id_UEI, string Id_UPI)
        //{
        //    return string.Format("{0:00}{1:00}{2:00}", Id_URI, Id_UEI, Id_UPI);
        //}

        public static string Id_Accion(string cve_presupuesto)
        {
            return cve_presupuesto.Substring(21, 3);
        }

        public static string IdAlcance(string cve_presupuesto)
        {
            return cve_presupuesto.Substring(24, 1);
        }

        public static string IdTipoG(string cve_presupuesto)
        {
            return cve_presupuesto.Substring(25, 1);
        }

        public static string IdFuente(string cve_presupuesto)
        {
            return cve_presupuesto.Substring(26, 4);
        }

        public static string IdAnioFin(string cve_presupuesto)
        {
            return cve_presupuesto.Substring(30, 2);
        }

        public static string IdActEconomica(short Sector, short Actividad)
        {
            return string.Format("{0:00}{1:00}", Sector, Actividad);
        }
        public static string IdActividadInst(byte Id_ActividadInst)
        {
            return string.Format("{0:00}", Id_ActividadInst);
        }
        public static string IdActividadInst(string cve_presupuesto)
        {
            return cve_presupuesto.Substring(9, 2);
        }
        public static string IdClasificacionProgramatica(string cve_presupuesto)
        {
            return cve_presupuesto.Substring(11, 1);
        }
        public static string IdPrograma(string cve_presupuesto)
        {
            return cve_presupuesto.Substring(12, 2);
        }
        public static string IdProceso(string cve_presupuesto)
        {
            return cve_presupuesto.Substring(14, 4);
        }
        public static string IdTipoMeta(string cve_presupuesto)
        {
            return cve_presupuesto.Substring(18, 1);
        }
        public static string IdActividadMIR(string cve_presupuesto)
        {
            return cve_presupuesto.Substring(19, 2);
        }
        public static string IdActividadMIR(byte Id_ActividadMIR)
        {
            return string.Format("{0:00}", Id_ActividadMIR);
        }

        public static string IdArea(byte Id_UP, byte Id_UR, byte Id_UE)
        {
            return string.Format("{0:00}{1:00}{2:00}", Id_UP, Id_UR, Id_UE);
        }
        public static string IdArea(string cve_presupuesto)
        {
            return cve_presupuesto.Substring(0, 6);
        }
        public static string IdConceptoIngreso(byte Rubro, byte Tipo, byte Clase)
        {
            return string.Format("{0:0}{1:0}{2:0}", Rubro, Tipo, Clase);
        }

        public static string IdCuenta(byte Genero, byte Grupo, byte Rubro, byte Cuenta, byte SubCuentaO1, int SubCuentaO2, short SubCuentaO3, int SubCuentaO4)
        {
            return string.Format("{0:0}{1:0}{2:0}{3:0}{4:0}{5:00000}{6:0000}{7:000000}", Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, SubCuentaO3, SubCuentaO4);
        }

        public static string IdCuentaFormato(byte Genero, byte Grupo, byte Rubro, byte Cuenta, byte SubCuentaO1, int SubCuentaO2, short SubCuentaO3, int SubCuentaO4)
        {
            return string.Format("{0:0}-{1:0}-{2:0}-{3:0}-{4:0}-{5:00000}-{6:0000}-{7:000000}", Genero, Grupo, Rubro, Cuenta, SubCuentaO1, SubCuentaO2, SubCuentaO3, SubCuentaO4);
        }

        public static string IdFuncion(byte Finalidad, byte Funcion, byte SubFuncion)
        {
            return string.Format("{0:0}{1:0}{2:0}", Finalidad, Funcion, SubFuncion);
        }

        public static string IdFuncion(string cve_presupuesto)
        {
            return cve_presupuesto.Substring(6, 3);
        }

        public static string IdImpuestoDeduccion(byte Id_Tipo_ImpDed, byte Id_ImpDed)
        {
            return string.Format("{0:00}{1:00}", Id_Tipo_ImpDed, Id_ImpDed);
        }

        public static string IdObjetoGasto(byte Capitulo, byte Concepto, byte PartidaGen, byte PartidaEsp)
        {
            return string.Format("{0:0}{1:0}{2:0}{3:00}", Capitulo, Concepto, PartidaGen, PartidaEsp);
        }

        public static string IdTipoImpuesto(byte TipoImpuesto, byte FolioImpuesto)
        {
            return string.Format("{0:00}{1:00}", TipoImpuesto, FolioImpuesto);
        }

        public static string IdTipoMovBancarios(byte Id_TipoMovB, byte Id_FolioMovB)
        {
            return string.Format("{0:0}{1:00}", Id_TipoMovB, Id_FolioMovB);
        }

        public static string IdClavePresupuesto(string Id_Area, string Id_Funcion, string Id_Actividad, string Id_ClasificacionP, string Id_Programa, string Id_Proceso, string Id_TipoMeta, string Id_ActividadMIR, string Id_Accion, string Id_Alcance, string Id_TipoG, string Id_Fuente, string AnioFin, string Id_ObjetoG)
        {
            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}", Id_Area, Id_Funcion, Id_Actividad, Id_ClasificacionP, Id_Programa, Id_Proceso, Id_TipoMeta, Id_ActividadMIR, Id_Accion, Id_Alcance, Id_TipoG, Id_Fuente, AnioFin, Id_ObjetoG);
        }

        public static string IdClavePresupuestoFormato(string IdClavePresupuesto)
        {
            MA_PresupuestoEg ma = new MaPresupuestoEgDAL().GetByID(x => x.Id_ClavePresupuesto == IdClavePresupuesto);
            if (ma == null)
                return "";
            return string.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}-{8}-{9}-{10}-{11}-{12}-{13}", ma.Id_Area, ma.Id_Funcion, ma.Id_Actividad, ma.Id_ClasificacionP, ma.Id_Programa, ma.Id_Proceso, ma.Id_TipoMeta, ma.Id_ActividadMIR, ma.Id_Accion, ma.Id_Alcance, ma.Id_TipoG, ma.Id_Fuente, ma.AnioFin, ma.Id_ObjetoG);
        }

        public static string IdClavePresupuestoIngreso(string Id_CentroRecaudador, string Id_Fuente, string AnioFin, string Id_Alcance, string Id_Concepto)
        {
            return string.Format("{0}{1}{2}{3}{4}", Id_CentroRecaudador, Id_Fuente, AnioFin, Id_Alcance, Id_Concepto);
        }

        public static string IdClavePresupuestoIngFormato(string idClavePresupuestoIng)
        {
            Ma_PresupuestoIng ma = new MaPresupuestoIngDAL().GetByID(x => x.Id_ClavePresupuesto == idClavePresupuestoIng);
            if (ma == null)
                return "";
            return string.Format("{0}-{1}-{2}-{3}-{4}", ma.Id_CentroRecaudador, ma.Id_Fuente, ma.AnioFin, ma.Id_Alcance, ma.Id_Concepto);
        }

        public static string Polizas(int? folio, byte? mes)
        {
            return string.Format("{0:00000}-{1}", folio, Diccionarios.Meses.SingleOrDefault(x => x.Key == mes).Value);
        }

        public static string Contrarecibo(byte? tipo, int? folio)
        {
            return string.Format("{0:00}-{1:00000}", tipo, folio);
        }

        public static string ContrareciboFormatoLetra(byte? tipo, int? folio)
        {
            return string.Format("{0}-{1:00000}", new TipoContrarecibosDAL().GetByID(x => x.Id_TipoCR == tipo).Descripcion, folio);
        }

        public static string Compromisos(byte? tipo, int? folio)
        {
            return string.Format("{0:00}-{1:00000}", tipo, folio);
        }

        public static string CvePresupuestalOG(string cvePresupuestal)
        {
            if (String.IsNullOrEmpty(cvePresupuestal))
                return "";
            return string.Format("{0}", cvePresupuestal.Substring(cvePresupuestal.Length - 5, 5));
        }

        public static string PolizasFormato(byte tipo, byte mes, int folio)
        {
            string aux = "";
            if (tipo == 1) aux = "I";
            if (tipo == 2) aux = "E";
            if (tipo == 3) aux = "D";
            if (tipo == 4) aux = "O";
            return string.Format("{0}-{1:00000}-{2:00}", aux, folio, mes);
        }

        public static string IdFuenteFin(byte Finalidad, byte Funcion, byte SubFuncion)
        {
            return string.Format("{0:0}{1:0}{2:00}", Finalidad, Funcion, SubFuncion);
        }


    }

    public class Control_Fechas
    {
        public DateTime Fecha_Min { get; set; }
        public DateTime Fecha_Max { get; set; }

        public Control_Fechas()
        {

            CierreMensualDAL cierre = new CierreMensualDAL();
            ParametrosDAL parametros = new ParametrosDAL();
            CA_Parametros ca = parametros.GetByID(x => x.Nombre == "Ejercicio");
            int mes = cierre.Get(x => x.Contable == true).Count();
            mes++;
            this.Fecha_Max = Convert.ToDateTime("31/12/" + ca.Valor);
            if (mes == 12)
                this.Fecha_Min = Fecha_Max;
            else
                this.Fecha_Min = Convert.ToDateTime("01/" + string.Format("{0:00}", mes) + "/" + ca.Valor);

        }
    }




    public class Tree
    {
        public int Id { get; set; }
        public byte Genero { get; set; }
        public byte Grupo { get; set; }
        public byte Rubro { get; set; }
        public byte Cuenta { get; set; }
        public byte SubCuentaO1 { get; set; }
        public int SubCuentaO2 { get; set; }
        public short SubCuentaO3 { get; set; }
        public int SubCuentaO4 { get; set; }
        public string IdCuentaF { get; set; }
        public decimal Monto { get; set; }
        public int IdPadre { get; set; }
        public bool Hijos { get; set; }

        public List<Tree> CrearListOne(ref string msn)
        {
            List<Tree> all = new List<Tree>();
            BD_TesoreriaEntities bd = new BD_TesoreriaEntities();
            List<CA_Cuentas> catalogo = bd.CA_Cuentas.Where(x => x.Grupo == 0).OrderBy(m => m.Id_CuentaFormato).ToList();
            return all;
        }


        public List<Tree> CrearList(ref string msn)
        {
            BD_TesoreriaEntities bd = new BD_TesoreriaEntities();
            List<CA_Cuentas> catalogo = bd.CA_Cuentas.OrderBy(m => m.Id_CuentaFormato).ToList();
            List<Tree> all = new List<Tree>();
            int aux = 1;
            foreach (CA_Cuentas item in catalogo)
            {
                try
                {
                    int padre = 0;
                    if (item.Genero > 0 && item.Grupo == 0)
                        padre = 0;
                    else
                        if (item.Grupo > 0 && item.Rubro == 0)
                            padre = all.Single(m => m.Genero == item.Genero && m.Grupo == 0).Id;
                        else
                            if (item.Rubro > 0 && item.Cuenta == 0)
                                padre = all.Single(m => m.Genero == item.Genero && m.Grupo == item.Grupo && m.Rubro == 0).Id;
                            else
                                if (item.Cuenta > 0 && item.SubCuentaO1 == 0)
                                    padre = all.Single(m => m.Genero == item.Genero && m.Grupo == item.Grupo && m.Rubro == item.Rubro && m.Cuenta == 0).Id;
                                else
                                    if (item.SubCuentaO1 > 0 && item.SubCuentaO2 == 0)
                                        padre = all.Single(m => m.Genero == item.Genero && m.Grupo == item.Grupo && m.Rubro == item.Rubro && m.Cuenta == item.Cuenta && m.SubCuentaO1 == 0).Id;
                                    else
                                        if (item.SubCuentaO2 > 0 && item.SubCuentaO3 == 0)
                                            padre = all.Single(m => m.Genero == item.Genero && m.Grupo == item.Grupo && m.Rubro == item.Rubro && m.Cuenta == item.Cuenta && m.SubCuentaO1 == item.SubCuentaO1 && m.SubCuentaO2 == 0).Id;
                                        else
                                            if (item.SubCuentaO3 > 0 && item.SubCuentaO4 == 0)
                                                padre = all.Single(m => m.Genero == item.Genero && m.Grupo == item.Grupo && m.Rubro == item.Rubro && m.Cuenta == item.Cuenta && m.SubCuentaO1 == item.SubCuentaO1 && m.SubCuentaO2 == item.SubCuentaO2 && m.SubCuentaO3 == 0).Id;

                    all.Add(new Tree() { Id = aux, Genero = item.Genero, Grupo = item.Grupo, Rubro = item.Rubro, Cuenta = item.Cuenta, SubCuentaO1 = item.SubCuentaO1, SubCuentaO2 = item.SubCuentaO2, SubCuentaO3 = item.SubCuentaO3, SubCuentaO4 = item.SubCuentaO4, IdCuentaF = item.Id_CuentaFormato, Monto = 1, IdPadre = padre });
                    if (padre != 0) TieneHijos(padre, ref all);
                    aux++;
                }
                catch
                {
                    msn = "No existe la cuenta ascendente de: " + item.Id_CuentaFormato;
                    return new List<Tree>();
                }
            }

            return all;
        }

        public void TieneHijos(int Id, ref List<Tree> LstTree)
        {
            Tree lst = LstTree.SingleOrDefault(m => m.Id == Id);
            lst.Hijos = true;
        }

        public List<Tree> ListaCuentas(int? Id)
        {
            CuentasDAL cuentas = new CuentasDAL();
            string msn = "";
            Tree tree = new Tree();
            List<Tree> lsttree = null;
            if (Id == 0)
                lsttree = tree.CrearList(ref msn).Where(x => x.Grupo == 0).ToList();
            else
                lsttree = tree.CrearList(ref msn).Where(x => x.IdPadre == Id).ToList();
            return lsttree;
        }




    }

    public sealed class Cuentas
    {
        private CuentasDAL _dalCuentas;
        private ParametrosDAL _dalParametros;
        public CuentasDAL dalCuentas
        {
            get { return _dalCuentas; }
            set { _dalCuentas = value; }
        }
        public ParametrosDAL dalParametros
        {
            get { return _dalParametros; }
            set { _dalParametros = value; }
        }
        public Cuentas()
        {
            if (dalCuentas == null) dalCuentas = new CuentasDAL();
            if (dalParametros == null) dalParametros = new ParametrosDAL();
        }

        public List<Ca_CuentasModel> listaCuentas(FiltrosCuentas filtros)
        {
            List<Ca_CuentasModel> dataModel = new List<Ca_CuentasModel>();
            if (!String.IsNullOrEmpty(filtros.GeneroStr))
                filtros.GeneroStr.Split(',').ToList().ForEach(x => { filtros.Genero.Add(Convert.ToInt32(x)); });
            if (!String.IsNullOrEmpty(filtros.GrupoStr))
                filtros.GrupoStr.Split(',').ToList().ForEach(x => { filtros.Grupos.Add(Convert.ToInt32(x)); });
            List<CA_Cuentas> cuentas = dalCuentas.Get(x =>
                (filtros.selectNoUltimoNivel == true ? x.Nivel != true : x.Nivel != null) &&
                (filtros.Genero.Count() > 0 ? filtros.Genero.Contains(x.Genero) : x.Genero != null) &&
                (filtros.Grupos.Count() > 0 ? filtros.Grupos.Contains(x.Grupo) : x.Grupo != null) &&
                (String.IsNullOrEmpty(filtros.IdCuenta) ? x.Id_Cuenta != null : x.Id_Cuenta.StartsWith(filtros.IdCuenta.Trim())) &&
                (String.IsNullOrEmpty(filtros.Descripcion) ? x.Descripcion != null : x.Descripcion.Contains(filtros.Descripcion.Trim())) &&
                (!filtros.Completa.HasValue ? x.Nivel != null : (filtros.Completa.Value == true ? x.Nivel == true : x.Nivel == false)) &&
                (!String.IsNullOrEmpty(filtros.IdCri) ? x.Id_Concepto == filtros.IdCri : x.Descripcion != "")
                ).ToList();
            if (filtros.RestringirCuentas.HasValue)
                if (filtros.RestringirCuentas.Value)
                {
                    if (String.IsNullOrEmpty(filtros.ParametroCuentas))
                        filtros.ParametroCuentas = "Cuenta_Banco";
                    CA_Parametros cuentasBancos = dalParametros.GetByID(x => x.Nombre.Equals(filtros.ParametroCuentas));
                    filtros.IdCuentasRestrictivas = cuentasBancos.Valor.Split(',').ToList();
                    List<CA_Cuentas> aux = new List<CA_Cuentas>();
                    foreach (String item in filtros.IdCuentasRestrictivas)
                    {
                        aux.AddRange(cuentas.Where(x => x.Id_Cuenta.StartsWith(item)));
                    }
                    cuentas = aux;
                }
            cuentas.ForEach(item => { dataModel.Add(ModelFactory.getModel<Ca_CuentasModel>(item, new Ca_CuentasModel())); });
            if (filtros.selectUltimoNivel.HasValue)
            {
                if (filtros.selectUltimoNivel.Value == true)
                    dataModel.ForEach(x => { x.SelectUltimoNivel = x.Nivel; });
                else
                    dataModel.ForEach(x => { x.SelectUltimoNivel = null; });
            }
            /*if (filtros.selectNoUltimoNivel.HasValue)
            {
                if (filtros.selectNoUltimoNivel.Value == true)
                    dataModel.ForEach(x => { x.SelectNoUltimoNivel = x.Nivel; });
                else
                    dataModel.ForEach(x => { x.SelectNoUltimoNivel = null; });
            }*/
            return dataModel;
        }

        public bool hasValid(FiltrosCuentas filtros)
        {
            try
            {
                List<Ca_CuentasModel> cuentas = listaCuentas(filtros);
                if (cuentas.Exists(x => x.Id_Cuenta.Trim().Equals(filtros.IdCuenta)))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                new Errores(ex.HResult, ex.Message);
                return false;
            }
        }
    }
    public class Tree2
    {
        public string IdCuenta { get; set; }
        public string Descripcion { get; set; }
        public byte Genero { get; set; }
        public byte Grupo { get; set; }
        public byte Rubro { get; set; }
        public byte Cuenta { get; set; }
        public byte SubCuentaO1 { get; set; }
        public int SubCuentaO2 { get; set; }
        public short SubCuentaO3 { get; set; }
        public int SubCuentaO4 { get; set; }
        public string IdCuentaFormato { get; set; }
        public decimal SaldoAnteriorDebe { get; set; }
        public decimal SaldoAnteriorHaber { get; set; }
        public decimal MovimientoDebe { get; set; }
        public decimal MovimientoHaber { get; set; }
        public decimal SaldoActualDebe { get; set; }
        public decimal SaldoActualHaber { get; set; }
        public string IdCuentaPadre { get; set; }
        public bool Hijos { get; set; }

        BD_TesoreriaEntities bd = new BD_TesoreriaEntities();

        public List<Tree2> Lista(byte? MesIni, byte? MesFin, string IdCuenta)
        {
            CuentasDAL cuentas = new CuentasDAL();
            List<Tree2> lst = new List<Tree2>();
            


            List<CA_Cuentas> auxlst = null;
            //Traer todos los registros 
            CA_Cuentas cta = cuentas.GetByID(x => x.Id_Cuenta == IdCuenta);
            //obtener el lvl de la cuenta
            if (IdCuenta == "0")
            {
                auxlst = cuentas.Get(x => x.Grupo == 0).ToList();
            }
            else
            {
                if (cta.Genero > 0 && cta.Grupo == 0) // lvl 1
                    auxlst = cuentas.Get(x => x.Genero == cta.Genero && x.Grupo > 0 && x.Rubro == 0).ToList();

                if (cta.Genero > 0 && cta.Grupo > 0 && cta.Rubro == 0) // lvl 2
                    auxlst = cuentas.Get(x => x.Genero == cta.Genero && x.Grupo == cta.Grupo && x.Rubro > 0 && x.Cuenta == 0).ToList();

                if (cta.Genero > 0 && cta.Grupo > 0 && cta.Rubro > 0 && cta.Cuenta == 0) // lvl 3
                    auxlst = cuentas.Get(x => x.Genero == cta.Genero && x.Grupo == cta.Grupo && x.Rubro == cta.Rubro && x.Cuenta > 0 && x.SubCuentaO1 == 0).ToList();

                if (cta.Genero > 0 && cta.Grupo > 0 && cta.Rubro > 0 && cta.Cuenta > 0 && cta.SubCuentaO1 == 0) // lvl 4
                    auxlst = cuentas.Get(x => x.Genero == cta.Genero && x.Grupo == cta.Grupo && x.Rubro == cta.Rubro && x.Cuenta == cta.Cuenta && x.SubCuentaO1 > 0 && x.SubCuentaO2 == 0).ToList();

                if (cta.Genero > 0 && cta.Grupo > 0 && cta.Rubro > 0 && cta.Cuenta > 0 && cta.SubCuentaO1 > 0 && cta.SubCuentaO2 == 0) // lvl 5
                    auxlst = cuentas.Get(x => x.Genero == cta.Genero && x.Grupo == cta.Grupo && x.Rubro == cta.Rubro && x.Cuenta == cta.Cuenta && x.SubCuentaO1 == cta.SubCuentaO1 && x.SubCuentaO2 > 0 && x.SubCuentaO3 == 0).ToList();

                if (cta.Genero > 0 && cta.Grupo > 0 && cta.Rubro > 0 && cta.Cuenta > 0 && cta.SubCuentaO1 > 0 && cta.SubCuentaO2 > 0 && cta.SubCuentaO3 == 0) // lvl 6
                    auxlst = cuentas.Get(x => x.Genero == cta.Genero && x.Grupo == cta.Grupo && x.Rubro == cta.Rubro && x.Cuenta == cta.Cuenta && x.SubCuentaO1 == cta.SubCuentaO1 && x.SubCuentaO2 == cta.SubCuentaO2 && x.SubCuentaO3 > 0 && x.SubCuentaO4 == 0).ToList();

                if (cta.Genero > 0 && cta.Grupo > 0 && cta.Rubro > 0 && cta.Cuenta > 0 && cta.SubCuentaO1 > 0 && cta.SubCuentaO2 == 0 && cta.SubCuentaO3 > 0 && cta.SubCuentaO4 == 0) // lvl 7
                    auxlst = cuentas.Get(x => x.Genero == cta.Genero && x.Grupo == cta.Grupo && x.Rubro == cta.Rubro && x.Cuenta == cta.Cuenta && x.SubCuentaO1 == cta.SubCuentaO1 && x.SubCuentaO2 == cta.SubCuentaO2 && x.SubCuentaO3 == cta.SubCuentaO3 && x.SubCuentaO4 > 0).ToList();
            }
            //if (cta.Genero > 0 && cta.Grupo == 0 && cta.Rubro > 0 && cta.Cuenta > 0 && cta.SubCuentaO1 > 0 && cta.SubCuentaO2 == 0 && cta.SubCuentaO3 > 0 && cta.SubCuentaO4 > 0) // lvl 8
            //    auxlst = cuentas.Get(x => x.Genero == cta.Genero && x.Grupo == cta.Grupo && x.Rubro == cta.Rubro && x.Cuenta == cta.Cuenta && x.SubCuentaO1 == cta.SubCuentaO1 && x.SubCuentaO2 == cta.SubCuentaO2 && x.SubCuentaO3 == cta.SubCuentaO3 && x.SubCuentaO4 > 0).ToList();

            foreach (CA_Cuentas item in auxlst)
            {
                byte? mesIni = MesIni;
                byte? mesFin = MesFin;

                decimal[] resultadoSp = new ProceduresDAL().PA_BalanzaPorCuenta(item.Id_Cuenta, MesIni, MesFin);
                lst.Add(new Tree2()
                {
                    IdCuenta = item.Id_Cuenta,
                    Genero = item.Genero,
                    Grupo = item.Grupo,
                    Rubro = item.Rubro,
                    Cuenta = item.Cuenta,
                    SubCuentaO1 = item.SubCuentaO1,
                    SubCuentaO2 = item.SubCuentaO2,
                    SubCuentaO3 = item.SubCuentaO3,
                    SubCuentaO4 = item.SubCuentaO4,
                    IdCuentaFormato = item.Id_CuentaFormato + " " + item.Descripcion,
                    SaldoAnteriorDebe = resultadoSp[0],
                    SaldoAnteriorHaber = resultadoSp[1],
                    MovimientoDebe = resultadoSp[2],
                    MovimientoHaber = resultadoSp[3],
                    SaldoActualDebe = resultadoSp[4],
                    SaldoActualHaber = resultadoSp[5],
                    Hijos = item.Nivel,
                    IdCuentaPadre = IdCuenta
                });
            }

            return lst;
        }

        public CA_Cuentas DatosCuentas(string IdCuenta)
        {
            CuentasDAL cuentas = new CuentasDAL();
            return cuentas.GetByID(x => x.Id_Cuenta == IdCuenta);
        }

        public List<De_Polizas> DatosPolizas(string IdCuenta)
        {
            BD_TesoreriaEntities bd = new BD_TesoreriaEntities();
            return bd.De_Polizas.Where(x => x.Id_Cuenta == IdCuenta).ToList();
        }
    }
    public class SaldosIniciales
    {
        [Required(ErrorMessage = "*")]
        public string Id_Cuenta { get; set; }
        [Required(ErrorMessage = "*")]
        public string DescripcionSaldo { get; set; }
        [Required(ErrorMessage = "*")]
        public Int16 Id_TipoMovimiento { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal Importe { get; set; }
        public DateTime Fecha { get; set; }
        public string Id_CuentaFormato { get; set; }
        public string DescCuenta { get; set; }
    }
    public class GuardarMAPoliza
    {
        [Required(ErrorMessage = "*")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime Fecha { get; set; }
    }
    public class PruebaCheques
    {
        public int id { get; set; }
        public string Tipo { get; set; }
        public string Folio { get; set; }
        public string Vencimiento { get; set; }
        public string Proveedor { get; set; }
        public string Ejericio { get; set; }
    }
    /// <summary>
    /// Usado para la generación de los compromisos a partir de los archivos de Excel (Proceso de Egresos). [/Importar/Compromisos]
    /// </summary>
    public class CompromisoNomina
    {
        public string Id_Area { get; set; }
        public string Id_Funcion { get; set; }
        public string Id_Actividad { get; set; }
        public string Id_ClasificacionP { get; set; }
        public string Id_Programa { get; set; }
        public string Id_Proceso { get; set; }
        public string Id_TipoMeta { get; set; }
        public string Id_ActividadMIR { get; set; }
        public string Id_Accion { get; set; }
        public string Id_Alcance { get; set; }
        public string Id_TipoG { get; set; }
        public string Id_Fuente { get; set; }
        public string AnioFin { get; set; }
        public string Id_ObjetoG { get; set; }
        public string Id_ClavePresupuesto { get; set; }
        public string Cuenta { get; set; }
        public decimal Cargo { get; set; }
        public decimal Abono { get; set; }
        public bool Disponibilidad { get; set; }
    }
    #region Clases para importación de Excel de Pago a Proveedores
    /// <summary>
    /// Usado para la generación de los compromisos a partir de los archivos de Excel. [/ExcelCompromisos/Compromisos]
    /// </summary>
    public class PagoProveedoresExcel
    {
        public short? tipoCompromiso { get; set; }
        public String celdatipoCompromiso { get; set; }
        public int consecutivo { get; set; }
        public String celdaConsecutivo { get; set; }
        public String fuenteFinanciamiento { get; set; }
        public int idCuentaBancaria { get; set; }
        public String celdaIdCuentaBancaria { get; set; }
        public String celdaFuenteFinanciamiento { get; set; }
        public int noCheque { get; set; }
        public String celdaNoCheque { get; set; }
        public int spei { get; set; }
        public String celdaSpei { get; set; }
        public DateTime fechaPago { get; set; }
        public String celdaFechaPago { get; set; }
        public int? noBeneficiario { get; set; }
        public String celdaNoBeneficiario { get; set; }
        public String cuentaBeneficiario { get; set; }
        public String celdaCuentaBeneficiario { get; set; }
        public String descripcion { get; set; }
        public String celdaDescripcion { get; set; }
        public decimal totalCargos { get; set; }
        public PagoProveedoresExcel() { }
    }
    /// <summary>
    /// Usado para la generación de los compromisos a partir de los archivos de Excel. [/ExcelCompromisos/Compromisos]
    /// </summary>
    public class DetallesPagoProveedoresExcel
    {
        public String centroGestor { get; set; }
        public String funcion { get; set; }
        public String compromiso { get; set; }
        public String clasificacion { get; set; }
        public String programa { get; set; }
        public String proyecto { get; set; }
        public String tipoMeta { get; set; }
        public String actividadMir { get; set; }
        public String accion { get; set; }
        public String dimensionGeografica { get; set; }
        public String tipoGasto { get; set; }
        public String tipoFuente { get; set; }
        public String AnioFin { get; set; }
        public String objetoGasto { get; set; }
        public decimal cargos { get; set; }
        public String celdaCentroGestor { get; set; }
        public String celdaFuncion { get; set; }
        public String celdaCompromiso { get; set; }
        public String celdaClasificacion { get; set; }
        public String celdaPrograma { get; set; }
        public String celdaProyecto { get; set; }
        public String celdaTipoMeta { get; set; }
        public String celdaTipoFuente { get; set; }

        public String celdaActividadMir { get; set; }
        public String celdaAccion { get; set; }
        public String celdaDimensionGeografica { get; set; }
        public String celdaTipoGasto { get; set; }
        public String celdaAnioFinanciamiento { get; set; }
        public String celdaObjetoGasto { get; set; }
        public String celdaCargos { get; set; }
        public int consecutivo { get; set; }
        public String clavePresupuestaria { get; set; }

        public DetallesPagoProveedoresExcel() { }
    }
    /// <summary>
    /// Usado para la generación de los compromisos a partir de los archivos de Excel. [/ExcelCompromisos/Compromisos]
    /// </summary>
    public class DocumentosPagoProveedores
    {
        public int noProveedor { get; set; }
        public String noFactura { get; set; }
        public int consecutivo { get; set; }
        public DateTime fechaFactura { get; set; }
        public decimal subtotal { get; set; }
        public decimal iva { get; set; }
        public decimal retencionIva { get; set; }
        public decimal retencionIsr { get; set; }
        public decimal retencionObra { get; set; }
        public decimal total { get; set; }
        public String celdaNoProveedor { get; set; }
        public String celdaNoFactura { get; set; }
        public String celdaFechaFactura { get; set; }
        public String celdaSubtotal { get; set; }
        public String celdaIva { get; set; }
        public String celdaRetencionIva { get; set; }
        public String celdaRetencionIsr { get; set; }
        public String celdaRetencionObra { get; set; }
        public String celdaTotal { get; set; }
        public String celdaConsecutivo { get; set; }
        public DocumentosPagoProveedores()
        {
            //this.iva = 0;
            //this.noFactura = 0;
            //this.noProveedor = 0;
            //this.retencionIsr = 0;
            //this.retencionIva = 0;
            //this.retencionObra = 0;
            //this.subtotal = 0;
            //this.total = 0;
        }
    }
    public enum PagoProveedores
    {
        tipoCompromiso = 0,
        consecutivo = 1,
        idCuentaBancaria = 2,
        fuenteFinanciamiento = 3,
        noCheque = 4,
        spei = 5,
        fechaPago = 6,
        noBeneficiario = 7,
        cuentaBeneficiario = 8,
        descripcion = 9
    }
    public enum PagoProveedoresDetalle
    {
        centroGestor = 0,
        funcion = 1,
        compromiso = 2,
        clasificacion = 3,
        programa = 4,
        proyecto = 5,
        tipoMeta = 6,
        actividadMir = 7,
        accion = 8,
        dimensionGeografica = 9,
        tipoGasto = 10,
        tipoFuente = 11,
        AnioFin = 12,
        objetoGasto = 13,
        cargos = 14

    }
    public enum ArrendamientosDetalle
    {
        centroGestor = 0,
        funcion = 1,
        compromiso = 2,
        clasificacion = 3,
        programa = 4,
        proyecto = 5,
        tipoMeta = 6,
        actividadMir = 7,
        accion = 8,
        dimensionGeografica = 9,
        tipoGasto = 10,
        tipoFuente = 11,
        AnioFin = 12,
        objetoGasto = 13,
        cuenta = 14,
        cargos = 15

    }
    public enum PagoProveedoresDocumentos
    {
        noProveedor = 0,
        noFactura = 1,
        fechaFactura = 2,
        subtotal = 3,
        iva = 4,
        retencionIva = 5,
        retencionIsr = 6,
        retencionObra = 7,
        total = 8
    }
    public enum FondosRevolvente
    {
        consecutivo = 0,
        idCuentaBancaria = 1,
        fuenteFinanciamiento = 2,
        noCheque = 3,
        spei = 4,
        fechaPago = 5,
        cuentaBeneficiario = 6,
        importe = 7,
        descripcion = 8,
        fechaComprobacion = 9,
        cuentaPorPagar = 10
    }
    public enum ReciboIngresosEnum
    {
        consecutivo = 0,
        fechaRecaudacion = 1,
        noCajaReceptora = 2,
        noContribuyente = 3,
        importeTotal = 4,
        idCtaBancaria = 5,
        fuenteFinanciamiento = 6,
        descripcion = 7
    }
    public enum DetalleReciboIngresos
    {
        centroRecaudador = 0,
        dimensionGeografica = 1,
        fuenteFinanciamiento = 2,
        anioFinanciamiento = 3,
        cri = 4,
        cur = 5,
        importe = 6
    }
    public enum CancelacionPasivosEnum
    {
        consecutivo = 0,
        noCuentaBancaria = 1,
        fuenteFinanciamiento = 2,
        noCheque = 3,
        spei = 4,
        fechaPago = 5,
        cuentaBeneficiario = 6,
        importeCheque = 7,
        descripcion = 8
    }
    public enum ProveedoresEnum
    {
        cuenta=1,
        nombreCompleto=2,
        nombre = 3,
        apellido1 = 4,
        apellido2 = 5
    }
    public enum EgresosNoPEnum
    {
        consecutivo = 0,
        noCuentaBancaria = 1,
        fuenteFinanciamiento = 2,
        noCheque = 3,
        spei = 4,
        fechaPago = 5,
        noBeneficiario = 6,
        importeCheque = 7,
        descripcion = 8
    }
    public enum DetalleEgresoNoPEnum
    {
        cuenta = 0,
        importe = 1
    }
    public enum ArrendamientoyHonorariosEnum
    {
        //tipoCompromiso = 0,
        consecutivo = 0,
        idCuentaBancaria = 1,
        fuenteFinanciamiento = 2,
        noCheque = 3,
        spei = 4,
        fechaPago = 5,
        noBeneficiario = 6,
        cuentaBeneficiario = 7,
        importeCheque = 8,
        cuentaPA = 9,
        importePA = 10,
        descripcion = 11,
        tipoMovimiento = 12
    }
    public enum HonorariosEnum
    {
        //tipoCompromiso = 0,
        consecutivo = 0,
        idCuentaBancaria = 1,
        fuenteFinanciamiento = 2,
        noCheque = 3,
        spei = 4,
        fechaPago = 5,
        noBeneficiario = 6,
        cuentaBeneficiario = 7,
        importeCheque = 8,
        cuentaPA = 9,
        importePA = 10,
        cuentaPA2 = 11,
        importePA2 = 12,
        cuentaPA3 = 13,
        importePA3 = 14,
        cuentaPA4 = 15,
        importePA4 = 16,
        descripcion = 17,
        tipoMovimiento = 18
    }
    public class Celda
    {
        public String value { get; set; }
        public String cell { get; set; }
        public Celda()
        {
            this.cell = "";
        }
    }
    public class ExcelPagoProveedor
    {
        public List<PagoProveedoresExcel> listaCompromisos { get; set; }
        public List<DetallesPagoProveedoresExcel> listaDetallesCompromiso { get; set; }
        public List<DocumentosPagoProveedores> listaDocumentos { get; set; }
        public List<String> listaErrores { get; set; }

    }
    #endregion
    #region Clases para importación de variosa archivos de Excel
    public class FondosyGastosExcel : PagoProveedoresExcel
    {
        public decimal importeCheque { get; set; }
        public DateTime? fechaComprobacion { get; set; }
        public String cuentaPorPagar { get; set; }
        public String celdaCuentaPorPagar { get; set; }
        public string celdaImporte { get; set; }
        public string celdaFechaComprobacion { get; set; }
    }
    public class ReciboIngresosExcel
    {
        public int noRecibo { get; set; }
        public DateTime fechaRecaudacion { get; set; }
        public int noCajaReceptora { get; set; }
        public int noContribuyente { get; set; }
        public decimal importe { get; set; }
        public int idCtaBancaria { get; set; }
        public String fuenteFinanciamiento { get; set; }
        public String descripcion { get; set; }
        public String celdaNoRecibo { get; set; }
        public String celdaFechaRecaudacion { get; set; }
        public String celdaNoCajaReceptora { get; set; }
        public String celdaNoContribuyente { get; set; }
        public String celdaImporte { get; set; }
        public String celdaFuenteFinanciamiento { get; set; }
        public String celdaDescripcion { get; set; }
        public String celdaIdCtaBancaria { get; set; }
    }
    public class CancelacionPasivosExcel
    {
        public int consecutivo { get; set; }
        public int noCuentaBancaria { get; set; }
        public String fuenteFinanciamiento { get; set; }
        public int noCheque { get; set; }
        public int spei { get; set; }
        public DateTime fechaPago { get; set; }
        public String cuentaBeneficiario { get; set; }
        public decimal importe { get; set; }
        public String descripcion { get; set; }
        public String celdaConsecutivo { get; set; }
        public String celdaNoCtaBancaria{ get; set; }
        public String celdaFuenteFinanciamiento{ get; set; }
        public String celdaFechaPago { get; set; }
        public String celdaSpei { get; set; }
        public String celdaNoCheque { get; set; }
        public String celdaCuentaBeneficiario { get; set; }
        public String celdaImporte { get; set; }
        public String celdaDescripcion { get; set; }
    }

    public class EgresosNoPresupuestalesExcel : PagoProveedoresExcel
    {
        public decimal importeCheque { get; set; }
        public String celdaImporteCheque { get; set; }
    }
    public class ArrendamientosyHonorariosExcel : PagoProveedoresExcel
    {
        public decimal importeCheque { get; set; }
        public string cuentaActivoPasivo { get; set; }
        public decimal importeActivoP{ get; set; }
        public string cuentaActivoPasivo2 { get; set; }
        public decimal importeActivoP2 { get; set; }
        public string cuentaActivoPasivo3 { get; set; }
        public decimal importeActivoP3 { get; set; }
        public string cuentaActivoPasivo4 { get; set; }
        public decimal importeActivoP4 { get; set; }
        public String celdaImporteCheque { get; set; }
        public String celdaCuentaActivoP{ get; set; }
        public String celdaImporteActivoP { get; set; }
        public String celdaCuentaActivoP2 { get; set; }
        public String celdaImporteActivoP2 { get; set; }
        public String celdaCuentaActivoP3 { get; set; }
        public String celdaImporteActivoP3 { get; set; }
        public String celdaCuentaActivoP4 { get; set; }
        public String celdaImporteActivoP4 { get; set; }
        public int TipoMovimiento { get; set; }
        public String celdaTipoMovimiento { get; set; }
        public int movimiento { get; set; }
    }
    public class DetallesEgresosNoPresupuestales
    {
        public int consecutivo { get; set; }
        public String cuenta { get; set; }
        public decimal importe { get; set; }
        public String celdaCuenta { get; set; }
        public String celdaImporte { get; set; }
    }
    public class DetallesFondosyGastos : DetallesPagoProveedoresExcel { }
    public class DetalleArrendamientos : DetallesPagoProveedoresExcel
    {
        public String cuenta { get; set; }
        public decimal abonos { get; set; }
        public String celdaCuenta { get; set; }
        public String celdaAbonos{ get; set; }
    }
    public class DocumentosFondosRevolventes : DocumentosPagoProveedores { }
    public class ExcelFondosRevolventes
    {
        public List<FondosyGastosExcel> listaFondosRevolventes { get; set; }
        public List<DetallesFondosyGastos> listaDetallesFondos { get; set; }
        public List<DocumentosPagoProveedores> listaDocumentosFondos { get; set; }
        public List<String> listaErrores { get; set; }
    }
    public class ExcelReciboIngresos
    {
        public List<ReciboIngresosExcel> listaReciboIngresos { get; set; }
        public List<DetalleReciboIngreso> listaDetallesReciboIngresos { get; set; }
        public List<String> listaErrores { get; set; }
    }
    public class ExcelCancelacionPasivos
    {
        public List<CancelacionPasivosExcel> cancelaciones { get; set; }
        public List<String> errores{ get; set; }
    }
    public class ExcelEgresosNoPresupuestarios
    {
        public List<EgresosNoPresupuestalesExcel> egresos { get; set; }
        public List<DetallesEgresosNoPresupuestales> detalles { get; set; }
        public List<String> errores { get; set; }
    }
    public class ExcelArrendamientosyHonorarios
    {
        public List<ArrendamientosyHonorariosExcel> arrendamientos { get; set; }
        public List<DetalleArrendamientos> detalles { get; set; }
        public List<DocumentosPagoProveedores> documentos { get; set; }
        public List<String> errores { get; set; }
    }
    public class ExcelBeneficiaros
    {
        public List<BeneficiariosExcel> beneficiarios { get; set; }
        public string NoCuenta { get; set; }
        public List<String> errores { get; set; }
    }
    #endregion
    #region Clases para importación de Excel y Recibo de Ingresos

    public class BeneficiariosExcel
    {
        public string nombreCompleto { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public String celdaNombreCompleto { get; set; }
        public String celdaNombre { get; set; }
        public String celdaApellido1 { get; set; }
        public String celdaApellido2 { get; set; }

    }
    public class ReciboIngreso
    {
        public int consecutivo { get; set; }
        public String celdaConsecutivo { get; set; }
        public DateTime fechaRecaudacion { get; set; }
        public String celdaFechaRecaudacion { get; set; }
        public int noCajaReceptora { get; set; }
        public String celdaNoCajaReceptora { get; set; }
        public int noContribuyente { get; set; }
        public String celdaNoContribuyente { get; set; }
        public decimal importeTotal { get; set; }
        public String celdaImporte { get; set; }
        public String fuenteFinanciamiento { get; set; }
        public String celdaFuente { get; set; }
        public String descripcion { get; set; }
        public String celdaDescripcion { get; set; }
    }
    public class DetalleReciboIngreso
    {
        public String centroRecaudador { get; set; }
        public String celdaCentroRecaudador { get; set; }
        public String dimension { get; set; }
        public String celdaDimension { get; set; }
        public String fuenteFinanciamiento { get; set; }
        public String celdaFuenteFinanciamiento { get; set; }
        public String anioFinanciamiento { get; set; }
        public String celdaAnioFinanciamiento { get; set; }
        public String cri { get; set; }
        public String celdaCri { get; set; }
        public String cur { get; set; }
        public String celdaCur { get; set; }
        public decimal importe { get; set; }
        public String celdaImporte { get; set; }
        public string clavePresupuestaria { get; set; }
        public int noRecibo { get; set; }
    }
    #endregion
    public class Egresos
    {
        public string Cuenta { get; set; }
        public decimal Cargos { get; set; }
        public decimal Abonos { get; set; }
    }
    public class Cheques
    {
        public int Id_TipoCR { get; set; }
        public int Id_FolioCR { get; set; }
    }
    public class ResetControlFinanciero
    {
        public string Mes { get; set; }
        [Display(Name = "Cuenta Bancaria")]
        [Required(ErrorMessage = "*")]
        public int IdCtaBancaria { get; set; }
        [Required(ErrorMessage = "*")]
        public Nullable<int> IdMes { get; set; }
        public Nullable<int> Estatus { get; set; }
        public SelectList Lista_CtaBancarias { get; set; }
        public bool borrado { get; set; }
        public Nullable<decimal> Desde { get; set; }
        public Nullable<decimal> Saldo { get; set; }
        public Nullable<decimal> Hasta { get; set; }
        [Display(Name = "Tipo de Movimiento")]
        public Nullable<Int16> TipoMovimiento { get; set; }
        [Display(Name = "Folio de Movimiento")]
        public Nullable<Int16> FolioMovimiento { get; set; }
        public SelectList Lista_TipoMovimiento { get; set; }
        public SelectList Lista_FolioMovimiento { get; set; }
        public SelectList Lista_Meses { get; set; }
        public SelectList Lista_Estatus { get; set; }

        public ResetControlFinanciero()
        {
            CuentasBancariasDAL dalCtaBancarias = new CuentasBancariasDAL();
            TipoMovBancariosDAL dalTipos = new TipoMovBancariosDAL();
            this.Lista_CtaBancarias = new SelectList(dalCtaBancarias.Get(), "Id_CtaBancaria", "Descripcion");
            this.Lista_FolioMovimiento = new SelectList(dalTipos.Get(), "Id_MovBancario", "Descripcion");
            this.Lista_TipoMovimiento = new SelectList(Diccionarios.TipoMovimientoBancario, "key", "value");
            this.Lista_Meses = new SelectList(Diccionarios.Meses, "key", "value");
            this.Lista_Estatus = new SelectList(Diccionarios.EstatusMovimientoBancario, "key", "value");
        }

    }
    public class ReporteConciliacion
    {
        public Nullable<decimal> SaldoEstadoCuenta { get; set; }
        public Nullable<decimal> SaldoMesAnterior { get; set; }
        public Nullable<decimal> Cargos { get; set; }
        public Nullable<decimal> Abonos { get; set; }
        public List<tblRepConciliacion> ListaSaldos { get; set; }
        public List<tblRepConciliacion> ListaCargos { get; set; }
        public List<tblRepConciliacion> ListaAbonos { get; set; }
        public ReporteConciliacion()
        {
            this.ListaCargos = new List<tblRepConciliacion>();
            this.ListaAbonos = new List<tblRepConciliacion>();
        }
    }
    public class BusquedaDisponibilidadAvanzada
    {
        public Nullable<bool> Todos { get; set; }
        public Nullable<bool> Aprobado { get; set; }
        public Nullable<bool> Ampliaciones { get; set; }
        public Nullable<bool> Reducciones { get; set; }
        public Nullable<bool> Modificado { get; set; }
        public Nullable<bool> PreComprometido { get; set; }
        public Nullable<bool> Comprometido { get; set; }
        public Nullable<bool> Devengado { get; set; }
        public Nullable<bool> Ejercido { get; set; }
        public Nullable<bool> Pagado { get; set; }
        public Nullable<bool> Disponible { get; set; }

        public BusquedaDisponibilidadAvanzada()
        {
            this.Todos = false;
            this.Aprobado = false;
            this.Ampliaciones = false;
            this.Reducciones = false;
            this.Modificado = false;
            this.PreComprometido = false;
            this.Comprometido = false;
            this.Devengado = false;
            this.Ejercido = false;
            this.Pagado = false;
            this.Disponible = false;
        }

    }
    public class BusquedaConciliacionAuto
    {
        public Int32 NoCuenta { get; set; }
        public DateTime Fecha { get; set; }
        public Int16 Sucursal { get; set; }
        public string Descripcion { get; set; }
        public char Tipo { get; set; }
        public Int16 Referencia { get; set; }
        public string Concepto { get; set; }
        [Display(Name = "Mes")]
        [Required(ErrorMessage = "*")]
        public Nullable<Int32> IdMes { get; set; }

        [Display(Name = "Banco")]
        [Required(ErrorMessage = "*")]
        public Int32 IdBanco { get; set; }

        [Display(Name = "Cuenta Bancaria")]
        [Required(ErrorMessage = "*")]
        public Nullable<Int32> IdCtaBancaria { get; set; }

        public Nullable<bool> Estatus { get; set; }
        public Nullable<decimal> Desde { get; set; }
        public Nullable<decimal> Hasta { get; set; }
        public SelectList Lista_Meses { get; set; }
        public SelectList Lista_Bancos { get; set; }
        public SelectList Lista_CtaBancarias { get; set; }
        public SelectList Lista_Estatus { get; set; }

        public BusquedaConciliacionAuto()
        {
            CuentasBancariasDAL dalCtaBancarias = new CuentasBancariasDAL();
            BancosDAL dalbancos = new BancosDAL();
            this.Lista_Bancos = new SelectList(dalbancos.Get(), "Id_Banco", "Descripcion");
            this.Lista_CtaBancarias = new SelectList(dalCtaBancarias.Get(), "Id_CtaBancaria", "Descripcion");
            this.Lista_Meses = new SelectList(Diccionarios.Meses, "key", "value");
            this.Lista_Estatus = new SelectList(Diccionarios.EstatusEstadosCuenta, "key", "value");
        }

    }
    public static class ReporteClavePresupuestaria
    {
        public static List<int> ObjetoGato()
        {
            List<int> ListaObjetoGasto = new List<int>(){10000,11000,12000,13000,14000,15000,16000,17000,20000,21000,22000,23000,24000,25000,26000,27000,28000,29000,30000,31000,32000,33000,34000,35000,36000,37000,38000,39000,40000,41000,42000,
                                                        43000,44000,45000,46000,47000,48000,49000,50000,51000,52000,53000,54000,55000,56000,57000,58000,59000,60000,61000,62000,63000,70000,71000,72000,73000,74000,75000,76000,79000,80000,81000,
                                                        83000,85000,90000,91000,92000,93000,94000,95000,96000,99000};
            return ListaObjetoGasto;
        }
        public static List<int> Funcion()
        {
            List<int> ListaFunciones = new List<int>(){100,110,120,130,140,150,160,170,180,200,210,220,230,240,250,260,270,300,310,320,330,340,350,360,370,380,390,400,410,420,430,440};
            return ListaFunciones;
        }
        public static List<string> Cri()
        {
            List<string> ListaCri = new List<string>() { "1", "2", "3","4","5","51","52","6","61","62","7","71","8","9","0" };
            return ListaCri;
        }
    }
    public static class ReporteProveedores
    {
        public static List<string> ListaCeldas()
        {
            List<string> ListaCeldas = new List<string>() { "Tipo CR-30", "Folio CR-30", "Fecha Pago-30", "RFC-30", "Proveedor-40", "CURP-30", "Estado-30", "Municipio-30", "Localidad-30", "CP-30", "Colonia-30", "Domicilio-30", "Docto-30", "No. Docto-30"
                                                            , "Fecha Factura-30", "SubTotal-30", "Ret ISR-30", "Ret. IVA-30", "IVA-30", "Ret_Obra-30", "Deducción-30", "Importe Deducción-30", "Impuestos-30", "Importe Impuestos-30", "Total-30"
                                                            , "Tipo Beneficiario-60", "Clasificación Beneficiario-60"};
            return ListaCeldas;
        }
        public static List<string> ListaCeldasAgrupado()
        {
            List<string> ListaCeldas = new List<string>() { "Proveedor-30", "SubTotal-30", "IVA-30", "ISR-30", "Ret.IVA-40", "Ret. Obra-30", "Otras Deducciones-30", "Otros Impuestos-30", "Total-30", "RFC-30", "CURP-30", "Estado-30", "Municipio-30", "Localidad-30"
                                                            , "CP-30", "Colonia-30", "Domicilio-30", "Tipo Beneficiario-60", "Clasificación Beneficiario-60"};
            return ListaCeldas;
        }
    }
    public class ReporteClavePresupuestariaModel
    {
        public string Clave { get; set; }
        public string Concepto { get; set; }
        public decimal Aprobado { get; set; }
        public decimal AmpRed { get; set; }
        public decimal Modificado { get; set; }
        public decimal Devengado { get; set; }
        public decimal Pagado { get; set; }
        public decimal Subejercicio { get; set; }
        public decimal Estimado { get; set; }
        public decimal Recaudado { get; set; }
        public decimal Diferencia { get; set; }
        public bool ultimoNivel { get; set; }
    }
    public class jsonDefault
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
    }
}
