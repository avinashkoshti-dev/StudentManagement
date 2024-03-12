using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly StudentManagementSystemContext dbcontext;

        public StudentAPIController(StudentManagementSystemContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        [HttpGet("getallstudents")]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            var studentslist = await dbcontext.Students.ToListAsync();
            return Ok(studentslist);
        }
        [HttpGet("getstudent/{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            var student = await dbcontext.Students.SingleOrDefaultAsync(x => x.Id == id);
            if(student==null)
            {
                res["status"] = 0;
                res["message"] = "Student Not Found!";
                return NotFound(res);
            }
            return Ok(student); 
        }

        [HttpPost("createstudent")]
        public async Task<ActionResult<Student>> CreateStudent(Student obj) {
            Dictionary<string, object> res = new Dictionary<string, object>();
            if(obj.Id==0)
            {
                await dbcontext.Students.AddAsync(obj);
                await dbcontext.SaveChangesAsync();
                res["status"] = 1;
                res["message"] = "Student Created Successfully!";
                return Ok(res);
            }
            else
            {
                if(dbcontext.Students.Any(x=>x.Id!=obj.Id && x.FirstName == obj.FirstName))
                {
                    res["status"] = 0;
                    res["message"] = "Student Not Found!";
                    return NotFound(res);
                }
                else
                {
                    dbcontext.Entry(obj).State = EntityState.Modified;
                    await dbcontext.SaveChangesAsync();
                    res["status"] = 1;
                    res["message"] = "Student Updated Successfully!";
                    return Ok(res);
                }
            }
        }

        [HttpDelete("deletestudent/{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            Student? obj = await dbcontext.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
            {
                res["status"] = 0;
                res["message"] = "Student Not Found!";
                return NotFound(res);
            }
            else
            {
                dbcontext.Students.Remove(obj);
                await dbcontext.SaveChangesAsync();
                res["status"] = 1;
                res["message"] = "Student Deleted Successfully!";
                return Ok(res);
            }

        }
    }
}
