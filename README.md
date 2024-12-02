# DataDial

O **DataDial** é uma aplicação C# que permite validar números de telefone, identificar se são válidos ou inválidos, obter informações sobre o número (como o código do país e o formato internacional), além de verificar se o número está associado a listas de SPAM. A aplicação também realiza uma busca online com Dorks para informações adicionais.

## Funcionalidades

- **Validação de número de telefone**: Valida se o número informado é um número válido.
- **Formatação do número**: Exibe o número no formato internacional.
- **Identificação do país**: Informa o país associado ao código do país do número.
- **Verificação de SPAM**: Checa se o número está presente em listas de SPAM.
- **Busca online**: Realiza uma busca online usando Dorks para verificar mais informações sobre o número.

## Requisitos

Para rodar o **DataDial** localmente, é necessário ter as seguintes dependências instaladas:

- **.NET Core SDK** (versão 3.1 ou superior)
- **Biblioteca `PhoneNumbers`**: Usada para parsear e validar números de telefone.
- **Biblioteca `DataDial.CountryCodeLookup`**: Usada para mapear o código do país para o nome do país.
- **Biblioteca `DataDial.Helpers`**: Utilizada para realizar a verificação de SPAM e outras funcionalidades auxiliares.

## Como Executar

1. Clone este repositório:
   ```bash
   git clone https://github.com/SayesCode/DataDial.git
   cd DataDial
   ```

2. Restaure as dependências:
   ```bash
   dotnet restore
   ```

3. Execute o projeto:
   ```bash
   dotnet run
   ```

4. O programa solicitará que você insira um número de telefone com o código do país (ex: `+5511999999999`). A partir daí, ele realizará a validação e outras verificações.

## Funcionalidade de Spam

O aplicativo verifica se o número informado está presente em listas de SPAM. Esta verificação é feita de forma assíncrona para garantir uma melhor performance. Caso o número seja identificado como SPAM, será mostrado um alerta vermelho na tela.

## Releases

Para baixar o pre-binario, acesse a seção **Releases** do repositório.

- **Windows**: Baixe o arquivo binário [win-x64.zip](https://github.com/SayesCode/DataDial/releases/download/1.3.0/win-x64.zip).
- **Windows**: Baixe o arquivo binário [win-arm64.zip](https://github.com/SayesCode/DataDial/releases/download/1.3.0/win-arm64.zip).
- **Linux**: Baixe o arquivo binário [linux-arm.zip](https://github.com/SayesCode/DataDial/releases/download/1.3.0/linux-arm.zip).
- **Linux**: Baixe o arquivo binário [linux-arm64.zip](https://github.com/SayesCode/DataDial/releases/download/1.3.0/linux-arm64.zip).
- **Linux**: Baixe o arquivo binário [linux-x64.zip](https://github.com/SayesCode/DataDial/releases/download/1.3.0/linux-x64.zip).

## Contribuindo

1. Faça um fork deste repositório.
2. Crie uma branch para sua feature (`git checkout -b feature/nome-da-feature`).
3. Faça suas alterações.
4. Envie um pull request para a branch `main` deste repositório.

## Licença

Este projeto está licenciado sob a MIT License - consulte o arquivo [LICENSE](LICENSE) para mais detalhes.

## Contato

Caso tenha dúvidas ou sugestões, entre em contato atráves de: [sayes.vercel.app](https://sayes.vercel.app/contact).
