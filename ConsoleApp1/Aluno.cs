using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ConsoleApp1
{
    public class Aluno : TableEntity
    {
        public Aluno() { }
        public Aluno(string partitionKey, string rowKey) : base(partitionKey, rowKey) { }

        public String Nome { get; set; }

        public String Email { get; set; }

        public String Celular { get; set; }

        public String Endereco { get; set; }

    }
}
