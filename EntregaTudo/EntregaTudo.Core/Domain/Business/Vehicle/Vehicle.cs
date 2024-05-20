using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntregaTudo.Core.Domain.Base;
using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Core.Domain.User;

namespace EntregaTudo.Core.Domain.Business.Vehicle;

/// <summary>
/// Classe base para os veículos
/// </summary>
public class Vehicle : MongoEntity
{
    /// <summary>
    /// Marca
    /// </summary>
    [Column]
    [Required]
    public string? Brand { get; set; }

    /// <summary>
    /// Modelo
    /// </summary>
    [Column]
    [Required]
    public string? Model { get; set; }

    /// <summary>
    /// Ano Fabricacao
    /// </summary>
    [Column]
    [Required]
    public int? ManufactureYear { get; set; }

    /// <summary>
    /// Placa do Veículo
    /// </summary>
    [Column]
    [Required]
    public string? LicensePlate { get; set; }
    /// <summary>
    /// Capacidade Carga
    /// </summary>
    [Column]
    [Required]
    public double? LoadCapacity { get; set; }

    /// <summary>
    /// Status do Veículo, se pode fazer entregas ou não
    /// </summary>
    [Column]
    public VehicleStatus? VehicleStatus { get; set; }

    /// <summary>
    /// Tipo do Veículo
    /// </summary>
    [Column]
    [Required]
    public VehicleType? VehicleType { get; set; }

    /// <summary>
    /// Método para setar a capacidade de Carga
    /// </summary>
    /// <param name="loadCapacity"></param>
    /// <returns></returns>
    public void SetDeliveryCapacity(double? loadCapacity)
    {
        LoadCapacity = loadCapacity;
    }

    public Person Person { get; set; }
}