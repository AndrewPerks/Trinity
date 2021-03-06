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
    
    public partial class Contact
    {
        public Contact()
        {
            this.Contact_Permission_Mapping = new HashSet<Contact_Permission_Mapping>();
            this.ContactCategories = new HashSet<ContactCategory>();
            this.FileTypes = new HashSet<FileType>();
            this.Roles = new HashSet<Role>();
        }
    
        public int Id { get; set; }
        public int Client_Id { get; set; }
        public Nullable<int> Title_Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public Nullable<int> ContactAddress_Id { get; set; }
        public Nullable<int> HomeAddress_Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] PasswordHash { get; set; }
        public int PasswordFormat_Id { get; set; }
        public string PasswordSalt { get; set; }
        public string AdminComment { get; set; }
        public bool LoginActive { get; set; }
        public int FailedLoginCount { get; set; }
        public bool Deleted { get; set; }
        public bool IsSystemAccount { get; set; }
        public string SystemName { get; set; }
        public string LastIpAddress { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public string Note { get; set; }
    
        public virtual ICollection<Contact_Permission_Mapping> Contact_Permission_Mapping { get; set; }
        public virtual ICollection<ContactCategory> ContactCategories { get; set; }
        public virtual ICollection<FileType> FileTypes { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual Address Address { get; set; }
        public virtual Address Address1 { get; set; }
        public virtual Client Client { get; set; }
    }
}
