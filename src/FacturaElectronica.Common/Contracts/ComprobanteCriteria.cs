using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FacturaElectronica.Common.Contracts
{
    [Serializable]
    [DataContract]
    public class ComprobanteCriteria
    {
        [DataMember]
        public DateTime? FechaVencDesde { get; set; }

        [DataMember]
        public DateTime? FechaVencHasta { get; set; }

        [DataMember]
        public DateTime? FechaDeCargaDesde { get; set; }

        [DataMember]
        public DateTime? FechaDeCargaHasta { get; set; }

        [DataMember]
        public int? TipoComprobanteId { get; set; }

        [DataMember]
        public long? NroComprobante { get; set; }

        [DataMember]
        public int? TipoContratoId { get; set; }

        [DataMember]
        public int? MesFacturacion { get; set; }

        [DataMember]
        public int? AnioFacturacion { get; set; }

        [DataMember]
        public string RazonSocial { get; set; }

        [DataMember]
        public long? ClienteId { get; set; }

        public bool DocumentosVencidos { get; set; }
    }
}
