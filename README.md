# DesafioBackEndAPI - Desafio Desenvolvedor BackEnd - 2025

### AUTOR
<ins>WEIDERSON MENDES QUEIROZ - ANALISTA DESENVOLVEDOR .NET</ins>

+ LINKDIN
www.linkedin.com/in/weiderson

+ CANAL
www.youtube.com/@weidersonmendes

+ EMAIL
weidersn@protonmail.com

### RECURSOS
+ AutoMapper
+ DTOs
+ Repository
+ UnitOfWork
+ Autenticação e Autorização (JWT)
+ Versionamento

### VERSÕES
+ .NET 9.0
+ ENTITY FRAMEWORK CORE 9.04
+ MYSQL Server version 8.0.42
+ Visual Studio 2022

### COMANDO PARA EF MIGRATIONS
+ dotnet ef migrations add NewMigration 
+ dotnet ef database update --project DesafioBackendAPI.csproj
+ Para popular as tabelas do banco descomente o código referente a "app.Services.CreateScope()" no Program.cs, e rode aplicação.
+ Em seguida, comente o código novamente. Caso prefira, rode os arquivos .sql para popular o banco.

### CONTA
+ Será possível excluir a CONTA caso o registro não possua saldo.
+ Será possível excluir CONTA caso o registro esteja ATIVO.
+ Será possível editar a CONTA caso o registro esteja coma situação ATIVA.
+ O download do documento relacionado a conta pode ser realizado através do nome/extensão do arquivo salvo ex. _1224456111626299747.jpg

### TRANSAÇÕES
+ Será possível adicionar TRANSAÇÕES caso a conta não esteja excluída LOGICA/FISICAMENTE.
+ Será possível adicionar TRANSAÇÕES caso a conta não esteja INATIVA/EXCLUÍDA.
+ A transação entre contas é informada tanto na busca da conta de ORIGEM quanto na de DESTINO.
+ Caso ocorra algum erro durante o cadastro da TRANSAÇÃO, o processo é desfeito através de Transaction sem salvar lixo no banco. 

### EXTRATO
+ A listagem de EXTRATO contém o SALDO na primeira linha e todas as TRANSAÇÕES, incluindo quando uma conta efetua e também recebe uma TRANFERÊNCIA entre CONTAS. 

### USUARIOS
+ Caso execute o método Seed para popular as tabelas. será necessário rodar o método de update para atualizar a senha antes de efetuar o login
+ para resgatar a chave JWT. 

### VERSOES V1 E V2
+ Os endpoints da versão v1 não exige autenticação/autorização (Incluindo os métodos de exclusão lógica.).
+ Os endpoints da versão v2 exige autenticação/autorização para os métodos de exclusão, visto que ocorre a exclusão física.

### LINKS EXTERNOS
+ MYSQL
https://dev.mysql.com/downloads/installer/

+ VISUAL STUDIO
https://visualstudio.microsoft.com/pt-br/downloads/
