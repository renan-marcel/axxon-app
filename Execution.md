**CONTEXTO:**
Você é um agente de desenvolvimento de software especializado em .NET e Clean Architecture. Sua tarefa é implementar o MVP (Produto Mínimo Viável) do sistema GED/Prontuários, seguindo rigorosamente as diretrizes arquitetônicas definidas no `GEMINI.md` e no arquivo `PROJECT_PLAN`.

**OBJETIVO DO MVP:**
Implementar a funcionalidade essencial para:
1.  **Inicie o Repositorio** Inicie o repositorio Git.
2.  **Gerenciar Tipos de Documento Simples:** Criar e listar Tipos de Documento.
3.  **Upload de Documento:** Realizar o upload de um arquivo, armazená-lo localmente, calcular seu hash e persistir os metadados.
4  **Visualizar/Download de Documento:** Recuperar e baixar um documento pelo ID.
5.  **Gerenciar Prontuários Básicos:** Criar um prontuário e associar documentos a ele, além de listar documentos de um prontuário.
6.  **Autenticação Básica:** Proteção dos endpoints API com autenticação JWT (considerando integração futura com Keycloak, para o MVP pode ser in-memory simples).
7.  **Tratamento de Erros:** Utilizar o Padrão Result e Problem Details para erros esperados.
8.  **Observabilidade Básica:** Logging e telemetria via Serilog/OpenTelemetry.

**RECURSOS ARQUITETÔNICOS:**
Consulte o `GEMINI.md` para todos os detalhes sobre camadas e padrões (Clean Architecture, CQRS, Event Sourcing, Result) e estrutura de projetos.

Importante: o framework Aspire.NET é um requisito técnico obrigatório para este projeto. Aspire será a plataforma de execução/orquestração no ambiente de desenvolvimento e produção, provendo integração com infraestrutura observability/telemetria, orquestração de serviços, e integração com Postgres/RabbitMQ/Marten/MassTransit quando aplicável.

Observação crítica: o uso do framework FluentValidation é um requisito de negócio obrigatório para validação de domínios, comandos e DTOs. Todas as regras de validação de entrada (Commands/DTOs) e validações de invariantes de domínio (Value Objects e Entidades) devem ser expressas e implementadas via FluentValidation (ou um adapter que delegue a FluentValidation). Essa regra visa padronizar mensagens de erro, facilitar composição de regras complexas, e garantir integração consistente com pipelines de MediatR e com o comportamento HTTP do ASP.NET.

Importante o uso de .net centralized package management (centralizado) para gerenciar as versões dos pacotes NuGet utilizados no projeto. Isso garante consistência e facilita atualizações futuras.

**INSTRUÇÕES DE VERSIONAMENTO**:
Para garantir que o projeto siga as melhores práticas de versionamento, utilize o seguinte padrão para commits e tags:
- **Commits**: Use mensagens claras e descritivas, seguindo o padrão "Tipo: Descrição" (ex.: "feat: Implementar upload de documento").
- **Tags**: Use tags semânticas para marcar versões (ex.: `v0.1.0`, `v0.2.0`).
- **Branches**: Utilize branches para desenvolvimento de novas funcionalidades ou correções, seguindo o padrão `feature/nome-da-funcionalidade` ou `bugfix/nome-do-bug`.
No Inicio de cada fase, crie uma branch específica para a fase (ex.: `fase-1-configuracao-fundacao`), e ao final da fase, faça o merge com a branch principal (`main` ou `develop`).

**INSTRUÇÕES PASSO A PASSO (Fases do Projeto Adaptadas para MVP):**

---

**FASE 1: Configuração e Fundação (Prioridade Máxima)**

1.  **Revisão Arquitetônica:**
        *   **Ação realizada:** `GEMINI.md` (design/arquitetura) foi revisado e os princípios adotados para o MVP.
        *   **Entrega:** Entendimento confirmado — optar por simplificar: EF Core (SQLite), armazenamento local, MediatR in-process.
