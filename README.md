# Sobre a aplicação:
  - Essa é uma api para consultar e realizar cadastros e aluguel de moto.
# Principais tecnologias
 - Aplicação feita em .net 8
 - serviços integrados rabbitMQ e MongoDB
 - Teste Unitários de alguns métodos
 - Swagger
# Instalação:
  Existe dois arquivos dentro da raiz do projeto para subir em um docker, necessário fazer configurações dentro dos arquivos yml. ex: user e pass.
  Comando para rodar o dokcer-compose. "docker compose up -d", parametro -d para rodar em segundo plano.     
# Usabilidade 
  - Pode ser feito request via Postman ou pelo próprio swagger, necessário realizar primeiro um cadastro de usuário e passar o token(JWT) nas request.
