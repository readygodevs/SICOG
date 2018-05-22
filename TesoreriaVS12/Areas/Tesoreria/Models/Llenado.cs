using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesoreriaVS12.Areas.Tesoreria.DAL.Catalogos;
using TesoreriaVS12.Areas.Tesoreria.DAL.Maestros;
using TesoreriaVS12.Utils;
using TesoreriaVS12.Models;
using TesoreriaVS12.DAL;
using System.Web.Mvc;
using TesoreriaVS12.Areas.Tesoreria.DAL.Detalles;
using TesoreriaVS12.Areas.Tesoreria.DAL.Vistas;

namespace TesoreriaVS12.Areas.Tesoreria.Models
{
    public class Llenado
    {
        protected AccionDAL accion { get; set; }
        protected ActividadMIRDAL actividad { get; set; }
        protected ActividadDAL actividadins { get; set; }
        protected AlcanceDAL alcancegeo { get; set; }
        protected DeFacturasDAL dalFacturas { get; set; }
        protected AreasDAL areas { get; set; }
        protected BancosDAL bancos { get; set; }
        protected BancosRHDAL bancosrh { get; set; }
        protected BeneficiariosDAL beneficiarios { get; set; }
        protected BeneficiariosCuentasDAL beneficiarioscuentas { get; set; }
        protected CallesDAL calles { get; set; }
        protected CausasCancelacionDAL causascancelacion { get; set; }
        protected CentroRecaudadorDAL CentroRecaudadorDAL { get; set; }
        protected CierreBancoDAL cierrebanco { get; set; }
        protected CierreMensualDAL cierremensual { get; set; }
        protected ClasificaBeneficiariosDAL clasificacionbeneficiario { get; set; }
        protected ClasificaPolizaDAL clasificacionpolizas { get; set; }
        protected ClasProgramaticaDAL clasprogramatica { get; set; }
        protected ColoniasDAL colonias { get; set; }
        protected ConceptosIngresosDAL conceptoingresos { get; set; }
        protected CuentasDAL cuentas { get; set; }
        protected CuentasBancariasDAL cuentasbancarias { get; set; }
        protected Ca_CURDAL curDal { get; set; }
        protected EstadosDAL estados { get; set; }
        protected FuenteDAL fuentesfin { get; set; }
        protected FuenteIngDAL FuenteIngDAL { get; set; }
        protected FuncionDAL funciones { get; set; }
        protected ImpuestosDeduccionDAL impuestodeduccion { get; set; }
        protected LocalidadesDAL localidades { get; set; }
        protected MunicipiosDAL municipios { get; set; }
        protected ObjetoGDAL objetogasto { get; set; }
        protected PaisesDAL paises { get; set; }
        protected ParametrosDAL parametros { get; set; }
        protected PercepDeducDAL percepdeduc { get; set; }
        protected PersonasDAL personas { get; set; }
        protected ProgramaDAL programas { get; set; }
        protected ProcesoDAL proyecto { get; set; }
        protected TipoBeneficiariosDAL tipobeneficiario { get; set; }
        protected TipoCompromisosDAL tipocompromiso { get; set; }
        protected TipoContrarecibosDAL tipocontrarecibo { get; set; }
        protected TipoDoctosDAL tipodoctos { get; set; }
        protected TipoFormatoChequesDAL tipoformatocheques { get; set; }
        protected TipoGastosDAL tipogasto { get; set; }
        protected TipoImpuestosDAL tipoimpuesto { get; set; }
        //protected TipoIngresosDAL tipoingresos { get; set; }
        protected TipoMetaDAL tipometa { get; set; }
        protected TipoMovBancariosDAL tipomovbancarios { get; set; }
        protected TipoPagosDAL tipopagos { get; set; }
        protected TipoPolizasDAL tipopolizas { get; set; }
        protected TipoTrasferenciasEgDAL tipotransferenciaseg { get; set; }
        protected TipoTransferenciasIngDAL tipotrasnferenciasing { get; set; }
        protected GirosDAL giros { get; set; }

        private CuentasDAL DALCuentas { get; set; }
        protected MaPolizasDAL mapolizas { get; set; }
        protected MaCompromisosDAL macompromisos { get; set; }
        protected MaContrarecibosDAL macontrarecibos { get; set; }
        protected ContraRecibosDAL Vmacontrarecibos { get; set; }
        protected MaPresupuestoEgDAL mapresupuestoeg { get; set; }
        protected MaTrasnferenciasDAL MaTranferenciasDAL { get; set; }
        protected MaTransferenciasIngDAL MaTransferenciasIngDAL { get; set; }
        protected MaPresupuestoIngDAL MaPresupuestoIngDAL { get; set; }
        protected MaRecibosDAL recibosDAL { get; set; }
        protected MaComprobacionesDAL macomprobacionesDAL { get; set; }

        protected DePolizasDAL depolizas { get; set; }
        protected DeContrarecibosDAL decontrarecibos { get; set; }
        protected DeCompromisosDAL decompromisos { get; set; }
        protected DeBeneficiariosContactosDAL debeneficiariocontactos { get; set; }
        protected DeBeneficiariosGirosDAL debeneficiariosgiros { get; set; }
        protected DeDisponibilidadDAL DALDeDisponiblidad { get; set; }
        protected DeTransferenciaDAL DeTransferenciasDAL { get; set; }
        protected DeReferenciasPolizasDAL referenciasDAL { get; private set; }
        protected DeTransferenciaIngDAL DeTransferenciaIngDAL { get; set; }
        protected DeRecibosDAL deRecibos { get; set; }
        protected DeComprobacionesDAL decomprobaciones { get; set; }

        protected UsuariosDAL usuarios { get; set; }
        protected Listas _listas { get; set; }

        protected VW_ProvedoresUsadosDAL vProveedoresUsadosDAL { get; set; }

        public Llenado()
        {
            if (accion == null) accion = new AccionDAL();
            if (referenciasDAL == null) referenciasDAL = new DeReferenciasPolizasDAL();
            if (actividad == null) actividad = new ActividadMIRDAL();
            if (actividadins == null) actividadins = new ActividadDAL();
            if (alcancegeo == null) alcancegeo = new AlcanceDAL();
            if (areas == null) areas = new AreasDAL();
            if (bancos == null) bancos = new BancosDAL();
            if (bancosrh == null) bancosrh = new BancosRHDAL();
            if (beneficiarios == null) beneficiarios = new BeneficiariosDAL();
            if (beneficiarioscuentas == null) beneficiarioscuentas = new BeneficiariosCuentasDAL();
            if (calles == null) calles = new CallesDAL();
            if (causascancelacion == null) causascancelacion = new CausasCancelacionDAL();
            if (CentroRecaudadorDAL == null) CentroRecaudadorDAL = new CentroRecaudadorDAL();
            if (cierrebanco == null) cierrebanco = new CierreBancoDAL();
            if (cierremensual == null) cierremensual = new CierreMensualDAL();
            if (clasificacionbeneficiario == null) clasificacionbeneficiario = new ClasificaBeneficiariosDAL();
            if (clasificacionpolizas == null) clasificacionpolizas = new ClasificaPolizaDAL();
            if (clasprogramatica == null) clasprogramatica = new ClasProgramaticaDAL();
            if (colonias == null) colonias = new ColoniasDAL();
            if (conceptoingresos == null) conceptoingresos = new ConceptosIngresosDAL();
            if (cuentas == null) cuentas = new CuentasDAL();
            if (cuentasbancarias == null) cuentasbancarias = new CuentasBancariasDAL();
            if (curDal == null) curDal = new Ca_CURDAL();
            if (estados == null) estados = new EstadosDAL();
            if (fuentesfin == null) fuentesfin = new FuenteDAL();
            if (FuenteIngDAL == null) FuenteIngDAL = new FuenteIngDAL();
            if (funciones == null) funciones = new FuncionDAL();
            if (impuestodeduccion == null) impuestodeduccion = new ImpuestosDeduccionDAL();
            if (localidades == null) localidades = new LocalidadesDAL();
            if (municipios == null) municipios = new MunicipiosDAL();
            if (objetogasto == null) objetogasto = new ObjetoGDAL();
            if (paises == null) paises = new PaisesDAL();
            if (parametros == null) parametros = new ParametrosDAL();
            if (percepdeduc == null) percepdeduc = new PercepDeducDAL();
            if (personas == null) personas = new PersonasDAL();
            if (programas == null) programas = new ProgramaDAL();
            if (proyecto == null) proyecto = new ProcesoDAL();
            if (tipobeneficiario == null) tipobeneficiario = new TipoBeneficiariosDAL();
            if (tipocompromiso == null) tipocompromiso = new TipoCompromisosDAL();
            if (tipocontrarecibo == null) tipocontrarecibo = new TipoContrarecibosDAL();
            if (tipodoctos == null) tipodoctos = new TipoDoctosDAL();
            if (tipoformatocheques == null) tipoformatocheques = new TipoFormatoChequesDAL();
            if (tipogasto == null) tipogasto = new TipoGastosDAL();
            if (tipoimpuesto == null) tipoimpuesto = new TipoImpuestosDAL();
            //if (tipoingresos == null) tipoingresos = new TipoIngresosDAL();
            if (tipometa == null) tipometa = new TipoMetaDAL();
            if (tipomovbancarios == null) tipomovbancarios = new TipoMovBancariosDAL();
            if (tipopagos == null) tipopagos = new TipoPagosDAL();
            if (tipopolizas == null) tipopolizas = new TipoPolizasDAL();
            if (tipotransferenciaseg == null) tipotransferenciaseg = new TipoTrasferenciasEgDAL();
            if (tipotrasnferenciasing == null) tipotrasnferenciasing = new TipoTransferenciasIngDAL();
            if (giros == null) giros = new GirosDAL();

            if (mapolizas == null) mapolizas = new MaPolizasDAL();
            if (macompromisos == null) macompromisos = new MaCompromisosDAL();
            if (macontrarecibos == null) macontrarecibos = new MaContrarecibosDAL();
            if (Vmacontrarecibos == null) Vmacontrarecibos = new ContraRecibosDAL();
            if (mapresupuestoeg == null) mapresupuestoeg = new MaPresupuestoEgDAL();
            if (MaTranferenciasDAL == null) MaTranferenciasDAL = new MaTrasnferenciasDAL();
            if (MaTransferenciasIngDAL == null) MaTransferenciasIngDAL = new MaTransferenciasIngDAL();
            if (MaPresupuestoIngDAL == null) MaPresupuestoIngDAL = new MaPresupuestoIngDAL();
            if (recibosDAL == null) recibosDAL = new MaRecibosDAL();
            if (macomprobacionesDAL == null) macomprobacionesDAL = new MaComprobacionesDAL();

            if (depolizas == null) depolizas = new DePolizasDAL();
            if (decompromisos == null) decompromisos = new DeCompromisosDAL();
            if (decontrarecibos == null) decontrarecibos = new DeContrarecibosDAL();
            if (debeneficiariocontactos == null) debeneficiariocontactos = new DeBeneficiariosContactosDAL();
            if (debeneficiariosgiros == null) debeneficiariosgiros = new DeBeneficiariosGirosDAL();
            if (DALDeDisponiblidad == null) DALDeDisponiblidad = new DeDisponibilidadDAL();
            if (DeTransferenciasDAL == null) DeTransferenciasDAL = new DeTransferenciaDAL();
            if (DeTransferenciaIngDAL == null) DeTransferenciaIngDAL = new DeTransferenciaIngDAL();
            if (deRecibos == null) deRecibos = new DeRecibosDAL();
            if (decomprobaciones == null) decomprobaciones = new DeComprobacionesDAL();

            if (usuarios == null) usuarios = new UsuariosDAL();
            if (dalFacturas == null) dalFacturas = new DeFacturasDAL();
            if (_listas == null) _listas = new Listas();
            if (vProveedoresUsadosDAL == null) vProveedoresUsadosDAL = new VW_ProvedoresUsadosDAL();
            if (DALCuentas == null) DALCuentas = new CuentasDAL();

        }