2.  **Setup do Repositório e Ambiente (executado):**
        *   **Ação realizada:** Repositório inicializado, `.gitignore` e `README.md` criados.
        *   **Ação realizada:** Solução `.sln` e projetos criados em `src/` e `tests/` conforme checklist (veja lista abaixo).
        *   **Entrega:** Repositório com scaffold .NET pronto (veja como verificar na seção "Como verificar").
3.  **Estrutura de Projetos (Solução .NET) (executado):**
        *   **Ação realizada:** Criação dos projetos:
                *   `src/ProdAbs.SharedKernel`
                *   `src/ProdAbs.Domain`
                *   `src/ProdAbs.Application`
                *   `src/ProdAbs.Infrastructure`
                *   `src/ProdAbs.Presentation.Api`
                *   `src/ProdAbs.CrossCutting`
                *   `tests/ProdAbs.UnitTests`
                *   `tests/ProdAbs.IntegrationTests`
        *   **Ação realizada:** Pacotes essenciais adicionados (MediatR, EF Core SQLite, Swashbuckle, Serilog, JWT Bearer) e referências de projeto configuradas.
        *   **Entrega:** Estrutura de projetos estabelecida e `dotnet build` executado com sucesso (veja saída no terminal).
4.  **Configuração do Aspire.NET (obrigatório):**
        *   **Descrição:** Aspire.NET deve ser configurado como plataforma de execução/orquestração desde a Fase 1. Para o MVP isso significa prover um ambiente local (ou containerizado) com os serviços essenciais que o sistema espera: PostgreSQL (banco principal), RabbitMQ (mensageria), e suporte a Marten/MassTransit quando usarmos Event Sourcing e mensageria. Aspire também será usado para padronizar as configurações de telemetria/OpenTelemetry e health checks.
        *   **Ações práticas:**
            - Provisionar um ambiente local de desenvolvimento baseado no stack do Aspire (p.ex. um docker-compose/stack fornecido pelo time Aspire ou templates oficiais). Esse stack deve expor as variáveis de conexão necessárias ao `appsettings.Development.json` (ConnectionStrings:Postgres, RabbitMQ:Host/User/Password, AspNetCore/Ports).
            - Garantir que a aplicação integre com Aspire via as extensões/bootstrapping definidas pelo projeto (ex.: registrar MassTransit com RabbitMQ, configurar Marten para Postgres, configurar OpenTelemetry/Serilog com endpoints/collectors fornecidos pelo Aspire). Se o projeto já possuir uma camada CrossCutting/DependencyInjection, adicionar um método para aplicar as configurações específicas do Aspire (ex.: AddAspireConfiguration or RegisterAspireServices) seguindo as convenções internas do repositório.
            - Documentar no README ou em `docs/` o passo-a-passo para subir o stack Aspire em dev: onde obter os manifests, quais variáveis de ambiente setar, e como validar que os serviços (Postgres/RabbitMQ) ficaram disponíveis.
        *   **Critérios de aceitação (mínimos):**
            - Um comando único (ou script) que sobe o ambiente Aspire local com PostgreSQL e RabbitMQ e deixa a API apta a conectar-se a esses serviços.
            - Variáveis de ambiente ou `appsettings.Development.json` documentadas e compatíveis com o stack Aspire.
            - Health checks da API reportando status "healthy" quando conectado ao Postgres/RabbitMQ via Aspire.
            - Observability básica (OpenTelemetry) apontando para collectors definidos pelo Aspire no ambiente de dev.
5.  **Commit de mudanças:** Criar git commit explicando alterações realizadas nestá alteração

Itens criados/implementados durante a Fase 1
- `.gitignore`, `README.md`
- Solution e projetos (ver lista acima)
- `ProdAbs.SharedKernel`:
    - `Result` (`Result`, `Result<T>`)
    - `HashUtility` (`CalculateSha256Async`)
- `ProdAbs.Domain`:
    - Entidades simplificadas `TipoDocumento`, `Documento`, `Prontuario`
- `ProdAbs.Application`:
    - Interface `IFileStorageService`
- `ProdAbs.Infrastructure`:
    - `AppDbContext` (SQLite)
    - `LocalFileStorageService` (implementação de `IFileStorageService`)
