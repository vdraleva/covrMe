using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles.Result.QueryResults
{
    public class GetVehicleByIdResultQuery
    {
        public GetVehicleByIdResultQuery()
        {
            this.VehicleByVehicleId = new GetVehicleByIdResultModel();
        }
        public GetVehicleByIdResultModel? VehicleByVehicleId { get; set; }
    }
}
