# PROJECT_PLAN.MD - Plano de Implementação do Sistema GED Axxon

Este documento descreve as etapas sequenciais para a construção do sistema do GED Axxon, seguindo a arquitetura detalhada no `GEMINI.md`. O objetivo é fornecer um roteiro claro e pragmático, focado na entrega incremental de valor e na aplicação dos princípios de Clean Architecture, DDD e uma estratégia de persistência dual.

---

## 1. Visão Geral do Projeto

**Nome do Projeto:** Sistema GED Axxon
**Objetivo Principal:** Desenvolver uma plataforma robusta e escalável para gerenciamento eletrônico de documentos e prontuários, com foco em segurança, conformidade, busca avançada e uma trilha de auditoria imutável.
**Documento de Arquitetura Base:** `GEMINI.md`
**Metodologia:** Abordagem iterativa e incremental (Scrum/Kanban).

## 2. Fases do Projeto

O projeto será dividido nas seguintes fases principais:

*   **Fase 1: Fundação e Estruturação da Solução**
*   **Fase 2: Modelagem e Persistência do Domínio (EF Core)**
*   **Fase 3: Implementação da Trilha de Auditoria (Marten)**
*   **Fase 4: Camada de Aplicação e Lógica de Negócio**
*   **Fase 5: API Core e Armazenamento de Arquivos**
*   **Fase 6: Funcionalidades Avançadas (Busca e Métricas)**
*   **Fase 7: Segurança, Testes e Qualidade**
*   **Fase 8: Implantação e Operações (DevOps)**

---

## Detalhamento das Fases e Etapas

### Fase 1: Fundação e Estruturação da Solução (Sprint 1)

**Objetivo:** Estabelecer o ambiente de desenvolvimento, a estrutura completa do projeto conforme a arquitetura, e as convenções iniciais.

*   **1.1. Alinhamento Arquitetônico (`GEMINI.md`):**
    *   Revisar e validar o documento `GEMINI.md` com a equipe, focando na estratégia de duas bases de dados e no escopo do Event Sourcing para auditoria.
*   **1.2. Setup do Repositório e Ferramentas:**
    *   Criar repositório Git (`AxxonGed`).
    *   Configurar `.gitignore` для .NET e `.editorconfig` para convenções de código.
*   **1.3. Criação da Estrutura da Solução (Via Script `Execution.md`):**
    *   Executar o roteiro detalhado no arquivo `Execution.md` para criar toda a estrutura da solução `Axxon`, incluindo:
        *   Criação de todos os projetos (`SharedKernel`, `Domain`, `Application`, `Infrastructure`, `Presentation.Api`, `AppHost`, `ServiceDefaults`).
        *   Adição dos projetos à solução.
        *   Configuração de todas as referências entre projetos.
        *   Criação da estrutura de pastas internas em cada projeto.
        *   Instalação dos pacotes NuGet essenciais em cada camada.
*   **1.4. Configuração do ASP.NET Aspire:**
    *   Configurar `Axxon.AppHost` para orquestrar dois contêineres PostgreSQL (um para a aplicação, outro para a auditoria) e um contêiner RabbitMQ (para MassTransit).
    *   Validar o startup local com o Aspire Dashboard.

**Entregas da Fase:** Repositório Git configurado, estrutura completa da solução criada e funcional, ambiente de desenvolvimento local com Aspire operacional.

### Fase 2: Modelagem e Persistência do Domínio (EF Core) (Sprints 2-3)

**Objetivo:** Modelar as entidades centrais e configurar a persistência principal com Entity Framework Core.

*   **2.1. Implementação do SharedKernel:**
    *   Criar as classes base `Entity<TId>` e `AggregateRoot<TId>` em `Axxon.SharedKernel/BaseClasses`.
    *   Implementar o padrão `Result` em `Axxon.SharedKernel/Utilities`.
*   **2.2. Modelagem do Domínio:**
    *   Em `Axxon.Domain`, definir os agregados `TipoDeDocumento`, `Documento` e `Prontuario`, e seus respectivos Value Objects (`CampoMetadata`, `RegraValidacao`, etc.).
