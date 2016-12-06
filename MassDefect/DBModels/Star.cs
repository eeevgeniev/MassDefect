namespace MassDefect.DBModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Star
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Star name is required.")]
        [MaxLength(200, ErrorMessage = "Star name length is more than 200 symbols.")]
        public string Name { get; set; }

        
        public int? SolarSystemId { get; set; }

        [ForeignKey("SolarSystemId")]
        public virtual SolarSystem SolarSystem { get; set; }
    }
}
