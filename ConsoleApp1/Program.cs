using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            #region CreateEntity
            //Aluno aluno = NovoAluno();
            #endregion

            #region CreateTable
            CloudTable tbAluno = CriarTabela();
            #endregion

            #region SaveNewEntity
            //Inserindo Aluno
            //TableOperation operation = TableOperation.Insert(aluno);
            //tbAluno.Execute(operation);
            #endregion

            #region Read
            //TableOperation retrieve = TableOperation.Retrieve<Aluno>("alunos","1");
            //TableResult result = tbAluno.Execute(retrieve);
            //Aluno aluno = result.Result as Aluno;

            //Console.WriteLine($"Nome: {aluno.Nome}");
            //Console.WriteLine($"Celular: {aluno.Celular}");
            //Console.WriteLine($"Email: {aluno.Email}");

            //Lendo uma Lista de Alunos
            //var entities = tbAluno.ExecuteQuery(new TableQuery<Aluno>()).ToList();

            //Lendo Lista de Alunos compondo query
            var query = new TableQuery<Aluno>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey",
                QueryComparisons.Equal, "alunos"));


            foreach (var aluno in tbAluno.ExecuteQuery(query))
            {
                Console.WriteLine($"Nome: {aluno.Nome}");
                Console.WriteLine($"Celular: {aluno.Celular}");
                Console.WriteLine($"Email: {aluno.Email}");
                Console.WriteLine($"Endereco: {aluno.Endereco}");
                Console.WriteLine();
            }

            #endregion


            #region Delete
            var retry = false;

            do
            {
                TableOperation op = TableOperation.Retrieve<Aluno>("alunos", "teste");
                TableResult result = tbAluno.Execute(op);

                //503: Service Unavaliable 
                //504: Gateway timeout
                //408: Request Timeout 
                if (result.HttpStatusCode == 503 || result.HttpStatusCode == 504 || result.HttpStatusCode == 408)
                {
                    retry = true;
                }
                else
                {
                    Aluno excluir = (Aluno)result.Result;
                    TableOperation delete = TableOperation.Delete(excluir);
                    tbAluno.Execute(delete);
                    retry = false;
                }              

            } while (retry == true);
            #endregion

            Console.WriteLine("Fim");
            Console.Read();

        }


        public static Aluno NovoAluno()
        {
            Console.WriteLine("Digite o nome do aluno");
            string nome = Console.ReadLine();

            Console.WriteLine("Digite o email do aluno");
            string email = Console.ReadLine();

            Console.WriteLine("Digite o celular do aluno");
            string celular = Console.ReadLine();

            Console.WriteLine("Digite o RM do aluno");
            string rm = Console.ReadLine();

            Console.WriteLine("Digite o Endereco do aluno");
            string endereco = Console.ReadLine();

            var aluno = new Aluno("alunos", rm)
            {
                Nome = nome,
                Email = email,
                Celular = celular,
                Endereco = endereco
            };
            return aluno;
        }

        public static CloudTable CriarTabela()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationSettings.AppSettings["ConnectionStringStorage"]);
            CloudTableClient table = account.CreateCloudTableClient();
            CloudTable tbAluno = table.GetTableReference("alunosfiap");
            tbAluno.CreateIfNotExists();
            return tbAluno;
        }
    }
}
