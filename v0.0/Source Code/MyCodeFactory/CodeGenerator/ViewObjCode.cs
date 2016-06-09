using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator
{
    public class ViewObjCode
    {
        private readonly Assembly _assembly = null;
        private readonly string _projectName = string.Empty;

        public ViewObjCode(Assembly assembly, string projectName)
        {
            this._assembly = assembly;
            this._projectName = projectName;
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.WriteContent(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.Data;", this._projectName);
           
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.ViewObj", this._projectName);
            writer.WriteLine("{");

            Type[] types = this._assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!item.IsPublic)
                    continue;

                if(!item.Name.EndsWith("Data"))
                    continue;

                string className = item.Name.Substring(0, item.Name.Length - "Data".Length);

                //Item class
                writer.WriteLine("\t[Serializable()]");
                writer.WriteLine("\tpublic partial class {0}: {0}Data", className);
                writer.WriteLine("\t{");

                writer.WriteLine("\t\tpublic {0}()", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tthis.InitMemberVariables();");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t public {0}({0}Data data) : base(data)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                //Parents
                SortedList<string, PropertyInfo> parents = this.GetParents(item);
                foreach (KeyValuePair<string, PropertyInfo> parent in parents)
                {
                    string parentType = parent.Value.PropertyType.Name.Substring(0, parent.Value.PropertyType.Name.Length - "Data".Length);
                    writer.WriteLine("\t\tpublic new {0} {1}", parentType, parent.Key);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget {{ return new {0}(base.{1});}}", parentType, parent.Key);
                    writer.WriteLine("\t\t\tset {{ base.{0} = value;}}", parent.Key);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine();
                }

                //Children
                SortedList<string, PropertyInfo> children = this.GetChildren(item);
                foreach (KeyValuePair<string, PropertyInfo> child in children)
                {
                    string childType = child.Value.PropertyType.Name.Substring(0, child.Value.PropertyType.Name.Length - "DataCollection".Length) + "Collection";
                    writer.WriteLine("\t\tpublic new {0} {1}", childType, child.Key);
                    writer.WriteLine("\t\t{");
                    writer.WriteLine("\t\t\tget {{ return (({0})(base.{1})); }}", childType, child.Key);
                    writer.WriteLine("\t\t\tset {{ base.{0} = value;}}", child.Key);
                    writer.WriteLine("\t\t}");
                    writer.WriteLine(); 
                }

                //CloneChildren
                writer.WriteLine("\t\tprotected override void CloneChildren(Cheke.BusinessEntity.BusinessBase data)");
                writer.WriteLine("\t\t{");
                if(children.Count > 0)
                {
                    writer.WriteLine("\t\t\t{0}Data dataObj = (({0}Data)(data));", className);
                    writer.WriteLine("\t\t\tif (dataObj != null)");
                    writer.WriteLine("\t\t\t{");
                    foreach (KeyValuePair<string, PropertyInfo> child in children)
                    {
                        string childType = child.Value.PropertyType.Name.Substring(0, child.Value.PropertyType.Name.Length - "DataCollection".Length) + "Collection";
                        writer.WriteLine("\t\t\t\tif (dataObj.{0} != null)", child.Key);
                        writer.WriteLine("\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\tthis.{0} = new {1}(dataObj.{0});", child.Key, childType);
                        writer.WriteLine("\t\t\t\t}");
                    }
                    writer.WriteLine("\t\t\t}");
                }
                writer.WriteLine("\t\t}");
                writer.WriteLine("\t}");
                writer.WriteLine();

                //List Class
                writer.WriteLine("\t[Serializable()]");
                writer.WriteLine("\tpublic partial class {0}Collection: {0}DataCollection", className);
                writer.WriteLine("\t{");

                writer.WriteLine("\t\tpublic {0}Collection()", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tbase._itemType = typeof({0});", className);
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic {0}Collection({0}DataCollection list)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tbase._itemType = typeof({0});", className);
                writer.WriteLine();
                writer.WriteLine("\t\t\tforeach ({0}Data item in list)", className);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.Add(new {0}(item));", className);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\tSystem.Collections.ArrayList deletedList = list.GetDeletedList();");
                writer.WriteLine("\t\t\tforeach ({0}Data item in deletedList)", className);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis._deletedList.Add(new {0}(item));", className);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\tthis.Block.Size = list.Block.Size;");
                writer.WriteLine("\t\t\tthis.Block.Index = list.Block.Index;");
                writer.WriteLine("\t\t\tthis.Block.Count = list.Block.Count;");
                writer.WriteLine("\t\t\tthis.Block.Records = list.Block.Records;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();

                writer.WriteLine("\t\tpublic new {0} this[int index]", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget {{ return (({0})(List[index])); }}", className);
                writer.WriteLine("\t\t\tset { List[index] = value; }");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void Add({0} item)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tList.Add(item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void AddRange({0}Collection list)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tforeach ({0} item in list)", className);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.Add(item);");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void Insert(int index, {0} item)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tList.Insert(index, item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic void Remove({0} item)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tList.Remove(item);");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic bool Contains({0} entity)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tforeach ({0} item in List)", className);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tif (entity.Equals(item))");
                writer.WriteLine("\t\t\t\t\treturn true;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\treturn false;");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\tpublic bool ContainsDeleted({0} entity)", className);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tforeach ({0} item in this._deletedList)", className);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tif (entity.Equals(item))");
                writer.WriteLine("\t\t\t\t\treturn true;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine();
                writer.WriteLine("\t\t\treturn false;");
                writer.WriteLine("\t\t}");
                writer.WriteLine("\t}");
                writer.WriteLine();
            }
            
            writer.WriteLine("}");
        }

        private SortedList<string, PropertyInfo> GetParents(Type type)
        {
            SortedList<string, PropertyInfo> list = new SortedList<string, PropertyInfo>();
            PropertyInfo[] infoList = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo info in infoList)
            {
                if(list.ContainsKey(info.Name))
                    continue;

                if(!info.CanRead || !info.CanWrite)
                    continue;

                if(info.PropertyType.IsValueType)
                    continue;

                if (info.PropertyType == typeof(string) || info.PropertyType == typeof(byte[]))
                    continue;

                if(info.PropertyType.Name.EndsWith("Data"))
                {
                    list.Add(info.Name, info);
                }
            }

            return list;
        }

        private SortedList<string, PropertyInfo> GetChildren(Type type)
        {
            SortedList<string, PropertyInfo> list = new SortedList<string, PropertyInfo>();
            PropertyInfo[] infoList = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo info in infoList)
            {
                if (list.ContainsKey(info.Name))
                    continue;

                if (!info.CanRead || !info.CanWrite)
                    continue;

                if (info.PropertyType.IsValueType)
                    continue;

                if (info.PropertyType == typeof(string) || info.PropertyType == typeof(byte[]))
                    continue;

                if (info.PropertyType.Name.EndsWith("DataCollection"))
                {
                    list.Add(info.Name, info);
                }
            }

            return list;
        }
    }
}