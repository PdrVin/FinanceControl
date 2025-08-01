# 💰 FinanceControl

Um sistema completo de gerenciamento de finanças pessoais desenvolvido para auxiliar no controle de receitas, despesas e saldos de contas de forma intuitiva e visual.

## ✨ Funcionalidades

-   **Dashboard Interativo:** Visualize de forma clara e rápida a saúde financeira com gráficos que detalham receitas e despesas por categoria.
-   **Gestão de Múltiplas Contas:** Gerencie o saldo atual e previsto de diferentes contas bancárias (ex: Nubank, PicPay, Carteira).
-   **Controle de Transações:** Registre e edite facilmente receitas, despesas, transferências e despesas com cartão de crédito.
-   **Arquitetura Sólida:** Desenvolvido com base em uma arquitetura limpa, utilizando DTOs, Serviços e Repositórios para garantir escalabilidade e manutenibilidade.
-   **Notificações:** Feedback visual para o usuário através de mensagens de sucesso ou erro para uma melhor experiência de uso.

## 🚀 Tecnologias Utilizadas

-   **Backend:**
    -   ASP.NET Core MVC
    -   Entity Framework Core (EF Core)
    -   C#
    -   AutoMapper

-   **Frontend:**
    -   HTML5, CSS3, JavaScript
    -   Bootstrap 5 (para um design responsivo e moderno)
    -   Icons: Bootstrap Icons

-   **Banco de Dados:**
    -   SQLite (por enquanto)

## 🏗️ Arquitetura do Projeto

O projeto segue os princípios da **Arquitetura Limpa (Clean Architecture)**, que prioriza a separação de responsabilidades e a independência das camadas. Essa abordagem garante que a lógica de negócio do sistema não dependa da camada de UI ou de detalhes de implementação do banco de dados, tornando a aplicação mais robusta, testável e fácil de manter.

A estrutura do projeto é dividida nas seguintes camadas:

-   **WebUI (Camada de Apresentação):** Implementada com **ASP.NET Core MVC**, esta camada é responsável por gerenciar as interações do usuário, processar requisições HTTP pelos `Controllers` e renderizar as `Views`.
-   **Application (Camada de Aplicação):** Contém a lógica de negócio da aplicação. É aqui que os `Services` residem, orquestrando as operações de dados, aplicando as regras de negócio e coordenando as interações entre as entidades do domínio e os repositórios. Também utiliza **AutoMapper** para a conversão de `DTOs`.
-   **Domain (Camada de Domínio):** O núcleo da aplicação. Esta camada é totalmente independente e contém as entidades e as regras de negócio essenciais, garantindo que o modelo de domínio seja consistente e protegido.
-   **Infra (Camada de Infraestrutura):** Responsável pelos detalhes de implementação, como o acesso a dados. É nesta camada que o **Entity Framework Core (EF Core)** é utilizado para interagir com o banco de dados através dos `Repositories`.

## 📌 Entidades Principais

O modelo de domínio é construído com as seguintes entidades:

-   `BankAccount`: Representa as contas financeiras do usuário, como contas bancárias e carteira.
-   `Income`: Modelagem de todas as receitas, com detalhes como valor, descrição, categoria e conta associada.
-   `Expense`: Representa as despesas, com informações como valor, descrição, categoria e a conta da qual o valor foi debitado.
-   `CreditCard`: Entidade para o gerenciamento de cartões de crédito, permitindo rastrear despesas e faturas.
-   `CardExpense`: Representa as despesas realizadas com um cartão de crédito específico.
-   `Invoice`: Entidade para o controle de faturas de cartão de crédito.
-   `InvoicePayment`: Detalha os pagamentos realizados para as faturas.
-   `Transfer`: Permite registrar a transferência de valores entre duas `BankAccounts` distintas.

## 🏁 Como Rodar o Projeto

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/PdrVin/FinanceControl.git
    cd seu-repositorio
    ```

2.  **Configurar o Banco de Dados:**
    -   Atualize a `connection string` no arquivo `appsettings.json`.
    -   Execute as migrações do Entity Framework Core:
        ```bash
        dotnet ef database update
        ```

3.  **Executar a aplicação:**
    ```bash
    dotnet run --project src/WebUI
    ```
    A aplicação estará disponível em `https://localhost:XXXX` (a porta padrão do seu projeto).

## 🤝 Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir uma _issue_ ou enviar um _pull request_.

## 📄 Licença

Este projeto está sob a licença [MIT](LICENSE).