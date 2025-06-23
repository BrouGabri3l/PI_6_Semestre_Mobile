using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projeto_pi.Models
{
    public class RecommendationModel
    {
        public int GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HeaderImage { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public bool IsFavorite { get; set; }
    }
}
