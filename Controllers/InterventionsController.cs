using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Newtonsoft.Json.Linq;
namespace TodoApi.Controllers
{
    [Route("api/intervention")]
    [ApiController]
    public class InterventionsController : ControllerBase {
        private readonly MysqlContext _context;

        public InterventionsController(MysqlContext context)
        {
            _context = context;
        }

        // GET api/intervention
       [HttpGet]
        public async Task<ActionResult<IEnumerable<interventions>>> GetInterventions()
        {
            return await _context.interventions
                .Where(intervention => intervention.start_intervention == null 
                    && intervention.status == "Pending")
                .ToListAsync();
        }


        // GET: api/interventions/id?
        [HttpGet("{id}")]
        public async Task<ActionResult<interventions>> GetById(long id)
        {
            var intervId = await _context.interventions.FindAsync(id);

            if (intervId == null)
            {
                return NotFound();
            }

            return intervId;
        }



        // PUT: api/interventions/id/inProgress
        [HttpPut("{id}/inProgress")]
        public async Task<IActionResult> StatusInProgress(long id) {
            var interv = _context.interventions.Find (id);
            if (interv == null) {
                return NotFound ();
            }

            interv.status = "InProgress";
            interv.start_intervention = DateTime.Now;

            _context.interventions.Update(interv);
            _context.SaveChanges();
            var json = new JObject();
            json["message"] = "The status and start time of intervention id: " + interv.id + " have been modified. Status now showing: " + interv.status;
            return Content(json.ToString(), "application/json");
        }



        // PUT: api/interventions/id/Completed
        [HttpPut("{id}/Completed")]
        public async Task<IActionResult> StatusCompleted(long id) {
            var interv = _context.interventions.Find (id);
            if (interv == null) {
                return NotFound ();
            }

            interv.status = "Completed";
            interv.end_intervention = DateTime.Now;

            _context.interventions.Update(interv);
            _context.SaveChanges();
            var json = new JObject();
            json["message"] = "The status and end time of intervention " + interv.id + " have been modified. Status now showing: " + interv.status;
            return Content(json.ToString(), "application/json");
        }
    }
}
