using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Contracts;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Common.Contracts.Search;
using FacturaElectronica.Data;
using Web.Framework.Mapper;
using System.Configuration;
using System.IO;
using FacturaElectronica.Common.Constants;

namespace FacturaElectronica.Business.Services
{
    public class SubidaArchivoService : ISubidaArchivoService
    {
        public CorridaSubidaArchivoDto CrearNuevaCorrida()
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                CorridaSubidaArchivo corrida = new CorridaSubidaArchivo();
                corrida.FechaProceso = DateTime.Now;
                corrida.Procesada = null;
                ctx.CorridaSubidaArchivoes.AddObject(corrida);
                ctx.SaveChanges();
                CorridaSubidaArchivoDto corridaDto = new CorridaSubidaArchivoDto();

                EntityMapper.Map(corrida, corridaDto);

                return corridaDto;
            }
        }

        public void EjecutarCorrida(long corridaId, List<string> files)
        {
            // Proceso los archivos en la corrida y marcos cuales están ok y cuales no
            using (var ctx = new FacturaElectronicaEntities())
            {
                CorridaSubidaArchivo dbCorrida = ctx.CorridaSubidaArchivoes.Where(c => c.Id == corridaId).SingleOrDefault();
                if (dbCorrida != null)
                {
                    try
                    {
                        // Armo los path destinos Ok y con errores
                        string fileDestinationPathOk = ConfigurationManager.AppSettings["PathDestinoArchivosFacturaConCAE"];
                        fileDestinationPathOk = Path.Combine(fileDestinationPathOk, corridaId.ToString());
                        string fileDestinationPathNoOk = fileDestinationPathOk = Path.Combine(fileDestinationPathOk, "ConErrores"); ;
                        fileDestinationPathOk = Path.Combine(fileDestinationPathOk, "OK");
                        if (!Directory.Exists(fileDestinationPathOk))
                        {
                            Directory.CreateDirectory(fileDestinationPathOk);
                        }

                        if (!Directory.Exists(fileDestinationPathNoOk))
                        {
                            Directory.CreateDirectory(fileDestinationPathNoOk);
                        }

                        // Una vez que tengo los paths, cargo los archivos
                        foreach (string filePath in files)
                        {
                            try
                            {
                                string fileName = Path.GetFileName(filePath);
                                GenerarLog(dbCorrida, string.Format("Procesando Archivo: {0}", fileName));

                                if (File.Exists(filePath))
                                {
                                    ArmarArchivoAsociado(ctx, dbCorrida, filePath, fileDestinationPathOk, fileDestinationPathNoOk);
                                }
                                else
                                {
                                    GenerarLog(dbCorrida, string.Format("No Existe el Archivo: {0}", fileName));
                                }

                                GenerarLog(dbCorrida, string.Format("Fin Procesamiento Archivo: {0}", fileName));
                            }
                            catch (Exception ex)
                            {
                                GenerarLog(dbCorrida, ex.Message);
                            }
                        }

                        dbCorrida.Procesada = true;
                        ctx.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        GenerarLog(dbCorrida, ex.Message);
                    }
                }
            }

            // Al final, envío emails a los clientes informando que ya están listos X cantidad de facturas para
            // ser visualizadas
        }

        private const int arch_cuit = 0;
        private const int arch_tipodocumento = 1;
        private const int arch_nrocomprobante = 2;
        private const int arch_ptovta = 3;
        private const int arch_periodofacturacion = 4;
        private const int arch_fechavencimiento = 5;
        private const int arch_tipoContrato = 6;
        private const int arch_montototal = 7;

        private void ArmarArchivoAsociado(FacturaElectronicaEntities ctx, CorridaSubidaArchivo dbCorrida, string filePath, string fileDestinationPathOk, string fileDestinationPathNoOk)
        {
            string fileName = Path.GetFileName(filePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string[] filePartes = fileNameWithoutExtension.Split('_');

            StringBuilder mensajeError = new StringBuilder();
            string errorStr = string.Empty;
            if (filePartes.Length != 8)
            {
                errorStr = "No se pudo interpretar el nombre del archivo " + fileNameWithoutExtension;
                mensajeError.AppendLine(errorStr);
                GenerarLog(dbCorrida, errorStr);
            }
            else
            {
                bool procesarArchivo = true;
                long cuit;
                if (!Int64.TryParse(filePartes[arch_cuit], out cuit))
                {
                    errorStr = "No se pudo interpretar el CUIT " + filePartes[arch_cuit];
                    mensajeError.AppendLine(errorStr);
                    GenerarLog(dbCorrida, errorStr);
                    procesarArchivo = false;
                }

                string tipoComprobante = filePartes[arch_tipodocumento];
                long? tipoComprobanteId = null;
                TipoComprobante tipoComprobanteObj = null;
                if (!String.IsNullOrEmpty(tipoComprobante))
                {
                    tipoComprobanteObj = GetTipoComprobante(ctx).Where(t => t.Codigo == tipoComprobante).SingleOrDefault();
                    tipoComprobanteId = tipoComprobanteObj.Id;
                    if (!tipoComprobanteId.HasValue)
                    {
                        errorStr = "No se pudo interpretar el Tipo de Comprobante " + tipoComprobante;
                        mensajeError.AppendLine(errorStr);
                        GenerarLog(dbCorrida, errorStr);
                        procesarArchivo = false;
                    }
                }
                else
                {
                    errorStr = "No se pudo interpretar el Tipo de Comprobante " + tipoComprobante;
                    mensajeError.AppendLine(errorStr);
                    GenerarLog(dbCorrida, errorStr);
                    procesarArchivo = false;
                }

                string nroComprobanteStr = filePartes[arch_nrocomprobante];
                long nroComprobante;
                if (!Int64.TryParse(nroComprobanteStr, out nroComprobante))
                {
                    errorStr = "No se pudo interpretar el Nro de comprobante " + nroComprobanteStr;
                    mensajeError.AppendLine(errorStr);
                    GenerarLog(dbCorrida, errorStr);                  
                    procesarArchivo = false;
                }

                string ptovtaStr = filePartes[arch_ptovta];
                long ptovta;
                if (!Int64.TryParse(ptovtaStr, out ptovta))
                {
                    errorStr = "No se pudo interpretar el Punto de venta " + ptovtaStr;
                    mensajeError.AppendLine(errorStr);
                    GenerarLog(dbCorrida, errorStr);                    
                    procesarArchivo = false;
                }

                string periodoFactuStr = filePartes[arch_periodofacturacion];
                string periodoFactuAnioStr = periodoFactuStr.Substring(0, 4);
                string periodoFactuMesStr = periodoFactuStr.Substring(4, 2);
                int periodoFacturacionAnio = 0;
                int periodoFacturacionMes = 0;
                if (!Int32.TryParse(periodoFactuAnioStr, out periodoFacturacionAnio) || !Int32.TryParse(periodoFactuMesStr, out periodoFacturacionMes))
                {
                    errorStr = "No se pudo interpretar el Periodo de Facturacion " + periodoFactuStr;
                    mensajeError.AppendLine(errorStr);
                    GenerarLog(dbCorrida, errorStr);                         
                    procesarArchivo = false;
                }

                string vencimientoFactuStr = filePartes[arch_fechavencimiento];
                string vencimientoFactuAnioStr = vencimientoFactuStr.Substring(0, 4);
                string vencimientoFactuMesStr = vencimientoFactuStr.Substring(4, 2);
                string vencimientoFactuDiaStr = vencimientoFactuStr.Substring(6, 2);
                bool tieneFechaVencimiento = false;
                DateTime fechaDeVencimiento = DateTime.MinValue;
                int diasDeVencimiento = 0;
                if (vencimientoFactuAnioStr == "9999")
                {
                    tieneFechaVencimiento = false;
                    if (!Int32.TryParse(vencimientoFactuDiaStr, out diasDeVencimiento))
                    {
                        errorStr = "No se pudo interpretar la fecha de vencimiento " + vencimientoFactuStr;
                        mensajeError.AppendLine(errorStr);
                        GenerarLog(dbCorrida, errorStr);                       
                        procesarArchivo = false;
                    }
                }
                else
                {
                    tieneFechaVencimiento = true;
                    if (!DateTime.TryParseExact(vencimientoFactuStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out fechaDeVencimiento))
                    {
                        errorStr = "No se pudo interpretar la fecha de vencimiento " + vencimientoFactuStr;
                        mensajeError.AppendLine(errorStr);
                        GenerarLog(dbCorrida, errorStr);                         
                        procesarArchivo = false;
                    }
                }

                string montoTotalStr = filePartes[arch_montototal];
                decimal montoTotal;
                if (!Decimal.TryParse(montoTotalStr, out montoTotal))
                {
                    errorStr =  "No se pudo interpretar el MontoTotal " + montoTotalStr;
                    mensajeError.AppendLine(errorStr);
                    GenerarLog(dbCorrida, errorStr);                     
                    procesarArchivo = false;
                }

                string tipoContratoStr = filePartes[arch_tipoContrato];                
                TipoContrato tipoContrato = null;
                if (!String.IsNullOrEmpty(tipoContratoStr))
                {
                    tipoContrato = GetTipoContrato(ctx).Where(t => t.Codigo == tipoContratoStr).SingleOrDefault();

                    if (tipoContrato == null)
                    {
                        errorStr = "No se pudo interpretar el Tipo de Contrato " + tipoContratoStr;
                        mensajeError.AppendLine(errorStr);
                        GenerarLog(dbCorrida, errorStr);
                        procesarArchivo = false;
                    }
                }
                else
                {
                    errorStr = "No se pudo interpretar el Tipo de Contrato " + tipoContratoStr;
                    mensajeError.AppendLine(errorStr);
                    GenerarLog(dbCorrida, errorStr);
                    procesarArchivo = false;
                }

                CorridaSubidaArchivoDetalle detalle = new CorridaSubidaArchivoDetalle();
                if (procesarArchivo)
                {
                    // obtengo el cliente
                    var dbCliente = ctx.Clientes.Where(c => c.CUIT == cuit).SingleOrDefault();
                    if (dbCliente == null)
                    {
                        errorStr = "No se encuentra el cliente " + cuit.ToString();
                        mensajeError.AppendLine(errorStr);
                        GenerarLog(dbCorrida, errorStr);                         
                    }
                    else
                    {
                        // Obtengo el comprobante al que hay que asociarle el archivo
                        var dbComprobante = ctx.Comprobantes.Where(c => c.TipoComprobanteId == tipoComprobanteId.Value && c.PtoVta == ptovta && c.CbteDesde <= nroComprobante && c.CbteHasta >= nroComprobante).First();
                        if (dbComprobante == null)
                        {
                            // No se puede procesar
                            errorStr = string.Format("No se encuentra el comprobante asociado. Nro {0}, Tipo {1}, PtoVta: {2}", nroComprobante, tipoComprobanteObj.Descripcion, ptovta);
                            mensajeError.AppendLine(errorStr);
                            GenerarLog(dbCorrida, errorStr);

                            // Lo copio a la carpeta de fallidos y registro el estado en el documento
                            File.Move(filePath, fileDestinationPathNoOk);
                            detalle.ProcesadoOK = false;
                        }
                        else
                        {

                            // Lo copio a la carpeta de ok y lo guardo en la base
                            string destPath = Path.Combine(fileDestinationPathOk, fileName);
                            detalle.ProcesadoOK = true;
                            File.Move(filePath, destPath);

                            // Puede ser que el archivo asociado ya exista y lo tengo que actualizar
                            ArchivoAsociado archivoAsociado;
                            archivoAsociado = ctx.ArchivoAsociadoes.Where(a => a.ComprobanteId == dbComprobante.Id && a.NombreArchivo == fileName).SingleOrDefault();
                            if (archivoAsociado == null)
                            {
                                archivoAsociado = new ArchivoAsociado();
                                dbComprobante.ArchivoAsociadoes.Add(archivoAsociado);
                            }

                            archivoAsociado.NombreArchivo = fileName;
                            archivoAsociado.PathArchivo = destPath;
                            archivoAsociado.NroComprobante = nroComprobante;
                            archivoAsociado.TipoContratoId = tipoContrato.Id;
                            if (tieneFechaVencimiento)
                            {
                                archivoAsociado.FechaVencimiento = fechaDeVencimiento;
                            }
                            else
                            {
                                archivoAsociado.DiasVencimiento = diasDeVencimiento;
                            }

                            archivoAsociado.FechaDeCarga = DateTime.Now;
                            archivoAsociado.MesFacturacion = periodoFacturacionMes;
                            archivoAsociado.AnioFacturacion = periodoFacturacionAnio;
                            archivoAsociado.EstadoId = GetEstadoArchivoAsociado(ctx).Where(c => c.Codigo == CodigosEstadoArchivoAsociado.NoVisualizado).Select(e => e.Id).Single();
                            archivoAsociado.MontoTotal = montoTotal;                            
                        }
                    }
                }
                
                detalle.NombreArchivo = fileName;               
                detalle.Mensaje = errorStr.ToString();
                dbCorrida.CorridaSubidaArchivoDetalles.Add(detalle);
            }
        }

        private List<TipoComprobante> tiposComprobante;
        private List<TipoComprobante> GetTipoComprobante(FacturaElectronicaEntities ctx)
        {
            if (tiposComprobante == null)
            {
                tiposComprobante = ctx.TipoComprobantes.ToList();
            }

            return tiposComprobante;
        }

        private List<TipoContrato> tiposContrato;
        private List<TipoContrato> GetTipoContrato(FacturaElectronicaEntities ctx)
        {
            if (tiposContrato == null)
            {
                tiposContrato = ctx.TipoContratoes.ToList();
            }

            return tiposContrato;
        }

        public List<EstadoArchivoAsociado> estadosArchivoAsociado;
        private List<EstadoArchivoAsociado> GetEstadoArchivoAsociado(FacturaElectronicaEntities ctx)
        {
            if (estadosArchivoAsociado == null)
            {
                estadosArchivoAsociado = ctx.EstadoArchivoAsociadoes.ToList();
            }

            return estadosArchivoAsociado;
        }

        private void GenerarLog(CorridaSubidaArchivo dbCorrida, string mensaje)
        {
            CorridaSubidaArchivoLog log = new CorridaSubidaArchivoLog();
            log.Mensaje = mensaje;
            log.Fecha = DateTime.Now;
            dbCorrida.CorridaSubidaArchivoLogs.Add(log);
        }

        /// <summary>
        /// Obtiene las corridas de subida de archivo
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<CorridaSubidaArchivoDto> ObtenerCorridas(CorridaSubidaArchivoSearch search)
        {
            return null;
        }
    }
}
