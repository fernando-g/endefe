using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacturaElectronica.Common.Contracts
{
    [Serializable]
    public class UsuarioDto
    {
        public long Id { get; set; }

        public string NombreUsuario { get; set; }

        public string Password { get; set; }

        public long? ClienteId { get; set; }

        public List<int> Roles { get; set; }
    }
}