        public CA_UsuariosModel LLenado_Usuarios(int IdUsuario)
        {
            CA_UsuariosModel model = ModelFactory.getModel<CA_UsuariosModel>(usuarios.GetByID(x => x.IdUsuario == IdUsuario), new CA_UsuariosModel());
            return model;
        }

        public Ca_FuentesFinModel LLenado_CaFuentesFin(string IdFuente)
        {
            Ca_FuentesFinModel model = ModelFactory.getModel<Ca_FuentesFinModel>(fuentesfin.GetByID(x => x.Id_Fuente == IdFuente), new Ca_FuentesFinModel());
            return model;
        }

        public Ca_FuentesFin_IngModel LLenado_CaFuentesFin_Ing(string IdFuente)
        {
            Ca_FuentesFin_IngModel model = ModelFactory.getModel<Ca_FuentesFin_IngModel>(FuenteIngDAL.GetByID(x => x.Id_FuenteFinancia == IdFuente), new Ca_FuentesFin_IngModel());
            return model;
        }

        public Ca_TipoGastosModel LLenado_CaTipoGastos(string IdTipoGasto)
        {
            Ca_TipoGastosModel model = ModelFactory.getModel<Ca_TipoGastosModel>(tipogasto.GetByID(x => x.Id_TipoGasto == IdTipoGasto), new Ca_TipoGastosModel());
            return model;
        }

        public Ca_AlcanceGeoModel LLenado_CaAlcanceGeo(string IdAlcanceGeo)
        {
            Ca_AlcanceGeoModel model = ModelFactory.getModel<Ca_AlcanceGeoModel>(alcancegeo.GetByID(x => x.Id_AlcanceGeo == IdAlcanceGeo), new Ca_AlcanceGeoModel());
            return model;
        }

        public Ca_AccionesModel LLenado_CaAcciones(string IdProceso, string IdActividadMIR, string IdAccion)
        {
            Ca_AccionesModel model = ModelFactory.getModel<Ca_AccionesModel>(accion.GetByID(x => x.Id_Proceso == IdProceso && x.Id_ActividadMIR2 == IdActividadMIR && x.Id_Accion2 == IdAccion), new Ca_AccionesModel());
            model.CA_Proyecto = LLenado_CaProyecto(model.Id_Proceso);
            model.CA_Actividad = LLenado_CaActividad(model.Id_Proceso, model.Id_ActividadMIR2);
            return model;
        }

        public Ca_ActividadModel LLenado_CaActividad(string IdProceso, string IdActividadMIR)
        {
            Ca_ActividadModel model = ModelFactory.getModel<Ca_ActividadModel>(actividad.GetByID(x => x.Id_Proceso == IdProceso && x.Id_ActividadMIR2 == IdActividadMIR), new Ca_ActividadModel());
            model.CA_Proyecto = LLenado_CaProyecto(model.Id_Proceso);
            return model;
        }

        public Ca_TipoMetaModel LLenado_CaTipoMeta(string IdTipoMeta)
        {
            Ca_TipoMetaModel model = ModelFactory.getModel<Ca_TipoMetaModel>(tipometa.GetByID(x => x.Id_TipoMeta == IdTipoMeta), new Ca_TipoMetaModel());
            return model;
        }

        public Ca_ProyectoModel LLenado_CaProyecto(string IdProceso)
        {
            Ca_ProyectoModel model = ModelFactory.getModel<Ca_ProyectoModel>(proyecto.GetByID(x => x.Id_Proceso == IdProceso), new Ca_ProyectoModel());
            return model;
        }

        public Ca_ProgramasModel LLenado_CaProgramas(string IdPrograma)
        {
            Ca_ProgramasModel model = ModelFactory.getModel<Ca_ProgramasModel>(programas.GetByID(x => x.Id_Programa == IdPrograma), new Ca_ProgramasModel());
            return model;
        }


        public Ca_ClasProgramaticaModel LLenado_CaClasProgramatica(string IdClasificacionP)
        {
            try
            {
                Ca_ClasProgramaticaModel model = ModelFactory.getModel<Ca_ClasProgramaticaModel>(clasprogramatica.GetByID(x => x.Id_ClasificacionP == IdClasificacionP), new Ca_ClasProgramaticaModel());
                return model;
            }
            catch (Exception ex)
            {
                new Errores(ex.HResult, ex.Message);
                return new Ca_ClasProgramaticaModel();
            }

        }

        public Ca_FuncionesModel LLenado_CaFunciones(string IdFuncion)
        {
            Ca_FuncionesModel model = ModelFactory.getModel<Ca_FuncionesModel>(funciones.GetByID(x => x.Id_Funcion == IdFuncion), new Ca_FuncionesModel());
            return model;
        }

        public Ca_BeneficiariosCuentasModel LLenado_CaBeneficiariosCuentas(int IdBeneficiario, byte IdClasBeneficiario)
        {
            Ca_BeneficiariosCuentasModel model = ModelFactory.getModel<Ca_BeneficiariosCuentasModel>(beneficiarioscuentas.GetByID(x => x.Id_Beneficiario == IdBeneficiario && x.Id_ClasBeneficiario == IdClasBeneficiario), new Ca_BeneficiariosCuentasModel());
            model.Ca_Cuentas = LLenado_CaCuentas(model.Id_Cuenta);
            model.Ca_ClasificaBeneficiarios = LLenado_CaClasificaBeneficiarios(model.Id_ClasBeneficiario);
            return model;
        }

        public Ca_ClasificaBeneficiariosModel LLenado_CaClasificaBeneficiarios(byte? IdClasificaBene)
        {
            Ca_ClasificaBeneficiariosModel model = ModelFactory.getModel<Ca_ClasificaBeneficiariosModel>(clasificacionbeneficiario.GetByID(x => x.Id_ClasificaBene == IdClasificaBene), new Ca_ClasificaBeneficiariosModel());
            model.Ca_Cuentas = LLenado_CaCuentas(model.Id_Cuenta);
            return model;
        }

        public Ca_AreasModel LLenado_CaAreas(string IdArea)
        {
            Ca_AreasModel model = ModelFactory.getModel<Ca_AreasModel>(areas.GetByID(x => x.Id_Area == IdArea), new Ca_AreasModel());
            return model;
        }
        public Ca_CentroRecaudadorModel LLenado_CaCentroRecaudador(string IdArea)
        {
            Ca_CentroRecaudadorModel model = ModelFactory.getModel<Ca_CentroRecaudadorModel>(CentroRecaudadorDAL.GetByID(x => x.Id_CRecaudador == IdArea), new Ca_CentroRecaudadorModel());
            return model;
        }

        public Ca_BancosModel LLenado_CaBancos(short? IdBanco)
        {
            Ca_BancosModel model = ModelFactory.getModel<Ca_BancosModel>(bancos.GetByID(x => x.Id_Banco == IdBanco), new Ca_BancosModel());
            return model;
        }

        public Ca_TipoPagosModel LLenado_CaTipoPagos(byte? IdTipoPago)
        {
            Ca_TipoPagosModel model = ModelFactory.getModel<Ca_TipoPagosModel>(tipopagos.GetByID(x => x.Id_TipoPago == IdTipoPago), new Ca_TipoPagosModel());
            return model;
        }

        public Ca_ActividadesInstModel LLenado_CaActividades(string IdActividad)
        {
            Ca_ActividadesInstModel model = ModelFactory.getModel<Ca_ActividadesInstModel>(actividadins.GetByID(x => x.Id_Actividad == IdActividad), new Ca_ActividadesInstModel());
            return model;
        }

        public Ca_CallesModel LLenado_CaCalles(byte? IdEstados, short? IdMunicipios, short? IdLocalidad, short? IdCalle)
        {
            Ca_CallesModel model = ModelFactory.getModel<Ca_CallesModel>(calles.GetByID(x => x.Id_Estado == IdEstados && x.Id_Municipio == IdMunicipios && x.Id_Localidad == IdLocalidad && x.id_calle == IdCalle), new Ca_CallesModel());
            return model;
        }

        public Ca_ColoniasModel LLenado_CaColonias(byte? IdEstados, short? IdMunicipios, short? IdLocalidad, short? IdColonia)
        {
            Ca_ColoniasModel model = ModelFactory.getModel<Ca_ColoniasModel>(colonias.GetByID(x => x.Id_Estado == IdEstados && x.Id_Municipio == IdMunicipios && x.Id_Localidad == IdLocalidad && x.id_colonia == IdColonia), new Ca_ColoniasModel());
            return model;
        }

        public Ca_LocalidadesModel LLenado_CaLocalidades(byte? IdEstados, short? IdMunicipios, short? IdLocalidad)
        {
            Ca_LocalidadesModel model = ModelFactory.getModel<Ca_LocalidadesModel>(localidades.GetByID(x => x.Id_Estado == IdEstados && x.Id_Municipio == IdMunicipios && x.Id_Localidad == IdLocalidad), new Ca_LocalidadesModel());
            return model;
        }

        public Ca_MunicipiosModel LLenado_CaMunicipios(byte? IdEstados, short? IdMunicipios)
        {
            Ca_MunicipiosModel model = ModelFactory.getModel<Ca_MunicipiosModel>(municipios.GetByID(x => x.Id_Estado == IdEstados && x.Id_Municipio == IdMunicipios), new Ca_MunicipiosModel());
            return model;
        }

        public Ca_EstadosModel LLenado_CaEstados(byte? IdEstados)
        {
            Ca_EstadosModel model = ModelFactory.getModel<Ca_EstadosModel>(estados.GetByID(x => x.Id_Estado == IdEstados), new Ca_EstadosModel());
            return model;
        }

        public Ca_TipoBeneficiariosModel LLenado_CaTipoBeneficiarios(byte? IdTipoBeneficiario)
        {
            Ca_TipoBeneficiariosModel model = ModelFactory.getModel<Ca_TipoBeneficiariosModel>(tipobeneficiario.GetByID(x => x.Id_TipoBene == IdTipoBeneficiario), new Ca_TipoBeneficiariosModel());
            return model;
        }

        public Ca_ConceptosIngresosModel LLenado_CaConceptosIngresos(string Idconcepto)
        {
            Ca_ConceptosIngresosModel model = ModelFactory.getModel<Ca_ConceptosIngresosModel>(conceptoingresos.GetByID(x => x.Id_Concepto == Idconcepto), new Ca_ConceptosIngresosModel());
            return model;
        }

        public Ca_GirosModel LLenado_CaGiros(short IdGiroComercial)
        {
            Ca_GirosModel model = ModelFactory.getModel<Ca_GirosModel>(giros.GetByID(x => x.Id_GiroComercial == IdGiroComercial), new Ca_GirosModel());
            return model;
        }

        public Ca_ObjetoGastoModel LLenado_CaObjetoGasto(string IdObjetoG)
        {
            Ca_ObjetoGastoModel model = ModelFactory.getModel<Ca_ObjetoGastoModel>(objetogasto.GetByID(x => x.Id_ObjetoG == IdObjetoG), new Ca_ObjetoGastoModel());
            return model;
        }

        public Ca_CuentasBancariasModel LLenado_CaCuentasBancarias(short? IdCtaBancaria)
        {
            Ca_CuentasBancariasModel model = ModelFactory.getModel<Ca_CuentasBancariasModel>(cuentasbancarias.GetByID(x => x.Id_CtaBancaria == IdCtaBancaria), new Ca_CuentasBancariasModel());
            model.Ca_Bancos = new Ca_BancosModel();
            if (model.Id_Banco != null)
                model.Ca_Bancos = LLenado_CaBancos(model.Id_Banco);
            if (model.Id_Cuenta != null)
                model.Ca_Cuentas = LLenado_CaCuentas(model.Id_Cuenta);
            return model;
        }

