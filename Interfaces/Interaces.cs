﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Interfaces
{
    public interface IClosable
    {
        public Action Close { set; get; }
        public bool CanClose();
    }
}
