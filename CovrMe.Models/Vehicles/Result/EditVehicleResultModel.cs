using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles.Result
{
    public class EditVehicleResultModel
    {
        public EditVehicleResultModel()
        {
            this.Vehicle = new VehicleResultModel();
        }
        public VehicleResultModel? Vehicle { get; set; }
    }
}

