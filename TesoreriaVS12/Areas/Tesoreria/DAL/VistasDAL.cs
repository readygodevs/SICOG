using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.Models;
using TesoreriaVS12.DAL;
using System.Data;
using TesoreriaVS12.Models;
using System.Data.Objects;

namespace TesoreriaVS12.Areas.Tesoreria.DAL.Vistas
{

    public class VerificaPolizaDAL : DALGeneric
    {
        public IEnumerable<vVerificaPoliza> Get(System.Linq.Expressions.Expression<Func<vVerificaPoliza, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.vVerificaPoliza;
            return this.Db.vVerificaPoliza.Where(filter);
        }

        public vVerificaPoliza GetByID(System.Linq.Expressions.Expression<Func<vVerificaPoliza, bool>> filter = null)
        {
            return this.Db.vVerificaPoliza.SingleOrDefault(filter);
        }
    }

    public class VWBeneficiariosDAL : DALGeneric
    {
        public IEnumerable<VW_Beneficiarios> Get(System.Linq.Expressions.Expression<Func<VW_Beneficiarios, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.VW_Beneficiarios;
            return this.Db.VW_Beneficiarios.Where(filter);
        }

        public VW_Beneficiarios GetByID(System.Linq.Expressions.Expression<Func<VW_Beneficiarios, bool>> filter = null)
        {
            return this.Db.VW_Beneficiarios.SingleOrDefault(filter);
        }
    }
    public class ContrarecibosSinFuenteDAL: DALGeneric
    {
        public IEnumerable<VW_contrarecibosSinFuente> Get(System.Linq.Expressions.Expression<Func<VW_contrarecibosSinFuente, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.VW_contrarecibosSinFuente;
            return this.Db.VW_contrarecibosSinFuente.Where(filter);
        }

        public VW_contrarecibosSinFuente GetByID(System.Linq.Expressions.Expression<Func<VW_contrarecibosSinFuente, bool>> filter = null)
        {
            return this.Db.VW_contrarecibosSinFuente.SingleOrDefault(filter);
        }
    }

    public class FechaAsignacionPPyPDDAL : DALGeneric
    {
        public IEnumerable<vFechaAsignacionPPyPD> Get(System.Linq.Expressions.Expression<Func<vFechaAsignacionPPyPD, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.vFechaAsignacionPPyPD;
            return this.Db.vFechaAsignacionPPyPD.Where(filter);
        }

        public vFechaAsignacionPPyPD GetByID(System.Linq.Expressions.Expression<Func<vFechaAsignacionPPyPD, bool>> filter = null)
        {
            return this.Db.vFechaAsignacionPPyPD.SingleOrDefault(filter);
        }
    }

    public class FechasAsignacionFRyGCDAL : DALGeneric
    {
        public IEnumerable<vFechasAsignacionFRyGC> Get(System.Linq.Expressions.Expression<Func<vFechasAsignacionFRyGC, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.vFechasAsignacionFRyGC;
            return this.Db.vFechasAsignacionFRyGC.Where(filter);
        }

        public vFechasAsignacionFRyGC GetByID(System.Linq.Expressions.Expression<Func<vFechasAsignacionFRyGC, bool>> filter = null)
        {
            return this.Db.vFechasAsignacionFRyGC.SingleOrDefault(filter);
        }
    }

    public class BancosImpresionChequesDAL : DALGeneric
    {
        public IEnumerable<vBancosImpresionCheques> Get(System.Linq.Expressions.Expression<Func<vBancosImpresionCheques, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.vBancosImpresionCheques;
            return this.Db.vBancosImpresionCheques.Where(filter);
        }

        public vBancosImpresionCheques GetByID(System.Linq.Expressions.Expression<Func<vBancosImpresionCheques, bool>> filter = null)
        {
            return this.Db.vBancosImpresionCheques.SingleOrDefault(filter);
        }
    }

    public class ListaChequesImpresionDAL : DALGeneric
    {
        public IEnumerable<vListaChequesImpresion> Get(System.Linq.Expressions.Expression<Func<vListaChequesImpresion, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.vListaChequesImpresion;
            return this.Db.vListaChequesImpresion.Where(filter);
        }

        public vListaChequesImpresion GetByID(System.Linq.Expressions.Expression<Func<vListaChequesImpresion, bool>> filter = null)
        {
            return this.Db.vListaChequesImpresion.SingleOrDefault(filter);
        }
    }

    public class DisponibilidadAnualDAL : DALGeneric
    {
        public IEnumerable<vDisponibilidadAnual> Get(System.Linq.Expressions.Expression<Func<vDisponibilidadAnual, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.vDisponibilidadAnual;
            return this.Db.vDisponibilidadAnual.Where(filter);
        }

