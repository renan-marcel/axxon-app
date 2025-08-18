
using System;
using System.Collections.Generic;

namespace ProdAbs.Domain.Entities
{
    public class Documento
    {
        public Guid Id { get; set; }
        public Guid TipoDeDocumentoId { get; set; }
        public string StorageLocation { get; set; }
        public long TamanhoEmBytes { get; set; }
        public string HashTipo { get; set; }
        public string HashValor { get; set; }
        public string NomeArquivoOriginal { get; set; }
        public string Formato { get; set; }
        public int Versao { get; set; }
        public IReadOnlyDictionary<string, string> DicionarioDeCamposValores { get; set; }
    }
}
