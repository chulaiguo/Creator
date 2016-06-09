using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.UIFactory
{
    public class DataBindingCode
    {
        private Type _type = null;

        public DataBindingCode(Type type)
        {
            this._type = type;
        }

        public string GenDataBindingCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteDataBinding(writer);

            return writer.ToString();
        }

        private void WriteDataBinding(StringWriter writer)
        {
            bool isMarkAsDeleted = false;

            string entityName =
                string.Format("{0}", this._type.Name.Substring(0, this._type.Name.Length - 4));

            string lowerEntityName =
                string.Format("{0}{1}", entityName.Substring(0, 1).ToLower(), entityName.Substring(1));


            PropertyInfo[] properties = this._type.GetProperties(BindingFlags.Public |
                                                                 BindingFlags.Instance | BindingFlags.DeclaredOnly);

            writer.WriteLine("//data binding");
            foreach (PropertyInfo item in properties)
            {
                if (item.Name == "MarkAsDeleted")
                {
                    isMarkAsDeleted = true;
                    continue;
                }

                if (item.Name == "RowVersion" || item.Name == "IsDirty" || item.Name == "IsValid")
                    continue;

                if (item.Name == "CreatedOn" || item.Name == "CreatedBy" || item.Name == "ModifiedOn"
                    || item.Name == "ModifiedBy" || item.Name == "LastModifiedAt" || item.Name == "LastModifiedBy")
                    continue;


                if (item.PropertyType == typeof(byte) || item.PropertyType == typeof(short)
                    || item.PropertyType == typeof(int) || item.PropertyType == typeof(double)
                    || item.PropertyType == typeof(float) || item.PropertyType == typeof(decimal))
                {
                    writer.WriteLine("this.txt{0}.BindingData(this.{1}, {1}Schema.{0});", item.Name, entityName);
                }
                else if (item.PropertyType == typeof (string))
                {
                    writer.WriteLine("this.txt{0}.BindingData(this.{1}, {1}Schema.{0});", item.Name, entityName);
                }
                else if (item.PropertyType == typeof (bool))
                {
                    writer.WriteLine("this.chk{0}.BindingData(this.{1}, {1}Schema.{0});", item.Name, entityName);
                }
                else if (item.PropertyType == typeof (DateTime))
                {
                    writer.WriteLine("this.date{0}.BindingData(this.{1}, {1}Schema.{0});", item.Name, entityName);
                }
                else if (item.PropertyType == typeof(byte[]))
                {
                    writer.WriteLine("this.pic{0}.BindingData(this.{1}, {1}Schema.{0});", item.Name, entityName);
                }
                else
                {
                }
            }

            writer.WriteLine();
            foreach (PropertyInfo item in properties)
            {
                if (item.PropertyType != typeof (Guid))
                    continue;

                if (item.Name == string.Format("{0}PK", this._type.Name.Substring(0, this._type.Name.Length - 4)))
                    continue;

                string lowerItemName =
                    string.Format("{0}{1}", item.Name.Substring(0, 1).ToLower(),
                                  item.Name.Substring(1, item.Name.Length - 3));

                writer.WriteLine("//{0}", item.Name);
                writer.WriteLine("if (this._{0}List == null || base.IsRefreshData)", lowerItemName);
                writer.WriteLine("{");
                if(isMarkAsDeleted)
                {
                    writer.WriteLine("\tthis._{0}List = {1}Data.GetAll(false);", lowerItemName,
                                 item.Name.Substring(0, item.Name.Length - 2));
                }
                else
                {
                    writer.WriteLine("\tthis._{0}List = {1}.GetAll();", lowerItemName,
                                 item.Name.Substring(0, item.Name.Length - 2)); 
                }
                writer.WriteLine("\tLookUpEditBuilder.Setup{0}(this.cmb{0}.Properties, this._{1}List, string.Empty);",
                                 item.Name.Substring(0, item.Name.Length - 2), lowerItemName);
                writer.WriteLine("}");

                writer.WriteLine("this.cmb{0}.BindingData(this.{1}, {1}Schema.{0});",
                                 item.Name.Substring(0, item.Name.Length - 2),
                                 entityName);

                writer.WriteLine();
            }

            writer.WriteLine("//Look up data source");
            foreach (PropertyInfo item in properties)
            {
                if (item.PropertyType != typeof(Guid))
                    continue;

                if (item.Name == string.Format("{0}PK", this._type.Name.Substring(0, this._type.Name.Length - 4)))
                    continue;

                string lowerItemName = string.Format("{0}{1}", item.Name.Substring(0, 1).ToLower(),
                  item.Name.Substring(1, item.Name.Length - 3));

                writer.WriteLine("private {0}Collection _{1}List = null;", item.Name.Substring(0, item.Name.Length - 2), lowerItemName);
            }

            writer.WriteLine();
            writer.WriteLine("//Look up builder");
            foreach (PropertyInfo item in properties)
            {
                if (item.PropertyType != typeof(Guid))
                    continue;

                if (item.Name == string.Format("{0}PK", this._type.Name.Substring(0, this._type.Name.Length - 4)))
                    continue;


                writer.WriteLine("public static void Setup{0}(RepositoryItemLookUpEdit lookUpEdit, object dataSource, string valueMember)", item.Name.Substring(0, item.Name.Length - 2));
                writer.WriteLine("{");
                writer.WriteLine("\tif (lookUpEdit.Columns.Count == 0)");
                writer.WriteLine("\t{");
                writer.WriteLine("\t\tLookUpColumnInfo col = new LookUpColumnInfo();");
                writer.WriteLine("\t\tcol.FieldName = {0}Schema.{0}Name;", item.Name.Substring(0, item.Name.Length - 2));
                writer.WriteLine("\t\tcol.Caption = \"XXX\";");
                writer.WriteLine("\t\tlookUpEdit.Columns.Add(col);");
                writer.WriteLine("\t}");
                writer.WriteLine();
                writer.WriteLine("\tlookUpEdit.DropDownRows = 10;");
                writer.WriteLine("\tlookUpEdit.PopupWidth = 100;");
                writer.WriteLine();
                writer.WriteLine("\tlookUpEdit.ValueMember = valueMember;");
                writer.WriteLine("\tlookUpEdit.DisplayMember = {0}Schema.{0}Name;", item.Name.Substring(0, item.Name.Length - 2));
                writer.WriteLine();
                writer.WriteLine("\tlookUpEdit.DataSource = dataSource;");
                writer.WriteLine("\tlookUpEdit.BestFit();");
                writer.WriteLine("}");
                writer.WriteLine();
            }
        }
    }
}