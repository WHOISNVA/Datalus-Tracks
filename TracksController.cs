using Sabio.Web.Domain;
using Sabio.Web.Models.ViewModels;
using Sabio.Web.Services;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sabio.Web.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("Tracks")]
    public class TracksController : BaseController
    {
        ICoursesService _coursesService;

        public TracksController(ICoursesService coursesService)
        {
            _coursesService = coursesService;
        }

        [Route("Create")]
        [Route("{id:int}/edit")]
        public ActionResult Create(int id = 0)
        {
            TrackViewModel model = new TrackViewModel();
            model = DecorateViewModel(model);
            model.Id = id;
            model.Courses = _coursesService.GetAll();
            return View(model);
        }

        [Route("~/")]  // Override Route Prefix to make this the endpoint for the entire site.
        [Route("")]
        public ActionResult Index()
        {
            BaseViewModel model = new BaseViewModel();
            model = DecorateViewModel(model);
            return View(model);
        }
    }
}