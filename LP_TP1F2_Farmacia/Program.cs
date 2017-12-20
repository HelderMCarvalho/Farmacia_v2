﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    Ações da Farmácia:
    - Respeitar a fila de atendimento;
    - Verificar validades.

    Detalhes:
    Criar as classes, variáveis e structs necessárias para modelar:
    - Medicamentos:
        Existem depois os vários tipos de medicamentos:
            - opiáceos que só podem ser levantados ao máximo 5 por semana (se na receita forem receitados 20, quer dizer que
            só ao fim de 4 semanas aquela  parte da receita pode ser de facto levantada totalmente);
    - Receita que é basicamente mas que só acaba quando todos os medicamentos forem levantados;
    - Todos os dados têm de ser carregados a partir de ficheiros. Como tal também devem haver métodos que permitam guardar 
    os dados em ficheiros;
    - Tem de ser estabelecidas filas para atender os clientes.
*/

namespace LP_TP1F2_Farmacia
{
    class Pessoa
    {
        protected int id;
        protected string nome;

        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }

        public Pessoa(int id, string nome)
        {
            this.id = id;
            this.nome = nome;
        }
    }

    class Funcionario : Pessoa
    {
        private string tipo; //Chefe ou Base

        public string Tipo { get => tipo; set => tipo = value; }

        public Funcionario(string tipo, int id, string nome) : base(id, nome)
        {
            this.tipo = tipo;
        }
    }

    class Cliente : Pessoa
    {
        private float dinheiro;
        private List<Receita> receitas;
        private bool cartaoFarmacias;
        private float conta;
        private int causaAnimal;

        public float Dinheiro { get => dinheiro; set => dinheiro = value; }
        public List<Receita> Receitas { get => receitas; set => receitas = value; }
        public bool CartaoFarmacias { get => cartaoFarmacias; set => cartaoFarmacias = value; }
        public float Conta { get => conta; set => conta = value; }
        public int CausaAnimal { get => causaAnimal; set => causaAnimal = value; }

        public Cliente(float dinheiro, List<Receita> receitas, bool cartaoFarmacias, float conta, int causaAnimal, int id, string nome) : base(id, nome)
        {
            this.dinheiro = dinheiro;
            this.receitas = receitas;
            this.cartaoFarmacias = cartaoFarmacias;
            this.conta = conta;
            this.causaAnimal = causaAnimal;
        }

        /// <summary>
        /// Recebe a Farmácia e a lista de Produtos encomendados
        /// Soma o total a pagar dos Produtos encomendados e adiciona as respetivas taxas
        /// Se o Cliente tiver dinheiro paga, se não tiver aparece a respetiva mensagem
        /// </summary>
        /// <param name="farmacia">Farmácia que vai vender os Produtos</param>
        /// <param name="encomenda">Lista de Produtos que vão ser comprados pelo Cliente</param>
        /// <param name="isReceita">Bool que representa se os Produtos vão ser comprados por Receita</param>
        public void pagar(Farmacia farmacia, List<Produto> encomenda, bool isReceita = false)
        {
            float totalPagar = 0;
            int contAnimal = 0;
            foreach (Produto produto in encomenda)
            {
                int quantidadeProduto = 0;
                foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                {
                    quantidadeProduto += validadeQuantidade.Quantidade;
                }
                if ((produto.SubCategoria== "AntiInflamatorio") || (produto.SubCategoria== "AntiSeptico"))
                {
                    float precoTemp = produto.Preco;
                    precoTemp += produto.Preco * 0.01f;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                }
                else if (produto.SubCategoria == "Injecao")
                {
                    float precoTemp = produto.Preco;
                    precoTemp += 1;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                }
                else if (produto.SubCategoria == "Higiene")
                {
                    float precoTemp = produto.Preco;
                    precoTemp += produto.Preco * 0.13f;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                }
                else if (produto.SubCategoria == "Hipoalergenico")
                {
                    float precoTemp = produto.Preco;
                    precoTemp += produto.Preco * 0.06f;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                }
                else if (produto.SubCategoria == "Animal")
                {
                    float precoTemp = produto.Preco;
                    precoTemp += 1;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                    contAnimal += quantidadeProduto;
                }
                else if (produto.SubCategoria == "Beleza")
                {
                    float precoTemp = produto.Preco;
                    precoTemp += produto.Preco * 0.23f;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                }
                else
                {
                    float precoTemp = produto.Preco;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                }
            }
            if (dinheiro >= totalPagar)
            {
                foreach (Produto produto in encomenda)
                {
                    int quantidadeProduto = 0;
                    foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                    {
                        quantidadeProduto += validadeQuantidade.Quantidade;
                    }
                    farmacia.retiraDoStock(produto.Id, quantidadeProduto);
                }
                dinheiro -= totalPagar;
                farmacia.CausaAnimal += contAnimal;
                farmacia.Dinheiro += (totalPagar - contAnimal);
                farmacia.ContadorVentas++;
                Venda venda = new Venda(farmacia.ContadorVentas, id, encomenda, totalPagar, false);
                farmacia.Vendas.Add(venda);
                Console.WriteLine("\nCompra efetuada com sucesso.");
                Console.WriteLine("O seu código de venda é: " + farmacia.ContadorVentas);
            }
            else
            {
                Console.Write("\nNão tem dinheiro suficiente.\nDeseja adicionar á conta ou cancelar a compra? (0 - Adicionar á conta | 1 - Cancelar): ");
                string pagarCancelar = Console.ReadLine();
                int pagarCancelarInt = Int32.Parse(pagarCancelar);
                if (pagarCancelarInt == 0)
                {
                    adicionarConta(farmacia, encomenda, isReceita);
                }
                else
                {
                    Console.WriteLine("\nCompra cancelada com sucesso!");
                }
            }
        }

        /// <summary>
        /// Recebe a Farmácia e a lista de Produtos encomendados
        /// Soma o total a pagar dos Produtos encomendados
        /// Se a conta do Cliente não exceder os 50€ a venda é criada e adicionado o valor é conta, senão o Cliente paga na hora ou cancela
        /// </summary>
        /// <param name="farmacia">Farmácia que vai vender os Produtos</param>
        /// <param name="encomenda">Lista de Produtos que vão ser comprados pelo Cliente</param>
        /// <param name="isReceita">Bool que representa se os Produtos vão ser comprados por Receita</param>
        public void adicionarConta(Farmacia farmacia, List<Produto> encomenda, bool isReceita = false)
        {
            float totalPagar = 0;
            int contAnimal = 0;
            foreach (Produto produto in encomenda)
            {
                int quantidadeProduto = 0;
                foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                {
                    quantidadeProduto += validadeQuantidade.Quantidade;
                }
                if ((produto.SubCategoria == "AntiInflamatorio") || (produto.SubCategoria == "AntiSeptico"))
                {
                    float precoTemp = produto.Preco;
                    precoTemp += produto.Preco * 0.01f;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                }
                else if (produto.SubCategoria == "Injecao")
                {
                    float precoTemp = produto.Preco;
                    precoTemp += 1;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                }
                else if (produto.SubCategoria == "Higiene")
                {
                    float precoTemp = produto.Preco;
                    precoTemp += produto.Preco * 0.13f;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                }
                else if (produto.SubCategoria == "Hipoalergenico")
                {
                    float precoTemp = produto.Preco;
                    precoTemp += produto.Preco * 0.06f;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                }
                else if (produto.SubCategoria == "Animal")
                {
                    float precoTemp = produto.Preco;
                    precoTemp += 1;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                    contAnimal += quantidadeProduto;
                }
                else if (produto.SubCategoria == "Beleza")
                {
                    float precoTemp = produto.Preco;
                    precoTemp += produto.Preco * 0.23f;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                }
                else
                {
                    float precoTemp = produto.Preco;
                    if (isReceita && produto.Comparticipacao) { precoTemp -= produto.Preco * 0.05f; }
                    if (cartaoFarmacias) { precoTemp -= produto.Preco * 0.05f; }
                    produto.Preco = precoTemp;
                    totalPagar += produto.Preco * quantidadeProduto;
                }
            }
            if ((conta + totalPagar) < 50)
            {
                foreach (Produto produto in encomenda)
                {
                    int quantidadeProduto = 0;
                    foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                    {
                        quantidadeProduto += validadeQuantidade.Quantidade;
                    }
                    farmacia.retiraDoStock(produto.Id, quantidadeProduto);
                }
                conta += (totalPagar - contAnimal);
                causaAnimal += contAnimal;
                farmacia.ContadorVentas++;
                Venda venda = new Venda(farmacia.ContadorVentas, id, encomenda, totalPagar, false);
                farmacia.Vendas.Add(venda);
                Console.WriteLine("\nCompra adicionada com sucesso á conta.");
                Console.WriteLine("O seu código de venda é: " + farmacia.ContadorVentas);
            }
            else
            {
                Console.Write("\nNão pode adicionar á conta porque a mesma excede os 50 euros.\nDeseja pagar agora ou cancelar a compra? (0 - Pagar agora | 1 - Cancelar): ");
                string pagarCancelar = Console.ReadLine();
                int pagarCancelarInt = Int32.Parse(pagarCancelar);
                if (pagarCancelarInt == 0)
                {
                    pagar(farmacia, encomenda, isReceita);
                }
                else
                {
                    Console.WriteLine("\nCompra cancelada com sucesso!");
                }
            }
        }

        /// <summary>
        /// Paga o valor que o Cliente tem em conta (caso tenha dinheiro para tal)
        /// </summary>
        /// <param name="farmacia">Farmácia à qual vai ser paga a conta</param>
        public void pagarConta(Farmacia farmacia)
        {
            if (dinheiro >= (conta + causaAnimal))
            {
                farmacia.Dinheiro += conta;
                farmacia.CausaAnimal += causaAnimal;
                dinheiro -= (conta + causaAnimal);
                conta = causaAnimal = 0;
            }
            else
            {
                Console.WriteLine("\nNão tem dinheiro para pagar o que deve!");
            }
        }

        /// <summary>
        /// Verifica se uma Receita existe
        /// </summary>
        /// <param name="idReceita">Int com o Id da Receita que vai ser testada</param>
        /// <returns>Bool onde 1 - Existe e 0 - Não existe</returns>
        public bool existeReceita(int idReceita)
        {
            bool existe = false;
            foreach (Receita receita in receitas)
            {
                if (receita.Codigo == idReceita)
                {
                    existe = true;
                    break;
                }
            }
            return existe;
        }

        /// <summary>
        /// Recebe o código da Receita e devolve o objeto Receita desse código
        /// </summary>
        /// <param name="idReceita">Int com o Id da Receita</param>
        /// <returns>Objeto Receita</returns>
        public Receita obterReceita(int idReceita)
        {
            Receita receitaAtual = null;
            foreach (Receita receita in receitas)
            {
                if (receita.Codigo == idReceita)
                {
                    receitaAtual = receita;
                    break;
                }
            }
            return receitaAtual;
        }

        /// <summary>
        /// Recebe a Farmácia e a lista de Produtos a ser devolvidos
        /// Soma o total a devolver dos Produtos a ser devolvidos
        /// Repõem os Produtos devolvidos no stock da Farmácia
        /// </summary>
        /// <param name="farmacia">Farmácia que vai receber a devolução</param>
        /// <param name="devolucao">Lista de Produtos que vão ser devolvidos</param>
        /// <param name="idVenda">Int com o Id da Venda que vao ter Produtos devolvidos</param>
        public void devolver(Farmacia farmacia, List<Produto> devolucao, int idVenda)
        {
            Venda venda = farmacia.obterVenda(idVenda);
            int contAnimal = 0;
            float totalReceber = 0;
            foreach (Produto produtoDevolucao in devolucao)
            {
                foreach (Produto produtoVenda in venda.Produtos)
                {
                    totalReceber += (produtoVenda.Preco * produtoDevolucao.ValidadesQuantidades.First().Quantidade);
                    for(int i = (produtoVenda.ValidadesQuantidades.Count()-1); i >= 0; i--)
                    {
                        produtoVenda.ValidadesQuantidades[i].Quantidade -= produtoDevolucao.ValidadesQuantidades.First().Quantidade;
                        if (produtoVenda.ValidadesQuantidades[i].Quantidade < 0)
                        {
                            int auxQuantidade = produtoDevolucao.ValidadesQuantidades.First().Quantidade;
                            produtoDevolucao.ValidadesQuantidades.First().Quantidade = (produtoVenda.ValidadesQuantidades[i].Quantidade * (-1));
                            produtoVenda.ValidadesQuantidades[i].Quantidade = 0;
                            auxQuantidade -= produtoDevolucao.ValidadesQuantidades.First().Quantidade;
                            farmacia.reporStock(farmacia.obterProduto(produtoDevolucao.Id), auxQuantidade, produtoVenda.ValidadesQuantidades[i].Validade);
                        }
                        else
                        {
                            farmacia.reporStock(farmacia.obterProduto(produtoDevolucao.Id), produtoDevolucao.ValidadesQuantidades.First().Quantidade, produtoVenda.ValidadesQuantidades[i].Validade);
                        }
                    }
                    if (produtoDevolucao.SubCategoria == "Animal")
                    {
                        contAnimal += produtoDevolucao.ValidadesQuantidades.First().Quantidade;
                    }
                }
            }
            dinheiro += totalReceber;
            venda.TotalPago -= totalReceber;
            farmacia.Dinheiro -= (totalReceber - contAnimal);
            farmacia.CausaAnimal -= contAnimal;
            Console.WriteLine("\nDevolução efetuada com sucesso!");
        }
    }

    class Receita
    {
        private int codigo;
        private List<Produto> produtos;
        private bool entregue;

        public int Codigo { get => codigo; set => codigo = value; }
        public List<Produto> Produtos { get => produtos; set => produtos = value; }
        public bool Entregue { get => entregue; set => entregue = value; }

        public Receita(int codigo, List<Produto> produtos, bool entregue)
        {
            this.codigo = codigo;
            this.produtos = produtos;
            this.entregue = entregue;
        }
    }

    class ValidadeQuantidade
    {
        private int quantidade;
        private DateTime validade;

        public int Quantidade { get => quantidade; set => quantidade = value; }
        public DateTime Validade { get => validade; set => validade = value; }

        public ValidadeQuantidade(int quantidade, DateTime validade)
        {
            this.quantidade = quantidade;
            this.validade = validade;
        }
    }

    class Produto
    {
        private int id;
        private string nome;
        private float preco;
        private bool comparticipacao;
        private List<ValidadeQuantidade> validadesQuantidades;
        private string descrição;
        private string categoria; //M, HA, B
        private string subCategoria; //Sub-categoria

        public string Nome { get => nome; set => nome = value; }
        public float Preco { get => preco; set => preco = value; }
        public bool Comparticipacao { get => comparticipacao; set => comparticipacao = value; }
        public int Id { get => id; set => id = value; }
        public string Descrição { get => descrição; set => descrição = value; }
        public string Categoria { get => categoria; set => categoria = value; }
        public string SubCategoria { get => subCategoria; set => subCategoria = value; }
        public List<ValidadeQuantidade> ValidadesQuantidades { get => validadesQuantidades; set => validadesQuantidades = value; }

        public Produto(int id, string nome, float preco, bool comparticipacao, List<ValidadeQuantidade> validadesQuantidades, string descrição, string categoria, string subCategoria)
        {
            this.id = id;
            this.nome = nome;
            this.preco = preco;
            this.comparticipacao = comparticipacao;
            this.validadesQuantidades = validadesQuantidades;
            this.descrição = descrição;
            this.categoria = categoria;
            this.subCategoria = subCategoria;
        }
    }

    class Farmacia
    {
        private List<Funcionario> funcionarios;
        private List<Cliente> clientes;
        private List<Produto> produtos;
        private int contadorVentas;
        private List<Venda> vendas;
        private float dinheiro;
        private DateTime data;
        private float causaAnimal;

        public List<Funcionario> Funcionarios { get => funcionarios; set => funcionarios = value; }
        public List<Cliente> Clientes { get => clientes; set => clientes = value; }
        public List<Produto> Medicamentos { get => produtos; set => produtos = value; }
        public float Dinheiro { get => dinheiro; set => dinheiro = value; }
        public List<Venda> Vendas { get => vendas; set => vendas = value; }
        public int ContadorVentas { get => contadorVentas; set => contadorVentas = value; }
        public DateTime Data { get => data; set => data = value; }
        public float CausaAnimal { get => causaAnimal; set => causaAnimal = value; }

        public Farmacia(List<Funcionario> funcionarios, List<Cliente> clientes, List<Produto> produtos, int contadorVentas, List<Venda> vendas, float dinheiro, DateTime data, float causaAnimal)
        {
            this.funcionarios = funcionarios;
            this.clientes = clientes;
            this.produtos = produtos;
            this.contadorVentas = contadorVentas;
            this.vendas = vendas;
            this.dinheiro = dinheiro;
            this.data = data;
            this.causaAnimal = causaAnimal;
        }

        /// <summary>
        /// Recebe o ID do Cliente e devolve um Objeto Cliente desse Id ou devolve um Objeto Cliente = null caso não exista
        /// </summary>
        /// <param name="idCliente">Int com o Id do Cliente que vai ser devolvido</param>
        /// <returns>Objeto Cliente</returns>
        public Cliente obterCliente(int idCliente)
        {
            Cliente clienteAtual = null;
            foreach (Cliente cliente in Clientes)
            {
                if (idCliente == cliente.Id)
                {
                    clienteAtual = cliente;
                    break;
                }
            }
            return clienteAtual;
        }

        /// <summary>
        /// Recebe o ID do Funcionário e devolve um Objeto Funcionario desse Id ou devolve um Objeto Funcionario = null caso não exista
        /// </summary>
        /// <param name="idFuncionario">Int com o Id do Funcionário que vai ser devolvido</param>
        /// <returns>Objeto Funcionário</returns>
        public Funcionario obterFuncionario(int idFuncionario)
        {
            Funcionario funcionarioAtual = null;
            foreach (Funcionario funcionario in Funcionarios)
            {
                if (idFuncionario == funcionario.Id)
                {
                    funcionarioAtual = funcionario;
                    break;
                }
            }
            return funcionarioAtual;
        }

        /// <summary>
        /// Lista todos os Produtos em stock
        /// </summary>
        public void mostrarProdutos()
        {
            Console.Clear();
            Console.WriteLine("Lista de produtos:\n");
            foreach (Produto produto in produtos)
            {
                int stock = 0;
                foreach(ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                {
                    if(validadeQuantidade.Validade >= data)
                    {
                        stock += validadeQuantidade.Quantidade;
                    }
                }
                if (stock > 0)
                {
                    Console.WriteLine(produto.Id + " - " + produto.Nome + " - " + produto.Preco + " euros" + " - " + produto.Descrição);
                }
            }
        }

        /// <summary>
        /// Verifica se um determinado Produto existe
        /// </summary>
        /// <param name="idProduto">Int dom o Id do Produto que vai ser testado</param>
        /// <returns>Bool onde 1 - Existe e 0 - Não existe</returns>
        public bool existeProduto(int idProduto)
        {
            bool existe = false;
            foreach (Produto produto in produtos)
            {
                if (idProduto == produto.Id)
                {
                    existe = true;
                    break;
                }
            }
            return existe;
        }

        /// <summary>
        /// Verifica se um determinado Produto existe em stock numa determinada quantidade
        /// </summary>
        /// <param name="idMedicamento">Int com o Id do Medicamento que vai ser testado</param>
        /// <param name="quantidade">Int com a Quantidade que vai ser testada</param>
        /// <returns>Bool onde 1 - Existe em stock e 0 - Não existe em stock</returns>
        public bool existeQuantidade(int idMedicamento, int quantidade)
        {
            bool existe = false;
            if (existeProduto(idMedicamento))
            {
                foreach (Produto produto in produtos)
                {
                    if (produto.Id == idMedicamento)
                    {
                        int stock = 0;
                        foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                        {
                            if (validadeQuantidade.Validade >= data)
                            {
                                stock += validadeQuantidade.Quantidade;
                            }
                        }
                        if (stock >= quantidade)
                        {
                            existe = true;
                            break;
                        }
                    }
                }
            }
            return existe;
        }

        /// <summary>
        /// Recebe o ID do Produto e devolve o objeto Produto desse Id
        /// </summary>
        /// <param name="idProduto">Int com o Id do Produto que vai ser devolvido</param>
        /// <returns>Objeto Produto</returns>
        public Produto obterProduto(int idProduto)
        {
            Produto produtoFinal = null;
            foreach (Produto produto in produtos)
            {
                if (idProduto == produto.Id)
                {
                    produtoFinal = produto;
                    break;
                }
            }
            return produtoFinal;
        }

        /// <summary>
        /// Retira do stock uma certa quantidade de Produtos
        /// </summary>
        /// <param name="idProduto">Int com o Id do Produto que vai ser retirado do stock</param>
        /// <param name="quantidade">Int com a Quantidade que vai ser retirada</param>
        public void retiraDoStock(int idProduto, int quantidade)
        {
            if (existeQuantidade(idProduto, quantidade))
            {
                foreach (Produto produto in produtos)
                {
                    if (idProduto == produto.Id)
                    {
                        foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                        {
                            if (validadeQuantidade.Validade >= data)
                            {
                                if (validadeQuantidade.Quantidade >= quantidade)
                                {
                                    validadeQuantidade.Quantidade -= quantidade;
                                    break;
                                }
                                else
                                {
                                    validadeQuantidade.Quantidade -= quantidade;
                                    quantidade = (validadeQuantidade.Quantidade * (-1));
                                    validadeQuantidade.Quantidade = 0;
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Precorre as ValidadesQuantidades de um Produto e devolve uma Lista de ValidadeQuantidade para a opção "Comprar Produtos"
        /// Função criada apenas para tornar a devolução de Produtos mais fácil. Sem esta função, não era possivel saber que data de validade tem o produto que vai ser devolvido
        /// </summary>
        /// <param name="idProduto">Id do Produto que vai ser adicionado ao carrinho</param>
        /// <param name="quantidadeProduto">quantidade que vai ser adicionada ao carrinho</param>
        /// <returns>Lista de ValidadeQuantidade</returns>
        public List<ValidadeQuantidade> ValidadeQuantidadeParaCompra(int idProduto, int quantidadeProduto)
        {
            Produto produto = obterProduto(idProduto);
            List<ValidadeQuantidade> validadesQuantidadesCompra = new List<ValidadeQuantidade>();
            foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
            {
                if (validadeQuantidade.Validade >= data)
                {
                    if (validadeQuantidade.Quantidade >= quantidadeProduto)
                    {
                        ValidadeQuantidade validadeQuantidadeAdicionarCompra = new ValidadeQuantidade(quantidadeProduto, validadeQuantidade.Validade);
                        validadesQuantidadesCompra.Add(validadeQuantidadeAdicionarCompra);
                        break;
                    }
                    else
                    {
                        ValidadeQuantidade validadeQuantidadeAdicionarCompra = new ValidadeQuantidade(validadeQuantidade.Quantidade, validadeQuantidade.Validade);
                        validadesQuantidadesCompra.Add(validadeQuantidadeAdicionarCompra);
                        quantidadeProduto -= validadeQuantidade.Quantidade;
                    }
                }
            }
            return validadesQuantidadesCompra;
        }

        /// <summary>
        /// Verifica se uma determinada Venda existe
        /// </summary>
        /// <param name="idVenda">Int com o Id da Venda que vai ser testada</param>
        /// <returns>Bool onde 1 - Existe e 0 - Não existe</returns>
        public bool existeVenda(int idVenda)
        {
            bool existe = false;
            foreach (Venda venda in vendas)
            {
                if (idVenda == venda.Id)
                {
                    existe = true;
                    break;
                }
            }
            return existe;
        }

        /// <summary>
        /// Lista os Produtos guardados numa Venda
        /// </summary>
        /// <param name="idVenda">Int com o Id da Venda cujos Produtos vão ser listados</param>
        public void mostrarProdutosDaVenda(int idVenda)
        {
            Venda venda = obterVenda(idVenda);
            if (existeVenda(idVenda))
            {
                Console.Clear();
                Console.WriteLine("Lista de produtos:\n");
                foreach (Produto produto in venda.Produtos)
                {
                    int quantidade = 0;
                    foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                    {
                        quantidade += validadeQuantidade.Quantidade;
                    }
                    Console.WriteLine(produto.Id + " - " + produto.Nome + " - " + produto.Preco + " euros - " + quantidade + " unidades");
                }
            }
        }

        /// <summary>
        /// Devolve um Objeto Venda a partir de um Id de Venda recebido
        /// </summary>
        /// <param name="idVenda">Int com o Id da Venda que vai ser devolvida</param>
        /// <returns>Objeto Venda</returns>
        public Venda obterVenda(int idVenda)
        {
            Venda vendaFinal = null;
            foreach (Venda venda in vendas)
            {
                if (venda.Id == idVenda)
                {
                    vendaFinal = venda;
                    break;
                }
            }
            return vendaFinal;
        }

        /// <summary>
        /// Recebe o Id da venda, do produto e a quantidade para ver se é possivel devolver essa quantidade desse produto nessa venda
        /// </summary>
        /// <param name="idVenda">Int com o Id da Venda que vai ter Produtos devolvidos</param>
        /// <param name="idProduto">Int com o Id do Produto que vai ser devolvido</param>
        /// <param name="quantidade">Int com a quantidade a ser testada</param>
        /// <returns>Bool onde 1 - Existe a quantidade na Venda e 0 - Não existe a quantidade na Venda</returns>
        public bool existeQuantidadeNaVenda(int idVenda, int idProduto, int quantidade)
        {
            bool existe = false;
            Venda venda = obterVenda(idVenda);
            foreach (Produto produto in venda.Produtos)
            {
                int quantidadeVenda = 0;
                foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                {
                    quantidadeVenda += validadeQuantidade.Quantidade;
                }

                if ((quantidade <= quantidadeVenda) && (produto.Id == idProduto))
                {
                    existe = true;
                    break;
                }
            }
            return existe;
        }

        /// <summary>
        /// Recebe um Objeto Produto, cria uma nova "ValidadeQuantidade" e adiciona-a a esse preoduto
        /// </summary>
        /// <param name="produto">Objeto Produto que vai ter o stock reposto</param>
        /// <param name="quantidadeAdicionar">Int da quantidade que vai ser reposta no stock</param>
        public void reporStock(Produto produto, int quantidadeAdicionar, DateTime validadeAdicionar)
        {
            bool existe = false;
            foreach(ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
            {
                if (validadeQuantidade.Validade == validadeAdicionar)
                {
                    validadeQuantidade.Quantidade += quantidadeAdicionar;
                    existe = true;
                    break;
                }
            }
            if (existe == false)
            {
                ValidadeQuantidade novaValidadeQuantidade = new ValidadeQuantidade(quantidadeAdicionar, validadeAdicionar);
                produto.ValidadesQuantidades.Add(novaValidadeQuantidade);
            }
        }

        /// <summary>
        /// Percorre os Produtos da Farmácia e calcula o valor total em stock
        /// </summary>
        /// <returns>float com o valor total do stock da Farmácia</returns>
        public float totalProdutos()
        {
            float totalProdutos = 0;
            foreach (Produto produto in produtos)
            {
                foreach(ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                {
                    if (validadeQuantidade.Validade >= data)
                    {
                        totalProdutos += (produto.Preco * validadeQuantidade.Quantidade);
                    }
                }
            }
            return totalProdutos;
        }

        /// <summary>
        /// Percorre os Produtos da Farmácia e calcula o valor total em stock de cada tipo
        /// </summary>
        public void totalProdutosPorTipo()
        {
            float totalOpiacio = 0, totalAntiInflamatorio = 0, totalInjecao = 0, totalHigiene = 0, totalHipoalergenico = 0, totalAnimal = 0, totalBeleza = 0;
            foreach(Produto produto in produtos)
            {
                switch (produto.SubCategoria)
                {
                    case "Opiacio":
                        {
                            foreach(ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                            {
                                if (validadeQuantidade.Validade >= data)
                                {
                                    totalOpiacio += produto.Preco * validadeQuantidade.Quantidade;
                                }
                            }
                            break;
                        }
                    case "AntiInflamatorio":
                        {
                            foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                            {
                                if (validadeQuantidade.Validade >= data)
                                {
                                    totalAntiInflamatorio += produto.Preco * validadeQuantidade.Quantidade;
                                }
                            }
                            break;
                        }
                    case "Injecao":
                        {
                            foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                            {
                                if (validadeQuantidade.Validade >= data)
                                {
                                    totalInjecao += produto.Preco * validadeQuantidade.Quantidade;
                                }
                            }
                            break;
                        }
                    case "Higiene":
                        {
                            foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                            {
                                totalHigiene += produto.Preco * validadeQuantidade.Quantidade;
                            }
                            break;
                        }
                    case "Hipoalergenico":
                        {
                            foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                            {
                                totalHipoalergenico += produto.Preco * validadeQuantidade.Quantidade;
                            }
                            break;
                        }
                    case "Animal":
                        {
                            foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                            {
                                totalAnimal += produto.Preco * validadeQuantidade.Quantidade;
                            }
                            break;
                        }
                    case "Beleza":
                        {
                            foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                            {
                                totalBeleza += produto.Preco * validadeQuantidade.Quantidade;
                            }
                            break;
                        }
                }
            }
            Console.WriteLine("A farmácia tem " + totalOpiacio + " euros em stock de produtos opiácios.");
            Console.WriteLine("A farmácia tem " + totalAntiInflamatorio + " euros em stock de produtos anti-inflamatórios.");
            Console.WriteLine("A farmácia tem " + totalInjecao + " euros em stock de produtos de injeções.");
            Console.WriteLine("A farmácia tem " + totalHigiene + " euros em stock de produtos hijiene.");
            Console.WriteLine("A farmácia tem " + totalHipoalergenico + " euros em stock de produtos hipoalergénicos.");
            Console.WriteLine("A farmácia tem " + totalAnimal + " euros em stock de produtos para animais.");
            Console.WriteLine("A farmácia tem " + totalBeleza + " euros em stock de produtos de beleza.");
        }

        /// <summary>
        /// Lista todos os Produtos (mesmo que não existam em stock)
        /// </summary>
        public void mostrarTodosProdutos()
        {
            Console.Clear();
            Console.WriteLine("Lista de produtos:\n");
            foreach (Produto produto in produtos)
            {
                int stockDentroValidade = 0, stockForaValidade = 0;
                foreach (ValidadeQuantidade validadeQuantidade in produto.ValidadesQuantidades)
                {
                    if (validadeQuantidade.Validade >= data)
                    {
                        stockDentroValidade += validadeQuantidade.Quantidade;
                    }
                    else
                    {
                        stockForaValidade += validadeQuantidade.Quantidade;
                    }
                }
                Console.WriteLine(produto.Id + " - " + produto.Nome + " - " + produto.Preco + " euros" + " - "  + stockDentroValidade + " unidades dentro da validade e " + stockForaValidade + " fora da validade");
            }
        }
    }

    class Venda
    {
        private int id;
        private int idCliente;
        private List<Produto> produtos;
        private float totalPago;
        private bool isReceita;

        public Venda(int id, int idCliente, List<Produto> produtos, float totalPago, bool isReceita)
        {
            this.id = id;
            this.idCliente = idCliente;
            this.produtos = produtos;
            this.totalPago = totalPago;
            this.isReceita = isReceita;
        }

        public int Id { get => id; set => id = value; }
        public int IdCliente { get => idCliente; set => idCliente = value; }
        public List<Produto> Produtos { get => produtos; set => produtos = value; }
        public float TotalPago { get => totalPago; set => totalPago = value; }
        public bool IsReceita { get => isReceita; set => isReceita = value; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Funcionario func1 = new Funcionario("Chefe", 1, "Toninho");
            Funcionario func2 = new Funcionario("Base", 2, "Hédinho");
            Funcionario func3 = new Funcionario("Base", 3, "Carlinhos");
            List<Funcionario> funcionarios = new List<Funcionario>();
            funcionarios.Add(func1);
            funcionarios.Add(func2);
            funcionarios.Add(func3);

            DateTime validade = new DateTime(2018, 12, 25);
            //DateTime validade2 = new DateTime(2019, 12, 25);
            ValidadeQuantidade validadeQuantidade1 = new ValidadeQuantidade(100, validade);
            ValidadeQuantidade validadeQuantidade2 = new ValidadeQuantidade(100, validade);
            ValidadeQuantidade validadeQuantidade3 = new ValidadeQuantidade(100, validade);
            ValidadeQuantidade validadeQuantidade4 = new ValidadeQuantidade(100, validade);
            ValidadeQuantidade validadeQuantidade5 = new ValidadeQuantidade(100, validade);
            ValidadeQuantidade validadeQuantidade6 = new ValidadeQuantidade(100, validade);
            ValidadeQuantidade validadeQuantidade7 = new ValidadeQuantidade(100, validade);
            ValidadeQuantidade validadeQuantidade8 = new ValidadeQuantidade(100, validade);
            ValidadeQuantidade validadeQuantidade9 = new ValidadeQuantidade(100, validade);
            ValidadeQuantidade validadeQuantidade10 = new ValidadeQuantidade(100, validade);
            ValidadeQuantidade validadeQuantidade11 = new ValidadeQuantidade(100, validade);
            ValidadeQuantidade validadeQuantidade12 = new ValidadeQuantidade(100, validade);
            ValidadeQuantidade validadeQuantidade13 = new ValidadeQuantidade(100, validade);
            ValidadeQuantidade validadeQuantidade14 = new ValidadeQuantidade(100, validade);
            //ValidadeQuantidade validadeQuantidade15 = new ValidadeQuantidade(2, validade2);
            List<ValidadeQuantidade> validadesQuantidades1 = new List<ValidadeQuantidade>();
            validadesQuantidades1.Add(validadeQuantidade1);
            //validadesQuantidades1.Add(validadeQuantidade15);
            List<ValidadeQuantidade> validadesQuantidades2 = new List<ValidadeQuantidade>();
            validadesQuantidades2.Add(validadeQuantidade2);
            List<ValidadeQuantidade> validadesQuantidades3 = new List<ValidadeQuantidade>();
            validadesQuantidades3.Add(validadeQuantidade3);
            List<ValidadeQuantidade> validadesQuantidades4 = new List<ValidadeQuantidade>();
            validadesQuantidades4.Add(validadeQuantidade4);
            List<ValidadeQuantidade> validadesQuantidades5 = new List<ValidadeQuantidade>();
            validadesQuantidades5.Add(validadeQuantidade5);
            List<ValidadeQuantidade> validadesQuantidades6 = new List<ValidadeQuantidade>();
            validadesQuantidades6.Add(validadeQuantidade6);
            List<ValidadeQuantidade> validadesQuantidades7 = new List<ValidadeQuantidade>();
            validadesQuantidades7.Add(validadeQuantidade7);
            List<ValidadeQuantidade> validadesQuantidades8 = new List<ValidadeQuantidade>();
            validadesQuantidades8.Add(validadeQuantidade8);
            List<ValidadeQuantidade> validadesQuantidades9 = new List<ValidadeQuantidade>();
            validadesQuantidades9.Add(validadeQuantidade9);
            List<ValidadeQuantidade> validadesQuantidades10 = new List<ValidadeQuantidade>();
            validadesQuantidades10.Add(validadeQuantidade10);
            List<ValidadeQuantidade> validadesQuantidades11 = new List<ValidadeQuantidade>();
            validadesQuantidades11.Add(validadeQuantidade11);
            List<ValidadeQuantidade> validadesQuantidades12 = new List<ValidadeQuantidade>();
            validadesQuantidades12.Add(validadeQuantidade12);
            List<ValidadeQuantidade> validadesQuantidades13 = new List<ValidadeQuantidade>();
            validadesQuantidades13.Add(validadeQuantidade13);
            List<ValidadeQuantidade> validadesQuantidades14 = new List<ValidadeQuantidade>();
            validadesQuantidades14.Add(validadeQuantidade14);

            Produto prod1 = new Produto(1, "Benuron", 5.0f, true, validadesQuantidades1, "Opiácio", "M", "Opiacio");
            Produto prod2 = new Produto(2, "Brufen", 6.0f, true, validadesQuantidades2, "Opiácio", "M", "Opiacio");
            Produto prod3 = new Produto(3, "Antiflan", 7.0f, true, validadesQuantidades3, "Anti-Inflamatório", "M", "AntiInflamatorio");
            Produto prod4 = new Produto(4, "Ceprofen", 8.0f, true, validadesQuantidades4, "Anti-Inflamatório", "M", "AntiInflamatorio");
            Produto prod5 = new Produto(5, "Vacina", 9.0f, true, validadesQuantidades5, "Injeções", "M", "Injecao");
            Produto prod6 = new Produto(6, "Noregyna", 10.0f, true, validadesQuantidades6, "Injeções", "M", "Injecao");
            Produto prod7 = new Produto(7, "Colgate", 11.0f, false, validadesQuantidades7, "Pasta de Dentes", "HA", "Higiene");
            Produto prod8 = new Produto(8, "Linic", 12.0f, false, validadesQuantidades8, "Champô", "HA", "Higiene");
            Produto prod9 = new Produto(9, "Papa s/ Glúten", 13.0f, false, validadesQuantidades9, "Papa sem Glúten", "HA", "Hipoalergenico");
            Produto prod10 = new Produto(10, "Papa s/ Amido", 14.0f, false, validadesQuantidades10, "Papa sem Amido", "HA", "Hipoalergenico");
            Produto prod11 = new Produto(11, "Scalibor", 15.0f, false, validadesQuantidades11, "Desparazitante de animal", "HA", "Animal");
            Produto prod12 = new Produto(12, "Amflee", 16.0f, false, validadesQuantidades12, "Desparazitante de animal", "HA", "Animal");
            Produto prod13 = new Produto(13, "Dove", 17.0f, false, validadesQuantidades13, "Creme hedratante", "B", "Beleza");
            Produto prod14 = new Produto(14, "Nivea", 18.0f, false, validadesQuantidades14, "Creme hedratante", "B", "Beleza");

            List<Produto> produtos = new List<Produto>();
            produtos.Add(prod1);
            produtos.Add(prod2);
            produtos.Add(prod3);
            produtos.Add(prod4);
            produtos.Add(prod5);
            produtos.Add(prod6);
            produtos.Add(prod7);
            produtos.Add(prod8);
            produtos.Add(prod9);
            produtos.Add(prod10);
            produtos.Add(prod11);
            produtos.Add(prod12);
            produtos.Add(prod13);
            produtos.Add(prod14);

            ValidadeQuantidade validadeQuantidadeReceita = new ValidadeQuantidade(2, validade);
            List<ValidadeQuantidade> validadesQuantidadesReceita = new List<ValidadeQuantidade>();
            validadesQuantidadesReceita.Add(validadeQuantidadeReceita);

            Produto prod1Receita = new Produto(1, "Benuron", 5.0f, true, validadesQuantidadesReceita, "Opiácio", "M", "Opiacio");
            Produto prod2Receita = new Produto(3, "Antiflan", 7.0f, true, validadesQuantidadesReceita, "Anti-Inflamatório", "M", "AntiInflamatorio");
            Produto prod3Receita = new Produto(5, "Vacina", 9.0f, true, validadesQuantidadesReceita, "Injeções", "M", "Injecao");
            Produto prod4Receita = new Produto(7, "Colgate", 11.0f, false, validadesQuantidadesReceita, "Pasta de Dentes", "HA", "Higiene");
            Produto prod5Receita = new Produto(9, "Papa s/ Glúten", 13.0f, false, validadesQuantidadesReceita, "Papa sem Glúten", "HA", "Hipoalergenico");
            Produto prod6Receita = new Produto(11, "Scalibor", 15.0f, false, validadesQuantidadesReceita, "Desparazitante de animal", "HA", "Animal");
            Produto prod7Receita = new Produto(13, "Dove", 17.0f, false, validadesQuantidadesReceita, "Creme hedratante", "B", "Beleza");
            List<Produto> produtosReceita = new List<Produto>();
            produtosReceita.Add(prod1Receita);
            produtosReceita.Add(prod2Receita);
            produtosReceita.Add(prod3Receita);
            produtosReceita.Add(prod4Receita);
            produtosReceita.Add(prod5Receita);
            produtosReceita.Add(prod6Receita);
            produtosReceita.Add(prod7Receita);
            Receita receita1 = new Receita(1, produtosReceita, false);
            List<Receita> receitas = new List<Receita>();
            receitas.Add(receita1);

            Cliente clie1 = new Cliente(100.0f, receitas, true, 0.0f, 0, 1, "Rebeca");
            Cliente clie2 = new Cliente(200.0f, receitas, false, 0.0f, 0, 2, "Toninha");
            Cliente clie3 = new Cliente(300.0f, receitas, false, 0.0f, 0, 3, "Ramira");
            List<Cliente> clientes = new List<Cliente>();
            clientes.Add(clie1);
            clientes.Add(clie2);
            clientes.Add(clie3);

            List<Venda> vendas = new List<Venda>();

            DateTime dataFarmacia = new DateTime(2017, 12, 14);
            Farmacia farmacia = new Farmacia(funcionarios, clientes, produtos, 0, vendas, 10000.0f, dataFarmacia, 0.0f);

            Cliente clienteAtual = null;
            Funcionario funcionarioAtual = null;
            bool acabou = false;
            while (!acabou)
            {
                Console.Clear();
                Console.Write("Que tipo de utilizador é? (0 - Cliente ou 1 - Funcionário): ");
                string tipoUtilizador = Console.ReadLine();
                switch (tipoUtilizador)
                {
                    case "0":
                        {
                            while (!acabou)
                            {
                                Console.Write("Introduza o seu código de cliente: ");
                                string id = Console.ReadLine();
                                int idInt = Int32.Parse(id);
                                clienteAtual = farmacia.obterCliente(idInt);
                                if (clienteAtual != null)
                                {
                                    acabou = true;
                                }
                                else
                                {
                                    Console.WriteLine("Número de cilente inválido. Introduza novamente.\n");
                                }
                            }
                            break;
                        }
                    case "1":
                        {
                            while (!acabou)
                            {
                                Console.Write("Introduza o seu código de funcionário: ");
                                string id = Console.ReadLine();
                                int idInt = Int32.Parse(id);
                                funcionarioAtual = farmacia.obterFuncionario(idInt);
                                if (funcionarioAtual != null)
                                {
                                    acabou = true;
                                }
                                else
                                {
                                    Console.WriteLine("Número de funcionário inválido. Introduza novamente.\n");
                                }
                            }
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Opção inválida.");
                            break;
                        }
                }
            }

            acabou = false;
            while (!acabou)
            {
                Console.Clear();
                Console.WriteLine("Bem-vindo à Farmácia WellSir");
                if (clienteAtual != null)
                {
                    Console.WriteLine("\nValor a dever: " + (clienteAtual.Conta + clienteAtual.CausaAnimal) + " euros. Vá para a opção 100 para pagar o que deve.");
                    Console.WriteLine("Fundos angariados para a causa \"Salvem as ratazanas de laboratório\": " + farmacia.CausaAnimal + " euros");
                }
                Console.WriteLine("\n----------MENU----------");
                Console.WriteLine("\nEscolha uma opção:");
                Console.WriteLine("1 - Comprar produtos");
                Console.WriteLine("2 - Mostrar receita");
                Console.WriteLine("3 - Procurar e verificar se existem produtos");
                Console.WriteLine("4 - Devolver produtos");
                Console.WriteLine("5 - Mostrar valor total em produtos no stock");
                Console.WriteLine("6 - Repor stock");
                Console.WriteLine("0 - SAIR");
                Console.Write("\nA sua opção: ");
                string opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "1":
                        {
                            Console.Clear();
                            if (clienteAtual == null)
                            {
                                Console.WriteLine("Não tem permissão para usar esta função.");
                            }
                            else
                            {
                                List<Produto> encomenda = new List<Produto>();
                                bool acabou1 = false;
                                while (!acabou1)
                                {
                                    farmacia.mostrarProdutos();
                                    Console.Write("\nIntroduza o código do produto que quer comprar (0 para finalizar a compra): ");
                                    string idProduto = Console.ReadLine();
                                    int idProdutoInt = Int32.Parse(idProduto);
                                    if (idProdutoInt != 0)
                                    {
                                        Console.Write("Introduza a quantidade do produto que quer comprar: ");
                                        string quantidadeProduto = Console.ReadLine();
                                        int quantidadeProdutoInt = Int32.Parse(quantidadeProduto);
                                        if (farmacia.existeQuantidade(idProdutoInt, quantidadeProdutoInt))
                                        {
                                            Produto prod = farmacia.obterProduto(idProdutoInt);
                                            Produto prodTemp = new Produto(prod.Id, prod.Nome, prod.Preco, prod.Comparticipacao, farmacia.ValidadeQuantidadeParaCompra(idProdutoInt, quantidadeProdutoInt), prod.Descrição, prod.Categoria, prod.SubCategoria);
                                            encomenda.Add(prodTemp);
                                            Console.WriteLine("\nProduto adicionado com sucesso.");
                                            while (Console.KeyAvailable)
                                            {
                                                Console.ReadKey(false);
                                            }
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nNão existe esse produto ou quantidade suficiente.");
                                            while (Console.KeyAvailable)
                                            {
                                                Console.ReadKey(false);
                                            }
                                            Console.ReadKey();
                                        }
                                    }
                                    else
                                    {
                                        acabou1 = true;
                                    }
                                }
                                Console.Write("Quer pagar agora ou deixar na conta? (0 - Pagar Agora | 1 - Deixar na conta): ");
                                string tipoPagamento = Console.ReadLine();
                                int tipoPagamentoInt = Int32.Parse(tipoPagamento);
                                if (tipoPagamentoInt == 0)
                                {
                                    //Adicionar á fila de atendimento
                                    clienteAtual.pagar(farmacia, encomenda);
                                }
                                else
                                {
                                    //Adicionar á fila de atendimento
                                    clienteAtual.adicionarConta(farmacia, encomenda);
                                }
                            }
                            while (Console.KeyAvailable)
                            {
                                Console.ReadKey(false);
                            }
                            Console.ReadKey();
                            break;
                        }
                    case "2":
                        {
                            Console.Clear();
                            if (clienteAtual == null)
                            {
                                Console.WriteLine("Não tem permissão para usar esta função.");
                            }
                            else
                            {
                                bool acabou1 = false;
                                while (!acabou1)
                                {
                                    Console.Clear();
                                    Console.Write("Introduza o código da receita: ");
                                    string codigoReceita = Console.ReadLine();
                                    int codigoReceitaInt = Int32.Parse(codigoReceita);
                                    if (clienteAtual.existeReceita(codigoReceitaInt))
                                    {
                                        Receita receita = clienteAtual.obterReceita(codigoReceitaInt);
                                        Console.Write("Quer pagar agora ou deixar na conta? (0 - Pagar Agora | 1 - Deixar na conta): ");
                                        string tipoPagamento = Console.ReadLine();
                                        int tipoPagamentoInt = Int32.Parse(tipoPagamento);
                                        if (tipoPagamentoInt == 0)
                                        {
                                            //Adicionar á fila de atendimento
                                            clienteAtual.pagar(farmacia, receita.Produtos, true);
                                        }
                                        else
                                        {
                                            //Adicionar á fila de atendimento
                                            clienteAtual.adicionarConta(farmacia, receita.Produtos, true);
                                        }
                                        acabou1 = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nEssa receita não existe.");
                                        while (Console.KeyAvailable)
                                        {
                                            Console.ReadKey(false);
                                        }
                                        Console.ReadKey();
                                    }
                                }
                            }
                            while (Console.KeyAvailable)
                            {
                                Console.ReadKey(false);
                            }
                            Console.ReadKey();
                            break;
                        }
                    case "3":
                        {
                            Console.Clear();
                            farmacia.mostrarProdutos();
                            while (Console.KeyAvailable)
                            {
                                Console.ReadKey(false);
                            }
                            Console.ReadKey();
                            break;
                        }
                    case "4":
                        {
                            Console.Clear();
                            if (clienteAtual == null)
                            {
                                Console.WriteLine("Não tem permissão para usar esta função.");
                            }
                            else
                            {
                                bool acabou1 = false;
                                while (!acabou1)
                                {
                                    Console.Clear();
                                    Console.Write("Introduza o código da sua venda: ");
                                    string codVenda = Console.ReadLine();
                                    int codVendaInt = Int32.Parse(codVenda);
                                    if (farmacia.existeVenda(codVendaInt))
                                    {
                                        List<Produto> devolucao = new List<Produto>();
                                        while (!acabou1)
                                        {
                                            farmacia.mostrarProdutosDaVenda(codVendaInt);
                                            Console.Write("\nIntroduza o código do produto que quer devolver (0 para finalizar a devolução): ");
                                            string idProduto = Console.ReadLine();
                                            int idProdutoInt = Int32.Parse(idProduto);
                                            if (idProdutoInt != 0)
                                            {
                                                Console.Write("Introduza a quantidade do produtos que quer devolver: ");
                                                string quantidadeProduto = Console.ReadLine();
                                                int quantidadeProdutoInt = Int32.Parse(quantidadeProduto);
                                                if (farmacia.existeQuantidadeNaVenda(codVendaInt, idProdutoInt, quantidadeProdutoInt))
                                                {
                                                    Produto prod = farmacia.obterProduto(idProdutoInt);
                                                    DateTime dataVQ = new DateTime(1, 1, 1);
                                                    ValidadeQuantidade vq = new ValidadeQuantidade(quantidadeProdutoInt, dataVQ);
                                                    List<ValidadeQuantidade> lvq = new List<ValidadeQuantidade>();
                                                    lvq.Add(vq);
                                                    Produto prodTemp = new Produto(prod.Id, prod.Nome, prod.Preco, prod.Comparticipacao, lvq, prod.Descrição, prod.Categoria, prod.SubCategoria);
                                                    devolucao.Add(prodTemp);
                                                    Console.WriteLine("\nProduto adicionado com sucesso para devolução.");
                                                    while (Console.KeyAvailable)
                                                    {
                                                        Console.ReadKey(false);
                                                    }
                                                    Console.ReadKey();
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\nNão é possível devolver essa quantidade ou produto.");
                                                    while (Console.KeyAvailable)
                                                    {
                                                        Console.ReadKey(false);
                                                    }
                                                    Console.ReadKey();
                                                    Console.Clear();
                                                }
                                            }
                                            else
                                            {
                                                acabou1 = true;
                                            }
                                        }
                                        clienteAtual.devolver(farmacia, devolucao, codVendaInt);
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nEssa venda não existe.");
                                        while (Console.KeyAvailable)
                                        {
                                            Console.ReadKey(false);
                                        }
                                        Console.ReadKey();
                                    }
                                }
                            }
                            while (Console.KeyAvailable)
                            {
                                Console.ReadKey(false);
                            }
                            Console.ReadKey();
                            break;
                        }
                    case "5":
                        {
                            Console.Clear();
                            if (funcionarioAtual == null)
                            {
                                Console.WriteLine("Não tem permissão para usar esta função.");
                            }
                            else
                            {
                                Console.WriteLine("A farmácia tem " + farmacia.totalProdutos() + " euros em stock de produtos.");
                                farmacia.totalProdutosPorTipo();
                            }
                            while (Console.KeyAvailable)
                            {
                                Console.ReadKey(false);
                            }
                            Console.ReadKey();
                            break;
                        }
                    case "6":
                        {
                            Console.Clear();
                            if ((funcionarioAtual == null) || (funcionarioAtual.Tipo != "Chefe"))
                            {
                                Console.WriteLine("Não tem permissão para usar esta função.");
                            }
                            else
                            {
                                bool acabou1 = false;
                                while (!acabou1)
                                {
                                    farmacia.mostrarTodosProdutos();
                                    Console.Write("\nIntroduza o código do produto que quer repor (0 para finalizar a reposição): ");
                                    string codigoProduto = Console.ReadLine();
                                    int codigocodigoProdutoInt = Int32.Parse(codigoProduto);
                                    if (codigocodigoProdutoInt != 0)
                                    {
                                        Console.Write("Introduza a quantidade a adicionar: ");
                                        string quantidade = Console.ReadLine();
                                        int quantidadeInt = Int32.Parse(quantidade);
                                        Console.Write("Introduza a data de validade desse(s) produto(s) (DD/MM/AAAA): ");
                                        string dataValidade = Console.ReadLine();
                                        DateTime dataValidadeDateTime = DateTime.Parse(dataValidade);
                                        farmacia.reporStock(farmacia.obterProduto(codigocodigoProdutoInt), quantidadeInt, dataValidadeDateTime);
                                        Console.WriteLine("\nProduto adicionado com sucesso ao stock.");
                                        while (Console.KeyAvailable)
                                        {
                                            Console.ReadKey(false);
                                        }
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        acabou1 = true;
                                    }
                                }
                            }
                            break;
                        }
                    case "100":
                        {
                            clienteAtual.pagarConta(farmacia);
                            break;
                        }
                    case "0":
                        {
                            Console.WriteLine("\nMuito obrigado pela sua preferência!");
                            acabou = true;
                            break;
                        }
                }
            }
        }
    }
}