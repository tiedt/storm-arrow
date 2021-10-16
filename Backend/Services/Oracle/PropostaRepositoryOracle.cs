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

    public class PropostaRepositoryOracle : TableBaseRepositoryOracle, IPropostaRepository{

        private readonly ICursoRepository cursoRepository;
        private readonly IInstituicaoRepository instituicaoRepository;
        private readonly ILogAcessoRepository logAcessoRepository;
        private readonly IAgrupadorArquivoRepository agrupadorArquivoRepository;
        private readonly IPropostaUniversitarioRepository propostaUniversitarioRepository;

        public PropostaRepositoryOracle(ICursoRepository cursoRepository, IInstituicaoRepository instituicaoRepository, ILogAcessoRepository logAcessoRepository, IAgrupadorArquivoRepository agrupadorArquivoRepository, IPropostaUniversitarioRepository propostaUniversitarioRepository, IConfiguration configuration) : base(configuration){
            this.cursoRepository = cursoRepository;
            this.instituicaoRepository = instituicaoRepository;
            this.logAcessoRepository = logAcessoRepository;
            this.agrupadorArquivoRepository = agrupadorArquivoRepository;
            this.propostaUniversitarioRepository = propostaUniversitarioRepository;
        }

        private void CheckModel(Proposta Model){
            if(Model.Nr_id <= 0 || Model.Nr_id_curso <= 0 || Model.Nr_id_instituicao <= 0)
                throw new Exception("Campos obrigatórios não foram informados.");
        }

        public async Task<Proposta> Insert(Proposta Model, List<IFormFile> Files){            
            Model.Nr_id = await GetNextValSequence(TBL_PROPOSTA.NR_ID.SEQUENCE);
            CheckModel(Model);
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            
            if(await Connection.ExecuteAsync(
                $@"INSERT INTO {TBL_PROPOSTA.NAME}
                        ({TBL_PROPOSTA.NR_ID},
                            {TBL_PROPOSTA.NR_ID_CURSO},
                            {TBL_PROPOSTA.NR_ID_INSTITUICAO},
                            {TBL_PROPOSTA.DS_NOME},
                            {TBL_PROPOSTA.DS_DESC_PROJETO},
                            {TBL_PROPOSTA.CD_STATUS},
                            {TBL_PROPOSTA.DS_REQUISITO},
                            {TBL_PROPOSTA.QT_PARTICIPANTES},
                            {TBL_PROPOSTA.NR_DURACAO},
                            {TBL_PROPOSTA.DS_INFO_CONTATOS},
                            {TBL_PROPOSTA.DS_TIPO},
                            {TBL_PROPOSTA.DT_GERACAO})
                    VALUES ({Model.Nr_id},
                            {Model.Nr_id_curso},
                            {Model.Nr_id_instituicao},
                            '{Model.Ds_nome}',
                            '{Model.Ds_desc_projeto}',
                            '{Model.Cd_status}',
                            '{Model.Ds_requisito}',
                            {Model.Qt_participantes},
                            {Model.Nr_duracao},
                            '{Model.Ds_info_contatos}',
                            '{Model.Ds_tipo}',
                            SYSDATE)") > 0){
                if(Files != null
                && Files.Count > 0)
                    await InsertImage(Model.Nr_id, 0, Files);
                return await GetById(Model.Nr_id, 0);
            }else
                return null;
        }

        public async Task<Proposta> GetById(int Id, int? Id_usuario){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            Proposta Model = await Connection.QueryFirstOrDefaultAsync<Proposta>(
                                    $@"SELECT * FROM {TBL_PROPOSTA.NAME}
                                        WHERE {TBL_PROPOSTA.NR_ID} = {Id}");
            if(Model != null){
                Model.Curso = await cursoRepository.GetById(Model.Nr_id_curso);
                Model.Instituicao = await instituicaoRepository.GetById(Model.Nr_id_instituicao);
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
                Model.Universitarios = await propostaUniversitarioRepository.ListAllByProposals(Model.Nr_id);                
            }
            if(Model != null && Id_usuario != null && Id_usuario > 0){
                await logAcessoRepository.Insert(new LogAcesso(Id, (int) Id_usuario)); // Registra o acesso do usuário a proposta
                Interesse interesse = await Connection.QueryFirstOrDefaultAsync<Interesse>(
                    $@"SELECT * FROM {TBL_INTERESSE.NAME} INT, {TBL_UNIVERSITARIO.NAME} UNI, {TBL_USUARIO.NAME} USU
                        WHERE INT.{TBL_INTERESSE.NR_ID_UNIVERSITARIO} = UNI.{TBL_UNIVERSITARIO.NR_ID}
                          AND UNI.{TBL_UNIVERSITARIO.NR_ID_USUARIO} = USU.{TBL_USUARIO.NR_ID}
                          AND INT.{TBL_INTERESSE.NR_ID_PROPOSTA} = {Id}
                          AND USU.{TBL_USUARIO.NR_ID} = {Id_usuario}");
                Model.Usuario_interessado = interesse != null && interesse.Ds_status.Equals("ATIVO");
            }
            return Model;
        }

        public async Task<IEnumerable<Proposta>> ListAll(){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            IEnumerable<Proposta> Models = await Connection.QueryAsync<Proposta>(
                $@"SELECT * FROM {TBL_PROPOSTA.NAME}");
            foreach(Proposta Model in Models){
                Model.Curso = await cursoRepository.GetById(Model.Nr_id_curso);
                Model.Instituicao = await instituicaoRepository.GetById(Model.Nr_id_instituicao);
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
                Model.Universitarios = await propostaUniversitarioRepository.ListAllByProposals(Model.Nr_id);
            }
            return Models;
        }

        public async Task<Proposta> Update(Proposta Model, List<IFormFile> Files){
            CheckModel(Model);
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            await propostaUniversitarioRepository.Delete(new PropostaUniversitario(0, 0, Model.Nr_id));
            if(Model.Universitarios != null)
                foreach(Universitario Universitario in Model.Universitarios)
                    await propostaUniversitarioRepository.Insert(new PropostaUniversitario(0, Universitario.Nr_id, Model.Nr_id));
            // Remove as que não precisam ser mantidas
            Proposta ModelOriginal = await this.GetById(Model.Nr_id, 0);
            if(ModelOriginal.AgrupadorArquivo != null)
                foreach(AgrupadorArquivo agrupadorArquivo in ModelOriginal.AgrupadorArquivo)
                    if(Model.AgrupadorArquivo != null
                    && Model.AgrupadorArquivo.AsList<AgrupadorArquivo>().Count > 0){
                        bool AchouNaLista = false;
                        foreach(AgrupadorArquivo Aux in Model.AgrupadorArquivo){
                            AchouNaLista = Aux.Nr_id_arquivo == agrupadorArquivo.Nr_id_arquivo;
                            if(AchouNaLista)
                                break;
                        }
                        if(!AchouNaLista)
                            await this.DeleteImage(Model.Nr_id, agrupadorArquivo.Nr_id_arquivo);
                    }else
                        await this.DeleteImage(Model.Nr_id, agrupadorArquivo.Nr_id_arquivo);
            // Insere as novas imagens da proposta
            if(Files != null
            && Files.Count > 0)
                await InsertImage(Model.Nr_id, Model.Nr_agrupador_arquivo, Files);
            if(await Connection.ExecuteAsync(
                $@"UPDATE {TBL_PROPOSTA.NAME}
                        SET {TBL_PROPOSTA.NR_ID_CURSO} = {Model.Nr_id_curso},
                            {TBL_PROPOSTA.NR_ID_INSTITUICAO} = {Model.Nr_id_instituicao},
                            {TBL_PROPOSTA.DS_NOME} = '{Model.Ds_nome}',
                            {TBL_PROPOSTA.DS_DESC_PROJETO} = '{Model.Ds_desc_projeto}',
                            {TBL_PROPOSTA.CD_STATUS} = '{Model.Cd_status}',
                            {TBL_PROPOSTA.DS_REQUISITO} = '{Model.Ds_requisito}',
                            {TBL_PROPOSTA.QT_PARTICIPANTES} = {Model.Qt_participantes},
                            {TBL_PROPOSTA.NR_DURACAO} = {Model.Nr_duracao},
                            {TBL_PROPOSTA.DS_INFO_CONTATOS} = '{Model.Ds_info_contatos}',
                            {TBL_PROPOSTA.DS_TIPO} = '{Model.Ds_tipo}'
                    WHERE {TBL_PROPOSTA.NR_ID} = {Model.Nr_id}") > 0)
                return await GetById(Model.Nr_id, 0);
            else
                return null;
        }

        public async Task<IEnumerable<Proposta>> GetMostAccessed(){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            IEnumerable<Proposta> Models = await Connection.QueryAsync<Proposta>(
                $@"SELECT * FROM VW_TEMAS_POPULARES");
            foreach(Proposta Model in Models){
                Model.Curso = await cursoRepository.GetById(Model.Nr_id_curso);
                Model.Instituicao = await instituicaoRepository.GetById(Model.Nr_id_instituicao);
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
                Model.Universitarios = await propostaUniversitarioRepository.ListAllByProposals(Model.Nr_id);
            }
            return Models;
        }

        public async Task<IEnumerable<Proposta>> GetRecommendations(int Id_usuario){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            IEnumerable<Proposta> Models = await Connection.QueryAsync<Proposta>(
                $@"SELECT PROP.* FROM {TBL_PROPOSTA.NAME} PROP, 
                            (SELECT PRO.{TBL_PROPOSTA.NR_ID}, PRO.{TBL_PROPOSTA.NR_ID_CURSO}, COUNT(*) {TBL_PROPOSTA.DS_NOME}
                            FROM  {TBL_PROPOSTA.NAME} PRO, {TBL_LOG_ACESSO.NAME} LA
                            WHERE PRO.{TBL_PROPOSTA.NR_ID} = LA.{TBL_LOG_ACESSO.NR_ID_PROPOSTA}
                            GROUP BY PRO.{TBL_PROPOSTA.NR_ID}, PRO.{TBL_PROPOSTA.NR_ID_CURSO}) MAIS_ACE,
                            (SELECT PR.{TBL_PROPOSTA.NR_ID_CURSO}, COUNT(*) {TBL_PROPOSTA.DS_NOME}
                            FROM {TBL_LOG_ACESSO.NAME} LA, {TBL_PROPOSTA.NAME} PR, {TBL_USUARIO.NAME} US
                            WHERE LA.{TBL_LOG_ACESSO.NR_ID_PROPOSTA} = PR.{TBL_PROPOSTA.NR_ID} AND LA.{TBL_LOG_ACESSO.NR_ID_USUARIO} = US.{TBL_USUARIO.NR_ID} 
                                AND LA.{TBL_LOG_ACESSO.NR_ID_USUARIO} = {Id_usuario}
                            GROUP BY PR.{TBL_PROPOSTA.NR_ID_CURSO}) MAIS_ACE_USU 
                    WHERE PROP.{TBL_PROPOSTA.NR_ID} = MAIS_ACE.{TBL_PROPOSTA.NR_ID} AND MAIS_ACE.{TBL_PROPOSTA.NR_ID_CURSO} = MAIS_ACE_USU.{TBL_PROPOSTA.NR_ID_CURSO} 
                        AND ROWNUM <= 3
                    ORDER BY MAIS_ACE_USU.{TBL_PROPOSTA.DS_NOME} DESC, MAIS_ACE.{TBL_PROPOSTA.DS_NOME} DESC");
            foreach(Proposta Model in Models){
                Model.Curso = await cursoRepository.GetById(Model.Nr_id_curso);
                Model.Instituicao = await instituicaoRepository.GetById(Model.Nr_id_instituicao);
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
                Model.Universitarios = await propostaUniversitarioRepository.ListAllByProposals(Model.Nr_id);
            }
            return Models;
        }

        public async Task<IEnumerable<Proposta>> GetNewProposals(){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            IEnumerable<Proposta> Models = await Connection.QueryAsync<Proposta>(
                $@"SELECT * FROM {TBL_PROPOSTA.NAME} PO
                    WHERE ROWNUM <= 10
                  ORDER BY PO.{TBL_PROPOSTA.DT_GERACAO} DESC");
            foreach(Proposta Model in Models){
                Model.Curso = await cursoRepository.GetById(Model.Nr_id_curso);
                Model.Instituicao = await instituicaoRepository.GetById(Model.Nr_id_instituicao);
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
                Model.Universitarios = await propostaUniversitarioRepository.ListAllByProposals(Model.Nr_id);
            }
            return Models;
        }

        public async Task<IEnumerable<Proposta>> GetByCategory(int Id_categoria){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            IEnumerable<Proposta> Models = await Connection.QueryAsync<Proposta>(
                $@"SELECT PO.*
                    FROM {TBL_PROPOSTA.NAME} PO, {TBL_CURSO.NAME} CR
                    WHERE PO.{TBL_PROPOSTA.NR_ID_CURSO} = CR.{TBL_CURSO.NR_ID}
                      AND PO.{TBL_PROPOSTA.NR_ID_CURSO} = {Id_categoria}");
            foreach(Proposta Model in Models){
                Model.Curso = await cursoRepository.GetById(Model.Nr_id_curso);
                Model.Instituicao = await instituicaoRepository.GetById(Model.Nr_id_instituicao);
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
                Model.Universitarios = await propostaUniversitarioRepository.ListAllByProposals(Model.Nr_id);
            }
            return Models;
        }

        public async Task<IEnumerable<Proposta>> GetByInstitution(int Id_instituicao){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            IEnumerable<Proposta> Models = await Connection.QueryAsync<Proposta>(
                $@"SELECT PO.*
                    FROM {TBL_PROPOSTA.NAME} PO, {TBL_INSTITUICAO.NAME} IT
                    WHERE PO.{TBL_PROPOSTA.NR_ID_INSTITUICAO} = IT.{TBL_INSTITUICAO.NR_ID}
                      AND PO.{TBL_PROPOSTA.NR_ID_INSTITUICAO} = {Id_instituicao}");
            foreach(Proposta Model in Models){
                Model.Curso = await cursoRepository.GetById(Model.Nr_id_curso);
                Model.Instituicao = await instituicaoRepository.GetById(Model.Nr_id_instituicao);
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
                Model.Universitarios = await propostaUniversitarioRepository.ListAllByProposals(Model.Nr_id);
            }
            return Models;
        }

        public async Task<IEnumerable<Proposta>> GetByParams(DateTime? Dt_geracaoIni, DateTime? Dt_geracaoFim, string? Ds_tipo,
                                                             int? Nr_id_curso, int? Qt_participantes, string? Ds_nome_ds_desc_projeto){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            string sql = $@"SELECT PO.* FROM {TBL_PROPOSTA.NAME} PO ";

            if(Dt_geracaoIni != null && Dt_geracaoFim != null){
                if(sql.Contains("WHERE"))
                    sql += "AND ";
                else 
                    sql += "WHERE ";
                sql += $@"{TBL_PROPOSTA.DT_GERACAO} BETWEEN '{Dt_geracaoIni}' AND '{Dt_geracaoFim}' ";
            }
            
            if (!string.IsNullOrEmpty(Ds_tipo)){
                if(sql.Contains("WHERE"))
                    sql += "AND ";
                else 
                    sql += "WHERE ";
                sql += $@"UPPER({TBL_PROPOSTA.DS_TIPO}) LIKE '%{Ds_tipo.ToUpper()}%' ";
            }
            
            if (Nr_id_curso > 0){
                if(sql.Contains("WHERE"))
                    sql += "AND ";
                else 
                    sql += "WHERE ";
                sql += $@"{TBL_PROPOSTA.NR_ID_CURSO} = {Nr_id_curso} ";
            }
            
            if (Qt_participantes > 0){
                if(sql.Contains("WHERE"))
                    sql += "AND ";
                else 
                    sql += "WHERE ";
                sql += $@"{TBL_PROPOSTA.QT_PARTICIPANTES} = {Qt_participantes} ";
            }
            
            if (!String.IsNullOrEmpty(Ds_nome_ds_desc_projeto)){
                if(sql.Contains("WHERE"))
                    sql += "AND ";
                else 
                    sql += "WHERE ";
                sql += $@"(UPPER({TBL_PROPOSTA.DS_NOME}) LIKE '%{Ds_nome_ds_desc_projeto.ToUpper()}%'
                          OR UPPER({TBL_PROPOSTA.DS_DESC_PROJETO}) LIKE '%{Ds_nome_ds_desc_projeto.ToUpper()}%') ";
            }

            if(!sql.Contains("WHERE")) // Se não foi informado filtro
                return null;           // Então não deixa executar a query
            IEnumerable<Proposta> Models = await Connection.QueryAsync<Proposta>(sql);
            foreach(Proposta Model in Models){
                Model.Curso = await cursoRepository.GetById(Model.Nr_id_curso);
                Model.Instituicao = await instituicaoRepository.GetById(Model.Nr_id_instituicao);
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
                Model.Universitarios = await propostaUniversitarioRepository.ListAllByProposals(Model.Nr_id);
            }
            return Models;
        }

        public async Task<IEnumerable<AgrupadorArquivo>> GetImages(int Id){ // id da proposta
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await agrupadorArquivoRepository.ListAllByAgrupador(await Connection.QueryFirstOrDefaultAsync<int>(
                                            $@"SELECT {TBL_PROPOSTA.NR_AGRUPADOR_ARQUIVO} FROM {TBL_PROPOSTA.NAME}
                                                WHERE {TBL_PROPOSTA.NR_ID} = {Id}"));
        }
        
        public async Task<bool> InsertImage(int Id, int Nr_agrupador, List<IFormFile> Files){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            if(Nr_agrupador <= 0)
                return await Connection.ExecuteAsync(
                    $@"UPDATE {TBL_PROPOSTA.NAME} 
                            SET {TBL_PROPOSTA.NR_AGRUPADOR_ARQUIVO} = {await agrupadorArquivoRepository.Insert(Nr_agrupador, Files)}
                        WHERE {TBL_PROPOSTA.NR_ID} = {Id}") > 0;
            else
                return await agrupadorArquivoRepository.Insert(Nr_agrupador, Files) > 0;
        }
        
        public async Task<bool> UpdateImage(int Id_arquivo, IFormFile File){
            return await agrupadorArquivoRepository.Update(Id_arquivo, File);
        }

        private async Task<bool> CheckLinkImages(int Id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            if(await Connection.QueryFirstOrDefaultAsync<int>( // Se não houver mais imagens ligadas a um agrupador 
                $@"SELECT COUNT(*) FROM {TBL_PROPOSTA.NAME} PRO, {TBL_AGRUPADOR_ARQUIVO.NAME} AA
                        WHERE PRO.{TBL_PROPOSTA.NR_AGRUPADOR_ARQUIVO} = AA.{TBL_AGRUPADOR_ARQUIVO.NR_AGRUPADOR}
                            AND PRO.{TBL_PROPOSTA.NR_ID} = {Id}") < 1)
                await Connection.ExecuteAsync( // remove o link na tabela proposta
                    $@"UPDATE {TBL_PROPOSTA.NAME} 
                            SET {TBL_PROPOSTA.NR_AGRUPADOR_ARQUIVO} = NULL
                        WHERE {TBL_PROPOSTA.NR_ID} = {Id}");
            return true;
        }
        
        public async Task<bool> DeleteImage(int Id, int Id_arquivo){
            return await agrupadorArquivoRepository.DeleteByFile(Id_arquivo)
                && await CheckLinkImages(Id);
        }

    }

}
