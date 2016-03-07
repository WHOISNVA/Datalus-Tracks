using Sabio.Data;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests.Tracks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services
{
    public class TrackService : BaseService, ITrackService
    {
        public int Insert(TracksAddRequest model)
        {
            int id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Tracks_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Name", model.Name);
                   paramCollection.AddWithValue("@Description", model.Description);
                   paramCollection.AddWithValue("@MoreInformation", model.MoreInformation);
                   paramCollection.AddWithValue("@UserId", UserService.GetCurrentUserId());

                   SqlParameter newIdParameter = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                   newIdParameter.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(newIdParameter);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@Id"].Value.ToString(), out id);
               }
               );

            if (model.CourseIds != null)
            {
                foreach (int trackCourseId in model.CourseIds)
                {
                    DataProvider.ExecuteNonQuery(GetConnection, "dbo.TrackCourses_Insert"
                        , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@TrackId", id);
                            paramCollection.AddWithValue("@CourseId", trackCourseId);
                            paramCollection.AddWithValue("@UserId", UserService.GetCurrentUserId());

                        }, returnParameters: null
                       );
                }
            }
            return id;
        }


        /*EDIT*/ //TODO: what up with tags?
        public void Update(TracksEditRequest model, int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Tracks_Update"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@id", id);
                   paramCollection.AddWithValue("@Name", model.Name);
                   paramCollection.AddWithValue("@Description", model.Description);
                   paramCollection.AddWithValue("@MoreInformation", model.MoreInformation);
                   paramCollection.AddWithValue("@UserId", UserService.GetCurrentUserId());
               }, returnParameters: null
               );

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.TrackCourses_DeleteByTrackId"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@TrackId", id);
                }, returnParameters: null);

            if (model.CourseIds != null)
            {
                foreach (int trackCourseId in model.CourseIds)
                {
                    DataProvider.ExecuteNonQuery(GetConnection, "dbo.TrackCourses_Insert"
                        , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                        {
                            paramCollection.AddWithValue("@TrackId", id);
                            paramCollection.AddWithValue("@CourseId", trackCourseId);
                            paramCollection.AddWithValue("@UserId", UserService.GetCurrentUserId());

                        }, returnParameters: null
                       );
                }
            }
        }

        public Track GetById(int id)
        {
            Track track = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Tracks_SelectById"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);
                }
                , map: delegate (IDataReader reader, short resultSetNumber)
                {
                    if (resultSetNumber == 0)
                    {
                        track = MapTrackPage(reader);
                    }
                    else if (resultSetNumber == 1)
                    {
                        int columnOrdPosition = 0;
                        int trackId = reader.GetSafeInt32(columnOrdPosition++);

                        CourseBase course = new CourseBase();
                        course.Id = reader.GetSafeInt32(columnOrdPosition++);
                        course.Name = reader.GetSafeString(columnOrdPosition++);
                        if (track.Courses == null)
                        {
                            track.Courses = new List<CourseBase>();
                        }
                        track.Courses.Add(course);
                    }
                }
                );
            return track;
        }

        public void Delete(int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.TrackCourses_DeleteByTrackId"
                       , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                       {
                           paramCollection.AddWithValue("@TrackId", id);
                       }, returnParameters: null
                       );

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Tracks_Delete"
               , inputParamMapper: delegate (SqlParameterCollection parameters)
               {
                   parameters.AddWithValue("@Id", id);
               }, returnParameters: null
            );
        }

        public List<Track> GetAll()
        {
            List<Track> trackCollection = null;
            Dictionary<int, Track> trackDictionary = null;
            DataProvider.ExecuteCmd(GetConnection, "dbo.Tracks_SelectAll"
                , inputParamMapper: null
                , map: delegate (IDataReader reader, short resultSetNumber)
                {
                    if (resultSetNumber == 0)
                    {
                        Track track = MapTrackPage(reader);
                        if (trackCollection == null)
                        {
                            trackCollection = new List<Track>();
                        }
                        if (trackDictionary == null)
                        {
                            trackDictionary = new Dictionary<int, Track>();
                        }
                        trackCollection.Add(track);
                        trackDictionary.Add(track.Id, track);
                    }
                    else if (resultSetNumber == 1)
                    {
                        int columnOrdPosition = 0;
                        int trackId = reader.GetSafeInt32(columnOrdPosition++);

                        CourseBase course = new CourseBase();
                        course.Id = reader.GetSafeInt32(columnOrdPosition++);
                        course.Name = reader.GetSafeString(columnOrdPosition++);
                        course.Description = reader.GetSafeString(columnOrdPosition++);
                        Track parentTrackPage = trackDictionary[trackId];
                        if (parentTrackPage.Courses == null)
                        {
                            parentTrackPage.Courses = new List<CourseBase>();
                        }
                        parentTrackPage.Courses.Add(course);
                    }
                }
                );
            return trackCollection;
        }

        private Track MapTrackPage(IDataReader reader)
        {
            Track track = new Track();
            int startingIndex = 0;
            track.Id = reader.GetSafeInt32(startingIndex++);
            track.Name = reader.GetSafeString(startingIndex++);
            track.Description = reader.GetSafeString(startingIndex++);
            track.MoreInformation = reader.GetSafeString(startingIndex++);
            return track;
        }
    }
}