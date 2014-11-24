using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using CodeGenerator.ServiceFactory;
using CodeGenerator.TestFactory;
using CodeGenerator.UIFactory;
using CodeGenerator.DataServiceExFactory;

namespace CodeGenerator
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = "D3000";
        }

        #region UI code
        private void btnControlCode_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("DevExpress.XtraEditors.v7.2.dll"));
            //Assembly assembly = this.GetAssembly(string.Format("DevExpress.XtraGrid.v7.2.dll"));
            if (assembly == null)
                return;

            FormControlName dlg = new FormControlName();
            if(DialogResult.OK != dlg.ShowDialog())
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if(item.Name != dlg.SelectedType)
                        continue;

                    string strPath = string.Format(@"F:\Tools\Creator\{0}.cs", item.Name);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        ControlCode service = new ControlCode(item);
                        writer.Write(service.GenControlCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnColumnsBuilder_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\ColumnsBuilder.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    ColumnsBuilderCode serviceFactory = new ColumnsBuilderCode(assembly, this.ProjectName);
                    writer.Write(serviceFactory.GenBuilderCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        private void btnLookupBuilder_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\LookUpEditBuilder.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    LookupBuilderCode factory = new LookupBuilderCode(assembly, this.ProjectName);
                    writer.Write(factory.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnDataBinding_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string strPath = string.Format(@"F:\Tools\Creator\{0}Binding.cs", item.Name);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        DataBindingCode dataBinding = new DataBindingCode(item);
                        writer.Write(dataBinding.GenDataBindingCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnGridDecorator_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string strPath = string.Format(@"F:\Tools\Creator\Grid{0}Decorator.cs", item.Name.Substring(0, item.Name.Length - 4));

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        GridDecoratorCode code = new GridDecoratorCode(item, this.ProjectName, this.chkHasChildren.Checked);
                        writer.Write(code.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnGroupDecorator_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            FormGroupDecorator dlg = new FormGroupDecorator(assembly);
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            if (dlg.LeftType == null || dlg.RightType == null
                || dlg.ParentType == null || dlg.LeftTypePK.Length == 0)
                return;

            string fileName = string.Format("Group{0}{1}Decorator",
                                            dlg.ParentType.Name.Substring(0, dlg.ParentType.Name.Length - 4),
                                            dlg.LeftType.Name.Substring(0, dlg.LeftType.Name.Length - 4));

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\{0}.cs", fileName);

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    GroupDecoratorCode code = new GroupDecoratorCode(dlg.LeftType, dlg.RightType, dlg.ParentType, dlg.LeftTypePK, this.ProjectName);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnDetailEditor_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string entityName = item.Name.Substring(0, item.Name.Length - 4);

                    string strPath = string.Format(@"F:\Tools\Creator\FormDetail{0}.cs", entityName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        DetailEditorCode code = new DetailEditorCode(item, this.ProjectName);
                        writer.Write(code.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnDetailEditorDesign_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string entityName = item.Name.Substring(0, item.Name.Length - 4);

                    string strPath = string.Format(@"F:\Tools\Creator\FormDetail{0}.Designer.cs", entityName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        DetailEditorDesignCode code = new DetailEditorDesignCode(item, this.ProjectName);
                        writer.Write(code.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnWorkList_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string entityName = item.Name.Substring(0, item.Name.Length - 4);

                    string strPath = string.Format(@"F:\Tools\Creator\FormWork{0}List.cs", entityName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        WorkListCode code = new WorkListCode(item, this.ProjectName);
                        writer.Write(code.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnWorkListDesign_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string entityName = item.Name.Substring(0, item.Name.Length - 4);

                    string strPath = string.Format(@"F:\Tools\Creator\FormWork{0}List.Designer.cs", entityName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        WorkListDesignCode code = new WorkListDesignCode(item, this.ProjectName);
                        writer.Write(code.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnWorkEditor_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string entityName = item.Name.Substring(0, item.Name.Length - 4);

                    string strPath = string.Format(@"F:\Tools\Creator\FormWork{0}.cs", entityName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        WorkEditorCode code = new WorkEditorCode(item, this.ProjectName);
                        writer.Write(code.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnWorkEditorDesign_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string entityName = item.Name.Substring(0, item.Name.Length - 4);

                    string strPath = string.Format(@"F:\Tools\Creator\FormWork{0}.Designer.cs", entityName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        WorkEditorDesignCode code = new WorkEditorDesignCode(item, this.ProjectName);
                        writer.Write(code.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }


        private void btnWorkSearch_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string entityName = item.Name.Substring(0, item.Name.Length - 4);

                    string strPath = string.Format(@"F:\Tools\Creator\FormWork{0}Search.cs", entityName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        WorkSearchCode code = new WorkSearchCode(item, this.ProjectName);
                        writer.Write(code.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnWorkSearchDesign_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string entityName = item.Name.Substring(0, item.Name.Length - 4);

                    string strPath = string.Format(@"F:\Tools\Creator\FormWork{0}Search.Designer.cs", entityName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        WorkSearchDesignCode code = new WorkSearchDesignCode(item, this.ProjectName);
                        writer.Write(code.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        #endregion

        #region Test code
        private void btnDataBuilder_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.FileName = "xxx.xls";
            if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                string[] paths = this.openFileDialog1.FileName.Split('\\');
                string name = paths[paths.Length - 1];
                string strPath = string.Format(@"F:\Tools\Creator\{0}Builder.cs", name.Substring(0, name.Length - 4));

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    DataBuilderCode builder = new DataBuilderCode(this.openFileDialog1.FileName, this.ProjectName);
                    writer.Write(builder.GenBuilderCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnDataBuilderFixture_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.FileName = "xxx.xls";
            if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                string[] paths = this.openFileDialog1.FileName.Split('\\');
                string name = paths[paths.Length - 1];
                string strPath = string.Format(@"F:\Tools\Creator\{0}BuilderFixture.cs", name.Substring(0, name.Length - 4));

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    DataBuilderFixture fixture = new DataBuilderFixture(this.openFileDialog1.FileName, this.ProjectName);
                    writer.Write(fixture.GenBuilderCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnDataBuilderBase_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\BuilderBase.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    DataBuilderBaseCode builder = new DataBuilderBaseCode(assembly, this.ProjectName);
                    writer.Write(builder.GenBuilderCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnBuilderFixtureBase_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\FixtureBase.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    DataBuilderFixtureBase fixture = new DataBuilderFixtureBase(this.ProjectName);
                    writer.Write(fixture.GenBuilderCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnTestDataConfig_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.Description = "the folder of xx.xls";
            if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\App.config");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    TestDataConfigCode code = new TestDataConfigCode(this.folderBrowserDialog1.SelectedPath, this.ProjectName);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnTestData_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\Program.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    TestDataProgramCode code = new TestDataProgramCode(this.ProjectName);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnDataServiceBuilder_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\ServiceBuilder.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    DataServiceBuilderCode dataServiceFactory = new DataServiceBuilderCode(assembly, this.ProjectName);
                    writer.Write(dataServiceFactory.GenBuilderCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        #endregion

        #region Service code
        private void btnServiceBase_Click(object sender, EventArgs e)
        {
            string strPath = string.Format(@"F:\Tools\Creator\ServiceBase.cs");

            FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fs.SetLength(0);
            using (StreamWriter writer = new StreamWriter(fs))
            {
                ServiceBaseCode service = new ServiceBaseCode(this.ProjectName);
                writer.Write(service.GenServiceBaseCode());
            }
        }

        private void btnGenIService_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Service"))
                        continue;

                    string serviceName = item.Name.Substring(0, item.Name.Length - 11) + "Service";
                    string strPath = string.Format(@"F:\Tools\Creator\IGen{0}.cs", serviceName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        IGenServiceCode service = new IGenServiceCode(item, this.ProjectName);
                        writer.Write(service.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnGenIFactory_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\IGenServiceFactory.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    IGenServiceFactoryCode serviceFactory = new IGenServiceFactoryCode(assembly, this.ProjectName);
                    writer.Write(serviceFactory.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnGenService_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Service"))
                        continue;

                    string serviceName = item.Name.Substring(0, item.Name.Length - 11) + "Service";
                    string strPath = string.Format(@"F:\Tools\Creator\Gen{0}.cs", serviceName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        GenServiceCode service = new GenServiceCode(item, this.ProjectName);
                        writer.Write(service.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnGenFactory_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\GenServiceFactory.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    GenServiceFactoryCode serviceFactory = new GenServiceFactoryCode(assembly, this.ProjectName);
                    writer.Write(serviceFactory.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnService_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Service"))
                        continue;

                    string serviceName = item.Name.Substring(0, item.Name.Length - 11) + "Service";
                    string strPath = string.Format(@"F:\Tools\Creator\{0}.cs", serviceName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        ServiceCode service = new ServiceCode(item, this.ProjectName);
                        writer.Write(service.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnIService_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Service"))
                        continue;

                    string serviceName = item.Name.Substring(0, item.Name.Length - 11) + "Service";
                    string strPath = string.Format(@"F:\Tools\Creator\I{0}.cs", serviceName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        IServiceCode service = new IServiceCode(item, this.ProjectName);
                        writer.Write(service.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnFactory_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\ServiceFactory.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    ServiceFactoryCode serviceFactory = new ServiceFactoryCode(this.ProjectName);
                    writer.Write(serviceFactory.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnIFactory_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\IServiceFactory.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    IServiceFactoryCode serviceFactory = new IServiceFactoryCode(this.ProjectName);
                    writer.Write(serviceFactory.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        #endregion

        #region DataServiceEx
        private void btnIServiceEx_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataServiceBase.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if(!item.IsPublic || !item.Name.EndsWith("ServiceBase"))
                        continue;

                    string name = item.Name.Substring(0, item.Name.Length - "Base".Length);
                    string strPath = string.Format(@"F:\Tools\Creator\I{0}.cs", name);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        IServiceExCode service = new IServiceExCode(item, this.ProjectName);
                        writer.Write(service.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnIFactoryEx_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataServiceBase.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\IDataServiceFactory.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    IServiceFactoryExCode serviceFactory = new IServiceFactoryExCode(assembly, this.ProjectName);
                    writer.Write(serviceFactory.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnServiceEx_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataServiceBase.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            Assembly dataAssembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (dataAssembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("ServiceBase"))
                        continue;

                    string name = item.Name.Substring(0, item.Name.Length - "Base".Length);
                    string strPath = string.Format(@"F:\Tools\Creator\{0}.cs", name);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        ServiceExCode service = new ServiceExCode(item, this.ProjectName, dataAssembly);
                        writer.Write(service.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnInnerFactory_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\DataServiceBuilder.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    InnerServiceBuilderCode serviceFactory = new InnerServiceBuilderCode(assembly, this.ProjectName);
                    writer.Write(serviceFactory.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnOutFactory_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataServiceBase.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\DataServiceFactory.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    ServiceFactoryExCode serviceFactory = new ServiceFactoryExCode(assembly, this.ProjectName);
                    writer.Write(serviceFactory.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        #endregion

        #region Obj Code
        private void btnServiceBuilder_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\ServiceBuilder.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    ServiceBuilderCode serviceFactory = new ServiceBuilderCode(this.ProjectName);
                    writer.Write(serviceFactory.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnGenServiceBuilder_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\GenServiceBuilder.cs");

                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    GenServiceBuilderCode serviceFactory = new GenServiceBuilderCode(assembly, this.ProjectName);
                    writer.Write(serviceFactory.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnBizObj_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string objectName = item.Name.Substring(0, item.Name.Length - 4);
                    string strPath = string.Format(@"F:\Tools\Creator\{0}.cs", objectName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        BizObjCode service = new BizObjCode(item, this.ProjectName);
                        writer.Write(service.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnBizObjCollection_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string objectName = item.Name.Substring(0, item.Name.Length - 4);
                    string strPath = string.Format(@"F:\Tools\Creator\{0}Collection.cs", objectName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        BizObjCollectionCode service = new BizObjCollectionCode(item, this.ProjectName);
                        writer.Write(service.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnGenBizObj_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string objectName = "Biz" + item.Name.Substring(0, item.Name.Length - 4);
                    string strPath = string.Format(@"F:\Tools\Creator\Gen{0}.cs", objectName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        GenBizObjCode service = new GenBizObjCode(item, this.ProjectName);
                        writer.Write(service.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnGenBizObjCollection_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string objectName = "Biz" + item.Name.Substring(0, item.Name.Length - 4);
                    string strPath = string.Format(@"F:\Tools\Creator\Gen{0}Collection.cs", objectName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        GenBizObjCollectionCode service = new GenBizObjCollectionCode(item, this.ProjectName);
                        writer.Write(service.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        #endregion

        #region Helper functions

        private string ProjectName
        {
            get
            {
                return this.textBox1.Text.Trim();
            }
        }

        private Assembly GetAssembly(string dllName)
        {
            return this.GetAssembly(dllName, string.Empty);
        }

        private Assembly GetAssembly(string dllName, string title)
        {
            if(!string.IsNullOrEmpty(title))
            {
                this.openFileDialog1.Title = title;
            }
            
            this.openFileDialog1.FileName = dllName;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return Assembly.LoadFrom(this.openFileDialog1.FileName);
            }

            return null;
        }

        #endregion

        private void btnTest_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;

                    string entityName = item.Name.Substring(0, item.Name.Length - 4);

                    string strPath = string.Format(@"F:\Tools\Creator\{0}_Asp.ent.cs", entityName);

                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        TestCode code = new TestCode(item);
                        writer.Write(code.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void btnPropertyToUpper_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                ViewObjOverrideCode factory = new ViewObjOverrideCode(assembly, this.textBox1.Text);

                string strPath = string.Format(@"F:\Tools\Creator\ViewOverrideProperties.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(factory.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || !item.Name.EndsWith("Data"))
                        continue;
                    
                    string entityName = item.Name.Substring(0, item.Name.Length - 4);

                    //View
                    string strPath = string.Format(@"F:\Tools\Creator\{0}View.cs", entityName);
                    FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        ViewCode code = new ViewCode(item, this.textBox1.Text);
                        writer.Write(code.GenCode());
                    }

                    //ViewCollection
                    strPath = string.Format(@"F:\Tools\Creator\{0}ViewCollection.cs", entityName);
                    fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        ViewCollectionCode code = new ViewCollectionCode(item, this.textBox1.Text);
                        writer.Write(code.GenCode());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnDataSchema_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\DataSchema.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    SchemaCode code = new SchemaCode(assembly, this.ProjectName);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnBizSchema_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.BizData.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\BizSchema.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    SchemaCode code = new SchemaCode(assembly, this.ProjectName);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnViewSchema_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.ViewObj.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\ViewSchema.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    SchemaCode code = new SchemaCode(assembly, this.ProjectName);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnDBSchemaCode_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\DBSchema.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    DBSchemaCode code = new DBSchemaCode(assembly, this.textBox1.Text);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnDataServiceHelperBuilder_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.IDataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\DataServiceBuilder.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    DataServiceHelpFactory code = new DataServiceHelpFactory(assembly.GetTypes(), this.textBox1.Text);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnDataServiceHelper_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.IDataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\DataServiceHelper.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    DataServiceHelp code = new DataServiceHelp(assembly.GetTypes(), this.textBox1.Text);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnDataServiceExHelper_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.IDataServiceEx.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\DataServiceExHelper.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    DataServiceHelp code = new DataServiceHelp(assembly.GetTypes(), this.textBox1.Text);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnFacadeWrapper_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.IFacadeService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\FacadeMethods.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    FacadeWrapperCode code = new FacadeWrapperCode(assembly, this.textBox1.Text);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnBizServiceWrapper_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Empty);
            if (assembly == null)
                return;

            try
            {
                //Service Builder
                string strPath = string.Format(@"F:\Tools\Creator\ServiceBuilder.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    BizServiceBuilderCode code = new BizServiceBuilderCode(assembly);
                    writer.Write(code.GenCode());
                }

                //ServiceWrapper
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || item.Name == "IServiceFactory")
                        continue;

                    string entityName = item.Name.Substring(1, item.Name.Length - 8);

                    strPath = string.Format(@"F:\Tools\Creator\{0}Wrapper.cs", entityName);
                    fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        BizServiceWrapperCode code = new BizServiceWrapperCode(item);
                        writer.Write(code.GenCode());
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnMFServiceWrapper_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.IMicroService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\MicroServiceWrapper.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    MFServiceWrapperCode code = new MFServiceWrapperCode(assembly);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnMyServiceWrapper_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Empty);
            if (assembly == null)
                return;

            try
            {
                //Service Builder
                string strPath = string.Format(@"F:\Tools\Creator\ServiceBuilder.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    MyServiceBuilderCode code = new MyServiceBuilderCode(assembly);
                    writer.Write(code.GenCode());
                }

                //ServiceWrapper
                Type[] types = assembly.GetTypes();
                foreach (Type item in types)
                {
                    if (!item.IsPublic || item.Name == "IServiceFactory")
                        continue;

                    string entityName = item.Name.Substring(1, item.Name.Length - 8);

                    strPath = string.Format(@"F:\Tools\Creator\{0}Wrapper.cs", entityName);
                    fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        MyServiceWrapperCode code = new MyServiceWrapperCode(item);
                        writer.Write(code.GenCode());
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnfacadeService_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Empty);
            if (assembly == null)
                return;

            try
            {
                string[] splits = assembly.GetName().Name.Split('.');
                if (splits.Length < 2)
                    return;

                //Interface
                string strPath = string.Format(@"F:\Tools\Creator\IBiz{0}.cs", splits[1].Substring(1));
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    FacadeIBizServiceCode code = new FacadeIBizServiceCode(assembly);
                    writer.Write(code.GenCode());
                }

                //Impl
                strPath = string.Format(@"F:\Tools\Creator\Biz{0}.cs", splits[1].Substring(1));
                fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    FacadeBizServiceCode code = new FacadeBizServiceCode(assembly);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnTestService_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Empty);
            if (assembly == null)
                return;

            try
            {
                string[] splits = assembly.GetName().Name.Split('.');
                if (splits.Length < 2)
                    return;

                //Interface
                string strPath = string.Format(@"F:\Tools\Creator\IBizPanelService.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    TestIBizServiceCode code = new TestIBizServiceCode(assembly);
                    writer.Write(code.GenCode());
                }

                //Impl
                strPath = string.Format(@"F:\Tools\Creator\BizPanelService.cs");
                fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    TestBizServiceCode code = new TestBizServiceCode(assembly);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnGetViewBy_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\GetViewBy.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    GetViewByCode code = new GetViewByCode(assembly);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnExtractInterface_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.DataService.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\Interface.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    ExtractInterfaceCode code = new ExtractInterfaceCode(assembly);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnCRUDCode_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.CRUD.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\CRUD.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    CRUDTestCode code = new CRUDTestCode(assembly);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnWrapperFactory_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openDlg = new OpenFileDialog();
                openDlg.Filter = "CS File(*.cs)|*.cs";
                if (openDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                SaveFileDialog saveDlg = new SaveFileDialog();
                saveDlg.Filter = "CS File(*.cs)|*.cs";
                if(saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                FileStream fs = new FileStream(saveDlg.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    Wrapper code = new Wrapper(openDlg.FileName);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnClearFiles_Click(object sender, EventArgs e)
        {
            string startPath = Application.StartupPath;
            string[] files = Directory.GetFiles(startPath);
            foreach (string file in files)
            {
                if(file.EndsWith("exe"))
                    continue;

                File.Delete(file);
            }
        }

        private void btnSaveOld_Click(object sender, EventArgs e)
        {
            ObjectSchemaCollection list = this.GetAssemblySchema(string.Format("{0}.Data.dll", this.textBox1.Text), "Select old data.dll");

            string fileName = string.Format("{0}\\old.data", Application.StartupPath);
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, list);
            fs.Close();
        }

        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            ObjectSchemaCollection list = this.GetAssemblySchema(string.Format("{0}.Data.dll", this.textBox1.Text), "Select current data.dll");
            
            string fileName = string.Format("{0}\\new.data", Application.StartupPath);
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, list);
            fs.Close();
        }

        private ObjectSchemaCollection GetAssemblySchema(string dllName, string title)
        {
            Assembly old = this.GetAssembly(dllName, title);
            if (old == null)
                return null;

            Type[] types = old.GetTypes();
            ObjectSchemaCollection retList = new ObjectSchemaCollection();
            foreach (Type type in types)
            {
                if (!type.IsClass || !type.IsPublic)
                    continue;

                if (type.IsAbstract || type.IsInterface || type.IsNested)
                    continue;

                if (!type.Name.EndsWith("Data") || type.Name.StartsWith("ZZ"))
                    continue;

                string key = type.Name.Substring(0, type.Name.Length - 4);
                ObjectSchema entity = new ObjectSchema();
                entity.ObjectName = key;
                retList.Add(entity);

                PropertyInfo[] oldInfo = type.GetProperties();
                foreach (PropertyInfo info in oldInfo)
                {
                    FieldSchema field = new FieldSchema();
                    field.FieldName = info.Name;
                    field.FieldType = info.PropertyType.ToString();
                    entity.FieldList.Add(field);
                }
            }

            return retList;
        }

        private void btnDataDiff_Click(object sender, EventArgs e)
        {
            string fileName = string.Format("{0}\\old.data", Application.StartupPath);
            FileStream fs = new FileStream(fileName, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            ObjectSchemaCollection oldList = bf.Deserialize(fs) as ObjectSchemaCollection;
            fs.Close();

            if (oldList == null)
                return;

            fileName = string.Format("{0}\\new.data", Application.StartupPath);
            fs = new FileStream(fileName, FileMode.Open);
            bf = new BinaryFormatter();
            ObjectSchemaCollection currentList = bf.Deserialize(fs) as ObjectSchemaCollection;
            fs.Close();

            if (currentList == null)
                return;

            //Old
            SortedList<string, ObjectSchema> oldTypeIndex = new SortedList<string, ObjectSchema>();
            foreach (ObjectSchema item in oldList)
            {
                oldTypeIndex.Add(item.ObjectName, item);
            }

            //Current
            SortedList<string, ObjectSchema> currentTypeIndex = new SortedList<string, ObjectSchema>();
            foreach (ObjectSchema item in currentList)
            {
                currentTypeIndex.Add(item.ObjectName, item);
            }

            //Diff
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, ObjectSchema> pair in currentTypeIndex)
            {
                builder.AppendLine(string.Format("{0}: ", pair.Key));
                if(!oldTypeIndex.ContainsKey(pair.Key))
                {
                    builder.AppendLine(string.Format("++++++++++++++++++++ Is New Table.+++++++++++++++++++"));
                }
                else
                {
                    string diff = DiffTableField(oldTypeIndex[pair.Key], pair.Value);
                    if(string.IsNullOrEmpty(diff))
                    {
                        builder.AppendLine(string.Format("======================================== OK!"));
                    }
                    else
                    {
                        builder.AppendLine(diff);
                    }
                }
            }

            foreach (KeyValuePair<string, ObjectSchema> pair in oldTypeIndex)
            {
                if(!currentTypeIndex.ContainsKey(pair.Key))
                {
                    builder.AppendLine(string.Format("{0}: ", pair.Key));
                    builder.AppendLine(string.Format("------------------- Is Deleted Table.--------------------"));
                }
            }

            FormNotes dlg = new FormNotes(builder.ToString());
            dlg.ShowDialog();
        }

        private string DiffTableField(ObjectSchema old, ObjectSchema current)
        {
            SortedList<string, FieldSchema> oldIndex = new SortedList<string, FieldSchema>();
            foreach (FieldSchema info in old.FieldList)
            {
                if(oldIndex.ContainsKey(info.FieldName))
                    continue;

                oldIndex.Add(info.FieldName, info);
            }

            SortedList<string, FieldSchema> currentIndex = new SortedList<string, FieldSchema>();
            foreach (FieldSchema info in current.FieldList)
            {
                if (currentIndex.ContainsKey(info.FieldName))
                    continue;

                currentIndex.Add(info.FieldName, info);
            }

            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, FieldSchema> pair in currentIndex)
            {
                if(!oldIndex.ContainsKey(pair.Key))
                {
                    builder.AppendLine(string.Format("+++++ {0}: Field Inserted;+++++", pair.Key));
                }
                else
                {
                    FieldSchema oldProperty = oldIndex[pair.Key];
                    if(oldProperty.FieldType != pair.Value.FieldType)
                    {
                        builder.AppendLine(string.Format("***** {0}: Field Type Modified;*****", pair.Key));
                    }
                }
            }

            foreach (KeyValuePair<string, FieldSchema> pair in oldIndex)
            {
                if(!currentIndex.ContainsKey(pair.Key))
                {
                    builder.AppendLine(string.Format("----- {0}: Field Deleted;-----", pair.Key));
                }
            }

            return builder.ToString();
        }

        private void btnViewObj_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.Data.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\ViewObjs.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    ViewObjCode code = new ViewObjCode(assembly, this.ProjectName);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnViewMethod_Click(object sender, EventArgs e)
        {
            Assembly assembly = this.GetAssembly(string.Format("{0}.BasicServiceWrapper.dll", this.textBox1.Text));
            if (assembly == null)
                return;

            try
            {
                string strPath = string.Format(@"F:\Tools\Creator\ViewMethods.cs");
                FileStream fs = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.SetLength(0);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    ViewMethodCode code = new ViewMethodCode(assembly, this.ProjectName);
                    writer.Write(code.GenCode());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
    }

    [Serializable]
    public class ObjectSchema
    {
        private string _objectName = string.Empty;
        private readonly FieldSchemaCollection _fieldList = new FieldSchemaCollection();

        public string ObjectName
        {
            get { return _objectName; }
            set { _objectName = value; }
        }

        public FieldSchemaCollection FieldList
        {
            get { return _fieldList; }
        }
    }

    [Serializable]
    public class ObjectSchemaCollection : CollectionBase
    {
        public void Add(ObjectSchema entity)
        {
            this.List.Add(entity);
        }

        public ObjectSchema this[int index]
        {
            get { return (ObjectSchema)base.List[index]; }
            set { base.List[index] = value; }
        }
    }

    [Serializable]
    public class FieldSchema
    {
        private string _fieldName = string.Empty;
        private string _fieldType = string.Empty;

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public string FieldType
        {
            get { return _fieldType; }
            set { _fieldType = value; }
        }
    }

    [Serializable]
    public class FieldSchemaCollection : CollectionBase
    {
        public void Add(FieldSchema entity)
        {
            this.List.Add(entity);
        }

        public FieldSchema this[int index]
        {
            get { return (FieldSchema)base.List[index]; }
            set { base.List[index] = value; }
        }
    }
}