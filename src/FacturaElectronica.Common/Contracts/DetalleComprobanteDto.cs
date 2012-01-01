using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacturaElectronica.Common.Contracts
{
    public class DetalleComprobanteDto
    {
        public long Id { get; set; }
        public int Concepto { get; set; }
        public string ConceptoDesc { get; set; }
        public int DocTipo { get; set; }
        public string DocTipoDesc { get; set; }
        public long DocNro { get; set; }
        public long CbteDesde { get; set; }
        public long CbteHasta { get; set; }
        public DateTime CbteFecha { get; set; }
        public string CAE { get; set; }
        public DateTime CAEFechaVto { get; set; }

        public List<ObservacionComprobanteDto> Observaciones { get; set; }
    }
}
