using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Afip.Business.Wsfe;
using FacturaElectronica.Afip.Business.Wsaa;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using FacturaElectronica.Afip.Business.Helpers;
using FacturaElectronica.Afip.Ws.Wsfe;
using FacturaElectronica.Common.Services;

namespace FacturaElectronica.Afip.Business.Wsfe
{
    public class WsfeClient : IAfipWrapperService
    {
        private string idServicioNegocio;
        private string certSigner;
        private string cuit;
        private StoreName storeName;
        private StoreLocation storeLocation;

        public WsfeClient()
        {
            this.idServicioNegocio = ConfigurationManager.AppSettings["idServicioNegocio"];
            this.certSigner = ConfigurationManager.AppSettings["certSigner"];
            this.cuit = ConfigurationManager.AppSettings["cuit"];

            this.storeName = ConfigHelper.ObtenerStoreName();
            this.storeLocation = ConfigHelper.ObtenerStoreLocation();
        }

        public DummyResponse DummyResponse()
        {
            try
            {
                DummyResponse result;
                using (ServiceSoapClient client = new ServiceSoapClient()) 
                {
                    return result = client.FEDummy();
                }
            }

            catch (Exception excepcionAlInvocarWsaa)
            {
                throw new Exception("***Error INVOCANDO al servicio WSAA : " + excepcionAlInvocarWsaa.Message);
            }
        }

        #region [Maestros]

        public MonedaResponse GetTiposMonedas(FEAuthRequest feAuthRequest)
        {
            try
            {
                MonedaResponse result;
                using (ServiceSoapClient client = new ServiceSoapClient())
                {
                    return result = client.FEParamGetTiposMonedas(feAuthRequest);                    
                }
            }

            catch (Exception excepcionAlInvocarWsfe)
            {
                throw new Exception("***Error INVOCANDO al servicio WFSE : " + excepcionAlInvocarWsfe.Message);
            }
        }

        public FECotizacionResponse GetCotizacion(FEAuthRequest feAuthRequest, string MonId)
        {
            try
            {
                FECotizacionResponse result;
                using (ServiceSoapClient client = new ServiceSoapClient())
                {
                    return result = client.FEParamGetCotizacion(feAuthRequest, MonId);
                }
            }

            catch (Exception excepcionAlInvocarWsfe)
            {
                throw new Exception("***Error INVOCANDO al servicio WFSE : " + excepcionAlInvocarWsfe.Message);
            }
        }

        public CbteTipoResponse GetTiposCbte(FEAuthRequest feAuthRequest)
        {
            try
            {
                CbteTipoResponse result;
                using (ServiceSoapClient client = new ServiceSoapClient())
                {
                    return result = client.FEParamGetTiposCbte(feAuthRequest);
                }
            }

            catch (Exception excepcionAlInvocarWsfe)
            {
                throw new Exception("***Error INVOCANDO al servicio WFSE : " + excepcionAlInvocarWsfe.Message);
            }
        }

        public FETributoResponse GetTiposTributos(FEAuthRequest feAuthRequest)
        {
            try
            {
                FETributoResponse result;
                using (ServiceSoapClient client = new ServiceSoapClient())
                {
                    return result = client.FEParamGetTiposTributos(feAuthRequest);
                }
            }

            catch (Exception excepcionAlInvocarWsfe)
            {
                throw new Exception("***Error INVOCANDO al servicio WFSE : " + excepcionAlInvocarWsfe.Message);
            }
        }

        public IvaTipoResponse GetTiposIva(FEAuthRequest feAuthRequest)
        {
            try
            {
                IvaTipoResponse result;
                using (ServiceSoapClient client = new ServiceSoapClient())
                {
                    return result = client.FEParamGetTiposIva(feAuthRequest);
                }
            }

            catch (Exception excepcionAlInvocarWsfe)
            {
                throw new Exception("***Error INVOCANDO al servicio WFSE : " + excepcionAlInvocarWsfe.Message);
            }
        }

