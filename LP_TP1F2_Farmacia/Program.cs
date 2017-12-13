using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    Criar elementos para representar:
    - Funcionários e Clientes;
    - Medicamentos {Opiáceos, Anti-Inflamatórios, Anti-Sépticos, Orais, Cutâneos, ...};
    - Produtos de Higiene {champô, pasta dentífrica, produtos-antialergénicos}
    - Produtos de beleza {cremes de rosto, mãos, pés, hidratantes, de noite, de dia, maquilhagem}
    - Receita;

    Ações da Farmácia:
    - Respeitar a fila de atendimento;
    - Comprar medicamentos e outros produtos;
    - Verificar e Levantar a receita;
    - Encomendar medicamentos;
    - Verificar validades.
    Detalhes:
    Criar as classes, variáveis e structs necessárias para modelar:
    - Funcionários que tem um nome e id. O chefe tem para além disso a função de comprar e repor os stocks.
    - Clientes com nome, dinheiro e podendo ter nenhuma ou várias receitas. Para além disso podem ter ou não um cartão das 
    farmácias portuguesas que lhe conferem a possibilidade de ter descontos de 5%. Para além disso existe a categoria de 
    clientes habituais que quando pagarem podem deixar na conta (com limite de 50 €);
    - Medicamentos que tem um nome, quantidade, preço e validade. Existem depois os vários tipos de medicamentos:
        - opiáceos que só podem ser levantados ao máximo 5 por semana (se na receita forem receitados 20, quer dizer que só 
        ao fim de 4 semanas aquela  parte da receita pode ser de facto levantada totalmente);
        - Anti-inflamatórios e anti-sépticos tem uma taxa de 1% a acrescer ao preço normal;
        - Injeções que possuem um preço acrescido de 1€ por trazerem uma agulha esterilizada.
    - Produtos de Higiene e Alimentares que tem nome, quantidade, preço e descrição.
    Os vários produtos são:
        - Champôs e Pastas dentífricas que têm uma taxa de 13%;
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
        private int id;
        private string nome;

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

    }

    class Receita
    {
        private int codigo;
        private List<Medicamento> medicamentos;
        private bool entregue;

        //Gets e Sets
        public int Codigo { get => codigo; set => codigo = value; }
        public bool Entregue { get => entregue; set => entregue = value; }
        public List<Medicamento> Medicamentos { get => medicamentos; set => medicamentos = value; }

        //Construtor
        public Receita(int codigo, List<Medicamento> medicamentos, bool entregue)
        {
            this.codigo = codigo;
            this.medicamentos = medicamentos;
            this.entregue = entregue;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
