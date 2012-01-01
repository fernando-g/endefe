﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Common.Contracts;
using System.Transactions;
using FacturaElectronica.Data;

namespace FacturaElectronica.Business.Services
{
    public class ClienteService : IClienteService
    {
        #region IClienteService Members

        public ClienteDto CrearCliente(ClienteDto clienteDto)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (var ctx = new FacturaElectronicaEntities())
                {
                    Cliente cliente = new Cliente();
                    ToCliente(clienteDto, cliente);
                    ctx.Clientes.AddObject(cliente);
                    ctx.SaveChanges();
                    ts.Complete();
                    clienteDto.Id = cliente.Id;
                }
                return clienteDto;

            }
        }

        public ClienteDto EditarCliente(ClienteDto clienteDto)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                using (var ctx = new FacturaElectronicaEntities())
                {
                    Cliente cliente = this.ObtenerCliente(ctx, clienteDto.Id);
                    ToCliente(clienteDto, cliente);
                    
                    ctx.SaveChanges();
                    ts.Complete();
                }

                return clienteDto;
            }
        }

        public bool EliminarCliente(long clienteId)
        {
            int result = 0;
            using (var ctx = new FacturaElectronicaEntities())
            {
                Cliente cliente = this.ObtenerCliente(ctx, clienteId);
                if(cliente.Usuarios.Count>0)
                    cliente.Usuarios.Clear();
                ctx.Clientes.DeleteObject(cliente);
                result = ctx.SaveChanges();
            }
            return result > 0;
        }

        public ClienteDto ObtenerCliente(long clienteId)
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                return ToClienteDto(this.ObtenerCliente(ctx, clienteId));
            }
        }

        public List<ClienteDto> ObtenerClientes(ClienteCriteria criteria)
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                IQueryable<Cliente> query = ctx.Clientes;

                if (!string.IsNullOrEmpty(criteria.RazonSocial))
                {
                    query.Where(c => c.RazonSocial.Contains(criteria.RazonSocial));
                }

                if (criteria.CUIT.HasValue)
                { 
                    query.Where(c => c.CUIT == criteria.CUIT.Value);
                }

                if (!string.IsNullOrEmpty(criteria.NombreContacto))
                {
                    query.Where(c => c.NombreContacto.Contains(criteria.NombreContacto));
                }

                if (!string.IsNullOrEmpty(criteria.ApellidoContacto))
                {
                    query.Where(c => c.NombreContacto.Contains(criteria.ApellidoContacto));
                }

                return ToClienteDtoList(query.ToList());
            }

        }

        private Cliente ObtenerCliente(FacturaElectronicaEntities ctx, long clienteId)
        {
            return ctx.Clientes.Where(c => c.Id == clienteId).SingleOrDefault();
        }

        public ClienteDto ObtenerClientePorCuit(long cuit)
        {
            using (var ctx = new FacturaElectronicaEntities())
            {
                return ToClienteDto(ctx.Clientes.Where(c => c.CUIT == cuit).SingleOrDefault());
            }
        }


        #endregion

        #region [Conversion]

        private static void ToCliente(ClienteDto clienteDto, Cliente cliente)
        {
            cliente.NombreContacto = clienteDto.NombreContacto;
            cliente.NombreContactoSecundario = clienteDto.NombreContactoSecundario;
            cliente.RazonSocial = clienteDto.RazonSocial;
            cliente.ApellidoContacto = clienteDto.ApellidoContacto;
            cliente.ApellidoContactoSecundario = clienteDto.ApellidoContactoSecundario;
            cliente.CUIT = clienteDto.CUIT;
            cliente.EmailContacto = clienteDto.EmailContacto;
            cliente.EmailContactoSecundario = clienteDto.EmailContactoSecundario;
        }

        private static ClienteDto ToClienteDto(Cliente cliente)
        {
            if (cliente == null) return null;

            ClienteDto clienteDto = new ClienteDto();
            clienteDto.Id = cliente.Id;
            clienteDto.NombreContacto = cliente.NombreContacto;
            clienteDto.NombreContactoSecundario = cliente.NombreContactoSecundario;
            clienteDto.RazonSocial = cliente.RazonSocial;
            clienteDto.ApellidoContacto = cliente.ApellidoContacto;
            clienteDto.ApellidoContactoSecundario = cliente.ApellidoContactoSecundario;
            clienteDto.CUIT = cliente.CUIT;
            clienteDto.EmailContacto = cliente.EmailContacto;
            clienteDto.EmailContactoSecundario = cliente.EmailContactoSecundario;

            Usuario usuario = cliente.Usuarios.FirstOrDefault();
            if(usuario != null)
            {
                clienteDto.UsuarioId = usuario.Id;
            }

            return clienteDto;
        }

        private static List<ClienteDto> ToClienteDtoList(List<Cliente> clienteList)
        {
            List<ClienteDto> clienteDtoList = new List<ClienteDto>();
            if (clienteList.Count > 0)
            {
                foreach (var cliente in clienteList)
                {
                    clienteDtoList.Add(ToClienteDto(cliente));
                }
            }
            return clienteDtoList;
        }
        
        #endregion [Conversion]
    }
}