        public OpcionalTipoResponse GetTiposOpcional(FEAuthRequest feAuthRequest)
        {
            try
            {
                OpcionalTipoResponse result;
                using (ServiceSoapClient client = new ServiceSoapClient())
                {
                    return result = client.FEParamGetTiposOpcional(feAuthRequest);
                }
            }

            catch (Exception excepcionAlInvocarWsfe)
            {
                throw new Exception("***Error INVOCANDO al servicio WFSE : " + excepcionAlInvocarWsfe.Message);
            }
        }

        public FERegXReqResponse CompTotXRequest(FEAuthRequest feAuthRequest)
        {
            try
            {
                FERegXReqResponse result;
                using (ServiceSoapClient client = new ServiceSoapClient())
                {
                    return result = client.FECompTotXRequest(feAuthRequest);
                }
            }

            catch (Exception excepcionAlInvocarWsfe)
            {
                throw new Exception("***Error INVOCANDO al servicio WFSE : " + excepcionAlInvocarWsfe.Message);
            }
        }

        public DocTipoResponse GetTiposDoc(FEAuthRequest feAuthRequest)
        {
            try
            {
                DocTipoResponse result;
                using (ServiceSoapClient client = new ServiceSoapClient())
                {
                    return result = client.FEParamGetTiposDoc(feAuthRequest);
                }
            }

            catch (Exception excepcionAlInvocarWsfe)
            {
                throw new Exception("***Error INVOCANDO al servicio WFSE : " + excepcionAlInvocarWsfe.Message);
            }
        }

        public FERecuperaLastCbteResponse CompUltimoAutorizado(FEAuthRequest feAuthRequest, int ptoVta, int cbteTipo)
        {
            try
            {
                FERecuperaLastCbteResponse result;
                using (ServiceSoapClient client = new ServiceSoapClient())
                {
                    return result = client.FECompUltimoAutorizado(feAuthRequest, ptoVta, cbteTipo); 
                }
            }

            catch (Exception excepcionAlInvocarWsfe)
            {
                throw new Exception("***Error INVOCANDO al servicio WFSE : " + excepcionAlInvocarWsfe.Message);
            }
        }

        public FECompConsultaResponse CompConsultar(FEAuthRequest feAuthRequest, FECompConsultaReq feCompConsultaReq)
        {
            try
            {
                FECompConsultaResponse result;
                using (ServiceSoapClient client = new ServiceSoapClient())
                {
                    return result = client.FECompConsultar(feAuthRequest, feCompConsultaReq);
                }
            }

            catch (Exception excepcionAlInvocarWsfe)
            {
                throw new Exception("***Error INVOCANDO al servicio WFSE : " + excepcionAlInvocarWsfe.Message);
            }
        }

        public ConceptoTipoResponse GetTiposConcepto(FEAuthRequest feAuthRequest)
        {
            try
            {
                ConceptoTipoResponse result;
                using (ServiceSoapClient client = new ServiceSoapClient())
                {
                    return result = client.FEParamGetTiposConcepto(feAuthRequest);
                }
            }

            catch (Exception excepcionAlInvocarWsfe)
            {
                throw new Exception("***Error INVOCANDO al servicio WFSE : " + excepcionAlInvocarWsfe.Message);
            }
        }

        #endregion [Maestros]

        public FECAEResponse AutorizarComprobantes(FEAuthRequest feAuthRequest, FECAERequest feCAERequest)
        {
            try
            {
                FECAEResponse result;
                using (ServiceSoapClient client = new ServiceSoapClient())
                {
                    return result = client.FECAESolicitar(feAuthRequest,feCAERequest);
                }
            }

            catch (Exception ex)
            {
                throw new Exception("***Error INVOCANDO al servicio WSFE : " + ex.Message);
            }
        
        }
       
        #region [Ticket Autorizacion]

        public FEAuthRequest ObtenerTicket()
        {
            LoginTicket loginTicket = new LoginTicket();
            string ticketReponse = loginTicket.ObtenerLoginTicketResponse(this.idServicioNegocio, this.certSigner, false, this.storeName, this.storeLocation);

            FEAuthRequest feAuthRequest = new FEAuthRequest();
            feAuthRequest.Cuit = Convert.ToInt64(this.cuit);
            feAuthRequest.Sign = loginTicket.Sign;
            feAuthRequest.Token = loginTicket.Token;

            return feAuthRequest;
        }
        
        #endregion [Ticket Autorizacion]
    }
}
