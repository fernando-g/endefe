using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacturaElectronica.Common.Contracts
{
    public class CorridaAutorizacionDto
    {
        public long Id { get; set; }

        public DateTime Fecha { get; set; }

        public string NombreDeArchivo { get; set; }

        public string PathArchivo { get; set; }

        public string TipoComprobante { get; set; }

        public List<DetalleComprobanteDto> ComprobantesAutorizados { get; set; }

        public List<DetalleComprobanteDto> ComprobantesConObservaciones { get; set; }

        public List<DetalleErrorDto> Errores { get; set; }

        public List<DetalleEventoDto> Eventos { get; set; }
    }
}
