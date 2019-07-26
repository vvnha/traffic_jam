using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace UpServer.Controllers
{
    [RoutePrefix("api/image")]
    public class ImageController : ApiController
    {
        SqlCommand cmd;
        SqlConnection con;
        [Route("upimg")]
        [HttpPost]
        public HttpResponseMessage Upload()
        {
            try
            {
                var request = HttpContext.Current.Request;
                var description = request.Form["description"];
                var photo = request.Files["photo"];
                var imgPath = HttpContext.Current.Server.MapPath("~/Content/Uploads/" + photo.FileName);
                AddImage(photo.FileName);
                photo.SaveAs(imgPath);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public List<upimage> GetFoodLists()
        {
            DBImageDataContext db = new DBImageDataContext();
            return db.upimages.ToList();
        }
        [HttpGet]
        public upimage GetFood(int id)
        {
            DBImageDataContext db = new DBImageDataContext();
            return db.upimages.FirstOrDefault(x => x.imgID == id);
        }
        public void AddImage(String name)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = @"Server=desktop-8n0o4eo\mssqlserver1;Database=ImageUpload;User ID=sa;Password=nhavovvv;";
            //SqlCommand sqlCmd = new SqlCommand("INSERT INTO tblEmployee (EmployeeId,Name,ManagerId) Values (@EmployeeId,@Name,@ManagerId)", myConnection);  
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "INSERT INTO upimage Values (@Name)";
            sqlCmd.Connection = myConnection;

            sqlCmd.Parameters.AddWithValue("@Name", name);

            myConnection.Open();
            int rowInserted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}
