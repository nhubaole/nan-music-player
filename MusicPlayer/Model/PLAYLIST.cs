//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MusicPlayer.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PLAYLIST
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PLAYLIST()
        {
            this.SONGs = new HashSet<SONG>();
        }
    
        public int PLAYLISTID { get; set; }
        public string PLAYLISTNAME { get; set; }
        public int USERID { get; set; }
    
        public virtual USER USER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SONG> SONGs { get; set; }
    }
}
