using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using FaciTWebApi.Helpers;
using FaciTWebApi.Models;

namespace FaciTUserWebApi.Controllers
{
    [Authorize]
    public class FaciTUsersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IHttpActionResult Post([FromBody] FaciTUser facitUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stream = new MemoryStream(facitUser.ImageArray);
            var guid = Guid.NewGuid().ToString();
            var file = String.Format("{0}.jpg", guid);
            var folder = "~/Content/Users";
            var fullPath = String.Format("{0}/{1}", folder, file);
            var response = FilesHelper.UploadPhoto(stream, folder, file);
            if (response)
            {
                facitUser.ImagePath = fullPath;
            }
            var user = new FaciTUser()
            {
                UserName = facitUser.UserName,
                UserGroup = facitUser.UserGroup,
                Email = facitUser.Email,
                Phone = facitUser.Phone,
                Region = facitUser.Region,
                Date = facitUser.Date,
                ImagePath = facitUser.ImagePath
            };

            db.FaciTUsers.Add(user);
            db.SaveChanges();
            return StatusCode(HttpStatusCode.Created);

        }

        public IEnumerable<FaciTUser> Get(string userGroup, string region)
        {
            var facitUsers = db.FaciTUsers.Where(user => user.UserGroup.StartsWith(userGroup) && user.Region.StartsWith(region));
            return facitUsers;
        }

        public IEnumerable<FaciTUser> Get()
        {
            var latestUsers = db.FaciTUsers.OrderByDescending(user => user.Date);
            return latestUsers;
        }

    }
}