# GEMINI.MD - Estrutura de Arquitetura da Aplicação .NET

Este documento detalha a estrutura de arquitetura da aplicação .NET, fornecendo um guia abrangente sobre os princípios, padrões, tecnologias e convenções que governam seu desenvolvimento. Nosso objetivo é construir uma aplicação robusta, escalável, manutenível e testável, seguindo as melhores práticas da indústria e **evitando over-engineering** através de escolhas arquitetônicas pragmáticas e comprovadas.

---

## 1. Visão Geral e Propósito

Este documento serve como a fonte primária de verdade para a arquitetura do projeto. Ele descreve:
*   Os princípios arquitetônicos fundamentais.
*   A escolha e o rationale por trás do estilo arquitetônico (Clean Architecture).
*   A estrutura de camadas e projetos.
*   As principais tecnologias e ferramentas utilizadas.
*   As estratégias para testes, segurança, logging, etc.
*   **Linguagem Ubíqua (Ubiquitous Language)**: Uma linguagem compartilhada entre desenvolvedores e especialistas do domínio para descrever o software, neste contexto, incluindo termos como **Documento**, **Tipo de Documento**, **Prontuário**, **Metadados**, **Regras de Vencimento**, **Armazenamento de Arquivos**, **Consulta Avançada**, **Uso de Armazenamento**, **Hash do Documento** e **Resultado da Operação (Result)**.

Ele é destinado a todos os membros da equipe de desenvolvimento, operações e stakeholders interessados em entender a fundação técnica da aplicação.

## 2. Princípios Arquitetônicos Fundamentais

Nossa arquitetura é guiada pelos seguintes princípios:

*   **Separação de Preocupações (SoC):** Cada componente ou camada deve ter uma responsabilidade bem definida e única. *No contexto do GED, o domínio de "Documentos" e "Prontuários" é claramente separado da infraestrutura de armazenamento, da interface de usuário e dos serviços de busca.*
*   **Baixo Acoplamento:** As camadas e módulos devem ser o mais independentes possível, minimizando dependências diretas. *A lógica de negócio de validação de documentos não deve depender do banco de dados específico, nem do tipo de armazenamento físico do arquivo, nem do motor de busca.*
*   **Alta Coesão:** Os elementos dentro de um módulo ou camada devem estar fortemente relacionados entre si. *Todas as regras de um "Tipo de Documento" devem residir em um único local lógico.*
*   **Testabilidade:** A arquitetura deve facilitar a escrita de testes unitários, de integração e ponta a ponta. *As regras complexas de vencimento ou agregação de documentos devem ser facilmente testáveis sem infraestrutura externa. Os serviços de busca e armazenamento de arquivos podem ser mockados.*
*   **Escalabilidade:** Capacidade de a aplicação crescer e lidar com aumento de carga de trabalho eficientemente. *Um alto volume de documentos e prontuários deve ser suportado, com o sistema podendo escalar para atender a demandas crescentes, incluindo o armazenamento de um volume crescente de arquivos e a capacidade de realizar consultas avançadas em grandes datasets.*
*   **Manutenibilidade:** Código claro, organizado e fácil de entender e modificar. *Adicionar um novo "Tipo de Documento" ou uma nova regra de validação deve ser direto, assim como mudar a estratégia de armazenamento de arquivos ou a tecnologia de busca.*
*   **Extensibilidade:** Capacidade de adicionar novas funcionalidades com mínimo impacto no código existente. *A introdução de novos tipos de anexos, novas formas de cálculo em documentos, ou novos critérios de busca deve ser facilitada.*
*   **Performance:** Otimização para garantir respostas rápidas e uso eficiente de recursos. *A busca e recuperação de documentos e prontuários devem ser rápidas, mesmo com grandes volumes de dados, e o acesso aos arquivos armazenados deve ser eficiente. A consulta avançada deve retornar resultados em tempo hábil.*
*   **Segurança:** Implementação de controles de segurança em todas as camadas, desde o design inicial. *A confidencialidade dos Prontuários (especialmente B2P) é primordial, com controle de acesso granular e trilha de auditoria rigorosa, estendendo-se à segurança do acesso aos arquivos armazenados e à **integridade dos documentos (via hash).***
*   **Princípios SOLID:**
    *   **S**ingle Responsibility Principle (SRP)
    *   **O**pen/Closed Principle (OCP)
    *   **L**iskov Substitution Principle (LSP)
    *   **I**nterface Segregation Principle (ISP)
    *   **D**ependency Inversion Principle (DIP)
