using Sabio.Web.Domain;
using Sabio.Web.Models.Requests.Tracks;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/Track")]
    public class TracksApiController : ApiController
    {
        ITrackService _trackService;

        public TracksApiController(ITrackService trackService)
        {
            _trackService = trackService;
        }

        [Route("EchoTrack"), HttpPost]
        public HttpResponseMessage EchoTracks(TracksAddRequest model)
        {
            return Request.CreateResponse(model);
        }

        [Route, HttpPost]
        public HttpResponseMessage Add(TracksAddRequest model)
        {
            if (ModelState.IsValid)
            {
                ItemResponse<int> response = new ItemResponse<int>();
                response.Item = _trackService.Insert(model);
                return Request.CreateResponse(response);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("{id:int}"), HttpPut]
        public HttpResponseMessage Update(TracksEditRequest model, int id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            SuccessResponse response = new SuccessResponse();
            _trackService.Update(model, id);
            return Request.CreateResponse(response);
        }


        [Route("{id:int}"), HttpGet]
        public HttpResponseMessage TracksInfo(int id)
        {
            Track track = _trackService.GetById(id);
            ItemResponse<Track> response = new ItemResponse<Track>();
            response.Item = track;
            return Request.CreateResponse(response);
        }

        [Route, HttpGet]
        public HttpResponseMessage GetAll()
        {
            ItemsResponse<Track> response = new ItemsResponse<Track>();
            response.Items = _trackService.GetAll();
            return Request.CreateResponse(response);
        }

        [Route("{id:int}"), HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                _trackService.Delete(id);
                SuccessResponse response = new SuccessResponse();
                return Request.CreateResponse(response);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
