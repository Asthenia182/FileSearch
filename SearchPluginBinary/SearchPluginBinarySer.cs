using FileSearch.Common;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace SearchPluginBinary
{
    public class SearchPluginBinarySer : IFileSearchPlugin
    {
        private IFileSearchApp App;

        public void Initialize(IFileSearchApp app)
        {
            App = app;

            var saveToolStripItem = new ToolStripMenuItem("Save searched text as binary", null, new EventHandler(saveStripItem_Click)); 
            var readToolStripItem = new ToolStripMenuItem("Read searched text from binary", null, new EventHandler(readStripItem_Click));

            app.PluginsToolStripMenuItem.DropDownItems.Add(saveToolStripItem);
            app.PluginsToolStripMenuItem.DropDownItems.Add(readToolStripItem);
        }

        private void readStripItem_Click(object sender, EventArgs e)
        {
            FromBinary();
        }

        private void saveStripItem_Click(object sender, EventArgs e)
        {
            ToBinary();
        }

        public void ToBinary()
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "*.bin|*.bin";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    // Serialize the items and save it to a file.
                    using (System.IO.FileStream fs = System.IO.File.Create(dlg.FileName))
                    {
                        var bf = new BinaryFormatter();
                        bf.Serialize(fs, App.SearchingRecord);
                    }
                }
            }
        }

        public void FromBinary()
        {
            var loadedSearchRecord = new SearchRecord();

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "*.bin|*.bin";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var fs = new FileStream(ofd.FileName, FileMode.Open))
                    {
                        if (fs.Length > 0)
                        {                    
                            var bf = new BinaryFormatter();

                            loadedSearchRecord =  (SearchRecord)bf.Deserialize(fs);
                        }
                    }
                }
            }

            App.SearchingRecord = loadedSearchRecord;
            
            App.FillListView();
            App.FillTextBoxes();
       }

    }
}
