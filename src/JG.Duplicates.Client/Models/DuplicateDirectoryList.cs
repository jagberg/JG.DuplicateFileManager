﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.Duplicates.Client
{
    public class DuplicateDirectoryList
    {
        public string DirectoryName { get; set; }

        public List<FileItemInfo> Children { get; set; }
    }
}