- `ProdAbs.CrossCutting`:
    - `DependencyInjection` extensão para registrar infraestrutura
- `ProdAbs.Presentation.Api`:
    - `Program.cs` configurado com MediatR, EF Core (SQLite), Swagger e JWT mínimo

Como verificar (local)
- Build: executar na raiz do repositório:
```powershell
dotnet restore
dotnet build
```
- Subir o stack Aspire local (obrigatório para testes que dependem de infra):
- Documentar/usar o script fornecido para subir o Aspire dev stack (exemplo genérico):
```powershell
# Exemplo: subir stack Aspire (substitua pelo manifesto/criptografia do projeto)
docker-compose -f .\infrastructure\aspire\aspire-dev.yml up -d
# ou um script local: .\scripts\start-aspire-dev.ps1
```
- Após o stack Aspire estar em execução, executar a API apontando para as configurações do Aspire (ou use o comando dotnet run se a integração estiver configurada):
```powershell
dotnet run --project src/ProdAbs.Presentation.Api
```
- Validar health check da API (deve retornar OK e indicar conexão com Postgres/RabbitMQ via Aspire).
- Verificar arquivos criados: `git status` e `git log --oneline`.

Observações/assunções
- Aspire.NET é obrigatório: não será permitido um MVP que ignore a integração com Aspire em ambientes de desenvolvimento e produção. Para acelerar iterações locais, é aceitável ter um modo "dev-simplificado" que use SQLite/local storage para testes unitários rápidos, porém qualquer teste de integração ou validação end-to-end do MVP deverá executar contra o stack provido pelo Aspire (Postgres + RabbitMQ + coletores de telemetria).
- Não consegui capturar automaticamente o hash do commit via terminal nesta sessão; por favor rode `git rev-parse --short HEAD` localmente para confirmar o commit atual.

Próximos passos recomendados (após Fase 1)
- Implementar Controllers (TiposDocumento, Documentos, Prontuarios) e handlers MediatR mínimos.
- Escrever testes unitários simples para `HashUtility` e `LocalFileStorageService`.
- Executar um fluxo de upload/download via API (smoke test).

**Testes nesta fase:**
- Criar estrutura inicial de testes (`tests/ProdAbs.UnitTests`, `tests/ProdAbs.IntegrationTests`).
- Configurar framework de testes (xUnit/NUnit) e mocking (Moq).
- Criar testes básicos de *smoke test*:
  - `dotnet build` sem falhas.
  - API sobe e responde no health check.
- **Confirmação antes de avançar:** testes de smoke test devem passar no pipeline.

---


**FASE 2: Modelagem do Domínio e Abstrações (Prioridade Alta)**

1.  **SharedKernel: `Result` Pattern e `HashUtility`:**
    *   **Ação:** Em `ProjectName.SharedKernel/Utilities`, criar as classes `Result` e `Result<T>` conforme o padrão Result.
    *   **Ação:** Em `ProjectName.SharedKernel/Utilities`, criar a classe `HashUtility` com um método para calcular SHA256 de um `Stream`.
    *   **Entrega:** Classes `Result` e `HashUtility` funcionais e testadas unitariamente (seja com testes internos ou por um subprojeto de testes unitários dedicado ao SharedKernel).
2.  **Modelagem de `TipoDeDocumento` (Domain):**
    *   **Ação:** Em `ProjectName.Domain/Entities`, definir a entidade `TipoDeDocumento`.
    *   **Ação:** Em `ProjectName.Domain/ValueObjects`, definir `CampoMetadata` com `Label`, `RegraDeValidacao`, `Mascara`. `RegraValidacao` com `TipoDeDados` (string/int/date/etc.), `Obrigatoriedade`, `FormatoEspecífico`. (Não incluir `Regras de Vencimento`, `Agregamentos`, `Cálculos` para o MVP).
    *   **FluentValidation (obrigatório):** Para cada Value Object criado, adicionar um `Validator` baseado em FluentValidation (por exemplo `CampoMetadataValidator`) em `src/ProdAbs.Application/Validators` ou `src/ProdAbs.Domain/Validators` conforme convenção. Esses validators devem validar formatos, obrigatoriedade e regras simples de negócio.
    *   **Ação:** Em `ProjectName.Domain/Interfaces`, definir `ITipoDeDocumentoRepository`.
    *   **Entrega:** Modelos de domínio para `TipoDeDocumento` e VOs associados. Testes unitários para a criação e validação básica desses VOs.
