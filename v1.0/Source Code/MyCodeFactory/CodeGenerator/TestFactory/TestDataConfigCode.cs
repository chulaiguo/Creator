using System.IO;
using System.Text;

namespace CodeGenerator.TestFactory
{
    public class TestDataConfigCode
    {
        private string _excelDir = string.Empty;
        private string _projectName = string.Empty;

        public TestDataConfigCode(string excelDir, string projectName)
        {
            this._excelDir = excelDir;
            this._projectName = projectName;
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.BeginWrite(writer);
            this.WriteContent(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            writer.WriteLine("<configuration>");
            writer.WriteLine("\t<configSections>");
            writer.WriteLine("\t\t<section name=\"Framework\" type=\"Cheke.Configuration.ConfigurationHandler,Cheke.Configuration\"/>");
            writer.WriteLine("\t</configSections>");

            writer.WriteLine("\t<Framework type=\"Cheke.Configuration.ConfigurationManager,Cheke.Configuration\">");
            writer.WriteLine("\t\t<ClassFactory>");
            writer.WriteLine("\t\t\t<Class name=\"DataServiceFactory\" type=\"{0}.IDataServiceEx.IDataServiceFactoryEx, {0}.IDataServiceEx\"", this._projectName);
            writer.WriteLine("\t\t\t\tlocation=\"http://develop.chekeit.com/{0}/DataService/DataServiceFactory.soap\"/>", this._projectName);
            writer.WriteLine("\t\t</ClassFactory>");
            writer.WriteLine("\t</Framework>");
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\t<appSettings>");

            DirectoryInfo directory = new DirectoryInfo(this._excelDir);
            foreach (FileInfo item in directory.GetFiles())
            {
                if(!item.Extension.StartsWith(".xls"))
                    continue;

                int index = item.Name.IndexOf('.');
                writer.WriteLine("\t\t<add key=\"Excel:{0}\" value=\"..\\..\\..\\..\\..\\..\\Doc\\TestData\\{1}\" />", item.Name.Substring(0, index), item.Name);
            }
            writer.WriteLine("\t\t");
            writer.WriteLine("\t</appSettings>");
        }


        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("</configuration>");
        }
    }
}