*   **Previsibilidade e Tratamento Explícito de Falhas:** Utilização do **Padrão Result** para operações que podem falhar de forma previsível (ex: validações, regras de negócio violadas, recurso não encontrado), tornando o fluxo de erro claro e explícito, separando-o de exceções para falhas inesperadas.

## 3. Estilo Arquitetônico: Clean Architecture (Arquitetura Limpa)

Optamos pela **Clean Architecture** (também conhecida como Onion Architecture, Hexagonal Architecture) devido à sua capacidade de criar sistemas robustos, independentes de frameworks, bancos de dados e interfaces de usuário.

### 3.1. Rationale para Clean Architecture

*   **Independência:** O código do domínio (regras de negócio) é independente de qualquer tecnologia externa (banco de dados, frameworks web, etc.). *Isso significa que as regras de validação, vencimento e agregação de documentos são o cerne da aplicação, independentes de como são persistidas ou apresentadas, de onde e como os arquivos são fisicamente armazenados, e de como a busca é realizada.*
*   **Testabilidade:** Facilita a escrita de testes automatizados, pois as regras de negócio podem ser testadas isoladamente, sem a necessidade de infraestrutura. *Testar a lógica de um "Tipo de Documento" ou a validade de um "Documento" não requer um banco de dados real, nem um sistema de arquivos/bucket de nuvem, nem um motor de busca.*
*   **Manutenibilidade:** Alterações em uma camada raramente afetam as outras, desde que as interfaces sejam mantidas. *Mudar a tecnologia de banco de dados não afetaria as regras de negócio de documentos, e mudar o provedor de armazenamento de arquivos (ex: de local para S3) ou o motor de busca (ex: de ElasticSearch para Azure Cognitive Search) não afetaria o domínio ou a aplicação.*
*   **Flexibilidade:** Permite a substituição de tecnologias subjacentes com impacto mínimo (ex: trocar de SQL Server para PostgreSQL). *A mesma flexibilidade se aplica ao armazenamento de arquivos e ao motor de busca.*

### 3.2. As Camadas da Clean Architecture no Contexto GED/Prontuários

A Clean Architecture é organizada em camadas concêntricas, onde as dependências fluem apenas para dentro (as camadas externas dependem das internas, mas as internas não dependem das externas).

