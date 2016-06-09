using System.IO;
using System.Text;

namespace CodeGenerator
{
    public class Wrapper
    {
        private readonly string _fileName = string.Empty;

        public Wrapper(string fileName)
        {
            this._fileName = fileName;
        }

        public string GenCode()
        {
            if (!File.Exists(this._fileName))
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);
            string[] lines = File.ReadAllLines(this._fileName);
            foreach (string item in lines)
            {
                if(item.Trim().Length == 0)
                {
                    writer.WriteLine("writer.WriteLine(\"\");");
                    continue;
                }

                string text = item;
                int count = this.GetTabs(text);
                text = item.TrimStart('\t');
                count += this.GetBlanks(text)/4;
                text = text.Trim();

                string tabString = "";
                for (int i = 0; i < count; i++)
                {
                    tabString += "\\t";
                }

                text = this.TranslateString(text);
                string newLine = string.Format("writer.WriteLine(\"{0}{1}\");", tabString, text);
                writer.WriteLine(newLine);
            }

            return writer.ToString();
        }

        private int GetTabs(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != '\t')
                    return i;
            }

            return 0;
        }

        private int GetBlanks(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if(text[i] != ' ')
                    return i;
            }

            return 0;
        }

        private string TranslateString(string text)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\"')
                {
                    builder.Append("\\\"");
                }
                else
                {
                    builder.Append(text[i]);
                }
            }

            return builder.ToString();
        }
    }
}