        public vDisponibilidadAnual GetByID(System.Linq.Expressions.Expression<Func<vDisponibilidadAnual, bool>> filter = null)
        {
            return this.Db.vDisponibilidadAnual.SingleOrDefault(filter);
        }
    }

    public class EvolucionAnualDAL : DALGeneric
    {
        public IEnumerable<vEvolucionAnual> Get(System.Linq.Expressions.Expression<Func<vEvolucionAnual, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.vEvolucionAnual;
            return this.Db.vEvolucionAnual.Where(filter);
        }

        public vEvolucionAnual GetByID(System.Linq.Expressions.Expression<Func<vEvolucionAnual, bool>> filter = null)
        {
            return this.Db.vEvolucionAnual.SingleOrDefault(filter);
        }
    }


    public class ContraRecibosDAL : DALGeneric
    {
        public IEnumerable<VW_Contrarecibos> Get(System.Linq.Expressions.Expression<Func<VW_Contrarecibos, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.VW_Contrarecibos;
            return this.Db.VW_Contrarecibos.Where(filter);
        }

        public VW_Contrarecibos GetByID(System.Linq.Expressions.Expression<Func<VW_Contrarecibos, bool>> filter = null)
        {
            return this.Db.VW_Contrarecibos.SingleOrDefault(filter);
        }
    }
    public class vReciboIngresosDAL : DALGeneric
    {
        public IEnumerable<VW_DetalleReciboIng> Get(System.Linq.Expressions.Expression<Func<VW_DetalleReciboIng, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.VW_DetalleReciboIng;
            return this.Db.VW_DetalleReciboIng.Where(filter);
        }

        public VW_DetalleReciboIng GetByID(System.Linq.Expressions.Expression<Func<VW_DetalleReciboIng, bool>> filter = null)
        {
            return this.Db.VW_DetalleReciboIng.SingleOrDefault(filter);
        }
    }
    public class VW_DisponibilidadMesesDAL : DALGeneric
    {
        public IEnumerable<VW_DisponibilidadMeses> Get(System.Linq.Expressions.Expression<Func<VW_DisponibilidadMeses, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.VW_DisponibilidadMeses;
            return this.Db.VW_DisponibilidadMeses.Where(filter);
        }

        public VW_DisponibilidadMeses GetByID(System.Linq.Expressions.Expression<Func<VW_DisponibilidadMeses, bool>> filter = null)
        {
            return this.Db.VW_DisponibilidadMeses.SingleOrDefault(filter);
        }
    }
    public class VW_ListaContraRecibosAsignacionChequesDAL : DALGeneric
    {
        public IEnumerable<VW_ListaContraRecibosAsignacionCheques> Get(System.Linq.Expressions.Expression<Func<VW_ListaContraRecibosAsignacionCheques, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.VW_ListaContraRecibosAsignacionCheques;
            return this.Db.VW_ListaContraRecibosAsignacionCheques.Where(filter);
        }
    }
    public class VW_ConciliacionDAL : DALGeneric
    {
        public IEnumerable<VW_Conciliacion> Get(System.Linq.Expressions.Expression<Func<VW_Conciliacion, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.VW_Conciliacion;
            return this.Db.VW_Conciliacion.Where(filter);
        }
        public VW_Conciliacion GetById(System.Linq.Expressions.Expression<Func<VW_Conciliacion, bool>> filter = null)
        {
            return this.Db.VW_Conciliacion.SingleOrDefault(filter);
        }
    }
    public class VW_ProvedoresUsadosDAL : DALGeneric
    {
        public IEnumerable<VW_ProvedoresUsados> Get(System.Linq.Expressions.Expression<Func<VW_ProvedoresUsados, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.VW_ProvedoresUsados;
            return this.Db.VW_ProvedoresUsados.Where(filter);
        }
        public VW_ProvedoresUsados GetById(System.Linq.Expressions.Expression<Func<VW_ProvedoresUsados, bool>> filter = null)
        {
            return this.Db.VW_ProvedoresUsados.SingleOrDefault(filter);
        }
    }
    public class VW_ProvedoresUsadosAgrupadosDAL : DALGeneric
    {
        public IEnumerable<VW_ProvedoresUsadosAgrupado> Get(System.Linq.Expressions.Expression<Func<VW_ProvedoresUsadosAgrupado, bool>> filter = null)
        {
            if (filter == null)
                return this.Db.VW_ProvedoresUsadosAgrupado;
            return this.Db.VW_ProvedoresUsadosAgrupado.Where(filter);
        }
        public VW_ProvedoresUsadosAgrupado GetById(System.Linq.Expressions.Expression<Func<VW_ProvedoresUsadosAgrupado, bool>> filter = null)
        {
            return this.Db.VW_ProvedoresUsadosAgrupado.SingleOrDefault(filter);
        }
    }
}
