//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace compserv
{
    using System;
    using System.Collections.Generic;
    
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            this.Visits = new HashSet<Visits>();
        }
    
        public int ClientID { get; set; }
        public string Lname { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public System.DateTime DateBirth { get; set; }
        public string PhonNumber { get; set; }
        public int VidClient { get; set; }
        public int TypeClient { get; set; }
        public string Adress { get; set; }
    
        public virtual Type_Client Type_Client { get; set; }
        public virtual Vid_Client Vid_Client { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visits> Visits { get; set; }
    }
}
