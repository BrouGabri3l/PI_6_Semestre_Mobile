using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projeto_pi.Models
{
    public class QuestionModel
    {
        public string Text { get; set; } = string.Empty;
        public ObservableCollection<OptionModel> Options { get; set; } = new();
        public int MinimumSelections { get; set; } = 1;
    }
}
