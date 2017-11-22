using System.Data.Entity;
using Trinity.Model;

namespace Trinity.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for the ITrinityContext class
    /// </summary>
    public interface ITrinityContext
    {
        DbSet<Address> Addresses { get; set; }
        DbSet<ApplicationSetting> ApplicationSettings { get; set; }
        DbSet<Client> Clients { get; set; }
        DbSet<ClientAttribute> ClientAttributes { get; set; }
        DbSet<ClientAttributeValue> ClientAttributeValues { get; set; }
        DbSet<ClientCategory> ClientCategories { get; set; }
        DbSet<ClientFee> ClientFees { get; set; }
        DbSet<ClientNote> ClientNotes { get; set; }
        DbSet<ClientNoteCategory> ClientNoteCategories { get; set; }
        DbSet<ClientNoteTag> ClientNoteTags { get; set; }
        DbSet<Contact> Contacts { get; set; }
        DbSet<Contact_Permission_Mapping> ContactPermissionMappings { get; set; }
        DbSet<ContactCategory> ContactCategories { get; set; }
        DbSet<Contract> Contracts { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<DdInstruction> DdInstructions { get; set; }
        DbSet<DdInstructionAttribute> DdInstructionAttributes { get; set; }
        DbSet<DdInstructionAttributeValue> DdInstructionAttributeValues { get; set; }
        DbSet<DdInstructionNote> DdInstructionNotes { get; set; }
        DbSet<DdInstructionNoteCategory> DdInstructionNoteCategories { get; set; }
        DbSet<DdNewSignUpControl> DdNewSignUpControls { get; set; }
        DbSet<DdScheme> DdSchemes { get; set; }
        DbSet<DdSchemeType> DdSchemeTypes { get; set; }
        DbSet<FeeType> FeeTypes { get; set; }
        DbSet<FileType> FileTypes { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<PermissionGrp> PermissionGrps { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Setting> Settings { get; set; }
        DbSet<Title> Titles { get; set; }
        

        int SaveChanges();
    }
}
