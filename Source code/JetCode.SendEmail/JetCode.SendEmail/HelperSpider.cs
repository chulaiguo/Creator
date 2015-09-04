using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace WindowsFormsApplication3
{
    internal static class HelperSpider
    {
        public static readonly SortedList<string, bool> _KeyWordList = new SortedList<string, bool>();
        private static readonly SortedList<string, bool> _LinkList = new SortedList<string, bool>();

        public static void Retrive(string seed)
        {
            LoadKeyWord(seed);

            while (true)
            {
                string link = GetNextUrl();
                if(string.IsNullOrEmpty(link))
                    break;

                LoadKeyWord(link);
            }
        }

        public static string GetNextUrl()
        {
            try
            {
                string url = string.Empty;
                foreach (KeyValuePair<string, bool> pair in _LinkList)
                {
                    if(pair.Value)
                        continue;

                    url = pair.Key;
                    break;
                }

                if (url.Length > 0)
                {
                    _LinkList[url] = true;
                }
                return url;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void LoadKeyWord(string url)
        {
            try
            {
                string data = GetResponse(url);
                if (string.IsNullOrEmpty(data))
                {
                    Thread.Sleep(1000);
                    data = GetResponse(url);
                }
                if (string.IsNullOrEmpty(data))
                {
                    Thread.Sleep(1000);
                    data = GetResponse(url);
                }
                if (string.IsNullOrEmpty(data))
                {
                    return;
                }

                string[] lines = data.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in lines)
                {
                    string line = item.Trim();

                    //KeyWord
                    string info = RetriveKeyWord(line);
                    if (!string.IsNullOrEmpty(info))
                    {
                        if (!_KeyWordList.ContainsKey(info))
                        {
                            _KeyWordList.Add(info, false);
                        }
                    }

                    //Link
                    string link = RetriveLink(line);
                    if (!string.IsNullOrEmpty(link))
                    {
                        if (!_LinkList.ContainsKey(link))
                        {
                            _LinkList.Add(link, false);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private static string RetriveKeyWord(string data)
        {
            try
            {
                //l[0]='>';l[1]='a';l[2]='/';l[3]='<';l[4]='|109';l[5]='|111';l[6]='|99';l[7]='|46';l[8]='|115';l[9]='|109';l[10]='|101';l[11]='|116';l[12]='|115';l[13]='|121';l[14]='|115';l[15]='|121';l[16]='|103';l[17]='|114';l[18]='|101';l[19]='|110';l[20]='|101';l[21]='|45';l[22]='|115';l[23]='|101';l[24]='|114';l[25]='|97';l[26]='|64';l[27]='|101';l[28]='|99';l[29]='|105';l[30]='|102';l[31]='|102';l[32]='|111';l[33]='>';l[34]='"';l[35]='|109';l[36]='|111';l[37]='|99';l[38]='|46';l[39]='|115';l[40]='|109';l[41]='|101';l[42]='|116';l[43]='|115';l[44]='|121';l[45]='|115';l[46]='|121';l[47]='|103';l[48]='|114';l[49]='|101';l[50]='|110';l[51]='|101';l[52]='|45';l[53]='|115';l[54]='|101';l[55]='|114';l[56]='|97';l[57]='|64';l[58]='|101';l[59]='|99';l[60]='|105';l[61]='|102';l[62]='|102';l[63]='|111';l[64]='';l[65]=':';l[66]='o';l[67]='t';l[68]='l';l[69]='i';l[70]='a';l[71]='m';l[72]='"';l[73]='=';l[74]='f';l[75]='e';l[76]='r';l[77]='h';l[78]=' ';l[79]='"';l[80]='t';l[81]='x';l[82]='e';l[83]='t';l[84]='d';l[85]='i';l[86]='m';l[87]='"';l[88]='=';l[89]='s';l[90]='s';l[91]='a';l[92]='l';l[93]='c';l[94]=' ';l[95]='a';l[96]='<';    
                if (!data.StartsWith("l[0]='>';l[1]='a';l[2]='/';l[3]='<';"))
                    return string.Empty;

                List<string> list = new List<string>();
                string[] splits = data.Split(';');
                if (splits.Length <= 4)
                    return string.Empty;

                for (int i = 4; i < splits.Length; i++)
                {
                    string split = splits[i];
                    if (string.IsNullOrEmpty(split))
                        continue;

                    string[] content = split.Split('=');
                    if (content.Length != 2)
                        continue;

                    string item = content[1].Trim('\'');
                    if (item == ">")
                        break;

                    if (!item.StartsWith("|"))
                        continue;

                    list.Add(item.Substring(1));
                }

                StringBuilder builder = new StringBuilder();
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    builder.Append((char)int.Parse(list[i]));
                }

                return builder.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string RetriveLink(string data)
        {
            try
            {
                //<li><a href="/directory/equipment">Production Equipment</a></li>
                int index = data.IndexOf("href=\"/directory/", StringComparison.Ordinal);
                if (index == -1)
                    return string.Empty;

                data = data.Substring(index + "href=\"".Length);
                index = data.IndexOf("\"", StringComparison.Ordinal);
                if (index == -1)
                    return string.Empty;

                return "http://www.enfsolar.com" + data.Substring(0, index);

            }
            catch
            {
                return string.Empty;
            }
        }

        private static string GetResponse(string url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse webResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                if (stream == null)
                    return string.Empty;

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
