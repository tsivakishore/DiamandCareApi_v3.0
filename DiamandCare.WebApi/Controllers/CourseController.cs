using DiamandCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DiamandCare.WebApi
{
    [RoutePrefix("api/coursemaster")]
    public class CourseController : ApiController
    {
        private CourseRepository _repo = null;
        public CourseController(CourseRepository repository)
        {
            _repo = repository;
        }

        [Authorize]
        [Route("GetCourseMasterDetails")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<CourseMasterViewModel>>> GetCourseMasterDetails()
        {
            Tuple<bool, string, List<CourseMasterViewModel>> result = null;
            try
            {
                result = await _repo.GetCourseMasterDetails();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("CreateCourseMaster")]
        [HttpPost]
        public async Task<Tuple<bool, string>> CreateCourseMaster(CourseMasterModel courseMasterModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.CreateCourseMaster(courseMasterModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GetCourseDetailsByCourseMasterID")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<CourseViewModel>>> GetCourseDetailsByCourseMasterID(int CourseMasterID)
        {
            Tuple<bool, string, List<CourseViewModel>> result = null;
            try
            {
                result = await _repo.GetCourseDetailsByCourseMasterID(CourseMasterID);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("CreateCourse")]
        [HttpPost]
        public async Task<Tuple<bool, string>> CreateCourse(CourseModel courseModel)
        {
            Tuple<bool, string> result = null;
            try
            {
                result = await _repo.CreateCourse(courseModel);
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }

        [Authorize]
        [Route("GetCourses")]
        [HttpGet]
        public async Task<Tuple<bool, string, List<CoursesModel>>> GetCourses()
        {
            Tuple<bool, string, List<CoursesModel>> result = null;
            try
            {
                result = await _repo.GetCourses();
            }
            catch (Exception ex)
            {
                ErrorLog.Write(ex);
            }

            return result;
        }
    }
}
