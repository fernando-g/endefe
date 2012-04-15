using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacturaElectronica.Common.Contracts
{
    [Serializable]
    public class MensajeCriteria
    {
        public long? ClienteId { get; set; }

        public DateTime? FechaDeCargaDesde { get; set; }

        public DateTime? FechaDeCargaHasta { get; set; }

        public string Asunto { get; set; }

        public string NombreDeArchivo { get; set; }

        public bool? Leido { get; set; }
    }
}
