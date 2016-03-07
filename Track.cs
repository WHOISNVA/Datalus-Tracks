using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sabio.Web.Domain
{
    public class Track
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string MoreInformation { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public List<CourseBase> Courses { get; set; }
    }
}