3.  **Modelagem de `Documento` (Domain):**
    *   **Ação:** Em `ProjectName.Domain/Entities`, definir a entidade `Documento` com as propriedades `Id`, `TipoDeDocumentoId`, `StorageLocation` (string), `TamanhoEmBytes` (long), `HashTipo` (string), `HashValor` (string), `NomeArquivoOriginal` (string), `Formato` (string), `Status` (enum simples: "Criado", "Ativo"), `Versao` (int), e uma `IReadOnlyDictionary<string, string>` para `DicionarioDeCamposValores`.
    *   **Ação:** Em `ProjectName.Domain/ValueObjects`, definir `MetadadoDocumento` para encapsular `TamanhoEmBytes`, `HashTipo`, `HashValor`, `NomeArquivoOriginal`, `Formato`, `Versao`.
    *   **FluentValidation (obrigatório):** Implementar `MetadadoDocumentoValidator : AbstractValidator<MetadadoDocumento>` e escrever testes unitários cobrindo cenários válidos e inválidos.
    *   **Ação:** Em `ProjectName.Domain/Interfaces`, definir `IDocumentoRepository`.
    *   **Ação:** Em `ProjectName.SharedKernel/Events`, criar `DocumentoCriadoEvent` (contendo Id, TipoDocumentoId, StorageLocation, TamanhoEmBytes, HashValor) e `DocumentoRemovidoEvent`.
    *   **Entrega:** Modelo de domínio para `Documento` e VOs. Eventos de domínio básicos. Testes unitários para criação de `Documento`.
4.  **Modelagem de `Prontuário` (Domain):**
    *   **Ação:** Em `ProjectName.Domain/Entities`, definir a entidade `Prontuario` com `Id`, `IdentificadorEntidade` (ex: CPF/CNPJ), `TipoProntuario` (ex: "Paciente", "ClienteB2B"). Incluir uma lista de IDs de documentos (`List<Guid> DocumentoIds`).
    *   **Ação:** Em `ProjectName.Domain/Interfaces`, definir `IProntuarioRepository`.
    *   **Entrega:** Modelo de domínio para `Prontuário`. Testes unitários básicos para `Prontuário`.

**Testes nesta fase:**
- Criar testes unitários para:
  - `Result`, `Result<T>`.
  - `HashUtility`.
  - Entidade `TipoDeDocumento` e Value Objects associados.
- Adicionar validações negativas (ex.: criação inválida de VO).
- **Confirmação antes de avançar:** todos os testes unitários de domínio passando.

---

**FASE 3: Implementação da Camada de Aplicação (Core) (Prioridade Alta)**

1.  **Configuração do MediatR e DTOs:**
    *   **Ação:** No `ProjectName.Application`, adicionar o pacote `MediatR`.
    *   **Ação:** No `ProjectName.Application/DTOs`, criar os DTOs correspondentes aos modelos de domínio e respostas da API (e.g., `TipoDocumentoDetalhesDTO`, `DocumentoDTO`, `ProntuarioResumoDTO`).
    *   **Entrega:** MediatR configurado, DTOs essenciais criados.
2.  **Interfaces de Serviços de Aplicação:**
    *   **Ação:** Em `ProjectName.Application/Interfaces`, definir:
        *   `IFileStorageService`: `Task<Result<string>> UploadAsync(Stream fileStream, string fileName, string contentType);`, `Task<Result<Stream>> GetAsync(string storageLocation);`, `Task<Result> DeleteAsync(string storageLocation);`.
        *   `IEmailNotifier`: `Task SendEmailAsync(string to, string subject, string body);` (pode ser um mock simples para MVP).
        *   `IAuditLogger`: `Task LogAuditAsync(string action, string entityId, string userId, string details);` (pode ser um mock simples para MVP).
    *   **Entrega:** Interfaces de serviço de aplicação definidas.
