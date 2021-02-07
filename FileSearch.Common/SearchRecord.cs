using System;
using System.Collections.Generic;
using System.IO;

namespace FileSearch.Common
{
    [Serializable]
    public class SearchRecord
    {
        string text;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        public List<FileInfo> fileInfos;

        public List<FileInfo> FileInfos
        {
            get
            {
                return fileInfos;
            }
            set
            {
                fileInfos = value;
            }
        }

        public string DirectoryName;


    }
}
