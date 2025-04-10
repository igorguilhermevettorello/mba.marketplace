# Feedback - Avaliação Geral

## Front End
### Navegação
**Pontos positivos:**
-  Estrutura básica de controllers e views está presente.

### Design
  - Será avaliado na entrega final.

### Funcionalidade
**Pontos positivos:**
-  Controllers e views para Produto e Categoria existem, com rotas aparentemente corretas.

**Pontos negativos:**
  - Será avaliado na entrega final

## Back End
### Arquitetura
**Pontos positivos:**
-  Projeto segue uma arquitetura enxuta
-  MVC e API estão separados fisicamente e não se consomem diretamente — conforme exigido no escopo.

**Pontos negativos:**
-  Camada Core e Data se conflitam, apenas uma é necessária nesse caso.
-  Não há abstração clara entre lógica de negócio e persistência de dados.
-  `MBA.Marketplace.Data` assume parte do papel da camada de domínio ao implementar serviços
-  Falta coesão entre os projetos — não está claro o papel exato de cada um sem leitura aprofundada.

### Funcionalidade
**Pontos positivos:**
-  CRUD de Produto e Categoria está implementado em controllers REST na API.
-  Autenticação via JWT está parcialmente configurada.
-  Vendedor é criado no momento do registro do usuário


### Modelagem
**Pontos positivos:**
-  Entidades `Produto`, `Categoria` e `Vendedor` estão presentes.
-  Estrutura básica de relacionamento está coerente com o escopo.

## Projeto
### Organização
**Pontos positivos:**
-  Organização em múltiplos projetos com nomes claros e coerentes com suas responsabilidades.

### Documentação
**Pontos positivos:**
-  README.md básico existe com nome e descrição do projeto.


### Instalação
**Pontos positivos:**
-  Uso de SQLite está implementado, conforme exigência do escopo para ambiente de desenvolvimento.

**Pontos negativos:**
-  Nenhum dado de exemplo ou script de seed para validação do CRUD.
-  Migrations não são aplicadas automaticamente.

