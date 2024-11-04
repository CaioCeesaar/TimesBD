## Bem vindo ao TimesBD

Este repositório contém o projeto desenvolvido durante a mentoria do devConnect da Ploomes que teve uma duração de aproximadamente 5 meses. Este projeto é uma demonstração prática de algumas tecnologias que exploramos e aprendemos ao longo do caminho. 

- **C#**: Linguagem de programação principal. 
- **Git e GitHub**: Para controle de versão e colaboração de código. 
- **.NET**: Framework para construção de aplicações de alta performance.
- **T-SQL**: Linguagem para consulta e manipulação de dados no SQL Server. 
- **MongoDB**: Banco de dados NoSQL para armazenamento de dados em formato de documentos. (nesse projeto utilizamos para simular um sistema de logs)
- **Dapper**: Micro ORM para mapeamento de objetos em SQL
- **Entity Framework Core**: ORM para acesso a dados.
- **Consulta a APIs externas**: Integração com serviços externos. 
- **Código limpo**: Práticas para manter o código legível e manutenível. 
- **Princípios SOLID**: Aplicação dos princípios de design de software.
- **Workers**: Utilização de trabalhadores para processos em segundo plano. 
- **Injeção de Dependência**: Para desacoplamento e teste de código.

## Arquitetura básica do sistema

![image](https://github.com/CaioCeesaar/TimesBD/assets/83191307/913d0e4c-d5cc-45ab-9eaf-53e9302bc4ff)

### Como começar?

1. Clone o repositório: 
```
gh repo clone CaioCeesaar/TimesBD
```
2. Entre no diretório do projeto clonado
3. Restaure os pacotes Nuget:
```
dotnet restore
```
4. Restaure o banco de dados: https://github.com/CaioCeesaar/BancoTimes
5. Atualize as strings de conexão para o SQL Server e o MongoDB no arquivo de configuração `appsettings.json`
6. Execute a aplicação:
```
dotnet run
```

## Contato

* Caio - [CaioCeesarr](https://github.com/CaioCeesaar)
* Tiago - [tiags38](https://github.com/tiags38)