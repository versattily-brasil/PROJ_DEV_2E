using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

namespace P2E.Automacao.Shared.Extensions
{
    public class AzCaptcha
    {
        /// <summary>
        /// Converte uma imagem a partir de uma URL para uma string Base64
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ConverteImageParaBase64Url(string url)
        {
            //var credentials = new NetworkCredential(user, pw);
            Uri uri = new Uri(url, UriKind.Absolute);
            WebRequest req = WebRequest.Create(uri);
            Stream stream = req.GetResponse().GetResponseStream();
            Image img = Image.FromStream(stream);

            using (System.Drawing.Image image = img)
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
        }

        /// <summary>
        /// Cria uma sctring com o Form de requisição do AzCaptcha
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static String PegarHtmlForm(string Url) //htmlform
        {

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();

            return result;
        }

        /// <summary>
        /// Retorna o resultado da leitura de uma imagem Captcha
        /// </summary>
        /// <param name="base64string">String de uma imagem Base64</param>
        /// <param name="key">Chave do cadastro no AzCaptcha</param>
        /// <returns></returns>
        public static string ResultadoCaptcha(string base64string, string key)
        {
            //POST
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var request = (HttpWebRequest)WebRequest.Create("http://azcaptcha.com/in.php");

                var postData = "method=base64&key=" + key + "&body=" + WebUtility.UrlEncode(base64string);

                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (responseString.Contains("OK|"))
                {
                    //  GET
                    string captcha = "";

                    while (true)
                    {
                        captcha = PegarHtmlForm("http://azcaptcha.com/res.php?key=" + key + "&action=get&id=" + responseString.Split('|')[1]);
                        if (captcha == "CAPCHA_NOT_READY")
                        {
                            System.Threading.Thread.Sleep(5000);
                            continue;
                        }
                        if (captcha == "_")
                        {
                            System.Threading.Thread.Sleep(5000);
                            continue;
                        }

                        if (captcha.Contains("OK|"))
                        {
                            captcha = captcha.Replace("OK|", "");

                        }
                        return captcha;

                    }
                }
                return "";
            }
            catch (Exception e)
            {
                string tt = e.Message;
                return tt;
            }
        }
    }
}
