using GestaoPresencasMVC.Models;

namespace GestaoPresencasMVC.DTOs
{
    // AulaWithPresencaCountDTO.cs
    public class AulaWithPresencaCountDTO
    {
        public Aula Aula { get; set; }
        public int PresenteCount { get; set; }
        public int TotalPresencaCount { get; set; }
    }

}
