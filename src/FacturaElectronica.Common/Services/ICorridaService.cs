using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Contracts;
using FacturaElectronica.Afip.Ws.Wsfe;

namespace FacturaElectronica.Common.Services
{
    public interface ICorridaService
    {
        CorridaAutorizacionDto CrearNuevaCorrida(string nombreDeArchivo);

        List<CorridaAutorizacionDto> ObtenerCorridas(long? corridaId, DateTime fechaDesde, DateTime fechaHasta);
        
        CorridaAutorizacionDto ProcesarCorrida(CorridaAutorizacionDto corridaDto, FECAEResponse feCAEResponse);

        void Log(long corridaId, string mensaje, string detalle);
       
        List<LogCorridaDto> ConsultarLog(long corridaId, DateTime fecha);
    }
}
