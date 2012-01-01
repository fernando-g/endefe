using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Common.Contracts;
using FacturaElectronica.Data;
using System.Data.Objects;

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

        public List<ComprobanteArchivoAsociadoDto> ObtenerComprobantes(ComprobanteCriteria criteria)
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                //IQueryable<Comprobante> query = ctx.Comprobantes;

                //if (criteria.FechaVencDesde.HasValue)
                //{
                //    query.Where(c => criteria.FechaVencDesde.Value <= c.FechaVencimiento);
                //}
                //if (criteria.FechaVencHasta.HasValue)
                //{
                //    query.Where(c => c.FechaVencimiento <= criteria.FechaVencHasta.Value);
                //}
                var query = from aa in ctx.ArchivoAsociadoes
                            join c in ctx.Comprobantes on aa.ComprobanteId equals c.Id
                            where (!criteria.FechaVencDesde.HasValue || criteria.FechaVencDesde.Value <= aa.FechaVencimiento)
                               && (!criteria.FechaVencHasta.HasValue || aa.FechaVencimiento <= criteria.FechaVencHasta.Value)
                               && (string.IsNullOrEmpty(criteria.RazonSocial) || c.Cliente.RazonSocial.Contains(criteria.RazonSocial))
                               && (!criteria.TipoComprobanteId.HasValue || c.TipoComprobanteId == criteria.TipoComprobanteId.Value)
                               && (!criteria.ClienteId.HasValue || c.ClienteId == criteria.ClienteId.Value)
                               && (!criteria.TipoContratoId.HasValue || aa.TipoContratoId == criteria.TipoContratoId.Value)
                            select new ComprobanteArchivoAsociadoDto() 
                            {
                                ArchivoAsociadoId = aa.Id,
                                CAE = c.CAE,
                                CAEFechaVencimiento = c.CAEFechaVencimiento,
                                ClienteId = c.ClienteId,
                                ClienteRazonSocial = c.Cliente.RazonSocial,
                                ComprobanteId = c.Id,
                                DireccionIp = aa.VisualizacionComprobantes.Last().DireccionIP,
                                EstadoDescripcion = "",
                                EstadoId = 0,
                                FechaDeCarga = aa.FechaDeCarga
                            };
                return null;
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

        public ComprobanteDto ObtenerComprobantes(long comprobanteId)
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
                return ToTipoComprobanteDtoList(ctx.TipoComprobantes.OrderBy(c => c.Descripcion).ToList());
            }
        
        }

        #endregion [Tipos Comprobante]

        #region [Estados]

        public EstadoComprobanteDto ObtenerEstado(string codigo)
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                return ToEstadoComprobanteDto(ctx.EstadoComprobantes.Where(e => e.Codigo == codigo).FirstOrDefault());
            }            
        }

        #endregion [Estados]

        #region [Conversion]

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
            dto.EstadoId = comprobante.EstadoId;
            dto.EstadoDescripcion = comprobante.EstadoComprobante.Descripcion;
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

        #region [Comprobante]

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

        #endregion [Comprobante]

        #region [TipoComprobante]

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

        private static EstadoComprobanteDto ToEstadoComprobanteDto(EstadoComprobante estado)
        {
            if (estado == null) return null;

            EstadoComprobanteDto dto = new EstadoComprobanteDto();

            dto.Id = estado.Id;
            dto.Codigo = estado.Codigo;
            dto.Descripcion = estado.Descripcion;

            return dto;
        }
        
        #endregion [TipoComprobante]

        #endregion [Conversion]
    }
}
