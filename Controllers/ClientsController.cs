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
    public class ClientsController : ControllerBase
    {
        private readonly iService<PrivateClient> _privateClientService;
        private readonly iService<PublicClient> _publicClientService;

        public ClientsController(iService<PrivateClient> privateClientService,
            iService<PublicClient> publicClientService)
        {
            _privateClientService = privateClientService;
            _publicClientService = publicClientService;
        }

        [HttpGet("{ClientType}")]
        public ActionResult<IEnumerable<Client>> GetClients(string clientType)
        {
            if (clientType == "Private")
                return _privateClientService.GetAllWithData();
            else if (clientType == "Public")
                return _publicClientService.GetAllWithData();
            else
                return BadRequest("Debe especificar el tipo de cliente");
        }

        [HttpGet("{ClientType}/{id}")]
        public async Task<ActionResult<Client>> GetClient(string clientType, Guid id)
        {
            if (clientType == "Private")
                return await _privateClientService.GetByID(id);
            else if (clientType == "Public")
                return await _publicClientService.GetByID(id);
            else
                return BadRequest("Debe especificar el tipo de cliente");
        }

        [HttpPost("{ClientType}")]
        public async Task<ActionResult<Client>> CreateClient(string clientType, Client client)
        {
            if (clientType == "Private")
                return await _privateClientService.Create(EntityConverter.ConvertEntity<PrivateClient>(client));
            else if (clientType == "Public")
                return await _publicClientService.Create(EntityConverter.ConvertEntity<PublicClient>(client));
            else
                return BadRequest("Debe especificar el tipo de cliente");
        }

        [HttpPut("{ClientType}/{id}")]
        public async Task<ActionResult<Client>> UpdateClient(string clientType, Guid id, Client client)
        {
            if (id != client.Id)
                return BadRequest();

            Client updatedEntity;

            if (clientType == "Private")
                updatedEntity = await _privateClientService.Update(EntityConverter.ConvertEntity<PrivateClient>(client));
            else if (clientType == "Public")
                updatedEntity = await _publicClientService.Update(EntityConverter.ConvertEntity<PublicClient>(client));
            else
                return BadRequest("Debe especificar el tipo de cliente");

            if (updatedEntity == null)
                return NotFound();

            return updatedEntity;
        }

        [HttpDelete("{ClientType}/{id}")]
        public async Task<IActionResult> DeleteClient(string clientType, Guid id)
        {
            if (clientType == "Private")
            {
                if (await _privateClientService.GetByID(id) == null)
                    return NotFound();

                await _privateClientService.Delete(id);
            }
            else if (clientType == "Public")
            {
                if (await _publicClientService.GetByID(id) == null)
                    return NotFound();

                await _publicClientService.Delete(id);
            }
            else
                return BadRequest("Debe especificar el tipo de cliente");

            return NoContent();
        }
    }
}