using EntregaTudo.Shared.Dto.Base;
using EntregaTudo.Shared.Enums;

namespace EntregaTudo.Shared.Dto;

public class VehicleDto : DtoBase
{
    /// <summary>
    /// Marca
    /// </summary>
    public string? Brand { get; set; }

    /// <summary>
    /// Modelo
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Ano Fabricacao
    /// </summary>
    public int? ManufactureYear { get; set; }

    /// <summary>
    /// Placa do Veículo
    /// </summary>
    public string? LicensePlate { get; set; }
    /// <summary>
    /// Capacidade Carga
    /// </summary>
    public double? LoadCapacity { get; set; }

    /// <summary>
    /// Status do Veículo, se pode fazer entregas ou não
    /// </summary>
    public VehicleStatus? VehicleStatus { get; set; }

    /// <summary>
    /// Tipo do Veículo
    /// </summary>
    public VehicleType? VehicleType { get; set; }
}