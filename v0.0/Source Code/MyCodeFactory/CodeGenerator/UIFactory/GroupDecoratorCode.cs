using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeGenerator.UIFactory
{
    public class GroupDecoratorCode
    {
        private string _projectName = string.Empty;
        private Type _leftType = null;
        private Type _rightType = null;
        private Type _parentType = null;

        private string _leftTypePK = string.Empty;

        private string _parentObjName = string.Empty;
        private string _leftObjName = string.Empty;
        private string _rightObjName = string.Empty;

        private string _parentVar = string.Empty;
        private string _leftVar = string.Empty;
        private string _rightVar = string.Empty;

        private string _className = string.Empty;

        public GroupDecoratorCode(Type leftType, Type rightType, Type parentType, string leftTypePK, string projectName)
        {
            this._projectName = projectName;
            this._rightType = rightType;
            this._leftType = leftType;
            this._leftTypePK = leftTypePK;

            this._parentType = parentType;

            this._parentObjName = this._parentType.Name.Substring(0, this._parentType.Name.Length - 4);
            this._leftObjName = this._leftType.Name.Substring(0, this._leftType.Name.Length - 4);
            this._rightObjName = this._rightType.Name.Substring(0, this._rightType.Name.Length - 4);

            this._parentVar = string.Format("{0}{1}", this._parentObjName.Substring(0, 1).ToLower(), this._parentObjName.Substring(1));
            this._leftVar = string.Format("{0}{1}", this._leftObjName.Substring(0, 1).ToLower(), this._leftObjName.Substring(1));
            this._rightVar = string.Format("{0}{1}", this._rightObjName.Substring(0, 1).ToLower(), this._rightObjName.Substring(1));

            this._className = string.Format("Group{0}{1}Decorator", this._parentObjName, this._leftObjName);
        }

        public string GenCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);
            this.WriteConstruct(writer);
            this.WriteLeftColumns(writer);
            this.WriteRightColumns(writer);
            this.WriteCompareEntity(writer);
            this.WriteCreateRightEntity(writer);
            this.WriteDetailFormType(writer);
            this.WriteReplaceEntity(writer);
            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using Cheke.BusinessEntity;");
            writer.WriteLine("using Cheke.WinCtrl.Decoration;");
            writer.WriteLine("using Cheke.WinCtrl.GridGroupBuddy;");
            writer.WriteLine("using DevExpress.Utils;");
            writer.WriteLine("using DevExpress.XtraGrid.Columns;");
            writer.WriteLine("using DevExpress.XtraGrid.Views.Grid;");
            writer.WriteLine("using {0}.ViewObj;", this._projectName);
            writer.WriteLine("using {0}.Schema;", this._projectName);
            writer.WriteLine("using {0}.WinUI.FormDetailEditor;", this._projectName);

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace {0}.WinUI.GroupDecorator", this._projectName);
            writer.WriteLine("{");
            writer.WriteLine("\tpublic class {0} : GridGroupControlDecorator", this._className);
            writer.WriteLine("\t{");
        }

        private void WriteConstruct(StringWriter writer)
        {
            writer.WriteLine("\t\tprivate {0} _{1} = null;", this._parentObjName, this._parentVar);
            writer.WriteLine();
            writer.WriteLine("\t\tpublic {0}(string userId, GridGroupControl gridGroupControl)", this._className);
            writer.WriteLine("\t\t\t: base(userId, gridGroupControl)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t}");
            writer.WriteLine();

            writer.WriteLine("\t\tpublic {0} {0}", this._parentObjName);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn this._{0};", this._parentVar);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t\tset");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\tthis._{0} = value;", this._parentVar);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteLeftColumns(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void SetLeftDisplayColumns(GridView view)");
            writer.WriteLine("\t\t{");

            PropertyInfo[] properties = this._leftType.GetProperties(BindingFlags.Public |
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

                writer.WriteLine();
                writer.WriteLine("\t\t\tGridColumn col{0} = new GridColumn();", info.Name);
                writer.WriteLine("\t\t\tcol{0}.Caption = \"{0}\";", info.Name);
                writer.WriteLine("\t\t\tcol{0}.FieldName = {1}Schema.{0};", info.Name,
                                 this._leftType.Name.Substring(0, this._leftType.Name.Length - 4));
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

            }

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteRightColumns(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override void SetRightDisplayColumns(GridView view)");
            writer.WriteLine("\t\t{");

            PropertyInfo[] properties = this._rightType.GetProperties(BindingFlags.Public |
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

                writer.WriteLine();
                writer.WriteLine("\t\t\tGridColumn col{0} = new GridColumn();", info.Name);
                writer.WriteLine("\t\t\tcol{0}.Caption = \"{0}\";", info.Name);
                writer.WriteLine("\t\t\tcol{0}.FieldName = {1}Schema.{0};", info.Name,
                                 this._rightType.Name.Substring(0, this._rightType.Name.Length - 4));
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

            }

            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteCompareEntity(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override bool CompareEntity(BusinessBase left, BusinessBase right)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0} {1} = left as {0};", this._leftObjName, this._leftVar);
            writer.WriteLine("\t\t\t{0} {1} = right as {0};", this._rightObjName, this._rightVar);
            writer.WriteLine("\t\t\tif({0} == null || {1} == null)", this._leftVar, this._rightVar);
            writer.WriteLine("\t\t\t\treturn false;");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn {0}.{2} == {1}.{2};", this._leftVar, this._rightVar, this._leftTypePK);
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteCreateRightEntity(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override BusinessBase CreateRightEntity(BusinessBase leftEntity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\t{0} {1} = leftEntity as {0};", this._leftObjName, this._leftVar);
            writer.WriteLine("\t\t\tif({0} == null || this.{1} == null)", this._leftVar, this._parentObjName);
            writer.WriteLine("\t\t\t\treturn null;");
            writer.WriteLine();
            if (this.HasMarkasDeleted(this._rightType))
            {
                writer.WriteLine("\t\t\t{0} right = {0}.GetByPK();//Todo:", this._rightObjName);
                writer.WriteLine("\t\t\tif (right == null)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tright = new {0}();", this._rightObjName);

                writer.WriteLine("\t\t\t\tright.CopyParent({0});", this._leftVar);
                writer.WriteLine("\t\t\t\tright.CopyParent({0});", this._parentObjName);

                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\telse");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tright.MarkAsDeleted = false;");
                writer.WriteLine("\t\t\t}");
            }
            else
            {
                writer.WriteLine("\t\t\t{0} right = new {0}();", this._rightObjName);
                writer.WriteLine("\t\t\t\tright.CopyParent({0});", this._leftVar);
                writer.WriteLine("\t\t\t\tright.CopyParent({0});", this._parentObjName);
            }
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn right;");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteDetailFormType(StringWriter writer)
        {
            writer.WriteLine("\t\tprotected override Type DetailFormType");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tget");
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\treturn typeof (FormDetail{0});", this._leftObjName);
            writer.WriteLine("\t\t\t}");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteReplaceEntity(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic override bool ReplaceData(BusinessBase entity)");
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tif (entity.GetType() == typeof({0}))", this._rightObjName);
            writer.WriteLine("\t\t\t{");
            writer.WriteLine("\t\t\t\t{0} right = entity as {0};", this._rightObjName);
            writer.WriteLine("\t\t\t\tif (right == null || right.{0}PK != this.{0}.{0}PK)", this._parentObjName);
            writer.WriteLine("\t\t\t\t\treturn true;");
            writer.WriteLine("\t\t\t}");
            writer.WriteLine();
            writer.WriteLine("\t\t\treturn base.ReplaceData(entity);");
            writer.WriteLine("\t\t}");
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        private bool HasMarkasDeleted(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo item in properties)
            {
                if(item.Name == "MarkAsDeleted")
                    return true;
            }

            return false;
        }

        private StringCollection GetSameParent(Type rightType, Type parentType)
        {
            StringCollection list = new StringCollection();

            PropertyInfo[] childProperties = rightType.GetProperties();
            PropertyInfo[] parentProperties = parentType.GetProperties();
            foreach (PropertyInfo parent in parentProperties)
            {
                if (!parent.PropertyType.IsSubclassOf(parentType.BaseType))
                    continue;

                foreach (PropertyInfo child in childProperties)
                {
                    if(child.Name == parent.Name)
                    {
                        list.Add(parent.Name);
                        break;
                    }
                }
            }

            return list;
        }
    }
}