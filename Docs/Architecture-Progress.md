# Documentação da Arquitetura e Progresso do Sistema GED/Prontuários

Este documento detalha a arquitetura do sistema GED/Prontuários e o progresso alcançado até o momento, cobrindo as Fases 1, 2 e 3 do plano de execução.

## 1. Visão Geral da Arquitetura

O sistema é construído sobre os princípios da **Clean Architecture (Arquitetura Limpa)**, visando criar uma aplicação robusta, escalável, manutenível e testável. Os princípios fundamentais incluem Separação de Preocupações, Baixo Acoplamento, Alta Coesão, Testabilidade, Escalabilidade, Manutenibilidade, Extensibilidade, Performance, Segurança e os princípios SOLID. O **Padrão Result** é utilizado para tratamento explícito de falhas previsíveis.

A estrutura de camadas segue o modelo concêntrico da Clean Architecture:
*   **Shared Kernel:** Tipos e utilitários comuns (ex: `Result`, `HashUtility`).
*   **Domain:** O coração da aplicação, contendo entidades, Value Objects, interfaces de repositório e regras de negócio.
*   **Application:** Casos de uso (Commands e Queries), DTOs e interfaces para serviços externos.
*   **Infrastructure:** Implementações concretas das interfaces de Domain e Application (banco de dados, armazenamento de arquivos).
*   **Presentation (API/UI):** Ponto de entrada da aplicação (API REST).

O projeto utiliza **.NET 9.0** e **ASP.NET Aspire** para orquestração e telemetria, **MediatR** para CQRS leve, e **FluentValidation** para validação.

## 2. Progresso por Fase

### Fase 1: Configuração e Fundação (Concluída)

Esta fase estabeleceu a base do projeto, garantindo que a estrutura e as ferramentas essenciais estivessem prontas.

**Principais Entregas:**
*   **Estrutura de Projetos:** Criação e configuração de todos os projetos da solução (`ProdAbs.SharedKernel`, `ProdAbs.Domain`, `ProdAbs.Application`, `ProdAbs.Infrastructure`, `ProdAbs.Presentation.Api`, `ProdAbs.CrossCutting`, `ProdAbs.AppHost`, `ProdAbs.ServiceDefaults`, `ProdAbs.UnitTests`, `ProdAbs.IntegrationTests`).
*   **Alinhamento de TargetFramework:** Todos os projetos foram configurados para `net9.0` para garantir compatibilidade.
*   **Referências de Projeto:** As dependências entre as camadas foram estabelecidas corretamente.
*   **Pacotes NuGet Essenciais:** Adição de pacotes como `MediatR`, `Microsoft.EntityFrameworkCore.Sqlite`, `Serilog`, `Swashbuckle.AspNetCore`, `Microsoft.AspNetCore.Authentication.JwtBearer`, `Moq`, `FluentValidation`.
*   **Utilitários Core:** Implementação das classes `Result` (para tratamento de erros) e `HashUtility` (para cálculo de hash de arquivos) no `ProdAbs.SharedKernel`.
*   **Entidades Iniciais:** Criação de versões simplificadas das entidades `TipoDocumento`, `Documento` e `Prontuario` no `ProdAbs.Domain`.
*   **Serviços de Infraestrutura Básicos:** Definição da interface `IFileStorageService` no `Application` e sua implementação `LocalFileStorageService` no `Infrastructure`, além do `AppDbContext` para acesso a dados via SQLite.
*   **Configuração de DI e API:** Atualização do `DependencyInjection.cs` no `CrossCutting` para registrar serviços e configuração inicial do `Program.cs` da API com Swagger e autenticação JWT básica.
*   **Controle de Versão:** Configuração do `.gitignore` e inicialização do repositório Git, com o primeiro commit contendo toda a estrutura base.

### Fase 2: Modelagem do Domínio e Abstrações (Concluída)

Esta fase aprofundou a modelagem do domínio, adicionando detalhes e abstrações cruciais.

**Principais Entregas:**
*   **Value Objects (VOs):** Criação de `RegraValidacao`, `CampoMetadata` e `MetadadoDocumento` no `ProdAbs.Domain.ValueObjects`.
*   **Entidades Detalhadas:** A entidade `TipoDocumento` foi atualizada para incluir uma lista de `CampoMetadata`, refletindo a estrutura de campos de um tipo de documento.
*   **Interfaces de Repositório:** Definição das interfaces `ITipoDeDocumentoRepository`, `IDocumentoRepository` e `IProntuarioRepository` no `ProdAbs.Domain.Interfaces`.
*   **Eventos de Domínio:** Criação de `DocumentoCriadoEvent` e `DocumentoRemovidoEvent` no `ProdAbs.SharedKernel.Events`.
*   **Validação com FluentValidation:** Integração do FluentValidation e criação de validadores para os Value Objects (`CampoMetadataValidator`, `MetadadoDocumentoValidator`) no `ProdAbs.Application.Validators`.

### Fase 3: Implementação da Camada de Aplicação (Core) (Concluída)

Esta fase focou na lógica de aplicação, orquestrando as operações do sistema.

**Principais Entregas:**
*   **DTOs:** Criação de `TipoDocumentoDetalhesDTO`, `DocumentoDTO` e `ProntuarioResumoDTO` no `ProdAbs.Application.DTOs`.
*   **Interfaces de Serviços de Aplicação:** Definição de `IEmailNotifier` e `IAuditLogger` no `ProdAbs.Application.Interfaces`.
*   **Commands e Queries:** Criação de todos os Commands (para operações de escrita) e Queries (para operações de leitura) para as funcionalidades de MVP de Tipos de Documento, Documentos e Prontuários.
*   **Handlers:** Implementação de todos os Handlers correspondentes aos Commands e Queries, contendo a lógica de aplicação que interage com o domínio e os serviços de infraestrutura.
    *   **Observação:** A implementação dos Controllers (Camada de Apresentação) foi realizada nesta fase para permitir a compilação completa do projeto, embora sua implementação detalhada e refinamento estejam previstos para a Fase 5 do plano original.
*   **Implementações Placeholder de Repositórios:** Para permitir a compilação da Fase 3, foram criadas implementações placeholder para `TipoDeDocumentoRepository`, `DocumentoRepository` e `ProntuarioRepository` no `ProdAbs.Infrastructure.Data.Repositories`. Essas implementações básicas utilizam o `AppDbContext` e serão substituídas por implementações mais robustas (com Marten e EF Core para CQRS) na Fase 4.

## 3. Gerenciamento Centralizado de Pacotes (CPM)

O projeto foi configurado para utilizar o **Gerenciamento Centralizado de Pacotes (CPM)** do .NET. Um arquivo `Directory.Packages.props` foi criado na raiz do repositório, contendo as versões de todos os pacotes NuGet utilizados na solução. Isso garante consistência nas versões dos pacotes em todos os projetos e simplifica futuras atualizações.

## 4. Próximos Passos

Com as Fases 1, 2 e 3 concluídas, o sistema possui uma base arquitetural sólida e a lógica de aplicação central implementada. O próximo passo é a **Fase 4: Integrações de Infraestrutura Essenciais**, que focará na implementação robusta dos repositórios e na configuração de mensageria e Event Sourcing.
