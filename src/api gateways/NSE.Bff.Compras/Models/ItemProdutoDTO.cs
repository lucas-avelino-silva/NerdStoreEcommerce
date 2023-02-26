namespace NSE.Bff.Compras.Models
{
    public class CarrinhoDTO
    {
        public decimal ValorTotal { get; set; }

        public VoucherDTO Voucher { get; set; }

        public bool VoucherUtilizado { get; set; }

        public decimal Desconto { get; set; }

        public List<ItemCarrinhoDTO> Itens { get; set; } = new List<ItemCarrinhoDTO>();
    }

    //representa o produto no carrinho
    public class ItemCarrinhoDTO
    {
        public Guid ProdutoId { get; set; }

        public string? Nome { get; set; }

        public int Quantidade { get; set; }

        public decimal? Valor { get; set; }

        public string? Imagem { get; set; }
    }

    //representa o produto do catalogo
    public class ItemProdutoDTO
    {
        public Guid Id { get; set; }

        public string? Nome { get; set; }

        public decimal? Valor { get; set; }

        public string? Imagem { get; set; }
        public int QuantidadeEstoque { get; set; }

    }
}