3.  **Commands e Queries para o MVP:**
    *   **Ação:** Em `ProjectName.Application/Features/TiposDocumento`, criar: `CriarTipoDocumentoCommand` e `ListarTiposDocumentoQuery`.
    *   **Ação:** Em `ProjectName.Application/Features/Documentos`, criar:
        *   `CriarDocumentoCommand` (deve incluir `Stream` do arquivo, nome do arquivo, contentType, `TipoDocumentoId`, `DicionarioDeCamposValores`).
        *   `GetDocumentoByIdQuery`.
        *   `DownloadDocumentoQuery`.
    *   **Ação:** Em `ProjectName.Application/Features/Prontuarios`, criar:
        *   `CriarProntuarioCommand`.
        *   `AdicionarDocumentoAoProntuarioCommand` (inclui `ProntuarioId`, `DocumentoId`).
        *   `GetDocumentosDoProntuarioQuery` (retorna lista de `DocumentoDTO`s).
    *   **Entrega:** Comandos e Queries para o MVP definidos.
4.  **Implementação de Handlers (Lógica de Aplicação):**
    *   **Ação:** Para cada Command e Query acima, criar o handler correspondente em `ProjectName.Application/Features/.../Handlers`.
    *   **Ação:** Os handlers devem usar as interfaces de repositório (Domain) e as interfaces de serviços externos (Application).
    *   **Ação:** O `CriarDocumentoCommandHandler` deve:
        *   Receber o `Stream` do arquivo.
        *   Chamar `HashUtility.CalculateSha256HashAsync` no Stream.
        *   Chamar `IFileStorageService.UploadAsync` para armazenar o arquivo.
        *   Criar o `Documento` no domínio com a `StorageLocation`, `TamanhoEmBytes`, `HashTipo`, `HashValor`.
        *   Persistir o `Documento` via `IDocumentoRepository`.
        *   Publicar `DocumentoCriadoEvent` via MediatR (ou MassTransit, veja Fase 4).
        *   Retornar `Result<DocumentoDTO>`.
    *   **Ação:** O `DownloadDocumentoQueryHandler` deve:
        *   Buscar o `Documento` pelo ID (do Read Model).
        *   Chamar `IFileStorageService.GetAsync` com a `StorageLocation`.
        *   Retornar `Result<Stream>`.
    *   **Ação:** Utilizar `Result.Ok()`, `Result.Fail()` e `Result.Success<T>()`, `Result.Failure<T>()` em todos os retornos de handler.
    *   **Entrega:** Handlers implementados para o MVP, utilizando o Padrão Result. Testes unitários para os handlers (mockando todas as dependências externas e de repositório).

**Testes nesta fase:**
- Criar testes unitários para:
  - Commands/Queries (usando mocks de repositórios e serviços externos).
  - Handlers (cenários de sucesso e falha).
- Executar testes de **integração leve** para validar DTOs e serialização básica.
- **Confirmação antes de avançar:** cobertura mínima validando comandos e queries principais.

---

**FASE 4: Integrações de Infraestrutura Essenciais (Prioridade Alta)**

1.  **Implementação dos Repositórios (Marten & EF Core):**
    *   **Ação:** Em `ProjectName.Infrastructure/Data/Marten`, configurar o `MartenRegistry` para mapear os agregados `TipoDeDocumento`, `Documento`, `Prontuario` e os eventos de domínio.
    *   **Ação:** Em `ProjectName.Infrastructure/Data/EfCore`, criar `AppDbContext`.
    *   **Ação:** Em `ProjectName.Infrastructure/Data/Repositories`, implementar `DocumentoRepository`, `TipoDocumentoRepository`, `ProntuarioRepository` usando o `IDocumentSession` do Marten para operações de escrita. Para queries de leitura de documentos/prontuários/tipos (que não precisam de Event Sourcing), usar o `AppDbContext` do EF Core.
    *   **Ação:** Criar uma projeção Marten inicial (em `ProjectName.Infrastructure/Data/Marten/Projections`) que consome `DocumentoCriadoEvent` (via MassTransit) e popula um `DocumentoReadModel` no EF Core para consultas otimizadas.
    *   **Entrega:** Repositórios funcionais para persistência do domínio via Marten e consultas via EF Core.
