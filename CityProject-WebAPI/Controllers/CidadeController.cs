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
            return BadRequest();
        }

        [HttpPost("arquivo")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("O arquivo não é valido");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream).ConfigureAwait(false);

                using (var package = new OfficeOpenXml.ExcelPackage(memoryStream))
                {
                    for (int i = 1; i <= package.Workbook.Worksheets.Count; i++)
                    {
                        var totalRows = package.Workbook.Worksheets[i].Dimension?.Rows;
                        var totalCollumns = package.Workbook.Worksheets[i].Dimension?.Columns;
                        for (int j = 1; j <= totalRows.Value; j++)
                        {
                            for (int k = 1; k <= totalCollumns.Value; k++)
                            {
                                cidadeArquivo.Nome = package.Workbook.Worksheets[i].Cells[j, k].Value.ToString();
                                cidadeArquivo.EstadoId = int.Parse(package.Workbook.Worksheets[i].Cells[j, k].Value.ToString());
                                cidadeArquivo.Populacao = int.Parse(package.Workbook.Worksheets[i].Cells[j, k].Value.ToString());

                                try
                                {
                                    var cidade = await _repo.GetAllCidadesAsync(false);
                                    foreach (var item in cidade)
                                    {
                                        
                                        if(item.Nome.ToUpper() != cidadeArquivo.Nome.ToUpper() && item.EstadoId != cidadeArquivo.EstadoId)
                                        {

                                            var parametroCusto = await _repo.GetParametroCusto();

                                                if(cidadeArquivo.Populacao > parametroCusto.ValorCorte)
                                                {
                                                    cidadeArquivo.CustoCidadeUS = (parametroCusto.ValorCorte * parametroCusto.PorPessoa) 
                                                    + ((cidadeArquivo.Populacao - parametroCusto.ValorCorte) 
                                                    * (cidadeArquivo.CustoCidadeUS 
                                                    -(cidadeArquivo.CustoCidadeUS * (parametroCusto.Desconto * 100))));
                                                }
                                                else  
                                                {
                                                    cidadeArquivo.CustoCidadeUS = (cidadeArquivo.Populacao * parametroCusto.PorPessoa);
                                                }
                                        
                                                _repo.Add(cidadeArquivo);

                                                if(await _repo.SaveChangesAsync())
                                                {
                                                    return Ok(cidadeArquivo);
                                                }  
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou " + e.Message );
                                }     
                            }
                        }
                    }
                    return Content(cidadeArquivo.Nome);
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> post(Cidade model)
        {
            try
            {
                var cidades = await _repo.GetAllCidadesAsync(true);
                foreach (var item in cidades)
                {
                    if(model.Nome.ToUpper() == item.Nome.ToUpper() && model.EstadoId == item.EstadoId)
                    {
                        return BadRequest("Cidade já cadastrada nesse estado");
                    }
                }
                var parametroCusto = await _repo.GetParametroCusto();

                 if(model.Populacao > parametroCusto.ValorCorte)
                {
                    model.CustoCidadeUS = (parametroCusto.ValorCorte * parametroCusto.PorPessoa) 
                                            + ((model.Populacao - parametroCusto.ValorCorte) 
                                            * (model.CustoCidadeUS 
                                            -(model.CustoCidadeUS * (parametroCusto.Desconto * 100))));
                }
                else  
                {
                    model.CustoCidadeUS = (model.Populacao * parametroCusto.PorPessoa);
                }
                
                // cidade.Estado.Populacao = cidade.Estado.Populacao + cidade.Populacao;
                // cidade.Estado.CustoEstadoUS = cidade.Estado.CustoEstadoUS + cidade.CustoCidadeUS;
            
                _repo.Add(model);

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
                                            * (model.CustoCidadeUS 
                                            -(model.CustoCidadeUS * (parametroCusto.Desconto * 100))));
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
                var cidade = await _repo.GetCidadesAsyncById(cidadeId, true);
                var estado = await _repo.GetEstadosAsyncById(cidade.EstadoId, true);
                if (cidade == null)
                    return NotFound("Cidade não encontrada");

                if (estado.Nome == "Rio Grande do Sul")
                {
                    return BadRequest("Não é possivel remover essa cidade");
                }
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