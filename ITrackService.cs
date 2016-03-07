using System.Collections.Generic;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests.Tracks;

namespace Sabio.Web.Services
{
    public interface ITrackService
    {
        void Delete(int id);
        List<Track> GetAll();
        Track GetById(int id);
        int Insert(TracksAddRequest model);
        void Update(TracksEditRequest model, int id);
    }
}