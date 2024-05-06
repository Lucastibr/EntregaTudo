using System.ComponentModel.DataAnnotations;

namespace EntregaTudo.Api.Helpers;

public class SearchAddress
{
    [Display(Name = "bairro")]
    public string Bairro { get; set; }

    [Display(Name = "cidade")]
    public string Cidade { get; set; }

    [Display(Name = "logradouro")]
    public string Logradouro { get; set; }
}