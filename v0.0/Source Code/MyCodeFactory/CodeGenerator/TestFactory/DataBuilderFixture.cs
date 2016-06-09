using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace CodeGenerator.TestFactory
{
    public class DataBuilderFixture
    {
        private string _excelName = string.Empty;
        private string _projectName = string.Empty;

        public DataBuilderFixture(string excelName, string projectName)
        {
            this._excelName = excelName;
            this._projectName = projectName;
        }

        public string GenBuilderCode()
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
            writer.WriteLine("using NUnit.Framework;");
            writer.WriteLine("using {0}.Excel;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            string[] paths = this._excelName.Split('\\');
            string name = paths[paths.Length - 1];
            string className = name.Substring(0, name.Length - 4);

            writer.WriteLine("namespace {0}.ExcelFixture", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\t[TestFixture]");
            writer.WriteLine("\tpublic class {0}BuilderFixture : FixtureBase", className);
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tprivate {0}Builder _builder = null;", className);
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            string connString = string.Format("provider=Microsoft.Jet.OLEDB.4.0;data source={0};Extended Properties=Excel 8.0;Persist Security Info=False", this._excelName);
            OleDbConnection connection = new OleDbConnection(connString);
            connection.Open();
            DataTable dt = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            string[] paths = this._excelName.Split('\\');
            string name = paths[paths.Length - 1];
            string className = name.Substring(0, name.Length - 4);

            writer.WriteLine("\t\t[TestFixtureSetUp]");
            writer.WriteLine("\t\tpublic void FixtureSetUp()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._builder = new {0}Builder();", className);
            writer.WriteLine("\t\t\tthis._builder.InsertParent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\t[TestFixtureTearDown]");
            writer.WriteLine("\t\tpublic void FixtureTearDown()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._builder.DeleteParent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();


            writer.WriteLine("\t\t[SetUp]");
            writer.WriteLine("\t\tpublic void SetUp()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis._builder.DeleteAll();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\t[TearDown]");
            writer.WriteLine("\t\tpublic void TearDown()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\t[Test]");
            writer.WriteLine("\t\tpublic void InsertAll()");
            writer.WriteLine("\t\t{");
            foreach (DataRow item in dt.Rows)
            {
                string tableName = item["TABLE_NAME"].ToString().TrimEnd('$');
                writer.WriteLine("\t\t\tAssert.IsTrue(this._builder.{0}List.Count > 0);", tableName);
            }
            writer.WriteLine();
            writer.WriteLine("\t\t\tbase.ValidateResult(this._builder.InsertAll());");
            writer.WriteLine("\t\t\tbase.ValidateResult(this._builder.DeleteAll());");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            connection.Close();
        }


        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}