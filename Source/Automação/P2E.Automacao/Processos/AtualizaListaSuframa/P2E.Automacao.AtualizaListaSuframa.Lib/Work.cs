using Newtonsoft.Json;
using P2E.Automacao.Entidades;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace P2E.Automacao.Processos.AtualizaListaSuframa.Lib
{
    public class Work
    {
        #region Variaveis Estáticas

        public string _urlSite = @"ftp://ftp2.suframa.gov.br/projetos/pli_suframa.exe";

        private string _urlApiBase;
        private List<DetalheNCM> registros;
        private string arquivoPath = "";
        #endregion

        public Work()
        {
            Console.WriteLine("#################  INICIALIZANDO - ATUALIZAÇÃO DA LISTAGEM PADRÃO DA SUFRAMA  ################# ");
            _urlApiBase = System.Configuration.ConfigurationSettings.AppSettings["ApiBaseUrl"];
        }

        public async Task ExecutarAsync()
        {
            await ExtraiBancoAcessAsync();
        }

        private async Task ExtraiBancoAcessAsync()
        {
            Console.WriteLine("Fazendo Download do Banco Acess");
            var aux = BaixarArquivoFTP(_urlSite);

            Console.WriteLine("Executando arquivo .exe");
            aux = ExecutaExe(arquivoPath);

            Console.WriteLine("Conectando ao Banco Acess");
            ConnectionDBAcess(@"C:\Rotinas_Automaticas\Listagem_Insumos\pli_suframa.mdb");

            Console.WriteLine("Robô Concluído !");
            //Console.ReadKey();
        }

        private static bool ExecutaExe(string path)
        {
            Process myProcess = new Process();

            try
            {
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = path;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                myProcess.Start();

                Thread.Sleep(5000);
                SendKeys.SendWait("{ENTER}");
                Thread.Sleep(3000);
                SendKeys.SendWait("{ENTER}");
                Thread.Sleep(3000);
                SendKeys.SendWait("{ENTER}");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool BaixarArquivoFTP(string url)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.UseBinary = true;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    var horaData = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");

                    //FUTURAMENTE ESSE CAMINHO SERÁ CONFIGURADO EM UMA TABELA
                    if (!System.IO.Directory.Exists(@"C:\Versatilly\"))
                    {
                        System.IO.Directory.CreateDirectory(@"C:\Versatilly\");
                    }

                    arquivoPath = Path.Combine("C:\\Versatilly\\", horaData + "-BancoDadosAcess.exe");

                    using (Stream rs = response.GetResponseStream())
                    {
                        using (FileStream ws = new FileStream(arquivoPath, FileMode.Create))
                        {
                            byte[] buffer = new byte[2048];
                            int bytesRead = rs.Read(buffer, 0, buffer.Length);
                            while (bytesRead > 0)
                            {
                                ws.Write(buffer, 0, bytesRead);
                                bytesRead = rs.Read(buffer, 0, buffer.Length);
                            }
                        }
                    }
                }

                Console.WriteLine("Download Concluido");

                return true;
            }
            catch
            {
                Console.WriteLine("Erro no Download");

                return false;

            }
        }

        public async Task ConnectionDBAcess(string path)
        {
            DetalheNCM detalheNCM = new DetalheNCM();

            //cria a conexão com o banco de dados
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path);

            //cria o objeto command and armazena a consulta SQL
            OleDbCommand aCommand = new OleDbCommand("Select * From SUF_NCM", aConnection);
            try
            {
                aConnection.Open();
                //cria o objeto datareader para fazer a conexao com a tabela
                OleDbDataReader aReader = aCommand.ExecuteReader();

                Console.WriteLine("Limpando tabela NCM...");
                await DeleteDetalheNCM(detalheNCM);

                Console.WriteLine("Inserindo Dados....");
                //Faz a interação com o banco de dados lendo os dados da tabela
                while (aReader.Read())
                {
                    Console.WriteLine("Codigo: " + aReader.GetString(0) + " Detalhe: " + aReader.GetString(1) + " Descrição: " + aReader.GetString(2));

                    detalheNCM.TX_SFNCM_CODIGO = aReader.GetString(0);
                    detalheNCM.TX_SFNCM_DETALHE = aReader.GetString(1);
                    detalheNCM.TX_SFNCM_DESCRICAO = aReader.GetString(2);

                    await InsertDetalheNCM(detalheNCM, aReader.GetString(0));
                }

                Console.WriteLine("Insert Concluído...");

                //fecha o reader
                aReader.Close();
                //fecha a conexao
                aConnection.Close();

                //return true;
            }
            //Trata a exceção
            catch (OleDbException e)
            {
                Console.WriteLine("Error: {0}", e.Errors[0].Message);
                //return false;
            }
        }

        private async Task InsertDetalheNCM(DetalheNCM detalheNcm, string codigo)
        {
            try
            {
                HttpResponseMessage resultado;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_urlApiBase);
                    resultado = await client.PutAsJsonAsync($"imp/v1/ncm/{0}", detalheNcm);
                    resultado.EnsureSuccessStatusCode();

                    Console.WriteLine("Registro salvo com sucesso.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao atualizar o NCM nº {detalheNcm.TX_SFNCM_CODIGO}.");
            }
        }

        private async Task DeleteDetalheNCM(DetalheNCM detalheNcm)
        {
            try
            {
                string url = _urlApiBase + $"imp/v1/ncm";

                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(url+"/todos");
                    var aux = await result.Content.ReadAsStringAsync();
                    registros = JsonConvert.DeserializeObject<List<DetalheNCM>>(aux);

                    if (registros != null && registros.Any())
                    {
                        string responseBody = string.Empty;

                        foreach (var ncm in registros)
                        {
                            result = await client.DeleteAsync($"{url}/{ncm.CD_DET_NCM}");
                            responseBody = await result.Content.ReadAsStringAsync();
                            result.EnsureSuccessStatusCode();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao deletar tabela NCM");
            }
        }
    }
}