2.  **Implementação do `LocalFileStorageService`:**
    *   **Ação:** Em `ProjectName.Infrastructure/FileStorage`, criar `LocalFileStorageService` que implementa `IFileStorageService`. Este serviço deve salvar/carregar arquivos em uma pasta local configurável (e.g., via `IOptions<FileStorageSettings>`).
    *   **Entrega:** Serviço de armazenamento de arquivos local funcional.
3.  **Configuração do MassTransit (Mínimo para Eventos):**
    *   **Ação:** Em `ProjectName.Infrastructure/Messaging`, configurar o MassTransit para se conectar ao RabbitMQ (orquestrado pelo Aspire).
    *   **Ação:** Registrar o *consumer* da projeção Marten (`DocumentoReadModelProjection`) para o `DocumentoCriadoEvent`.
    *   **Entrega:** Mensageria de eventos básica funcionando para Event Sourcing.
4.  **Configuração de Injeção de Dependência:**
    *   **Ação:** Em `ProjectName.CrossCutting/DependencyInjection.cs`, configurar todos os serviços, repositórios e handlers para injeção de dependência. Garantir que a implementação de `IFileStorageService` (`LocalFileStorageService`) seja registrada.

**Entregas da Fase:** Integração completa entre Application e Infrastructure para persistência e armazenamento de arquivos. Testes de integração para os repositórios e `LocalFileStorageService`.

**Testes nesta fase:**
- Criar testes de integração para:
  - `AppDbContext` (in-memory ou SQLite).
  - `LocalFileStorageService` (upload/download/delete).
  - Repositórios (`TipoDocumentoRepository`, `DocumentoRepository`, `ProntuarioRepository`).
- **Confirmação antes de avançar:** todos os testes de integração passando para persistência e storage.

---

**FASE 5: Desenvolvimento da Camada de Apresentação (API Core) (Prioridade Alta)**

1.  **Criação de Controllers para MVP:**
    *   **Ação:** Em `ProjectName.Presentation.Api/Controllers`, criar:
        *   `TiposDocumentoController`: Com endpoints POST para criar e GET para listar.
        *   `DocumentosController`: Com endpoints POST para upload de arquivos (`IFormFile`), GET para buscar por ID e GET para download de arquivos.
        *   `ProntuariosController`: Com endpoints POST para criar prontuários, POST para adicionar documento a prontuário e GET para listar documentos de um prontuário.
    *   **Ação:** Utilizar `[ApiController]` e `[Route("api/v1/[controller]")]`.
    *   **Entrega:** Controllers básicos para o MVP.
2.  **Manuseio de `Result` em Controllers:**
    *   **Ação:** Para cada endpoint, chamar o MediatR para disparar o Command ou Query.
    *   **Ação:** Implementar a lógica para inspecionar o `Result` retornado pelo handler:
        *   Se `Result.IsSuccess`, retornar `Ok()`, `CreatedAtAction()`, ou `File()`.
        *   Se `Result.IsFailure`, retornar um `ProblemDetails` apropriado (ex: `BadRequest()` para validação/negócio, `NotFound()` para recurso não encontrado).
    *   **Entrega:** Controllers que traduzem `Result` para respostas HTTP.
3.  **Configuração de Segurança (Autenticação Básica):**
    *   **Ação:** Configurar JWT Bearer Authentication na `Program.cs` (`AddAuthentication`, `AddAuthorization`).
    *   **Ação:** Adicionar atributos `[Authorize]` nos controllers para proteger os endpoints.
    *   *(Para MVP, pode-se usar um User/Role de teste em memória ou um JWT estático, deixando a integração completa com Keycloak para fases futuras).*
    *   **Entrega:** Endpoints API protegidos por autenticação.
4.  **Tratamento Global de Exceções:**
    *   **Ação:** Na `Program.cs`, adicionar um middleware para capturar exceções não tratadas e retornar `Problem Details` (`app.UseExceptionHandler("/error")` e um endpoint `/error` ou um custom middleware mais robusto).
    *   **Entrega:** Tratamento de exceções global configurado.
