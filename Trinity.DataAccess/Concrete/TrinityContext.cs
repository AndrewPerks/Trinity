using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Trinity.DataAccess.Interfaces;
using Trinity.Model;

namespace Trinity.DataAccess.Concrete
{
    /// <summary>
    /// Custom DB Context
    /// </summary>
    public class TrinityContext : DbContext, ITrinityContext
    {

        public TrinityContext(): base("TrinityContext")
        {
            
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientAttribute> ClientAttributes { get; set; }
        public DbSet<ClientAttributeValue> ClientAttributeValues { get; set; }
        public DbSet<ClientCategory> ClientCategories { get; set; }
        public DbSet<ClientFee> ClientFees { get; set; }
        public DbSet<ClientNote> ClientNotes { get; set; }
        public DbSet<ClientNoteCategory> ClientNoteCategories { get; set; }
        public DbSet<ClientNoteTag> ClientNoteTags { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Contact_Permission_Mapping> ContactPermissionMappings { get; set; }
        public DbSet<ContactCategory> ContactCategories { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<DdInstruction> DdInstructions { get; set; }
        public DbSet<DdInstructionAttribute> DdInstructionAttributes { get; set; }
        public DbSet<DdInstructionAttributeValue> DdInstructionAttributeValues { get; set; }
        public DbSet<DdInstructionNote> DdInstructionNotes { get; set; }
        public DbSet<DdInstructionNoteCategory> DdInstructionNoteCategories { get; set; }
        public DbSet<DdNewSignUpControl> DdNewSignUpControls { get; set; }
        public DbSet<DdScheme> DdSchemes { get; set; }
        public DbSet<DdSchemeType> DdSchemeTypes { get; set; }
        public DbSet<FeeType> FeeTypes { get; set; }
        public DbSet<FileType> FileTypes { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionGrp> PermissionGrps { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Title> Titles { get; set; }        

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            ObjectContext ctx = ((IObjectContextAdapter)this).ObjectContext;
            List<ObjectStateEntry> objectStateEntryList =
                ctx.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified |
                                                             EntityState.Deleted).ToList();

            foreach (ObjectStateEntry entry in objectStateEntryList)
            {
                if (!entry.IsRelationship)
                {

                }
            }
            var result = base.SaveChanges();

            return result;
        }

        public static TrinityContext Create()
        {
            return new TrinityContext();
        }
    }
}
