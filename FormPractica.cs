using PracticaCore.Models;
using PracticaCore.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticaCore
{
    public partial class FormPractica : Form
    {
        RepositoryClientePedido repo;
        public FormPractica()
        {
            InitializeComponent();
            this.repo = new RepositoryClientePedido();
            this.GetClientes();
        }
        public void GetClientes()
        {
            this.cmbclientes.Items.Clear();
            List<string> clientes = this.repo.GetClientes();
            foreach (string cliente in clientes)
            {
                this.cmbclientes.Items.Add(cliente);
            }
        }
        public void GetPedidos()
        {
            this.lstpedidos.Items.Clear();
            string empresa = this.cmbclientes.SelectedItem.ToString();
            ResumenCliente resumen = this.repo.GetCliente(empresa);
            this.txtempresa.Text = resumen.Empresa;
            this.txtcontacto.Text = resumen.Contacto;
            this.txtcargo.Text = resumen.Cargo;
            this.txtciudad.Text = resumen.Ciudad;
            this.txttelefono.Text = resumen.Telefono.ToString();
            List<string> pedidos = this.repo.GetPedidos(empresa);
            foreach (string pedido in pedidos)
            {
                this.lstpedidos.Items.Add(pedido);
            }
        }
        public void GetPedido()
        {
            if (this.lstpedidos.SelectedIndex != -1)
            {
                string idPedido = this.lstpedidos.SelectedItem.ToString();
                ResumenPedido resumen = this.repo.GetPedido(idPedido);
                this.txtcodigopedido.Text = resumen.CodigoPedido;
                this.txtfechaentrega.Text = resumen.FechaEntrega;
                this.txtformaenvio.Text = resumen.FormaEnvio;
                this.txtimporte.Text = resumen.Importe;
            }
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetPedidos();
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetPedido();
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {

        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            string codigopedido = this.txtcodigopedido.Text.ToString();
            string codigousuario = this.cmbclientes.SelectedItem.ToString();
            this.repo.DeletePedido(codigopedido, codigousuario);
            this.GetPedidos();
        }
    }
}
