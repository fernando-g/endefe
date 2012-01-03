using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Contracts;

namespace FacturaElectronica.Common.Services
{
    public interface IComprobanteService
    {
        ComprobanteDto ObtenerComprobante(long comprobanteId);

        List<ComprobanteArchivoAsociadoDto> ObtenerComprobantesPorCliente(ComprobanteCriteria criteria);

        List<ComprobanteArchivoAsociadoDto> ObtenerComprobantes(ComprobanteCriteria criteria);

        ComprobanteDto ObtenerUltimoComprobanteCargado(int ptoVta, int cbeTipo);

        TipoComprobanteDto ObtenerTipoComprobantePorCodigoAfip(int codigoAfip);

        List<TipoComprobanteDto> ObtenerTiposComprobantes();

        List<TipoContratoDto> ObtenerTiposContrato();

        List<int> ObtenerAniosFacturacion();

        void AgregarVisualizacion(VisualizacionComprobanteDto dto);

        void CambiarEstado(long archivoAsociadoId, string codigoEstado);
    }
}
