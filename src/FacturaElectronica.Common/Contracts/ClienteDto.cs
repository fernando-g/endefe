using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacturaElectronica.Common.Contracts
{
    [Serializable]
    public class ClienteDto
    {
        public long Id { get; set; }

        public string RazonSocial { get; set; }

        public long CUIT { get; set; }

        public string NombreContacto { get; set; }

        public string ApellidoContacto { get; set; }

        public string EmailContacto { get; set; }

        public string NombreContactoSecundario { get; set; }

        public string ApellidoContactoSecundario { get; set; }

        public string EmailContactoSecundario { get; set; }

        public long UsuarioId { get; set; }

    }
}