        public Ca_PersonasModel Llenado_CaPersonas(int? IdPersona)
        {
            Ca_PersonasModel model = ModelFactory.getModel<Ca_PersonasModel>(personas.GetByID(x => x.IdPersona == IdPersona), new Ca_PersonasModel());
            model.IdPais = 1;
            model.RFC = model.RFC.Trim();
            if (model.PersonaFisica == true)
                model.NombreCompleto = String.Format("{0} {1} {2}", model.Nombre, model.ApellidoPaterno, model.ApellidoMaterno);
            else
                model.NombreCompleto = model.RazonSocial;

            if (model.IdEstado != null)
                model.Ca_EstadosModel = LLenado_CaEstados(model.IdEstado);
            if (model.IdMunicipio != null)
                model.Ca_MunicipiosModel = LLenado_CaMunicipios(model.IdEstado, model.IdMunicipio);
            if (model.IdLocalidad != null)
                model.Ca_LocalidadesModel = LLenado_CaLocalidades(model.IdEstado, model.IdMunicipio, model.IdLocalidad);
            if (model.IdColonia != null)
                model.Ca_ColoniasModel = LLenado_CaColonias(model.IdEstado, model.IdMunicipio, model.IdLocalidad, model.IdColonia);
            if (model.IdCalle != null)
                model.Ca_CallesModel = LLenado_CaCalles(model.IdEstado, model.IdMunicipio, model.IdLocalidad, model.IdCalle);
                model.Resultado = model.Ca_CallesModel != null ? true : false;
            if (model.IdEstado != null)
                model.ListaIdEstado = new SelectList(estados.Get(), "Id_Estado", "Descripcion", model.IdEstado);
            if (model.IdEstado != null && model.IdMunicipio != null)
                model.ListaIdMunicipio = new SelectList(municipios.Get(x => x.Id_Estado == model.IdEstado), "Id_Municipio", "Descripcion", model.IdMunicipio);
            if (model.IdEstado != null && model.IdMunicipio != null && model.IdLocalidad != null)
                model.ListaIdLocalidad = new SelectList(localidades.Get(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio), "Id_Localidad", "Descripcion", model.IdLocalidad);
            if (model.IdEstado != null && model.IdMunicipio != null && model.IdLocalidad != null && model.IdColonia != null)
                model.ListaIdColonia = new SelectList(colonias.Get(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad), "id_colonia", "Descripcion", model.IdColonia);
            if (model.IdEstado != null && model.IdMunicipio != null && model.IdLocalidad != null && model.IdCalle != null)
                model.ListaIdCalle = new SelectList(calles.Get(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad), "id_calle", "Descripcion", model.IdCalle);
            return model;
        }

        public Ca_PersonasModel Llenado_CaPersonasBusqueda(int? IdPersona)
        {
            Ca_PersonasModel model = ModelFactory.getModel<Ca_PersonasModel>(personas.GetByID(x => x.IdPersona == IdPersona), new Ca_PersonasModel());
            model.IdPais = 1;
            if (model.PersonaFisica == true)
                model.NombreCompleto = String.Format("{0} {1} {2}", model.Nombre, model.ApellidoPaterno, model.ApellidoMaterno);
            else
                model.NombreCompleto = model.RazonSocial;
            return model;
        }

        public Ca_BeneficiariosModel BusquedaBeneficiario(int? IdBeneficiario, byte? Id_TipoBeneficiario)
        {
            Ca_BeneficiariosModel model = new Ca_BeneficiariosModel();
            if (IdBeneficiario != null)
            {
                model = ModelFactory.getModel<Ca_BeneficiariosModel>(Llenado_CaPersonasBusqueda(beneficiarios.GetByID(x => x.Id_Beneficiario == IdBeneficiario).IdPersona), model);
                if (Id_TipoBeneficiario.HasValue)
                    model.Ca_TipoBeneficiariosModel = LLenado_CaTipoBeneficiarios(Id_TipoBeneficiario.Value);
                else
                    model.Ca_TipoBeneficiariosModel = new Ca_TipoBeneficiariosModel();
                model.Id_Beneficiario = IdBeneficiario.Value;
            }

            return model;
        }

        public Ca_BeneficiariosModel Llenado_CaBeneficiarios(int? IdBeneficiario)
        {
            Ca_BeneficiariosModel model = new Ca_BeneficiariosModel();
            if (IdBeneficiario != null)
            {

                model = ModelFactory.getModel<Ca_BeneficiariosModel>(beneficiarios.GetByID(x => x.Id_Beneficiario == IdBeneficiario), new Ca_BeneficiariosModel());
                model = ModelFactory.getModel<Ca_BeneficiariosModel>(Llenado_CaPersonas(model.IdPersona), model);


                if (model.Id_TipoBeneficiario != null)
                    model.Ca_TipoBeneficiariosModel = LLenado_CaTipoBeneficiarios(model.Id_TipoBeneficiario);
                else
                    model.Ca_TipoBeneficiariosModel = new Ca_TipoBeneficiariosModel();

                model.ListaCa_BeneficiariosCuentasModel = new List<Ca_BeneficiariosCuentasModel>();
                foreach (CA_BeneficiariosCuentas item in beneficiarioscuentas.Get(x => x.Id_Beneficiario == model.Id_Beneficiario))
                {
                    model.ListaCa_BeneficiariosCuentasModel.Add(LLenado_CaBeneficiariosCuentas(item.Id_Beneficiario, item.Id_ClasBeneficiario));
                }

                model.ListaDE_BeneficiarioContactosModel = new List<De_BeneficiarioContactosModel>();
                foreach (DE_BeneficiarioContactos item in debeneficiariocontactos.Get(x => x.Id_Beneficiario == model.Id_Beneficiario))
                {
                    model.ListaDE_BeneficiarioContactosModel.Add(LLenado_DeBeneficiarioContactos(item.Id_Beneficiario, item.IdContacto));
                }

                model.ListaDe_Beneficiarios_GirosModel = new List<De_Beneficiarios_GirosModel>();
                foreach (De_Beneficiarios_Giros item in debeneficiariosgiros.Get(x => x.Id_Beneficiario == model.Id_Beneficiario))
                {
                    model.ListaDe_Beneficiarios_GirosModel.Add(LLenado_DeBeneficiariosGiros(item.Id_Beneficiario, item.Id_GiroComercial));
                }

                if (model.IdEstado != null)
                    model.ListaIdEstado = new SelectList(estados.Get(), "Id_Estado", "Descripcion");
                if (model.IdEstado != null && model.IdMunicipio != null)
                    model.ListaIdMunicipio = new SelectList(municipios.Get(x => x.Id_Estado == model.IdEstado), "Id_Municipio", "Descripcion");
                if (model.IdEstado != null && model.IdMunicipio != null && model.IdLocalidad != null)
                    model.ListaIdLocalidad = new SelectList(localidades.Get(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio), "Id_Localidad", "Descripcion");
                if (model.IdEstado != null && model.IdMunicipio != null && model.IdLocalidad != null && model.IdColonia != null)
                    model.ListaIdColonia = new SelectList(colonias.Get(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad), "id_colonia", "Descripcion");
                if (model.IdEstado != null && model.IdMunicipio != null && model.IdLocalidad != null && model.IdCalle != null)
                    model.ListaIdCalle = new SelectList(calles.Get(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad), "id_calle", "Descripcion");

            }
            return model;
        }

        public Ca_CuentasModel LLenado_CaCuentas(string IdCuenta)
        {
            Ca_CuentasModel model = ModelFactory.getModel<Ca_CuentasModel>(cuentas.GetByID(x => x.Id_Cuenta == IdCuenta), new Ca_CuentasModel());
            if (model.Id_ObjetoG != null)
                model.Ca_ObjetoGastos = LLenado_CaObjetoGasto(model.Id_ObjetoG);
            if (model.Id_Concepto != null)
                model.Ca_ConceptosIngresos = LLenado_CaConceptosIngresos(model.Id_Concepto);
            if (model.Naturaleza != null)
                model.Di_Naturaleza = new SelectList(Diccionarios.Naturaleza, "Key", "Value");
            //if (model.Tipo_Tiempo_Depreciacion != null)
            //    model.Di_TipoTiempoDeapreciacion = Diccionarios.Tipo_Tiempo_Depreciacion;
            return model;
        }

        public De_BeneficiarioContactosModel LLenado_DeBeneficiarioContactos(int IdBeneficiario, short IdContacto)
        {
            De_BeneficiarioContactosModel model = ModelFactory.getModel<De_BeneficiarioContactosModel>(debeneficiariocontactos.GetByID(x => x.Id_Beneficiario == IdBeneficiario && x.IdContacto == IdContacto), new De_BeneficiarioContactosModel());
            //model.NombreCompleto = String.Format("{0} {1} {2}", model.Nombre, model.ApellidoPaterno, model.ApellidoMaterno);
            return model;
        }

        public De_Beneficiarios_GirosModel LLenado_DeBeneficiariosGiros(int IdBeneficiario, short IdGiroComercial)
        {
            De_Beneficiarios_GirosModel model = ModelFactory.getModel<De_Beneficiarios_GirosModel>(debeneficiariosgiros.GetByID(x => x.Id_Beneficiario == IdBeneficiario && x.Id_GiroComercial == IdGiroComercial), new De_Beneficiarios_GirosModel());
            if (model.Id_GiroComercial != null)
                model.Ca_GirosModel = LLenado_CaGiros(model.Id_GiroComercial);
            return model;
        }


        public Ca_TipoCompromisosModel LLenado_CaTipoCompromisos(short IdTipoCompromiso)
        {
            Ca_TipoCompromisosModel model = ModelFactory.getModel<Ca_TipoCompromisosModel>(tipocompromiso.GetByID(x => x.Id_TipoCompromiso == IdTipoCompromiso), new Ca_TipoCompromisosModel());
            return model;
        }

        public Ca_TipoContrarecibosModel LLenado_CaTipoContrarecibos(short IdTipoCR)
        {
            return ModelFactory.getModel<Ca_TipoContrarecibosModel>(tipocontrarecibo.GetByID(x => x.Id_TipoCR == IdTipoCR), new Ca_TipoContrarecibosModel());
        }

        public Ca_TipoMovBancariosModel LLenado_CaTipoMovBancarios(byte? IdTipoMovB, byte? IdFolioMovB)
        {
            Ca_TipoMovBancariosModel model = ModelFactory.getModel<Ca_TipoMovBancariosModel>(tipomovbancarios.GetByID(x => x.Id_TipoMovB == IdTipoMovB && x.Id_FolioMovB == IdFolioMovB), new Ca_TipoMovBancariosModel());
            return model;
        }


