using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.Requests.Tracks
{
    public class TracksEditRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string MoreInformation { get; set; }
        [Required]
        public List<int> CourseIds { get; set; }
    }
}