using System;
using System.Threading.Tasks;
using CityProject_WebAPI.Data;
using CityProject_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using ClosedXML.Excel;

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

        [HttpPost]
        [Route("enviarexcelfile")]
        public async Task<IActionResult> ExcelFile(IFormFile file)
        {
            using (XLWorkbook workBook = new XLWorkbook(file.OpenReadStream()))
            {
                IXLWorksheet workSheet = workBook.Worksheet(1);

                DataTable dt = new DataTable();

                bool firstRow = true;
                foreach (IXLRow row in workSheet.Rows())
                {
                    
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        
                        dt.Rows.Add();
                        int i = 0;

                        foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }  
                }
                string excelString = JsonConvert.SerializeObject(dt);
                 var results = JsonConvert.DeserializeObject<List<Cidade>>(excelString);
                foreach (var item in results)
                {
                    await post(item);
                    
                }
            }
            return Ok();
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