        public De_PolizasModel LLenado_DePolizas(byte TipoPoliza, int FolioPoliza, byte MesPoliza, int Registro)
        {
            De_PolizasModel model = ModelFactory.getModel<De_PolizasModel>(depolizas.GetByID(x => x.Id_TipoPoliza == TipoPoliza && x.Id_FolioPoliza == FolioPoliza && x.Id_MesPoliza == MesPoliza && x.Id_Registro == Registro), new De_PolizasModel());
            if (model.Id_Cuenta != null)
                model.Ca_Cuentas = LLenado_CaCuentas(model.Id_Cuenta);
            if (model.Id_ClavePresupuesto != null)
            {
                model.Ma_PresupuestoEg = LLenado_MaPresupuestoEG(model.Id_ClavePresupuesto);
                model.Ca_Areas = LLenado_CaAreas(model.Ma_PresupuestoEg.Id_Area);
                model.Id_Area = model.Ca_Areas.Id_Area;
                model.Ca_Funciones = LLenado_CaFunciones(model.Ma_PresupuestoEg.Id_Funcion);
                model.Id_Funcion = model.Ca_Funciones.Id_Funcion;
                model.Ca_ActividadInst = LLenado_CaActividades(model.Ma_PresupuestoEg.Id_Actividad);
                model.Id_Actividad = model.Ca_ActividadInst.Id_Actividad;
                model.Ca_ClasProgramatica = LLenado_CaClasProgramatica(model.Ma_PresupuestoEg.Id_ClasificacionP);
                model.Id_ClasificacionP = model.Ca_ClasProgramatica.Id_ClasificacionP;
                model.Ca_Programas = LLenado_CaProgramas(model.Ma_PresupuestoEg.Id_Programa);
                model.Id_Programa = model.Ca_Programas.Id_Programa;
                model.Ca_Proyecto = LLenado_CaProyecto(model.Ma_PresupuestoEg.Id_Proceso);
                model.Id_Proceso = model.Ca_Proyecto.Id_Proceso;
                model.Ca_TipoMeta = LLenado_CaTipoMeta(model.Ma_PresupuestoEg.Id_TipoMeta);
                model.Id_TipoMeta = model.Ca_TipoMeta.Id_TipoMeta;
                model.Ca_Actividad = LLenado_CaActividad(model.Ma_PresupuestoEg.Id_Proceso, model.Ma_PresupuestoEg.Id_ActividadMIR);
                model.Id_ActividadMIR = model.Ca_Actividad.Id_ActividadMIR2;
                model.Ca_Acciones = LLenado_CaAcciones(model.Ma_PresupuestoEg.Id_Proceso, model.Ma_PresupuestoEg.Id_ActividadMIR, model.Ma_PresupuestoEg.Id_Accion);
                model.Id_Accion = model.Ca_Acciones.Id_Accion2;
                model.Ca_AlcanceGeo = LLenado_CaAlcanceGeo(model.Ma_PresupuestoEg.Id_Alcance);
                model.Id_Alcance = model.Ca_AlcanceGeo.Id_AlcanceGeo;
                model.Ca_TipoGastos = LLenado_CaTipoGastos(model.Ma_PresupuestoEg.Id_TipoG);
                model.Id_TipoG = model.Ca_TipoGastos.Id_TipoGasto;
                model.Ca_FuentesFin = LLenado_CaFuentesFin(model.Ma_PresupuestoEg.Id_Fuente);
                model.Id_Fuente = model.Ca_FuentesFin.Id_Fuente;
                model.Id_Fuente_Filtro = model.Id_Fuente;
                model.Id_Fuente_Filtro = model.Id_Fuente;
                model.AnioFin = model.Ma_PresupuestoEg.AnioFin;
                model.Ca_ObjetoGasto = LLenado_CaObjetoGasto(model.Ma_PresupuestoEg.Id_ObjetoG);
                model.Id_ObjetoG = model.Ca_ObjetoGasto.Id_ObjetoG;
            }
            if (model.Id_ClavePresupuestoIng != null)
            {
                model.Ma_PresupuestoIng = LLenado_MaPresupuestoING(model.Id_ClavePresupuestoIng);
                model.Ca_CentroRecaudador = LLenado_CaCentroRecaudador(model.Id_ClavePresupuestoIng.Substring(0, 6));
                model.Id_CentroRecaudador = model.Ca_CentroRecaudador.Id_CRecaudador;
                model.Ca_FuentesFin_Ing = LLenado_CaFuentesFin_Ing(model.Id_ClavePresupuestoIng.Substring(6, 4));
                model.Id_Fuente = model.Ca_FuentesFin_Ing.Id_FuenteFinancia;
                model.AnioFin = model.Id_ClavePresupuestoIng.Substring(10, 2);
                model.Ca_AlcanceGeo = LLenado_CaAlcanceGeo(model.Id_ClavePresupuestoIng.Substring(12, 1));
                model.Id_Alcance = model.Ca_AlcanceGeo.Id_AlcanceGeo;
                model.Ca_ConceptoIngresos = LLenado_CaConceptosIngresos(model.Id_ClavePresupuestoIng.Substring(13, 3));
                model.Ca_Cur = ModelFactory.getModel<CA_CURModel>(curDal.GetByID(x => x.IdCUR == model.IdCur), new CA_CURModel());
                model.Id_Concepto = model.Ca_ConceptoIngresos.Id_Concepto;
                model.IdCur = model.Ca_Cur.IdCUR;
            }
            return model;
        }

        public Ma_PolizasModel LLenado_MaPolizas(byte TipoPoliza, int FolioPoliza, byte MesPoliza)
        {
            Ma_PolizasModel model = ModelFactory.getModel<Ma_PolizasModel>(mapolizas.GetByID(x => x.Id_TipoPoliza == TipoPoliza && x.Id_FolioPoliza == FolioPoliza && x.Id_MesPoliza == MesPoliza), new Ma_PolizasModel());
            model.De_Polizas = new List<De_PolizasModel>();
            IEnumerable<De_Polizas> lstdpol = depolizas.Get(x => x.Id_TipoPoliza == TipoPoliza && x.Id_FolioPoliza == FolioPoliza && x.Id_MesPoliza == MesPoliza).OrderBy(x => x.Id_Movimiento).ThenBy(x => x.Id_Cuenta);

            foreach (De_Polizas item in lstdpol)
            {
                De_PolizasModel depol = LLenado_DePolizas(item.Id_TipoPoliza, item.Id_FolioPoliza, item.Id_MesPoliza, item.Id_Registro);
                model.De_Polizas.Add(depol);
            }

            model.De_ReferenciasPolizas = ModelFactory.getModel<De_ReferenciasPolizasModel>(referenciasDAL.GetByID(x => x.IdFolioPoliza == FolioPoliza && x.IdMesPoliza == MesPoliza && x.IdTipoPoliza == TipoPoliza), new De_ReferenciasPolizasModel());

            if (model.De_ReferenciasPolizas.IdMesPCom != null && model.De_ReferenciasPolizas.IdFolioPCom != null)
                model.Poliza_Comprometido = StringID.Polizas(model.De_ReferenciasPolizas.IdFolioPCom, model.De_ReferenciasPolizas.IdMesPCom);
            if (model.De_ReferenciasPolizas.IdFolioPDev != null && model.De_ReferenciasPolizas.IdMesPDev != null)
                model.Poliza_Devengado = StringID.Polizas(model.De_ReferenciasPolizas.IdFolioPDev, model.De_ReferenciasPolizas.IdMesPDev);
            if (model.De_ReferenciasPolizas.IdFolioPEje != null && model.De_ReferenciasPolizas.IdMesPEje != null)
                model.Poliza_Ejercido = StringID.Polizas(model.De_ReferenciasPolizas.IdFolioPEje, model.De_ReferenciasPolizas.IdMesPEje);
            if (model.De_ReferenciasPolizas.IdFolioPPag != null && model.De_ReferenciasPolizas.IdMesPPag != null)
                model.Poliza_Pagado = StringID.Polizas(model.De_ReferenciasPolizas.IdFolioPPag, model.De_ReferenciasPolizas.IdMesPPag);
            if (model.De_ReferenciasPolizas.IdFolioPDevI != null && model.De_ReferenciasPolizas.IdMesPDevI != null)
                model.Poliza_DevengadoIng = StringID.Polizas(model.De_ReferenciasPolizas.IdFolioPDevI, model.De_ReferenciasPolizas.IdMesPDevI);
            if (model.De_ReferenciasPolizas.IdFolioPRec != null && model.De_ReferenciasPolizas.IdMesPDevI != null)
                model.Poliza_Recaudado = StringID.Polizas(model.De_ReferenciasPolizas.IdFolioPRec, model.De_ReferenciasPolizas.IdMesPDevI);
            /*Cancelados*/
            if (model.De_ReferenciasPolizas.IdMesPComC != null && model.De_ReferenciasPolizas.IdFolioPComC != null)
                model.Poliza_ComprometidoC = StringID.Polizas(model.De_ReferenciasPolizas.IdFolioPComC, model.De_ReferenciasPolizas.IdMesPComC);
            if (model.De_ReferenciasPolizas.IdFolioPDevC != null && model.De_ReferenciasPolizas.IdMesPDevC != null)
                model.Poliza_DevengadoC = StringID.Polizas(model.De_ReferenciasPolizas.IdFolioPDevC, model.De_ReferenciasPolizas.IdMesPDevC);
            if (model.De_ReferenciasPolizas.IdFolioPEjeC != null && model.De_ReferenciasPolizas.IdMesPEjeC != null)
                model.Poliza_EjercidoC = StringID.Polizas(model.De_ReferenciasPolizas.IdFolioPEjeC, model.De_ReferenciasPolizas.IdMesPEjeC);
            if (model.De_ReferenciasPolizas.IdFolioPPagC != null && model.De_ReferenciasPolizas.IdMesPPagC != null)
                model.Poliza_PagadoC = StringID.Polizas(model.De_ReferenciasPolizas.IdFolioPPagC, model.De_ReferenciasPolizas.IdMesPPagC);
            if (model.De_ReferenciasPolizas.IdFolioPDevIC != null && model.De_ReferenciasPolizas.IdMesPDevIC != null)
                model.Poliza_DevengadoIngC = StringID.Polizas(model.De_ReferenciasPolizas.IdFolioPDevIC, model.De_ReferenciasPolizas.IdMesPDevIC);
            if (model.De_ReferenciasPolizas.IdFolioPRecC != null && model.De_ReferenciasPolizas.IdMesPDevIC != null)
                model.Poliza_RecaudadoC = StringID.Polizas(model.De_ReferenciasPolizas.IdFolioPRecC, model.De_ReferenciasPolizas.IdMesPDevIC);
            if (model.De_ReferenciasPolizas.IdFolioPolizaDC != null && model.De_ReferenciasPolizas.IdMesPolizaDC != null)
                model.Poliza_DiarioC = StringID.Polizas(model.De_ReferenciasPolizas.IdFolioPolizaDC, model.De_ReferenciasPolizas.IdMesPolizaDC);
            if (model.Id_Beneficiario != null)
                model.Ca_Beneficiarios = Llenado_CaBeneficiarios(model.Id_Beneficiario);
            if (model.Id_ClasPoliza != null)
                model.Ca_ClasificaPolizas = ModelFactory.getModel<Ca_ClasificaPolizasModel>(clasificacionpolizas.GetByID(x => x.Id_TipoPoliza == model.Id_TipoPoliza && x.Id_ClasificaPoliza == model.Id_ClasPoliza && x.Id_SubClasificaPoliza == model.Id_SubClasificaPol), new Ca_ClasificaPolizasModel());
            model.MesCerrado = cierremensual.GetByID(x => x.Id_Mes == model.Fecha.Month).Contable;
            if (model.Id_TipoPoliza > 0)
                model.Ca_TipoPolizas = ModelFactory.getModel<Ca_TipoPolizasModel>(tipopolizas.GetByID(x => x.Id_TipoPoliza == model.Id_TipoPoliza), new Ca_TipoPolizasModel());
            if (model.Id_TipoCR > 0)
                model.Ca_TipoContrarecibo = ModelFactory.getModel<Ca_TipoContrarecibosModel>(tipocontrarecibo.GetByID(x => x.Id_TipoCR == model.Id_TipoCR), new Ca_TipoContrarecibosModel());

            model.ListaId_ClasPoliza = new SelectList(clasificacionpolizas.Get(x => x.Id_SubClasificaPoliza == 0 && x.Id_ClasificaPoliza != 0), "Id_ClasificaPoliza", "Descripcion", model.Id_ClasPoliza);
            model.ListaId_SubClasificaPol = new SelectList(clasificacionpolizas.Get(x => x.Id_SubClasificaPoliza != 0 && x.Id_ClasificaPoliza == model.Id_ClasPoliza), "Id_SubClasificaPoliza", "Descripcion", model.Id_SubClasificaPol);
            model.TipoPoliza = tipopolizas.GetByID(x => x.Id_TipoPoliza == model.Id_TipoPoliza).Descripcion;

            if (model.Id_TipoCR != null && model.Id_FolioCR != null)
                model.Ma_Contrarecibos = LLenado_MaContrarecibos(model.Id_TipoCR, model.Id_FolioCR);
            return model;
        }

