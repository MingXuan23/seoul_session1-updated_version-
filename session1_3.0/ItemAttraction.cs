//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace session1_3._0
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemAttraction
    {
        public long ID { get; set; }
        public System.Guid GUID { get; set; }
        public long ItemID { get; set; }
        public long AttractionID { get; set; }
        public Nullable<decimal> Distance { get; set; }
        public Nullable<long> DurationOnFoot { get; set; }
        public Nullable<long> DurationByCar { get; set; }
    
        public virtual Attraction Attraction { get; set; }
        public virtual Item Item { get; set; }
    }
}