![Clean Architecture Diagram](https://raw.githubusercontent.com/ardalis/CleanArchitecture/main/docs/clean-architecture-diagram.png)
_Fonte: [ardalis/CleanArchitecture](https://github.com/ardalis/CleanArchitecture)_

1.  **Shared Kernel:**
    *   Contém tipos de dados e interfaces comuns que não pertencem a um domínio específico, mas são usadas amplamente.
    *   Exemplos: `BaseValueObject`, enums genéricos.
    *   **Adições:**
        *   **`Entity<TId>`**: Uma classe base abstrata da qual todas as entidades do domínio herdam. Ela fornece uma implementação padrão para identidade, garantindo que as entidades sejam comparadas por seu `Id` único, e não por seus valores de propriedade. Inclui sobrecargas para operadores de igualdade (`==`, `!=`) e `GetHashCode`.
        *   **`AggregateRoot<TId>`**: Uma classe base que herda de `Entity<TId>` e serve como marcador e base para as Raízes de Agregado. Ela é o ponto de entrada para um agregado, responsável por manter a consistência transacional de todos os objetos dentro de seus limites.
        *   **`Result` e `Result<T>`:** Classes que encapsulam o resultado de uma operação, indicando sucesso ou falha, e fornecendo informações sobre o erro em caso de falha. Isso promove o tratamento explícito de erros esperados.
        *   Classes utilitárias para geração de hash (`HashUtility`).

2.  **Domain (Entidades e Regras de Negócio):**
    *   **Coração da aplicação.** Contém as **entidades**, **value objects**, **agregados**, **especificações**, **domain services** e **interfaces de repositório**. Não possui dependências de outras camadas. É a camada mais interna e estável.
    *   **Responsabilidade:** Encapsular as regras de negócio da empresa e o estado das entidades.
    *   **No contexto GED/Prontuários:**
        *   **Entidades/Agregados:**
            *   `Documento`: Entidade principal que representa um documento único. **Herda de `AggregateRoot<Guid>`**, encapsulando seu `TipoDeDocumento` (por referência), seus metadados e os campos/valores preenchidos.
            *   `TipoDeDocumento`: Uma **`AggregateRoot<Guid>`** que define a estrutura de um tipo específico de documento, incluindo seus campos, regras de validação, máscaras, regras de vencimento, etc.
            *   `Prontuario`: Uma **`AggregateRoot<Guid>`** que atua como um container lógico para agrupar múltiplos `Documentos` relacionados a uma entidade específica (pessoa, empresa, projeto).
        *   **Value Objects:**
            *   `CampoMetadata`, `ValorCampo`, `RegraValidacao`, `RegraVencimento`, `MetadadoDocumento`.
        *   **Domain Services:**
            *   **Podem retornar `Result` ou `Result<T>`:** Ex: `ServicoValidacaoDocumento.ValidarDocumento(Documento doc, TipoDeDocumento tipo)` pode retornar `Result`.
        *   **Interfaces de Repositório:** `IDocumentoRepository`, `ITipoDeDocumentoRepository`, `IProntuarioRepository`.

3.  **Application (Casos de Uso e Orquestração):**
    *   Contém os **casos de uso (commands e queries)**, **DTOs**, **Application Services** e **interfaces para serviços externos**. Depende apenas da camada **Domain**.
    *   **Responsabilidade:** Orquestrar o fluxo de dados entre o Domain e a interface de usuário/infraestrutura.
    *   **No contexto GED/Prontuários:**
        *   **Commands:** (Escrita) **Retornam `Result` ou `Result<T>`**.
        *   **Queries:** (Leitura) **Retornam `Result<T>`**.
        *   **Interfaces de Serviços Externos:** `IFileStorageService`, `ISearchService`, `IStorageMetricsService`, `IEmailNotifier`, `IAuditLogger`. As operações destas interfaces devem, preferencialmente, retornar `Result<T>` para falhas esperadas.
        *   **Handlers:** Classes que implementam `IRequestHandler` (via MediatR).

4.  **Infrastructure (Implementações Externas):**
    *   Contém as **implementações concretas das interfaces** definidas em Domain e Application. Depende das camadas **Domain** e **Application**.
    *   **Responsabilidade:** Lidar com a tecnologia externa (banco de dados, sistemas de arquivos, APIs de terceiros).
    *   **No contexto GED/Prontuários:**
        *   **Data/Persistence:** Implementações dos repositórios (Marten para escrita, EF Core para projeções de leitura).
        *   **FileStorage/:** Implementações de `IFileStorageService`.
        *   **Search/:** Implementações de `ISearchService`.
        *   **Metrics/:** Implementação de `IStorageMetricsService`.

5.  **Presentation (API / UI):**
    *   Ponto de entrada da aplicação (API REST, Blazor UI, etc.). Depende da camada **Application**.
    *   **Responsabilidade:** Receber requisições, traduzir para comandos/queries e retornar as respostas.
    *   **No contexto GED/Prontuários:**
        *   **Controllers API REST:** `DocumentosController`, `ProntuariosController`, etc.
        *   **Manuseio de `Result` em Controllers:** Traduzem objetos `Result` para respostas HTTP apropriadas (`200 OK`, `400 Bad Request`, `404 Not Found`).
        *   **Configuração de DI:** O `Program.cs` atua como a **Raiz de Composição (Composition Root)**, orquestrando o registro de todas as dependências.

## 4. Estrutura de Projetos (Visual Studio)

A estrutura de pastas e projetos reflete a arquitetura de camadas:

```

├── src/
│ ├── ProjectName.Domain/ (Camada de Domínio)
│ │ ├── Entities/
│ │ │ ├── Documento.cs (Herda de AggregateRoot<Guid>)
│ │ │ ├── TipoDocumento.cs (Herda de AggregateRoot<Guid>)
│ │ │ └── Prontuario.cs (Herda de AggregateRoot<Guid>)
│ │ ├── ValueObjects/
│ │ │ ├── ...
│ │ ├── Services/ (Domain Services)
│ │ │ ├── ...
│ │ └── Interfaces/ (IRepository, IUnitOfWork)
│ │ ├── ...
│ │
│ ├── ProjectName.Application/ (Camada de Aplicação)
│ │ ├── Features/
│ │ │ ├── Documentos/
│ │ │ │ ├── Commands/
│ │ │ │ ├── Queries/
│ │ │ │ └── Handlers/
│ │ │ ├── ... (Outras features)
│ │ ├── DTOs/
│ │ ├── Interfaces/
│ │ └── DependencyInjection.cs (NOVO: Injeções de dependência da camada de Aplicação)
│ │
│ ├── ProjectName.Infrastructure/ (Camada de Infraestrutura)
│ │ ├── Data/
│ │ │ ├── Repositories/
│ │ │ ├── ...
│ │ ├── Services/
│ │ ├── FileStorage/
│ │ ├── Search/
│ │ ├── Metrics/
│ │ └── DependencyInjection.cs (NOVO: Injeções de dependência da camada de Infraestrutura)
│ │
│ ├── ProjectName.Presentation.Api/ (Camada de Apresentação - Exemplo API REST)
│ │ ├── Controllers/
│ │ │ ├── ...
│ │ ├── Middlewares/
│ │ ├── appsettings.json
│ │ ├── Program.cs (Chama os métodos de injeção das outras camadas)
│ │ └── DependencyInjection.cs (NOVO: Injeções de dependência da camada de Apresentação)
│ │
│ ├── ProjectName.SharedKernel/
│ │ ├── BaseClasses/ (NOVO)
│ │ │ ├── Entity.cs (NOVO: Classe base para entidades com identidade e comparação)
│ │ │ └── AggregateRoot.cs (NOVO: Classe base para Raízes de Agregado)
│ │ ├── Events/
│ │ ├── Utilities/
│ │ │ ├── HashUtility.cs
│ │ │ └── Result.cs
│ │
│ ├── ProjectName.AppHost/
│ │ └── Program.cs
│ │
│ └── ProjectName.ServiceDefaults/
│ └── Extensions.cs
│
├── tests/
│ ├── ProjectName.UnitTests/
│ ├── ProjectName.IntegrationTests/
│ └── ProjectName.EndToEndTests/
│
├── docs/
├── build/
└── .github/

```


## 5. Tecnologias e Ferramentas Principais

*   **.NET (Core) X.X**: Framework principal.
*   **C#**: Linguagem de programação.
*   **ASP.NET Core**: Para APIs web.
*   **ASP.NET Aspire**: Para orquestração local de serviços e telemetria.
*   **Entity Framework Core (EF Core)**: Para o lado de leitura (Queries) e projeções.
*   **MediatR**: Para implementação do padrão CQRS leve.
*   **Mapperly**: Para mapeamento de objetos.
*   **FluentValidation**: Para validação de dados.
*   **xUnit**: Framework de testes unitários.
*   **Moq / NSubstitute**: Para mocking em testes.
*   **Serilog / Seq**: Para logging estruturado.
*   **Swagger / OpenAPI**: Para documentação de APIs.
*   **Docker**: Para conteinerização.
*   **Git**: Controle de versão.
*   **MassTransit**: Para mensageria e eventos de domínio.
*   **Marten**: Para Event Sourcing, utilizado especificamente para a trilha de auditoria.
*   **PostgreSQL**: Banco de dados principal, utilizado em duas instâncias separadas: uma para os dados da aplicação (via EF Core) e outra dedicada para a trilha de auditoria (via Marten).
*   **SonarAnalyzer.CSharp**: Análise de código estático.
*   **Keycloak**: Para gerenciamento de identidade e acesso.
*   **OpenTelemetry**: Para coleta de telemetria.
*   **SDKs de Provedores de Nuvem**: Para interagir com serviços de armazenamento.
*   **Elasticsearch.Client / Azure.Search.Documents**: Para interagir com o motor de busca.

## 6. Estratégias e Padrões Chave

### 6.1. Gerenciamento de Dependências (Injeção de Dependência - DI)

*   Utilizamos o container de DI built-in do .NET Core. A configuração das dependências é distribuída, seguindo o princípio da Separação de Preocupações.
*   **Injeção de Dependência por Camada:** Cada camada (`Application`, `Infrastructure`, `Presentation.Api`) possui seu próprio arquivo `DependencyInjection.cs`. Este arquivo contém um método de extensão estático para `IServiceCollection` (ex: `AddApplicationServices`, `AddInfrastructureServices`) que é responsável por registrar todas as interfaces e implementações daquela camada específica.
*   **Composição na Camada de Apresentação:** O ponto de entrada da aplicação (o arquivo `Program.cs` em `ProjectName.Presentation.Api`) atua como a **Raiz de Composição (Composition Root)**. Ele invoca os métodos de extensão de cada camada para construir o grafo completo de dependências da aplicação. Essa abordagem mantém a configuração de cada camada isolada e auto-contida.
*   **Princípio DIP (Dependency Inversion Principle):** É seguido rigorosamente, dependendo de abstrações (interfaces) ao invés de implementações concretas. *Isso permite que as regras de domínio de documentos e prontuários sejam desacopladas da sua persistência, da forma como são exibidas, de como os arquivos são fisicamente armazenados, de como a busca é realizada e de como as métricas são coletadas.*
*   As interfaces `IFileStorageService`, `ISearchService` e `IStorageMetricsService` são definidas na camada `Application` e suas implementações concretas são registradas no `DependencyInjection.cs` da camada `Infrastructure`.
*   **ASP.NET Aspire** simplificará o registro e a configuração de *backing services* e outros projetos da solução para o contêiner de DI.

### 6.2. Acesso a Dados (Persistence)

*   **Estratégia de Duas Bases de Dados:** Para garantir um isolamento robusto entre os dados operacionais e os dados de auditoria, utilizaremos duas bases de dados PostgreSQL separadas:
    1.  **Base de Dados da Aplicação:** Contém o estado atual de todas as entidades (`Documento`, `TipoDeDocumento`, `Prontuario`, etc.). É gerenciada pelo **Entity Framework Core**.
    2.  **Base de Dados de Auditoria:** Contém um log imutável de todos os eventos que modificam o estado do sistema. É gerenciada pelo **Marten**, atuando como nossa trilha de auditoria (`Audit Trail`).
*   **CQRS (Command Query Responsibility Segregation):** Adotaremos um estilo CQRS leve.
    *   **Lado de Escrita (Commands):** Utilização de **Entity Framework Core** para persistir o estado atual dos agregados na base de dados da aplicação. Eventos de domínio são despachados para registrar as alterações na base de dados de auditoria via Marten.
    *   **Lado de Leitura (Queries):** Utilização de **Entity Framework Core** sobre a base de dados da aplicação para consultas e projeções otimizadas.
*   **Padrão Repository & Unit of Work:** Interfaces de repositório definidas na camada `Domain` e implementadas na `Infrastructure`, operando sobre a base de dados da aplicação.
*   **Design por Domínio (Domain-Driven Design - DDD):** Aplicaremos conceitos de DDD como Agregados, Raízes de Agregados (formalizados através da herança da classe base **`AggregateRoot<TId>`** para `Documento`, `TipoDeDocumento` e `Prontuario`), e Value Objects (`CampoMetadata`, `RegraValidacao`) para modelar o domínio de forma rica e transacionalmente consistente.

### 6.3. API Design

*   **RESTful Principles:** Seguiremos os princípios REST para APIs HTTP.
*   **Versionamento:** As APIs serão versionadas (ex: `api/v1/documentos`).
*   **Tratamento de Resultados:** Endpoints da API inspecionam o `Result` retornado pelos handlers e retornam a resposta HTTP apropriada (200, 400, 404, etc.).
*   **Autenticação e Autorização:** Utilização de JWT e integração com **Keycloak**.

### 6.4. Tratamento de Erros e Exceções

*   **Padrão Result para Falhas Esperadas:** Operações nas camadas `Domain` e `Application` retornam `Result` ou `Result<T>` para indicar sucesso ou falha previsível (erros de validação, regras de negócio, recurso não encontrado).
*   **Exceções para Falhas Inesperadas:** Exceções são lançadas para problemas sistêmicos (banco de dados inacessível, falha de rede). Um **Middleware Global de Exceções** as captura e transforma em respostas `500 Internal Server Error` com `Problem Details`.

### 6.5. Logging e Monitoramento

*   **Serilog:** Utilizado para logging estruturado em todas as camadas. Logs são enriquecidos com informações de sucesso/falha dos `Result` e contexto de auditoria.
*   **OpenTelemetry:** Integração facilitada pelo **ASP.NET Aspire** para coletar logs, métricas e traces distribuídos, permitindo visibilidade completa do fluxo de requisições.

### 6.6. Configuração

*   Utilização de `appsettings.json`, variáveis de ambiente e serviços de segredos (Azure Key Vault). O arquivo de configuração conterá duas connection strings distintas: uma para a base de dados da aplicação (EF Core) e outra para a base de dados de auditoria (Marten).
*   Padrão `IOptions<T>` para injeção de configurações tipadas.

### 6.7. Testes

*   **Testes Unitários:** Focados nas camadas `Domain` e `Application`, validando regras de negócio e a lógica dos handlers, incluindo a verificação do retorno correto de `Result`. As dependências externas (`IFileStorageService`, etc.) são mockadas.
*   **Testes de Integração:** Validam a interação entre camadas, usando instâncias reais (via Testcontainers) de banco de dados, serviços de armazenamento, etc. Verificam os retornos `Result` das implementações concretas.
*   **Testes End-to-End (E2E):** (Opcional) Simulam o fluxo completo do usuário.

### 6.8. Padrões de Projeto Adicionais

*   **Builder Pattern**: Para criação de objetos complexos (ex: DTOs, Commands).
*   **Event Sourcing para Auditoria**: Implementado com **Marten** para garantir um histórico imutável de todas as alterações, fundamental para a trilha de auditoria.
*   **Scheduler/Background Jobs**: Para tarefas como verificação de vencimento de documentos (via Hosted Services).

### 6.9. Estratégia de Armazenamento de Arquivos Digitais

*   **Abstração via Interface:** Uma interface `IFileStorageService` na `Application` define as operações (`UploadAsync`, `GetAsync`, `DeleteAsync`), cujos métodos retornam `Result` para tratamento explícito de falhas.
*   **Implementações na Infraestrutura:** A camada `Infrastructure.FileStorage` contém as implementações concretas (`LocalFileStorageService`, `AzureBlobStorageService`).
*   **Referência no Domínio:** A entidade `Documento` armazena apenas a `StorageLocation` (identificador único), nunca o conteúdo binário.
*   **Fluxo de Upload:** O handler do comando calcula o hash, chama o `IFileStorageService` para fazer o upload e, se bem-sucedido, persiste a `StorageLocation`, tamanho e hash no agregado `Documento`.

### 6.10. Consulta Avançada de Documentos

*   **Abstração via Interface:** Uma interface `ISearchService` na `Application`.
*   **Tecnologia de Busca Dedicada:** Um motor de busca como **Elasticsearch** ou **Azure Cognitive Search**.
*   **Mecanismo de Indexação:** Consumidores de eventos (`DocumentoCriadoEvent`) usam o `ISearchService` para indexar os documentos e seus metadados no motor de busca.
*   **Mecanismo de Consulta:** O handler da query de busca avançada usa o `ISearchService` para executar a consulta, retornando os resultados.

### 6.11. Total Utilizado de Armazenamento

*   **Interface de Serviço de Métricas:** `IStorageMetricsService` na `Application`.
*   **Persistência Dedicada:** Uma tabela simples no PostgreSQL (via EF Core) para armazenar o uso total.
*   **Atualização por Eventos:** Consumidores de eventos (`DocumentoCriadoEvent`, `DocumentoRemovidoEvent`) chamam os métodos do `IStorageMetricsService` para adicionar ou remover o tamanho do arquivo da métrica total.

### 6.12. Coleta de Hash do Documento por Versão

*   **Cálculo na Criação/Atualização:** O hash do arquivo é calculado pelo handler do comando antes do upload.
*   **Armazenamento no Domínio:** As propriedades `HashTipo` e `HashValor` são armazenadas na entidade `Documento`. O histórico de alterações, incluindo hashes anteriores, é mantido na trilha de auditoria via Event Sourcing.
*   **Verificação de Integridade:** Opcionalmente, o hash pode ser verificado no momento do download para garantir que o arquivo não foi corrompido.

## 7. Princípios de Aplicações Cloud-Native (The Twelve-Factor App)

Aderimos aos princípios do [The Twelve-Factor App](https://12factor.net/pt_br/) para guiar o desenvolvimento, com o **ASP.NET Aspire** nos auxiliando na implementação de muitos desses fatores, como dependências explícitas, configuração no ambiente e paridade dev/prod.

1.  **I. Codebase:** Um código-base no Git, muitas implantações.
2.  **II. Dependencies:** Dependências explícitas (`.csproj`) e isoladas (interfaces, DI).
3.  **III. Config:** Configurações armazenadas no ambiente ou em serviços de segredos.
4.  **IV. Backing Services:** Serviços de apoio (banco de dados, mensageria, armazenamento, busca) tratados como recursos conectados e plugáveis.
5.  **V. Build, Release, Run:** Estágios de construção e execução rigorosamente separados.
6.  **VI. Processes:** Aplicação executada como processos stateless, permitindo escalabilidade horizontal.
7.  **VII. Port Binding:** Serviços exportados via associação de portas.
8.  **VIII. Concurrency:** Escalabilidade horizontal via o modelo de processo.
9.  **IX. Disposability:** Inicialização rápida e desligamento elegante.
10. **X. Dev/Prod Parity:** Ambientes de desenvolvimento e produção o mais similares possível.
11. **XI. Logs:** Logs tratados como fluxos de eventos e enviados para um agregador central.
12. **XII. Admin Processes:** Tarefas administrativas executadas como processos one-off.

## 8. Considerações Adicionais para GED/Prontuários

### 8.1. Modelagem para B2B e B2P

A arquitetura é flexível para suportar ambos os cenários. A diferenciação ocorre primariamente na modelagem de **domínio**, com entidades e regras específicas para cada contexto (`ProntuarioPaciente` vs. `ProntuarioFornecedor`).

### 8.2. Segurança e Conformidade (LGPD, HIPAA, etc.)

*   **Controle de Acesso Baseado em Papéis (RBAC):** Integrado com Keycloak.
*   **Criptografia:** Dados sensíveis em repouso e em trânsito.
*   **Trilha de Auditoria (Audit Trail):** O uso de Event Sourcing com Marten, especificamente para a auditoria, garante uma trilha de alterações completa e imutável, armazenada em uma base de dados dedicada e isolada para máxima segurança.
*   **Políticas de Retenção e Descarte:** As `Regras de Vencimento` guiam o ciclo de vida dos documentos.
*   **Integridade via Hash:** O hash armazenado garante a não-repúdio e a integridade do documento.

## 9. Próximos Passos e Contribuição

*   **Ambiente de Desenvolvimento:** Veja o `CONTRIBUTING.MD` (se houver) para configurar o ambiente com Aspire.
*   **Convenções de Código:** Siga as convenções definidas e as práticas padrão do .NET.
*   **Processo de Pull Request:** Todas as alterações devem ser revisadas via Pull Requests.
*   **Manutenção:** Este documento é vivo e será atualizado conforme a arquitetura evolui.

## 10. Contato

Para dúvidas ou discussões arquitetônicas, entre em contato com Renan ou abra uma issue no repositório do projeto.