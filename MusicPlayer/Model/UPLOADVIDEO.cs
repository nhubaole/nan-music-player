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
    
    public partial class UPLOADVIDEO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UPLOADVIDEO()
        {
            this.USERS = new HashSet<USER>();
        }
    
        public int VIDEOID { get; set; }
        public string VIDEONAME { get; set; }
        public string SINGERNAME { get; set; }
        public string IMAGEPATH { get; set; }
        public string SAVEPATH { get; set; }
        public Nullable<double> DURATION { get; set; }
        public Nullable<double> POSITION { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER> USERS { get; set; }
    }
}
