using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Common.Contracts;
using FacturaElectronica.Data;
using System.Data.Objects;
using FacturaElectronica.Common.Constants;

namespace FacturaElectronica.Business.Services
{
    public class ComprobanteService : IComprobanteService
    {
        #region [Comprobantes]

        public void CrearComprobante(ComprobanteDto dto)
        { 
            using(var ctx = new FacturaElectronicaEntities())
            {
                Comprobante cbte = new Comprobante();
                ToComprobante(dto, cbte);
                ctx.Comprobantes.AddObject(cbte);
                ctx.SaveChanges();
            }
        }

        public List<ComprobanteArchivoAsociadoDto> ObtenerComprobantesPorCliente(ComprobanteCriteria criteria)
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                return (from aa in ctx.ArchivoAsociadoes
                        join c in ctx.Comprobantes on aa.ComprobanteId equals c.Id
                        where 
                        // Fecha Vencimiento
                        (!criteria.FechaVencDesde.HasValue || criteria.FechaVencDesde.Value <= aa.FechaVencimiento)
                        && (!criteria.FechaVencHasta.HasValue || aa.FechaVencimiento <= criteria.FechaVencHasta.Value)
                        // Fecha De Carga
                        && (!criteria.FechaDeCargaDesde.HasValue || criteria.FechaDeCargaDesde.Value <= aa.FechaDeCarga)
                        && (!criteria.FechaDeCargaHasta.HasValue || aa.FechaDeCarga <= criteria.FechaDeCargaHasta.Value)
                        // Razon Social
                        && (string.IsNullOrEmpty(criteria.RazonSocial) || c.Cliente.RazonSocial.Contains(criteria.RazonSocial))
                        // Tipo Comprobante
                        && (!criteria.TipoComprobanteId.HasValue || c.TipoComprobanteId == criteria.TipoComprobanteId.Value)
                        // Nro Comprobante
                        && (!criteria.NroComprobante.HasValue || aa.NroComprobante == criteria.NroComprobante.Value)
                        // Cliente Id
                        && (!criteria.ClienteId.HasValue || c.ClienteId == criteria.ClienteId.Value)
                        // Tipo Contrato
                        && (!criteria.TipoContratoId.HasValue || aa.TipoContratoId == criteria.TipoContratoId.Value)
                        // Periodo Facturacion
                        && (!criteria.MesFacturacion.HasValue || aa.MesFacturacion == criteria.MesFacturacion.Value)
                        && (!criteria.AnioFacturacion.HasValue || aa.AnioFacturacion == criteria.AnioFacturacion.Value)
                        // Documentos Vencidos
                        && (!criteria.IncluirDocumentosVencidos || aa.FechaVencimiento < DateTime.Now)
                        // Sacar Documentos Eliminados
                        && (aa.EstadoArchivoAsociado.Codigo != CodigosEstadoArchivoAsociado.Eliminado)
                        select new ComprobanteArchivoAsociadoDto()
                        {            
                            ArchivoAsociadoId = aa.Id,
                            ClienteId = c.ClienteId,
                            ClienteRazonSocial = c.Cliente.RazonSocial,
                            ComprobanteId = c.Id,
                            EstadoCodigo = aa.EstadoArchivoAsociado.Codigo,
                            EstadoDescripcion = aa.EstadoArchivoAsociado.Descripcion,
                            FechaDeCarga = aa.FechaDeCarga,
                            FechaVencimiento = aa.FechaVencimiento,
                            DiasVencimiento = aa.DiasVencimiento,
                            NroComprobante = aa.NroComprobante,
                            PathArchivo = aa.PathArchivo,
                            PtoVta = c.PtoVta,
                            TipoComprobanteId = c.TipoComprobanteId,
                            TipoComprobanteDescripcion = c.TipoComprobante.Descripcion,
                            TipoContratoId = aa.TipoContratoId,
                            TipoContratoDescripcion = aa.TipoContratoId.HasValue ? aa.TipoContrato.Descripcion : string.Empty,
                         }).ToList();         
                
   
            }
        }

        public List<ComprobanteArchivoAsociadoDto> ObtenerComprobantes(ComprobanteCriteria criteria)
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                return (from aa in ctx.ArchivoAsociadoes
                        join c in ctx.Comprobantes on aa.ComprobanteId equals c.Id
                        where
                            // Fecha Vencimiento
                        (!criteria.FechaVencDesde.HasValue || criteria.FechaVencDesde.Value <= aa.FechaVencimiento)
                        && (!criteria.FechaVencHasta.HasValue || aa.FechaVencimiento <= criteria.FechaVencHasta.Value)
                            // Fecha De Carga
                        && (!criteria.FechaDeCargaDesde.HasValue || criteria.FechaDeCargaDesde.Value <= aa.FechaDeCarga)
                        && (!criteria.FechaDeCargaHasta.HasValue || aa.FechaDeCarga <= criteria.FechaDeCargaHasta.Value)
                            // Razon Social
                        && (string.IsNullOrEmpty(criteria.RazonSocial) || c.Cliente.RazonSocial.Contains(criteria.RazonSocial))
                            // Tipo Comprobante
                        && (!criteria.TipoComprobanteId.HasValue || c.TipoComprobanteId == criteria.TipoComprobanteId.Value)
                            // Nro Comprobante
                        && (!criteria.NroComprobante.HasValue || aa.NroComprobante == criteria.NroComprobante.Value)
                            // Cliente Id
                        && (!criteria.ClienteId.HasValue || c.ClienteId == criteria.ClienteId.Value)
                            // Tipo Contrato
                        && (!criteria.TipoContratoId.HasValue || aa.TipoContratoId == criteria.TipoContratoId.Value)
                            // Periodo Facturacion
                        && (!criteria.MesFacturacion.HasValue || aa.MesFacturacion == criteria.MesFacturacion.Value)
                        && (!criteria.AnioFacturacion.HasValue || aa.AnioFacturacion == criteria.AnioFacturacion.Value)
                            // Documentos Vencidos
                        && (!criteria.IncluirDocumentosVencidos || aa.FechaVencimiento < DateTime.Now)
                            // Sacar Documentos Eliminados
                        && (aa.EstadoArchivoAsociado.Codigo != CodigosEstadoArchivoAsociado.Eliminado)
                        select new ComprobanteArchivoAsociadoDto()
                        {
                            ArchivoAsociadoId = aa.Id,
                            CAE = c.CAE,
                            CAEFechaVencimiento = c.CAEFechaVencimiento,
                            ClienteId = c.ClienteId,
                            ClienteRazonSocial = c.Cliente.RazonSocial,
                            ComprobanteId = c.Id,
                            EstadoCodigo = aa.EstadoArchivoAsociado.Codigo,
                            EstadoDescripcion = aa.EstadoArchivoAsociado.Descripcion,
                            FechaDeCarga = aa.FechaDeCarga,
                            FechaVencimiento = aa.FechaVencimiento,
                            DiasVencimiento = aa.DiasVencimiento,
                            NroComprobante = aa.NroComprobante,
                            PathArchivo = aa.PathArchivo,
                            PtoVta = c.PtoVta,
                            TipoComprobanteId = c.TipoComprobanteId,
                            TipoComprobanteDescripcion = c.TipoComprobante.Descripcion,
                            TipoContratoId = aa.TipoContratoId,
                            TipoContratoDescripcion = aa.TipoContratoId.HasValue ? aa.TipoContrato.Descripcion : string.Empty,
                        }).ToList();


            }
        }

        public ComprobanteDto ObtenerUltimoComprobanteCargado(int ptoVta, int cbeTipo)
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                Comprobante cbte = ctx.Comprobantes.Where(c => c.PtoVta == ptoVta && c.TipoComprobante.CodigoAfip == cbeTipo).OrderByDescending(c=>c.CbteDesde).FirstOrDefault();
                return ToComprobanteDto(cbte);
            }
        }

        public ComprobanteDto ObtenerComprobante(long comprobanteId)
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                return ToComprobanteDto(ctx.Comprobantes.Where(c => c.Id == comprobanteId).FirstOrDefault());
            }
        }

        #endregion [Comprobantes]

        #region [Tipos Comprobante]

        public TipoComprobanteDto ObtenerTipoComprobantePorCodigoAfip(int codigoAfip)
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                TipoComprobante tipoCbte = ctx.TipoComprobantes.Where(t => t.CodigoAfip == codigoAfip).FirstOrDefault();
                return ToTipoComprobanteDto(tipoCbte);
            }
        }

        public List<TipoComprobanteDto> ObtenerTiposComprobantes()
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                var list = (from aa in ctx.ArchivoAsociadoes
                             select aa.Comprobante.TipoComprobante).Distinct().OrderBy(tc => tc.Descripcion).ToList();

                return ToTipoComprobanteDtoList(list);
            }
        
        }

        #endregion [Tipos Comprobante]

        #region [Estados]

        //public EstadoComprobanteDto ObtenerEstado(string codigo)
        //{
        //    using (var ctx = new FacturaElectronicaEntities())
        //    {
        //        return ToEstadoComprobanteDto(ctx.EstadoComprobantes.Where(e => e.Codigo == codigo).FirstOrDefault());
        //    }            
        //}

        #endregion [Estados]

        #region [Tipos Contrato]

        public List<TipoContratoDto> ObtenerTiposContrato()
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                // Obtengo el listado con los tipos de contrato que existen en la base
                var tiposContrato = (from aa in ctx.ArchivoAsociadoes
                                    select aa.TipoContrato).Distinct().ToList();

                return ToTipoContratoDtoList(tiposContrato);
            }
        }

        #endregion [Tipo Contrato]

        #region [Visualizaciones]

        public void AgregarVisualizacion(VisualizacionComprobanteDto dto)
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                ctx.Comprobantes.Where(c => c.Id == dto.ArchivoAsociadoId);
            }
        }

        #endregion [Visualizaciones]

        public List<int> ObtenerAniosFacturacion()
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                // Si no hay archivos asociados con ningun periodo de facturacion
                List<int> aniosFacturacion = (from aa in ctx.ArchivoAsociadoes
                                              select aa.AnioFacturacion).Distinct().ToList();
                aniosFacturacion.Sort();
                
                if (aniosFacturacion.Count == 0)
                {
                    aniosFacturacion.Add(DateTime.Now.Year);
                }
                return aniosFacturacion;
            }
        }

        #region [Conversion]

        #region [Comprobante]
        
        private static void ToComprobante(ComprobanteDto dto, Comprobante cbte)
        {
            cbte.CAE = dto.CAE;
            cbte.CAEFechaVencimiento = dto.CAEFechaVencimiento;
            cbte.FechaDeCarga = DateTime.Now;
            cbte.CbteDesde = dto.CbteDesde;
            cbte.CbteHasta = dto.CbteHasta;
            cbte.CbteFecha = dto.CbteFecha;
            cbte.ClienteId = dto.ClienteId;
            cbte.EstadoId = dto.EstadoId;
            cbte.TipoComprobanteId = dto.TipoComprobanteId;
            cbte.PtoVta = dto.PtoVta;
        }

        private static ComprobanteDto ToComprobanteDto(Comprobante comprobante)
        {
            if (comprobante == null) return null;

            ComprobanteDto dto = new ComprobanteDto();            
            dto.Id = comprobante.Id;
            dto.CAE = comprobante.CAE;
            dto.ClienteId = comprobante.ClienteId;
            dto.ClienteNombre = comprobante.Cliente != null ? comprobante.Cliente.RazonSocial : string.Empty;
            dto.CAEFechaVencimiento = comprobante.CAEFechaVencimiento;
            dto.FechaDeCarga = comprobante.FechaDeCarga;
            dto.TipoComprobanteId = comprobante.TipoComprobanteId;
            dto.TipoComprobanteDescripcion = comprobante.TipoComprobante.Descripcion;
            dto.CbteDesde = comprobante.CbteDesde;
            dto.CbteHasta = comprobante.CbteHasta;
            dto.CbteFecha = comprobante.CbteFecha;
            dto.PtoVta = comprobante.PtoVta;
            //#TODO: logica para obtener comprobante de acuerdo al nro.
            // Recordar lo de Factura B que se puede poner un rango.
            ArchivoAsociado archivoAsociado = comprobante.ArchivoAsociadoes.FirstOrDefault();
            if (archivoAsociado != null)
            {
                dto.PathFile = comprobante.ArchivoAsociadoes.First().PathArchivo;
                VisualizacionComprobante visualizacionCbte = archivoAsociado.VisualizacionComprobantes.FirstOrDefault();
                if (visualizacionCbte != null)
                {
                    dto.DireccionIp = visualizacionCbte.DireccionIP;
                    dto.FechaVisualizacion = visualizacionCbte.Fecha;
                }
            }
            
            return dto;
        }        

        private static List<ComprobanteDto> ToComprobanteDtoList(List<Comprobante> comprobanteList)
        {
            List<ComprobanteDto> dtoList = new List<ComprobanteDto>();
            if (comprobanteList.Count > 0)
            {
                foreach (var comprobante in comprobanteList)
                {
                    dtoList.Add(ToComprobanteDto(comprobante));
                }
            }
            return dtoList;
        }

        #endregion [Comprobante]

        #region [TipoComprobante]

        private static TipoComprobanteDto ToTipoComprobanteDto(TipoComprobante tipoComprobante)
        {
            if (tipoComprobante == null) return null;

            TipoComprobanteDto dto = new TipoComprobanteDto();
            dto.Id = tipoComprobante.Id;
            dto.Descripcion = tipoComprobante.Descripcion;
            dto.CodigoAfip = tipoComprobante.CodigoAfip.HasValue ? tipoComprobante.CodigoAfip.Value : (int?)null;
            dto.Codigo = tipoComprobante.Codigo;

            return dto;
        }

        private static List<TipoComprobanteDto> ToTipoComprobanteDtoList(List<TipoComprobante> tiposComprobantesList)
        {
            List<TipoComprobanteDto> dtoList = new List<TipoComprobanteDto>();
            if (tiposComprobantesList.Count > 0)
            {
                foreach (var tipoComprobante in tiposComprobantesList)
                {
                    dtoList.Add(ToTipoComprobanteDto(tipoComprobante));
                }
            }
            return dtoList;
        }

        #endregion [TipoComprobante]

        #region [Estado]

        //private static EstadoComprobanteDto ToEstadoComprobanteDto(EstadoComprobante estado)
        //{
        //    if (estado == null) return null;

        //    EstadoComprobanteDto dto = new EstadoComprobanteDto();

        //    dto.Id = estado.Id;
        //    dto.Codigo = estado.Codigo;
        //    dto.Descripcion = estado.Descripcion;

        //    return dto;
        //}

        #endregion [Estado]

        #region [Tipo Contrato]

        private static TipoContratoDto ToTipoContratoDto(TipoContrato tipoContrato)
        {
            if (tipoContrato == null) return null;

            TipoContratoDto dto = new TipoContratoDto();
            dto.Id = tipoContrato.Id;
            dto.Codigo = tipoContrato.Codigo;
            dto.Descripcion = tipoContrato.Descripcion;
            

            return dto;
        }

        private static List<TipoContratoDto> ToTipoContratoDtoList(List<TipoContrato> tiposContratoList)
        {
            List<TipoContratoDto> dtoList = new List<TipoContratoDto>();
            if (tiposContratoList.Count > 0)
            {
                foreach (var tipoContrato in tiposContratoList)
                {
                    dtoList.Add(ToTipoContratoDto(tipoContrato));
                }
            }
            return dtoList;
        }

        #endregion [Tipo Contrato]

        #endregion [Conversion]
    }
}
