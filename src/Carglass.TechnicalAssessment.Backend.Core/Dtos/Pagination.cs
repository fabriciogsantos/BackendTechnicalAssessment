namespace Carglass.TechnicalAssessment.Backend.Core.Dtos
{
    public class Pagination
    {
        public int Page { get; set; } = 1;
        public int Take { get; set; } = 50;

        public int TakeSkip()
        {
            return (Page - 1) * Take;
        }
    }
}
