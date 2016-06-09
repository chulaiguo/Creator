using System.IO;
using System.Text;

namespace CodeGenerator.TestFactory
{
    public class TestDataProgramCode
    {
        private string _projectName = string.Empty;

        public TestDataProgramCode(string projectName)
        {
            this._projectName = projectName;
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);
            this.WriteContent(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Excel;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.TestData", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class Program");
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tstatic void Main(string[] args)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (args.Length != 1)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tShowHelpMenu();");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tswitch (args[0].ToLower())");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tcase \"/?\":");
            writer.WriteLine("\t\t\t\tcase \"/help\":");
            writer.WriteLine("\t\t\t\t\tShowHelpMenu();");
            writer.WriteLine("\t\t\t\t\tbreak;");

            writer.WriteLine("\t\t\t\tcase \"/i\":");
            writer.WriteLine("\t\t\t\t\tConsole.WriteLine(\"Start to create test data ....\");");
            writer.WriteLine("\t\t\t\t\ttry");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\tLookupBuilder lookup = new LookupBuilder();");
            writer.WriteLine("\t\t\t\t\t\tInsert(lookup, \"Lookup\");");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t\tcatch (Exception ex)");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\tConsole.WriteLine(ex.Message);");
            writer.WriteLine("\t\t\t\t\t\tif (ex.InnerException != null)");
            writer.WriteLine("\t\t\t\t\t\t\tConsole.WriteLine(ex.InnerException.Message);");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t\tConsole.WriteLine(\"Finished creating test data ....\");");
            writer.WriteLine("\t\t\t\t\tbreak;");

            writer.WriteLine("\t\t\t\tcase \"/u\":");
            writer.WriteLine("\t\t\t\t\tConsole.WriteLine(\"Start to delete test data ....\");");
            writer.WriteLine("\t\t\t\t\ttry");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\tLookupBuilder lookup = new LookupBuilder();");
            writer.WriteLine("\t\t\t\t\t\tDelete(lookup, \"Lookup\");");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t\tcatch (Exception ex)");
            writer.WriteLine("\t\t\t\t\t{");
            writer.WriteLine("\t\t\t\t\t\tConsole.WriteLine(ex.Message);");
            writer.WriteLine("\t\t\t\t\t\tif (ex.InnerException != null)");
            writer.WriteLine("\t\t\t\t\t\t\tConsole.WriteLine(ex.InnerException.Message);");
            writer.WriteLine("\t\t\t\t\t}");
            writer.WriteLine("\t\t\t\t\tConsole.WriteLine(\"Finished deleting test data ....\");");
            writer.WriteLine("\t\t\t\t\tbreak;");

            writer.WriteLine("\t\t\t\tdefault:");
            writer.WriteLine("\t\t\t\t\tShowHelpMenu();");
            writer.WriteLine("\t\t\t\t\tbreak;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tConsole.ReadLine();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate static void ShowHelpMenu()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tConsole.WriteLine(\"USAGE: {0}.TestData.exe [options]\");", this._projectName);
            writer.WriteLine("\t\t\tConsole.WriteLine(\"Options:\");");
            writer.WriteLine("\t\t\tConsole.WriteLine(\"\\t/? or /help\\tDisplay this usage message.\");");
            writer.WriteLine("\t\t\tConsole.WriteLine(\"\\t/u\\t\\tUn-load data from the database\");");
            writer.WriteLine("\t\t\tConsole.WriteLine(\"\\t/i\\t\\tInstall data into the database\");");
            writer.WriteLine("");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate static void Insert(BuilderBase builder, string name)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tResult r = builder.InsertAll();");
            writer.WriteLine("\t\t\tif (!r.OK)");
            writer.WriteLine("\t\t\t\tConsole.WriteLine(r.ToString());");
            writer.WriteLine("\t\t\telse");
            writer.WriteLine(
                "\t\t\t\t Console.WriteLine(String.Format(\"{0} data has been created successfully ... {1}\", name, DateTime.Now.ToLongTimeString()));");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate static void Delete(BuilderBase builder, string name)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tResult r = builder.DeleteAll();");
            writer.WriteLine("\t\t\tif (!r.OK)");
            writer.WriteLine("\t\t\t\tConsole.WriteLine(r.ToString());");
            writer.WriteLine("\t\t\telse");
            writer.WriteLine(
                "\t\t\t\t Console.WriteLine(String.Format(\"{0} data has been deleted successfully ... {1}\", name, DateTime.Now.ToLongTimeString()));");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }


        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}