        public SelectList LLenado_ClasificacionBeneficiario(string IdCuenta, int? IdBeneficiario)
        {
            if (!String.IsNullOrEmpty(IdCuenta))
            {
                CA_BeneficiariosCuentas benecta = beneficiarioscuentas.GetByID(x => x.Id_Cuenta == IdCuenta);
                return new SelectList(clasificacionbeneficiario.Get(x => x.Id_ClasificaBene == benecta.Id_ClasBeneficiario), "Id_ClasificaBene", "Descripcion", benecta.Id_ClasBeneficiario);
            }
            if (!IdBeneficiario.HasValue)
                return new SelectList(new List<CA_BeneficiariosCuentas>(), "Id_ClasificaBene", "Descripcion");

            List<Ca_ClasificaBeneficiarios> clasben = new List<Ca_ClasificaBeneficiarios>();
            IEnumerable<CA_BeneficiariosCuentas> bencta = beneficiarioscuentas.Get(x => x.Id_Beneficiario == IdBeneficiario);
            foreach (CA_BeneficiariosCuentas item in bencta)
            {
                clasben.Add(clasificacionbeneficiario.GetByID(x => x.Id_ClasificaBene == item.Id_ClasBeneficiario));
            }
            return new SelectList(clasben, "Id_ClasificaBene", "Descripcion");
        }

        public Ma_CompromisosModel LLenado_MaCompromisos(short IdTipoCompromiso, int IdFolioCompromisos)
        {

            Ma_CompromisosModel model = ModelFactory.getModel<Ma_CompromisosModel>(macompromisos.GetByID(x => x.Id_TipoCompromiso == IdTipoCompromiso && x.Id_FolioCompromiso == IdFolioCompromisos), new Ma_CompromisosModel());
            model.De_Compromisos = new List<De_CompromisosModel>();
            model.PagarseEn = LLenado_CaTipoCompromisos(model.Id_TipoCompromiso).Pagarse;
            IEnumerable<De_Compromisos> lstdcom = decompromisos.Get(x => x.Id_TipoCompromiso == IdTipoCompromiso && x.Id_FolioCompromiso == IdFolioCompromisos);
            foreach (De_Compromisos item in lstdcom)
            {
                De_CompromisosModel decont = LLenado_DeCompromisos(item.Id_TipoCompromiso, item.Id_FolioCompromiso, item.Id_Registro);
                model.De_Compromisos.Add(decont);
            }
            if (model.Id_Beneficiario != null)
                model.Ca_Beneficiarios = Llenado_CaBeneficiarios(model.Id_Beneficiario);
            if (model.Id_Cuenta_Beneficiario != null)
                model.Ca_Cuentas_Beneficiario = LLenado_CaCuentas(model.Id_Cuenta_Beneficiario);
            if (model.Id_Area_Solicitud != null)
                model.Ca_Areas_Solicitud = LLenado_CaAreas(model.Id_Area_Solicitud);
            if (model.Id_Area_Entrega != null)
                model.Ca_Areas_Entrega = LLenado_CaAreas(model.Id_Area_Entrega);
            if (model.Id_TipoCR != null && model.Id_FolioCR != null)
                model.ContraRecibo = StringID.Contrarecibo(model.Id_TipoCR, model.Id_FolioCR);
            if (model.Id_FolioPO_Comprometido != null && model.Id_MesPO_Comprometido != null)
                model.Poliza_Comprometido = StringID.Polizas(model.Id_FolioPO_Comprometido, model.Id_MesPO_Comprometido);
            if (model.Id_FolioPO_Comprometido_C != null && model.Id_MesPO_Comprometido_C != null)
                model.Poliza_Comprometido_C = StringID.Polizas(model.Id_FolioPO_Comprometido_C, model.Id_MesPO_Comprometido_C);
            if (model.Id_FolioPO_Devengado != null && model.Id_MesPO_Devengado != null)
                model.Poliza_Devengado = StringID.Polizas(model.Id_FolioPO_Devengado, model.Id_MesPO_Devengado);
            if (model.Id_FolioPO_Devengado_C != null && model.Id_MesPO_Devengado_C != null)
                model.Poliza_Devengado_C = StringID.Polizas(model.Id_FolioPO_Devengado_C, model.Id_MesPO_Devengado_C);
            if (model.Id_FolioPoliza != null && model.Id_MesPoliza != null)
                model.Poliza_Fincamiento = StringID.Polizas(model.Id_FolioPoliza, model.Id_MesPoliza);
            if (model.Id_FolioPoliza_C != null && model.Id_MesPoliza_C != null)
                model.Poliza_Fincamiento_C = StringID.Polizas(model.Id_FolioPoliza_C, model.Id_MesPoliza_C);
            if (model.Id_Cuenta_Beneficiario != null)
                model.ListaId_ClasificaBeneficiario = LLenado_ClasificacionBeneficiario(model.Id_Cuenta_Beneficiario, null);
            if (model.Id_TipoCompromiso != null)
                model.Ca_TipoCompromisos = LLenado_CaTipoCompromisos(model.Id_TipoCompromiso);
            if (model.Estatus != null)
                model.Descripcion_Estatus = Diccionarios.EstatusCompromisos[Convert.ToInt16(model.Estatus)];

            return model;
        }
        public Ma_CompromisosModel LLenado_ReporteMaCompromisos(short IdTipoCompromiso, int IdFolioCompromisos)
        {

            Ma_CompromisosModel model = ModelFactory.getModel<Ma_CompromisosModel>(macompromisos.GetByID(x => x.Id_TipoCompromiso == IdTipoCompromiso && x.Id_FolioCompromiso == IdFolioCompromisos), new Ma_CompromisosModel());
            model.De_Compromisos = new List<De_CompromisosModel>();
            model.PagarseEn = LLenado_CaTipoCompromisos(model.Id_TipoCompromiso).Pagarse;
            if (model.Id_Beneficiario != null)
                model.Ca_Beneficiarios = Llenado_CaBeneficiarios(model.Id_Beneficiario);
            if (model.Id_TipoCompromiso != null)
                model.Ca_TipoCompromisos = LLenado_CaTipoCompromisos(model.Id_TipoCompromiso);
            if (model.Estatus != null)
                model.Descripcion_Estatus = Diccionarios.EstatusCompromisos[Convert.ToInt16(model.Estatus)];

            return model;
        }

        public De_CompromisosModel LLenado_DeCompromisos(short IdTipoCompromiso, int IdFolioCompromiso, short IdRegistro)
        {
            De_CompromisosModel model = ModelFactory.getModel<De_CompromisosModel>(decompromisos.GetByID(x => x.Id_TipoCompromiso == IdTipoCompromiso && x.Id_FolioCompromiso == IdFolioCompromiso && x.Id_Registro == IdRegistro), new De_CompromisosModel());
            if (model.Id_Cuenta != null)
            {
                model.Ca_Cuentas = LLenado_CaCuentas(model.Id_Cuenta);
                model.Id_Cuenta = model.Ca_Cuentas.Id_Cuenta;
                //model
            }
            if (model.Id_ClavePresupuesto != null)
            {
                model.Ma_PresupuestoEg = LLenado_MaPresupuestoEG(model.Id_ClavePresupuesto);
                model.Ca_Areas = LLenado_CaAreas(model.Ma_PresupuestoEg.Id_Area);
                model.Id_Area = model.Ca_Areas.Id_Area;
                model.Ca_Funciones = LLenado_CaFunciones(model.Ma_PresupuestoEg.Id_Funcion);
                model.Id_Funcion = model.Ca_Funciones.Id_Funcion;
                model.Ca_ActividadInst = LLenado_CaActividades(model.Ma_PresupuestoEg.Id_Actividad);
                model.Id_Actividad = model.Ca_ActividadInst.Id_Actividad;
                model.Ca_ClasProgramatica = LLenado_CaClasProgramatica(model.Ma_PresupuestoEg.Id_ClasificacionP);
                model.Id_ClasificacionP = model.Ca_ClasProgramatica.Id_ClasificacionP;
                model.Ca_Programas = LLenado_CaProgramas(model.Ma_PresupuestoEg.Id_Programa);
                model.Id_Programa = model.Ca_Programas.Id_Programa;
                model.Ca_Proyecto = LLenado_CaProyecto(model.Ma_PresupuestoEg.Id_Proceso);
                model.Id_Proceso = model.Ca_Proyecto.Id_Proceso;
                model.Ca_TipoMeta = LLenado_CaTipoMeta(model.Ma_PresupuestoEg.Id_TipoMeta);
                model.Id_TipoMeta = model.Ca_TipoMeta.Id_TipoMeta;
                model.Ca_Actividad = LLenado_CaActividad(model.Ma_PresupuestoEg.Id_Proceso, model.Ma_PresupuestoEg.Id_ActividadMIR);
                model.Id_ActividadMIR = model.Ca_Actividad.Id_ActividadMIR2;
                model.Ca_Acciones = LLenado_CaAcciones(model.Ma_PresupuestoEg.Id_Proceso, model.Ma_PresupuestoEg.Id_ActividadMIR, model.Ma_PresupuestoEg.Id_Accion);
                model.Id_Accion = model.Ca_Acciones.Id_Accion2;
                model.Ca_AlcanceGeo = LLenado_CaAlcanceGeo(model.Ma_PresupuestoEg.Id_Alcance);
                model.Id_Alcance = model.Ca_AlcanceGeo.Id_AlcanceGeo;
                model.Ca_TipoGastos = LLenado_CaTipoGastos(model.Ma_PresupuestoEg.Id_TipoG);
                model.Id_TipoG = model.Ca_TipoGastos.Id_TipoGasto;
                model.Ca_FuentesFin = LLenado_CaFuentesFin(model.Ma_PresupuestoEg.Id_Fuente);
                model.Id_Fuente = model.Ca_FuentesFin.Id_Fuente;
                model.Id_Fuente_Filtro = model.Id_Fuente;
                model.AnioFin = model.Ma_PresupuestoEg.AnioFin;
                model.Ca_ObjetoGasto = LLenado_CaObjetoGasto(model.Ma_PresupuestoEg.Id_ObjetoG);
                model.Id_ObjetoG = model.Ca_ObjetoGasto.Id_ObjetoG;
            }

            return model;
        }


