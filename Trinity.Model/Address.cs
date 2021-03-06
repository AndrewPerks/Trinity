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
    
    public partial class Address
    {
        public Address()
        {
            this.Clients = new HashSet<Client>();
            this.Clients1 = new HashSet<Client>();
            this.Clients2 = new HashSet<Client>();
            this.Contacts = new HashSet<Contact>();
            this.Contacts1 = new HashSet<Contact>();
        }
    
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ClientName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostalCode { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public System.DateTime CreatedOn { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Client> Clients1 { get; set; }
        public virtual ICollection<Client> Clients2 { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Contact> Contacts1 { get; set; }
    }
}
