using PracticaCore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Windows.Forms;

namespace PracticaCore.Repositories
#region CODIGO SQL
/*    CREATE PROCEDURE SP_CLIENTES
AS
    SELECT Empresa FROM clientes
GO

ALTER PROCEDURE SP_CLIENTES_DATOS
(@EMPRESA VARCHAR(MAX))
AS
    DECLARE @IDCLIENTE VARCHAR(MAX)


    SELECT @IDCLIENTE = CODIGOCLIENTE

    FROM CLIENTES WHERE EMPRESA = @EMPRESA


    SELECT*
    FROM CLIENTES
    WHERE CODIGOCLIENTE = @IDCLIENTE
GO

ALTER PROCEDURE SP_CLIENTES_PEDIDOS
(@EMPRESA VARCHAR(MAX))
AS
    DECLARE @IDCLIENTE VARCHAR(MAX)


    SELECT @IDCLIENTE = CODIGOCLIENTE

    FROM CLIENTES WHERE EMPRESA = @EMPRESA


    SELECT CODIGOPEDIDO FROM PEDIDOS

    WHERE CODIGOCLIENTE = @IDCLIENTE
GO

ALTER PROCEDURE SP_CLIENTES_PEDIDOS
(@EMPRESA VARCHAR(MAX))
AS
    SELECT CODIGOPEDIDO FROM PEDIDOS
    WHERE CODIGOCLIENTE = @IDCLIENTE
GO

ALTER PROCEDURE SP_CLIENTE_PEDIDO_DETALLE
(@IDPEDIDO VARCHAR(MAX))
AS

    SELECT* FROM PEDIDOS
    WHERE CODIGOPEDIDO = @IDPEDIDO
GO

CREATE PROCEDURE SP_CLIENTE_NUEVO_PEDIDO
(@CODIGOPEDIDO VARCHAR(MAX), @CODIGOCLIENTE VARCHAR(MAX),
@FECHAENTREGA DATETIME, @FORMAENVIO VARCHAR(MAX),
@IMPORTE INT)
AS
    INSERT INTO PEDIDOS

    VALUES(@CODIGOPEDIDO, @CODIGOCLIENTE, @FECHAENTREGA, @FORMAENVIO, @IMPORTE)
GO

ALTER PROCEDURE SP_CLIENTE_DELETE_PEDIDO
(@IDPEDIDO VARCHAR(MAX), @EMPRESA VARCHAR(MAX))
AS
    DECLARE @IDCLIENTE VARCHAR(MAX)


    SELECT @IDCLIENTE = CODIGOCLIENTE

    FROM CLIENTES WHERE EMPRESA = @EMPRESA


    DELETE FROM PEDIDOS
    WHERE CODIGOPEDIDO = @IDPEDIDO AND CODIGOCLIENTE = @IDCLIENTE
GO*/

#endregion
{
    public class RepositoryClientePedido
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;

        public RepositoryClientePedido()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=NETCORE;User ID=sa;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<string> GetClientes()
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTES";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<string> clientes = new List<string>();
            while (this.reader.Read())
            {
                clientes.Add(this.reader["Empresa"].ToString());
            }
            this.reader.Close();
            this.cn.Close();
            return clientes;
        }        
        public ResumenCliente GetCliente(string empresa)
        {
            SqlParameter pamEmpresa = new SqlParameter("@EMPRESA", empresa);
            this.com.Parameters.Add(pamEmpresa);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTES_DATOS";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            ResumenCliente resumen = new ResumenCliente();
            resumen.Empresa = empresa;
            while (this.reader.Read())
            {
                resumen.Contacto = this.reader["CONTACTO"].ToString();
                resumen.Cargo = this.reader["CARGO"].ToString();
                resumen.Ciudad = this.reader["CIUDAD"].ToString();
                resumen.Telefono = this.reader["TELEFONO"].ToString();
            }
            this.reader.Close();
            
            this.cn.Close();
            this.com.Parameters.Clear();
            return resumen;
        }        
        public List<string> GetPedidos(string empresa)
        {
            SqlParameter pamEmpresa = new SqlParameter("@EMPRESA", empresa);
            this.com.Parameters.Add(pamEmpresa);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTES_PEDIDOS";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<string> pedidos = new List<string>();
            while (this.reader.Read())
            {
                pedidos.Add(this.reader["CODIGOPEDIDO"].ToString());
            }
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return pedidos;
        }        
        public ResumenPedido GetPedido(string idPedido)
        {
            SqlParameter pamId = new SqlParameter("@IDPEDIDO", idPedido);
            this.com.Parameters.Add(pamId);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTE_PEDIDO_DETALLE";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            ResumenPedido resumen = new ResumenPedido();
            while (this.reader.Read())
            {
                resumen.CodigoPedido = idPedido;
                resumen.CodigoCliente = this.reader["CodigoCliente"].ToString();
                resumen.FechaEntrega = this.reader["FechaEntrega"].ToString();
                resumen.FormaEnvio = this.reader["FormaEnvio"].ToString();
                resumen.Importe = this.reader["Importe"].ToString();
            }
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return resumen;
        }

        public void DeletePedido(string idPedido, string idCliente)
        {
            SqlParameter pamId = new SqlParameter("@IDPEDIDO", idPedido);
            this.com.Parameters.Add(pamId); 
            SqlParameter pamCliente = new SqlParameter("@EMPRESA", idCliente);
            this.com.Parameters.Add(pamCliente);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTE_DELETE_PEDIDO";
            this.cn.Open();
            int resultado = this.com.ExecuteNonQuery();
            MessageBox.Show("Eliminadas "+resultado);
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
        
/*        public ResumenCliente GetClientePedido(string empresa)
        {
            SqlParameter pamEmpresa = new SqlParameter("@EMPRESA", empresa);
            SqlParameter pamContacto = new SqlParameter();
            pamContacto.Value = "a";
            pamContacto.SqlDbType = SqlDbType.NVarChar;
            pamContacto.ParameterName = "@CONTACTO";
            pamContacto.Direction = ParameterDirection.Output;
            SqlParameter pamCargo = new SqlParameter();
            pamCargo.Value = "a";
            pamCargo.ParameterName = "@CARGO";
            pamCargo.Direction = ParameterDirection.Output;
            SqlParameter pamCiudad = new SqlParameter();
            pamCiudad.Value = "a";
            pamCiudad.ParameterName = "@CIUDAD";
            pamCiudad.Direction = ParameterDirection.Output;
            SqlParameter pamTelf = new SqlParameter();
            pamTelf.Value = 0;
            pamTelf.ParameterName = "@TELEFONO";
            pamTelf.Direction = ParameterDirection.Output;
            this.com.Parameters.Add(pamEmpresa);
            this.com.Parameters.Add(pamContacto);
            this.com.Parameters.Add(pamCargo);
            this.com.Parameters.Add(pamCiudad);
            this.com.Parameters.Add(pamTelf);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTES_DATOS";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            ResumenCliente resumen = new ResumenCliente();
            resumen.Empresa = empresa;
            while (this.reader.Read())
            {
                resumen.Pedidos.Add(this.reader["CODIGOPEDIDO"].ToString());
            }
            this.reader.Close();
            resumen.Contacto = pamContacto.Value.ToString();
            resumen.Cargo = pamCargo.Value.ToString();
            resumen.Ciudad = pamCiudad
            resumen.Telefono = int.Parse(pamTelf.Value.ToString());
            this.cn.Close();
            this.com.Parameters.Clear();
            return resumen;
        }*/
    }
}
