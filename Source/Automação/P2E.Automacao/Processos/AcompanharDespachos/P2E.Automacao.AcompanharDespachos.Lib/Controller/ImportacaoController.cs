//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using static P2E.Automacao.AcompanharDespachos.Lib.Entities.Importacao;

//namespace P2E.Automacao.AcompanharDespachos.Lib.Controller
//{
//    public class ImportacaoController
//    {
//        public static async Task AtualizaStatus(TBImportacao import, string cd_imp)
//        {
//            try
//            {
//                using (var client = new HttpClient())
//                {
//                    client.BaseAddress = new Uri("http://localhost:7000/");

//                    var resultado = await client.PutAsJsonAsync($"imp/v1/tbimportacao/{cd_imp}", import);
//                    var responseBody = await resultado.Content.ReadAsStringAsync();
//                    resultado.EnsureSuccessStatusCode();
//                }
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }
//        }

//        public static async Task Carregar()
//        {
//            try
//            {
//                using (var client = new HttpClient())
//                {
//                    TBImportacao tbImportacao = null;

//                    client.BaseAddress = new Uri("http://localhost:7000/");
//                    var result = client.GetAsync($"imp/v1/tbimportacao/todos").Result;
//                    result.EnsureSuccessStatusCode();

//                    if (result.IsSuccessStatusCode)
//                    {
//                        var aux = await result.Content.ReadAsStringAsync();
//                        var importacao = JsonConvert.DeserializeObject<List<TBImportacao>>(aux);

//                        foreach (var item in importacao)
//                        {
//                            if (item.NR_NUM_DEC.ToString().Length == 10)
//                            {
//                               // Work.Acessar(item, item.NR_NUM_DEC.ToString(), item.CD_IMP.ToString());
//                            }
//                        }
//                    }
//                }

//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }
//        }
//    }
//}
