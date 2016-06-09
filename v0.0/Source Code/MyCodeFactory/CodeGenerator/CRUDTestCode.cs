using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class CRUDTestCode
    {
        private Assembly _assembly = null;

        public CRUDTestCode(Assembly assembly)
        {
            this._assembly = assembly;
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteContent(writer);

            return writer.ToString();
        }

        private void WriteContent(StringWriter writer)
        {
            Type[] types = this._assembly.GetTypes();
            foreach (Type type in types)
            {
                PropertyInfo propertyInfo = this.GetSQLPropertyInfo(type);
                if(propertyInfo == null)
                    continue;

                object obj = Activator.CreateInstance(type, "");
                if(obj == null)
                    continue;

                object sql = propertyInfo.GetValue(obj, null);
                if(sql == null)
                    continue;

                writer.WriteLine("/*{0}*/", type.Name);
                writer.WriteLine(sql.ToString());
                writer.WriteLine();
            }
        }

        private PropertyInfo GetSQLPropertyInfo(Type type)
        {
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (PropertyInfo item in propertyInfos)
            {
                if(item.Name == "SQLSelect")
                    return item;
            }

            return null;
        }
    }
}