5.  **Documentação da API (Swagger):**
    *   **Ação:** Configurar Swagger/OpenAPI na `Program.cs` para documentar os endpoints do MVP.
    *   **Entrega:** Swagger UI disponível.
6.  **Observabilidade Básica:**
    *   **Ação:** Configurar Serilog na `Program.cs` e `appsettings.json` para logging estruturado no console e em arquivo (ou um sink simples para Seq se disponível no ambiente de dev).
    *   **Ação:** Adicionar `UseOpenTelemetryPrometheusScraping()` e `AddServiceDefaults()` para telemetria básica com Aspire.
    *   **Entrega:** Logging e telemetria básica em funcionamento.

**Testes nesta fase:**
- Criar testes de integração (Web API tests) para:
  - Endpoints de TiposDocumento, Documentos e Prontuários.
  - Autenticação JWT básica.
  - Tratamento de erros (`ProblemDetails`).
- **Confirmação antes de avançar:** fluxo end-to-end validado via testes (upload, download e prontuário).

---

**FASE 6: Testes e Refinamento do MVP (Prioridade Alta)**

1.  **Testes Unitários:**
    *   **Ação:** Garantir que todos os componentes das camadas Domain e Application (Value Objects, Entities, Domain Services, Commands, Queries, Handlers) tenham testes unitários cobrindo os cenários de sucesso e falha (retorno de `Result`).
    *   **Ação:** Mockar `IFileStorageService`, `IDocumentoRepository`, etc., nos testes da camada Application.
    *   **Entrega:** Conjunto robusto de testes unitários para o MVP.
2.  **Testes de Integração:**
    *   **Ação:** Escrever testes de integração para os repositórios (Marten e EF Core) para verificar a persistência correta.
    *   **Ação:** Escrever testes de integração para o `LocalFileStorageService`.
    *   **Ação:** Escrever testes de integração da API (Web.Tests) para validar os fluxos completos do MVP, incluindo upload, download e criação de prontuários.
    *   **Entrega:** Testes de integração para as funcionalidades do MVP.

**Testes nesta fase:**
- Testes unitários de todas as camadas (Domain, Application).
- Testes de integração de Infraestrutura e API.
- **Confirmação final:** somente avançar para release se todos os testes estiverem verdes.


---

**RESULTADO FINAL ESPERADO DO MVP:**

*   Uma solução .NET funcional que pode ser iniciada e operada via Aspire (stack obrigatório). A solução deve conectar-se aos serviços gerenciados pelo Aspire: PostgreSQL (persistência principal), RabbitMQ (mensageria/eventos), e collectors de OpenTelemetry para observabilidade.
*   API REST que permite:
    *   Criar e listar Tipos de Documento.
    *   Fazer upload de documentos (PDF, JPG, etc.), calculando e armazenando seu hash e tamanho.
    *   Baixar documentos pelo ID.
    *   Criar prontuários e associar documentos a eles.
    *   Listar documentos de um prontuário.
*   Autenticação básica funcionando na API.
*   Tratamento de erros via Padrão Result e Problem Details.
*   Logs e telemetria básicos coletados.
*   Conjunto de testes unitários e de integração para o MVP.

**COMO REPORTAR O PROGRESSO:**

Após cada etapa concluída (ou em marcos intermediários significativos), forneça:
*   A confirmação da conclusão da etapa.
*   Um breve `git status` para mostrar as modificações.
*   Comandos executados (ex: `dotnet new`, `dotnet add package`, `dotnet run`).
*   Qualquer saída relevante (ex: logs do Aspire Dashboard, resultados de testes).
*   Perguntas ou problemas encontrados.

**AÇÃO INICIAL:**
Comece pela `FASE 1: Configuração e Fundação`. Confirme a conclusão de 1.1 "Revisão e Alinhamento da Arquitetura Base" antes de prosseguir.

** Observação Importante: **
Certifique-se de não utilizar --force em nenhum dos comandos dotnet, a menos que explicitamente solicitado. O uso de --force pode causar perda de dados ou inconsistências no repositório.