//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectronicsStore.ADO
{
    using System;
    using System.Collections.Generic;
    
    public partial class Basket
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Product_Id { get; set; }
        public Nullable<int> Count { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
