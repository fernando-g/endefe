using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacturaElectronica.Common.Contracts
{
    [Serializable]
    public class RolDto
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }
    }
}
