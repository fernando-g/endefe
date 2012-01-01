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

        public int? TipoComprobanteId { get; set; }

        public long? NroComprobante { get; set; }

        public int? TipoContratoId { get; set; }

        public DateTime? MesFacturacion { get; set; }
    }
}
