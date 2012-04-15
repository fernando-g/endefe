using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacturaElectronica.Common.Contracts
{
    [Serializable]
    public class LogCriteria
    {
        public long CorridaId { get; set; }

        public long? LogCorridaId { get; set; }
    }
}
