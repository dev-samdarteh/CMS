
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace RhemaCMS.Controllers.con_adhc
{
    public class AppUtility_SMS
    {
        public bool sendMessage(string from, string to, string message, string username, string password)
        {
            bool success = false;
            WebRequest request;
            HttpWebResponse response;
            string responseMess = "";
            string apiparams = "username={0}&password={1}&from={2}&to={3}&message={4}";
            string URL = "http://isms.wigalsolutions.com/ismsweb/sendmsg/";
            apiparams = string.Format(apiparams, username, password, from, to, message);

            try
            {
                request = HttpWebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                var data = Encoding.ASCII.GetBytes(apiparams);
                request.ContentLength = data.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
                request.Timeout = 5000;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (object Certsender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseMess = reader.ReadToEnd();
                response.Close();

                if (responseMess.StartsWith("SUCCESS"))
                    success = true;
                else
                    success = false;
            }
            catch (Exception __unusedException1__)
            {
                success = false;
            }

            return success;
        }

    }
}




public class appSMSAPI
{
    // Namespace CROSSBalance.CommonRes
    // Class appSMSAPI
   
}
