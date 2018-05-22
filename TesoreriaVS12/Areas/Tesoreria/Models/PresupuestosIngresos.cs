using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;

namespace TesoreriaVS12.Areas.Tesoreria.Models
{
    public class ClaseJson
    {
        public List<String> Descripcion { get; set; }
        public List<String> Ids { get; set; }
    }
    public class PresupuestosIngresos
    {
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        private static CentroRecaudadorDAL DALCentro = new CentroRecaudadorDAL();
        private static FuenteIngDAL DALFuenteFin = new FuenteIngDAL();
        private static ConceptosIngresosDAL DALConceptos = new ConceptosIngresosDAL();
        private static AlcanceDAL DALAlcance = new AlcanceDAL();
        private static MaPresupuestoIngDAL DALPresupuesto = new MaPresupuestoIngDAL();
        public ClaseJson  GetDescripcion(string id, string objeto, string parametros, string tipo)
        {
            ClaseJson ClaseJson = new Models.ClaseJson();
            int o = Convert.ToInt16(objeto);
            MaPresupuestoIngDAL DALPrespuesto = new MaPresupuestoIngDAL();
            List<MA_PresupuestoEg> presupuesto = new List<MA_PresupuestoEg>();
            string[] arrParametros = null;
            List<String> dataModel = new List<String>();
            List<String> dataIds = new List<string>();
            if (!String.IsNullOrEmpty(parametros))
            {
                arrParametros = parametros.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            }
            switch (o)
            {
                case 1:
                    if (tipo == "2")
                    {
                        List<Ma_PresupuestoIng> resultados = DALPresupuesto.Get(x => (id == "-" ? x.Id_CentroRecaudador!= null : x.Id_CentroRecaudador.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_CentroRecaudador temp = DALCentro.GetByID(x=>x.Id_CRecaudador == item.Id_CentroRecaudador);
                            if (!dataIds.Contains(temp.Id_CRecaudador))
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_CRecaudador, temp.Descripcion));
                                dataIds.Add(temp.Id_CRecaudador);
                            }
                        }
                    }
                    else
                    {
                        var Centros = DALCentro.Get(x => x.UltimoNivel == true && (x.Id_CRecaudador.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id))).OrderBy(x => x.Id_CRecaudador).GroupBy(x => x.Id_CRecaudador).Take(15);
                        foreach (var item in Centros)
                        {
                            Ca_CentroRecaudador temp = DALCentro.GetByID(x => x.Id_CRecaudador == item.Key);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_CRecaudador, temp.Descripcion));
                            dataIds.Add(temp.Id_CRecaudador);
                        }
                    }
                    ClaseJson.Descripcion= dataModel;
                    ClaseJson.Ids = dataIds;
                    break;
                case 2:
                    if (tipo == "2")
                    {
                        List<Ma_PresupuestoIng> resultados = DALPresupuesto.Get(x => (id == "-" ? x.Id_CentroRecaudador != null : x.Id_Fuente.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_FuentesFin_Ing temp = DALFuenteFin.GetByID(x => x.Id_FuenteFinancia == item.Id_Fuente);
                            if (!dataIds.Contains(temp.Id_FuenteFinancia))
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_FuenteFinancia, temp.Descripcion));
                                dataIds.Add(temp.Id_FuenteFinancia);
                            }
                        }
                    }
                    else
                    {
                        var Fuentes = DALFuenteFin.Get(x => x.UltimoNivel == true && (x.Id_FuenteFinancia.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id))).OrderBy(x => x.Id_FuenteFinancia).GroupBy(x => x.Id_FuenteFinancia).Take(15);
                        foreach (var item in Fuentes)
                        {
                            Ca_FuentesFin_Ing temp = DALFuenteFin.GetByID(x => x.Id_FuenteFinancia == item.Key);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_FuenteFinancia, temp.Descripcion));
                            dataIds.Add(temp.Id_FuenteFinancia);
                        }
                    }
                    ClaseJson.Descripcion= dataModel;
                    ClaseJson.Ids = dataIds;
                    break;
                case 3:
                    if (tipo == "2")
                    {
                        List<Ma_PresupuestoIng> resultados = DALPresupuesto.Get(x => (id == "-" ? x.Id_CentroRecaudador != null : x.Id_Alcance.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_AlcanceGeo temp = DALAlcance.GetByID(x => x.Id_AlcanceGeo == item.Id_Alcance);
                            if (!dataIds.Contains(temp.Id_AlcanceGeo))
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_AlcanceGeo, temp.Descripcion));
                                dataIds.Add(temp.Id_AlcanceGeo);
                            }
                        }
                    }
                    else
                    {
                        var Alcances = DALAlcance.Get(x => x.Id_AlcanceGeo.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id)).OrderBy(x => x.Id_AlcanceGeo).GroupBy(x => x.Id_AlcanceGeo).Take(15);
                        foreach (var item in Alcances)
                        {
                            Ca_AlcanceGeo temp = DALAlcance.GetByID(x => x.Id_AlcanceGeo == item.Key);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_AlcanceGeo, temp.Descripcion));
                            dataIds.Add(temp.Id_AlcanceGeo);
                        }
                    }
                    ClaseJson.Descripcion= dataModel;
                    ClaseJson.Ids = dataIds;
                    break;
                case 4:
                    if (tipo == "2")
                    {
                        List<Ma_PresupuestoIng> resultados = DALPresupuesto.Get(x => (id == "-" ? x.Id_CentroRecaudador != null : x.Id_Concepto.Contains(id))).ToList();
                        foreach (var item in resultados)
                        {
                            Ca_ConceptosIngresos temp = DALConceptos.GetByID(x => x.Id_Concepto == item.Id_Concepto);
                            if (!dataIds.Contains(temp.Id_Concepto))
                            {
                                dataModel.Add(String.Format("{0}-{1}", temp.Id_Concepto, temp.Descripcion));
                                dataIds.Add(temp.Id_Concepto);
                            }
                        }
                    }
                    else
                    {
                        var Conceptos = DALConceptos.Get(x => x.UltimoNivel == true && (x.Id_Concepto.Contains(id) || id == "-" ? x.Descripcion != null : x.Descripcion.Contains(id))).OrderBy(x => x.Id_Concepto).GroupBy(x => x.Id_Concepto).Take(15);
                        foreach (var item in Conceptos)
                        {
                            Ca_ConceptosIngresos temp = DALConceptos.GetByID(x => x.Id_Concepto == item.Key);
                            dataModel.Add(String.Format("{0}-{1}", temp.Id_Concepto, temp.Descripcion));
                            dataIds.Add(temp.Id_Concepto);
                        }
                    }
                    ClaseJson.Descripcion= dataModel;
                    ClaseJson.Ids = dataIds;
                    break;
                case 5:
                    if (tipo == "2")
                    {
                        List<Ma_PresupuestoIng> resultados = DALPresupuesto.Get(x => id == "-" ? x.AnioFin != null : x.AnioFin.Contains(id)).ToList();
                        foreach (var item in resultados.GroupBy(x=>x.AnioFin))
                        {
                            dataModel.Add(String.Format("{0}-{1}", item.Key, Convert.ToInt16(item.Key) + 2000));
                            dataIds.Add(item.Key);
                        }
                    }
                    ClaseJson.Descripcion= dataModel;
                    ClaseJson.Ids = dataIds;
                    break;
            }
            return ClaseJson;
        }
        public static List<PresupuestosIngresos> ListaIngresos(string clave, string descripcion, string tipo, int tipoBusqueda = 0)
        {
            List<PresupuestosIngresos> lista = new List<PresupuestosIngresos>();
            int o = Convert.ToInt16(tipo);
            switch (o)
            {
                case 1:
                    List<Ca_CentroRecaudador> areas = new List<Ca_CentroRecaudador>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        areas = DALCentro.Get(x => (tipoBusqueda == 1 ? x.Id_CRecaudador.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_CRecaudador.Contains(clave) : x.Id_CRecaudador.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        areas = DALCentro.Get(x => (tipoBusqueda == 1 ? x.Id_CRecaudador.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_CRecaudador.Contains(clave) : x.Id_CRecaudador.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        areas = DALCentro.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        areas = DALCentro.Get().ToList();
                    }
                    foreach (var a in areas)
                    {
                        PresupuestosIngresos p = new PresupuestosIngresos();
                        p.Clave = a.Id_CRecaudador;
                        p.Descripcion = a.Descripcion;
                        lista.Add(p);
                    }
                    break;
                case 2:
                    List<Ca_FuentesFin_Ing> fuentes = new List<Ca_FuentesFin_Ing>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        fuentes = DALFuenteFin.Get(x => (tipoBusqueda == 1 ? x.Id_FuenteFinancia.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_FuenteFinancia.Contains(clave) : x.Id_FuenteFinancia.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        fuentes = DALFuenteFin.Get(x => (tipoBusqueda == 1 ? x.Id_FuenteFinancia.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_FuenteFinancia.Contains(clave) : x.Id_FuenteFinancia.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        fuentes = DALFuenteFin.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        fuentes = DALFuenteFin.Get().ToList();
                    }
                    foreach (var a in fuentes)
                    {
                        PresupuestosIngresos p = new PresupuestosIngresos();
                        p.Clave = a.Id_FuenteFinancia;
                        p.Descripcion = a.Descripcion;
                        lista.Add(p);
                    }
                    break;
                case 3:
                    List<Ca_AlcanceGeo> alcances = new List<Ca_AlcanceGeo>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        alcances = DALAlcance.Get(x => (tipoBusqueda == 1 ? x.Id_AlcanceGeo.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_AlcanceGeo.Contains(clave) : x.Id_AlcanceGeo.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        alcances = DALAlcance.Get(x => (tipoBusqueda == 1 ? x.Id_AlcanceGeo.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_AlcanceGeo.Contains(clave) : x.Id_AlcanceGeo.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        alcances = DALAlcance.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        alcances = DALAlcance.Get().ToList();
                    }
                    foreach (var a in alcances)
                    {
                        PresupuestosIngresos p = new PresupuestosIngresos();
                        p.Clave = a.Id_AlcanceGeo;
                        p.Descripcion = a.Descripcion;
                        lista.Add(p);
                    }
                    break;
                case 4:
                    List<Ca_ConceptosIngresos> concepto = new List<Ca_ConceptosIngresos>();
                    if (!String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        concepto = DALConceptos.Get(x => (tipoBusqueda == 1 ? x.Id_Concepto.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Concepto.Contains(clave) : x.Id_Concepto.EndsWith(clave))).ToList();
                    }
                    if (!String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        concepto = DALConceptos.Get(x => (tipoBusqueda == 1 ? x.Id_Concepto.StartsWith(clave) : tipoBusqueda == 2 ? x.Id_Concepto.Contains(clave) : x.Id_Concepto.EndsWith(clave)) && (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && !String.IsNullOrEmpty(descripcion))
                    {
                        concepto = DALConceptos.Get(x => (tipoBusqueda == 1 ? x.Descripcion.StartsWith(descripcion) : tipoBusqueda == 2 ? x.Descripcion.Contains(descripcion) : x.Descripcion.EndsWith(descripcion))).ToList();
                    }
                    if (String.IsNullOrEmpty(clave) && String.IsNullOrEmpty(descripcion))
                    {
                        concepto = DALConceptos.Get().ToList();
                    }
                    foreach (var a in concepto)
                    {
                        PresupuestosIngresos p = new PresupuestosIngresos();
                        p.Clave = a.Id_Concepto;
                        p.Descripcion = a.Descripcion;
                        lista.Add(p);
                    }
                    break;
            }
            return lista;
        }
        public static List<Ma_PresupuestoIng> BuscarPresupuesto(ModalPresupuestoIngModel presupuesto)
        {
            List<Ma_PresupuestoIng> listaPresupuestos = new List<Ma_PresupuestoIng>();
            try
            {
                MaPresupuestoIngDAL pDal = new MaPresupuestoIngDAL();
                listaPresupuestos = pDal.Get(x => (!String.IsNullOrEmpty(presupuesto.Id_CentroRecaudadorModal) ? x.Id_CentroRecaudador == presupuesto.Id_CentroRecaudadorModal : x.Id_CentroRecaudador != null) &&
                    (!String.IsNullOrEmpty(presupuesto.Id_FuenteModal) ? x.Id_Fuente== presupuesto.Id_FuenteModal: x.Id_Fuente!= null) &&
                    (!String.IsNullOrEmpty(presupuesto.AnioFinModal) ? x.AnioFin== presupuesto.AnioFinModal: x.AnioFin!= null) &&
                    (!String.IsNullOrEmpty(presupuesto.Id_AlcanceModal) ? x.Id_Alcance== presupuesto.Id_AlcanceModal: x.Id_Alcance!= null) &&
                    (!String.IsNullOrEmpty(presupuesto.Id_ConceptoModal) ? x.Id_Concepto== presupuesto.Id_ConceptoModal: x.Id_Concepto!= null)).ToList();
            }
            catch (Exception ex)
            {
                return listaPresupuestos;
            }

            return listaPresupuestos;
        }
    }
}