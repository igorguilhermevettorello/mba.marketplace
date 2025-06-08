# Feedback - Avaliação Geral

## Front End

### Navegação
  * Pontos positivos:
    - Views implementadas para login, registro, cadastro e edição de produtos e categorias.
    - Navegação clara, com rotas definidas conforme os requisitos de CRUD para produtos e categorias.

  * Pontos negativos:
    - Nenhum ponto negativo observado quanto à navegação.

### Design
  - A interface web é funcional, com layout adequado ao propósito administrativo. Uso básico de HTML e CSS, suficiente para o escopo do projeto.

### Funcionalidade
  * Pontos positivos:
    - Todas as funcionalidades de CRUD para produtos e categorias estão implementadas.
    - Exibição pública de produtos na home conforme esperado.
    - Acesso restrito por autenticação em ações protegidas.
    - Registro de usuário e criação simultânea do vendedor implementados.

  * Pontos negativos:
    - Nenhum ponto negativo encontrado quanto à cobertura funcional.

## Back End

### Arquitetura
  * Pontos positivos:
    - Projeto segue a separação API, MVC e camada de dados.
    - Organização clara entre camadas e responsabilidades.

  * Pontos negativos:
    - A camada `MBA.Marketplace.Data` assume responsabilidades além de persistência, como lógica de negócio e autenticação, sendo mais apropriado nomeá-la como `Core`.
    - Acoplamento da entidade `Vendedor` na `ApplicationUser` fere a separação recomendada entre identidade e domínio.

### Funcionalidade
  * Pontos positivos:
    - Uso correto de ASP.NET Identity para autenticação/autorização.
    - Uso do EF Core com SQLite.
    - Seed automático de dados e execução de migrations no startup da aplicação implementados corretamente.

  * Pontos negativos:
    - Nenhum ponto negativo funcional observado.

### Modelagem
  * Pontos positivos:
    - Entidades bem definidas, simples e coerentes com o domínio.
    - Associação de produtos com vendedor implementada.

  * Pontos negativos:
    - Uso de data annotations diretamente nas entidades, em vez de classes de configuração (via `IEntityTypeConfiguration`), reduz a flexibilidade da modelagem.

## Projeto

### Organização
  * Pontos positivos:
    - Separação clara dos projetos (API, MVC, Data).
    - Solution file presente e bem estruturado.
    - Estrutura de pastas coesa e nomeações consistentes.

  * Pontos negativos:
    - Projeto `Data` com papel de domínio, idealmente deveria ser renomeado para `Core`.

### Documentação
  * Pontos positivos:
    - README.md presente com boas instruções.
    - Swagger implementado.
    - Arquivo `FEEDBACK.md` presente.

  * Pontos negativos:
    - Nenhum ponto negativo relevante.

### Instalação
  * Pontos positivos:
    - SQLite configurado.
    - Execução de migrations e seed no startup automatizada.

  * Pontos negativos:
    - Nenhum ponto negativo identificado.

---

# 📊 Matriz de Avaliação de Projetos

| **Critério**                   | **Peso** | **Nota** | **Resultado Ponderado**                  |
|-------------------------------|----------|----------|------------------------------------------|
| **Funcionalidade**            | 30%      | 9        | 2,7                                      |
| **Qualidade do Código**       | 20%      | 9        | 1,8                                      |
| **Eficiência e Desempenho**   | 20%      | 9        | 1,8                                      |
| **Inovação e Diferenciais**   | 10%      | 10       | 1,0                                      |
| **Documentação e Organização**| 10%      | 9        | 0,9                                      |
| **Resolução de Feedbacks**    | 10%      | 10       | 1,0                                      |
| **Total**                     | 100%     | -        | **9,2**                                  |

## 🎯 **Nota Final: 9,2 / 10**
