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
    
    public partial class Courses
    {
        public Courses()
        {
            this.CourseMotions = new HashSet<CourseMotions>();
            this.PatientCourses = new HashSet<PatientCourses>();
            this.PatientSchedule = new HashSet<PatientSchedule>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<CourseMotions> CourseMotions { get; set; }
        public virtual ICollection<PatientCourses> PatientCourses { get; set; }
        public virtual ICollection<PatientSchedule> PatientSchedule { get; set; }
    }
}
