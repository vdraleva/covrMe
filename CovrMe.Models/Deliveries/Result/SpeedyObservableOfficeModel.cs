using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Deliveries.Result
{
    public partial class SpeedyObservableOfficeModel : ObservableObject
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int SiteId { get; set; }

        [ObservableProperty]
        public bool isChecked;
    }
}
