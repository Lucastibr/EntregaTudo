using EntregaTudo.Shared.Dto.Base;

namespace EntregaTudo.Shared;

public class LoginDto : DtoBase
{
    public string Email { get; set; }

    public string Password { get; set; }
}