//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Library
{
    using System;
    using System.Collections.Generic;
    
    public partial class Orders
    {
        public int id { get; set; }
        public int ClientID { get; set; }
        public int BookID { get; set; }
        public System.DateTime OrderDate { get; set; }
        public System.DateTime ReturnDate { get; set; }
    
        public virtual Clients Clients { get; set; }
        public virtual Books Books { get; set; }
    }
}
