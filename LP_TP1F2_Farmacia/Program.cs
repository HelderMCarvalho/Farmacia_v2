using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    Criar elementos para representar:
    - Receita;

    Ações da Farmácia:
    - Respeitar a fila de atendimento;
    - Verificar e Levantar a receita;
    - Verificar validades.

    Detalhes:
    Criar as classes, variáveis e structs necessárias para modelar:
    - Funcionários: o chefe tem para além disso a função de comprar e repor os stocks.
    - Medicamentos:
        Existem depois os vários tipos de medicamentos:
            - opiáceos que só podem ser levantados ao máximo 5 por semana (se na receita forem receitados 20, quer dizer que
            só ao fim de 4 semanas aquela  parte da receita pode ser de facto levantada totalmente);
    - Produtos de Higiene e Alimentares:
        Os vários produtos são:
            - Produtos hipoalergénicos (papas sem glúten, sem amido) com taxas a 6%
            - Produtos 100% naturais para animais (taxa adicional de 1€ por compra para a causa “Salvem as ratazanas de 
            laboratório”)
    - Produtos de beleza que são variados e que possuem também nome, quantidade e preço mas cuja taxa é de 23% de iva.
    - Receita que é basicamente uma lista de medicamentos e quantidades mas que só acaba quando todos os medicamentos forem 
    levantados;
    - Todos os dados têm de ser carregados a partir de ficheiros. Como tal também devem haver métodos que permitam guardar 
    os dados em ficheiros;
    - A farmácia tem de ter um valor de tempo para poder comparar com o tempo das validades dos produtos (medicamentos 
    apenas, os outros são vitalícios)
    - Tem de ser estabelecidas filas para atender os clientes.
    - Os menus devem refletir tanto a parte de se ser cliente como de se ser funcionário.
    - Saber o valor dos produtos por tipo (valor dos opiáceos, dos produtos de beleza, ...)
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
        private string tipo; /*Chefe ou Base*/

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

        public float Dinheiro { get => dinheiro; set => dinheiro = value; }
        public List<Receita> Receitas { get => receitas; set => receitas = value; }
        public bool CartaoFarmacias { get => cartaoFarmacias; set => cartaoFarmacias = value; }
        public float Conta { get => conta; set => conta = value; }

        public Cliente(float dinheiro, List<Receita> receitas, bool cartaoFarmacias, float conta, int id, string nome) : base(id, nome)
        {
            this.dinheiro = dinheiro;
            this.receitas = receitas;
            this.cartaoFarmacias = cartaoFarmacias;
            this.conta = conta;
        }

        /// <summary>
        /// Recebe a farmácia e a lista de produtos encomendados
        /// Soma o total a pagar dos produtos encomendados e adiciona as respetivas taxas
        /// Se o cliente tiver dinheiro paga, se não tiver aparece a respetiva mensagem
        /// <param name="farmacia"></param>
        /// <param name="encomenda"></param>
        public void pagar(Farmacia farmacia, List<Produto> encomenda)
        {
            float totalPagar = 0;
            foreach (Produto produto in encomenda)
            {
                if (cartaoFarmacias == true)
                {
                    if((produto.SubCategoria== "AntiInflamatorio") ||(produto.SubCategoria== "AntiSeptico"))
                    {
                        produto.Preco += (produto.Preco * 0.01f);
                        totalPagar += ((produto.Preco - (produto.Preco * 0.05f)) * produto.Quantidade);
                    }
                    else if (produto.SubCategoria == "Injecao")
                    {
                        produto.Preco += 1;
                        totalPagar += ((produto.Preco - (produto.Preco * 0.05f)) * produto.Quantidade);
                    }
                    else if (produto.SubCategoria == "Higiene")
                    {
                        produto.Preco += (produto.Preco * 0.13f);
                        totalPagar += ((produto.Preco - (produto.Preco * 0.05f)) * produto.Quantidade);
                    }
                    else
                    {
                        totalPagar += ((produto.Preco - (produto.Preco * 0.05f)) * produto.Quantidade);
                    }
                    //Fazer taxas em produtos hipoalergénicos e animais
                }
                else
                {
                    if ((produto.SubCategoria == "AntiInflamatorio") || (produto.SubCategoria == "AntiSeptico"))
                    {
                        produto.Preco += (produto.Preco * 0.01f);
                        totalPagar += (produto.Preco * produto.Quantidade);
                    }
                    else if (produto.SubCategoria == "Injecao")
                    {
                        produto.Preco += 1;
                        totalPagar += (produto.Preco * produto.Quantidade);
                    }
                    else if (produto.SubCategoria == "Higiene")
                    {
                        produto.Preco += (produto.Preco * 0.13f);
                        totalPagar += (produto.Preco * produto.Quantidade);
                    }
                    else
                    {
                        totalPagar += (produto.Preco * produto.Quantidade);
                    }
                }
            }
            if (dinheiro >= totalPagar)
            {
                foreach (Produto produto in encomenda)
                {
                    farmacia.retiraDoStock(produto.Id, produto.Quantidade);
                }
                dinheiro -= totalPagar;
                farmacia.Dinheiro += totalPagar;
                farmacia.ContadorVentas++;
                Venda venda = new Venda(farmacia.ContadorVentas, id, encomenda, totalPagar, false);
                farmacia.Vendas.Add(venda);
                Console.WriteLine("\nCompra efetuada com sucesso.");
                Console.WriteLine("O seu código de venda é: " + farmacia.ContadorVentas);
            }
            else
            {
                Console.WriteLine("\nNão tem dinheiro suficiente.\nDeseja adicionar á conta ou cancelar a compra? (0 - Adicionar á conta | 1 - Cancelar)");
                string pagarCancelar = Console.ReadLine();
                int pagarCancelarInt = Int32.Parse(pagarCancelar);
                if (pagarCancelarInt == 0)
                {
                    adicionarConta(farmacia, encomenda);
                }
                else
                {
                    Console.WriteLine("\nCompra cancelada com sucesso!");
                }
            }
        }

        /// <summary>
        /// Recebe a farmácia e a lista de produtos encomendados
        /// Soma o total a pagar dos produtos encomendados
        /// Se a conta do cliente não exceder os 50€ a venda é criada e adicionado o valor é conta, senão o cliente paga na hora ou cancela
        /// </summary>
        /// <param name="farmacia"></param>
        /// <param name="encomenda"></param>
        public void adicionarConta(Farmacia farmacia, List<Produto> encomenda)
        {
            float totalPagar = 0;
            foreach (Produto produto in encomenda)
            {
                if (cartaoFarmacias == true)
                {
                    totalPagar += ((produto.Preco - (produto.Preco * 0.05f)) * produto.Quantidade);
                }
                else
                {
                    totalPagar += (produto.Preco * produto.Quantidade);
                }
            }
            if((conta + totalPagar) < 50)
            {
                foreach (Produto produto in encomenda)
                {
                    farmacia.retiraDoStock(produto.Id, produto.Quantidade);
                }
                conta += totalPagar;
                farmacia.ContadorVentas++;
                Venda venda = new Venda(farmacia.ContadorVentas, id, encomenda, totalPagar, false);
                farmacia.Vendas.Add(venda);
                Console.WriteLine("\nCompra adicionada com sucesso á conta.");
                Console.WriteLine("O seu código de venda é: " + farmacia.ContadorVentas);
            }
            else
            {
                Console.WriteLine("\nNão pode adicionar á conta porque a mesma excede os 50 euros.\nDeseja pagar agora ou cancelar a compra? (0 - Pagar agora | 1 - Cancelar)");
                string pagarCancelar = Console.ReadLine();
                int pagarCancelarInt = Int32.Parse(pagarCancelar);
                if (pagarCancelarInt == 0)
                {
                    pagar(farmacia, encomenda);
                }
                else
                {
                    Console.WriteLine("\nCompra cancelada com sucesso!");
                }
            }
        }

        /// <summary>
        /// Paga o valor que o cliente tem em conta (caso tenha dinheiro para tal)
        /// </summary>
        /// <param name="farmacia"></param>
        public void pagarConta(Farmacia farmacia)
        {
            if (dinheiro >= conta)
            {
                farmacia.Dinheiro += conta;
                dinheiro -= conta;
                conta = 0;
            }
            else
            {
                Console.WriteLine("\nNão tem dinheiro para pagar o que deve!");
            }
        }
    }

    class Receita
    {
        private int codigo;
        private List<Produto> produtos;
        private bool entregue;

        public int Codigo { get => codigo; set => codigo = value; }
        public bool Entregue { get => entregue; set => entregue = value; }
        public bool Entregue1 { get => entregue; set => entregue = value; }
        
        public Receita(int codigo, List<Produto> produtos, bool entregue)
        {
            this.codigo = codigo;
            this.produtos = produtos;
            this.entregue = entregue;
        }
    }

    class Produto
    {
        private int id;
        private string nome;
        private float preco;
        private int quantidade;
        private bool comparticipacao;
        private DateTime validade;
        private string descrição;
        private string categoria; //M, HA, B
        private string subCategoria; //Sub-categoria

        public string Nome { get => nome; set => nome = value; }
        public float Preco { get => preco; set => preco = value; }
        public int Quantidade { get => quantidade; set => quantidade = value; }
        public bool Comparticipacao { get => comparticipacao; set => comparticipacao = value; }
        public int Id { get => id; set => id = value; }
        public DateTime Validade { get => validade; set => validade = value; }
        public string Descrição { get => descrição; set => descrição = value; }
        public string Categoria { get => categoria; set => categoria = value; }
        public string SubCategoria { get => subCategoria; set => subCategoria = value; }

        public Produto(int id, string nome, float preco, int quantidade, bool comparticipacao, DateTime validade, string descrição, string categoria, string subCategoria)
        {
            this.id = id;
            this.nome = nome;
            this.preco = preco;
            this.quantidade = quantidade;
            this.comparticipacao = comparticipacao;
            this.validade = validade;
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

        public List<Funcionario> Funcionarios { get => funcionarios; set => funcionarios = value; }
        public List<Cliente> Clientes { get => clientes; set => clientes = value; }
        public List<Produto> Medicamentos { get => produtos; set => produtos = value; }
        public float Dinheiro { get => dinheiro; set => dinheiro = value; }
        public List<Venda> Vendas { get => vendas; set => vendas = value; }
        public int ContadorVentas { get => contadorVentas; set => contadorVentas = value; }
        public DateTime Data { get => data; set => data = value; }

        public Farmacia(List<Funcionario> funcionarios, List<Cliente> clientes, List<Produto> produtos, int contadorVentas, List<Venda> vendas, float dinheiro, DateTime data)
        {
            this.funcionarios = funcionarios;
            this.clientes = clientes;
            this.produtos = produtos;
            this.contadorVentas = contadorVentas;
            this.vendas = vendas;
            this.dinheiro = dinheiro;
            this.data = data;
        }

        /// <summary>
        /// Recebe o ID do cliente e devolve um Objeto Cliente desse código ou devolve um Objeto Cliente = null caso não exista
        /// </summary>
        /// <param name="idCliente"></param>
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
        /// Recebe o ID do funcionário e devolve um Objeto Funcionario desse código ou devolve um Objeto Funcionario = null caso não exista
        /// </summary>
        /// <param name="idFuncionario"></param>
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
        /// Lista todos os produtos em stock
        /// </summary>
        public void mostrarMedicamentos()
        {
            Console.Clear();
            Console.WriteLine("Lista de produtos:\n");
            foreach (Produto produto in produtos)
            {
                if (produto.Quantidade > 0)
                {
                    Console.WriteLine(produto.Id + " - " + produto.Nome + " - " + produto.Preco + " euros" + " - " + produto.Validade.ToString("d") + " - " + produto.Descrição);
                }
            }
        }

        /// <summary>
        /// Verifica se um determinado produto existe
        /// </summary>
        /// <param name="idMedicamento"></param>
        /// <returns>bool onde 1 - Existe e 0 - Não existe</returns>
        public bool existeMedicamento(int idMedicamento)
        {
            bool existe = false;
            foreach (Produto produto in produtos)
            {
                if (idMedicamento == produto.Id)
                {
                    existe = true;
                    break;
                }
            }
            return existe;
        }

        /// <summary>
        /// Verifica se um determinado produto existe em stock numa determinada quantidade
        /// </summary>
        /// <param name="idMedicamento"></param>
        /// <param name="quantidade"></param>
        /// <returns>bool onde 1 - Existe em stock e 0 - Não existe em stock</returns>
        public bool existeQuantidade(int idMedicamento, int quantidade)
        {
            bool existe = false;
            if (existeMedicamento(idMedicamento))
            {
                foreach (Produto produto in produtos)
                {
                    if ((quantidade <= produto.Quantidade) && (produto.Id == idMedicamento))
                    {
                        existe = true;
                        break;
                    }
                }
            }
            return existe;
        }

        /// <summary>
        /// Recebe o ID do produto e devolve o objeto Produto desse código
        /// </summary>
        /// <param name="idProduto"></param>
        /// <returns>Devolve um objeto Medicamento</returns>
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
        /// Retira do stock uma certa quantidade de produtos
        /// </summary>
        /// <param name="idProduto"></param>
        /// <param name="quantidade"></param>
        public void retiraDoStock(int idProduto, int quantidade)
        {
            if (existeQuantidade(idProduto, quantidade))
            {
                foreach (Produto produto in produtos)
                {
                    if (idProduto == produto.Id)
                    {
                        produto.Quantidade -= quantidade;
                        break;
                    }
                }
            }
        }
    }

    struct Venda
    {
        private int codigo;
        private int codigoCliente;
        private List<Produto> produtos;
        private float totalPago;
        private bool isReceita;

        public Venda(int codigo, int codigoCliente, List<Produto> produtos, float totalPago, bool isReceita)
        {
            this.codigo = codigo;
            this.codigoCliente = codigoCliente;
            this.produtos = produtos;
            this.totalPago = totalPago;
            this.isReceita = isReceita;
        }

        public int Codigo { get => codigo; set => codigo = value; }
        public int CodigoCliente { get => codigoCliente; set => codigoCliente = value; }
        public List<Produto> Medicamentos { get => produtos; set => produtos = value; }
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

            DateTime data = new DateTime(2018, 12, 25);
            Produto prod1 = new Produto(1, "Benuron", 5.0f, 100, true, data, "Opiácio", "M", "Opiacio");
            Produto prod2 = new Produto(2, "Brufen", 6.0f, 100, true, data, "Opiácio", "M", "Opiacio");
            Produto prod3 = new Produto(3, "Antiflan", 7.0f, 100, true, data, "Anti-Inflamatório", "M", "AntiInflamatorio");
            Produto prod4 = new Produto(4, "Ceprofen", 8.0f, 100, true, data, "Anti-Inflamatório", "M", "AntiInflamatorio");
            Produto prod5 = new Produto(5, "Vacina", 9.0f, 100, true, data, "Injeções", "M", "Injecao");
            Produto prod6 = new Produto(6, "Noregyna", 10.0f, 100, true, data, "Injeções", "M", "Injecao");
            Produto prod7 = new Produto(7, "Colgate", 11.0f, 100, false, data, "Pasta de Dentes", "HA", "Higiene");
            Produto prod8 = new Produto(8, "Linic", 12.0f, 100, false, data, "Champô", "HA", "Higiene");
            Produto prod9 = new Produto(9, "Papa s/ Glúten", 13.0f, 100, false, data, "Papa sem Glúten", "HA", "Alimentar");
            Produto prod10 = new Produto(10, "Papa s/ Amido", 14.0f, 100, false, data, "Papa sem Amido", "HA", "Alimentar");
            Produto prod11 = new Produto(11, "Scalibor", 15.0f, 100, false, data, "Desparazitante de animal", "HA", "Animal");
            Produto prod12 = new Produto(12, "Amflee", 16.0f, 100, false, data, "Desparazitante de animal", "HA", "Animal");
            Produto prod13 = new Produto(13, "Dove", 17.0f, 100, false, data, "Creme hedratante", "B", "Beleza");
            Produto prod14 = new Produto(14, "Nivea", 18.0f, 100, false, data, "Creme hedratante", "B", "Beleza");

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

            Produto prod1Receita = new Produto(1, "Benuron", 5.0f, 2, true, data, "Opiácio", "M", "Opiacio");
            Produto prod2Receita = new Produto(3, "Antiflan", 7.0f, 2, true, data, "Anti-Inflamatório", "M", "AntiInflamatorio");
            Produto prod3Receita = new Produto(5, "Vacina", 9.0f, 100, true, data, "Injeções", "M", "Injecao");
            Produto prod4Receita = new Produto(7, "Colgate", 11.0f, 100, false, data, "Pasta de Dentes", "HA", "Higiene");
            Produto prod5Receita = new Produto(9, "Papa s/ Glúten", 13.0f, 100, false, data, "Papa sem Glúten", "HA", "Alimentar");
            Produto prod6Receita = new Produto(11, "Scalibor", 15.0f, 100, false, data, "Desparazitante de animal", "HA", "Animal");
            Produto prod7Receita = new Produto(13, "Dove", 17.0f, 100, false, data, "Creme hedratante", "B", "Beleza");
            List<Produto> produtosReceita = new List<Produto>();
            produtosReceita.Add(prod1Receita);
            produtosReceita.Add(prod2Receita);
            produtosReceita.Add(prod3Receita);
            produtosReceita.Add(prod4Receita);
            produtosReceita.Add(prod5Receita);
            produtosReceita.Add(prod6Receita);
            produtosReceita.Add(prod7Receita);
            Receita receita = new Receita(1, produtosReceita, false);
            List<Receita> receitas = new List<Receita>();
            receitas.Add(receita);

            Cliente clie1 = new Cliente(100.0f, receitas, true, 0, 1, "Rebeca");
            Cliente clie2 = new Cliente(200.0f, receitas, false, 0, 2, "Toninha");
            Cliente clie3 = new Cliente(300.0f, receitas, false, 0, 3, "Ramira");
            List<Cliente> clientes = new List<Cliente>();
            clientes.Add(clie1);
            clientes.Add(clie2);
            clientes.Add(clie3);

            List<Venda> vendas = new List<Venda>();

            DateTime dataFarmacia = new DateTime(2017, 12, 14);
            Farmacia farmacia = new Farmacia(funcionarios, clientes, produtos, 0, vendas, 10000.0f, dataFarmacia);

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
                    Console.WriteLine("\nValor a dever: " + clienteAtual.Conta + " euros\nVá para a opção 100 para pagar o que deve.");
                }
                Console.WriteLine("\n----------MENU----------");
                Console.WriteLine("\nEscolha uma opção:");
                Console.WriteLine("1 - Comprar medicamentos");
                Console.WriteLine("2 - Mostrar receita");
                Console.WriteLine("3 - Procurar e verificar se existem medicamentos");
                Console.WriteLine("4 - Devolver medicamentos");
                Console.WriteLine("5 - Mostrar valor total em medicamentos");
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
                                    farmacia.mostrarMedicamentos();
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
                                            Produto prodTemp = new Produto(prod.Id, prod.Nome, prod.Preco, quantidadeProdutoInt, prod.Comparticipacao, prod.Validade, prod.Descrição, prod.Categoria, prod.SubCategoria);
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
                                            Console.WriteLine("\nNão existe quantidade suficiente.");
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