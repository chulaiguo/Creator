using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.UIFactory
{
    public class DetailEditorCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;

        private string _entityName = string.Empty;
        private List<PropertyInfo> _propertyList = null;

        public DetailEditorCode(Type type, string projectName)
        {
            this._type = type;
            this._projectName = projectName;

            this._entityName = this._type.Name.Substring(0, this._type.Name.Length - 4);
            this._propertyList = this.GetProperties();
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);
            this.WriteDataBinding(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using Cheke.WinCtrl;");
            writer.WriteLine("using {0}.ViewObj;", this._projectName);
            writer.WriteLine("using {0}.Schema;", this._projectName);
            writer.WriteLine("using {0}.WinUI.Utils;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            string entityName = this._type.Name.Substring(0, this._type.Name.Length - 4);
            writer.WriteLine("namespace {0}.WinUI.FormDetailEditor", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic partial class FormDetail{0} : FormDetailEditorBase", entityName);
            writer.WriteLine("\t{");

            this.WriteLookupData(writer);
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormDetail{0}()", entityName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic FormDetail{0}(string userid, {0} entity)", entityName);
            writer.WriteLine("\t\t\t: base(userid, entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprivate {0} {0}", entityName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn base.Entity as {0};", entityName);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteLookupData(StringWriter writer)
        {
            foreach (PropertyInfo item in this._propertyList)
            {
                if (item.PropertyType != typeof(Guid))
                    continue;

                string lowerItemName = string.Format("{0}{1}", item.Name.Substring(0, 1).ToLower(),
                  item.Name.Substring(1, item.Name.Length - 3));

                writer.WriteLine("\t\tprivate {0}Collection _{1}List = null;", item.Name.Substring(0, item.Name.Length - 2), lowerItemName);
            }
        }

        private void WriteDataBinding(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void DataBindingEntity()");
            writer.WriteLine("\t\t{");
            foreach (PropertyInfo item in this._propertyList)
            {
                if (item.PropertyType == typeof(Guid))
                    continue;

                if (item.PropertyType == typeof (bool))
                {
                    writer.WriteLine("\t\t\tthis.chk{0}.BindingData(this.{1}, {1}Schema.{0});", item.Name, this._entityName);
                }
                else if (item.PropertyType == typeof (DateTime))
                {
                    writer.WriteLine("\t\t\tthis.date{0}.BindingData(this.{1}, {1}Schema.{0});", item.Name, this._entityName);
                }
                else
                {
                    writer.WriteLine("\t\t\tthis.txt{0}.BindingData(this.{1}, {1}Schema.{0});", item.Name, this._entityName);
                }
            }

            writer.WriteLine();
            foreach (PropertyInfo item in this._propertyList)
            {
                if (item.PropertyType != typeof (Guid))
                    continue;


                string lowerItemName =
                    string.Format("{0}{1}", item.Name.Substring(0, 1).ToLower(),
                                  item.Name.Substring(1, item.Name.Length - 3));

                writer.WriteLine("\t\t\t//{0}", item.Name);
                writer.WriteLine("\t\t\tif (this._{0}List == null || base.IsRefreshData)", lowerItemName);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis._{0}List = {1}.GetAll();", lowerItemName,
                                 item.Name.Substring(0, item.Name.Length - 2)); 
                writer.WriteLine("\t\t\t\tLookUpEditBuilder.Setup{0}(this.cmb{0}.Properties, this._{1}List, string.Empty);",
                                 item.Name.Substring(0, item.Name.Length - 2), lowerItemName);
                writer.WriteLine("\t\t\t}");

                writer.WriteLine("\t\t\tthis.cmb{0}.BindingData(this.{1}, {1}Schema.{0});",
                                 item.Name.Substring(0, item.Name.Length - 2),
                                 this._entityName);

                writer.WriteLine();
            }

            writer.WriteLine("\t\t}");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        private List<PropertyInfo> GetProperties()
        {
            List<PropertyInfo> list = new List<PropertyInfo>();

            PropertyInfo[] properties = this._type.GetProperties(BindingFlags.Public |
                                                     BindingFlags.Instance |
                                                     BindingFlags.DeclaredOnly);

            foreach (PropertyInfo item in properties)
            {
                if(item.PropertyType == typeof(byte[]))
                    continue;

                if (item.PropertyType.Name.EndsWith("Collection"))
                    continue;

                if (item.Name == string.Format("{0}PK", this._entityName))
                    continue;

                if (item.Name == "RowVersion" || item.Name == "IsDirty" || item.Name == "IsValid" 
                    || item.Name == "PKString" || item.Name == "MarkAsDeleted" || item.Name == "TableName")
                    continue;

                if (item.Name == "CreatedOn" || item.Name == "CreatedBy" || item.Name == "ModifiedOn"
                    || item.Name == "ModifiedBy" || item.Name == "LastModifiedAt" || item.Name == "LastModifiedBy")
                    continue;

                list.Add(item);
            }

            return list;
        }
    }
}