        public Ma_ContrarecibosModel LLenado_MaContrarecibos(byte? IdTipoCR, int? IdFolioCR)
        {
            Ma_ContrarecibosModel model = ModelFactory.getModel<Ma_ContrarecibosModel>(macontrarecibos.GetByID(x => x.Id_TipoCR == IdTipoCR && x.Id_FolioCR == IdFolioCR), new Ma_ContrarecibosModel());
            model.De_Contrarecibos = new List<De_ContrarecibosModel>();
            model.Ca_CuentasBancarias = new Ca_CuentasBancariasModel();
            //Llenar si tiene detalles el contrerecibos
            IEnumerable<De_Contrarecibos> lstdcont = decontrarecibos.Get(x => x.Id_TipoCR == IdTipoCR && x.Id_FolioCR == IdFolioCR);
            foreach (De_Contrarecibos item in lstdcont)
            {
                De_ContrarecibosModel decont = LLenado_DeContrarecibos(item.Id_TipoCR, item.Id_FolioCR, item.Id_Registro);
                model.De_Contrarecibos.Add(decont);
            }
            if (model.De_Contrarecibos.Count > 0 && model.Cargos > 0 && model.Id_EstatusCR != Diccionarios.Valores_Estatus_CR.Cancelado)
            {
                switch (model.Id_TipoCR)
                {
                    case Diccionarios.TiposCR.ContraRecibos:
                    case Diccionarios.TiposCR.Arrendamientos:
                    case Diccionarios.TiposCR.Honorarios:
                    case Diccionarios.TiposCR.CancelacionActivos:
                    case Diccionarios.TiposCR.HonorariosAsimilables:
                        Ma_Compromisos compromiso = macompromisos.GetByID(x => x.Id_FolioCR == IdFolioCR && x.Id_TipoCR == IdTipoCR);
                        if (compromiso != null)
                            model.FolioCompromiso = compromiso.Id_FolioCompromiso;
                        break;
                }
            }
            model.NombreCompleto = model.Usu_CR;
            if (model.Id_Beneficiario != null)
                model.Ca_Beneficiarios = Llenado_CaBeneficiarios(model.Id_Beneficiario);
            if (model.IdPersona_ENP != null)
                model.Ca_Personas = Llenado_CaPersonas(model.IdPersona_ENP);

            if (model.Impreso_CH.HasValue)
            {
                if (!string.IsNullOrEmpty(model.Id_Fuente) && model.Impreso_CH.Value)
                {
                    Ca_CuentasBancarias ctaban = cuentasbancarias.GetByID(x => x.Id_Fuente == model.Id_Fuente && x.Id_CtaBancaria == model.Id_CtaBancaria);
                    if (ctaban != null)
                        model.Ca_CuentasBancarias = LLenado_CaCuentasBancarias(ctaban.Id_CtaBancaria);
                }
            }
            if (model.Id_CuentaFR != null)
            {
                model.Ca_Cuentas_FR = LLenado_CaCuentas(model.Id_CuentaFR);
                model.Ca_Beneficiarios = new Ca_BeneficiariosModel();
                model.Ca_Beneficiarios.NombreCompleto = model.Ca_Cuentas_FR.Descripcion;
            }
            if (model.Id_Cuenta_AH != null)
            {
                model.Ca_Cuentas_FR = LLenado_CaCuentas(model.Id_Cuenta_AH);
                //model.Ca_Beneficiarios = new Ca_BeneficiariosModel();
                //model.Ca_Beneficiarios.NombreCompleto = model.Ca_Cuentas_FR.Descripcion;
            }

            model.Ca_Cuentas_FR.Descripcion2 = !String.IsNullOrEmpty(model.Id_Cuenta_AH2) ? DALCuentas.GetByID(x => x.Id_Cuenta == model.Id_Cuenta_AH2).Descripcion : String.Empty;
            model.Ca_Cuentas_FR.Descripcion3 = !String.IsNullOrEmpty(model.Id_Cuenta_AH3) ? DALCuentas.GetByID(x => x.Id_Cuenta == model.Id_Cuenta_AH3).Descripcion : String.Empty;
            model.Ca_Cuentas_FR.Descripcion4 = !String.IsNullOrEmpty(model.Id_Cuenta_AH4) ? DALCuentas.GetByID(x => x.Id_Cuenta == model.Id_Cuenta_AH4).Descripcion : String.Empty;

            if (model.Id_FolioPolizaCR != null)
                model.PolizaCR = StringID.Polizas(model.Id_FolioPolizaCR, model.Id_MesPolizaCR);
            if (model.Id_FolioPolizaCR_C != null)
                model.PolizaCR_C = StringID.Polizas(model.Id_FolioPolizaCR_C, model.Id_MesPolizaCR_C);
            if (model.Id_FolioPO_Ejercido != null)
                model.Poliza_Ejercido = StringID.Polizas(model.Id_FolioPO_Ejercido, model.Id_MesPO_Ejercido);
            if (model.Id_FolioPO_Ejercido_C != null)
                model.Poliza_Ejercido_C = StringID.Polizas(model.Id_FolioPO_Ejercido_C, model.Id_MesPO_Ejercido_C);
            if (model.Id_FolioPO_Pagado != null)
                model.Poliza_Pagado = StringID.Polizas(model.Id_FolioPO_Pagado, model.Id_MesPO_Pagado);
            if (model.Id_FolioPO_Pagado_C != null)
                model.Poliza_Pagado_C = StringID.Polizas(model.Id_FolioPO_Pagado_C, model.Id_MesPO_Pagado_C);
            if (model.Id_FolioPolizaCH != null)
                model.PolizaCheque = StringID.Polizas(model.Id_FolioPolizaCH, model.Id_MesPolizaCH);
            if (model.Id_FolioPolizaCH_C != null)
                model.PolizaCheque_C = StringID.Polizas(model.Id_FolioPolizaCH_C, model.Id_MesPolizaCH_C);
            if (model.Id_TipoCompromiso != null)
            {
                model.Ca_TipoCompromisos = LLenado_CaTipoCompromisos(model.Id_TipoCompromiso.Value);
                model.ListaId_TipoCompromiso = new SelectList(tipocompromiso.Get(), "Id_TipoCompromiso", "Descripcion", model.Id_TipoCompromiso);
            }
            if (model.IdPersona_ENP != null)
                model.Ca_Persona = Llenado_CaPersonas(model.IdPersona_ENP);
            model.Ca_TipoContrarecibos = LLenado_CaTipoContrarecibos(model.Id_TipoCR);
            if (model.Id_EstatusCR != null)
                model.DEscripcionEstatus = Diccionarios.Estatus_CR[(short)model.Id_EstatusCR];
            if (!string.IsNullOrEmpty(model.Id_Fuente))
                model.Ca_FuentesFin = this.LLenado_CaFuentesFin(model.Id_Fuente);
            else
                model.Ca_FuentesFin = new Ca_FuentesFinModel();
            if (model.Id_TipoMovimiento_AH.HasValue)
            {
                model.TipoMovmientoAH = model.Id_TipoMovimiento_AH.Value == 1 ? "Cargo" : "Abono";
            }
            return model;
        }

        public VW_ContrarecibosModel LLenado_VistaMaContrarecibos(byte? IdTipoCR, int? IdFolioCR)
        {
            VW_ContrarecibosModel model = ModelFactory.getModel<VW_ContrarecibosModel>(Vmacontrarecibos.GetByID(x => x.Id_TipoCR == IdTipoCR && x.Id_FolioCR == IdFolioCR), new VW_ContrarecibosModel());
            if (model.Cargos > 0 && model.Id_TipoCR == Diccionarios.TiposCR.ContraRecibos)
                model.FolioCompromiso = macompromisos.GetByID(x => x.Id_FolioCR == IdFolioCR && x.Id_TipoCR == IdTipoCR).Id_FolioCompromiso;
            if (model.Id_CtaBancaria != null)
                model.Ca_CuentasBancarias = LLenado_CaCuentasBancarias(model.Id_CtaBancaria);
            if (model.Id_FolioPolizaCR != null)
                model.PolizaCR = StringID.Polizas(model.Id_FolioPolizaCR, model.Id_MesPolizaCR);
            if (model.Id_FolioPolizaCR_C != null)
                model.PolizaCR_C = StringID.Polizas(model.Id_FolioPolizaCR_C, model.Id_MesPolizaCR_C);
            if (model.Id_FolioPO_Ejercido != null)
                model.Poliza_Ejercido = StringID.Polizas(model.Id_FolioPO_Ejercido, model.Id_MesPO_Ejercido);
            if (model.Id_FolioPO_Ejercido_C != null)
                model.Poliza_Ejercido_C = StringID.Polizas(model.Id_FolioPO_Ejercido_C, model.Id_MesPO_Ejercido_C);
            if (model.Id_FolioPO_Pagado != null)
                model.Poliza_Pagado = StringID.Polizas(model.Id_FolioPO_Pagado, model.Id_MesPO_Pagado);
            if (model.Id_FolioPO_Pagado_C != null)
                model.Poliza_Pagado_C = StringID.Polizas(model.Id_FolioPO_Pagado_C, model.Id_MesPO_Pagado_C);
            if (model.Id_FolioPolizaCH != null)
                model.PolizaCheque = StringID.Polizas(model.Id_FolioPolizaCH, model.Id_MesPolizaCH);
            if (model.Id_FolioPolizaCH_C != null)
                model.PolizaCheque_C = StringID.Polizas(model.Id_FolioPolizaCH_C, model.Id_MesPolizaCH_C);
            if (model.Id_TipoCompromiso != null)
                model.Ca_TipoCompromisos = LLenado_CaTipoCompromisos(model.Id_TipoCompromiso.Value);
            model.Ca_TipoContrarecibos = LLenado_CaTipoContrarecibos(model.Id_TipoCR);
            if (model.Id_EstatusCR != null)
                model.DEscripcionEstatus = Diccionarios.Estatus_CR[(short)model.Id_EstatusCR];
            return model;
        }

        public De_ComprobacionesModel LLenado_DeComprobaciones(int IdFolioGC, short IdConsecutivo)
        {
            De_ComprobacionesModel model = ModelFactory.getModel<De_ComprobacionesModel>(decomprobaciones.GetByID(x => x.Id_FolioGC == IdFolioGC && x.Id_Consecutivo == IdConsecutivo), new De_ComprobacionesModel());
            if (model.Id_Cuenta != null)
                model.Ca_Cuentas = LLenado_CaCuentas(model.Id_Cuenta);
            if (model.Id_CuentaBancaria != null)
                model.Ca_CuentasBancarias = LLenado_CaCuentasBancarias(model.Id_CuentaBancaria.Value);
            return model;
        }

        public Ma_ComprobacionesModel LLenado_MaComprobaciones(int IdFolioGC)
        {
            Ma_ComprobacionesModel model = ModelFactory.getModel<Ma_ComprobacionesModel>(macomprobacionesDAL.GetByID(x => x.Id_FolioGC == IdFolioGC), new Ma_ComprobacionesModel());
            model.De_Comprobaciones = new List<De_ComprobacionesModel>();
            IEnumerable<De_Comprobaciones> lstdcont = decomprobaciones.Get(x => x.Id_FolioGC == IdFolioGC);
            foreach (De_Comprobaciones item in lstdcont)
            {
                De_ComprobacionesModel decont = LLenado_DeComprobaciones(item.Id_FolioGC, item.Id_Consecutivo);
                model.De_Comprobaciones.Add(decont);
            }

            if (model.Id_FolioPO_Comprometido.HasValue)
                model.Poliza_Comprometido = StringID.Polizas(model.Id_FolioPO_Comprometido, model.Id_MesPO_Comprometido);
            if (model.Id_FolioPO_Comprometido_C.HasValue)
                model.Poliza_Comprometido_C = StringID.Polizas(model.Id_FolioPO_Comprometido_C, model.Id_MesPO_Comprometido_C);
            if (model.Id_FolioPO_Devengado.HasValue)
                model.Poliza_Devengado = StringID.Polizas(model.Id_FolioPO_Devengado, model.Id_MesPO_Devengado);
            if (model.Id_FolioPO_Devengado_C.HasValue)
                model.Poliza_Devengado_C = StringID.Polizas(model.Id_FolioPO_Devengado_C, model.Id_MesPO_Devengado_C);

            return model;
        }

