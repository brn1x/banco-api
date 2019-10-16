using System;
using System.Collections.Generic;
using System.Linq;
using BancoWeb.Model.Entity;
using Microsoft.AspNetCore.Mvc;

namespace BancoWeb.Controllers
{
    [Route("api/cliente")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        DbConn dbConn = new DbConn();
        
        [HttpGet]
        public ActionResult<IEnumerable<Cliente>> Get(){
            return dbConn.Clientes.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Cliente> Get(int id)
        {
            return dbConn.Clientes.Where(w => w.id.Equals(id)).FirstOrDefault();
        }

        [HttpPost]
        public void Post([FromBody] Cliente value)
        {
            dbConn.Clientes.Add(value);
            dbConn.SaveChangesAsync();
        }

        [HttpPut]
        public void Put([FromBody] Cliente value)
        {
            dbConn.Clientes.Update(value);
            dbConn.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public void Delete(int id){
            var refCliente = dbConn.Clientes.Where(w => w.id.Equals(id)).FirstOrDefault();

            dbConn.Clientes.Remove(refCliente);
            dbConn.SaveChangesAsync();
        }
    }
}