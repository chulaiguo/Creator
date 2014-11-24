using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class TestCode
    {
        private Type _type = null;
        private string _entityName = string.Empty;

        public TestCode(Type type)
        {
            this._type = type;
            this._entityName = string.Format("{0}", this._type.Name.Substring(0, this._type.Name.Length - 4));
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteTable(writer);
            writer.WriteLine();
            this.WriteLoadData(writer);
            writer.WriteLine();
            this.WriteUpdateData(writer);

            return writer.ToString();
        }

        private void WriteTable(StringWriter writer)
        {
            List<PropertyInfo> list = this.GetPropertyList();
            writer.WriteLine("<table>");
            foreach (PropertyInfo item in list)
            {
                writer.WriteLine("<tr>");
                writer.WriteLine("\t<td align=\"right\">");
                writer.WriteLine("\t\t<asp:Label ID=\"lbl{0}\" runat=\"server\" Text=\"{0}\"></asp:Label>", item.Name);
                writer.WriteLine("\t</td>");
                writer.WriteLine("\t<td align=\"left\">");
                if (item.PropertyType == typeof(bool))
                {
                    writer.WriteLine("\t\t<asp:CheckBox ID=\"chk{0}\" runat=\"server\" />",item.Name);
                }
                else
                {
                    writer.WriteLine("\t\t<asp:TextBox ID=\"txt{0}\" runat=\"server\" Width=\"300px\"></asp:TextBox>",
                                     item.Name);
                }
                writer.WriteLine("\t</td>");
                writer.WriteLine("</tr>");
            }

            writer.WriteLine("</table>");
        }

        private void WriteLoadData(StringWriter writer)
        {
            string lowerItemName = string.Format("{0}{1}", this._entityName.Substring(0, 1).ToLower(),
                  this._entityName.Substring(1, this._entityName.Length - 3));

            List<PropertyInfo> list = this.GetPropertyList();
            writer.WriteLine("\t\tprivate void LoadData()");
            writer.WriteLine("\t\t{");
            foreach (PropertyInfo item in list)
            {
                if (item.PropertyType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t\tthis.txt{0}.Text = this._{1}.{0}.ToShortDateString();", item.Name, lowerItemName);
                }
                if (item.PropertyType == typeof(decimal) || item.PropertyType == typeof(float))
                {
                    writer.WriteLine("\t\t\tthis.txt{0}.Text = this._{1}.{0}.ToString(\"f2\");", item.Name, lowerItemName);
                }
                else if (item.PropertyType == typeof(bool))
                {
                    writer.WriteLine("\t\t\tthis.chk{0}.Checked = this._{1}.{0};", item.Name, lowerItemName);
                }
                else if (item.PropertyType == typeof(string))
                {
                    writer.WriteLine("\t\t\tthis.txt{0}.Text = this._{1}.{0};", item.Name, lowerItemName);
                }
                else
                {
                    writer.WriteLine("\t\t\tthis.txt{0}.Text = this._{1}.{0}.ToString();", item.Name, lowerItemName);
                }
            }

            writer.WriteLine("\t\t}");
        }

        private void WriteUpdateData(StringWriter writer)
        {
            string lowerItemName = string.Format("{0}{1}", this._entityName.Substring(0, 1).ToLower(),
                  this._entityName.Substring(1, this._entityName.Length - 3));

            List<PropertyInfo> list = this.GetPropertyList();
            writer.WriteLine("\t\tprivate void UpdateData()");
            writer.WriteLine("\t\t{");
            foreach (PropertyInfo item in list)
            {
                if (item.PropertyType == typeof(DateTime))
                {
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tDateTime date{0};", item.Name);
                    writer.WriteLine("\t\t\tif(DateTime.TryParse(this.txt{0}.Text.Trim(), out date{0}))", item.Name);
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis._{1}.{0} = date{0};", item.Name, lowerItemName);
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                }
                else if (item.PropertyType == typeof(decimal))
                {
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tdecimal decimal{0};", item.Name);
                    writer.WriteLine("\t\t\tif(decimal.TryParse(this.txt{0}.Text.Trim(), out decimal{0}))", item.Name);
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis._{1}.{0} = decimal{0};", item.Name, lowerItemName);
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                }
                else if (item.PropertyType == typeof(float))
                {
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tfloat float{0};", item.Name);
                    writer.WriteLine("\t\t\tif(float.TryParse(this.txt{0}.Text.Trim(), out float{0}))", item.Name);
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis._{1}.{0} = float{0};", item.Name, lowerItemName);
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                }
                else if (item.PropertyType == typeof(int))
                {
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tint int{0};", item.Name);
                    writer.WriteLine("\t\t\tif(int.TryParse(this.txt{0}.Text.Trim(), out int{0}))", item.Name);
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis._{1}.{0} = int{0};", item.Name, lowerItemName);
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                }
                else if (item.PropertyType == typeof(short))
                {
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tshort short{0};", item.Name);
                    writer.WriteLine("\t\t\tif(short.TryParse(this.txt{0}.Text.Trim(), out short{0}))", item.Name);
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis._{1}.{0} = short{0};", item.Name, lowerItemName);
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                }
                else if (item.PropertyType == typeof(byte))
                {
                    writer.WriteLine();
                    writer.WriteLine("\t\t\tbyte byte{0};", item.Name);
                    writer.WriteLine("\t\t\tif(byte.TryParse(this.txt{0}.Text.Trim(), out byte{0}))", item.Name);
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis._{1}.{0} = byte{0};", item.Name, lowerItemName);
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                }
                else if (item.PropertyType == typeof(bool))
                {
                    writer.WriteLine("\t\t\tthis._{1}.{0} = this.chk{0}.Checked;", item.Name, lowerItemName);
                }
                else
                {
                    writer.WriteLine("\t\t\tthis._{1}.{0} = this.txt{0}.Text;", item.Name, lowerItemName);
                }
            }

            writer.WriteLine("\t\t}");
        }

        private List<PropertyInfo> GetPropertyList()
        {
            List <PropertyInfo> list = new List<PropertyInfo>();
            PropertyInfo[] properties = this._type.GetProperties(BindingFlags.Public |
                                                     BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (PropertyInfo item in properties)
            {
                if (item.Name == "MarkAsDeleted" || item.Name == "PKString" || item.Name == "TableName"
                    || item.Name == "Active")
                    continue;

                if (item.Name == "RowVersion" || item.Name == "IsDirty" || item.Name == "IsValid")
                    continue;

                if (item.Name == "CreatedOn" || item.Name == "CreatedBy" || item.Name == "ModifiedOn"
                    || item.Name == "ModifiedBy" || item.Name == "LastModifiedAt" || item.Name == "LastModifiedBy")
                    continue;

                if (item.PropertyType == typeof(Guid))
                    continue;

                if (item.PropertyType.IsValueType || item.PropertyType == typeof(string))
                {
                   list.Add(item);
                }
            }

            return list;
        }
    }
}