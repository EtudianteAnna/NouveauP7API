using System.ComponentModel.DataAnnotations;
namespace NouveauP7API.Domain
{
    public class Rating
    {
        public int Note;

        // TODO: Map columns in data table RATING with corresponding fields

        public int Id { get; set; }

        [Required]
        public string? MoodysRating { get; set; }
        [Required]
        public string? SandPRating { get; set; }

        [Required]
        public string? FitchRating { get; set; }

        public byte? OrderNumber { get; set; }

        public Rating()
        {
            MoodysRating = string.Empty;
        }


    }
}
