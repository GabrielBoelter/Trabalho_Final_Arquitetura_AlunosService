# 💰 Academia - Microsserviço: AlunosService

Este repositório faz parte de um sistema de academia desenvolvido com **C# (.NET 8)** e arquitetura de **microsserviços**, utilizando **SQLite** como persistência local e comunicação via **HTTP POST** entre os serviços.

## 🎯 Propósito do Microsserviço

Gerenciar o cadastro, status e consulta dos alunos da academia. Também serve como ponto central de comunicação com os microsserviços de **pagamentos** e **treinos**, realizando validações e atualizações de status quando necessário.


## 👥 Usuários do Sistema

- **Funcionários da recepção**: realizam cadastros e atualizações de alunos.
- **Instrutores**: acessam treinos vinculados aos alunos.
- **Administradores**: acompanham status e inadimplência dos alunos.

---

## ✅ Requisitos Funcionais Atendidos

- RF01: Cadastrar alunos com dados pessoais e status.
- RF02: Atualizar ou deletar alunos existentes.
- RF04: Consultar alunos (por ID, CPF, e-mail, status ou termo).
- RF05: Atualizar status do aluno via POST externo.
- RF06: Validar status do aluno antes da criação de treinos.
- RF07: Exibir lista de treinos por integração via POST com TreinosService.

---

## 🔁 Integrações Entre Microsserviços

| Tipo de Integração | Serviço Origem     | Serviço Destino     | Ação Realizada                                                                  |
|--------------------|--------------------|----------------------|----------------------------------------------------------------------------------|
| POST (validação)   | TreinosService     | AlunosService        | Verifica se o aluno está ativo antes de cadastrar treino                        |
| POST (consulta)    | AlunosService      | TreinosService       | Consulta e exibe lista de treinos vinculados ao aluno                           |
| POST (alteração)   | PagamentosService  | AlunosService        | Atualiza status do aluno para "inadimplente" ao detectar falta de pagamento     |

---

## 📦 Estrutura do Projeto

O projeto segue o padrão em camadas, com pastas separadas por responsabilidade:

```
AlunosService/
├── Controllers/ # Endpoints REST (AlunosController.cs)
├── DTOs/ # Objetos de transferência de dados
│ ├── AlunoCreateDTO.cs
│ ├── AlunoRequestDTO.cs
│ ├── AlunoResponseDTO.cs
│ ├── AlunoUpdateDTO.cs
│ ├── AlunoPagamentosDTO.cs
│ └── AlunoCompletoDTO.cs
├── Models/ # Entidade Aluno.cs + enum StatusAluno
├── Repositories/ # Interfaces e implementações de dados
│ ├── IAlunoRepository.cs
│ └── AlunoRepository.cs
├── Services/ # Regras de negócio
│ ├── IAlunoService.cs
│ └── AlunoService.cs
├── External/ # Comunicação com PagamentosService e TreinosService
│ ├── PagamentosServiceClient.cs
│ ├── TreinosServiceClient.cs
│ ├── IPagamentosServiceClient.cs
│ └── ITreinosServiceClient.cs
├── Data/ # Contexto e fábrica do EF Core
│ ├── AppDbContext.cs
│ └── AppDbContextFactory.cs
├── Migrations/ # Histórico de migrações do banco
│ ├── 20250620195002_InitialCreate.cs
│ └── AppDbContextModelSnapshot.cs
├── appsettings.json # Configurações do banco SQLite
├── alunos.db # Arquivo do banco SQLite
├── Program.cs # Configuração da aplicação
└── AlunosService.http # Testes de requisição HTTP

---

## 🔗 Endpoints Disponíveis

| Verbo | Rota                              | Ação                                                      |
|-------|-----------------------------------|-----------------------------------------------------------|
| POST  | `/api/alunos`                     | Cadastra um novo aluno                                    |
| GET   | `/api/alunos`                     | Lista todos os alunos                                     |
| GET   | `/api/alunos/{id}`                | Retorna aluno por ID                                      |
| GET   | `/api/alunos/cpf/{cpf}`           | Busca aluno por CPF                                       |
| GET   | `/api/alunos/email/{email}`       | Busca aluno por email                                     |
| GET   | `/api/alunos/status/{status}`     | Lista alunos com determinado status                       |
| GET   | `/api/alunos/total`               | Retorna total de alunos cadastrados                       |
| GET   | `/api/alunos/buscar?termo=xxx`    | Busca alunos por termo (nome, email, cpf)                 |
| PUT   | `/api/alunos/{id}`                | Atualiza dados do aluno                                   |
| DELETE| `/api/alunos/{id}`                | Remove um aluno                                           |
| POST  | `/api/alunos/sincronizar-treinos` | Consulta treinos de um aluno (integração com Treinos)     |
| POST  | `/api/alunos/inadimplente`        | Marca um aluno como inadimplente (via POST externo)       |

> ⚠️ Endpoints de integração utilizam `HttpClient` e JSON para comunicação com outros serviços.

---

## 💾 Banco de Dados

- Utiliza **SQLite** como armazenamento local.
- Configurado via `appsettings.json` no caminho `alunos.db`.
- Gerenciado por **Entity Framework Core** com suporte a migrações.

---

## 🛠️ Tecnologias e Ferramentas

- **.NET 8 Web API**
- **C#**
- **Entity Framework Core**
- **SQLite**
- **Swagger** para testes e documentação da API
- **.http file** e **Postman** para testes de integração
- **Injeção de Dependência**
- **Arquitetura em Camadas** (Controller, Service, Repository, DTOs, Models)

---


## 🚀 Como Executar Localmente

1. Clone o repositório:
   ```bash
   git clone https://github.com/SEU_USUARIO/academia-pagamentos-service.git
   ```

2. Abra a solução no **Visual Studio 2022**.

3. Execute as migrações (se necessário):
   ```bash
   dotnet ef database update
   ```

4. Rode o projeto (`F5`) e acesse o Swagger:
   ```
   https://localhost:xxxx/swagger
   ```

5. Teste os endpoints diretamente via Swagger ou Postman.

---

## 📂 Repositórios Relacionados

- [`academia-alunos-service`](https://github.com/SEU_USUARIO/academia-alunos-service)
- [`academia-treinos-service`](https://github.com/SEU_USUARIO/academia-treinos-service)
- [`academia-pagamentos-service`](https://github.com/SEU_USUARIO/academia-pagamentos-service)

---

## 👨‍🏫 Desenvolvido para a disciplina de Arquitetura de Software  
**Centro Universitário SATC** – Prof. Eduardo Cizeski Meneghel  
