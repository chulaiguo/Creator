using System.IO;
using System.Text;

namespace CodeGenerator.TestFactory
{
    public class DataBuilderFixtureBase
    {
        private string _projectName = string.Empty;

        public DataBuilderFixtureBase(string projectName)
        {
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
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using NUnit.Framework;");

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.ExcelFixture", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\t public class FixtureBase");
            writer.WriteLine("\t{");
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected void ValidateResult(Result r)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (!r.OK)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tConsole.Error.WriteLine(r.ToString());");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tAssert.IsTrue(r.OK);");
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