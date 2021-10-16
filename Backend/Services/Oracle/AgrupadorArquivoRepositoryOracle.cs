using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SIMP.Constants;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{

    public class AgrupadorArquivoRepositoryOracle : TableBaseRepositoryOracle, IAgrupadorArquivoRepository{
    
        private readonly IArquivoRepository arquivoRepository;

        public AgrupadorArquivoRepositoryOracle(IArquivoRepository arquivoRepository, IConfiguration configuration) : base(configuration){
            this.arquivoRepository = arquivoRepository;
        }

        private async Task<int> GetProximoAgrupador(int AgrupadorAtual){
            if(AgrupadorAtual > 0)
                return AgrupadorAtual;
            else{
                if(Connection.State != ConnectionState.Open)
                    Connection.Open();
                return await Connection.QueryFirstOrDefaultAsync<int>(
                    $@"SELECT NVL((SELECT MAX({TBL_AGRUPADOR_ARQUIVO.NR_AGRUPADOR}) FROM {TBL_AGRUPADOR_ARQUIVO.NAME}),0) + 1 FROM DUAL");
            }
        }

        public async Task<int> Insert(int Agrupador_arquivo, List<IFormFile> Files){
            Agrupador_arquivo = await GetProximoAgrupador(Agrupador_arquivo);
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            if(Files != null)
                foreach(IFormFile File in Files){
                    await Connection.ExecuteAsync(
                        $@"INSERT INTO {TBL_AGRUPADOR_ARQUIVO.NAME}
                                ({TBL_AGRUPADOR_ARQUIVO.NR_ID},
                                {TBL_AGRUPADOR_ARQUIVO.NR_AGRUPADOR},
                                {TBL_AGRUPADOR_ARQUIVO.NR_ID_ARQUIVO})
                            VALUES ({await GetNextValSequence(TBL_AGRUPADOR_ARQUIVO.NR_ID.SEQUENCE)},
                                    {Agrupador_arquivo},
                                    {await arquivoRepository.Insert(File)})");
                }
            return Agrupador_arquivo;
        }

        public async Task<bool> Update(int Id_arquivo, IFormFile File){
            return await arquivoRepository.Update(Id_arquivo, File);
        }

        public async Task<bool> DeleteByFile(int Id_arquivo){
            if(Id_arquivo <= 0)
                throw new Exception("Campos obrigatórios não foram informados");

            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.ExecuteAsync(
                $@"DELETE FROM {TBL_AGRUPADOR_ARQUIVO.NAME}
                        WHERE {TBL_AGRUPADOR_ARQUIVO.NR_ID_ARQUIVO} = {Id_arquivo}") > 0
            && await arquivoRepository.Delete(Id_arquivo);
        }

        public async Task<bool> DeleteByAgrupador(int Agrupador_arquivo){
            if(Agrupador_arquivo <= 0)
                throw new Exception("Campos obrigatórios não foram informados");
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            IEnumerable<AgrupadorArquivo> Models = await ListAllByAgrupador(Agrupador_arquivo);
            await Connection.ExecuteAsync(
                $@"DELETE FROM {TBL_AGRUPADOR_ARQUIVO.NAME}
                        WHERE {TBL_AGRUPADOR_ARQUIVO.NR_AGRUPADOR} = {Agrupador_arquivo}");
            foreach(AgrupadorArquivo Model in Models)
                await arquivoRepository.Delete(Model.Nr_id_arquivo);
            return true;
        }

        public async Task<IEnumerable<AgrupadorArquivo>> ListAllByAgrupador(int Agrupador_arquivo){
            if(Agrupador_arquivo <= 0)
                return null;
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            IEnumerable<AgrupadorArquivo> Models = await Connection.QueryAsync<AgrupadorArquivo>(
                $@"SELECT * FROM {TBL_AGRUPADOR_ARQUIVO.NAME}
                    WHERE {TBL_AGRUPADOR_ARQUIVO.NR_AGRUPADOR} = {Agrupador_arquivo}");
            foreach(AgrupadorArquivo Model in Models)
                Model.Arquivo = await arquivoRepository.GetById(Model.Nr_id_arquivo);
            return Models;
        }

        public async Task<List<Microsoft.AspNetCore.Http.IFormFile>> ListAllFilesByAgrupador(int Agrupador_arquivo){
            if(Agrupador_arquivo <= 0)
                return null;
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            List<Microsoft.AspNetCore.Http.IFormFile> result = new List<Microsoft.AspNetCore.Http.IFormFile>();
            IEnumerable<AgrupadorArquivo> Models = await Connection.QueryAsync<AgrupadorArquivo>(
                $@"SELECT * FROM {TBL_AGRUPADOR_ARQUIVO.NAME}
                    WHERE {TBL_AGRUPADOR_ARQUIVO.NR_AGRUPADOR} = {Agrupador_arquivo}");
            foreach(AgrupadorArquivo Model in Models){
                Arquivo arquivo = await arquivoRepository.GetById(Model.Nr_id_arquivo);
                result.Add(new FormFile(new MemoryStream(arquivo.Bl_arquivo), 0, arquivo.Bl_arquivo.Length, arquivo.Ds_nome, "FileOutput"));
            }
            return result;
        }

        public async Task<AgrupadorArquivo> GetByIdArquivo(int Id_arquivo){
            if(Id_arquivo <= 0)
                return null;
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            AgrupadorArquivo Model = await Connection.QueryFirstOrDefaultAsync<AgrupadorArquivo>(
                $@"SELECT * FROM {TBL_AGRUPADOR_ARQUIVO.NAME}
                    WHERE {TBL_AGRUPADOR_ARQUIVO.NR_ID_ARQUIVO} = {Id_arquivo}");
            Model.Arquivo = await arquivoRepository.GetById(Id_arquivo);
            return Model;
        }

    }

}
