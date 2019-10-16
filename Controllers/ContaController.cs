using System;
using System.Collections.Generic;
using System.Linq;
using BancoWeb.View;
using BancoWeb.Model.Entity;
using Microsoft.AspNetCore.Mvc;

namespace BancoWeb.Controllers
{
    [Route("api/conta")]
    [ApiController]
    public class ContaController : ControllerBase
    {
        DbConn dbConn = new DbConn();

        /*
         * Get all Accounts
         * @Method GET
         * */
        [HttpGet]
        public ActionResult<List<ContaView>> Get(){
            return dbConn.Contas.Select(s => new ContaView(){
                id = s.id,
                nomeAgencia = s.agencia.nome,
                nomeCliente = s.cliente.nome,
                saldo = s.saldo
            }).ToList();
        }

        /*
         * Get Account by ID
         * @Method GET
         * @param ID in URI
         * **/
        [HttpGet("{id}")]
        public ActionResult<ContaView> Get(int id)
        {
            return dbConn.Contas.Select(s => new ContaView(){
                id = s.id,
                nomeAgencia = s.agencia.nome,
                nomeCliente = s.cliente.nome,
                saldo = s.saldo
            }).Where(w => w.id.Equals(id)).FirstOrDefault();
        }

        /*
         * Create account
         * @Method POST
         * @param [obj] Account
         * **/
        [HttpPost]
        public ActionResult<Conta> Post([FromBody] Conta value)
        {
            try{
                dbConn.Attach(value.cliente);
                dbConn.Attach(value.agencia);
                dbConn.Contas.Add(value);
                dbConn.SaveChanges();
                return value;
            } catch {
                return BadRequest("Erro ao cadastrar Conta!");
            }
        }

        /*
         * Update account
         * @Method PUT
         * @param [obj] Account
         * **/
        [HttpPut]
        public void Put([FromBody] Conta value){
            dbConn.Contas.Update(value);
            dbConn.SaveChanges();
        }

        /*
         * Delete account
         * @Method DELETE
         * @param accountID
         * **/
        [HttpDelete("{id}")]
        public void Delete(int id){
            var refConta = dbConn.Contas.Where(w => w.id.Equals(id)).FirstOrDefault();

            dbConn.Contas.Remove(refConta);
            dbConn.SaveChanges();
        }

        /*
         * Deposit into an account
         * @Method POST
         * @param balance
         * @param deposit
         * @param accountID
         * **/
        [Route("depositar")]
        [HttpPost]
        public ActionResult<ContaDepositoView> Depositar([FromBody] ContaDepositoView value){
            try{
                var refConta = dbConn.Contas.Where(w=> w.id.Equals(value.id)).FirstOrDefault();
                refConta.Depositar(value.deposito);
                value.saldo = refConta.saldo;
                dbConn.Contas.Update(refConta);
                dbConn.SaveChanges();
                
                return value;
            }catch{
                return BadRequest("Erro ao alterar o saldo da conta!");
            }
        }

        /*
         * Withdraw from an account
         * @Method POST
         * @param accountID
         * @param balance
         * @param withdraw
         * **/
        [Route("sacar")]
        [HttpPost]
        public ActionResult<ContaSaqueView> Sacar([FromBody] ContaSaqueView value){
            try{
                var refConta = dbConn.Contas.Where(w => w.id.Equals(value.id)).FirstOrDefault();
                if(refConta.Sacar(value.saque) == 1){
                    value.saldo = refConta.saldo;
                    dbConn.Update(refConta);
                    dbConn.SaveChanges();
                    return value;
                } else {
                    return BadRequest("Valor de saque maior que o saldo!");
                }
            }catch{
                return BadRequest("Erro ao tentar sacar da conta, devido aos parametros estarem incorretos");
            }
        }
    }
}