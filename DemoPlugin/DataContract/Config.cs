﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DemoPlugin.DataContract
{
    [DataContract]
    public class Config
    {
        [DataMember]
        public string Name;

    }
}
