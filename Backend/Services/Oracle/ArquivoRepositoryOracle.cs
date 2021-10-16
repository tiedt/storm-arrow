using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SIMP.Constants;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{
    
    public class ArquivoRepositoryOracle : TableBaseRepositoryOracle, IArquivoRepository{

        public ArquivoRepositoryOracle(IConfiguration configuration) : base(configuration) { }
        
        public async Task<int> Insert(IFormFile File){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            int Id = await GetNextValSequence(TBL_ARQUIVO.NR_ID.SEQUENCE);
            Stream StreamFile = File.OpenReadStream();
            byte[] Blob = new BinaryReader(StreamFile).ReadBytes((Int32) StreamFile.Length);
            string Sql = $@"INSERT INTO {TBL_ARQUIVO.NAME} 
                                        ({TBL_ARQUIVO.NR_ID},
                                        {TBL_ARQUIVO.DS_NOME},
                                        {TBL_ARQUIVO.BL_ARQUIVO}) 
                                VALUES ({Id},
                                        '{File.FileName}',
                                        :{TBL_ARQUIVO.BL_ARQUIVO})";
            if(await Connection.ExecuteAsync(Sql, new { Bl_arquivo = Blob }) > 0)
                return Id;
            else   
                return 0;
        }

        public async Task<bool> Delete(int Id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            
            return await Connection.ExecuteAsync(
                $@"DELETE FROM {TBL_ARQUIVO.NAME}
                        WHERE {TBL_ARQUIVO.NR_ID} = {Id}") > 0;
        }

        public async Task<Arquivo> GetById(int Id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.QueryFirstOrDefaultAsync<Arquivo>(
                $@"SELECT * FROM {TBL_ARQUIVO.NAME}
                    WHERE {TBL_ARQUIVO.NR_ID} = {Id}");
            //Stream stream      = reader.GetStream(reader.GetOrdinal(TBL_ARQUIVO.BL_ARQUIVO));
            //BinaryReader binaryReader = new BinaryReader(stream);
            //model.bl_arquivo = binaryReader.ReadBytes((Int32)stream.Length);
            //model.bl_arquivo64 = Convert.ToBase64String(model.bl_arquivo);
            //return models;
        }

        public async Task<bool> Update(int Id, IFormFile File){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            Stream StreamFile = File.OpenReadStream();
            byte[] Blob = new BinaryReader(StreamFile).ReadBytes((Int32) StreamFile.Length);
            string Sql = $@"UPDATE {TBL_ARQUIVO.NAME} 
                                SET {TBL_ARQUIVO.DS_NOME} = '{File.FileName}',
                                    {TBL_ARQUIVO.BL_ARQUIVO} = :{TBL_ARQUIVO.BL_ARQUIVO}
                                WHERE {TBL_ARQUIVO.NR_ID} = {Id}";
            return await Connection.ExecuteAsync(Sql, new { Bl_arquivo = Blob }) > 0;
        }
    }
}
