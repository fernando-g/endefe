using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacturaElectronica.Common.Contracts
{
    [Serializable]
    public class ComprobanteDto
    {
        public long Id { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        public int TipoComprobanteId { get; set; }

        public string TipoComprobanteDescripcion { get; set; }

        public long NroComprobante { get; set; }

        public DateTime FechaDeCarga { get; set; }

        public long? ClienteId { get; set; }

        public string ClienteNombre { get; set; }

        public int EstadoId { get; set; }

        public string EstadoDescripcion { get; set; }

        public string DireccionIp { get; set; }

        public DateTime? FechaVisualizacion { get; set; }

        public string CAE { get; set; }

        public DateTime? CAEFechaVencimiento { get; set; }

        public string PathFile { get; set; }

        public long? CbteDesde { get; set; }
        
        public long? CbteHasta { get; set; }

        public DateTime? CbteFecha { get; set; }

        public int? PtoVta { get; set; }
    }
}
