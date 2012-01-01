using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Contracts;

namespace FacturaElectronica.Common.Services
{
    public interface IComprobanteService
    {
        ComprobanteDto ObtenerComprobantes(long comprobanteId);

        List<ComprobanteDto> ObtenerComprobantes(ComprobanteCriteria criteria);

        ComprobanteDto ObtenerUltimoComprobanteCargado(int ptoVta, int cbeTipo);

        TipoComprobanteDto ObtenerTipoComprobantePorCodigoAfip(int codigoAfip);

        List<TipoComprobanteDto> ObtenerTiposComprobantes();
    }
}
