using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.UIFactory
{
    public class GridDecoratorCode
    {
        private Type _type = null;
        private string _projectName = string.Empty;

        private string _entityName = string.Empty;
        private Dictionary<string, Type> _parentList = null;
        private Dictionary<string, Type> _childList = null;

        public GridDecoratorCode(Type type, string projectName, bool hasChildren)
        {
            this._type = type;
            this._projectName = projectName;

            this._entityName = this._type.Name.Substring(0, this._type.Name.Length - 4);

            if (!hasChildren)
            {
                this._parentList = new Dictionary<string, Type>();
                this._childList = new Dictionary<string, Type>();
            }
            else
            {
                this._parentList = this.GetParents(this._type);
                this._childList = this.GetChildren(this._type);
            }
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);
            this.WriteParentVar(writer);
            this.WriteConstruct(writer);
            this.WriteParentPropertry(writer);
            this.WriteDisplayColumns(writer, this._type, string.Empty);
            this.WriteEntitySettingDictionary(writer);

            if(this._childList.Count > 0)
            {
                writer.WriteLine();
                this.WriteInitNewEntity(writer);
                writer.WriteLine();

                this.WriteSetupDetailViewTree(writer);
                writer.WriteLine();
                if (this._childList.Count > 1)
                {
                    this.WriteMasterRowGetRelationCount(writer);
                    writer.WriteLine();
                }
                this.WriteMasterRowGetChildList(writer);
                writer.WriteLine();
                this.WriteMasterRowGetRelationName(writer);
            }
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using {0}.ViewObj;", this._projectName);
            writer.WriteLine("using {0}.Schema;", this._projectName);
            writer.WriteLine("using {0}.WinUI.FormDetailEditor;", this._projectName);
            writer.WriteLine("using Cheke.WinCtrl.Decoration;");
            writer.WriteLine("using DevExpress.Utils;");
            writer.WriteLine("using DevExpress.XtraGrid;");
            writer.WriteLine("using DevExpress.XtraGrid.Columns;");
            writer.WriteLine("using DevExpress.XtraGrid.Views.Grid;");

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.WinUI.GridDecorator", this._projectName);
            writer.WriteLine("{");
            if (this._childList.Count == 0)
            {
                writer.WriteLine("\tpublic class Grid{0}Decorator : GridControlDecorator", this._entityName); 
            }
            else
            {
                writer.WriteLine("\tpublic class Grid{0}Decorator : GridMasterDetailDecorator", this._entityName);
            }
            writer.WriteLine("\t{");
        }

        private void WriteParentVar(StringWriter writer)
        {
            if (this._parentList.Count == 0)
                return;

            foreach (KeyValuePair<string, Type> item in this._parentList)
            {
                writer.WriteLine("\t\tprivate {0} _{1} = null;", item.Value.Name, this.LowerFirstChar(item.Key));
            }
            writer.WriteLine();
        }

        private void WriteConstruct(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic Grid{0}Decorator(string userId, GridControl gridControl)", this._entityName);
            writer.WriteLine("\t\t\t: base(userId, gridControl)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteParentPropertry(StringWriter writer)
        {
            if (this._parentList.Count == 0)
                return;

            foreach (KeyValuePair<string, Type> item in this._parentList)
            {
                writer.WriteLine("\t\tpublic {0} {1}", item.Value.Name, item.Key);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tget");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\treturn this._{0};", this.LowerFirstChar(item.Key));
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\tset");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis._{0} = value;", this.LowerFirstChar(item.Key));
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
            }
            writer.WriteLine();
        }

        private void WriteDisplayColumns(StringWriter writer, Type type, string functionName)
        {
            if(functionName.Length == 0)
            {
                writer.WriteLine("\t\tprotected override void SetDisplayColumns(GridView view)");
            }
            else
            {
                writer.WriteLine("\t\tprivate void {0}(GridView view)", functionName);
            }
            
            writer.WriteLine("\t\t{");

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public |
                                               BindingFlags.DeclaredOnly | BindingFlags.Instance);
            foreach (PropertyInfo info in properties)
            {
                if (info.Name == "IsDirty" || info.Name == "IsValid" || info.Name == "PKString"
                    || info.Name == "MarkAsDeleted" || info.Name == "TableName")
                    continue;

                if (info.PropertyType == typeof(Guid))
                    continue;

                if (!info.PropertyType.IsValueType && info.PropertyType != typeof(string))
                    continue;

                writer.WriteLine("\t\t\tGridColumn col{0} = new GridColumn();", info.Name);
                writer.WriteLine("\t\t\tcol{0}.Caption = \"{0}\";", info.Name);
                writer.WriteLine("\t\t\tcol{0}.FieldName = {1}Schema.{0};", info.Name,
                                 type.Name.Substring(0, type.Name.Length - 4));
                if (info.Name == "CreatedBy"
                    || info.Name == "ModifiedBy"
                    || info.Name == "LastModifiedBy")
                {
                    writer.WriteLine("\t\t\tcol{0}.VisibleIndex = -1;", info.Name);
                    writer.WriteLine("\t\t\tcol{0}.OptionsColumn.ShowInCustomizationForm = false;", info.Name);
                }
                else if (info.Name == "CreatedOn"
                     || info.Name == "ModifiedOn"
                     || info.Name == "LastModifiedAt")
                {
                    writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatType = FormatType.DateTime;", info.Name);
                    writer.WriteLine("\t\t\tcol{0}.DisplayFormat.FormatString = \"MM/dd/yyyy HH:mm:ss\";", info.Name);
                    writer.WriteLine("\t\t\tcol{0}.VisibleIndex = -1;", info.Name);
                    writer.WriteLine("\t\t\tcol{0}.OptionsColumn.ShowInCustomizationForm = false;", info.Name);
                }
                else
                {
                    writer.WriteLine("\t\t\tcol{0}.OptionsColumn.AllowEdit = false;", info.Name);
                    writer.WriteLine("\t\t\tcol{0}.OptionsColumn.AllowFocus = false;", info.Name);
                    writer.WriteLine("\t\t\tcol{0}.VisibleIndex = view.Columns.Count;", info.Name);
                }
                writer.WriteLine("\t\t\tview.Columns.Add(col{0});", info.Name);
                writer.WriteLine();
            }


            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteInitNewEntity(StringWriter writer)
        {
            string varName = this.LowerFirstChar(this._entityName);
            writer.WriteLine("\t\tprotected override void InitNewEntity(GridView view, BusinessBase entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0} {1} = entity as {0};", this._type.Name, varName);
            writer.WriteLine("\t\t\tif ({0} != null)", varName);
            writer.WriteLine("\t\t\t{");
            foreach (KeyValuePair<string, Type> item in this._parentList)
            {
                writer.WriteLine("\t\t\t\tif (this.{0} != null)", item.Key);
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t{0}.{1} = this.{1};", varName, item.Key);
                StringCollection list = this.GetSameParent(this._type, item.Value);
                foreach (string s in list)
                {
                    writer.WriteLine("\t\t\t\t\t{0}.{2} = this.{1}.{2};", varName, item.Key, s);
                }
                writer.WriteLine("\t\t\t\t}");

                writer.WriteLine();
            }
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine("\t\t\t}");

            if (this._childList.Count > 0)
            {
                writer.WriteLine();
                writer.WriteLine("\t\t\t//Master Detail");
                foreach (KeyValuePair<string, Type> item in this._childList)
                {
                    string varChildName = this.GetVarName(item.Value);
                    writer.WriteLine("\t\t\t{0} {1} = entity as {0};", item.Value.Name, varChildName);
                    writer.WriteLine("\t\t\tif ({0} != null)", varChildName);
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tGridView parentView = view.ParentView as GridView;");
                    writer.WriteLine("\t\t\t\tif (parentView == null)");
                    writer.WriteLine("\t\t\t\t\treturn;");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\t\t{0} parent = parentView.GetRow(parentView.FocusedRowHandle) as {0};", this._type.Name);
                    writer.WriteLine("\t\t\t\tif (parent == null)");
                    writer.WriteLine("\t\t\t\t\treturn;");
                    writer.WriteLine();
                    writer.WriteLine("\t\t\t\t{0}.{1} = parent;", varChildName, item.Key.Substring(0, item.Key.Length - 4));
                    StringCollection list = this.GetSameParent(this._type, item.Value);
                    foreach (string s in list)
                    {
                        writer.WriteLine("\t\t\t\t{0}.{2} = this.{1}.{2};", varName, item.Key, s);
                    }
                    writer.WriteLine();
                    writer.WriteLine("\t\t\t\treturn;");
                    writer.WriteLine("\t\t\t}");
                    writer.WriteLine();
                }
            }

            writer.WriteLine("\t\t}");

            writer.WriteLine();
        }

        private void WriteEntitySettingDictionary(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override Dictionary<Type, EntitySetting> EntitySettingDictionary");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tDictionary<Type, EntitySetting> list = new Dictionary<Type, EntitySetting>();");
            writer.WriteLine("\t\t\t\tlist.Add(typeof ({0}), new EntitySetting(typeof (FormDetail{0})));", this._entityName);
            foreach (KeyValuePair<string, Type> item in _childList)
            {
                writer.WriteLine("\t\t\t\tlist.Add(typeof ({0}), new EntitySetting(typeof (FormDetail{0})));", item.Value.Name.Substring(0, item.Value.Name.Length - 4));
            }
            writer.WriteLine();
            writer.WriteLine("\t\t\t\treturn list;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        private void WriteSetupDetailViewTree(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void SetupDetailViewTree()");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tGridLevelNode node;");
            foreach (KeyValuePair<string, Type> item in _childList)
            {
                string varName = item.Value.Name.Substring(0, item.Value.Name.Length - 4);
                writer.WriteLine("\t\t\tnode = base.AddLevelTreeNode({0}Schema.TableName, \"{0}\", false);", varName);
                writer.WriteLine("\t\t\tthis.Setup{0}Columns(node.LevelTemplate as GridView);", varName);
                writer.WriteLine();
            }
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            foreach (KeyValuePair<string, Type> item in _childList)
            {
                string varName = item.Value.Name.Substring(0, item.Value.Name.Length - 4);
                string functionName = string.Format("Setup{0}Columns", varName);
                this.WriteDisplayColumns(writer, item.Value, functionName);
            }
        }

        private void WriteMasterRowGetRelationCount(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void GridView_MasterRowGetRelationCount(object sender, MasterRowGetRelationCountEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\te.RelationCount = {0};", this._childList.Count);
            writer.WriteLine("\t\t}");
        }

        private void WriteMasterRowGetChildList(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void GridView_MasterRowGetChildList(object sender, MasterRowGetChildListEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tGridView view = sender as GridView;");
            writer.WriteLine("\t\t\tif (view == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();
            writer.WriteLine("\t\t\t{0} entity = view.GetRow(e.RowHandle) as {0};", this._type.Name);
            writer.WriteLine("\t\t\tif (entity == null)");
            writer.WriteLine("\t\t\t\treturn;");
            writer.WriteLine();
            writer.WriteLine("\t\t\tswitch (e.RelationIndex)");
            writer.WriteLine("\t\t\t{");
            int i = 0;
            foreach (KeyValuePair<string, Type> item in _childList)
            {
                writer.WriteLine("\t\t\t\tcase {0}:", i);
                writer.WriteLine("\t\t\t\t\tif (entity.{0} == null)", item.Key);
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tentity.Get{0}();", item.Key);
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\te.ChildList = entity.{0};", item.Key);
                writer.WriteLine("\t\t\t\t\tbreak;");

                i++;
            }
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        private void WriteMasterRowGetRelationName(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void GridView_MasterRowGetRelationName(object sender, MasterRowGetRelationNameEventArgs e)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tswitch (e.RelationIndex)");
            writer.WriteLine("\t\t\t{");
            int i = 0;
            foreach (KeyValuePair<string, Type> item in _childList)
            {
                string varName = item.Value.Name.Substring(0, item.Value.Name.Length - 4);
                writer.WriteLine("\t\t\t\tcase {0}:", i);
                writer.WriteLine("\t\t\t\t\te.RelationName = {0}Schema.TableName;", varName);
                writer.WriteLine("\t\t\t\t\tbreak;");

                i++;
            }
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        private Dictionary<string, Type> GetParents(Type type)
        {
            Dictionary<string, Type> list = new Dictionary<string, Type>();

            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo item in properties)
            {
                if (!item.PropertyType.IsSubclassOf(type.BaseType))
                    continue;

                list.Add(item.Name, item.PropertyType);
            }

            return list;
        }

        private Dictionary<string, Type> GetChildren(Type type)
        {
            Dictionary<string, Type> list = new Dictionary<string, Type>();

            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo item in properties)
            {
                if (!item.PropertyType.Name.EndsWith("Collection"))
                    continue;

                Type childType =
                    this.GetTypeByName(item.PropertyType.Name.Substring(0, item.PropertyType.Name.Length - 10));
                if (childType != null)
                {
                    list.Add(item.Name, childType);
                }
            }

            return list;
        }

        private StringCollection GetSameParent(Type childType, Type parentType)
        {
            StringCollection list = new StringCollection();

            PropertyInfo[] childProperties = childType.GetProperties();
            PropertyInfo[] parentProperties = parentType.GetProperties();
            foreach (PropertyInfo parent in parentProperties)
            {
                if (!parent.PropertyType.IsSubclassOf(parentType.BaseType))
                    continue;

                foreach (PropertyInfo child in childProperties)
                {
                    if (child.Name == parent.Name)
                    {
                        list.Add(parent.Name);
                        break;
                    }
                }
            }

            return list;
        }

        private Type GetTypeByName(string typeName)
        {
            Type[] types = this._type.Assembly.GetTypes();
            foreach (Type item in types)
            {
                if(item.Name == typeName)
                    return item;
            }

            return null;
        }

        private string LowerFirstChar(string text)
        {
            return text.Substring(0, 1).ToLower() + text.Substring(1);
        }

        private string GetVarName(Type type)
        {
            return type.Name.Substring(0, 1).ToLower() + type.Name.Substring(1, type.Name.Length - 5);
        }
    }
}