        public Ma_TransferenciasModel LLenado_MaTransferencias(Int32 idTransferencia)
        {
            Ma_TransferenciasModel model = ModelFactory.getModel<Ma_TransferenciasModel>(MaTranferenciasDAL.GetByID(x => x.Id_Transferencia == idTransferencia), new Ma_TransferenciasModel());
            model.De_Transferencias = new List<De_TransferenciaModel>();
            //Llenar si tiene detalles el contrerecibos
            IEnumerable<De_Transferencia> lstdtrans = DeTransferenciasDAL.Get(x => x.Id_Transferencia == idTransferencia);
            foreach (De_Transferencia item in lstdtrans)
            {
                De_TransferenciaModel decont = LLenado_DeTransferencias(item.Id_Transferencia, item.Id_Consecutivo);
                model.De_Transferencias.Add(decont);
            }
            if (model.Id_Area != null)
                model.Ca_Areas = LLenado_CaAreas(model.Id_Area);
            return model;
        }
        public Ma_TransferenciasIngModel LLenado_MaTransferenciasIng(Int32 folio)
        {
            Ma_TransferenciasIngModel model = ModelFactory.getModel<Ma_TransferenciasIngModel>(MaTransferenciasIngDAL.GetByID(x => x.Folio == folio), new Ma_TransferenciasIngModel());
            model.De_Transferencias = new List<De_TransferenciaIngModel>();
            //Llenar si tiene detalles el contrerecibos
            IEnumerable<De_TransferenciaIng> lstdtrans = DeTransferenciaIngDAL.Get(x => x.Folio == folio);
            foreach (De_TransferenciaIng item in lstdtrans)
            {
                De_TransferenciaIngModel decont = LLenado_DeTransferenciasIng(item.Folio, item.IdRegistro);
                model.De_Transferencias.Add(decont);
            }
            if (model.Id_Area != null)
                model.Ca_Areas = LLenado_CaAreas(model.Id_Area);
            return model;
        }
        public De_TransferenciaModel LLenado_DeTransferencias(Int32 idTransferencia, Int32 idConsecutivo)
        {
            De_TransferenciaModel model = ModelFactory.getModel<De_TransferenciaModel>(DeTransferenciasDAL.GetByID(x => x.Id_Transferencia == idTransferencia && x.Id_Consecutivo == idConsecutivo), new De_TransferenciaModel());

            if (model.Id_ClavePresupuesto != null)
            {
                model.Ma_PresupuestoEg = LLenado_MaPresupuestoEG(model.Id_ClavePresupuesto);
                model.Ca_Areas = LLenado_CaAreas(model.Ma_PresupuestoEg.Id_Area);
                model.Id_Area = model.Ca_Areas.Id_Area;
                model.Ca_Funciones = LLenado_CaFunciones(model.Ma_PresupuestoEg.Id_Funcion);
                model.Id_Funcion = model.Ca_Funciones.Id_Funcion;
                model.Ca_ActividadInst = LLenado_CaActividades(model.Ma_PresupuestoEg.Id_Actividad);
                model.Id_Actividad = model.Ca_ActividadInst.Id_Actividad;
                model.Ca_ClasProgramatica = LLenado_CaClasProgramatica(model.Ma_PresupuestoEg.Id_ClasificacionP);
                model.Id_ClasificacionP = model.Ca_ClasProgramatica.Id_ClasificacionP;
                model.Ca_Programas = LLenado_CaProgramas(model.Ma_PresupuestoEg.Id_Programa);
                model.Id_Programa = model.Ca_Programas.Id_Programa;
                model.Ca_Proyecto = LLenado_CaProyecto(model.Ma_PresupuestoEg.Id_Proceso);
                model.Id_Proceso = model.Ca_Proyecto.Id_Proceso;
                model.Ca_TipoMeta = LLenado_CaTipoMeta(model.Ma_PresupuestoEg.Id_TipoMeta);
                model.Id_TipoMeta = model.Ca_TipoMeta.Id_TipoMeta;
                model.Ca_Actividad = LLenado_CaActividad(model.Ma_PresupuestoEg.Id_Proceso, model.Ma_PresupuestoEg.Id_ActividadMIR);
                model.Id_ActividadMIR = model.Ca_Actividad.Id_ActividadMIR2;
                model.Ca_Acciones = LLenado_CaAcciones(model.Ma_PresupuestoEg.Id_Proceso, model.Ma_PresupuestoEg.Id_ActividadMIR, model.Ma_PresupuestoEg.Id_Accion);
                model.Id_Accion = model.Ca_Acciones.Id_Accion2;
                model.Ca_AlcanceGeo = LLenado_CaAlcanceGeo(model.Ma_PresupuestoEg.Id_Alcance);
                model.Id_Alcance = model.Ca_AlcanceGeo.Id_AlcanceGeo;
                model.Ca_TipoGastos = LLenado_CaTipoGastos(model.Ma_PresupuestoEg.Id_TipoG);
                model.Id_TipoG = model.Ca_TipoGastos.Id_TipoGasto;
                model.Ca_FuentesFin = LLenado_CaFuentesFin(model.Ma_PresupuestoEg.Id_Fuente);
                model.Id_Fuente = model.Ca_FuentesFin.Id_Fuente;
                model.AnioFin = model.Ma_PresupuestoEg.AnioFin;
                model.Ca_ObjetoGasto = LLenado_CaObjetoGasto(model.Ma_PresupuestoEg.Id_ObjetoG);
                model.Id_ObjetoG = model.Ca_ObjetoGasto.Id_ObjetoG;
            }
            return model;
        }
        public De_TransferenciaIngModel LLenado_DeTransferenciasIng(Int32 Folio, Int32 IdRegistro)
        {
            De_TransferenciaIngModel model = ModelFactory.getModel<De_TransferenciaIngModel>(DeTransferenciaIngDAL.GetByID(x => x.Folio == Folio && x.IdRegistro == IdRegistro), new De_TransferenciaIngModel());

            if (model.Id_ClavePresupuestoIng != null)
            {
                model.Ma_PresupuestoIng = LLenado_MaPresupuestoING(model.Id_ClavePresupuestoIng);
                model.Ca_CentroRecaudador = LLenado_CaCentroRecaudador(model.Ma_PresupuestoIng.Id_CentroRecaudador);
                model.Id_CentroRecaudador = model.Ca_CentroRecaudador.Id_CRecaudador;
                model.Ca_FuentesFin_Ing = LLenado_CaFuentesFin_Ing(model.Ma_PresupuestoIng.Id_Fuente);
                model.Id_Fuente = model.Ca_FuentesFin_Ing.Id_FuenteFinancia;
                model.Ca_AlcanceGeo = LLenado_CaAlcanceGeo(model.Ma_PresupuestoIng.Id_Alcance);
                model.Id_Alcance = model.Ca_AlcanceGeo.Id_AlcanceGeo;
                model.Ca_ConceptoIngresos = LLenado_CaConceptosIngresos(model.Ma_PresupuestoIng.Id_Concepto);
                model.Id_Concepto = model.Ca_ConceptoIngresos.Id_Concepto;
                model.AnioFin = model.Ma_PresupuestoIng.AnioFin;
            }
            return model;
        }
        public De_ContrarecibosModel LLenado_DeContrarecibos(byte IdTipoCR, int IdFolioCR, short IdConsecutivo)
        {
            De_ContrarecibosModel model = ModelFactory.getModel<De_ContrarecibosModel>(decontrarecibos.GetByID(x => x.Id_TipoCR == IdTipoCR && x.Id_FolioCR == IdFolioCR && x.Id_Registro == IdConsecutivo), new De_ContrarecibosModel());
            if (model.Id_Cuenta != null)
                model.Ca_Cuentas = LLenado_CaCuentas(model.Id_Cuenta);
            if (model.Id_ClavePresupuesto != null)
                model.Ma_PresupuestoEg = LLenado_MaPresupuestoEG(model.Id_ClavePresupuesto);
            model.TipoContrarecibo = tipocontrarecibo.GetByID(x => x.Id_TipoCR == model.Id_TipoCR).Descripcion;
            model.TipoMovimiento = Diccionarios.Movimiento[Convert.ToInt16(model.Id_Movimiento)];
            if (model.Id_ClavePresupuesto != null)
            {
                model.Ma_PresupuestoEg = LLenado_MaPresupuestoEG(model.Id_ClavePresupuesto);
                model.Ca_Areas = LLenado_CaAreas(model.Ma_PresupuestoEg.Id_Area);
                model.Id_Area = model.Ca_Areas.Id_Area;
                model.Ca_Funciones = LLenado_CaFunciones(model.Ma_PresupuestoEg.Id_Funcion);
                model.Id_Funcion = model.Ca_Funciones.Id_Funcion;
                model.Ca_ActividadInst = LLenado_CaActividades(model.Ma_PresupuestoEg.Id_Actividad);
                model.Id_Actividad = model.Ca_ActividadInst.Id_Actividad;
                model.Ca_ClasProgramatica = LLenado_CaClasProgramatica(model.Ma_PresupuestoEg.Id_ClasificacionP);
                model.Id_ClasificacionP = model.Ca_ClasProgramatica.Id_ClasificacionP;
                model.Ca_Programas = LLenado_CaProgramas(model.Ma_PresupuestoEg.Id_Programa);
                model.Id_Programa = model.Ca_Programas.Id_Programa;
                model.Ca_Proyecto = LLenado_CaProyecto(model.Ma_PresupuestoEg.Id_Proceso);
                model.Id_Proceso = model.Ca_Proyecto.Id_Proceso;
                model.Ca_TipoMeta = LLenado_CaTipoMeta(model.Ma_PresupuestoEg.Id_TipoMeta);
                model.Id_TipoMeta = model.Ca_TipoMeta.Id_TipoMeta;
                model.Ca_Actividad = LLenado_CaActividad(model.Ma_PresupuestoEg.Id_Proceso, model.Ma_PresupuestoEg.Id_ActividadMIR);
                model.Id_ActividadMIR = model.Ca_Actividad.Id_ActividadMIR2;
                model.Ca_Acciones = LLenado_CaAcciones(model.Ma_PresupuestoEg.Id_Proceso, model.Ma_PresupuestoEg.Id_ActividadMIR, model.Ma_PresupuestoEg.Id_Accion);
                model.Id_Accion = model.Ca_Acciones.Id_Accion2;
                model.Ca_AlcanceGeo = LLenado_CaAlcanceGeo(model.Ma_PresupuestoEg.Id_Alcance);
                model.Id_Alcance = model.Ca_AlcanceGeo.Id_AlcanceGeo;
                model.Ca_TipoGastos = LLenado_CaTipoGastos(model.Ma_PresupuestoEg.Id_TipoG);
                model.Id_TipoG = model.Ca_TipoGastos.Id_TipoGasto;
                model.Ca_FuentesFin = LLenado_CaFuentesFin(model.Ma_PresupuestoEg.Id_Fuente);
                model.Id_Fuente = model.Ca_FuentesFin.Id_Fuente;
                model.AnioFin = model.Ma_PresupuestoEg.AnioFin;
                model.Ca_ObjetoGasto = LLenado_CaObjetoGasto(model.Ma_PresupuestoEg.Id_ObjetoG);
                model.Id_ObjetoG = model.Ca_ObjetoGasto.Id_ObjetoG;
            }

            return model;
        }

        public De_FacturasModel LLenado_DeFacturas(Int32 FolioCR, Byte TipoCR, Int32 IdFactura, Int32 IdProveedor)
        {
            De_FacturasModel model = ModelFactory.getModel<De_FacturasModel>(dalFacturas.GetByID(x => x.Id_FolioCR == FolioCR && x.Id_TipoCR == TipoCR && x.Id_Factura == IdFactura && x.Id_Proveedor == IdProveedor), new De_FacturasModel());
            Ca_Beneficiarios bene = beneficiarios.GetByID(x => x.Id_Beneficiario == model.Id_Proveedor);
            if (bene != null)
            {
                CA_Personas persona = personas.GetByID(x => x.IdPersona == bene.IdPersona);
                if (persona.PersonaFisica == true)
                    model.ProveedorName = String.Format("{0} {1} {2}", persona.Nombre, persona.ApellidoPaterno, persona.ApellidoMaterno);
                else
                    model.ProveedorName = persona.RazonSocial;
            }
            Ca_TipoDoctos docto = tipodoctos.GetByID(x => x.Id_Tipodocto == model.Id_TipoDocto);
            if (docto != null)
                model.TipoDocumento = docto.Descripcion;
            //Ma_Contrarecibos contra = macontrarecibos.GetByID(x => x.Id_FolioCR == FolioCR && x.Id_TipoCR == TipoCR);
            //if(contra != null)
            //{
            //    bene = beneficiarios.GetByID(x => x.Id_Beneficiario == contra.Id_Beneficiario);
            //    if (bene.TipoPersona == 1)
            //        model.BeneficiarioName = String.Format("{0} {1} {2}", bene.Nombre, bene.ApellidoPaterno, bene.ApellidoMaterno);
            //    else
            //        model.BeneficiarioName = bene.Representante;
            //}
            return model;
        }



