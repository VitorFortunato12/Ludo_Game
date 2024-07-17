using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ProjetoATP_Final
{
    public class Peoa
    {
        public int time, id;
        public int posiçao = 0;

        public Peoa(int time, int id)
        {
            this.time = time;
            this.id = id;
        }
        public int Mover(int casa)
        {
            posiçao += casa;
            return posiçao;
        }
        public void Retorna()
        {
            posiçao = 0;
        }
    }
    public class Tabuleiro
    {
        public Peoa[,] peao;

        public Tabuleiro(int ContarJogador)
        {
            peao=new Peoa[4,ContarJogador];
            for(int i = 0; i < ContarJogador; i++)
            {
                for(int j = 0; j < peao.GetLength(0); j++)
                {
                    peao[i,j]=new Peoa(i,j);
                }
            }
        }

        public void TabuleiroStatus()
        {
            for(int i = 0;i < peao.GetLength(0); i++)
            {
                for (int j = 0;j < peao.GetLength(1); j++)
                {
                    Console.Write(peao[i, j].posiçao + " ");
                }
                Console.WriteLine();
            }
        }

        public bool PeaoComido(int UltimoMoviento, int jogador)
        {
            TabuleiroStatus();
            int posi = peao[UltimoMoviento - 1, jogador].posiçao;

            switch (jogador)
            {
                case 0:
                    int[] offsets1 = { 0, 26, 13, 39, 0, 26, 39, 13 };
                    if (ChecarCap(jogador, posi, offsets1))
                    {
                        return true;
                    }
                    break;
                case 1:
                    int[] offsets2 = { 26, 0, 39, 13, 26, 0, 13, 39 };
                    if (ChecarCap(jogador, posi, offsets2))
                    {
                        return true;
                    }
                    break;
                case 2:
                    int[] offsets3 = { 39, 13, 0, 26, 13, 39, 0, 26 };
                    if (ChecarCap(jogador, posi, offsets3))
                    {
                        return true;
                    }
                    break;
                case 3:
                    int[] offsets4 = { 13, 39, 26, 0, 39, 13, 26, 0 };
                    if (ChecarCap(jogador, posi, offsets4))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        public bool ChecarCap(int jogador,int posi,int[] offsets)
        {
            int failsafe = 0;
            bool result = false;
            if (jogador == 0)
            {
                failsafe++;
            }
            if(posi <= 1 || posi == 9 || posi == 14 || posi == 22 || posi == 27 || posi == 35 || posi == 40 || posi == 48 || posi >= 52)
            {
                failsafe++;
            }
            else
            {
                for (int i = 0 + failsafe; i < peao.GetLength(1); i++)
                {
                    for (int j = 0; j < peao.GetLength(0); j++)
                    {
                        if (peao[j, i].posiçao >= 52 || peao[j, i].posiçao <= 1)
                        {

                        }
                        else
                        {
                            if (posi > peao[j, i].posiçao)
                            {
                                if (posi == (peao[j, i].posiçao + offsets[i]))
                                {
                                    Console.Write($"Comeu o peão {peao[j, i].id + 1} do ");
                                    Console.WriteLine($"jogador {peao[j, i].time + 1}!");
                                    peao[j, i].Retorna();
                                    result = true;
                                }
                            }
                            else
                            {
                                if ((posi + offsets[i + 4]) == peao[j, i].posiçao)
                                {
                                    Console.Write($"Comeu o peão {peao[j, i].id + 1} do ");
                                    Console.WriteLine($"jogador {peao[j, i].time + 1}!");
                                    peao[j, i].Retorna();
                                    result = true;
                                }
                            }

                        }
                    }
                    if ((i + 1) == jogador)
                    {
                        i++;
                    }
                }
            }
            return result;
        }
    }
    public class Jogo
    {
        static void Main(string[] args)
        {
            int playercount = 0;
            while (playercount != 2 && playercount != 4)
            {
                Console.Clear();
                Console.WriteLine("Bem vindo ao Ludo!");
                Console.WriteLine("\nQuantos Jogadores? (2 ou 4)");
                try
                {
                    playercount = int.Parse(Console.ReadLine());
                }
                catch
                {

                }
            }
            for (int i = 0; i < playercount; i++)
            {
                Console.WriteLine($"Jogador {i + 1}");
            }
            Console.WriteLine("\nEnter para começar!");
            string debug = Console.ReadLine();
            IniciarJogo(playercount, debug);
        }
        static void IniciarJogo(int playercount, string debug)
        {
            Tabuleiro board = new Tabuleiro(playercount);
            bool gameon = true;
            int roundcounter = 1;
            int winner = -1;
            while (gameon) // para o jogo inteiro
            {
                for (int i = 0; i < playercount && gameon; i++) // para cada round
                {

                    winner = JogaRodada(i, board);
                    if (winner >= 0)
                    {
                        gameon = false;
                    }
                    roundcounter++;
                }
            }
            Console.Clear();
            Console.WriteLine("Fim de jogo!!");
            Console.Write($"\nO vencedor é... ");
            Console.Write($"Jogador {winner + 1}");
            Console.WriteLine("! Bom jogo!");
            Console.ReadLine();
        }

        static int JogaRodada(int player, Tabuleiro board)
        {
            Console.Clear();
            bool reroll = false;
            string[] dicetext = { $"O ", $"jogador {player + 1} ", "lança o dado!" };
            do
            {
                if (reroll)
                {
                    reroll = false;
                }
                Random r = new Random();
                int[] dice = new int[4];
                for (int i = 0; i < 3; i++)
                {
                    dice[i] = r.Next(1, 7);
                }
                int count = 0;
                bool stay = true;
                for (int i = 0; i < dicetext.Length; i++)
                {
                    Console.WriteLine(dicetext[i]);
                }
                Console.WriteLine("");
                while (stay)
                {
                    if (dice[count] != 0)
                    {
                        Console.WriteLine("Resultado: ");
                        Console.WriteLine(" " + dice[count].ToString() + " ");
          
                    }
                    if (dice[count] == 6 && count < 3)
                    {
                        count++;
                    }
                    else
                    {
                        stay = false;
                    }
                }
                if (count >= 3)
                {
                    Console.WriteLine("Passou a vez!");
                }
                else
                {
                    for (int i = 0; i <= count; i++)
                    {
                        if (EscolherPeao(board, player, dice[i]) || PeaoGanho(board, player))
                        {
                            reroll = true;
                            dicetext[2] = "ganha outro dado!";

                        }
                        if (GameOver(board) >= 0)
                        {
                            return player;
                        }
                    }
                }
            } while (reroll);
            Console.WriteLine("Fim da jogada!\n");
            Console.ReadLine();
            Console.Clear();
            return -1;
        }

        static bool EscolherPeao(Tabuleiro board, int player, int dado)
        {
            int temppawn = -1;
            int[] valid = new int[4];
            if (dado >= 6)
            {
                for (int i = 0; i < board.peao.GetLength(0); i++)
                {
                    if ((board.peao[i, player].posiçao + dado) <= 57)
                    {
                        valid[i] = i + 1;
                    }
                }
                if (valid[0] == 0 && valid[1] == 0 && valid[2] == 0 && valid[3] == 0)
                {
                    return false;
                }
                else
                {
                    Console.Write($"Mover qual peão com o dado {dado}? ");
                    foreach (int i in valid)
                    {
                        if (i != 0)
                        {
                            Console.Write($"({i}) ");
                        }
                    }
                   Console.WriteLine("");
                    while ((temppawn != valid[0] && temppawn != valid[1] && temppawn != valid[2] && temppawn != valid[3]) || temppawn <= 0 || temppawn >= 5)
                    {
                        try
                        {
                            temppawn = int.Parse(Console.ReadLine());
                        }
                        catch
                        {
                           Console.WriteLine("Digite o numero do peão");
                        }
                    }
                }
                if (board.peao[(temppawn - 1), player].posiçao <= 0)
                {
                    dado -= 5;
                }
            }
            else
            {
                for (int i = 0; i < board.peao.GetLength(0); i++)
                {
                    if (board.peao[i, player].posiçao > 0 && (board.peao[i, player].posiçao + dado) <= 57)
                    {
                        valid[i] = i + 1;

                    }
                }
                if (valid[0] == 0 && valid[1] == 0 && valid[2] == 0 && valid[3] == 0)
                {
                    return false;
                }
                else
                {
                    Console.Write($"Mover qual peão com o dado {dado}? ");
                    foreach (int i in valid)
                    {
                        if (i != 0)
                        {
                            Console.Write($"({i}) ");
                        }
                    }
                    Console.WriteLine("");
                    while ((temppawn != valid[0] && temppawn != valid[1] && temppawn != valid[2] && temppawn != valid[3]) || temppawn <= 0 || temppawn >= 5)
                    {
                        try
                        {
                            temppawn = int.Parse(Console.ReadLine());
                        }
                        catch
                        {
                            Console.WriteLine("Digite o numero do peão");
                        }
                    }
                }
            }
            board.peao[temppawn - 1, player].Mover(dado);
            if (board.PeaoComido(temppawn, player))
            {
                return true;
            }
            return false;
        }

        static bool PeaoGanho(Tabuleiro board, int player)
        {
            for (int i = 0; i < board.peao.GetLength(0); i++)
            {
                if (board.peao[i, player].posiçao == 57)
                {
                    Console.WriteLine($"O peão {i + 1} chegou ao final!");
                    board.peao[i, player].Mover(1);
                    return true;
                }
            }
            return false;
        }

        static int GameOver(Tabuleiro board)
        {
            for (int i = 0; i < board.peao.GetLength(1); i++)
            {
                int cont = 0;
                for (int j = 0; j < board.peao.GetLength(0); j++)
                {
                    if (board.peao[j, i].posiçao == 58)
                    {
                        cont++;
                    }
                }
                if (cont >= 4)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
