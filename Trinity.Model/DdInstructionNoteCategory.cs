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
    
    public partial class DdInstructionNoteCategory
    {
        public DdInstructionNoteCategory()
        {
            this.DdInstructionNotes = new HashSet<DdInstructionNote>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<DdInstructionNote> DdInstructionNotes { get; set; }
    }
}
