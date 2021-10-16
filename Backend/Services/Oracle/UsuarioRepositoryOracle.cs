using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SIMP.Constants;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{    

    public class UsuarioRepositoryOracle : TableBaseRepositoryOracle, IUsuarioRepository{

        private readonly IUniversitarioRepository universitarioRepository;
        private readonly IInstituicaoRepository instituicaoRepository;
        private readonly IAgrupadorArquivoRepository agrupadorArquivoRepository;

        public UsuarioRepositoryOracle(IUniversitarioRepository universitarioRepository,  IInstituicaoRepository instituicaoRepository, IAgrupadorArquivoRepository agrupadorArquivoRepository, IConfiguration configuration) : base(configuration){
            this.universitarioRepository = universitarioRepository;
            this.instituicaoRepository = instituicaoRepository;
            this.agrupadorArquivoRepository = agrupadorArquivoRepository;
        }

        private void CheckModel(Usuario Model){
            if(Model.Nr_id <= 0 || String.IsNullOrEmpty(Model.Ds_email) || String.IsNullOrEmpty(Model.Ds_senha))
                throw new Exception("Campos obrigatórios não foram informados.");
        }

        public async Task<Object> Insert(Object Obj){

            Usuario Model = new Usuario(){
                Nr_id = await GetNextValSequence(TBL_USUARIO.NR_ID.SEQUENCE)
            };

            CheckModel(Model);
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            await Connection.ExecuteAsync(
                $@"INSERT INTO {TBL_USUARIO.NAME}
                        ({TBL_USUARIO.NR_ID},
                        {TBL_USUARIO.DS_EMAIL},
                        {TBL_USUARIO.DS_SENHA},
                        {TBL_USUARIO.DS_NOME_EXIBIDO},
                        {TBL_USUARIO.NR_AGRUPADOR_ARQUIVO})
                    VALUES ({Model.Nr_id},
                            '{JSON.GetValuePropertyString(Obj, TBL_USUARIO.DS_EMAIL.ToLower())}',
                            '{Hash.EncryptStringSalt(JSON.GetValuePropertyString(Obj, TBL_USUARIO.DS_SENHA.ToLower()), Model.Ds_email)}',
                            '{JSON.GetValuePropertyString(Obj, TBL_USUARIO.DS_NOME_EXIBIDO.ToLower())}',
                            {JSON.GetValuePropertyInt32(Obj, TBL_USUARIO.NR_AGRUPADOR_ARQUIVO.ToLower())})");

            if (JSON.HasProperty(Obj, TBL_UNIVERSITARIO.DS_GRAU.ToLower())){ // É Universitário?
                Universitario Universitario = new Universitario(Model){
                    Nr_id_cidade      = JSON.GetValuePropertyInt32(Obj , TBL_UNIVERSITARIO.NR_ID_CIDADE.ToLower()),
                    Nr_id_estado      = JSON.GetValuePropertyInt32(Obj , TBL_UNIVERSITARIO.NR_ID_ESTADO.ToLower()),
                    Nr_id_instituicao = JSON.GetValuePropertyInt32(Obj , TBL_UNIVERSITARIO.NR_ID_INSTITUICAO.ToLower()),
                    Ds_nome           = JSON.GetValuePropertyString(Obj, TBL_UNIVERSITARIO.DS_NOME.ToLower()),
                    Ds_sobrenome      = JSON.GetValuePropertyString(Obj, TBL_UNIVERSITARIO.DS_SOBRENOME.ToLower()),
                    Ds_telefone       = JSON.GetValuePropertyString(Obj, TBL_UNIVERSITARIO.DS_TELEFONE.ToLower()),
                    Ds_grau           = JSON.GetValuePropertyString(Obj, TBL_UNIVERSITARIO.DS_GRAU.ToLower())
                }; // Load das propriedades do JSON para as respectivas classes                
                await universitarioRepository.Insert(Universitario);
            }else{ 
                Instituicao Instituicao = new Instituicao(Model){
                    Nr_id_cidade             = JSON.GetValuePropertyInt32(Obj , TBL_INSTITUICAO.NR_ID_CIDADE.ToLower()),
                    Nr_id_estado             = JSON.GetValuePropertyInt32(Obj , TBL_INSTITUICAO.NR_ID_ESTADO.ToLower()),
                    Ds_razao_social          = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_RAZAO_SOCIAL.ToLower()),
                    Ds_ramo                  = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_RAMO.ToLower()),
                    Cd_cnpj                  = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.CD_CNPJ.ToLower()),
                    Ds_resumo                = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_RESUMO.ToLower()),
                    Ds_descricao             = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_DESCRICAO.ToLower()),
                    Ds_email_contato         = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_EMAIL_CONTATO.ToLower()),
                    Ds_telefone              = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_TELEFONE.ToLower()),
                    Ds_horario_funcionamento = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_HORARIO_FUNCIONAMENTO.ToLower())
                };
                await instituicaoRepository.Insert(Instituicao);
            }
            return await GetById(Model.Nr_id);
        }

        public async Task<Object> GetById(int Id){

            Object Obj = await instituicaoRepository.GetByIdUser(Id);
            if(Obj != null)
                return Obj;
            else
                return await universitarioRepository.GetByIdUser(Id);
        }

        public async Task<Object> GetByEmailPassword(string Email, string Password){
            Object Obj = await instituicaoRepository.GetByEmailPasswordUser(Email, Password);
            if(Obj != null)
                return Obj;
            else
                return await universitarioRepository.GetByEmailPasswordUser(Email, Password);
        }

        public async Task<IEnumerable<Object>> ListAll(){
            
            List<Object> Models = new List<Object>();

            Models.AddRange(await instituicaoRepository.ListAll());
            Models.AddRange(await universitarioRepository.ListAll());

            return Models;
        }

        public async Task<Object> Update(Object Obj){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            Usuario Model = new Usuario(){
                Nr_id = JSON.GetValuePropertyInt32(Obj, TBL_USUARIO.NR_ID.ToString().ToLower())
            };
            if(!JSON.GetValuePropertyString(Obj, TBL_USUARIO.DS_SENHA.ToLower()).Equals("")){ // Alterou a senha do usuário?
                await Connection.ExecuteAsync(
                    $@"UPDATE {TBL_USUARIO.NAME}
                            SET {TBL_USUARIO.DS_SENHA} = '{Hash.EncryptStringSalt(JSON.GetValuePropertyString(Obj, TBL_USUARIO.DS_SENHA.ToLower()), JSON.GetValuePropertyString(Obj, TBL_USUARIO.DS_EMAIL.ToLower()))}',
                                {TBL_USUARIO.DS_NOME_EXIBIDO} = '{JSON.GetValuePropertyString(Obj, TBL_USUARIO.DS_NOME_EXIBIDO.ToLower())}'
                        WHERE {TBL_USUARIO.NR_ID} = {Model.Nr_id}");
            }else{
                await Connection.ExecuteAsync(
                    $@"UPDATE {TBL_USUARIO.NAME}
                            SET {TBL_USUARIO.DS_NOME_EXIBIDO} = '{JSON.GetValuePropertyString(Obj, TBL_USUARIO.DS_NOME_EXIBIDO.ToLower())}'
                        WHERE {TBL_USUARIO.NR_ID} = {Model.Nr_id}");
            }

            if (JSON.HasProperty(Obj, TBL_UNIVERSITARIO.DS_GRAU.ToLower())){ // É universitário?
                Universitario Universitario = new Universitario(Model){ 
                    Nr_id_cidade      = JSON.GetValuePropertyInt32(Obj , TBL_UNIVERSITARIO.NR_ID_CIDADE.ToLower()),
                    Nr_id_estado      = JSON.GetValuePropertyInt32(Obj , TBL_UNIVERSITARIO.NR_ID_ESTADO.ToLower()),
                    Nr_id_instituicao = JSON.GetValuePropertyInt32(Obj , TBL_UNIVERSITARIO.NR_ID_INSTITUICAO.ToLower()),
                    Ds_nome           = JSON.GetValuePropertyString(Obj, TBL_UNIVERSITARIO.DS_NOME.ToLower()),
                    Ds_sobrenome      = JSON.GetValuePropertyString(Obj, TBL_UNIVERSITARIO.DS_SOBRENOME.ToLower()),
                    Ds_telefone       = JSON.GetValuePropertyString(Obj, TBL_UNIVERSITARIO.DS_TELEFONE.ToLower()),
                    Ds_grau           = JSON.GetValuePropertyString(Obj, TBL_UNIVERSITARIO.DS_GRAU.ToLower())
                }; // Load das propriedades do JSON para as respectivas classes                
                await universitarioRepository.Update(Universitario);
            }else{
                Instituicao Instituicao = new Instituicao(Model){
                    Nr_id_cidade             = JSON.GetValuePropertyInt32(Obj , TBL_INSTITUICAO.NR_ID_CIDADE.ToLower()),
                    Nr_id_estado             = JSON.GetValuePropertyInt32(Obj , TBL_INSTITUICAO.NR_ID_ESTADO.ToLower()),
                    Ds_razao_social          = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_RAZAO_SOCIAL.ToLower()),
                    Ds_ramo                  = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_RAMO.ToLower()),
                    Cd_cnpj                  = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.CD_CNPJ.ToLower()),
                    Ds_resumo                = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_RESUMO.ToLower()),
                    Ds_descricao             = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_DESCRICAO.ToLower()),
                    Ds_email_contato         = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_EMAIL_CONTATO.ToLower()),
                    Ds_telefone              = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_TELEFONE.ToLower()),
                    Ds_horario_funcionamento = JSON.GetValuePropertyString(Obj, TBL_INSTITUICAO.DS_HORARIO_FUNCIONAMENTO.ToLower())
                };                
                await instituicaoRepository.Update(Instituicao);
            }
            return await GetById(Model.Nr_id);
        }

        public async Task<IEnumerable<AgrupadorArquivo>> GetImages(int Id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await agrupadorArquivoRepository.ListAllByAgrupador(await Connection.QueryFirstOrDefaultAsync<int>(
                $@"SELECT {TBL_USUARIO.NR_AGRUPADOR_ARQUIVO} FROM {TBL_USUARIO.NAME}
                    WHERE {TBL_USUARIO.NR_ID} = {Id}"));
        }

        public async Task<bool> InsertImage(int Id, int Nr_agrupador, List<IFormFile> Files){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            if(Nr_agrupador <= 0)
                return await Connection.ExecuteAsync(
                            $@"UPDATE {TBL_USUARIO.NAME} 
                                    SET {TBL_USUARIO.NR_AGRUPADOR_ARQUIVO} = {await agrupadorArquivoRepository.Insert(Nr_agrupador, Files)}
                                WHERE {TBL_USUARIO.NR_ID} = {Id}") > 0;
            else
                return await agrupadorArquivoRepository.Insert(Nr_agrupador, Files) > 0;
        }
        
        public async Task<bool> UpdateImage(int Id_arquivo, IFormFile File){
            return await agrupadorArquivoRepository.Update(Id_arquivo, File);
        }
        
        private async Task<bool> CheckLinkImages(int Id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            if (await Connection.QueryFirstOrDefaultAsync<int>(
                $@"SELECT COUNT(*) FROM {TBL_USUARIO.NAME} USU, {TBL_AGRUPADOR_ARQUIVO.NAME} AA
                    WHERE USU.{TBL_USUARIO.NR_AGRUPADOR_ARQUIVO} = AA.{TBL_AGRUPADOR_ARQUIVO.NR_AGRUPADOR}
                        AND USU.{TBL_USUARIO.NR_ID} = {Id}") < 1) // Todas as imagens do usuário foram removidas?
                return await Connection.ExecuteAsync( // Remove o link com as imagens do usuário
                    $@"UPDATE {TBL_USUARIO.NAME} 
                            SET {TBL_USUARIO.NR_AGRUPADOR_ARQUIVO} = {null}
                        WHERE {TBL_USUARIO.NR_ID} = {Id}") > 0;
            return true;
        }

        public async Task<bool> DeleteImage(int Id, int Id_arquivo){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await agrupadorArquivoRepository.DeleteByFile(Id_arquivo)
                && await CheckLinkImages(Id);
        }
    }
}
