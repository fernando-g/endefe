using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacturaElectronica.Common.Contracts
{
    public class TipoComprobanteDto
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public int? CodigoAfip { get; set; }

        public string Codigo { get; set; }
    }
}