using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PracticeAPI.Models;
using PracticeAPI.Services;

namespace PracticeAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ParentsController : ControllerBase
    {
        private readonly iService<ParentA> _parentAService;
        private readonly iService<ParentB> _parentBService;

        public ParentsController(iService<ParentA> parentAService,
            iService<ParentB> parentBService)
        {
            _parentAService = parentAService;
            _parentBService = parentBService;
        }

        [HttpGet("{ParentType}")]
        public ActionResult<IEnumerable<Parent>> GetParents(string parentType)
        {
            if (parentType == "A")
                return _parentAService.GetAllWithData();
            else if (parentType == "B")
                return _parentBService.GetAllWithData();
            else
                return BadRequest("Debe especificar el tipo de parent");
        }

        [HttpGet("{ParentType}/{id}")]
        public async Task<ActionResult<Parent>> GetParent(string parentType, Guid id)
        {
            if (parentType == "A")
                return await _parentAService.GetByID(id);
            else if (parentType == "B")
                return await _parentBService.GetByID(id);
            else
                return BadRequest("Debe especificar el tipo de parent");
        }

        [HttpPost("{ParentType}")]
        public async Task<ActionResult<Parent>> CreateParent(string parentType, Parent parent)
        {
            if (parentType == "A")
                return await _parentAService.Create(EntityConverter.ConvertEntity<ParentA>(parent));
            else if (parentType == "B")
                return await _parentBService.Create(EntityConverter.ConvertEntity<ParentB>(parent));
            else
                return BadRequest("Debe especificar el tipo de parent");
        }

        [HttpPut("{ParentType}/{id}")]
        public async Task<ActionResult<Parent>> UpdateParent(string parentType, Guid id, Parent parent)
        {
            if (id != parent.Id)
                return BadRequest();

            Parent updatedEntity;

            if (parentType == "A")
                updatedEntity = await _parentAService.Update(EntityConverter.ConvertEntity<ParentA>(parent));
            else if (parentType == "B")
                updatedEntity = await _parentBService.Update(EntityConverter.ConvertEntity<ParentB>(parent));
            else
                return BadRequest("Debe especificar el tipo de parent");

            if (updatedEntity == null)
                return NotFound();

            return updatedEntity;
        }

        [HttpDelete("{ParentType}/{id}")]
        public async Task<IActionResult> DeleteParent(string parentType, Guid id)
        {
            if (parentType == "A")
            {
                if (await _parentAService.GetByID(id) == null)
                    return NotFound();

                await _parentAService.Delete(id);
            }
            else if (parentType == "B")
            {
                if (await _parentBService.GetByID(id) == null)
                    return NotFound();

                await _parentBService.Delete(id);
            }
            else
                return BadRequest("Debe especificar el tipo de parent");

            return NoContent();
        }
    }
}