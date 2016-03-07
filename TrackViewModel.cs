using Sabio.Web.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.ViewModels
{
    public class TrackViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public List<Course> Courses { get; set; }
    }
}