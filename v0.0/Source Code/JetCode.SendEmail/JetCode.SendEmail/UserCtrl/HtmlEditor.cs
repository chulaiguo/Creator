using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace JetCode.SendEmail.UserCtrl
{
    [Description("Html Editor"), ClassInterface(ClassInterfaceType.AutoDispatch)]
    public partial class HtmlEditor : UserControl
    {
        private int _dataUpdate = 0;
        public HtmlEditor()
        {
            InitializeComponent();

            InitializeControls();
        }

        #region Expand Properties

        public override string Text
        {
            get
            {
                return webBrowserBody.DocumentText;
            }
            set
            {
                webBrowserBody.DocumentText = value.Replace(@"\r\n", @"<br>");
            }
        }

        public string[] Images
        {
            get
            {
                List<string> images = new List<string>();

                foreach (HtmlElement element in this.Document.Images)
                {
                    string image = element.GetAttribute("src");
                    if (!images.Contains(image))
                    {
                        images.Add(image);
                    }
                }

                return images.ToArray();
            }
        }

        public HtmlDocument Document
        {
            get { return webBrowserBody.Document; }
        }

        #endregion

        #region Methods

        private void InitializeControls()
        {
            BeginUpdate();

            //Tools
            foreach (FontFamily family in FontFamily.Families)
            {
                toolStripComboBoxName.Items.Add(family.Name);
            }

            toolStripComboBoxSize.Items.AddRange(FontSize.All.ToArray());

            //Browser
            webBrowserBody.DocumentText = string.Empty;
            this.Document.Click += webBrowserBody_DocumentClick;
            this.Document.Focusing += webBrowserBody_DocumentFocusing;
            this.Document.ExecCommand("EditMode", false, null);
            this.Document.ExecCommand("LiveResize", false, null);

            EndUpdate();
        }

        private void RefreshToolBar()
        {
            BeginUpdate();

            try
            {
                mshtml.IHTMLDocument2 document = (mshtml.IHTMLDocument2)this.Document.DomDocument;

                toolStripComboBoxName.Text = document.queryCommandValue("FontName").ToString();
                toolStripComboBoxSize.SelectedItem = FontSize.Find((int)document.queryCommandValue("FontSize"));
                toolStripButtonBold.Checked = document.queryCommandState("Bold");
                toolStripButtonItalic.Checked = document.queryCommandState("Italic");
                toolStripButtonUnderline.Checked = document.queryCommandState("Underline");

                toolStripButtonNumbers.Checked = document.queryCommandState("InsertOrderedList");
                toolStripButtonBullets.Checked = document.queryCommandState("InsertUnorderedList");

                toolStripButtonLeft.Checked = document.queryCommandState("JustifyLeft");
                toolStripButtonCenter.Checked = document.queryCommandState("JustifyCenter");
                toolStripButtonRight.Checked = document.queryCommandState("JustifyRight");
                toolStripButtonFull.Checked = document.queryCommandState("JustifyFull");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            finally
            {
                EndUpdate();
            }
        }

        #endregion

        #region Update
        private bool Updating
        {
            get
            {
                return _dataUpdate != 0;
            }
        }

        private void BeginUpdate()
        {
            ++_dataUpdate;
        }
        private void EndUpdate()
        {
            --_dataUpdate;
        }

        #endregion

        #region Tools

        private void toolStripComboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("FontName", false, toolStripComboBoxName.Text);
        }
        private void toolStripComboBoxSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            FontSize fontSize = toolStripComboBoxSize.SelectedItem as FontSize;
            if(fontSize == null)
                return;

            int size = (toolStripComboBoxSize.SelectedItem == null) ? 1 : fontSize.Value;
            this.Document.ExecCommand("FontSize", false, size);
        }
        private void toolStripButtonBold_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("Bold", false, null);
            RefreshToolBar();
        }
        private void toolStripButtonItalic_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("Italic", false, null);
            RefreshToolBar();
        }
        private void toolStripButtonUnderline_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("Underline", false, null);
            RefreshToolBar();
        }
        private void toolStripButtonColor_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            int fontcolor = (int)((mshtml.IHTMLDocument2)this.Document.DomDocument).queryCommandValue("ForeColor");

            ColorDialog dialog = new ColorDialog();
            dialog.Color = Color.FromArgb(0xff, fontcolor & 0xff, (fontcolor >> 8) & 0xff, (fontcolor >> 16) & 0xff);

            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string color = dialog.Color.Name;
                if (!dialog.Color.IsNamedColor)
                {
                    color = "#" + color.Remove(0, 2);
                }

                this.Document.ExecCommand("ForeColor", false, color);
            }
            RefreshToolBar();
        }

        private void toolStripButtonNumbers_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("InsertOrderedList", false, null);
            RefreshToolBar();
        }
        private void toolStripButtonBullets_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("InsertUnorderedList", false, null);
            RefreshToolBar();
        }
        private void toolStripButtonOutdent_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("Outdent", false, null);
            RefreshToolBar();
        }
        private void toolStripButtonIndent_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("Indent", false, null);
            RefreshToolBar();
        }

        private void toolStripButtonLeft_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("JustifyLeft", false, null);
            RefreshToolBar();
        }
        private void toolStripButtonCenter_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("JustifyCenter", false, null);
            RefreshToolBar();
        }
        private void toolStripButtonRight_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("JustifyRight", false, null);
            RefreshToolBar();
        }
        private void toolStripButtonFull_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("JustifyFull", false, null);
            RefreshToolBar();
        }

        private void toolStripButtonLine_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("InsertHorizontalRule", false, null);
            RefreshToolBar();
        }
        private void toolStripButtonHyperlink_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("CreateLink", true, null);
            RefreshToolBar();
        }
        private void toolStripButtonPicture_Click(object sender, EventArgs e)
        {
            if (Updating)
            {
                return;
            }

            this.Document.ExecCommand("InsertImage", true, null);
            RefreshToolBar();
        }

        #endregion

        #region Browser

        private void webBrowserBody_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        private void webBrowserBody_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.IsInputKey)
            {
                return;
            }
            RefreshToolBar();
        }

        private void webBrowserBody_DocumentClick(object sender, HtmlElementEventArgs e)
        {
            RefreshToolBar();
        }

        private void webBrowserBody_DocumentFocusing(object sender, HtmlElementEventArgs e)
        {
            RefreshToolBar();
        }

        #endregion

        #region Font Size Class

        private class FontSize
        {
            private static List<FontSize> _allFontSize = null;
            public static List<FontSize> All
            {
                get
                {
                    if (_allFontSize == null)
                    {
                        _allFontSize = new List<FontSize>();
                        _allFontSize.Add(new FontSize(8, 1));
                        _allFontSize.Add(new FontSize(10, 2));
                        _allFontSize.Add(new FontSize(12, 3));
                        _allFontSize.Add(new FontSize(14, 4));
                        _allFontSize.Add(new FontSize(18, 5));
                        _allFontSize.Add(new FontSize(24, 6));
                        _allFontSize.Add(new FontSize(36, 7));
                    }

                    return _allFontSize;
                }
            }

            public static FontSize Find(int value)
            {
                if (value < 1)
                {
                    return All[0];
                }

                if (value > 7)
                {
                    return All[6];
                }

                return All[value - 1];
            }

            private FontSize(int display, int value)
            {
                _displaySize = display;
                _valueSize = value;
            }

            private readonly int _valueSize;
            public int Value
            {
                get
                {
                    return _valueSize;
                }
            }

            private readonly int _displaySize;
            public int Display
            {
                get
                {
                    return _displaySize;
                }
            }

            public override string ToString()
            {
                return this.Display.ToString();
            }
        }

        #endregion

        #region ComboBoxEx Class

        private class ToolStripComboBoxEx : ToolStripComboBox
        {
            public override Size GetPreferredSize(Size constrainingSize)
            {
                Size size = base.GetPreferredSize(constrainingSize);
                size.Width = Math.Max(Width, 0x20);
                return size;
            }
        }

        #endregion
    }
}
