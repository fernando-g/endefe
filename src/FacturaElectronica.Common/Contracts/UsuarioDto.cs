using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FacturaElectronica.Common.Contracts
{
    [Serializable]
    [DataContract]
    public class UsuarioDto
    {
        public UsuarioDto()
        {
            Roles = new List<int>();
        }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string NombreUsuario { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public long? ClienteId { get; set; }

        [DataMember]
        public List<int> Roles { get; set; }
    }
}
