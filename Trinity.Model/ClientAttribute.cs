//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Trinity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClientAttribute
    {
        public ClientAttribute()
        {
            this.ClientAttributeValues = new HashSet<ClientAttributeValue>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public int AttributeControlTypeId { get; set; }
        public int DisplayOrder { get; set; }
        public bool AdminOnly { get; set; }
        public string AttributeOptions { get; set; }
    
        public virtual ICollection<ClientAttributeValue> ClientAttributeValues { get; set; }
    }
}
