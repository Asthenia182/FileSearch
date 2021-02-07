using System.Collections.Generic;
using System.Windows.Forms;

namespace FileSearch.Common
{ 
    public interface IFileSearchApp
{
        ToolStripMenuItem PluginsToolStripMenuItem { get; set; }

        SearchRecord SearchingRecord { get; set; }

        List<IFileSearchPlugin> FileSearchPlugins { get; set; }

        void FillListView();

        void FillTextBoxes();
    }
}
