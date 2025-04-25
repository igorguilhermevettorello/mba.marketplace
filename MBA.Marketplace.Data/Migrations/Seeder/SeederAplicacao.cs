using MBA.Marketplace.Data.Data;
using MBA.Marketplace.Data.Entities;
using MBA.Marketplace.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MBA.Marketplace.Data.Migrations.Seeder
{
    public static class SeederAplicacao
    {
        public static async Task SeedVendedorComUsuarioAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            Guid vendedorIdUm = Guid.NewGuid();
            string email = "vendedor1@email.com";
            string senha = "SenhaForte123!";

            // Verifica se o usuário já existe
            var usuarioExistente = await userManager.FindByEmailAsync(email);

            if (usuarioExistente == null)
            {
                var novoUsuario = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var resultado = await userManager.CreateAsync(novoUsuario, senha);

                if (resultado.Succeeded)
                {
                    // Cria o vendedor associado ao novo usuário
                    var novoVendedor = new Vendedor
                    {
                        Id = vendedorIdUm,
                        Nome = "Loja Exemplo Um",
                        UsuarioId = novoUsuario.Id,
                        Email = email
                    };

                    context.Vendedores.Add(novoVendedor);
                    await context.SaveChangesAsync();
                }
                else
                {
                    var erros = string.Join(", ", resultado.Errors.Select(e => e.Description));
                    Console.WriteLine($"Erro ao criar usuário: {erros}");
                }
            }

            Guid vendedorIdDois = Guid.NewGuid();
            string emailDois = "vendedor2@email.com";
            string senhaDois = "SenhaForte123!";

            // Verifica se o usuário já existe
            var usuarioExistenteDois = await userManager.FindByEmailAsync(emailDois);

            if (usuarioExistenteDois == null)
            {
                var novoUsuario = new ApplicationUser
                {
                    UserName = emailDois,
                    Email = emailDois,
                    EmailConfirmed = true
                };

                var resultado = await userManager.CreateAsync(novoUsuario, senhaDois);

                if (resultado.Succeeded)
                {
                    // Cria o vendedor associado ao novo usuário
                    var novoVendedor = new Vendedor
                    {
                        Id = vendedorIdDois,
                        Nome = "Loja Exemplo Dois",
                        UsuarioId = novoUsuario.Id,
                        Email = emailDois
                    };

                    context.Vendedores.Add(novoVendedor);
                    await context.SaveChangesAsync();
                }
                else
                {
                    var erros = string.Join(", ", resultado.Errors.Select(e => e.Description));
                    Console.WriteLine($"Erro ao criar usuário: {erros}");
                }
            }

            Guid eletronicoId = Guid.NewGuid();
            Guid roupaId = Guid.NewGuid();
            Guid livroId = Guid.NewGuid();
            if (!context.Categorias.Any())
            {
                context.Categorias.AddRange(
                    new Categoria { Id = eletronicoId, Nome = "Eletrônicos", Descricao = "Eletrônicos em geral", CreatedAt = DateTime.Now },
                    new Categoria { Id = roupaId, Nome = "Roupas", Descricao = "Roupas em geral", CreatedAt = DateTime.Now },
                    new Categoria { Id = livroId, Nome = "Livros", Descricao = "Livros em geral", CreatedAt = DateTime.Now }
                );
            }

            if (!context.Produtos.Any())
            {
                var agora = DateTime.Now;
                context.Produtos.AddRange(
                    new Produto
                    {
                        Id = Guid.NewGuid(),
                        Nome = "Smartphone X100",
                        Descricao = "Smartphone de última geração com câmera de 108MP",
                        Imagem = "/imagens/smartphone.jpg",
                        Preco = 2999.90m,
                        Estoque = 15,
                        CategoriaId = eletronicoId,
                        VendedorId = vendedorIdUm,
                        CreatedAt = agora,
                        UpdatedAt = agora
                    },
                    new Produto
                    {
                        Id = Guid.NewGuid(),
                        Nome = "Camisa Social Masculina",
                        Descricao = "Camisa social de algodão premium",
                        Imagem = "/imagens/camisa.jpg",
                        Preco = 129.90m,
                        Estoque = 40,
                        CategoriaId = roupaId,
                        VendedorId = vendedorIdDois,
                        CreatedAt = agora,
                        UpdatedAt = agora
                    },
                    new Produto
                    {
                        Id = Guid.NewGuid(),
                        Nome = "Livro de Design de Software",
                        Descricao = "Um guia completo sobre padrões e arquitetura de software",
                        Imagem = "/imagens/livro.jpg",
                        Preco = 89.90m,
                        Estoque = 25,
                        CategoriaId = livroId,
                        VendedorId = vendedorIdUm,
                        CreatedAt = agora,
                        UpdatedAt = agora
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
