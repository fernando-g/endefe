using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Contracts;
using System.ServiceModel;

namespace FacturaElectronica.Common.Services
{
    [ServiceContract]
    public interface IComprobanteService
    {
        ComprobanteDto ObtenerComprobante(long comprobanteId);

        List<ComprobanteArchivoAsociadoDto> ObtenerComprobantesPorCliente(ComprobanteCriteria criteria, int cantUltimosCbtes);

        List<ComprobanteArchivoAsociadoDto> ObtenerComprobantes(ComprobanteCriteria criteria);

        ComprobanteDto ObtenerUltimoComprobanteCargado(int ptoVta, int cbeTipo);

        TipoComprobanteDto ObtenerTipoComprobantePorCodigoAfip(int codigoAfip);

        [OperationContract]
        List<TipoComprobanteDto> ObtenerTiposComprobantes();

        List<TipoContratoDto> ObtenerTiposContrato();

        List<int> ObtenerAniosFacturacion();

        void AgregarVisualizacion(VisualizacionComprobanteDto dto);

        void CambiarEstado(long archivoAsociadoId, string codigoEstado);

        EstadoArchivoAsociadoDto ObtenerEstado(string codigo);

        string ObtenerArchivo(long archivoId);
        
        string ObtenerArchivo(long archivoId, long clienteId);
    }
}
