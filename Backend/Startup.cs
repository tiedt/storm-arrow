using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simp.Models;
using SIMP.Repositories;
using SIMP.Services.Oracle;

namespace SIMP
{
    public class Startup{

        public readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration){
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services){

            services.AddSingleton<ISimpConnection                 , SimpConnection>();
            services.AddSingleton<IUsuarioRepository              , UsuarioRepositoryOracle>();
            services.AddSingleton<IUniversitarioRepository        , UniversitarioRepositoryOracle>();
            services.AddSingleton<IInstituicaoRepository          , InstituicaoRepositoryOracle>();
            services.AddSingleton<ICursoRepository                , CursoRepositoryOracle>();
            services.AddSingleton<IPropostaRepository             , PropostaRepositoryOracle>();
            services.AddSingleton<ILogAcessoRepository            , LogAcessoRepositoryOracle>();
            services.AddSingleton<IPropostaUniversitarioRepository, PropostaUniversitarioRepositoryOracle>();
            services.AddSingleton<IPaisRepository                 , PaisRepositoryOracle>();
            services.AddSingleton<IEstadoRepository               , EstadoRepositoryOracle>();
            services.AddSingleton<ICidadeRepository               , CidadeRepositoryOracle>();
            services.AddSingleton<IArquivoRepository              , ArquivoRepositoryOracle>();
            services.AddSingleton<IAcessosPropostaRepository      , AcessosPropostaRepositoryOracle>();
            services.AddSingleton<IAgrupadorArquivoRepository     , AgrupadorArquivoRepositoryOracle>();
            services.AddSingleton<IInteresseRepository            , InteresseRepositoryOracle>();
            services.AddSingleton<IServidorEmailRepository        , ServidorEmailRepositoryOracle>();
            services.AddSingleton<IEmailSender                    , EmailSenderOracle>();
            services.AddSingleton<ITokenRepository                , TokenRepositoryOracle>();

            // Miguel
            services.AddSingleton<IThingRepository                , ThingRepositoryOracle>();
            services.AddSingleton<IDragRaceRepository             , DragRaceRepositoryOracle>();
            services.AddSingleton<IProdutoRepository              , ProdutoRepositoryOracle>();
            services.AddSingleton<IEstoqueRepository              , EstoqueRepositoryOracle>();
            services.AddSingleton<IEletivaRepository              , EletivaRepositoryOracle>();

            // Luciana
            services.AddSingleton<IPerfilRepository               , PerfilRepositoryOracle>();
            services.AddSingleton<IPontuacaoMusicaRepository      , PontuacaoMusicaRepositoryOracle>();
            services.AddSingleton<IPerfilConfiguracoesRepository  , PerfilConfiguracoesRepositoryOracle>();

            services.AddControllers();
            //services.AddControllers().AddNewtonsoftJson(options =>
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //);
            
            services.AddCors(options =>{
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env){
            
            if (env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>{
                endpoints.MapControllers();
            });
        }
    }
}
