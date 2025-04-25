# **MBA Marketplace - Aplicação Marketplace com MVC e API RESTful**

## **1. Apresentação**

Bem-vindo ao repositório do projeto **MBA Marketplace**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **Introdução ao Desenvolvimento ASP.NET Core**.
O MBA Marketplace é uma plataforma de e-commerce no formato marketplace, onde múltiplos vendedores podem se cadastrar, criar produtos, e gerenciar seu catálogo de forma independente. O projeto foi desenvolvido com foco em boas práticas de arquitetura, autenticação segura com JWT e Cookies, consumo de API via ASP.NET Core MVC, e integração com SQLite (dev) e SQL Server (prod).

### **Autor(es)**
- **Igor Guilherme Vettorello**


## **2. Proposta do Projeto**

O projeto consiste em:

- **Aplicação MVC:** Interface web para interação com a aplicação marketplace.
- **API RESTful:** Exposição dos recursos do sistema para integração com outras aplicações ou desenvolvimento de front-ends alternativos.
- **Autenticação e Autorização:** Implementação de controle de acesso. O sistema garante que apenas o Vendedor proprietário pode consultar, visualizar e modificar os seus Produtos
- **Acesso a Dados:** Implementação de acesso ao banco de dados através de ORM.

## **3. Tecnologias Utilizadas**

- **Linguagem de Programação:** C#
- **Frameworks:**
  - ASP.NET Core MVC
  - ASP.NET Core Web API
  - Entity Framework Core
- **Banco de Dados:** SQLite (dev) / SQL Server (prod)
- **Autenticação e Autorização:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token) para autenticação na API
- **Front-end:**
  - Razor Pages/Views
  - HTML/CSS para estilização básica
- **Documentação da API:** Swagger

## **4. Estrutura do Projeto**

A estrutura do projeto é organizada da seguinte forma:

- mba.marketplace/
  - MBA.Marketplace.Web/ - Projeto MVC
  - MBA.Marketplace.Api/ - API RESTful
  - MBA.Marketplace.Data/ - Modelos de dados, configuração do EF Core, entidades, interfaces, Enums, Utils
- README.md - Arquivo de Documentação do Projeto
- FEEDBACK.md - Arquivo para Consolidação dos Feedbacks
- .gitignore - Arquivo de Ignoração do Git

## **5. Funcionalidades Implementadas**

- **CRUD para Categoria e Produtos:** Permite criar, editar, visualizar e excluir categorias e produtos.
- **Autenticação e Autorização:** Diferenciação entre vendedores.
- **API RESTful:** Exposição de endpoints para operações CRUD via API.
- **Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.

## **6. Como Executar o Projeto**

### **Pré-requisitos**

- .NET SDK 8.0 ou superior
- SQL Server
- SQLite
- Visual Studio 2022 ou superior (ou qualquer IDE de sua preferência)
- Git

### **Passos para Execução**

1. **Clone o Repositório:**
   - `git clone https://github.com/igorguilhermevettorello/mba.marketplace.git`
   - `cd mba.marketplace`

2. **Configuração do Banco de Dados:**
   - No arquivo `appsettings.json`, configure a string de conexão do SQLite e SQL Server.
   - Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos

3. **Executar a Aplicação MVC:**
   - `cd MBA.Marketplace.Web`
   - `dotnet run`
   - Acesse a aplicação em: https://localhost:7015/

4. **Executar a API:**
   - `cd MBA.Marketplace.API`
   - `dotnet run`
   - Acesse a documentação da API em: https://localhost:7053/swagger/index.html

## **7. Instruções de Configuração**

- **JWT para API:** As chaves de configuração do JWT estão no `appsettings.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **8. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

https://localhost:7053/swagger/index.html

## **9. Avaliação**

- Este projeto é parte de um curso acadêmico e não aceita contribuições externas. 
- Para feedbacks ou dúvidas utilize o recurso de Issues
- O arquivo `FEEDBACK.md` é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.
