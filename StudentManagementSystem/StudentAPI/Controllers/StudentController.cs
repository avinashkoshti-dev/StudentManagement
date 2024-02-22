using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentManagementSystemContext context;

        public StudentController(StudentManagementSystemContext context)
        {
            this.context = context;
        }

        [HttpGet("GetStudents")]
        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            var students = await context.Students.ToListAsync();
            return Ok(students);
        }

        [HttpGet("GetStudentById/{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await context.Students.FindAsync(id);
            if(student==null)
            {
                return NotFound();
            }
            return Ok(student);
        }
        [HttpPost("CreateStudent")]
        public async Task<ActionResult<Student>> CreateStudent(Student objstudent)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            if (objstudent.Id == 0)
            {
                await context.Students.AddAsync(objstudent);
                await context.SaveChangesAsync();
                res["status"] = 1;
                res["message"] = "Student Created Successfully!";
                return Ok(res);
            }
            else {                 
                if(context.Students.Any(x=>x.Id!=objstudent.Id))
                {
                    res["status"] = 0;
                    res["message"] = "Student Not Found!";
                    return BadRequest(res);
                }
                else
                {
                    context.Entry(objstudent).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    res["status"] = 1;
                    res["message"] = "Student Updated Successfully!";
                    return Ok(res);
                }
            }

        }

        [HttpGet("DeleteStudent/{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            var student =  await context.Students.FindAsync(id);
            if (student == null)
            {
                res["status"] = 0;
                res["message"] = "Student Not Found!";
                return BadRequest(res);
            }
            
            context.Entry(student).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            res["success"] = 1;
            res["message"] = "Student Deleted successfully";
            return Ok(res);

        }
    }
}
