using NSE.Pedido.API.Application.DTO;
using SNE.Core.Messages;

namespace NSE.Pedido.API.Application.Commands
{
    public class AdicionarPedidoCommand : Command
    {
        // Pedido
        public Guid ClienteId { get; set; }

        public decimal ValorTotal { get; set; }

        public List<PedidoItemDTO> PedidoItens { get; set; }

        // Voucher
        public decimal Desconto { get; set; }

        public string VoucherCodigo { get; set; }

        public bool VoucherUtilizado { get; set; }

        // Endereco
        public EnderecoDTO Endereco { get; set; }

        // Cartao
        public string NumeroCartao { get; set; }

        public string NomeCartao { get; set; }

        public string ExpiracaoCartao { get; set; }

        public string CvvCartao { get; set; }
    }
}
