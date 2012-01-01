using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacturaElectronica.Common.Contracts
{
    public class ComprobanteArchivoAsociadoDto
    {
        public long ArchivoAsociadoId { get; set; }
        
        public long ComprobanteId { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        public int TipoComprobanteId { get; set; }

        public string TipoComprobanteDescripcion { get; set; }

        public long NroComprobante { get; set; }

        public DateTime FechaDeCarga { get; set; }

        public long? ClienteId { get; set; }

        public string ClienteRazonSocial { get; set; }

        public int EstadoId { get; set; }

        public string EstadoDescripcion { get; set; }

        public string DireccionIp { get; set; }

        public DateTime? FechaVisualizacion { get; set; }

        public string CAE { get; set; }

        public DateTime? CAEFechaVencimiento { get; set; }

        public string PathArchivo { get; set; }

        public int? PtoVta { get; set; }

        public int? TipoContratoId { get; set; }

        public string TipoContratoDescripcion { get; set; }

        public string PeriodoFacturacion { get; set; }
    }
}