*   **2.3. Definição das Interfaces de Repositório:**
    *   Em `Axxon.Domain/Interfaces`, definir `IDocumentoRepository`, `ITipoDeDocumentoRepository`, `IProntuarioRepository`.
*   **2.4. Configuração da Base de Dados da Aplicação (EF Core):**
    *   Em `Axxon.Infrastructure/Data`, criar o `AppDbContext` para o EF Core.
    *   Configurar o mapeamento das entidades do domínio no `AppDbContext`.
    *   Gerar e aplicar a migração inicial do banco de dados da aplicação.
*   **2.5. Implementação dos Repositórios (EF Core):**
    *   Em `Axxon.Infrastructure/Data/Repositories`, implementar as interfaces de repositório usando o `AppDbContext`.

**Entregas da Fase:** Modelos de domínio implementados, base de dados da aplicação configurada e acessível via repositórios. Testes de integração para os repositórios EF Core.

### Fase 3: Implementação da Trilha de Auditoria (Marten) (Sprint 4)

**Objetivo:** Configurar a base de dados de auditoria e o mecanismo de gravação de eventos com Marten.

*   **3.1. Definição dos Eventos de Domínio:**
    *   Em `Axxon.SharedKernel/Events`, definir os eventos de auditoria, como `DocumentoCriadoEvent`, `DocumentoAtualizadoEvent`, `DocumentoRemovidoEvent`.
*   **3.2. Configuração da Base de Dados de Auditoria (Marten):**
    *   Em `Axxon.Infrastructure`, configurar a conexão com a segunda base de dados PostgreSQL, dedicada ao Marten.
    *   Configurar o `IDocumentSession` do Marten para ser injetado via DI.
*   **3.3. Criação do Serviço de Auditoria:**
    *   Definir a interface `IAuditLogger` em `Axxon.Application/Interfaces`.
    *   Implementar `MartenAuditLogger` em `Axxon.Infrastructure/Services`, que utilizará o `IDocumentSession` para gravar os eventos de domínio.
*   **3.4. Integração com MassTransit (Opcional inicial):**
    *   Configurar MassTransit para, futuramente, despachar eventos de forma assíncrona para o serviço de auditoria. Inicialmente, a chamada pode ser direta.

**Entregas da Fase:** Segunda base de dados configurada, serviço de auditoria funcional capaz de gravar eventos de forma imutável no Marten. Testes de integração para o `MartenAuditLogger`.

### Fase 4: Camada de Aplicação e Lógica de Negócio (Sprints 5-6)

**Objetivo:** Implementar os casos de uso (Commands/Queries) e a orquestração entre o domínio, a persistência e a auditoria.

*   **4.1. Implementação de Commands e Queries:**
    *   Em `Axxon.Application/Features`, criar os comandos (`CriarDocumentoCommand`) e queries (`GetDocumentoByIdQuery`) para as entidades principais.
*   **4.2. Implementação dos Handlers:**
    *   Implementar os `RequestHandlers` (MediatR) para cada comando e query.
    *   **Exemplo (CriarDocumentoCommandHandler):**
        1.  Validar o comando (FluentValidation).
        2.  Criar a entidade `Documento`.
        3.  Usar o `IDocumentoRepository` (EF Core) para salvar a entidade.
        4.  Criar um `DocumentoCriadoEvent`.
        5.  Chamar o `IAuditLogger` para registrar o evento no Marten.
        6.  Retornar um `Result` de sucesso.
*   **4.3. Definição de DTOs e Mapeamento:**
    *   Criar os DTOs necessários em `Axxon.Application/DTOs`.
    *   Implementar os mapeamentos usando Mapperly.

**Entregas da Fase:** Lógica de aplicação implementada, orquestrando a escrita na base principal e o registro na trilha de auditoria. Testes unitários para os Handlers.

### Fase 5: API Core e Armazenamento de Arquivos (Sprints 7-8)

**Objetivo:** Expor a funcionalidade via API REST e implementar o armazenamento de arquivos físicos.

*   **5.1. Implementação dos Controllers da API:**
    *   Em `Axxon.Presentation.Api/Controllers`, criar os endpoints para as operações de Documentos, Tipos de Documento e Prontuários.
