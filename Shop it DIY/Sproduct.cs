//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shop_it_DIY
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sproduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sproduct()
        {
            this.SOrderItems = new HashSet<SOrderItem>();
        }
    
        public int Pd_nID { get; set; }
        public string Pd_ID { get; set; }
        public string Pd_name { get; set; }
        public string Pd_detail { get; set; }
        public decimal Pd_unitprice { get; set; }
        public int Pd_stock { get; set; }
        public string Pd_type { get; set; }
        public string Pd_img { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOrderItem> SOrderItems { get; set; }
    }
}
