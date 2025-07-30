namespace WebUI.Configuration;

public static class RouteConfig
{
    public static void ConfigureRoutes(IEndpointRouteBuilder endpoints)
    {
        // Rota para o Dashboard (ponto de entrada principal)
        endpoints.MapControllerRoute(
            name: "Dashboard",
            pattern: "Dashboard/{action=Index}/{id?}",
            defaults: new { controller = "Home", action = "Index" }
        );

        // Rota para as Contas (BankAccounts)
        endpoints.MapControllerRoute(
            name: "BankAccount",
            pattern: "Contas/{action=Index}/{id?}",
            defaults: new { controller = "BankAccount", action = "Index" }
        );

        // Rota para as Transações (Transactions)
        endpoints.MapControllerRoute(
            name: "Transaction",
            pattern: "Transacoes/{action=Index}/{year?}/{month?}/{page?}/{pageSize?}",
            defaults: new { controller = "Transaction", action = "Index" }
        );

        // Rotas para as ações de criação de diferentes tipos de transação
        // (Se você tiver [HttpGet("Create")] em cada controller, estas podem ser redundantes,
        // mas são incluídas para centralização conforme solicitado)
        endpoints.MapControllerRoute(
            name: "CreateExpense",
            pattern: "Expense/Create",
            defaults: new { controller = "Expense", action = "Create" }
        );
        endpoints.MapControllerRoute(
            name: "CreateRecipe",
            pattern: "Recipe/Create",
            defaults: new { controller = "Recipe", action = "Create" }
        );
        endpoints.MapControllerRoute(
            name: "CreateCardExpense",
            pattern: "CardExpense/Create",
            defaults: new { controller = "CardExpense", action = "Create" }
        );
        endpoints.MapControllerRoute(
            name: "CreateTransfer",
            pattern: "Transfer/Create",
            defaults: new { controller = "Transfer", action = "Create" }
        );

        // Rota Padrão (Default) - Deve ser a última para evitar conflitos com rotas mais específicas
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Rota de Fallback - Para capturar URLs não mapeadas (útil para SPAs ou erros 404 customizados)
        // Geralmente aponta para a página inicial ou um controlador de erro.
        endpoints.MapFallbackToController("Index", "Home");
    }
}