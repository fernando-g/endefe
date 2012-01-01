using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacturaElectronica.Common.Contracts
{
    public class DetalleErrorDto
    {
        public long Id { get; set; }

        public int Codigo { get; set; }

        public string Mensaje { get; set; }

    }
}
