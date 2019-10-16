using System;
using System.Collections.Generic;
using System.Linq;
using BancoWeb.Model.Entity;
using BancoWeb.View;
using Microsoft.AspNetCore.Mvc;


namespace BancoWeb.Controllers
{
    [Route("api/agencia")]
    [ApiController]
    public class AgenciaController : ControllerBase
    {
        DbConn dbConn = new DbConn();

        [HttpGet]
        public ActionResult<IEnumerable<AgenciaView>> Get(){
            return dbConn.Agencias.Select(s => new AgenciaView() {
                nomeAgencia = s.nome,
                id = s.id
            }).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<AgenciaView> Get(int id)
        {
            return dbConn.Agencias.Select(s => new AgenciaView() {
                id = s.id,
                nomeAgencia = s.nome
            }).Where(w => w.id.Equals(id)).FirstOrDefault();
        }

        [Route("true/{id}")]
        [HttpGet]
        public ActionResult<Agencia> GetTrue(int id)
        {
            return dbConn.Agencias.Where(busca => busca.id.Equals(id)).FirstOrDefault();
        }

        [HttpPost]
        public ActionResult<Agencia> Post([FromBody] Agencia value)
        {
            dbConn.Agencias.Add(value);
            dbConn.SaveChangesAsync();
            return value;
        }

        [HttpPut]
        public void Put([FromBody] Agencia value)
        {
            dbConn.Agencias.Update(value);
            dbConn.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public void Delete(int id){
            var refAgencia = dbConn.Agencias.Where(w => w.id.Equals(id)).FirstOrDefault();

            dbConn.Agencias.Remove(refAgencia);
            dbConn.SaveChangesAsync();
        }        
    }
}