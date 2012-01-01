using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacturaElectronica.Common.Contracts
{
    public class LogCorridaDto
    {
        public long Id { get; set; }

        public long CorridaId { get; set; }

        public DateTime Fecha { get; set; }

        public string Mensaje { get; set; }

    }
}
