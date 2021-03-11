using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CityProject_WebAPI.Data;
using CityProject_WebAPI.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using OfficeOpenXml;

namespace CityProject_WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CidadeController : ControllerBase
    {
        public IRepository _repo { get; }

        public Cidade cidadeArquivo = new Cidade();

        public CidadeController(IRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _repo.GetAllCidadesAsync(true);
                return Ok(result);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou " + e.Message );
            }
        }

        [HttpGet("{cidadeId}")]
         public async Task<IActionResult> GetById(int cidadeId)
        {
            try
            {
                
                var result = await _repo.GetCidadesAsyncById(cidadeId, true);
                return Ok(result);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou " + e.Message );
            }
        }

        [HttpGet("ByEstado/{estadoId}")]
        public async Task<IActionResult> getCidadeByEstadoId(int estadoId){
            try
            {
                var estado = await _repo.GetEstadosAsyncById(estadoId, false);
                if (estado == null)
                    return BadRequest("Estado não encontrado");
                
                var result = await _repo.GetCidadesAsyncByEstadoId(estadoId);
                return Ok(result);
                
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou " + e.Message );
            }
        }

        [HttpPost]
        public async Task<IActionResult> post(Cidade model)
        {
            try
            {
                var cidades = await _repo.GetAllCidadesAsync(false);
                foreach (var item in cidades)
                {
                    if(model.Nome.ToUpper() == item.Nome.ToUpper() && model.EstadoId == item.EstadoId)
                    {
                        return BadRequest("Cidade já cadastrada nesse estado");
                    }
                }
                var parametroCusto = await _repo.GetParametroCusto();
                
                if (parametroCusto == null)
                {
                    return BadRequest("Parametro custo não possui informações para os cálculos.");  
                }

                 if(model.Populacao > parametroCusto.ValorCorte)
                 {
                    model.CustoCidadeUS = (parametroCusto.ValorCorte * parametroCusto.PorPessoa) 
                                            + ((model.Populacao - parametroCusto.ValorCorte) * parametroCusto.PorPessoa
                                            - (parametroCusto.PorPessoa * (model.Populacao * parametroCusto.Desconto)/100));
                 }
                else  
                {
                    model.CustoCidadeUS = (model.Populacao * parametroCusto.PorPessoa);
                }
                
                var estado = await _repo.GetEstadosAsyncById(model.EstadoId, false);

                estado.CustoEstadoUS += model.CustoCidadeUS;
                estado.Populacao += model.Populacao;

                _repo.Add(model);
                _repo.Update(estado);

                if(await _repo.SaveChangesAsync())
                {
                    return Ok(model);
                }                
            }
           catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou " + e.Message );
            }
            return BadRequest();
        }

        [HttpPut ("{cidadeId}")]
        public async Task<IActionResult> put(int cidadeId, Cidade model)
        {
            try
            {
                var parametroCusto = await _repo.GetParametroCusto();
                if(model.Populacao > parametroCusto.ValorCorte)
                {
                    model.CustoCidadeUS = (parametroCusto.ValorCorte * parametroCusto.PorPessoa) 
                                            + ((model.Populacao - parametroCusto.ValorCorte) 
                                            - ((model.Populacao * parametroCusto.Desconto)/100));
                }
                else  
                {
                    model.CustoCidadeUS = (model.Populacao * parametroCusto.PorPessoa);
                }

                // if (estado.Populacao < model.Populacao)
                // {
                //     estado.Populacao += (model.Populacao - estado.Populacao);
                //     estado.CustoEstadoUS += (model.CustoCidadeUS - estado.CustoEstadoUS);
                // }
                // else
                // {

                //     estado.Populacao -= (estado.Populacao - model.Populacao);
                //     estado.CustoEstadoUS -= (estado.CustoEstadoUS - model.CustoCidadeUS);
                // }

                // model.Estado = estado;

                _repo.Update(model);
                

                 if(await _repo.SaveChangesAsync())
                {
                    return Ok(model);
                }  
            }
           catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou " + e.Message );
            }
            return BadRequest();
        }

        [HttpDelete("{cidadeId}")]
        public async Task<IActionResult> delete(int cidadeId)
        {
            try
            { 
                var cidade = await _repo.GetCidadesAsyncById(cidadeId, false);
                var estado = await _repo.GetEstadosAsyncById(cidade.EstadoId, false);
                if (cidade == null)
                    return NotFound("Cidade não encontrada");

                if (estado.Nome == "Rio Grande do Sul")
                {
                    return BadRequest("Não é possivel remover essa cidade");
                }

                estado.CustoEstadoUS -= cidade.CustoCidadeUS;
                estado.Populacao -= cidade.Populacao;

                _repo.Update(estado);
                _repo.Delete(cidade);

                if(await _repo.SaveChangesAsync())
                {
                    return Ok(new {message = "Deletado"});
                }                
            }
           catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou " + e.Message );
            }
            return BadRequest();
        }
    }   
}