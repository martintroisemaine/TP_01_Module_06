using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TP_01_Module_06.Models
{
    public class SamouraiViewModel
    {
        public Samourai Samourai { get; set; }

        public List<SelectListItem> Armes { get; set; } = new List<SelectListItem>();

        public int? IdArme { get; set; }
    }
}