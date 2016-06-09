using System.Data;
using System.Data.OleDb;
using System.Text;
using System.IO;

namespace CodeGenerator.TestFactory
{
    public class DataBuilderCode
    {
        private string _excelName = string.Empty;
        private string _projectName = string.Empty;

        public DataBuilderCode(string excelName, string projectName)
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
            writer.WriteLine("using System.Configuration;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            string[] paths = this._excelName.Split('\\');
            string name = paths[paths.Length - 1];
            string className = name.Substring(0, name.Length - 4);

            writer.WriteLine("namespace {0}.Excel", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class {0}Builder : BuilderBase", className);
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tpublic {0}Builder() : base(ConfigurationManager.AppSettings[\"Excel:{0}\"])", className);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            string connString = string.Format("provider=Microsoft.Jet.OLEDB.4.0;data source={0};Extended Properties=Excel 8.0;Persist Security Info=False", this._excelName);
            OleDbConnection connection = new OleDbConnection(connString);
            connection.Open(); 

            DataTable dt = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,new object[] {null, null, null, "TABLE"});

            //Override
            writer.WriteLine("\t\t#region Override");
            writer.WriteLine("\t\tpublic override Result InsertParent()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn new Result(true);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic override Result DeleteParent()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn new Result(true);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            
            writer.WriteLine("\t\tpublic override Result InsertAll()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tResult r;");
            writer.WriteLine();
            foreach (DataRow item in dt.Rows)
            {
                string tableName = item["TABLE_NAME"].ToString().TrimEnd('$');
                writer.WriteLine("\t\t\tif (!(r = this.Insert{0}()).OK)", tableName);
                writer.WriteLine("\t\t\t\treturn r;");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t\treturn new Result(true);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic override Result DeleteAll()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tResult r;");
            writer.WriteLine();
            foreach (DataRow item in dt.Rows)
            {
                string tableName = item["TABLE_NAME"].ToString().TrimEnd('$');
                writer.WriteLine("\t\t\tif (!(r = this.Delete{0}()).OK)", tableName);
                writer.WriteLine("\t\t\t\treturn r;");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t\treturn new Result(true);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();

            //Collection Properties
            writer.WriteLine("\t\t#region Collection Properties");
            foreach (DataRow item in dt.Rows) 
            {
                string tableName = item["TABLE_NAME"].ToString().TrimEnd('$');
                writer.WriteLine("\t\tpublic {0}DataCollection {0}List", tableName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn base.LoadFromExcel(\"{0}\", typeof ({0}Data)) as {0}DataCollection;", tableName);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            } 
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();

            //Insert
            writer.WriteLine("\t\t#region Insert");
            foreach (DataRow item in dt.Rows)
            {
                string tableName = item["TABLE_NAME"].ToString().TrimEnd('$');
                writer.WriteLine("\t\tpublic Result Insert{0}()", tableName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn base.InsertCollection(this.{0}List);", tableName);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();

            //delete
            writer.WriteLine("\t\t#region Delete");
            foreach (DataRow item in dt.Rows)
            {
                string tableName = item["TABLE_NAME"].ToString().TrimEnd('$');
                writer.WriteLine("\t\tpublic Result Delete{0}()", tableName);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\treturn base.DeleteCollection(ServiceBuilder.{0}DataService.GetAll());", tableName);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }
            writer.WriteLine("\t\t#endregion");

            connection.Close(); 
        }


        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}