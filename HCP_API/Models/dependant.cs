//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HCP_API.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    
    public partial class dependant
    {
        public string phone_number { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string national_id { get; set; }
        public Nullable<System.DateTime> dob { get; set; }
        public string surname { get; set; }
        public string principal_number { get; set; }
        public string cover { get; set; }
    
        [JsonIgnore]
        public virtual registered registered { get; set; }
    }
}
