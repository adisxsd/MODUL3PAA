using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MODUL_PAA_2043.Models;

namespace MODUL_PAA_2043.Controllers
{
    public class PersonController : Controller
    {
        private string __constr;
        private string __ErrorMsg;
        private readonly List<Person> persons;
        public PersonController(IConfiguration configuration)
        {
            __constr = configuration.GetConnectionString("WebApiDatabase");
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("api/person")]
        public ActionResult<Person> ListPerson()
        {
            PersonContext context = new PersonContext(this.__constr);
            List<Person> ListPerson = context.ListPerson();
            return Ok(ListPerson);
        }

        [HttpPost("api/person_auth"), Authorize]
        public ActionResult<Person> ListPersonWithAuth()
        {
            PersonContext context = new PersonContext(this.__constr);
            List<Person> ListPerson = context.ListPerson();
            return Ok(ListPerson);
        }
        [HttpPost("api/person")]
        public ActionResult CreatePerson([FromBody] Person newPersonData)
        {
            if (newPersonData == null)
            {
                return BadRequest("Masukkan Data");
            }

            PersonContext context = new PersonContext(this.__constr);
            context.AddPerson(newPersonData);

            return Ok(newPersonData);
        }
        [HttpPut("{id_person}")]
        public ActionResult UpdatePerson(int id_person, [FromBody] Person updatePersonData)
        {
            if (updatePersonData == null)
            {
                return BadRequest("Belum ada perubahan");
            }

            updatePersonData.id_person = id_person;

            PersonContext context = new PersonContext(this.__constr);
            context.UpdatePerson(updatePersonData);


            return Ok(updatePersonData);
        }

        [HttpDelete("{id_person}")]
        public ActionResult DeletePerson(int id_person, [FromBody] Person deletePersonData)
        {
            if (deletePersonData == null)
            {
                return BadRequest("Tidak ada data");
            }

            deletePersonData.id_person = id_person;
            PersonContext context = new PersonContext(this.__constr);
            context.DeletePerson(deletePersonData);
            return Ok(deletePersonData);
        }
    }
}
