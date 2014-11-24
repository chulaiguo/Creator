using System;
using System.IO;
using System.Text;

namespace CodeGenerator
{
    public class ViewCollectionCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;
        private string _className = string.Empty;
        private string _entityName = string.Empty;

        public ViewCollectionCode(Type type, string projectName)
        {
            this._type = type;
            this._projectName = projectName;

            this._entityName = string.Format("{0}View", this._type.Name.Substring(0, this._type.Name.Length - 4));
            this._className = string.Format("{0}ViewCollection", this._type.Name.Substring(0, this._type.Name.Length - 4));
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections;");

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.Data", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\t[Serializable]");
            writer.WriteLine("\tpublic class {0} : CollectionBase", this._className);
            writer.WriteLine("\t{");

            writer.WriteLine("\t\tpublic {0}()", this._className);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}({1}Collection list)", this._className, this._type.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach ({0} item in list)", this._type.Name);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.List.Add(new {0}(item));", _entityName);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0}Collection To{0}Collection()", this._type.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0}Collection retList = new {0}Collection();", this._type.Name);
            writer.WriteLine("\t\t\tforeach ({0} item in base.List)", this._entityName);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tretList.Add(item.To{0}());", this._type.Name);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\tretList.AcceptChanges();");
            writer.WriteLine("\t\t\treturn retList;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void Add({0} entity)", this._entityName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.List.Add(entity);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void Remove({0} entity)", this._entityName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tthis.List.Remove(entity);");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic void AddRange({0} list)", this._className);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tforeach ({0} item in list)", this._entityName);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis.List.Add(item);");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0} this[int index]", this._entityName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn ({0})base.List[index];", this._entityName);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tset");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tbase.List[index] = value;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }
    }
}
