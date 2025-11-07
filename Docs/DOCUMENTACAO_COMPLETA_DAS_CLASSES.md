# Documentação Completa das Classes - ProdAbs

Este documento fornece uma documentação detalhada de todas as classes do sistema ProdAbs (Sistema de Gerenciamento Eletrônico de Documentos e Prontuários), organizada seguindo a arquitetura Clean Architecture.

---

## Índice

1. [Camada SharedKernel](#1-camada-sharedkernel)
2. [Camada Domain](#2-camada-domain)
3. [Camada Application](#3-camada-application)
4. [Camada Infrastructure](#4-camada-infrastructure)
5. [Camada Presentation](#5-camada-presentation)
6. [AppHost e ServiceDefaults](#6-apphost-e-servicedefaults)

---

## 1. Camada SharedKernel

A camada SharedKernel contém tipos de dados e interfaces comuns que são utilizados amplamente em todo o projeto.

### 1.1. BaseClasses

#### `Entity<TId>`
**Namespace:** `ProdAbs.SharedKernel.BaseClasses`

**Propósito:** Classe base abstrata para todas as entidades do domínio.

**Responsabilidades:**
- Fornece implementação padrão de identidade para entidades
- Garante que entidades sejam comparadas por seu `Id` único
- Implementa `IEquatable<Entity<TId>>` para comparação de identidade

**Propriedades:**
- `TId Id`: Identificador único da entidade (protected set)

**Métodos Principais:**
- `bool Equals(Entity<TId>? other)`: Compara duas entidades pelo ID
- `override int GetHashCode()`: Retorna o hash code baseado no ID
- `operator ==` e `operator !=`: Operadores de igualdade/desigualdade

**Características:**
- Classe abstrata e genérica
- Tipo genérico `TId` deve ser não-nulo

---

#### `AggregateRoot<TId>`
**Namespace:** `ProdAbs.SharedKernel.BaseClasses`

**Propósito:** Classe base que marca e serve como raiz de um agregado no DDD.

**Responsabilidades:**
- Identifica a raiz de um agregado
- Herda funcionalidade de identidade da classe `Entity<TId>`
- Mantém consistência transacional dentro dos limites do agregado

**Herança:** Herda de `Entity<TId>`

**Construtor:**
- `protected AggregateRoot(TId id)`: Inicializa com um identificador único

---

### 1.2. Result Pattern

#### `Result`
**Namespace:** `ProdAbs.SharedKernel`

**Propósito:** Classe que encapsula o resultado de uma operação, indicando sucesso ou falha.

**Responsabilidades:**
- Fornecer tratamento explícito de erros esperados
- Separar falhas previsíveis de exceções inesperadas
- Evitar uso excessivo de exceções para controle de fluxo

**Propriedades:**
- `bool IsSuccess`: Indica se a operação foi bem-sucedida
- `bool IsFailure`: Retorna `!IsSuccess`
- `string Error`: Mensagem de erro (vazia se sucesso)

**Métodos Estáticos:**
- `static Result Ok()`: Cria um resultado de sucesso
- `static Result Fail(string message)`: Cria um resultado de falha
- `static Result<T> Ok<T>(T value)`: Cria um resultado genérico de sucesso
- `static Result<T> Fail<T>(string message)`: Cria um resultado genérico de falha

**Validações:**
- Não permite sucesso com mensagem de erro
- Não permite falha sem mensagem de erro

---

#### `Result<T>`
**Namespace:** `ProdAbs.SharedKernel`

**Propósito:** Versão genérica do Result que encapsula um valor de retorno.

**Responsabilidades:**
- Encapsular valor de retorno junto com indicação de sucesso/falha
- Fornecer acesso seguro ao valor apenas em caso de sucesso

**Herança:** Herda de `Result`

**Propriedades:**
- `T Value`: Valor encapsulado (somente leitura, lança exceção se acessado em falha)

**Características:**
- O valor só pode ser acessado se `IsSuccess` for verdadeiro
- Lança `InvalidOperationException` ao tentar acessar valor em caso de falha

---

### 1.3. Utilities

#### `HashUtility`
**Namespace:** `ProdAbs.SharedKernel`

**Propósito:** Classe utilitária para cálculo de hashes criptográficos.

**Responsabilidades:**
- Calcular hash SHA-256 de streams de dados
- Garantir integridade de documentos

**Métodos:**
- `static async Task<string> CalculateSha256Async(Stream stream)`: Calcula o hash SHA-256 de um stream e retorna como string hexadecimal

**Características:**
- Classe estática
- Operação assíncrona
- Retorna hash em formato hexadecimal

---

#### `Uuid`
**Namespace:** `System` (ProdAbs.SharedKernel.System)

**Propósito:** Representa um identificador universalmente único (UUID) compatível com Java.

**Responsabilidades:**
- Fornecer compatibilidade com UUIDs do padrão Java
- Converter entre `Uuid` e `Guid` do .NET
- Manter representação de 128 bits

**Propriedades:**
- `long LeastSignificantBits`: 64 bits menos significativos
- `long MostSignificantBits`: 64 bits mais significativos

**Métodos Principais:**
- `override string ToString()`: Representação em string do UUID
- `static Uuid FromString(string input)`: Cria UUID a partir de string
- `explicit operator Guid(Uuid uuid)`: Converte Uuid para Guid
- `implicit operator Uuid(Guid value)`: Converte Guid para Uuid
- `bool Equals(Uuid uuid)`: Compara dois UUIDs

**Características:**
- Struct imutável
- Implementa `IEquatable<Uuid>`
- Conversão bidirecional com `Guid`

---

### 1.4. Interfaces

#### `IHasCreatedDate`
**Namespace:** `ProdAbs.SharedKernel.Interfaces`

**Propósito:** Interface marcadora para entidades que possuem data de criação.

**Propriedade:**
- `DateTimeOffset CreatedDate { get; }`: Data e hora de criação

**Uso:** Aplicada em entidades como `Documento` e `Prontuario`

---

### 1.5. Events (Eventos de Domínio)

#### `IDocumentEvent`
**Namespace:** `ProdAbs.SharedKernel.Events`

**Propósito:** Interface base para todos os eventos relacionados a documentos.

**Propriedade:**
- `Guid Id { get; set; }`: Identificador do documento

---

#### `IDocumentoCriadoEvent`
**Namespace:** `ProdAbs.SharedKernel.Events`

**Propósito:** Interface para eventos de criação de documento.

**Herança:** Herda de `IDocumentEvent`

**Propriedades:**
- `Guid TipoDocumentoId { get; set; }`: ID do tipo de documento
- `string StorageLocation { get; set; }`: Localização do arquivo armazenado
- `long TamanhoEmBytes { get; set; }`: Tamanho do arquivo

---

#### `DocumentoCriadoEvent`
**Namespace:** `ProdAbs.SharedKernel.Events`

**Propósito:** Implementação concreta do evento de documento criado.

**Implementa:** `IDocumentoCriadoEvent`

**Propriedades:**
- `Guid Id`: Identificador do documento
- `Guid TipoDocumentoId`: ID do tipo de documento
- `string StorageLocation`: Localização do armazenamento
- `long TamanhoEmBytes`: Tamanho em bytes
- `string HashValor`: Hash do documento para verificação de integridade

**Uso:** Disparado quando um novo documento é criado no sistema

---

#### `DocumentoRemovidoEvent`
**Namespace:** `ProdAbs.SharedKernel.Events`

**Propósito:** Evento disparado quando um documento é removido.

**Implementa:** `IDocumentEvent`

**Propriedade:**
- `Guid Id`: Identificador do documento removido

---

## 2. Camada Domain

A camada Domain contém as entidades, value objects, agregados e interfaces de repositório que representam o coração da lógica de negócio.

### 2.1. Entities (Entidades)

#### `Documento`
**Namespace:** `ProdAbs.Domain.Entities`

**Propósito:** Entidade principal que representa um documento único no sistema.

**Herança:** `AggregateRoot<Guid>`, implementa `IHasCreatedDate`

**Responsabilidades:**
- Representar um documento com seus metadados
- Armazenar referência ao tipo de documento
- Manter hash para verificação de integridade
- Controlar versionamento do documento

**Propriedades:**
- `Guid TipoDeDocumentoId`: Referência ao tipo de documento
- `string StorageLocation`: Localização do arquivo armazenado
- `long TamanhoEmBytes`: Tamanho do arquivo
- `string HashTipo`: Tipo de hash utilizado (ex: "SHA256")
- `string HashValor`: Valor do hash calculado
- `string NomeArquivoOriginal`: Nome original do arquivo
- `string Formato`: Tipo MIME ou formato do arquivo
- `int Versao`: Número da versão do documento
- `IReadOnlyDictionary<string, string> DicionarioDeCamposValores`: Dicionário com campos personalizados e seus valores
- `DateTimeOffset CreatedDate`: Data de criação

**Construtores:**
- Construtor privado para EF Core
- Construtor público completo para criação de documentos

**Características:**
- Aggregate Root do contexto de documentos
- Propriedades com setters privados (imutabilidade)
- Não armazena o arquivo binário, apenas referência

---

#### `TipoDocumento`
**Namespace:** `ProdAbs.Domain.Entities`

**Propósito:** Define a estrutura e metadados de um tipo específico de documento.

**Herança:** `AggregateRoot<Guid>`

**Responsabilidades:**
- Definir estrutura de campos personalizados para documentos
- Especificar regras de validação
- Estabelecer máscaras de entrada

**Propriedades:**
- `string Nome`: Nome do tipo de documento
- `List<CampoMetadata> Campos`: Lista de campos com suas configurações

**Construtores:**
- Construtor privado para EF Core
- Construtor público com nome e campos

**Características:**
- Aggregate Root
- Permite configuração flexível de campos personalizados
- Usado como template para criar documentos

---

#### `Prontuario`
**Namespace:** `ProdAbs.Domain.Entities`

**Propósito:** Container lógico para agrupar múltiplos documentos relacionados.

**Herança:** `AggregateRoot<Guid>`, implementa `IHasCreatedDate`

**Responsabilidades:**
- Agrupar documentos relacionados a uma entidade (pessoa, empresa, projeto)
- Manter lista de referências de documentos
- Classificar tipo de prontuário (B2B, B2P, etc.)

**Propriedades:**
- `string IdentificadorEntidade`: Identificador da entidade (CPF, CNPJ, etc.)
- `string TipoProntuario`: Tipo do prontuário (ex: "B2P", "B2B")
- `List<Guid> DocumentoIds`: Lista de IDs dos documentos associados
- `DateTimeOffset CreatedDate`: Data de criação

**Métodos:**
- `void AdicionarDocumento(Guid documentoId)`: Adiciona documento ao prontuário (evita duplicatas)

**Construtores:**
- Construtor privado para EF Core
- Construtor público completo

**Características:**
- Aggregate Root
- Gerencia coleção de documentos
- Suporte a diferentes tipos de prontuários

---

### 2.2. Value Objects

#### `CampoMetadata`
**Namespace:** `ProdAbs.Domain.ValueObjects`

**Propósito:** Define metadados de um campo personalizado em um tipo de documento.

**Responsabilidades:**
- Especificar label do campo
- Definir regras de validação
- Estabelecer máscara de entrada

**Propriedades:**
- `string Label`: Rótulo/nome do campo
- `RegraValidacao RegraDeValidacao`: Regras de validação aplicáveis
- `string Mascara`: Máscara de formatação (ex: "###.###.###-##")

**Construtores:**
- Construtor privado para EF Core
- Construtor público completo

**Características:**
- Value Object (comparado por valor)
- Propriedades imutáveis (setters privados)

---

#### `RegraValidacao`
**Namespace:** `ProdAbs.Domain.ValueObjects`

**Propósito:** Define regras de validação para campos personalizados.

**Responsabilidades:**
- Especificar tipo de dados esperado
- Indicar se o campo é obrigatório
- Definir formatos específicos (regex)

**Propriedades:**
- `TipoDeDados TipoDeDados`: Tipo de dados (String, Int, Date)
- `bool Obrigatorio`: Indica se o campo é obrigatório
- `string FormatoEspecifico`: Formato específico (ex: expressão regular)

**Construtores:**
- Construtor privado para EF Core
- Construtor público completo

**Características:**
- Value Object
- Propriedades imutáveis

---

#### `TipoDeDados` (Enum)
**Namespace:** `ProdAbs.Domain.ValueObjects`

**Propósito:** Enumera os tipos de dados suportados para campos personalizados.

**Valores:**
- `String`: Campo de texto
- `Int`: Campo numérico inteiro
- `Date`: Campo de data

---

#### `MetadadoDocumento`
**Namespace:** `ProdAbs.Domain.ValueObjects`

**Propósito:** Encapsula metadados técnicos de um documento.

**Propriedades:**
- `long TamanhoEmBytes`: Tamanho do arquivo
- `string HashTipo`: Tipo de hash
- `string HashValor`: Valor do hash
- `string NomeArquivoOriginal`: Nome original
- `string Formato`: Formato/tipo MIME
- `int Versao`: Versão do documento

**Características:**
- DTO simples com propriedades públicas
- Usado para transferência de metadados

---

### 2.3. Interfaces de Repositório

#### `IDocumentoRepository`
**Namespace:** `ProdAbs.Domain.Interfaces`

**Propósito:** Define contrato para persistência de documentos.

**Métodos:**
- `Task<Documento> GetByIdAsync(Guid id)`: Recupera documento por ID
- `Task AddAsync(Documento documento)`: Adiciona novo documento

**Características:**
- Interface definida na camada Domain
- Implementada na camada Infrastructure
- Princípio DIP (Dependency Inversion Principle)

---

#### `ITipoDeDocumentoRepository`
**Namespace:** `ProdAbs.Domain.Interfaces`

**Propósito:** Define contrato para persistência de tipos de documento.

**Métodos:**
- `Task<TipoDocumento> GetByIdAsync(Guid id)`: Recupera tipo por ID
- `Task AddAsync(TipoDocumento tipoDocumento)`: Adiciona novo tipo
- `Task<List<TipoDocumento>> GetAllAsync()`: Lista todos os tipos

---

#### `IProntuarioRepository`
**Namespace:** `ProdAbs.Domain.Interfaces`

**Propósito:** Define contrato para persistência de prontuários.

**Métodos:**
- `Task<Prontuario> GetByIdAsync(Guid id)`: Recupera prontuário por ID
- `Task AddAsync(Prontuario prontuario)`: Adiciona novo prontuário
- `Task UpdateAsync(Prontuario prontuario)`: Atualiza prontuário existente

---

## 3. Camada Application

A camada Application contém casos de uso (commands e queries), handlers, DTOs, validações e interfaces para serviços externos.

### 3.1. Commands (Comandos de Escrita)

#### `CriarDocumentoCommand`
**Namespace:** `ProdAbs.Application.Features.Documentos.Commands`

**Propósito:** Comando para criar um novo documento no sistema.

**Implementa:** `IRequest<Result<Guid>>` (MediatR)

**Propriedades:**
- `IFormFile File`: Arquivo a ser enviado
- `Guid TipoDocumentoId`: ID do tipo de documento

**Características:**
- Retorna `Result<Guid>` com ID do documento criado
- Usado com MediatR para CQRS

---

#### `CriarTipoDocumentoCommand`
**Namespace:** `ProdAbs.Application.Features.TiposDocumento.Commands`

**Propósito:** Comando para criar um novo tipo de documento.

**Implementa:** `IRequest<Result<Guid>>`

**Propriedades:**
- `string Nome`: Nome do tipo
- `List<CampoMetadata> Campos`: Lista de campos do tipo

**Validação:** `CriarTipoDocumentoCommandValidator`

---

#### `CriarProntuarioCommand`
**Namespace:** `ProdAbs.Application.Features.Prontuarios.Commands`

**Propósito:** Comando para criar um novo prontuário.

**Implementa:** `IRequest<Result<Guid>>`

**Propriedades:**
- `string IdentificadorEntidade`: CPF, CNPJ, etc.
- `string TipoProntuario`: Tipo do prontuário

---

#### `AdicionarDocumentoAoProntuarioCommand`
**Namespace:** `ProdAbs.Application.Features.Prontuarios.Commands`

**Propósito:** Comando para adicionar documento a um prontuário.

**Implementa:** `IRequest<Result>`

**Propriedades:**
- `Guid ProntuarioId`: ID do prontuário
- `Guid DocumentoId`: ID do documento a adicionar

---

### 3.2. Queries (Consultas de Leitura)

#### `GetDocumentoByIdQuery`
**Namespace:** `ProdAbs.Application.Features.Documentos.Queries`

**Propósito:** Query para recuperar documento por ID.

**Implementa:** `IRequest<Result<DocumentoDTO>>`

**Propriedades:**
- `Guid Id`: ID do documento

---

#### `DownloadDocumentoQuery`
**Namespace:** `ProdAbs.Application.Features.Documentos.Queries`

**Propósito:** Query para fazer download de um documento.

**Implementa:** `IRequest<Result<FileDownloadDTO>>`

**Propriedades:**
- `Guid Id`: ID do documento

---

#### `ListarTiposDocumentoQuery`
**Namespace:** `ProdAbs.Application.Features.TiposDocumento.Queries`

**Propósito:** Query para listar todos os tipos de documento.

**Implementa:** `IRequest<Result<List<TipoDocumentoDetalhesDTO>>>`

---

#### `GetDocumentosDoProntuarioQuery`
**Namespace:** `ProdAbs.Application.Features.Prontuarios.Queries`

**Propósito:** Query para recuperar documentos de um prontuário.

**Implementa:** `IRequest<Result<ProntuarioResumoDTO>>`

**Propriedades:**
- `Guid ProntuarioId`: ID do prontuário

---

### 3.3. Handlers (Manipuladores)

#### `CriarDocumentoCommandHandler`
**Namespace:** `ProdAbs.Application.Features.Documentos.Handlers`

**Propósito:** Manipula o comando de criação de documento.

**Implementa:** `IRequestHandler<CriarDocumentoCommand, Result<Guid>>`

**Dependências:**
- `IDocumentoRepository`: Para persistir documento
- `IFileStorageService`: Para armazenar arquivo
- `ITopicProducer<Guid, IDocumentoCriadoEvent>`: Para publicar evento
- `TimeProvider`: Para obter timestamp

**Responsabilidades:**
1. Calcular hash do arquivo
2. Fazer upload do arquivo via `IFileStorageService`
3. Criar entidade `Documento` com metadados
4. Persistir no repositório
5. Publicar evento `DocumentoCriadoEvent` via MassTransit

**Fluxo:**
```
1. Abrir stream do arquivo
2. Calcular hash SHA-256
3. Fazer upload do arquivo
4. Criar entidade Documento
5. Persistir no banco
6. Publicar evento
7. Retornar Result<Guid>
```

---

#### `GetDocumentoByIdQueryHandler`
**Namespace:** `ProdAbs.Application.Features.Documentos.Handlers`

**Propósito:** Recupera documento por ID e retorna DTO.

**Implementa:** `IRequestHandler<GetDocumentoByIdQuery, Result<DocumentoDTO>>`

**Dependências:**
- `IDocumentoRepository`

**Responsabilidades:**
- Buscar documento no repositório
- Mapear para DTO
- Retornar Result

---

#### `DownloadDocumentoQueryHandler`
**Namespace:** `ProdAbs.Application.Features.Documentos.Handlers`

**Propósito:** Recupera o arquivo físico de um documento para download.

**Implementa:** `IRequestHandler<DownloadDocumentoQuery, Result<FileDownloadDTO>>`

**Dependências:**
- `IDocumentoRepository`
- `IFileStorageService`

**Responsabilidades:**
- Buscar documento no repositório
- Recuperar arquivo do storage
- Retornar stream e metadados

---

#### `CriarTipoDocumentoCommandHandler`
**Namespace:** `ProdAbs.Application.Features.TiposDocumento.Handlers`

**Propósito:** Cria novo tipo de documento.

**Implementa:** `IRequestHandler<CriarTipoDocumentoCommand, Result<Guid>>`

**Dependências:**
- `ITipoDeDocumentoRepository`

---

#### `ListarTiposDocumentoQueryHandler`
**Namespace:** `ProdAbs.Application.Features.TiposDocumento.Handlers`

**Propósito:** Lista todos os tipos de documento cadastrados.

**Implementa:** `IRequestHandler<ListarTiposDocumentoQuery, Result<List<TipoDocumentoDetalhesDTO>>>`

---

#### `CriarProntuarioCommandHandler`
**Namespace:** `ProdAbs.Application.Features.Prontuarios.Handlers`

**Propósito:** Cria novo prontuário.

**Implementa:** `IRequestHandler<CriarProntuarioCommand, Result<Guid>>`

**Dependências:**
- `IProntuarioRepository`
- `TimeProvider`

---

#### `AdicionarDocumentoAoProntuarioCommandHandler`
**Namespace:** `ProdAbs.Application.Features.Prontuarios.Handlers`

**Propósito:** Adiciona documento a um prontuário existente.

**Implementa:** `IRequestHandler<AdicionarDocumentoAoProntuarioCommand, Result>`

**Dependências:**
- `IProntuarioRepository`
- `IDocumentoRepository`

**Responsabilidades:**
- Validar existência do prontuário
- Validar existência do documento
- Adicionar documento ao prontuário
- Persistir alteração

---

#### `GetDocumentosDoProntuarioQueryHandler`
**Namespace:** `ProdAbs.Application.Features.Prontuarios.Handlers`

**Propósito:** Recupera todos os documentos de um prontuário.

**Implementa:** `IRequestHandler<GetDocumentosDoProntuarioQuery, Result<ProntuarioResumoDTO>>`

**Dependências:**
- `IProntuarioRepository`
- `IDocumentoRepository`

---

### 3.4. DTOs (Data Transfer Objects)

#### `DocumentoDTO`
**Namespace:** `ProdAbs.Application.DTOs`

**Propósito:** Representa dados de um documento para transferência.

**Propriedades:**
- `Guid Id`
- `Guid TipoDeDocumentoId`
- `string NomeArquivoOriginal`
- `string Formato`
- `long TamanhoEmBytes`
- `string HashValor`
- `int Versao`
- `IReadOnlyDictionary<string, string> DicionarioDeCamposValores`

---

#### `FileDownloadDTO`
**Namespace:** `ProdAbs.Application.DTOs`

**Propósito:** Encapsula dados para download de arquivo.

**Propriedades:**
- `Stream File`: Stream do arquivo
- `string FileName`: Nome do arquivo
- `string ContentType`: Tipo MIME

---

#### `TipoDocumentoDetalhesDTO`
**Namespace:** `ProdAbs.Application.DTOs`

**Propósito:** Representa detalhes de um tipo de documento.

**Propriedades:**
- `Guid Id`
- `string Nome`
- `List<CampoMetadata> Campos`

---

#### `ProntuarioResumoDTO`
**Namespace:** `ProdAbs.Application.DTOs`

**Propósito:** Representa resumo de um prontuário com seus documentos.

**Propriedades:**
- `Guid Id`
- `string IdentificadorEntidade`
- `string TipoProntuario`
- `List<DocumentoDTO> Documentos`

---

### 3.5. Behaviors (Comportamentos do Pipeline)

#### `ValidationBehavior<TRequest, TResponse>`
**Namespace:** `ProdAbs.Application.Behaviors`

**Propósito:** Behavior do MediatR que executa validação automática de comandos/queries.

**Implementa:** `IPipelineBehavior<TRequest, TResponse>`

**Dependências:**
- `IEnumerable<IValidator<TRequest>>`: Injeção de todos os validadores FluentValidation

**Responsabilidades:**
- Interceptar requisições antes de chegarem ao handler
- Executar todos os validadores registrados
- Agregar erros de validação
- Lançar `ValidationException` se houver falhas

**Fluxo:**
1. Receber requisição
2. Se não houver validadores, continuar pipeline
3. Executar todos os validadores em paralelo
4. Coletar todas as falhas de validação
5. Se houver falhas, lançar ValidationException
6. Caso contrário, continuar para o próximo step do pipeline

---

### 3.6. Validators (Validadores)

#### `CriarTipoDocumentoCommandValidator`
**Namespace:** `ProdAbs.Application.Validators`

**Propósito:** Valida comando de criação de tipo de documento.

**Herança:** `AbstractValidator<CriarTipoDocumentoCommand>`

**Regras:**
- `Nome`: Não vazio, mínimo 3 caracteres
- `Campos`: Lista não vazia

---

#### `CampoMetadataValidator`
**Namespace:** `ProdAbs.Application.Validators`

**Propósito:** Valida metadados de campos personalizados.

**Herança:** `AbstractValidator<CampoMetadata>`

---

#### `MetadadoDocumentoValidator`
**Namespace:** `ProdAbs.Application.Validators`

**Propósito:** Valida metadados de documentos.

**Herança:** `AbstractValidator<MetadadoDocumento>`

---

### 3.7. Interfaces de Serviços Externos

#### `IFileStorageService`
**Namespace:** `ProdAbs.Application.Interfaces`

**Propósito:** Interface para serviços de armazenamento de arquivos.

**Métodos:**
- `Task<Result<string>> UploadAsync(Stream fileStream, string fileName, string contentType)`: Faz upload e retorna localização
- `Task<Result<Stream>> GetAsync(string storageLocation)`: Recupera arquivo
- `Task<Result> DeleteAsync(string storageLocation)`: Remove arquivo

**Propriedade:**
- `string StorageName { get; }`: Nome identificador do storage

**Implementações:**
- `LocalFileStorageService`
- `S3FileStorageService`
- `AzureBlobStorageService`

---

#### `IAuditLogger`
**Namespace:** `ProdAbs.Application.Interfaces`

**Propósito:** Interface para serviço de auditoria.

**Métodos:**
- `Task LogAuditAsync(string action, string entityId, string userId, string details)`: Registra evento de auditoria

---

#### `IEmailNotifier`
**Namespace:** `ProdAbs.Application.Interfaces`

**Propósito:** Interface para serviço de notificação por email.

**Métodos:**
- `Task SendEmailAsync(string to, string subject, string body)`: Envia email

---

### 3.8. Dependency Injection

#### `DependencyInjection`
**Namespace:** `ProdAbs.Application`

**Propósito:** Classe estática para registro de dependências da camada Application.

**Método:**
- `static IServiceCollection AddApplicationServices(this IServiceCollection services)`: Registra MediatR, validadores e behaviors

---

## 4. Camada Infrastructure

A camada Infrastructure contém implementações concretas de interfaces, acesso a dados, serviços externos e mensageria.

### 4.1. Data / Persistence

#### `AppDbContext`
**Namespace:** `ProdAbs.Infrastructure.Data`

**Propósito:** Contexto do Entity Framework Core para acesso ao banco de dados PostgreSQL.

**Herança:** `DbContext`

**DbSets:**
- `DbSet<TipoDocumento> TiposDeDocumento`
- `DbSet<Documento> Documentos`
- `DbSet<Prontuario> Prontuarios`

**Configurações (OnModelCreating):**
- Conversão JSON para `DicionarioDeCamposValores` em `Documento`
- Owned Entity para `Campos` em `TipoDocumento`
- Owned Entity para `RegraDeValidacao` dentro de `CampoMetadata`

**Características:**
- Usa PostgreSQL como banco de dados
- Configuração via Fluent API
- Suporte a tipos complexos via JSON

---

#### `DbInitializer`
**Namespace:** `ProdAbs.Infrastructure.Data`

**Propósito:** Inicializador do banco de dados (seed data, migrations).

**Responsabilidades:**
- Aplicar migrations pendentes
- Inserir dados iniciais se necessário

---

#### `DesignTimeDbContextFactory`
**Namespace:** `ProdAbs.Infrastructure.Data`

**Propósito:** Factory para criação do DbContext em tempo de design (EF Core Tools).

**Implementa:** `IDesignTimeDbContextFactory<AppDbContext>`

**Uso:** Utilizado por ferramentas EF Core (migrations, etc.)

---

### 4.2. Repositories

#### `DocumentoRepository`
**Namespace:** `ProdAbs.Infrastructure.Data.Repositories`

**Propósito:** Implementação concreta do repositório de documentos.

**Implementa:** `IDocumentoRepository`

**Dependências:**
- `IDbContextFactory<AppDbContext>`: Factory para criar contextos

**Métodos Implementados:**
- `Task AddAsync(Documento documento)`: Adiciona documento e salva
- `Task<Documento> GetByIdAsync(Guid id)`: Busca documento por ID

**Características:**
- Usa DbContext factory para criar contextos sob demanda
- Padrão repository sobre EF Core

---

#### `TipoDeDocumentoRepository`
**Namespace:** `ProdAbs.Infrastructure.Data.Repositories`

**Propósito:** Implementação do repositório de tipos de documento.

**Implementa:** `ITipoDeDocumentoRepository`

**Métodos Implementados:**
- `Task AddAsync(TipoDocumento tipoDocumento)`
- `Task<TipoDocumento> GetByIdAsync(Guid id)`
- `Task<List<TipoDocumento>> GetAllAsync()`

---

#### `ProntuarioRepository`
**Namespace:** `ProdAbs.Infrastructure.Data.Repositories`

**Propósito:** Implementação do repositório de prontuários.

**Implementa:** `IProntuarioRepository`

**Métodos Implementados:**
- `Task AddAsync(Prontuario prontuario)`
- `Task<Prontuario> GetByIdAsync(Guid id)`
- `Task UpdateAsync(Prontuario prontuario)`

---

### 4.3. Services (Serviços de Infraestrutura)

#### `LocalFileStorageService`
**Namespace:** `ProdAbs.Infrastructure.Services`

**Propósito:** Implementação de armazenamento de arquivos no sistema de arquivos local.

**Implementa:** `IFileStorageService`

**Dependências:**
- `IConfiguration`: Para obter caminho base
- `IHostEnvironment`: Informações do ambiente

**Propriedades:**
- `string StorageName = "LocalFileStorage"`

**Métodos Implementados:**
- `Task<Result<string>> UploadAsync(...)`: Salva arquivo localmente
- `Task<Result<Stream>> GetAsync(string storageLocation)`: Lê arquivo local
- `Task<Result> DeleteAsync(string storageLocation)`: Remove arquivo local

**Características:**
- Cria diretório se não existir
- Usa FileStream para operações
- Retorna Result para tratamento de erros

---

#### `S3FileStorageService`
**Namespace:** `ProdAbs.Infrastructure.Services`

**Propósito:** Implementação de armazenamento usando Amazon S3 (ou LocalStack).

**Implementa:** `IFileStorageService`

**Dependências:**
- `IConfiguration`: Para configurações S3
- `IHostEnvironment`: Ambiente da aplicação
- `AmazonS3Client`: Cliente SDK da AWS

**Propriedades:**
- `string StorageName = "S3FileStorage"`

**Configuração:**
- ServiceURL: http://localhost:4566 (LocalStack para desenvolvimento)
- ForcePathStyle: true
- AuthenticationRegion: us-east-1

**Métodos Implementados:**
- `Task<Result<string>> UploadAsync(...)`: Upload para S3
- `Task<Result<Stream>> GetAsync(...)`: Download do S3
- `Task<Result> DeleteAsync(...)`: Remoção do S3

**Características:**
- Usa LocalStack para desenvolvimento local
- Tratamento de exceções específicas do S3
- Retorna Result para erros

---

#### `AzureBlobStorageService`
**Namespace:** `ProdAbs.Infrastructure.Services`

**Propósito:** Implementação de armazenamento usando Azure Blob Storage.

**Implementa:** `IFileStorageService`

**Propriedades:**
- `string StorageName = "AzureBlobStorage"`

**Dependências:**
- Cliente SDK do Azure Storage Blobs

**Características:**
- Suporte a Azure Cloud
- Alternativa ao S3 para ambientes Azure

---

#### `SimpleAuditLogger`
**Namespace:** `ProdAbs.Infrastructure.Services`

**Propósito:** Implementação simples do logger de auditoria.

**Implementa:** `IAuditLogger`

**Método:**
- `Task LogAuditAsync(...)`: Registra ação de auditoria

**Características:**
- Implementação básica para auditoria
- Pode ser estendido para gravar em banco dedicado

---

#### `LocalEmailNotifier`
**Namespace:** `ProdAbs.Infrastructure.Services`

**Propósito:** Implementação local do notificador de email (para desenvolvimento).

**Implementa:** `IEmailNotifier`

**Método:**
- `Task SendEmailAsync(...)`: Simula envio de email

**Características:**
- Usado em desenvolvimento
- Pode ser substituído por implementação real (SendGrid, SMTP, etc.)

---

### 4.4. Messaging

#### `DocumentoCriadoConsumer`
**Namespace:** `ProdAbs.Infrastructure.Messaging`

**Propósito:** Consumidor de eventos de documento criado via MassTransit.

**Implementa:** `IConsumer<IDocumentoCriadoEvent>`

**Dependências:**
- `IAuditLogger`: Para registrar evento na auditoria

**Método:**
- `Task Consume(ConsumeContext<IDocumentoCriadoEvent> context)`: Processa evento

**Responsabilidades:**
- Receber evento de documento criado
- Registrar na trilha de auditoria
- Opcionalmente atualizar projeções de leitura

**Características:**
- Event-driven architecture
- Desacoplamento via mensageria
- MassTransit como barramento de eventos

---

### 4.5. Migrations (EF Core)

#### `20250824065518_InitialCreate`
**Namespace:** `ProdAbs.Infrastructure.Migrations`

**Propósito:** Migration inicial do banco de dados.

**Conteúdo:**
- Cria tabelas iniciais
- Define chaves primárias e estrangeiras
- Estabelece índices

---

#### `20250825024424_S3Storage`
**Namespace:** `ProdAbs.Infrastructure.Migrations`

**Propósito:** Migration relacionada ao suporte S3.

**Alterações:**
- Ajustes para suportar S3 storage

---

#### `AppDbContextModelSnapshot`
**Namespace:** `ProdAbs.Infrastructure.Migrations`

**Propósito:** Snapshot do modelo atual do banco de dados.

**Uso:** Utilizado pelo EF Core para comparação e geração de migrations

---

### 4.6. Dependency Injection

#### `DependencyInjection`
**Namespace:** `ProdAbs.Infrastructure`

**Propósito:** Registro de dependências da camada Infrastructure.

**Método:**
- `static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)`: Registra DbContext, repositórios, serviços, MassTransit

**Registros:**
- AppDbContext (com factory)
- Repositórios
- Serviços de storage
- Serviços de auditoria
- Serviços de email
- MassTransit e consumidores

---

## 5. Camada Presentation

A camada Presentation contém a API REST e a configuração da aplicação.

### 5.1. Controllers

#### `DocumentosController`
**Namespace:** `ProdAbs.Presentation.Api.Controllers`

**Propósito:** Controlador REST para operações com documentos.

**Rota Base:** `api/v1/documentos`

**Dependências:**
- `IMediator`: Para enviar comandos/queries

**Endpoints:**

**POST /upload**
- Descrição: Faz upload de um novo documento
- Parâmetro: `[FromForm] CriarDocumentoCommand`
- Retorno: `200 OK` com ID do documento, ou `400 Bad Request`

**GET /{id}**
- Descrição: Recupera metadados de um documento
- Parâmetro: `Guid id`
- Retorno: `200 OK` com DocumentoDTO, ou `404 Not Found`

**GET /{id}/download**
- Descrição: Faz download do arquivo do documento
- Parâmetro: `Guid id`
- Retorno: `File` stream, ou `404 Not Found`

**Características:**
- Usa MediatR para despachar comandos/queries
- Transforma Result em respostas HTTP apropriadas
- Comentário para Authorize (pode ser habilitado)

---

#### `TiposDocumentoController`
**Namespace:** `ProdAbs.Presentation.Api.Controllers`

**Propósito:** Controlador para gerenciamento de tipos de documento.

**Rota Base:** `api/v1/tiposdocumento`

**Endpoints:**

**POST /**
- Descrição: Cria novo tipo de documento
- Parâmetro: `[FromBody] CriarTipoDocumentoCommand`
- Retorno: `200 OK` com ID, ou `400 Bad Request`

**GET /**
- Descrição: Lista todos os tipos de documento
- Retorno: `200 OK` com lista de TipoDocumentoDetalhesDTO

---

#### `ProntuariosController`
**Namespace:** `ProdAbs.Presentation.Api.Controllers`

**Propósito:** Controlador para gerenciamento de prontuários.

**Rota Base:** `api/v1/prontuarios`

**Endpoints:**

**POST /**
- Descrição: Cria novo prontuário
- Parâmetro: `[FromBody] CriarProntuarioCommand`
- Retorno: `200 OK` com ID, ou `400 Bad Request`

**POST /{prontuarioId}/documentos**
- Descrição: Adiciona documento a um prontuário
- Parâmetros: `Guid prontuarioId`, `[FromBody] Guid documentoId`
- Retorno: `200 OK`, ou `400/404`

**GET /{id}**
- Descrição: Recupera prontuário com seus documentos
- Parâmetro: `Guid id`
- Retorno: `200 OK` com ProntuarioResumoDTO, ou `404 Not Found`

---

### 5.2. Program e Configuração

#### `Program`
**Namespace:** `ProdAbs.Presentation.Api`

**Propósito:** Ponto de entrada da aplicação e Composition Root.

**Responsabilidades:**
- Configurar WebApplication builder
- Registrar serviços (DI)
- Configurar middleware pipeline
- Integração com ASP.NET Aspire
- Configurar Swagger/OpenAPI
- Configurar logging (Serilog)

**Serviços Registrados:**
- Controllers
- Serviços da camada Application
- Serviços da camada Infrastructure
- Aspire service defaults
- MediatR
- FluentValidation

**Middleware:**
- Exception handling
- HTTPS redirection
- Authorization
- Controllers
- Swagger UI

---

#### `DependencyInjection`
**Namespace:** `ProdAbs.Presentation.Api`

**Propósito:** Registro de dependências específicas da camada Presentation.

**Método:**
- `static IServiceCollection AddPresentationServices(this IServiceCollection services)`: Registra controllers, CORS, autenticação

---

## 6. AppHost e ServiceDefaults

### 6.1. AppHost

#### `AppHost`
**Namespace:** `ProdAbs.AppHost`

**Propósito:** Orquestrador de serviços usando ASP.NET Aspire.

**Responsabilidades:**
- Configurar e orquestrar serviços da aplicação
- Gerenciar backing services (PostgreSQL, LocalStack, etc.)
- Configurar telemetria distribuída
- Facilitar desenvolvimento local

**Configurações Típicas:**
- Registro de API
- Registro de banco de dados PostgreSQL
- Registro de LocalStack (para S3 local)
- Configuração de service discovery

---

### 6.2. ServiceDefaults

#### `Extensions`
**Namespace:** `ProdAbs.ServiceDefaults`

**Propósito:** Métodos de extensão para configurações padrão de serviços.

**Responsabilidades:**
- Configurar OpenTelemetry
- Configurar health checks
- Configurar service discovery
- Padronizar configurações entre serviços

**Métodos:**
- `AddServiceDefaults(this IHostApplicationBuilder builder)`: Adiciona configurações padrão
- `ConfigureOpenTelemetry(...)`: Configura telemetria
- `AddDefaultHealthChecks(...)`: Adiciona health checks padrão

---

## Resumo da Arquitetura

### Fluxo de uma Requisição Típica

1. **Presentation (Controller)** → Recebe requisição HTTP
2. **MediatR** → Despacha comando/query
3. **ValidationBehavior** → Valida requisição
4. **Handler (Application)** → Orquestra lógica de negócio
5. **Domain** → Aplica regras de negócio
6. **Infrastructure (Repository)** → Persiste dados
7. **Infrastructure (Storage)** → Armazena arquivos
8. **MassTransit** → Publica eventos
9. **Consumer** → Processa eventos (auditoria, projeções)
10. **Presentation** → Retorna resposta HTTP

### Princípios Aplicados

- **Clean Architecture**: Separação clara de camadas
- **CQRS Leve**: Separação de comandos e queries
- **DDD**: Agregados, entidades, value objects
- **Result Pattern**: Tratamento explícito de erros
- **Event-Driven**: Eventos de domínio via MassTransit
- **Dependency Inversion**: Interfaces na Application/Domain, implementações na Infrastructure
- **SOLID**: Todos os princípios aplicados
- **Repository Pattern**: Abstração de acesso a dados
- **Pipeline Behavior**: Validação automatizada
- **DTO Pattern**: Separação entre entidades de domínio e contratos de API

### Tecnologias Principais

- **.NET 8+**: Framework base
- **ASP.NET Core**: Web API
- **ASP.NET Aspire**: Orquestração e telemetria
- **Entity Framework Core**: ORM para PostgreSQL
- **MediatR**: CQRS e mediação
- **MassTransit**: Mensageria e eventos
- **FluentValidation**: Validação de dados
- **PostgreSQL**: Banco de dados relacional
- **LocalStack**: S3 local para desenvolvimento
- **Serilog**: Logging estruturado
- **OpenTelemetry**: Observabilidade

---

## Conclusão

Este documento forneceu uma visão completa e detalhada de todas as 72 classes do sistema ProdAbs, organizadas por camada arquitetônica. Cada classe foi documentada com seu propósito, responsabilidades, propriedades principais e relacionamentos.

O sistema segue rigorosamente os princípios da Clean Architecture, garantindo separação de preocupações, testabilidade, manutenibilidade e escalabilidade. A adoção de padrões modernos como Result Pattern, CQRS, Event-Driven Architecture e DDD tornam o código robusto e preparado para evolução.

Para mais informações sobre a arquitetura geral do sistema, consulte o arquivo `GEMINI.md` na raiz do repositório.

---

**Última Atualização:** 07/11/2025  
**Autor:** Equipe ProdAbs  
**Versão:** 1.0
