using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using TesoreriaVS12.Models;

namespace TesoreriaVS12.DAL
{
    public class GenericDAL
    { 
        private ControlGeneralContainer _db;

        public ControlGeneralContainer db
        {
            get { return _db; }
            set { _db = value; }
        }

        //public BD_TesoreriaEntities db { get; set; }

        public GenericDAL()
        {
            if (db == null) db = new ControlGeneralContainer();
        }
    }
}