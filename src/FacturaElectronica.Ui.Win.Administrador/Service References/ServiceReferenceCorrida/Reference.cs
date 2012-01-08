﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FacturaElectronica.Ui.Win.Administrador.ServiceReferenceCorrida {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReferenceCorrida.IProcesoCorridaService")]
    public interface IProcesoCorridaService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProcesoCorridaService/CrearNuevaCorrida", ReplyAction="http://tempuri.org/IProcesoCorridaService/CrearNuevaCorridaResponse")]
        FacturaElectronica.Common.Contracts.CorridaAutorizacionDto CrearNuevaCorrida(string nombreDeArchivo);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IProcesoCorridaService/EjecutarCorrida")]
        void EjecutarCorrida(long corridaId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProcesoCorridaService/ObtenerCorridas", ReplyAction="http://tempuri.org/IProcesoCorridaService/ObtenerCorridasResponse")]
        System.Collections.Generic.List<FacturaElectronica.Common.Contracts.CorridaAutorizacionDto> ObtenerCorridas(FacturaElectronica.Common.Contracts.Search.CorridaSearch search);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProcesoCorridaService/ConsultarLog", ReplyAction="http://tempuri.org/IProcesoCorridaService/ConsultarLogResponse")]
        System.Collections.Generic.List<FacturaElectronica.Common.Contracts.LogCorridaDto> ConsultarLog(long corridaId, System.Nullable<System.DateTime> fecha);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IProcesoCorridaServiceChannel : FacturaElectronica.Ui.Win.Administrador.ServiceReferenceCorrida.IProcesoCorridaService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ProcesoCorridaServiceClient : System.ServiceModel.ClientBase<FacturaElectronica.Ui.Win.Administrador.ServiceReferenceCorrida.IProcesoCorridaService>, FacturaElectronica.Ui.Win.Administrador.ServiceReferenceCorrida.IProcesoCorridaService {
        
        public ProcesoCorridaServiceClient() {
        }
        
        public ProcesoCorridaServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ProcesoCorridaServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProcesoCorridaServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProcesoCorridaServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public FacturaElectronica.Common.Contracts.CorridaAutorizacionDto CrearNuevaCorrida(string nombreDeArchivo) {
            return base.Channel.CrearNuevaCorrida(nombreDeArchivo);
        }
        
        public void EjecutarCorrida(long corridaId) {
            base.Channel.EjecutarCorrida(corridaId);
        }
        
        public System.Collections.Generic.List<FacturaElectronica.Common.Contracts.CorridaAutorizacionDto> ObtenerCorridas(FacturaElectronica.Common.Contracts.Search.CorridaSearch search) {
            return base.Channel.ObtenerCorridas(search);
        }
        
        public System.Collections.Generic.List<FacturaElectronica.Common.Contracts.LogCorridaDto> ConsultarLog(long corridaId, System.Nullable<System.DateTime> fecha) {
            return base.Channel.ConsultarLog(corridaId, fecha);
        }
    }
}
