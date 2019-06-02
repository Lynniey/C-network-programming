using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    class YandexTranslator
    {
        public string Translate(string text, string language)
        {
            if (text.Length > 0)
            {
            	
            	string link = "https://translate.yandex.net/api/v1.5/tr.json/translate?";
           		string key = "trnsl.1.1.20161224T124733Z.6b01137325686872.7944b75c1dcebb456bae7f92a4026d276c7429fb";
            		
           		WebRequest request = (HttpWebRequest)WebRequest.Create(link + "key=" + key + "&text=" + text + "&lang=" + language);
           		WebResponse response = (HttpWebResponse)request.GetResponse();
            	

                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    string line;
                    if ((line = stream.ReadLine()) != null)
                    {
                        text = line.Substring(line.IndexOf(":[\"") + 3);
                        text = text.Remove(text.Length - 3);
                    }
                }

                return text;
            }
            else
                return " ";
        }
    }
}