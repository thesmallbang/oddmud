﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{

    public class DbPlayer : BaseEntity
    {
        
        public string Name { get; set; }
        public string Password { get; set; }
        public int LastMap { get; set; }

    }
}

