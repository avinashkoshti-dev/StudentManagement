using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var studentlist = await dbcontext.Students.ToListAsync();
            return Ok(studentlist);
        }
        [HttpGet("getstudent/{id}")]
        public async Task<ActionResult<Student>> GetlStudentById(int id)
        {
            var student = await dbcontext.Students.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(student);
        }

        [HttpPost("createstudent")]
        public async Task<ActionResult<Student>> CreateUpdateStudent(Student obj)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            if (obj.Id == 0)
            {
                await dbcontext.Students.AddAsync(obj);
                await dbcontext.SaveChangesAsync();
                res["status"] = 1;
                res["message"] = "Student Created Successfully!";
                return Ok(res);
            }
            else
            {
                if (dbcontext.Students.Any(x => x.Id != obj.Id))
                {
                    res["status"] = 0;
                    res["message"] = "Student Not Found!";
                    return Ok(res);
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

        [HttpGet("deleteemployee/{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            var obj = await dbcontext.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            dbcontext.Students.Remove(obj);
            await dbcontext.SaveChangesAsync();
            res["status"] = 1;
            res["message"] = "Student Deleted Successfully!";
            return Ok(res);
        }
    }
}
