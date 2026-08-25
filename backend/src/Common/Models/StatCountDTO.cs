namespace LeksanaStudio.Common.Models
{
    public class StatCountDTO
    {
        public int Count { get; set; }

        /// <summary>Percentage of the total (0–100), rounded to the nearest integer.</summary>
        public int Percentage { get; set; }
    }
}
