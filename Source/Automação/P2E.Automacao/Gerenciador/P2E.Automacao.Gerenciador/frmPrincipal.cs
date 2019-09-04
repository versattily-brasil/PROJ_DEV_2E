using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace P2E.Automacao.Gerenciador
{
    public partial class frmPrincipal : Form
    {
        List<Thread> _robosExecutados;

        public frmPrincipal()
        {
            InitializeComponent();


            var lista = new List<Robo>();
            lista.Add(new Robo() { Id = 1, Nome = "Acompanhar Despachos" });
            lista.Add(new Robo() { Id = 2, Nome = "Baixar Extratos" });
            lista.Add(new Robo() { Id = 3, Nome = "Enviar DAI" });
            lista.Add(new Robo() { Id = 4, Nome = "Enviar PLI" });
            lista.Add(new Robo() { Id = 5, Nome = "Tomar Ciência" });

            _robosExecutados = new List<Thread>();
            roboBindingSource.DataSource = lista;
        }

        private void Btnexecutar_Click(object sender, EventArgs e)
        {
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 2)
            {
                var selecionado = dataGridView1.Rows[e.RowIndex];

                switch (selecionado.Cells[0].Value)
                {
                    case 1:
                        var acompanhaDespacho = new P2E.Automacao.AcompanharDespachos.Lib.Work();
                        ThreadPool.QueueUserWorkItem(new WaitCallback(acompanhaDespacho.Executar), "Executar");
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        var enviarPLI = new P2E.Automacao.Processos.EnvioPLI.Lib.Work();
                        ThreadPool.QueueUserWorkItem(new WaitCallback(enviarPLI.Executar), "Executar");
                        break;
                    case 5:
                        break;
                    default:
                        break;
                }

                BindingSource bs = new BindingSource();

                bs.DataSource = _robosExecutados;
                gvEmExecucao.DataSource = bs;
            }
        }
    }

    public class Robo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Selecionado { get; set; }
    }

}
