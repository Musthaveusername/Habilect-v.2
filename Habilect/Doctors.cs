//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Habilect
{
    using System;
    using System.Collections.Generic;
    
    public partial class Doctors
    {
        public Doctors()
        {
            this.Patients = new HashSet<Patients>();
        }
    
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int AdminId { get; set; }
        public string AspNetUserId { get; set; }
    
        public virtual Admins Admins { get; set; }
        public virtual ICollection<Patients> Patients { get; set; }
    }
}
