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
    
    public partial class Client
    {
        public Client()
        {
            this.ClientAttributeValues = new HashSet<ClientAttributeValue>();
            this.ClientFees = new HashSet<ClientFee>();
            this.ClientNotes = new HashSet<ClientNote>();
            this.Contacts = new HashSet<Contact>();
            this.Contracts = new HashSet<Contract>();
            this.DdSchemes = new HashSet<DdScheme>();
            this.ClientCategories = new HashSet<ClientCategory>();
        }
    
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string WebSite { get; set; }
        public string Email { get; set; }
        public bool IsTaxExempt { get; set; }
        public bool Active { get; set; }
        public Nullable<int> ClientAddress_Id { get; set; }
        public Nullable<int> BillingAddress_Id { get; set; }
        public Nullable<int> ShippingAddress_Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FinanceLink { get; set; }
        public Nullable<int> PaymentTerms { get; set; }
        public string ClientCode { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual Address Address1 { get; set; }
        public virtual Address Address2 { get; set; }
        public virtual ICollection<ClientAttributeValue> ClientAttributeValues { get; set; }
        public virtual ICollection<ClientFee> ClientFees { get; set; }
        public virtual ICollection<ClientNote> ClientNotes { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<DdScheme> DdSchemes { get; set; }
        public virtual ICollection<ClientCategory> ClientCategories { get; set; }
    }
}