using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class SchemaCode
    {
        private Assembly _assembly = null;
        private string _projectName = string.Empty;

        public SchemaCode(Assembly assembly, string projectName)
        {
            this._assembly = assembly;
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
            writer.WriteLine("namespace {0}.Schema", this._projectName);
            writer.WriteLine("{");
        }

        private void WriteContent(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type type in types)
            {
                if(!type.IsClass || !type.IsPublic)
                    continue;

                if(type.IsAbstract || type.IsInterface || type.IsNested)
                    continue;

                if (type.Name.EndsWith("Collection"))
                    continue;

                List<PropertyInfo> propertyList = this.GetPropertyList(type);
                if(propertyList.Count == 0)
                    continue;

                string className;
                if (type.Name.EndsWith("Data"))
                {
                    className = string.Format("{0}Schema", type.Name.Substring(0, type.Name.Length - 4));
                }
                //else if(type.Name.StartsWith("Biz"))
                //{
                //    className = string.Format("{0}Schema", type.Name.Substring(3));
                //}
                else
                {
                    className = string.Format("{0}Schema", type.Name);
                }
                writer.WriteLine("\tpublic partial class {0}", className);
                writer.WriteLine("\t{");
                foreach (PropertyInfo item in propertyList)
                {
                    writer.WriteLine("\t\tpublic const string {0} = \"{0}\";", item.Name);
                }
                if (type.Name.EndsWith("Data"))
                {
                    writer.WriteLine("\t\tpublic const string TableName = \"{0}\";", type.Name.Substring(0, type.Name.Length - 4));
                    writer.WriteLine("\t\tpublic const string TableAlias = \"{0}\";", type.Name.Substring(0, type.Name.Length - 4));

                    writer.WriteLine("\t\tpublic const string {0} = \"{0}\";", "CreatedOn");
                    writer.WriteLine("\t\tpublic const string {0} = \"{0}\";", "CreatedBy");
                    writer.WriteLine("\t\tpublic const string {0} = \"{0}\";", "ModifiedOn");
                    writer.WriteLine("\t\tpublic const string {0} = \"{0}\";", "ModifiedBy");
                }
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("}");
        }

        private List<PropertyInfo> GetPropertyList(Type type)
        {
            List<PropertyInfo> list = new List<PropertyInfo>();

            Hashtable table = new Hashtable();
            this.GetPropertyList(type.BaseType, table);

            PropertyInfo[] properties = type.GetProperties(BindingFlags.GetProperty | BindingFlags.Public
               | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo item in properties)
            {
                //if (!item.PropertyType.IsValueType && item.PropertyType != typeof(string))
                //    continue;

                if (item.PropertyType.IsEnum)
                    continue;

                if(table.ContainsKey(item.Name))
                    continue;

                list.Add(item);
            }

            return list;
        }

        private void GetPropertyList(Type type, Hashtable table)
        {
            if(type == typeof(object))
                return;

            PropertyInfo[] properties = type.GetProperties(BindingFlags.GetProperty | BindingFlags.Public
               | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo item in properties)
            {
                //if (!item.PropertyType.IsValueType && item.PropertyType != typeof(string))
                //    continue;

                if(item.PropertyType.IsEnum)
                    continue;

                if(table.ContainsKey(item.Name))
                    continue;

                table.Add(item.Name, item);
            }

            this.GetPropertyList(type.BaseType, table);
        }
    }
}
