using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Common.Contracts;
using System.ServiceModel;
using FacturaElectronica.Business.Services;
using FacturaElectronica.Common.Contracts.Search;
using FacturaElectronica.Afip.Business;

namespace FacturaElectronica.Facade
{    
    public class ProcesoCorridaService : IProcesoCorridaService
    {        
        public CorridaAutorizacionDto CrearNuevaCorrida(string nombreDeArchivo)
        {
            CorridaService corridaSvc = new CorridaService();
            return corridaSvc.CrearNuevaCorrida(nombreDeArchivo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="corridaId"></param>
        /// <remarks>El metodo se ejecuta asincronicamente</remarks>
        public void EjecutarCorrida(long corridaId)
        {
            CorridaService corridaSvc = new CorridaService();

            var corridas = corridaSvc.ObtenerCorridas(new CorridaSearch() { CorridaId = corridaId });
            if (corridas.Count == 1)
            {
                CorridaAutorizacionDto corridaAutorizacionDto = corridas.Single();

                ProcesoAutorizador autorizador = new ProcesoAutorizador(true);
                autorizador.AutorizarComprobantes(corridaAutorizacionDto);
            }                        
        }

        public List<CorridaAutorizacionDto> ObtenerCorridas(CorridaSearch search)
        {
            CorridaService corridaSvc = new CorridaService();
            return corridaSvc.ObtenerCorridas(search);
        }

        public List<LogCorridaDto> ConsultarLog(long corridaId, DateTime fecha)
        {
            CorridaService corridaSvc = new CorridaService();
            return corridaSvc.ConsultarLog(corridaId, fecha);
        }
    }
}
