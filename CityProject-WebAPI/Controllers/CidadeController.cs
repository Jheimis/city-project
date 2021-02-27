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

namespace CityProject_WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CidadeController : ControllerBase
    {
        public IRepository _repo { get; }

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
                var cidades = await _repo.GetAllCidadesAsync(true);
                 foreach (var item in cidades)
                {
                    if(model.Nome.ToUpper() == item.Nome.ToUpper() && model.EstadoId == item.EstadoId)
                    {
                        return BadRequest("Cidade já cadastrada nesse estado");
                    }
                }
                
                var cidade = await _repo.GetCidadesAsyncById(cidadeId, true);
                if (cidade == null)
                {
                    return BadRequest("Cidade não encontrada");
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
                    return Ok("Deletado");
                }                
            }
           catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou " + e.Message );
            }
            return BadRequest();
        }

    //     [HttpPost]
    //      public async Task<IActionResult> UploadExcel()
    //     {
    //         try
    //         { 
    //             string message = "";
    //             HttpResponseMessage ResponseMessage = null;  
    //             var httpRequest = HttpContext.Current.Request._repo; 
    //             DataSet dsexcelRecords = new DataSet();  
    //             IExcelDataReader reader = null;  
    //             HttpPostedFile Inputfile = null;  
    //             Stream FileStream = null;

    //             if  (httpRequest.Files.Count> 0)  
    //             {  
    //                 Inputfile = httpRequest.Files [0];  
    //                 FileStream = Inputfile.InputStream;  

    //                 if  (Inputfile !=  null  && FileStream !=  null )  
    //                 {  
    //                     if    (Inputfile.FileName.EndsWith ( ".xls" ))  
    //                         reader = ExcelReaderFactory.CreateBinaryReader (FileStream);  
    //                     else if  (InputfilFe.FileName.EndsWith ( ".xlsx" ))   
    //                         reader = ExcelReaderFactory.CreateOpenXmlReader (FileStream);  
    //                     else
    //                     {
    //                         message =  "O formato do arquivo não é compatível." ;  

    //                     }  
    //                     dsexcelRecords = reader.AsDataSet();  
    //                     reader.Close();

    //                       if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)  
    //                     { 
    //                         DataTable dtCidadesRecords = dsexcelRecords.Tables[0];  
    //                         for (int i = 0; i < dtCidadesRecords.Rows.Count; i++)  
    //                         {  
                            
    //                             var estado = await _repo.GetEstadoAsyncByNome(Convert.ToString(dtCidadesRecords.Rows[i][1]));
    //                         if(estado!= null){

    //                             var cidade = await _repo.GetCidadeAsyncByNome(Convert.ToString(dtCidadesRecords.Rows[i][0]));
                                    
    //                                 if (cidade==null){
                                        
    //                                     Cidade Objcidade = new Cidade();
    //                                     {
    //                                         Objcidade.Nome =  Convert.ToString(dtCidadesRecords.Rows[i][0]);
    //                                         Objcidade.Populacao = Convert.ToInt32(dtCidadesRecords.Rows[i][1]);
    //                                         Objcidade.CustoCidadeUS = Convert.ToDouble(dtCidadesRecords.Rows[i][2]);
    //                                         Objcidade.EstadoId = Convert.ToInt32(dtCidadesRecords.Rows[i][3]);
    //                                         _repo.Add(Objcidade);

    //                                         }
    //                                 }

    //                             }
    //                         }

    //                         if(await _repo.SaveChangesAsync())
    //                         {
    //                             return Ok();
    //                         }
    //                     }



    //                 }
    //             }
    //         }
    //                 catch (Exception)
    //                     {
    //                 return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou");
    //                     }

    //                 return BadRequest();

    //  }

    }
}