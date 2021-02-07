using FileSearch.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace FileSearch
{
    public partial class AppForm : Form
    {
        public AppForm()
        {
            InitializeComponent();
            SearchingRecord = new SearchRecord
            {
                FileInfos = new List<FileInfo>()
            };
            FileSearchPlugins = new List<IFileSearchPlugin>();
            LoadPlugins();

        }
        private void LoadPlugins()
        {

            foreach (var file in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Plugins\"), "*.dll"))
            {
                var avaiableTypes = Assembly.LoadFrom(file).GetTypes();

                foreach (var type in avaiableTypes)
                {
                    if (type.GetInterfaces().Contains(typeof(IFileSearchPlugin)))
                        this.FileSearchPlugins.Add((IFileSearchPlugin)Activator.CreateInstance(type));
                }
            }

            foreach (var plugin in FileSearchPlugins)
            {
                plugin.Initialize(this);
            }
        }

        private void Searchbt_Click(object sender, EventArgs e)
        {
            SearchingRecord.Text = searchingTexttxtb.Text;
            SearchingRecord.DirectoryName = pathtxt.Text;

            if (String.IsNullOrEmpty(SearchingRecord.DirectoryName)) return;

            SearchingRecord.FileInfos.Clear();

            foreach (string file in Directory.EnumerateFiles(SearchingRecord.DirectoryName, "*.txt"))
            {
                string contents = File.ReadAllText(file);

                if (contents.Contains(SearchingRecord.Text))
                {
                    var fileInfo = new FileInfo(file);
                    SearchingRecord.FileInfos.Add(fileInfo);
                }
            }

            FillListView();
        }

        public void FillListView()
        {
            foundFileslv.Items.Clear();

            foreach (var info in SearchingRecord.FileInfos)
            {
                var listItem = new ListViewItem(info.FullName);
                listItem.SubItems.Add(info.Extension);
                listItem.SubItems.Add(info.Length.ToString());
                listItem.Tag = info;

                foundFileslv.Items.Add(listItem);
            }
        }

        public void FillTextBoxes()
        {
            searchingTexttxtb.Text = SearchingRecord.Text;
            pathtxt.Text = SearchingRecord.DirectoryName;
        }

        private void folderBrowsebt_Click(object sender, EventArgs e)
        {           
             DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                pathtxt.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        public ToolStripMenuItem PluginsToolStripMenuItem
        {
            get
            {
                return pluginsToolStripMenuItem;
            }
            set
            {
                pluginsToolStripMenuItem = value;
            }
        }

        private SearchRecord searchRecord;
        
        public SearchRecord SearchingRecord
        {
            get
            {
                return searchRecord;
            }
            set
            {
                searchRecord = value;
            }
        }

        private List<IFileSearchPlugin> fileSearchPlugins;
        private Button searchbt;
        private FolderBrowserDialog folderBrowserDialog1;

        public List<IFileSearchPlugin> FileSearchPlugins
        {
            get
            {
                return fileSearchPlugins;
            }
            set
            {
                fileSearchPlugins = value;
            }
        }

        private void newSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foundFileslv.Items.Clear();
            searchingTexttxtb.Clear();
            pathtxt.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