*   **5.2. Implementação do Armazenamento de Arquivos:**
    *   Definir `IFileStorageService` em `Axxon.Application/Interfaces`.
    *   Implementar `LocalFileStorageService` em `Axxon.Infrastructure/FileStorage` para desenvolvimento local.
    *   Integrar o `IFileStorageService` nos handlers de comando apropriados (ex: `CriarDocumentoCommandHandler`).
*   **5.3. Configuração da API:**
    *   Configurar Swagger/OpenAPI.
    *   Implementar middleware global de tratamento de exceções.
    *   Configurar Serilog para logging estruturado.

**Entregas da Fase:** API REST funcional para upload, download e gerenciamento de documentos. Armazenamento de arquivos em disco local funcional. Testes de integração para os endpoints da API.

### Fase 6: Funcionalidades Avançadas (Busca e Métricas) (Sprints 9-11)

**Objetivo:** Implementar busca avançada e o serviço de métricas de armazenamento.

*   **6.1. Consulta Avançada (Elasticsearch/Azure Cognitive Search):**
    *   Definir `ISearchService` na `Application`.
    *   Implementar o serviço concreto na `Infrastructure`.
    *   Criar consumidores de eventos (via MassTransit) que escutam os eventos de auditoria do Marten para indexar os dados no motor de busca.
    *   Implementar a query de busca avançada na `Application` e o endpoint na `API`.
*   **6.2. Métricas de Armazenamento:**
    *   Definir `IStorageMetricsService` na `Application`.
    *   Implementar o serviço na `Infrastructure`, usando uma tabela na base de dados da aplicação (EF Core).
    *   Criar consumidores de eventos que atualizam as métricas com base nos eventos de criação e remoção de documentos.
    *   Implementar a query e o endpoint para consultar as métricas.

**Entregas da Fase:** Funcionalidades de busca avançada e consulta de métricas de armazenamento implementadas e testadas.

### Fase 7: Segurança, Testes e Qualidade (Sprints 12-13)

**Objetivo:** Fortalecer a segurança, aumentar a cobertura de testes e garantir a qualidade geral da aplicação.

*   **7.1. Segurança (Keycloak):**
    *   Integrar a API com Keycloak para autenticação (JWT) e autorização (RBAC).
    *   Proteger todos os endpoints com as políticas de segurança adequadas.
*   **7.2. Expansão dos Testes:**
    *   Aumentar a cobertura de testes unitários e de integração.
    *   Desenvolver testes End-to-End para os fluxos de usuário mais críticos.
*   **7.3. Qualidade de Código e Performance:**
    *   Realizar análises de performance e otimizar gargalos.
    *   Garantir conformidade com as convenções de código e análise estática (SonarAnalyzer).

**Entregas da Fase:** Aplicação segura, com alta cobertura de testes e performance otimizada.

### Fase 8: Implantação e Operações (DevOps) (Sprints 14-15)

**Objetivo:** Automatizar a implantação e configurar o monitoramento para o ambiente de produção.

*   **8.1. Contêinerização:**
    *   Criar e otimizar `Dockerfile` para a API.
*   **8.2. Pipeline CI/CD (GitHub Actions):**
    *   Configurar um pipeline completo para build, teste, análise de código e deploy automatizado.
*   **8.3. Configuração de Ambiente de Produção:**
    *   Provisionar a infraestrutura na nuvem (duas instâncias de PostgreSQL, serviço de mensageria, motor de busca, etc.).
*   **8.4. Monitoramento e Observabilidade:**
    *   Configurar agregação de logs (Seq/ELK), métricas (Prometheus/Grafana) e tracing (OpenTelemetry) em produção.
    *   Definir alertas para falhas críticas.

**Entregas da Fase:** Aplicação implantada em produção com pipelines automatizados e monitoramento ativo.

---

## 4. Próximos Passos Imediatos

1.  **Reunião de Kick-off:** Apresentar este plano à equipe.
2.  **Executar a Fase 1:** Iniciar a estruturação do projeto usando o `Execution.md`.