        /// <summary>
        /// FALTA DE LLENAR LA INFO
        /// </summary>
        /// <param name="IdClavePresupuesto"></param>
        /// <returns></returns>
        public MA_PresupuestoEgModel LLenado_MaPresupuestoEG(string IdClavePresupuesto)
        {
            MA_PresupuestoEgModel model = ModelFactory.getModel<MA_PresupuestoEgModel>(mapresupuestoeg.GetByID(x => x.Id_ClavePresupuesto == IdClavePresupuesto), new MA_PresupuestoEgModel());
            if (model.Id_Area != null)
                model.Ca_Areas = LLenado_CaAreas(model.Id_Area);
            if (model.Id_Funcion != null)
                model.Ca_Funciones = LLenado_CaFunciones(model.Id_Funcion);
            if (model.Id_Actividad != null)
                model.Ca_ActividadInst = LLenado_CaActividades(model.Id_Actividad);
            if (model.Id_ClasificacionP != null)
                model.Ca_ClasProgramatica = LLenado_CaClasProgramatica(model.Id_ClasificacionP);
            if (model.Id_Programa != null)
                model.Ca_Programas = LLenado_CaProgramas(model.Id_Programa);
            if (model.Id_Proceso != null)
                model.Ca_Proyecto = LLenado_CaProyecto(model.Id_Proceso);
            if (model.Id_TipoMeta != null)
                model.Ca_TipoMeta = LLenado_CaTipoMeta(model.Id_TipoMeta);
            if (model.Id_ActividadMIR != null)
                model.Ca_Actividad = LLenado_CaActividad(model.Id_Proceso, model.Id_ActividadMIR);
            if (model.Id_Accion != null)
                model.Ca_Acciones = LLenado_CaAcciones(model.Id_Proceso, model.Id_ActividadMIR, model.Id_Accion);
            if (model.Id_Alcance != null)
                model.Ca_AlcanceGeo = LLenado_CaAlcanceGeo(model.Id_Alcance);
            if (model.Id_TipoG != null)
                model.Ca_TipoGastos = LLenado_CaTipoGastos(model.Id_TipoG);
            if (model.Id_Fuente != null)
                model.Ca_FuentesFin = LLenado_CaFuentesFin(model.Id_Fuente);
            if (model.Id_ObjetoG != null)
                model.Ca_ObjetoGasto = LLenado_CaObjetoGasto(model.Id_ObjetoG);
            if (model.Id_MesPO_Aprobado != null)
                model.FolioPoliza = StringID.Polizas(model.Id_FolioPO_Aprobado, model.Id_MesPO_Aprobado);
            if (model.Fecha_Aprobado.HasValue)
                model.Fecha_String = model.Fecha_Aprobado.Value.ToShortDateString();
            if (model.Id_ClavePresupuesto != null)
                model.Id_ClavePresupuestoEgFormato = StringID.IdClavePresupuestoFormato(model.Id_ClavePresupuesto);
            return model;
        }

        public Ma_PresupuestoIngModel LLenado_MaPresupuestoING(string IdClavePresupuesto)
        {
            Ma_PresupuestoIngModel model = ModelFactory.getModel<Ma_PresupuestoIngModel>(MaPresupuestoIngDAL.GetByID(x => x.Id_ClavePresupuesto == IdClavePresupuesto), new Ma_PresupuestoIngModel());
            if (model.Id_CentroRecaudador != null)
                model.Ca_CentroRecaudador = LLenado_CaCentroRecaudador(model.Id_CentroRecaudador);
            if (model.Id_Alcance != null)
                model.Ca_AlcanceGeo = LLenado_CaAlcanceGeo(model.Id_Alcance);
            if (model.Id_Fuente != null)
                model.Ca_FuentesFin_Ing = LLenado_CaFuentesFin_Ing(model.Id_Fuente);
            if (model.Id_Concepto != null)
                model.Ca_ConceptosIngresos = LLenado_CaConceptosIngresos(model.Id_Concepto);
            model.FolioPoliza = StringID.Polizas(model.Id_FolioPO_Estimado, model.Id_MesPO_Estimado);
            model.Id_ClavePresupuestoFormato = StringID.IdClavePresupuestoIngFormato(IdClavePresupuesto);
            return model;
        }
        public Ma_ReciboIngresosModel LLenado_MaRecibosIngresos(int IdRecibo)
        {
            Ma_ReciboIngresosModel model = ModelFactory.getModel<Ma_ReciboIngresosModel>(recibosDAL.GetByID(x => x.Folio == IdRecibo), new Ma_ReciboIngresosModel());
            //model.ListaCa_CajasReceptoras = _listas.ListaCajasReceptoras();
            model.EstatusDescipcion = Diccionarios.EstatusRecibos[(byte)model.IdEstatus];
            model.cfechas = new Control_Fechas();
            Ca_PersonasModel persona = this.Llenado_CaPersonas(model.IdContribuyente);
            model.DomicilioContribuyente = String.Format("{0} {1} {2}, C.P: {3} {4}, {5},{6}", persona.Ca_CallesModel.Descripcion, persona.NumeroExt, persona.NumeroInt, persona.Colonia, persona.CP, persona.Ca_MunicipiosModel.Descripcion, persona.Ca_LocalidadesModel.Descripcion, persona.Ca_EstadosModel.Descripcion);
            model.NombreContribuyente = String.Format("{0} {1} {2}", persona.Nombre, persona.ApellidoPaterno, persona.ApellidoMaterno);
            /*Polizas*/
            if (model.IdMesPDevengado != null)
                model.Poliza_Devengado = StringID.Polizas(model.IdFolioPDevengado, model.IdMesPDevengado);
            if (model.IdFolioPDevengadoC != null)
                model.Poliza_DevengadoC = StringID.Polizas(model.IdFolioPDevengadoC, model.IdMesPDevengadoC);
            if (model.IdMesPRecaudado != null)
                model.Poliza_Recaudado = StringID.Polizas(model.IdFolioPRecaudado, model.IdMesPRecaudado);
            if (model.IdMesPRecaudadoC != null)
                model.Poliza_RecaudadoC = StringID.Polizas(model.IdFolioPRecaudadoC, model.IdMesPRecaudadoC);
            if (model.Id_MesPIngresos != null)
                model.Poliza_Ingresos = StringID.Polizas(model.Id_FolioPIngresos, model.Id_MesPIngresos);
            if (model.Id_MesPIngresosC != null)
                model.Poliza_IngresosC = StringID.Polizas(model.Id_FolioPIngresosC, model.Id_MesPIngresosC);
            if (model.Id_MesPDiario != null)
                model.Poliza_Diario = StringID.Polizas(model.Id_FolioPDiario, model.Id_MesPDiario);
            if (model.Id_MesPDiarioC != null)
                model.Poliza_DiarioC = StringID.Polizas(model.Id_FolioPDiarioC, model.Id_MesPDiarioC);

            deRecibos.Get(x => x.Folio == model.Folio && x.Id_Movimiento == Diccionarios.ValorMovimientos.ABONO).ToList().ForEach(x => model.De_ReciboIngresos.Add(this.LLenado_DeRecibos(x.Folio, x.IdRegistro)));
            model.ListaId_Banco = new SelectList(bancos.Get(), "Id_Banco", "Descripcion", model.Id_Banco);
            model.ListaId_CtaBancaria = new SelectList(cuentasbancarias.Get(x => x.Id_Banco == model.Id_Banco), "Id_CtaBancaria", "Descripcion", model.Id_CtaBancaria);
            return model;
        }

        public De_ReciboIngresosModel LLenado_DeRecibos(int Folio, int IdRegistro)
        {
            De_ReciboIngresosModel model = ModelFactory.getModel<De_ReciboIngresosModel>(deRecibos.GetByID(x => x.IdRegistro == IdRegistro && x.Folio == Folio), new De_ReciboIngresosModel());
            model.Ca_Cuentas = this.LLenado_CaCuentas(model.Id_Cuenta);
            if (!String.IsNullOrEmpty(model.Id_ClavePresupuestoIng))
            {
                model.Ca_CentroRecaudador = LLenado_CaCentroRecaudador(model.Id_ClavePresupuestoIng.Substring(0, 6));
                model.Id_CentroRecaudador = model.Ca_CentroRecaudador.Id_CRecaudador;
                model.Ca_FuentesFin_Ing = LLenado_CaFuentesFin_Ing(model.Id_ClavePresupuestoIng.Substring(6, 4));
                model.Id_Fuente = model.Ca_FuentesFin_Ing.Id_FuenteFinancia;
                model.AnioFin = model.Id_ClavePresupuestoIng.Substring(10, 2);
                model.Ca_AlcanceGeo = LLenado_CaAlcanceGeo(model.Id_ClavePresupuestoIng.Substring(12, 1));
                model.Id_Alcance = model.Ca_AlcanceGeo.Id_AlcanceGeo;
                model.Ca_ConceptoIngresos = LLenado_CaConceptosIngresos(model.Id_ClavePresupuestoIng.Substring(13, 3));
                model.Ca_Cur = ModelFactory.getModel<CA_CURModel>(curDal.GetByID(x => x.IdCUR == model.IdCur), new CA_CURModel());
                model.Id_Concepto = model.Ca_ConceptoIngresos.Id_Concepto;
                model.IdCur = model.Ca_Cur.IdCUR;
            }
            return model;
        }
        public VW_ProvedoresUsadosModel LLenado_vProveedoresUsados(VW_ProvedoresUsados entitie)
        {
            VW_ProvedoresUsadosModel model = ModelFactory.getModel<VW_ProvedoresUsadosModel>(entitie, new VW_ProvedoresUsadosModel());
            if (model.IdEstado.HasValue)
                model.Estado = estados.GetByID(x => x.Id_Estado == model.IdEstado).Descripcion;
            if (model.IdEstado.HasValue && model.IdMunicipio.HasValue)
                model.Municipio = municipios.GetByID(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio).Descripcion;
            if (model.IdEstado.HasValue && model.IdMunicipio.HasValue && model.IdLocalidad.HasValue)
                model.Localidad = localidades.GetByID(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad).Descripcion;
            if (model.IdEstado.HasValue && model.IdMunicipio.HasValue && model.IdLocalidad.HasValue && model.IdColonia.HasValue)
                model.Colonia = colonias.GetByID(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad && x.id_colonia == model.IdColonia).Descripcion;
            if (model.IdEstado.HasValue && model.IdMunicipio.HasValue && model.IdLocalidad.HasValue && model.IdCalle.HasValue)
                model.Calle = calles.GetByID(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad && x.id_calle == model.IdCalle).Descripcion;
            model.TipoCR = tipocontrarecibo.GetByID(x => x.Id_TipoCR == model.Id_TipoCR).Descripcion;
            if (model.Id_TipoDocto.HasValue)
                model.TipoDocto = tipodoctos.GetByID(x => x.Id_Tipodocto == model.Id_TipoDocto).Descripcion;
            if (model.Id_Deduccion.HasValue)
                model.DescripcionDeduccion = impuestodeduccion.GetByID(x => x.Id_Tipo_ImpDed == 2 && x.Id_ImpDed == model.Id_Deduccion).Descripcion;
            if (model.Id_Impuesto.HasValue)
                model.DescripcionImpuesto = impuestodeduccion.GetByID(x => x.Id_Tipo_ImpDed == 1 && x.Id_ImpDed == model.Id_Impuesto).Descripcion;
            return model;
        }
        public VW_ProvedoresUsadosModel LLenado_vProveedoresUsados(VW_ProvedoresUsadosAgrupado entitie)
        {
            VW_ProvedoresUsadosModel model = ModelFactory.getModel<VW_ProvedoresUsadosModel>(entitie, new VW_ProvedoresUsadosModel());
            if (model.IdEstado.HasValue)
                model.Estado = estados.GetByID(x => x.Id_Estado == model.IdEstado).Descripcion;
            if (model.IdEstado.HasValue && model.IdMunicipio.HasValue)
                model.Municipio = municipios.GetByID(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio).Descripcion;
            if (model.IdEstado.HasValue && model.IdMunicipio.HasValue && model.IdLocalidad.HasValue)
                model.Localidad = localidades.GetByID(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad).Descripcion;
            if (model.IdEstado.HasValue && model.IdMunicipio.HasValue && model.IdLocalidad.HasValue && model.IdColonia.HasValue)
                model.Colonia = colonias.GetByID(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad && x.id_colonia == model.IdColonia).Descripcion;
            if (model.IdEstado.HasValue && model.IdMunicipio.HasValue && model.IdLocalidad.HasValue && model.IdCalle.HasValue)
                model.Calle = calles.GetByID(x => x.Id_Estado == model.IdEstado && x.Id_Municipio == model.IdMunicipio && x.Id_Localidad == model.IdLocalidad && x.id_calle == model.IdCalle).Descripcion;
            //model.TipoCR = tipocontrarecibo.GetByID(x => x.Id_TipoCR == model.Id_TipoCR).Descripcion;
            //if (model.Id_TipoDocto.HasValue)
            //    model.TipoDocto = tipodoctos.GetByID(x => x.Id_Tipodocto == model.Id_TipoDocto).Descripcion;
            return model;
        }

    }
}