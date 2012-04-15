using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Contracts;

namespace FacturaElectronica.Common.Services
{
    public interface IMensajeService 
    {
        MensajeDto CrearMensaje(MensajeDto mensajeDto);

        bool EliminarMensaje(long mensajeId);

        MensajeDto ObtenerMensaje(long mensajeId);

        List<MensajeDto> ObtenerMensajes(MensajeCriteria criteria);

        void MarcarComoLeido(long mensajeId, long clienteId);
    }
}
