using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class ViewCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;
        private string _className = string.Empty;
        private List<FieldInfo> _fieldList = null;

        public ViewCode(Type type, string projectName)
        {
            this._type = type;
            this._projectName = projectName;

            this._className = string.Format("{0}View", this._type.Name.Substring(0, this._type.Name.Length - 4));
            this._fieldList = this.GetFieldList();
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);
            this.WriteContent(writer);
            this.WriteConstruct(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Data", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\t[Serializable]");
            writer.WriteLine("\tpublic class {0}", this._className);
            writer.WriteLine("\t{");
        }

        private bool IsPKField(FieldInfo field)
        {
            if(field.FieldType != typeof(Guid))
                return false;

            string tableName = field.Name.Substring(1, field.Name.Length - 3);
            return string.Compare(tableName, this._type.Name.Substring(0, this._type.Name.Length - 4), true) == 0;
        }

        private void WriteContent(StringWriter writer)
        {
            foreach (FieldInfo item in _fieldList)
            {
                bool isPK = this.IsPKField(item);

                //Fields
                if (item.FieldType == typeof(Guid))
                {
                    if(isPK)
                    {
                        writer.WriteLine("\t\tprivate {0} {1} = Guid.Empty;", item.FieldType, item.Name);
                    }
                    else
                    {
                        writer.WriteLine("\t\t//private {0} {1} = Guid.Empty;", item.FieldType, item.Name);
                    }
                }
                else if (item.FieldType == typeof(string))
                {
                    writer.WriteLine("\t\t//private {0} {1} = string.Empty;", item.FieldType, item.Name);
                }
                else if (item.FieldType == typeof(DateTime))
                {
                    writer.WriteLine("\t\t//private {0} {1} = new DateTime(1900, 1, 1);", item.FieldType, item.Name);
                }
                else if (item.FieldType == typeof(bool))
                {
                    writer.WriteLine("\t\t//private {0} {1} = false;", item.FieldType, item.Name);
                }
                else
                {
                    writer.WriteLine("\t\t//private {0} {1} = 0;", item.FieldType, item.Name);
                }

                //Properties
                string propertyName = item.Name.TrimStart('_');
                propertyName = propertyName.Substring(0, 1).ToUpper() + propertyName.Substring(1);
                if (isPK)
                {
                    writer.WriteLine("\t\tpublic {0} {1}", item.FieldType, propertyName);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\treturn this.{0};", item.Name);
                    writer.WriteLine("\t\t\t}");

                    writer.WriteLine("\t\t\tset");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis.{0} = value;", item.Name);
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine("\t\t}");
                }
                else
                {
                    writer.WriteLine("\t\t//public {0} {1}", item.FieldType, propertyName);
                    writer.WriteLine("\t\t//{");
                    writer.WriteLine("\t\t//\tget");
                    writer.WriteLine("\t\t//\t{");
                    writer.WriteLine("\t\t//\t\treturn this.{0};", item.Name);
                    writer.WriteLine("\t\t//\t}");

                    writer.WriteLine("\t\t//\tset");
                    writer.WriteLine("\t\t//\t{");
                    writer.WriteLine("\t\t//\t\tthis.{0} = value;", item.Name);
                    writer.WriteLine("\t\t//\t}");
                    writer.WriteLine("\t\t//}");
                }
                writer.WriteLine();
            }
        }

        private void WriteConstruct(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic {0}()", this._className);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}({1} data)", this._className, this._type.Name);
            writer.WriteLine("\t\t{");
            foreach (FieldInfo item in _fieldList)
            {
                bool isPK = this.IsPKField(item);

                string propertyName = item.Name.TrimStart('_');
                propertyName = propertyName.Substring(0, 1).ToUpper() + propertyName.Substring(1);

                if (isPK)
                {
                    writer.WriteLine("\t\t\tthis.{0} = data.{0};", propertyName);
                }
                else
                {
                    writer.WriteLine("\t\t\t//this.{0} = data.{0};", propertyName);
                }
            }
            writer.WriteLine("\t\t}");
            writer.WriteLine();
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0} To{0}()", this._type.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0} entity = new {0}();", this._type.Name);
            foreach (FieldInfo item in _fieldList)
            {
                bool isPK = this.IsPKField(item);

                string propertyName = item.Name.TrimStart('_');
                propertyName = propertyName.Substring(0, 1).ToUpper() + propertyName.Substring(1);

                if (isPK)
                {
                    writer.WriteLine("\t\t\tentity.{0} = this.{0};", propertyName);
                }
                else
                {
                    writer.WriteLine("\t\t\t//entity.{0} = this.{0};", propertyName);
                }
            }
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn entity;");
            writer.WriteLine("\t\t}");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        private List<FieldInfo> GetFieldList()
        {
            List<FieldInfo> list = new List<FieldInfo>();

            FieldInfo[] fields = this._type.GetFields(BindingFlags.GetField | BindingFlags.NonPublic
               | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (FieldInfo item in fields)
            {
                if (!item.FieldType.IsValueType && item.FieldType != typeof(string))
                    continue;

                if (item.Name == "_createdBy" || item.Name == "_createdOn" || item.Name == "_modifiedBy"
                    || item.Name == "_modifiedOn" || item.Name == "_active")
                    continue;

               list.Add(item);
            }

            return list;
        }
    }
}
