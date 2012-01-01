using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacturaElectronica.Common.Contracts
{
    [Serializable]
    public class ComprobanteCriteria
    {
        public DateTime? FechaVencDesde { get; set; }

        public DateTime? FechaVencHasta { get; set; }

        public DateTime? FechaDeCargaDesde { get; set; }

        public DateTime? FechaDeCargaHasta { get; set; }

        public int? TipoComprobanteId { get; set; }

        public long? NroComprobante { get; set; }

        public int? TipoContratoId { get; set; }

        public int? MesFacturacion { get; set; }

        public int? AnioFacturacion { get; set; }

        public string RazonSocial { get; set; }

        public long? ClienteId { get; set; }
    }
}
