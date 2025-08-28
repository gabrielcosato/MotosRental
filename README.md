##  Visão Geral do Projeto

O projeto MotosRental é uma solução backend para gerenciar o ciclo de vida de aluguéis de motos, desde o cadastro de motos e motoristas até a gestão de planos de aluguel e locações. A aplicação foi projetada com foco em escalabilidade, manutenibilidade e testabilidade, utilizando padrões de design modernos e tecnologias amplamente adotadas no mercado.

## Requisitos do Projeto Cumpridos

-  **CRUD de Motos:** Funcionalidade para gerenciar motos no sistema.
-  **CRUD de Motoristas:** Funcionalidade para gerenciar motoristas no sistema.
-  **Gestão de Planos de Aluguel:** Definição e gerenciamento de diferentes planos de aluguel com suas regras de custo e duração.
-  **Locação de Motos:** Processo de aluguel de motos por motoristas, associando-os a um plano de aluguel.
-  **Notificação de Eventos (Mensageria):** Implementação de um sistema de mensageria com RabbitMQ para notificar sobre eventos importantes, como o cadastro de motos.

## Arquitetura

O projeto segue a **Clean Architecture**, promovendo uma clara separação de responsabilidades e independência de frameworks e bancos de dados. A estrutura é organizada em camadas:

-   **`MotosRental.Api`**: Contém os controladores da API REST, configuração de injeção de dependência, middleware de tratamento de erros.
-   **`MotosRental.Application`**: Contém a lógica de negócio com Services, DTOs, mapeamentos (AutoMapper) e consumidores de eventos.
-   **`MotosRental.Domain`**: Contém as entidades de negócio, enums e interfaces de repositórios e serviços.
-   **`MotosRental.Infraestructure`**: Contém as implementações concretas dos repositórios com DbContext, e a implementação do publicador de mensagens.

## Tecnologias e Ferramentas Utilizadas

-   **Backend:**
    -   C# (.NET 8)
    -   ASP.NET Core
    -   Entity Framework Core (ORM)
    -   AutoMapper (Mapeamento de Objetos)
    -   JWT Bearer
    -   JSON

-   **Banco de Dados:**
    -   PostgreSQL (Banco de dados relacional principal)
    -   MongoDB (Banco de dados NoSQL para notificações)

-   **Mensageria:**
    -   RabbitMQ (Broker de Mensagens)
  

-   **Controle de Versão:**
    -   Git

-   **Containerização:**
    -   Docker
    -   Docker Compose

-   **Documentação da API:**
    -   Swagger / OpenAPI

## Funcionalidades Principais

-   **Gerenciamento de Motos:** Cadastro, consulta, atualização e exclusão de motos.
-   **Gerenciamento de Motoristas:** Cadastro, consulta, atualização (incluindo upload de CNH) e exclusão de motoristas.
-   **Gestão de Planos de Aluguel:** Definição de planos com diferentes durações e custos.
-   **Processo de Locação:** Registro de aluguéis de motos por motoristas, com cálculo de custos e multas.
-   **Sistema de Notificações:** Publicação de eventos de cadastro de motos e consumo assíncrono para notificar sobre motos de anos específicos (ex: 2024), persistindo essas notificações.
-   **Autenticação JWT:** Proteção da API com tokens JWT para acesso seguro aos endpoints.

## Endpoints da API

A API pode ser acessada via Swagger UI após a execução do projeto. Abaixo estão alguns dos principais endpoints:

-   **Autenticação:**
    -   `POST /api/auth/login`: Autentica um usuário admin e retorna um token JWT.

-   **Motos:**
    -   `POST /api/motorcycle`: Cadastra uma nova moto.
    -   `GET /api/motorcycle`: Lista todas as motos.
    -   `GET /api/motorcycle/plate/{plate}`: Busca uma moto pela placa.
    -   `GET /api/motorcycle/{id}`: Busca uma moto por ID.
    -   `PUT /api/motorcycle/{id}/plate`: Atualiza a placa de uma moto.
    -   `DELETE /api/motorcycle/{id}`: Remove uma moto.

-   **Motoristas:**
    -   `POST /api/driver`: Cadastra um novo motorista.
    -   `GET /api/driver`: Lista todos os motoristas.
    -   `GET /api/driver/{id}`: Busca um motorista por ID.
    -   `PATCH /api/driver/{id}/upload-license`: Faz upload da imagem da CNH do motorista.
    -   `DELETE /api/driver/{id}`: Remove um motorista.

-   **Aluguéis:**
    -   `POST /api/rent`: Inicia um novo aluguel.
    -   `GET /api/rent/{id}`: Busca um aluguel por ID.
    -   `PUT /api/rent/{id}/devolucao`: Registra a data de devolução da moto.
    -   `DELETE /api/rent/{id}`: Remove um aluguel.

## Serviços Externos

O projeto utiliza os seguintes serviços externos, orquestrados via Docker Compose:

-   **PostgreSQL:** Banco de dados relacional para dados transacionais (motos, motoristas, aluguéis, planos).
-   **RabbitMQ:** Broker de mensagens para comunicação assíncrona e desacoplamento de serviços.
-   **MongoDB:** Banco de dados NoSQL para armazenamento de notificações de eventos.

## Como Rodar o Projeto

Para configurar e rodar o projeto em sua máquina local, siga os passos abaixo:

1.  **Pré-requisitos:**
    -   .NET SDK 8.0 ou superior
    -   Docker Desktop (com Docker Compose)
    -   Git
    -   Rider ou VS Code

2.  **Clone o Repositório:**
    ```bash
    git clone https://github.com/seu-usuario/MotosRental.git
    cd MotosRental
    ```
    
3.  **Iniciar os Serviços com Docker Compose:**
    -   Na raiz do projeto (onde está `docker-compose.yml` ), execute:
        ```bash
        docker-compose up -d
        ```
    -   Isso iniciará os contêineres para PostgreSQL, RabbitMQ e MongoDB.

4.  **Aplicar Migrações do Entity Framework:**
    
    -   Aplique as migrações para criar o esquema do banco de dados PostgreSQL:
        ```bash
        dotnet ef migrations add Initial
        
        dotnet ef database update
        ```

5.  **Rodar a Aplicação:**

 -   Com o docker rodando e as tabelas criadas agora é hora de rodar a API Back-End:
        ```bash
        dotnet run
        ```
    -   A API estará disponível em `http://localhost:7287` (ou a porta configurada ).

6.  **Acessar Swagger UI:**
    -   Abra seu navegador e vá para `http://localhost:7287/swagger/index.html` para interagir com a API.

## Configuração

-  `appsettings.json` : Configuração do PostgreSQL, RabbitMQ, MongoDB e JWT.
-  `docker-compose.yml` : Configuração do PostgreSQL, RabbitMQ, e MongoDB (Portas e credenciais).
-  `appsettings.Development.Json`

## Diferenciais
- EntityFramework
- Docker e Docker Compose
- Design Patterns
- Documentação
- Tratamento de erros
- Arquitetura e modelagem de dados
- Código escrito em língua inglesa
- Código limpo e organizado
- Seguir convenções utilizadas pela comunidade

