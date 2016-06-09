using System;
using System.IO;
using System.Text;

namespace CodeGenerator
{
    public class GenBizObjCollectionCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;
        private string _objectName = string.Empty;

        public GenBizObjCollectionCode(Type type, string projectName)
        {
            this._type = type;
            this._projectName = projectName;

            this._objectName = this._type.Name.Substring(0, this._type.Name.Length - 4);
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);
            this.WriteContent(writer);
            this.WriteSaveMethod(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke.BusinessEntity;");

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.BizObj", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\t[Serializable]");
            writer.WriteLine("\tpublic partial class {0}Collection : BusinessCollectionBase", this._objectName);
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tpublic {0}Collection()", this._objectName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase._itemType = typeof({0});", this._objectName);
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}Collection({1}Collection list)", this._objectName, this._type.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase._itemType = typeof({0});", this._objectName);
            writer.WriteLine();
            writer.WriteLine("\t\t\tforeach({0} item in list)", this._type.Name);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0} data = new {0}(item);", this._objectName);
            writer.WriteLine("\t\t\t\tbase.List.Add(data);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteContent(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic void Add({0} item)", this._objectName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.List.Add(item);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void AddRange({0}Collection list)", this._objectName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tIEnumerator enumerator = list.GetEnumerator();");
            writer.WriteLine("\t\t\twhile (enumerator.MoveNext())");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tbase.List.Add(enumerator.Current);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic bool Contains({0} item)", this._objectName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach ({0} data in base.List)", this._objectName);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (data.Equals(item))");
            writer.WriteLine("\t\t\t\t\treturn true;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn false;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic bool ContainsDeleted({0} item)", this._objectName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach ({0} data in base._deletedList)", this._objectName);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tif (data.Equals(item))");
            writer.WriteLine("\t\t\t\t\treturn true;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\treturn false;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void Insert(int index, {0} item)", this._objectName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tbase.List.Insert(index, item);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tprotected override void OnValidate(object item)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (item.GetType() != base._itemType)");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthrow new ArgumentException(\"The item must be a type of {0}\");", this._objectName);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void Remove({0} item)", this._objectName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t base.List.Remove(item);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0} this[int index]", this._objectName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn ({0}) base.List[index];", this._objectName);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tset");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tbase.List[index] = value;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteSaveMethod(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic override Result Save(string userid, string password)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\treturn {0}.Save(base.GetChanges(), userid, password);", this._objectName);
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
