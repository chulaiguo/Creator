using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace CodeGenerator.UIFactory
{
    public class ControlCode
    {
        private Type _type = null;
        private StringCollection _propertyList = null;

        public ControlCode(Type type)
        {
            this._type = type;
            this._propertyList = new StringCollection();
        }

        public string GenControlCode()
        {
            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            this.WriteUsing(writer);
            this.BeginWrite(writer);

            writer.WriteLine("\t\t #region Event Members");
            writer.WriteLine();
            this.WriteEvents(writer, this._type);
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();

            this.WriteConstructor(writer);

            writer.WriteLine("\t\t #region Property Members");
            writer.WriteLine();
            this.WriteProperties(writer, this._type);
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();


            writer.WriteLine("\t\t #region Register Event");
            writer.WriteLine();
            writer.WriteLine("\t\tprivate void RegisterEvents()");
            writer.WriteLine("\t\t{");
            this.WriteRegisterEvents(writer, this._type);
            writer.WriteLine("\t\t}");
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();

            writer.WriteLine("\t\t #region Event Methods");
            writer.WriteLine();
            this.WriteEventMethods(writer, this._type);
            writer.WriteLine("\t\t#endregion");
            writer.WriteLine();


            this.EndWrite(writer);

            return writer.ToString();
        }

        private void WriteUsing(StringWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.ComponentModel;");
            writer.WriteLine("using System.Drawing;");
            writer.WriteLine("using System.Drawing.Design;");

            writer.WriteLine();
        }

        private void BeginWrite(StringWriter writer)
        {
            writer.WriteLine("namespace Cheke.WinCtrl.Common");
            writer.WriteLine("{");
            writer.WriteLine("\t[ToolboxItem(true)]");
            writer.WriteLine("\t[ToolboxBitmap(typeof ({0}))]", this._type.Name);
            if(Attribute.IsDefined(this._type, typeof(DefaultEventAttribute)))
            {
                DefaultEventAttribute att =
                    Attribute.GetCustomAttribute(this._type, typeof (DefaultEventAttribute)) as DefaultEventAttribute;
                writer.WriteLine("\t[DefaultEvent(\"{0}\")]", att.Name);
            }
            if (Attribute.IsDefined(this._type, typeof(DefaultPropertyAttribute)))
            {
                DefaultPropertyAttribute att =
                    Attribute.GetCustomAttribute(this._type, typeof(DefaultPropertyAttribute)) as DefaultPropertyAttribute;

                writer.WriteLine("\t[DefaultProperty(\"{0}\")]", att.Name);
            }
            writer.WriteLine("\tpublic class {0}Ex: UserControl", this._type.Name);
            writer.WriteLine("\t{");
        }

        private void WriteConstructor(StringWriter writer)
        {
            writer.WriteLine("\t\tpublic {0}Ex()", this._type.Name);
            writer.WriteLine("\t\t{");
            writer.WriteLine("\t\t\tInitializeComponent();");
            writer.WriteLine("\t\t\tthis.RegisterEvents();");
            writer.WriteLine("\t\t}");
            writer.WriteLine();
        }

        private void WriteEvents(StringWriter writer, Type type)
        {
            EventInfo[] events = type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (EventInfo item in events)
            {
                if (Attribute.IsDefined(item, typeof(ObsoleteAttribute)))
                    continue;

                this.WriteAttributes(writer, item.GetCustomAttributes(true));

                if(this.IsUserControlEvent(item.Name))
                {
                     writer.WriteLine("\t\tpublic new event {0} {1};", item.EventHandlerType.Name, item.Name);
                }
                else
                {
                     writer.WriteLine("\t\tpublic event {0} {1};", item.EventHandlerType.Name, item.Name);
                }
            }

            if(type.BaseType != null && type.BaseType.Name != "BaseControl")
            {
                WriteEvents(writer, type.BaseType);
            }
        }

        private void WriteRegisterEvents(StringWriter writer, Type type)
        {
            EventInfo[] events = type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (EventInfo item in events)
            {
                if (Attribute.IsDefined(item, typeof(ObsoleteAttribute)))
                    continue;

                writer.WriteLine("\t\t\tthis.{0}.{1} += new {2}(this.{0}_{1});", this.GetVariant(), item.Name, item.EventHandlerType.FullName);
            }

            if (type.BaseType != null && type.BaseType.Name != "BaseControl")
            {
                WriteRegisterEvents(writer, type.BaseType);
            }
        }

        private void WriteEventMethods(StringWriter writer, Type type)
        {
            EventInfo[] events = type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (EventInfo item in events)
            {
                if (Attribute.IsDefined(item, typeof(ObsoleteAttribute)))
                    continue;

                string args = item.EventHandlerType.Name.Replace("Handler", "Args");
                writer.WriteLine("\t\tprivate void {0}_{1}(object sender, {2} e)", this.GetVariant(), item.Name, args);
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tif (this.{0} != null)", item.Name);
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tthis.{0}(sender, e);", item.Name);
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                writer.WriteLine();
            }

            if (type.BaseType != null && type.BaseType.Name != "BaseControl")
            {
                WriteEventMethods(writer, type.BaseType);
            }
        }

        private void WriteProperties(StringWriter writer, Type type)
        {
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo item in properties)
            {
                if (Attribute.IsDefined(item, typeof(ObsoleteAttribute)))
                    continue;

                if(type != this._type && this._propertyList.Contains(item.Name))
                {
                    continue;
                }

                this._propertyList.Add(item.Name);
                this.WriteAttributes(writer, item.GetCustomAttributes(true));
                if (IsUserControlProperty(item.Name))
                {
                    if(item.Name == "Text")
                    {
                        writer.WriteLine("\t\tpublic override {0} {1}", item.PropertyType.Name, item.Name);
                    }
                    else
                    {
                        writer.WriteLine("\t\tpublic new {0} {1}", item.PropertyType.Name, item.Name);
                    }
                }
                else
                {
                    writer.WriteLine("\t\tpublic {0} {1}", item.PropertyType.Name, item.Name);
                }
                writer.WriteLine("\t\t{");
                if (item.CanRead)
                {
                    writer.WriteLine("\t\t\tget");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\treturn this.{0}.{1};", this.GetVariant(), item.Name);
                    writer.WriteLine("\t\t\t}");
                }
                if(item.CanWrite)
                {
                    writer.WriteLine("\t\t\tset");
                    writer.WriteLine("\t\t\t{");
                    writer.WriteLine("\t\t\t\tthis.{0}.{1} = value;", this.GetVariant(), item.Name);
                    writer.WriteLine("\t\t\t}");
                }
                writer.WriteLine("\t\t}");

                writer.WriteLine();
            }

            if (type.BaseType != null && type.BaseType.Name != "BaseControl")
            {
                WriteProperties(writer, type.BaseType);
            }
        }

        private void EndWrite(StringWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        #region helper function
        private string GetVariant()
        {
            return string.Format("{0}{1}1", char.ToLower(this._type.Name[0]), this._type.Name.Substring(1));
        }

        private void WriteAttributes(StringWriter writer, object[] attributes)
        {
            foreach (object item in attributes)
            {
                if (item.GetType() == typeof(CategoryAttribute))
                {
                    CategoryAttribute att = item as CategoryAttribute;
                    writer.WriteLine("\t\t[Category(\"{0}\")]", att.Category);
                }
                else if (item.GetType() == typeof(BrowsableAttribute))
                {
                    BrowsableAttribute att = item as BrowsableAttribute;
                    writer.WriteLine("\t\t[Browsable({0})]", this.GetBooleanString(att.Browsable));
                }
                else if (item.GetType() == typeof(DescriptionAttribute))
                {
                    DescriptionAttribute att = item as DescriptionAttribute;
                    writer.WriteLine("\t\t[Description(\"{0}\")]", att.Description);
                }
                else if (item.GetType() == typeof(RefreshPropertiesAttribute))
                {
                    RefreshPropertiesAttribute att = item as RefreshPropertiesAttribute;
                    writer.WriteLine("\t\t[RefreshProperties(RefreshProperties.{0})]", att.RefreshProperties);
                }
                else if (item.GetType() == typeof(EditorAttribute))
                {
                    EditorAttribute att = item as EditorAttribute;
                    string editTypeName = string.Format("\"{0}\"", att.EditorTypeName);
                    string dditorBaseTypeName = string.Format("\"{0}\"", att.EditorBaseTypeName);

                    Type editType = Type.GetType(att.EditorTypeName);
                    Type editBaseType = Type.GetType(att.EditorBaseTypeName);
                    if(editType != null)
                    {
                        editTypeName = string.Format("typeof({0})", editType.Name);
                    }
                    if(editBaseType != null)
                    {
                        dditorBaseTypeName = string.Format("typeof({0})", editBaseType.Name);
                    }

                    writer.WriteLine("\t\t[Editor({0}, {1})]", editTypeName, dditorBaseTypeName);
                }
                else if (item.GetType() == typeof(DesignerSerializationVisibilityAttribute))
                {
                    DesignerSerializationVisibilityAttribute att = item as DesignerSerializationVisibilityAttribute;
                    writer.WriteLine("\t\t[DesignerSerializationVisibility(DesignerSerializationVisibility.{0})]", att.Visibility);
                }
                else if (item.GetType() == typeof(LocalizableAttribute))
                {
                    LocalizableAttribute att = item as LocalizableAttribute;
                    writer.WriteLine("\t\t[Localizable({0})]", this.GetBooleanString(att.IsLocalizable));
                }
                else if (item.GetType() == typeof(DefaultValueAttribute))
                {
                    DefaultValueAttribute att = item as DefaultValueAttribute;
                    if (att.Value == null)
                    {
                        writer.WriteLine("\t\t[DefaultValue(null)]");
                    }
                    else
                    {
                        if (att.Value is bool)
                        {
                            writer.WriteLine("\t\t[DefaultValue({0})]", this.GetBooleanString((bool) att.Value));
                        }
                        else if (att.Value.GetType().IsEnum)
                        {
                            writer.WriteLine("\t\t[DefaultValue({0}.{1})]", att.Value.GetType().Name, att.Value);
                        }
                        else
                        {
                            writer.WriteLine("\t\t[DefaultValue({0})]", att.Value);
                        }
                    }
                }
                else if (item.GetType() == typeof(BindableAttribute))
                {
                    BindableAttribute att = item as BindableAttribute;
                    writer.WriteLine("\t\t[Bindable({0})]", this.GetBooleanString(att.Bindable));
                }
                else if (item.GetType() == typeof(AttributeProviderAttribute))
                {
                    AttributeProviderAttribute att = item as AttributeProviderAttribute;
                    writer.WriteLine("\t\t[AttributeProvider(typeof ({0}))]", att.TypeName);
                }
                else if (item.GetType() == typeof(TypeConverterAttribute))
                {
                    TypeConverterAttribute att = item as TypeConverterAttribute;
                    string converterTypeName = string.Format("\"{0}\"", att.ConverterTypeName);
                    Type converterType = Type.GetType(att.ConverterTypeName);
                    if(converterType != null)
                    {
                        converterTypeName = string.Format("typeof({0})", converterType.Name);
                    }
                    writer.WriteLine("\t\t[TypeConverter({0})]", converterTypeName);
                }
                else if (item.GetType() == typeof(MergablePropertyAttribute))
                {
                    MergablePropertyAttribute att = item as MergablePropertyAttribute;
                    writer.WriteLine("\t\t[MergableProperty({0})]", this.GetBooleanString(att.AllowMerge));
                }
                else if (item.GetType() == typeof(EditorBrowsableAttribute))
                {
                    EditorBrowsableAttribute att = item as EditorBrowsableAttribute;
                    writer.WriteLine("\t\t[EditorBrowsable(EditorBrowsableState.{0})]", att.State);
                }
                else
                {
                    writer.WriteLine("\t\t[{0}]", item);
                }
            }
        }

        private string GetBooleanString(bool boolean)
        {
            if (boolean)
                return "true";
            else
                return "false";
        }

        private bool IsUserControlEvent(string eventName)
        {
            EventInfo[] events = typeof(UserControl).GetEvents(BindingFlags.Public | BindingFlags.Instance);
            foreach (EventInfo item in events)
            {
               if(item.Name == eventName)
                   return true;
            }

            return false;
        }

        private bool IsUserControlProperty(string propertyName)
        {
           PropertyInfo[] properties = typeof(UserControl).GetProperties(BindingFlags.Public | BindingFlags.Instance);
           foreach (PropertyInfo item in properties)
           {
               if(item.Name == propertyName)
                   return true;
           }

            return false;
        }
        #endregion
    }
}